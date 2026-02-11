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

      if (ExecutionServer.CurrentUserId != 1002) {
        Assertion.RequireFail("Esta funcionalidad está en proceso de desarrollo y pruebas, " +
                              "por lo que todavía no está disponible en producción.");
      }

      var paymentOrders = PaymentOrder.GetList<PaymentOrder>()
                                      .FindAll(x => x.Status == PaymentOrderStatus.Payed &&
                                                    x.PayableEntity.Budget.UID != BudgetType.None.UID);

      int counter = 0;

      foreach (var paymentOrder in paymentOrders) {
        if (counter >= 5) {
          break;
        }


        Order order = (Order) paymentOrder.PayableEntity;

        if (order.IsForMultipleBeneficiaries) {
          continue;
        }

        var txns = BudgetTransaction.GetFor(order);

        if (txns.Contains(x => x.OperationType == BudgetOperationType.Exercise)) {
          continue;
        }


        var txn = txns.FindLast(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                     x.Status == TransactionStatus.Closed);
        if (txn != null) {
          ExcerciseBudget(paymentOrder, txn);
          counter++;
          continue;
        }
      }

      return counter;
    }

    #endregion Application services

    #region Helpers

    private BudgetTransaction ExcerciseBudget(PaymentOrder paymentOrder, BudgetTransaction paymentApproval) {

      var order = Order.Parse(paymentOrder.PayableEntity.UID);

      using (var usecase = ProcurementBudgetingUseCases.UseCaseInteractor()) {
        BudgetTransaction budgetTxn = usecase.CreateAnSendBudgetTransaction(order, BudgetOperationType.Exercise);

        budgetTxn.Authorize();

        budgetTxn.Save();

        budgetTxn.SetExerciseData(paymentOrder.LastPaymentInstruction.PostingTime, paymentApproval);

        budgetTxn.Close();

        budgetTxn.Save();


        return budgetTxn;
      }
    }

    #endregion Helpers

  }  // class BudgetExerciseAppServices

}  // namespace Empiria.Banobras.Budgeting.AppServices
