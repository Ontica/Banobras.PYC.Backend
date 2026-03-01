/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Management                 Component : Application services Layer           *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Application service                  *
*  Type     : BudgetExerciseAppServices                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Application services for generate budget exercise transactions.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Services;
using Empiria.StateEnums;

using Empiria.Orders;
using Empiria.Orders.Contracts;

using Empiria.Payments;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

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

      CleanBudgetCommitsDates();

      CleanBudgetApprovePaymentDatesBeforeCommits();

      int counter = CleanBudgetCommits();

      counter += CleanBudgetApprovePaymentsDatesAfterCommits();

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

        //var commitTxns = BudgetTransaction.GetRelatedTo(txn)
        //                                  .FindAll(x => x.OperationType == BudgetOperationType.Commit);

        //entries = cleaner.CreateAdjustMonthsEntries(txn, commitTxns.SelectFlat(x => x.Entries));

        //if (entries.Count == 0) {
        //  continue;
        //}

        //foreach (var entry in entries) {
        //  entry.Save();
        //}

        //EmpiriaLog.Info($"Created {entries.Count} adjustment entries for payment transaction {txn.TransactionNo} - {txn.Id}.");

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
                                                  x.BudgetAccount.Id == paymentEntry.BudgetAccount.Id &&
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


    public int ExerciseBudget() {

      var budgetAppServices = BudgetExecutionAppServices.UseCaseInteractor();

      FixedList<PaymentOrder> paymentOrders = GetPayedPaymentOrders();

      int counter = 0;

      foreach (var paymentOrder in paymentOrders) {

        Order order = (Order) paymentOrder.PayableEntity;

        var exerciseDate = paymentOrder.LastPaymentInstruction.LastUpdateTime;

        BudgetTransaction approvePaymentTxn = TryGetUnexercisedApprovePaymentBudgetTransaction(order);

        if (approvePaymentTxn == null) {
          continue;
        }

        _ = budgetAppServices.ExerciseBudget(paymentOrder, approvePaymentTxn, exerciseDate);

        UpdatePaymentApprovalBudgetTransaction(approvePaymentTxn, paymentOrder);

        counter++;
      }

      return counter;
    }

    #endregion Application services

    #region Helpers

    static private FixedList<PaymentOrder> GetPayedPaymentOrders() {
      return PaymentOrder.GetList<PaymentOrder>()
                         .FindAll(x => x.Status == PaymentOrderStatus.Payed &&
                                       x.PayableEntity.Budget.UID != BudgetType.None.UID)
                         .ToFixedList()
                         .Sort((x, y) => x.LastPaymentInstruction.LastUpdateTime.CompareTo(y.LastPaymentInstruction.LastUpdateTime))
                         .Reverse();
    }


    static private BudgetTransaction TryGetUnexercisedApprovePaymentBudgetTransaction(Order order) {
      var txns = BudgetTransaction.GetFor(order);

      if (txns.Contains(x => x.OperationType == BudgetOperationType.Exercise)) {
        return null;
      }

      return txns.FindLast(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                x.Status == TransactionStatus.Closed);

    }


    static private void UpdatePaymentApprovalBudgetTransaction(BudgetTransaction paymentApproval,
                                                               PaymentOrder paymentOrder) {

      paymentApproval.SetPayable(paymentOrder);

      var sql = $"UPDATE FMS_BUDGET_TRANSACTIONS " +
                $"SET BDG_TXN_PAYABLE_ID = {paymentOrder.Id} " +
                $"WHERE BDG_TXN_ID = {paymentApproval.Id}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }

    #endregion Helpers

  }  // class BudgetExerciseAppServices

}  // namespace Empiria.Banobras.Budgeting.AppServices
