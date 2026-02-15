/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Management                 Component : Application services Layer           *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Application service                  *
*  Type     : BudgetExerciseAppServices                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Application services for generate budget exercise transactions.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.StateEnums;

using Empiria.Orders;
using Empiria.Payments;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

using Empiria.Banobras.Procurement.UseCases;
using Empiria.Data;

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

    public int ExerciseBudget() {

      const int BATCH_SIZE = 5;

      FixedList<PaymentOrder> paymentOrders = GetPayedPaymentOrders();

      int counter = 0;

      foreach (var paymentOrder in paymentOrders) {

        if (counter >= BATCH_SIZE) {
          break;
        }

        Order order = (Order) paymentOrder.PayableEntity;

        BudgetTransaction approvePaymentTxn = TryGetUnexercisedApprovePaymentBudgetTransaction(order);

        if (approvePaymentTxn == null) {
          continue;
        }

        ExcerciseBudget(paymentOrder, approvePaymentTxn);
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
                         .ToFixedList();
    }


    static private BudgetTransaction ExcerciseBudget(PaymentOrder paymentOrder,
                                                     BudgetTransaction paymentApproval) {

      var order = Order.Parse(paymentOrder.PayableEntity.UID);

      using (var usecase = ProcurementBudgetingUseCases.UseCaseInteractor()) {

        BudgetTransaction budgetTxn = usecase.CreateBudgetTransaction(order, BudgetOperationType.Exercise);

        budgetTxn.SetExerciseData(paymentOrder.LastPaymentInstruction.LastUpdateTime,
                                  paymentApproval,
                                  paymentOrder);

        budgetTxn.Close();

        budgetTxn.Save();

        UpdatePaymentApprovalBudgetTransaction(paymentApproval, paymentOrder);

        return budgetTxn;
      }
    }


    private BudgetTransaction TryGetUnexercisedApprovePaymentBudgetTransaction(Order order) {
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
