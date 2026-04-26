/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetEntriesJournalBuilder                   License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a dynamic journal with budget transaction's entries.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.DynamicData;
using Empiria.StateEnums;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  public class BudgetEntryJournalDto {

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

    public string BalanceColumn {
      get; internal set;
    }

    public decimal Deposit {
      get; internal set;
    }

    public decimal Withdrawal {
      get; internal set;
    }

    public string IsAdjustment {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public string Status {
      get; internal set;
    }

  }  // class BudgetEntryJournalDto



  /// <summary>Builds a dynamic journal with budget transaction's entries.</summary>
  public class BudgetEntriesJournalBuilder {

    private readonly FixedList<BudgetTransaction> _transactions;

    internal BudgetEntriesJournalBuilder(FixedList<BudgetTransaction> transactions) {
      _transactions = transactions;
    }


    internal FixedList<DataTableColumn> BuildColumns() {
      return new List<DataTableColumn>() {
        new DataTableColumn("budgetTransactionNo", "Transacción", "text-link",
                            linkField: "uid", action: "ViewBudgetTransaction"),
        new DataTableColumn("orgUnit", "Área", "text"),
        new DataTableColumn("budgetAccount", "Partida", "text"),
        new DataTableColumn("budgetProgram", "Programa", "text"),
        new DataTableColumn("description", "Descripción", "text"),
        new DataTableColumn("controlNo", "Núm Verif", "text-nowrap"),
        new DataTableColumn("monthName", "Mes", "text"),
        new DataTableColumn("balanceColumn", "Movimiento", "text"),
        new DataTableColumn("deposit", "Ampliaciones", "decimal"),
        new DataTableColumn("withdrawal", "Reducciones", "decimal"),
        new DataTableColumn("isAdjustment", "Ajuste", "text"),
        new DataTableColumn("applicationDate", "Fecha aplicación", "date"),
        new DataTableColumn("budget", "Presupuesto", "text"),
        new DataTableColumn("status", "Estado", "text")
      }.ToFixedList();
    }


    internal FixedList<BudgetEntryJournalDto> BuildEntries() {
      var entries = new List<BudgetEntryJournalDto>(_transactions.Count * 2);

      foreach (var txn in _transactions) {

        foreach (var entry in txn.Entries) {

          BudgetEntryJournalDto journalEntry = CreateJournalEntry(txn, entry);

          entries.Add(journalEntry);
        }
      }

      return entries.ToFixedList();
    }

    #region Helpers

    static private BudgetEntryJournalDto CreateJournalEntry(BudgetTransaction txn, BudgetEntry entry) {

      var journalEntry = new BudgetEntryJournalDto {
        UID = txn.UID,
        BudgetTransactionNo = txn.TransactionNo,
        OrgUnit = entry.BudgetAccount.OrganizationalUnit.Name,
        BudgetAccount = ((INamedEntity) entry.BudgetAccount).Name,
        BudgetProgram = entry.BudgetProgram.Code,
        ControlNo = entry.ControlNo,
        Description = EmpiriaString.FirstWithValue(entry.Description, txn.Description, txn.Justification),
        MonthName = entry.MonthName,
        BalanceColumn = entry.BalanceColumn.Name,
        Deposit = entry.Deposit,
        Withdrawal = entry.Withdrawal,
        IsAdjustment = entry.IsAdjustment ? "Sí" : "No",
        ApplicationDate = txn.ApplicationDate,
        Budget = entry.Budget.Name,
        Status = txn.Status.GetName()
      };

      return journalEntry;
    }

    #endregion Helpers

  }  // class BudgetEntriesJournalBuilder

}  // namespace Empiria.Budgeting.Reporting
