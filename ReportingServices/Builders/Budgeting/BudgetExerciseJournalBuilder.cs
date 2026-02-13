/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetExerciseJournalBuilder                  License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a dynamic journal with budget exercise transaction's entries.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.DynamicData;

using Empiria.Payments;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  public class BudgetExerciseJournalEntry {

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

    public string ControlNo {
      get; internal set;
    }

    public string BudgetTransactionNo {
      get; internal set;
    }

    public string MonthName {
      get; internal set;
    }

    public decimal ToPay {
      get; internal set;
    }

    public decimal Exercise {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string PaymentOrderNo {
      get; internal set;
    }

    public string PaymentMethod {
      get; internal set;
    }

    public string PayTo {
      get; internal set;
    }

    public DateTime PaymentDate {
      get; internal set;
    }

    public string Status {
      get; internal set;
    }

  }  // class BudgetExerciseJournalEntry



  /// <summary>Builds a dynamic journal with budget exercise transaction's entries.</summary>
  public class BudgetExerciseJournalBuilder {

    private readonly FixedList<BudgetTransaction> _transactions;

    internal BudgetExerciseJournalBuilder(FixedList<BudgetTransaction> transactions) {
      _transactions = transactions;
    }

    internal FixedList<DataTableColumn> BuildColumns() {
      return new List<DataTableColumn>() {
        new DataTableColumn("orgUnit", "Área", "text"),
        new DataTableColumn("budgetAccount", "Partida", "text"),
        new DataTableColumn("budgetProgram", "Programa", "text"),
        new DataTableColumn("budget", "Presupuesto", "text"),
        new DataTableColumn("controlNo", "Num Verif", "text-nowrap"),
        new DataTableColumn("budgetTransactionNo", "'Transacción'", "text-nowrap"),
        new DataTableColumn("monthName", "Mes", "text"),
        new DataTableColumn("toPay", "Por pagar", "decimal"),
        new DataTableColumn("exercise", "Ejercido", "decimal"),
        new DataTableColumn("description", "Descripción", "text"),
        new DataTableColumn("paymentOrderNo", "No Solicitud pago", "text-nowrap"),
        new DataTableColumn("paymentMethod", "Método de pago", "text"),
        new DataTableColumn("payTo", "Pagar a", "text"),
        new DataTableColumn("paymentDate", "Fecha pago", "date"),
        new DataTableColumn("status", "Estado", "text"),
      }.ToFixedList();
    }


    internal FixedList<BudgetExerciseJournalEntry> BuildEntries() {
      var entries = new List<BudgetExerciseJournalEntry>(_transactions.Count * 2);

      foreach (var txn in _transactions) {

        foreach (var entry in txn.Entries.FindAll(x => x.Deposit > 0)) {

          BudgetExerciseJournalEntry journalEntry = CreateJournalEntry(txn, entry);

          entries.Add(journalEntry);
        }
      }

      return entries.ToFixedList();
    }

    #region Helpers

    static private BudgetExerciseJournalEntry CreateJournalEntry(BudgetTransaction txn,
                                                                 BudgetEntry entry) {

      var journalEntry = new BudgetExerciseJournalEntry() {
        UID = txn.UID,
        OrgUnit = entry.BudgetAccount.OrganizationalUnit.FullName,
        BudgetAccount = ((INamedEntity) entry.BudgetAccount).Name,
        Budget = entry.Budget.Name,
        BudgetProgram = entry.BudgetProgram.Code,
        BudgetTransactionNo = txn.TransactionNo,
        ControlNo = entry.ControlNo,
        Description = entry.Description,
        MonthName = entry.MonthName,
        Status = txn.Status.ToString()
      };

      var paymentOrder = PaymentOrder.Parse(txn.PayableId);

      if (txn.OperationType == BudgetOperationType.Exercise) {

        journalEntry.Exercise = entry.Deposit;
        journalEntry.PaymentDate = paymentOrder.LastPaymentInstruction.LastUpdateTime;
        journalEntry.PaymentMethod = paymentOrder.PaymentMethod.Name;
        journalEntry.PayTo = paymentOrder.PayTo.Name;
        journalEntry.PaymentOrderNo = paymentOrder.PaymentOrderNo;


      } else if (!paymentOrder.IsEmptyInstance) {
        journalEntry.ToPay = entry.Deposit;
        journalEntry.PaymentDate = ExecutionServer.DateMinValue;
        journalEntry.PaymentMethod = paymentOrder.PaymentMethod.Name;
        journalEntry.PayTo = paymentOrder.PayTo.Name;
        journalEntry.PaymentOrderNo = paymentOrder.PaymentOrderNo;

      } else {
        journalEntry.ToPay = entry.Deposit;
        journalEntry.PaymentDate = ExecutionServer.DateMinValue;
      }

      return journalEntry;
    }

    #endregion Helpers

  }  // class BudgetExerciseJournalBuilder

}  // namespace Empiria.Budgeting.Reporting
