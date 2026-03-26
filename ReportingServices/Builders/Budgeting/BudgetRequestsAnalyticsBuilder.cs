/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetRequestsAnalyticsBuilder                License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds budget requests analytics entries.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Orders;
using Empiria.Orders.Contracts;

using Empiria.Payments;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  public class BudgetRequestsAnalyticsEntry {

    public BudgetAccount BudgetAccount {
      get; internal set;
    }

    public BudgetProgram BudgetProgram {
      get; internal set;
    }

    public Budget Budget {
      get; internal set;
    }

    public string ControlNo {
      get; internal set;
    }

    public BudgetTransaction RequestTxn {
      get; internal set;
    }

    public BudgetTransaction CommitTxn {
      get; internal set;
    }

    public BudgetTransaction ApprovePaymentTxn {
      get; internal set;
    }

    public BudgetTransaction ExerciseTxn {
      get; internal set;
    }

    public decimal Amount {
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

    public decimal ToExercise {
      get {
        return Requested + Committed + ToPay;
      }
    }

    public int Month {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public Requisition Requisition {
      get {
        return RequestTxn.GetEntity() as Requisition;
      }
    }

    public Contract Contract {
      get {
        if (CommitTxn.IsEmptyInstance) {
          return Contract.Empty;
        }
        if (CommitTxn.GetEntity() is Contract) {
          return CommitTxn.GetEntity() as Contract;
        }
        return Contract.Empty;
      }
    }

    public PayableOrder PayableOrder {
      get; internal set;
    }

    public PaymentOrder PaymentOrder {
      get; internal set;
    }

    public Payee Debtor {
      get {
        if (PaymentOrder.IsEmptyInstance) {
          return Payee.Empty;
        }
        if (!PaymentOrder.Debtor.IsEmptyInstance) {
          return PaymentOrder.Debtor as Payee;
        }
        return Payee.Empty;
      }
    }

    public string AccountingVoucher {
      get; internal set;
    }

  }  // class BudgetRequestsAnalyticsEntry



  /// <summary>Builds budget requests analytics entries.</summary>
  public class BudgetRequestsAnalyticsBuilder {

    private readonly FixedList<BudgetTransaction> _transactions;

    internal BudgetRequestsAnalyticsBuilder(FixedList<BudgetTransaction> transactions) {
      _transactions = transactions.FindAll(x => x.IsClosed);
    }


    internal FixedList<BudgetRequestsAnalyticsEntry> BuildEntries() {
      var toReturnEntries = new List<BudgetRequestsAnalyticsEntry>(_transactions.Count * 2);

      var allEntries = _transactions.SelectFlat(x => x.Entries)
                                    .FindAll(x => x.BalanceColumn.Distinct(BalanceColumn.Available) &&
                                                  x.NotAdjustment);

      foreach (var requestTxn in _transactions.FindAll(x => x.OperationType == BudgetOperationType.Request)) {

        var controlNos = allEntries.FindAll(x => x.Transaction.Equals(requestTxn))
                                   .SelectDistinct(x => x.ControlNo);

        var relatedEntries = allEntries.FindAll(x => controlNos.Contains(y => x.ControlNo.StartsWith(y)))
                                       .OrderBy(x => x.ControlNo)
                                       .ThenBy(x => x.Transaction.ApplicationDate)
                                       .ToFixedList();

        FixedList<BudgetRequestsAnalyticsEntry> analyticsEntries = BuildAnalyticsEntries(requestTxn, relatedEntries);

        toReturnEntries.AddRange(analyticsEntries);
      }

      return toReturnEntries.ToFixedList();
    }

    #region Helpers


    private FixedList<BudgetRequestsAnalyticsEntry> BuildAnalyticsEntries(BudgetTransaction requestTxn,
                                                                          FixedList<BudgetEntry> relatedEntries) {

      var entries = new List<BudgetRequestsAnalyticsEntry>(2);

      var controlNoGroups = relatedEntries.GroupBy(x => new { x.ControlNo, x.BudgetAccount.OrganizationalUnit })
                                          .OrderBy(x => x.Key.ControlNo)
                                          .ThenBy(x => x.Key.OrganizationalUnit.Code);

      foreach (var groupedEntries in controlNoGroups) {

        if (!groupedEntries.Key.ControlNo.Contains("/")) {
          entries.Add(BuildRequestEntry(requestTxn,
                                        relatedEntries.FindAll(x => x.ControlNo.StartsWith(groupedEntries.Key.ControlNo))));
        } else {
          var entry = TryBuildExerciseEntry(requestTxn,
                                            relatedEntries.ToFixedList().SelectDistinct(x => x.Transaction),
                                            groupedEntries.ToFixedList());

          if (entry != null) {
            entries.Add(entry);
          }
        }
      }

      return entries.ToFixedList();
    }


    static private BudgetRequestsAnalyticsEntry BuildRequestEntry(BudgetTransaction requestTxn,
                                                                  FixedList<BudgetEntry> entries) {

      var requestEntry = entries.Last(x => x.Transaction.OperationType == BudgetOperationType.Request);

      var commitTxn = entries.FindLast(x => x.Transaction.OperationType == BudgetOperationType.Commit)?.Transaction ?? BudgetTransaction.Empty;

      var journalEntry = new BudgetRequestsAnalyticsEntry {
        BudgetAccount = requestEntry.BudgetAccount,
        BudgetProgram = requestEntry.BudgetProgram,
        ControlNo = requestEntry.ControlNo,
        RequestTxn = requestTxn,
        CommitTxn = commitTxn,
        ApprovePaymentTxn = BudgetTransaction.Empty,
        ExerciseTxn = BudgetTransaction.Empty,
        PayableOrder = PayableOrder.Empty,
        PaymentOrder = PaymentOrder.Empty,
        AccountingVoucher = string.Empty,
        Month = requestEntry.Month,
        Description = EmpiriaString.FirstWithValue(requestEntry.Description, requestEntry.Transaction.Description,
                                                   requestEntry.Transaction.Justification),
        Budget = requestEntry.Budget,
      };

      var groupedEntries = entries.GroupBy(x => x.BalanceColumn);

      foreach (var columnGroup in groupedEntries) {

        var groupEntries = columnGroup.ToFixedList();

        if (columnGroup.Key.Id == BalanceColumn.Requested.Id) {

          journalEntry.Amount = groupEntries.Sum(x => x.Deposit > 0 ? x.Deposit : 0);

          journalEntry.Requested = groupEntries.Sum(x => x.Deposit > 0 ? x.Deposit : -1 * x.Withdrawal);

        } else if (columnGroup.Key.Id == BalanceColumn.Commited.Id) {

          journalEntry.Committed = groupEntries.Sum(x => x.Deposit > 0 ? x.Deposit : -1 * x.Withdrawal);

        } else if (columnGroup.Key.Id == BalanceColumn.ToPay.Id) {

          journalEntry.ToPay = 0;

        } else if (columnGroup.Key.Id == BalanceColumn.Exercised.Id) {

          journalEntry.Exercised = 0;

        }
      }

      return journalEntry;
    }


    static private BudgetRequestsAnalyticsEntry TryBuildExerciseEntry(BudgetTransaction requestTxn,
                                                                      FixedList<BudgetTransaction> relatedTxns,
                                                                      FixedList<BudgetEntry> entries) {

      var approvePaymentEntry = entries.FindLast(x => x.Transaction.OperationType == BudgetOperationType.ApprovePayment);

      if (approvePaymentEntry == null) {
        approvePaymentEntry = entries.FindLast(x => x.Transaction.OperationType == BudgetOperationType.Exercise);
      }

      if (approvePaymentEntry == null) {
        return null;
      }

      var commitTxn = relatedTxns.FindLast(x => x.OperationType == BudgetOperationType.Commit) ?? BudgetTransaction.Empty;

      var payableOrder = approvePaymentEntry.Transaction.GetEntity() as PayableOrder;

      var paymentOrder = PaymentOrder.Parse(approvePaymentEntry.Transaction.PayableId);

      var exerciseTxn = relatedTxns.Find(x => x.PayableId == paymentOrder.Id &&
                                              x.OperationType == BudgetOperationType.Exercise) ?? BudgetTransaction.Empty;


      var journalEntry = new BudgetRequestsAnalyticsEntry {
        BudgetAccount = approvePaymentEntry.BudgetAccount,
        BudgetProgram = approvePaymentEntry.BudgetProgram,
        RequestTxn = requestTxn,
        CommitTxn = commitTxn,
        ApprovePaymentTxn = approvePaymentEntry.Transaction,
        ExerciseTxn = exerciseTxn,
        ControlNo = approvePaymentEntry.ControlNo,
        PayableOrder = payableOrder,
        PaymentOrder = paymentOrder,
        Month = Math.Max(approvePaymentEntry.Month, exerciseTxn.ApplicationDate.Month),
        AccountingVoucher = !exerciseTxn.IsEmptyInstance ? "Por determinar" : string.Empty,
        Description = EmpiriaString.FirstWithValue(approvePaymentEntry.Description,
                                                   approvePaymentEntry.Transaction.Description,
                                                   approvePaymentEntry.Transaction.Justification),
        Budget = approvePaymentEntry.Budget
      };

      journalEntry.Amount = entries.FindAll(x => x.Deposit > 0 && x.BalanceColumn.Equals(BalanceColumn.ToPay))
                                   .Sum(x => x.Deposit);

      journalEntry.ToPay = entries.FindAll(x => x.BalanceColumn.Equals(BalanceColumn.ToPay))
                                  .Sum(x => x.Deposit > 0 ? x.Deposit : -1 * x.Withdrawal);

      journalEntry.Exercised = entries.FindAll(x => x.BalanceColumn.Equals(BalanceColumn.Exercised))
                                      .Sum(x => x.Deposit > 0 ? x.Deposit : -1 * x.Withdrawal);

      return journalEntry;
    }

    #endregion Helpers

  }  // class BudgetRequestsAnalyticsBuilder

}  // namespace Empiria.Budgeting.Reporting
