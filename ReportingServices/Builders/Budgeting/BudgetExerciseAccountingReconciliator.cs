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
using System.Linq;
using System.Threading.Tasks;

using Empiria.CashFlow.CashLedger.Adapters;

using Empiria.FinancialAccounting.ClientServices;

namespace Empiria.Banobras.Reporting.Builders.Budgeting {

  public class ExerciseReconciliationDto {

    public string TransaccionNo {
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

  }  // class ExerciseReconciliationDto



  /// <summary>Reconciliates accounting records with budget exercise transactions.</summary>
  internal class BudgetExerciseAccountingReconciliator {

    private DateTime _fromDate;
    private DateTime _toDate;

    public BudgetExerciseAccountingReconciliator(DateTime fromDate, DateTime toDate) {
      _fromDate = fromDate;
      _toDate = toDate;
    }


    internal async Task<FixedList<ExerciseReconciliationDto>> Build() {

      var financialAccountingServices = new CashTransactionServices();

      var query = new BaseCashLedgerQuery {
        FromAccountingDate = _fromDate,
        ToAccountingDate = _toDate,
        VoucherAccounts = new[] { "2.07.04.07", "2.07.04.08" }
      };

      FixedList<CashEntryExtendedDto> entries = await financialAccountingServices.SearchEntries(query);

      var grouped = entries.GroupBy(x => new { x.AccountNumber, x.AccountName, x.VerificationNumber });

      var dtos = grouped.Select(x => Map(x.Key.AccountNumber, x.Key.AccountName, x.Key.VerificationNumber, x.ToFixedList()))
                        .OrderBy(x => x.AccountNumber)
                        .ThenBy(x => x.VerificationNumber)
                        .ToFixedList();

      return dtos;
    }

    #region Helpers

    private ExerciseReconciliationDto Map(string accountNumber, string accountName,
                                          string verificationNumber,
                                          FixedList<CashEntryExtendedDto> list) {

      return new ExerciseReconciliationDto {
        TransaccionNo = "2026-EJR-CF-00110",
        BudgetAccount = "90103",
        BudgetAccountName = string.Empty,
        BudgetControlNumber = string.Empty,
        BudgetExercise = 0,
        AccountNumber = accountNumber,
        AccountName = accountName,
        VerificationNumber = verificationNumber,
        Debit = list.Sum(x => x.Debit),
        Credit = list.Sum(x => x.Credit)
      };
    }

    #endregion Helpers

  }  // class BudgetExerciseAccountingReconciliator

}  // namespace Empiria.Banobras.Reporting.Builders.Budgeting
