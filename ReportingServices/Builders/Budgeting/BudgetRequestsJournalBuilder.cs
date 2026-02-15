/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetRequestsJournalBuilder                  License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a dynamic journal with budget requests transaction's entries.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.DynamicData;
using Empiria.StateEnums;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  public class BudgetRequestsJournalEntry {

    public string UID {
      get; internal set;
    }

    public string OrgUnit {
      get; internal set;
    }

    public string BudgetAccount {
      get; internal set;
    }

    public string BudgetProgram {
      get; internal set;
    }

    public string Budget {
      get; internal set;
    }

    public string BudgetTransactionNo {
      get; internal set;
    }

    public string ControlNo {
      get; internal set;
    }

    public string MonthName {
      get; internal set;
    }

    public decimal Requested {
      get; internal set;
    }

    public decimal Committed {
      get; internal set;
    }

    public decimal ToPay {
      get; internal set;
    }

    public decimal Exercised {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string RequestedBy {
      get; internal set;
    }

    public DateTime RequestedDate {
      get; internal set;
    }

    public DateTime AuthorizationDate {
      get; internal set;
    }

    public string AuthorizedBy {
      get; internal set;
    }

    public string Status {
      get; internal set;
    }

  }  // class BudgetRequestsJournalEntry



  /// <summary>Builds a dynamic journal with budget requests transaction's entries.</summary>
  public class BudgetRequestsJournalBuilder {

    private readonly FixedList<BudgetTransaction> _transactions;

    internal BudgetRequestsJournalBuilder(FixedList<BudgetTransaction> transactions) {
      _transactions = transactions;
    }


    internal FixedList<DataTableColumn> BuildColumns() {
      return new List<DataTableColumn>() {
        new DataTableColumn("orgUnit", "Área", "text"),
        new DataTableColumn("budgetAccount", "Partida", "text"),
        new DataTableColumn("budgetProgram", "Programa", "text"),
        new DataTableColumn("controlNo", "Núm Verif", "text-nowrap"),
        new DataTableColumn("monthName", "Mes", "text"),
        new DataTableColumn("requested", "Apartado", "decimal"),
        new DataTableColumn("committed", "Comprometido", "decimal"),
        new DataTableColumn("toPay", "Por pagar", "decimal"),
        new DataTableColumn("exercised", "Ejercido", "decimal"),
        new DataTableColumn("budgetTransactionNo", "Transacción", "text-nowrap"),
        new DataTableColumn("description", "Descripción", "text"),
        new DataTableColumn("requestedDate", "Solicitado el", "date"),
        new DataTableColumn("requestedBy", "Solicitado por", "text"),
        new DataTableColumn("authorizationDate", "Autorizado el", "date"),
        new DataTableColumn("authorizedBy", "Autorizado por", "text"),
        new DataTableColumn("budget", "Presupuesto", "text"),
        new DataTableColumn("status", "Estado", "text")
      }.ToFixedList();
    }


    internal FixedList<BudgetRequestsJournalEntry> BuildEntries() {
      var entries = new List<BudgetRequestsJournalEntry>(_transactions.Count * 2);

      foreach (var txn in _transactions) {

        foreach (var entry in txn.Entries.FindAll(x => x.Deposit > 0)) {

          BudgetRequestsJournalEntry journalEntry = CreateJournalEntry(txn, entry);

          entries.Add(journalEntry);
        }
      }

      return entries.ToFixedList();
    }

    #region Helpers

    static private BudgetRequestsJournalEntry CreateJournalEntry(BudgetTransaction txn,
                                                                 BudgetEntry entry) {

      var journalEntry = new BudgetRequestsJournalEntry() {
        UID = txn.UID,
        OrgUnit = entry.BudgetAccount.OrganizationalUnit.FullName,
        BudgetAccount = ((INamedEntity) entry.BudgetAccount).Name,
        Budget = entry.Budget.Name,
        BudgetProgram = entry.BudgetProgram.Code,
        BudgetTransactionNo = txn.TransactionNo,
        ControlNo = entry.ControlNo,
        Description = EmpiriaString.FirstWithValue(entry.Description, txn.Description, txn.Justification),
        MonthName = entry.MonthName,
        RequestedDate = txn.RequestedDate,
        RequestedBy = txn.RequestedBy.Name,
        AuthorizationDate = txn.AuthorizationDate,
        AuthorizedBy = txn.AuthorizedBy.Name,
        Status = txn.Status.GetName()
      };

      if (entry.BalanceColumn.Equals(BalanceColumn.Requested)) {
        journalEntry.Requested = entry.Deposit;

      } else if (entry.BalanceColumn.Equals(BalanceColumn.Commited)) {
        journalEntry.Committed = entry.Deposit;

      } else if (entry.BalanceColumn.Equals(BalanceColumn.ToPay)) {
        journalEntry.ToPay = entry.Deposit;

      } else if (entry.BalanceColumn.Equals(BalanceColumn.Exercised)) {
        journalEntry.Exercised = entry.Deposit;

      }

      return journalEntry;
    }

    #endregion Helpers

  }  // class BudgetRequestsJournalBuilder

}  // namespace Empiria.Budgeting.Reporting
