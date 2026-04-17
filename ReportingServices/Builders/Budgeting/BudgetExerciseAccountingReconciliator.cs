/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetExerciseAccountingReconciliator         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Reconciliates accounting records with budget exercise transactions.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Empiria.CashFlow.CashLedger.Adapters;

using Empiria.FinancialAccounting.ClientServices;

using Empiria.Budgeting.Transactions;

namespace Empiria.Banobras.Reporting.Builders.Budgeting {

  public class ExerciseReconciliationDto {

    public string BudgetTransactionNo {
      get; internal set;
    }

    public string BudgetTransactionOrgUnit {
      get; internal set;
    }

    public string BudgetAccount {
      get; internal set;
    }

    public string BudgetAccountName {
      get; internal set;
    }

    public string BudgetControlNumber {
      get; internal set;
    }

    public decimal BudgetExercise {
      get; internal set;
    }

    public string AccountNumber {
      get; internal set;
    }

    public string AccountName {
      get; internal set;
    }

    public string VerificationNumber {
      get; internal set;
    }

    public decimal Debit {
      get; internal set;
    }

    public decimal Credit {
      get; internal set;
    }

    public string AccountingVoucherNo {
      get; internal set;
    }

  }  // class ExerciseReconciliationDto



  /// <summary>Reconciliates accounting records with budget exercise transactions.</summary>
  internal class BudgetExerciseAccountingReconciliator {

    private class Reconcilation {

      internal Reconcilation(BudgetEntry exerciseTxnEntry, CashEntryExtendedDto accountingEntry) {
        BudgetEntry = exerciseTxnEntry;
        AccountingEntry = accountingEntry;
      }

      internal BudgetEntry BudgetEntry {
        get; set;
      }

      internal CashEntryExtendedDto AccountingEntry {
        get; set;
      }
    }

    private DateTime _fromDate;
    private DateTime _toDate;

    public BudgetExerciseAccountingReconciliator(DateTime fromDate, DateTime toDate) {
      _fromDate = fromDate;
      _toDate = toDate;
    }


    internal async Task<FixedList<ExerciseReconciliationDto>> Build() {

      FixedList<BudgetTransaction> exerciseTxns = GetBudgetExerciseTransactions();

      FixedList<CashEntryExtendedDto> accountingEntries = await GetAccountingEntries();

      var reconciliationList = new List<Reconcilation>(exerciseTxns.Count);

      foreach (var txn in exerciseTxns) {
        var reconcilationEntries = Reconciliate(txn, accountingEntries);

        reconciliationList.AddRange(reconcilationEntries);
      }

      return reconciliationList.Select(x => Map(x))
                               .OrderBy(x => x.BudgetTransactionNo)
                               .ThenBy(x => x.BudgetControlNumber)
                               .ToFixedList();
    }


    private ExerciseReconciliationDto Map(Reconcilation reconcilation) {

      return new ExerciseReconciliationDto {
        BudgetTransactionNo = reconcilation.BudgetEntry.Transaction.TransactionNo,
        BudgetTransactionOrgUnit = reconcilation.BudgetEntry.BudgetAccount.OrganizationalUnit.Code,
        BudgetControlNumber = reconcilation.BudgetEntry.ControlNo,
        BudgetAccount = reconcilation.BudgetEntry.BudgetAccount.AccountNo,
        BudgetAccountName = reconcilation.BudgetEntry.BudgetAccount.Name,
        BudgetExercise = reconcilation.BudgetEntry.Amount,
        AccountName = reconcilation.AccountingEntry.AccountName,
        AccountNumber = reconcilation.AccountingEntry.AccountNumber,
        VerificationNumber = reconcilation.AccountingEntry.VerificationNumber,
        Credit = reconcilation.AccountingEntry.Credit,
        Debit = reconcilation.AccountingEntry.Debit,
        AccountingVoucherNo = reconcilation.AccountingEntry.TransactionNumber
      };
    }


    private FixedList<Reconcilation> Reconciliate(BudgetTransaction txn,
                                                  FixedList<CashEntryExtendedDto> accountingEntries) {

      var budgetExerciseEntries = txn.Entries.FindAll(x => x.BalanceColumn == BalanceColumn.Exercised &&
                                                           x.NotAdjustment);

      var reconciliationList = new List<Reconcilation>(budgetExerciseEntries.Count);

      foreach (var txnEntry in budgetExerciseEntries) {

        var reconcilationEntry = Reconcilate(txnEntry, accountingEntries);

        reconciliationList.Add(reconcilationEntry);
      }


      var remainingEntries = accountingEntries.FindAll(x => x.Debit > 0 &&
                                                            !reconciliationList.ToFixedList()
                                                            .Contains(y => y.AccountingEntry.Id == x.Id));


      foreach (var acctEntry in remainingEntries) {
        reconciliationList.Add(new Reconcilation(BudgetEntry.Empty, acctEntry));
      }

      return reconciliationList.ToFixedList();
    }


    private Reconcilation Reconcilate(BudgetEntry exerciseTxnEntry,
                                      FixedList<CashEntryExtendedDto> accountingEntries) {

      var accountingEntry = accountingEntries.Find(x => x.Debit == exerciseTxnEntry.Amount &&
                                                        exerciseTxnEntry.ControlNo.Contains(x.VerificationNumber));

      if (accountingEntry != null) {
        return new Reconcilation(exerciseTxnEntry, accountingEntry);
      } else {
        return new Reconcilation(exerciseTxnEntry, CashEntryExtendedDto.Empty);
      }
    }

    #region Helpers

    private async Task<FixedList<CashEntryExtendedDto>> GetAccountingEntries() {
      var financialAccountingServices = new CashTransactionServices();

      var query = new BaseCashLedgerQuery {
        FromAccountingDate = _fromDate,
        ToAccountingDate = _toDate,
        VoucherAccounts = new[] { "2.07.04.07", "2.07.04.08" }
      };

      return await financialAccountingServices.SearchEntries(query);
    }


    private FixedList<BudgetTransaction> GetBudgetExerciseTransactions() {
      return BaseObject.GetFullList<BudgetTransaction>()
                       .FindAll(x => x.OperationType == BudgetOperationType.Exercise &&
                                     _fromDate <= x.ApplicationDate && x.ApplicationDate <= _toDate);
    }

    #endregion Helpers

  }  // class BudgetExerciseAccountingReconciliator

}  // namespace Empiria.Banobras.Reporting.Builders.Budgeting
