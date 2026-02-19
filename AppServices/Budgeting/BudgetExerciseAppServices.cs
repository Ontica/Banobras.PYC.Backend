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
using Empiria.Parties;
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

    public int ExerciseBudget() {

      const int BATCH_SIZE = 2;

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

        ExerciseBudget(paymentOrder, approvePaymentTxn);
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


    static private BudgetTransaction ExerciseBudget(PaymentOrder paymentOrder,
                                                    BudgetTransaction paymentApproval) {

      var GERENCIA_DE_PAGOS = Party.Parse(145);
      var SISTEMA_DE_PAGOS = OperationSource.ParseNamedKey("SISTEMA_DE_PAGOS");

      var exerciseDate = paymentOrder.LastPaymentInstruction.LastUpdateTime;

      var builder = new BudgetTransactionBuilder(paymentApproval,
                                                 BudgetOperationType.Exercise,
                                                 SISTEMA_DE_PAGOS,
                                                 exerciseDate);

      BudgetTransaction exerciseTxn = builder.Build();

      exerciseTxn.SetExerciseData(paymentOrder, GERENCIA_DE_PAGOS);

      exerciseTxn.Close();

      exerciseTxn.Save();

      var order = Order.Parse(paymentOrder.PayableEntity.UID);

      UpdatePaymentApprovalBudgetTransaction(paymentApproval, paymentOrder);

      return exerciseTxn;
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
