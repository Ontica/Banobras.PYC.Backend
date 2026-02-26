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

    public int CleanBudgetCommits() {

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


    public int ExerciseBudget() {

      var budgetAppServices = BudgetExecutionAppServices.UseCaseInteractor();

      FixedList<PaymentOrder> paymentOrders = GetPayedPaymentOrders();

      int counter = 0;

      foreach (var paymentOrder in paymentOrders) {

        if (counter >= CommonData.BATCH_SIZE) {
          break;
        }

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
