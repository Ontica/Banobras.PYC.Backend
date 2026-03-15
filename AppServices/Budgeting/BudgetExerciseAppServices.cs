/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Management                 Component : Application services Layer           *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Application service                  *
*  Type     : BudgetExerciseAppServices                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Application services for generate budget exercise transactions.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;
using Empiria.Financial;
using Empiria.Services;
using Empiria.StateEnums;

using Empiria.Billing;

using Empiria.Orders;
using Empiria.Orders.Contracts;
using Empiria.Orders.Data;

using Empiria.Payments;
using Empiria.Payments.Data;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Data;

namespace Empiria.Banobras.Budgeting.AppServices {

  /// <summary>Application services for generate budget exercise transactions.</summary>
  public class BudgetExerciseAppServices : UseCase {

    #region Constructors and parsers

    protected BudgetExerciseAppServices() {

    }

    static public BudgetExerciseAppServices UseCaseInteractor() {
      return CreateInstance<BudgetExerciseAppServices>();
    }

    #endregion Constructors and parsers

    #region Application services

    public int CleanBudget() {

      int counter = SetPayables();

      CleanOrders();

      CleanPaymentOrders();

      CleanTransactions();

      CleanBudgetCommitsDates();

      CleanBudgetApprovePaymentDatesBeforeCommits();

      counter += CleanBudgetCommits();

      counter += CleanBudgetApprovePaymentsDatesAfterCommits();

      counter += CleanBudgetApprovePayments();

      counter += CleanBudgetCommitsWithCrossedAccounts();

      return counter;
    }


    public int ExerciseBudget() {

      var budgetAppServices = BudgetExecutionAppServices.UseCaseInteractor();

      FixedList<PaymentOrder> paymentOrders = GetPaymentOrders(false);

      int counter = 0;

      foreach (var paymentOrder in paymentOrders) {

        Order order = (Order) paymentOrder.PayableEntity;

        var exerciseDate = paymentOrder.LastPaymentInstruction.LastUpdateTime;

        BudgetTransaction approvePaymentTxn = TryGetUnexercisedApprovePaymentBudgetTransaction(order);

        if (approvePaymentTxn == null) {
          continue;
        }

        _ = budgetAppServices.ExerciseBudget(paymentOrder, approvePaymentTxn, exerciseDate);

        CloseOrder(order);

        CloseBills(order);

        counter++;
      }

      EmpiriaLog.Info($"Se generaron automáticamente {counter} transacciones del ejercicio presupuestal.");

      return counter;
    }

    #endregion Application services

    #region Budget cleaners

    private int CleanBudgetApprovePayments() {
      int counter = 0;

      FixedList<BudgetTransaction> paymentTxns = BudgetTransaction.GetFullList<BudgetTransaction>()
                                                                  .FindAll(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                                                                (x.InProcess || x.IsClosed) &&
                                                                                !x.Entries.Contains(y => y.IsAdjustment));

      var cleaner = new BudgetTransactionCleaner();

      foreach (var paymentTxn in paymentTxns) {

        Order commitOrder = (Order) paymentTxn.GetEntity();

        if (commitOrder is ContractOrder) {
          commitOrder = ((ContractOrder) commitOrder).Contract;
        }

        var commitTxns = BudgetTransaction.GetRelatedTo(paymentTxn)
                                          .FindAll(x => x.OperationType == BudgetOperationType.Commit &&
                                                       (x.InProcess || x.IsClosed) &&
                                                       (x.GetEntity().Equals(commitOrder) ||
                                                        x.GetEntity().Equals(paymentTxn.GetEntity())));

        if (commitTxns.Count > 1) {
          EmpiriaLog.Info($"Payment transaction {paymentTxn.TransactionNo} - {paymentTxn.Id} found " +
                          $"with {commitTxns.Count} commit transactions. Commit order : {commitOrder.OrderNo}");

        } else if (commitTxns.Count == 0) {
          counter += UpdateWithdrawalColumns(paymentTxn, BalanceColumn.Commited, BalanceColumn.Requested);

          continue;
        }

        try {
          FixedList<BudgetEntry> entries = cleaner.CreateAdjustMonthsEntries(paymentTxn, commitTxns.SelectFlat(x => x.Entries));

          if (entries.Count == 0) {
            continue;
          }

          foreach (var entry in entries) {
            entry.Save();
          }

          EmpiriaLog.Info($"Created {entries.Count} adjustment entries for payment transaction {paymentTxn.TransactionNo} - {paymentTxn.Id}.");

        } catch (Exception e) {
          EmpiriaLog.Error($"Error creating adjustment entries for payment transaction " +
                           $"{paymentTxn.TransactionNo} - {paymentTxn.Id}: {e.Message}");
          continue;
        }

        counter++;
      }

      return counter;
    }

    private int CleanBudgetApprovePaymentsDatesAfterCommits() {
      int counter = 0;

      FixedList<BudgetTransaction> paymentTxns = BudgetStatusAppServices.BudgetPaymentTxnForAdjustment();

      var cleaner = new BudgetTransactionCleaner();

      foreach (var txn in paymentTxns) {

        var commitTxns = BudgetTransaction.GetRelatedTo(txn)
                                          .FindAll(x => x.IsClosed && x.OperationType == BudgetOperationType.Commit &&
                                                         x.GetEntity().Id == txn.GetEntity().Id);

        if (commitTxns.Count > 1) {

          EmpiriaLog.Info($"Expected zero or one commit transactions for payment transaction {txn.TransactionNo} - {txn.Id}, but found {commitTxns.Count}.");
          continue;

        } else if (commitTxns.Count == 0 && txn.GetEntity() is ContractOrder contractOrder) {

          commitTxns = BudgetTransaction.GetRelatedTo(txn)
                                        .FindAll(x => x.IsClosed && x.OperationType == BudgetOperationType.Commit &&
                                                      x.GetEntity().Id == contractOrder.Contract.Id);

          if (commitTxns.Count != 1) {

            EmpiriaLog.Info($"Expected one commit transaction for contract linked payment transaction {txn.TransactionNo} - {txn.Id}, but found {commitTxns.Count}.");
            continue;

          } else {

            int cleaned = CleanBudgetApprovePaymentsDatesForContractOrders(txn, commitTxns[0]);
            counter += cleaned;
            continue;

          }

        } else if (commitTxns.Count == 0 && !(txn.GetEntity() is ContractOrder)) {
          EmpiriaLog.Info($"No commit transaction found for not contract linked payment transaction {txn.TransactionNo} - {txn.Id}.");
          continue;
        }

        var commitEntries = commitTxns[0].Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment && x.BalanceColumn == BalanceColumn.Commited);

        var entries = txn.Entries.FindAll(x => x.Withdrawal > 0 && x.BalanceColumn == BalanceColumn.Commited &&
                                               commitEntries.Contains(y => y.Month != x.Month));

        if (entries.Count == 0) {
          continue;
        }

        bool changed = false;

        foreach (var entry in entries) {
          var commitEntry = commitEntries.Find(x => x.EntityId == entry.EntityId && x.BudgetAccount.Id == entry.BudgetAccount.Id &&
                                                    x.Amount == entry.Amount && x.Month != entry.Month);

          if (commitEntry == null) {
            continue;
          }

          entry.SetDate(commitEntries[0].Date);
          entry.Save();
          changed = true;
        }

        if (changed) {
          EmpiriaLog.Info($"Created {entries.Count} date adjustment entries for payment transaction {txn.TransactionNo} - {txn.Id}.");
        }

        counter++;
      }

      return counter;
    }


    private int CleanBudgetApprovePaymentsDatesForContractOrders(BudgetTransaction txn, BudgetTransaction contractCommit) {

      var commitEntries = contractCommit.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment &&
                                                              x.BalanceColumn == BalanceColumn.Commited);

      var paymentEntries = txn.Entries.FindAll(x => x.Withdrawal > 0 && x.BalanceColumn == BalanceColumn.Commited &&
                                                    commitEntries.Contains(y => y.Month != x.Month));

      bool changed = false;

      foreach (var paymentEntry in paymentEntries) {
        var contractItem = (ContractItem) ContractOrderItem.Parse(paymentEntry.EntityId).ContractItem;

        var commitEntry = commitEntries.Find(x => x.EntityId == contractItem.Id && paymentEntry.ControlNo.StartsWith(x.ControlNo) &&
                                                  x.BudgetAccount.StandardAccount.Id == paymentEntry.BudgetAccount.StandardAccount.Id &&
                                                  x.Month != paymentEntry.Month);
        if (commitEntry == null) {
          continue;
        }

        paymentEntry.SetDate(commitEntry.Date);
        paymentEntry.Save();
        changed = true;
      }

      if (changed) {
        EmpiriaLog.Info($"Created {paymentEntries.Count} date adjustment entries for contract order payment transaction" +
                        $" {txn.TransactionNo} - {txn.Id}. - contract txn {contractCommit.TransactionNo}.");
        return 1;
      } else {
        return 0;
      }
    }

    private int CleanBudgetCommits() {
      int counter = 0;

      FixedList<BudgetTransaction> commitTxns = BudgetStatusAppServices.BudgetCommitTxnForAdjustment();

      var cleaner = new BudgetTransactionCleaner();

      foreach (var txn in commitTxns) {

        var requestTxns = BudgetTransaction.GetRelatedTo(txn)
                                           .FindAll(y => y.OperationType == BudgetOperationType.Request);


        FixedList<BudgetEntry> entries = cleaner.CreateAdjustMonthsEntries(txn, requestTxns.SelectFlat(x => x.Entries));

        if (entries.Count == 0) {
          continue;
        }

        foreach (var entry in entries) {
          entry.Save();
        }

        EmpiriaLog.Info($"Created {entries.Count} adjustment entries for commit transaction {txn.TransactionNo} - {txn.Id}.");

        counter++;
      }

      return counter;
    }


    private int CleanBudgetCommitsWithCrossedAccounts() {
      int counter = 0;

      var orders = Order.GetFullList<Order>().FindAll(x => x.HasCrossedBeneficiaries() && !(x is Requisition));

      foreach (var order in orders) {

        var commits = BudgetTransaction.GetFor(order is ContractOrder ? order.Contract : order)
                                       .FindAll(x => x.OperationType == BudgetOperationType.Commit &&
                                                (x.InProcess || x.IsClosed));

        var approvePayment = BudgetTransaction.GetFor(order)
                                              .FindAll(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                                            (x.InProcess || x.IsClosed));

        if (commits.Count != 1 || approvePayment.Count != 1) {
          EmpiriaLog.Info($"Order {order.OrderNo} - {order.Id} has crossed accounts. " +
                          $"Commits {string.Join(", ", commits.Select(x => x.TransactionNo))} --- " +
                          $"Approve Payments {string.Join(", ", approvePayment.Select(x => x.TransactionNo))}");
          continue;
        }

        var cleaner = new BudgetTransactionCleaner();

        FixedList<BudgetEntry> entries = cleaner.CreateCrossedAccountsAdjustEntries(approvePayment[0], commits[0]);

        if (entries.Count == 0) {
          continue;
        }

        foreach (var entry in entries) {
          entry.Save();
        }

        EmpiriaLog.Info($"Se agregaron {entries.Count} partidas a la transacción {approvePayment[0].TransactionNo} con cuentas cruzadas multiárea.");

        counter += entries.Count;
      }

      return counter;
    }


    private void CleanBudgetApprovePaymentDatesBeforeCommits() {

      FixedList<BudgetTransaction> paymentTxns = BudgetTransaction.GetFullList<BudgetTransaction>()
                                                                  .FindAll(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                                                                (x.InProcess || x.IsClosed));

      foreach (var txn in paymentTxns) {

        var applicationDate = txn.ApplicationDate;

        var toCleanEntries = txn.Entries.FindAll(x => x.Withdrawal > 0 && x.NotAdjustment && x.BalanceColumn == BalanceColumn.Commited &&
                                                      x.Date != applicationDate && x.Month == applicationDate.Month);

        foreach (var entry in toCleanEntries) {
          entry.SetDate(applicationDate);
          entry.Save();
        }

        toCleanEntries = txn.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment && x.BalanceColumn == BalanceColumn.ToPay &&
                                                  x.Date != applicationDate && x.Month == applicationDate.Month);

        foreach (var entry in toCleanEntries) {
          entry.SetDate(applicationDate);
          entry.Save();
        }

        toCleanEntries = txn.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment && x.BalanceColumn == BalanceColumn.ToPay &&
                                                  x.Month != applicationDate.Month);

        if (toCleanEntries.Count > 0) {
          EmpiriaLog.Info($"La transaction {txn.TransactionNo} {txn.Id} tiene mal el mes de aprobación .");
        }

      }

    }

    private void CleanBudgetCommitsDates() {

      FixedList<BudgetTransaction> commitTxns = BudgetTransaction.GetFullList<BudgetTransaction>()
                                                                 .FindAll(x => x.OperationType == BudgetOperationType.Commit &&
                                                                              (x.InProcess || x.IsClosed));

      foreach (var txn in commitTxns) {

        var applicationDate = txn.ApplicationDate;

        var toCleanEntries = txn.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment &&
                                                      x.BalanceColumn == BalanceColumn.Commited &&
                                                      x.Date != applicationDate && x.Month == applicationDate.Month);

        foreach (var entry in toCleanEntries) {
          entry.SetDate(applicationDate);
          entry.Save();
        }

      }
    }

    #endregion Budget cleaners

    #region Helpers

    static private void CleanOrders() {
      var orders = Order.GetFullList<Order>();

      foreach (var order in orders) {
        OrdersData.CleanOrder(order);
      }
    }


    static private void CleanPaymentOrders() {
      var paymentOrders = PaymentOrder.GetFullList<PaymentOrder>();

      foreach (var paymentOrder in paymentOrders) {
        PaymentOrderData.CleanPaymentOrder(paymentOrder);
      }
    }


    static private void CleanTransactions() {
      var transactions = BudgetTransaction.GetFullList<BudgetTransaction>()
                                          .FindAll(x => x.HasEntity);

      foreach (var txn in transactions) {
        BudgetTransactionDataService.CleanTransaction(txn);
      }
    }


    static private void CloseBills(Order order) {
      FixedList<Bill> bills = Bill.GetListFor((IPayableEntity) order);

      foreach (var bill in bills) {
        if (bill.Status == BillStatus.Payed) {
          continue;
        }

        try {
          bill.SetAsPayed();
          bill.Save();
          EmpiriaLog.Info($"Bill {bill.BillNo} linked to order {order.OrderNo} was marked as payed.");
        } catch (Exception e) {
          EmpiriaLog.Error($"Error marking bill {bill.BillNo} linked to order {order.OrderNo} as payed: {e.Message}");
        }
      }
    }


    static private void CloseOrder(Order order) {
      if (order.Status != EntityStatus.Closed) {
        try {
          order.Close(CommonData.GERENCIA_DE_PAGOS);
          order.Save();

          EmpiriaLog.Info($"Order {order.OrderNo} was auto-closed.");
        } catch (Exception e) {
          EmpiriaLog.Error($"Error auto-closing order {order.OrderNo}: {e.Message}");
        }
      }

      if (!(order is ContractOrder) &&
          order.Requisition.GetTotal() <= order.GetTotal() &&
          order.Requisition.Status != EntityStatus.Closed) {

        try {
          order.Requisition.Close(CommonData.GERENCIA_DE_CONTROL_PRESUPUESTAL);
          order.Requisition.Save();

          EmpiriaLog.Info($"Requisition {order.Requisition.OrderNo} was auto-closed, linked to order {order.OrderNo}.");

          if (order.Requisition.GetTotal() < order.GetTotal()) {
            EmpiriaLog.Critical($"WARNING !! Requisition {order.Requisition.OrderNo} total is {order.Requisition.GetTotal()} less than order {order.OrderNo} {order.GetTotal()} .");
          }
        } catch (Exception e) {
          EmpiriaLog.Error($"Error auto-closing requisition {order.Requisition.OrderNo}: {e.Message}");
        }
      }
    }


    static private FixedList<PaymentOrder> GetPaymentOrders(bool includeUnpaid) {
      return PaymentOrder.GetList<PaymentOrder>()
                         .FindAll(x => (x.Status == PaymentOrderStatus.Payed || (includeUnpaid && !x.IsEmptyInstance)) &&
                                        ((Budget) x.PayableEntity.Budget).BudgetType.UID != BudgetType.None.UID)
                         .ToFixedList()
                         .Sort((x, y) => x.LastPaymentInstruction.LastUpdateTime.CompareTo(y.LastPaymentInstruction.LastUpdateTime))
                         .Reverse();
    }


    static private int SetPayables() {

      FixedList<PaymentOrder> paymentOrders = GetPaymentOrders(true);

      int counter = 0;

      foreach (var paymentOrder in paymentOrders) {

        Order order = (Order) paymentOrder.PayableEntity;

        var txns = BudgetTransaction.GetFor(order)
                                    .FindAll(x => x.OperationType == BudgetOperationType.ApprovePayment);

        if (txns.Count == 0 && (paymentOrder.HasActivePaymentInstruction || paymentOrder.Payed)) {

          EmpiriaLog.Info($"No approve payment transaction found for " +
                          $"payment order {paymentOrder.PaymentOrderNo} linked to {order.OrderNo}. PO Status {paymentOrder.Status}");

          continue;
        }

        foreach (var txn in txns) {

          if (txn.PayableId != -1) {
            continue;
          }

          if (txn.GetEntity().Equals(order)) {

            txn.SetPayable(paymentOrder);

            var sql = $"UPDATE FMS_BUDGET_TRANSACTIONS " +
                      $"SET BDG_TXN_PAYABLE_ID = {paymentOrder.Id} " +
                      $"WHERE BDG_TXN_ID = {txn.Id}";

            var op = DataOperation.Parse(sql);

            DataWriter.Execute(op);

            EmpiriaLog.Info($"Payable set for {txn.TransactionNo} with {paymentOrder.PaymentOrderNo}.");

            counter++;

          } else {
            EmpiriaLog.Info($"The entity of the transaction {txn.TransactionNo} " +
                            $"linked to order {order.OrderNo} is not the order itself.");
          }
        }
      }

      return counter;
    }


    private int UpdateWithdrawalColumns(BudgetTransaction txn, BalanceColumn fromColumn, BalanceColumn toColumn) {
      var entries = txn.Entries.FindAll(x => x.Withdrawal > 0 && x.BalanceColumn == fromColumn);

      if (entries.Count == 0) {
        return 0;
      }

      foreach (var entry in entries) {
        entry.SetBalanceColumn(toColumn);
        entry.Save();
      }

      EmpiriaLog.Info($"Updated {entries.Count} entries from {fromColumn.Name} to {toColumn.Name} for transaction {txn.TransactionNo} - {txn.Id}.");

      return entries.Count;
    }


    static private BudgetTransaction TryGetUnexercisedApprovePaymentBudgetTransaction(Order order) {
      var txns = BudgetTransaction.GetFor(order);

      if (txns.Contains(x => x.OperationType == BudgetOperationType.Exercise)) {
        return null;
      }

      return txns.FindLast(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                x.Status == TransactionStatus.Closed);

    }

    #endregion Helpers

  }  // class BudgetExerciseAppServices

}  // namespace Empiria.Banobras.Budgeting.AppServices
