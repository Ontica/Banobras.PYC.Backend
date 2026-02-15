/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetAllocationJournalBuilder                License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a dynamic journal with budget allocation transaction's entries.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.DynamicData;
using Empiria.StateEnums;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  public class BudgetAllocationJournalEntry {

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

    public string MonthName {
      get; internal set;
    }

    public decimal Authorized {
      get; internal set;
    }

    public decimal Expanded {
      get; internal set;
    }

    public decimal Reduced {
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

  }  // class BudgetAllocationJournalEntry



  /// <summary>Builds a dynamic journal with budget allocation transaction's entries.</summary>
  public class BudgetAllocationJournalBuilder {

    private readonly FixedList<BudgetTransaction> _transactions;

    internal BudgetAllocationJournalBuilder(FixedList<BudgetTransaction> transactions) {
      _transactions = transactions;
    }

    internal FixedList<DataTableColumn> BuildColumns() {
      return new List<DataTableColumn>() {
        new DataTableColumn("orgUnit", "Área", "text"),
        new DataTableColumn("budgetAccount", "Partida", "text"),
        new DataTableColumn("budgetProgram", "Programa", "text"),
        new DataTableColumn("monthName", "Mes", "text"),
        new DataTableColumn("authorized", "Autorizado", "decimal"),
        new DataTableColumn("expanded", "Ampliaciones", "decimal"),
        new DataTableColumn("reduced", "Reducciones", "decimal"),
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


    internal FixedList<BudgetAllocationJournalEntry> BuildEntries() {
      var entries = new List<BudgetAllocationJournalEntry>(_transactions.Count * 2);

      foreach (var txn in _transactions) {

        foreach (var entry in txn.Entries) {

          BudgetAllocationJournalEntry journalEntry = CreateJournalEntry(txn, entry);

          entries.Add(journalEntry);
        }
      }

      return entries.ToFixedList();
    }

    #region Helpers

    static private BudgetAllocationJournalEntry CreateJournalEntry(BudgetTransaction txn,
                                                                   BudgetEntry entry) {

      var journalEntry = new BudgetAllocationJournalEntry() {
        UID = txn.UID,
        OrgUnit = entry.BudgetAccount.OrganizationalUnit.FullName,
        BudgetAccount = ((INamedEntity) entry.BudgetAccount).Name,
        Budget = entry.Budget.Name,
        BudgetProgram = entry.BudgetProgram.Code,
        BudgetTransactionNo = txn.TransactionNo,
        Description = EmpiriaString.FirstWithValue(entry.Description, txn.Description, txn.Justification),
        MonthName = entry.MonthName,
        RequestedDate = txn.RequestedDate,
        RequestedBy = txn.RequestedBy.Name,
        AuthorizationDate = txn.AuthorizationDate,
        AuthorizedBy = txn.AuthorizedBy.Name,
        Status = txn.Status.GetName()
      };

      decimal amount = entry.Deposit > 0 ? entry.Deposit : entry.Withdrawal * -1;

      if (entry.BalanceColumn.Equals(BalanceColumn.Authorized)) {
        journalEntry.Authorized = amount;

      } else if (entry.BalanceColumn.Equals(BalanceColumn.Expanded)) {

        journalEntry.Expanded = amount;

      } else if (entry.BalanceColumn.Equals(BalanceColumn.Reduced)) {
        journalEntry.Reduced = amount;

      }

      return journalEntry;
    }

    #endregion Helpers

  }  // class BudgetAllocationJournalBuilder

}  // namespace Empiria.Budgeting.Reporting
