/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Management                 Component : Application services Layer           *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Application service                  *
*  Type     : BudgetExecutionAppServices                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Application services for generate budget execution transactions.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Payments;
using Empiria.Services;

using Empiria.Orders;
using Empiria.Orders.Contracts;

using Empiria.Budgeting.Transactions;

namespace Empiria.Banobras.Budgeting.AppServices {

  /// <summary>Application services for generate budget execution transactions.</summary>
  public class BudgetExecutionAppServices : UseCase {

    #region Constructors and parsers

    protected BudgetExecutionAppServices() {

    }

    static public BudgetExecutionAppServices UseCaseInteractor() {
      return CreateInstance<BudgetExecutionAppServices>();
    }

    #endregion Constructors and parsers

    #region Application services

    public BudgetTransaction ApprovePayment(PaymentOrder paymentOrder,
                                            DateTime? applicationDate = null) {
      Assertion.Require(paymentOrder, nameof(paymentOrder));

      applicationDate = applicationDate ?? DateTime.Today.Date;

      var order = Order.Parse(paymentOrder.PayableEntity.UID);

      Assertion.Require(paymentOrder.Rules.CanApproveBudget(),
        $"No es posible solicitar la autorización presupuestal de pago para esta solicitud de pago, " +
        $"ya que su estado actual no permite ejecutar esta operación.");


      Assertion.Require(order.HasBudgetableItems,
        $"La(el) {order.OrderType.DisplayName} asociada(o) a esta solicitud de pago no tiene conceptos " +
        $"ligados a partidas presupuestales. No es posible solicitar la autorización presupuestal de pago.");

      BudgetTransaction commitTransaction = TryGetBaseTransaction(BudgetOperationType.ApprovePayment, order);

      Assertion.Require(commitTransaction,
        $"No es posible solicitar la autorización presupuestal de pago debido a " +
        $"que no se encontró el compromiso presupuestal asociado a la misma.");

      if (paymentOrder.Currency.Distinct(Currency.Default) && paymentOrder.ExchangeRate == decimal.One) {
        Assertion.RequireFail($"La orden de pago es en {paymentOrder.Currency.Name}. " +
                              $"Se requiere se proporcione el tipo de cambio para " +
                              "poder solicitar la autorización presupuestal.");
      }


      var builder = new BudgetTransactionBuilder(order, CommonData.SISTEMA_DE_PAGOS,
                                                 applicationDate.Value, paymentOrder.ExchangeRate);

      BudgetTransaction approvePaymentTxn = builder.Build(BudgetOperationType.ApprovePayment, commitTransaction);

      order.Activate();

      order.Save();

      approvePaymentTxn.SetPayable(paymentOrder);

      SendBudgetTransaction(approvePaymentTxn, order);

      return approvePaymentTxn;
    }


    #endregion Application services

    #region Helpers

    private void SendBudgetTransaction(BudgetTransaction budgetTxn, Order order) {

      budgetTxn.SendToAuthorization();

      budgetTxn.Save();

      var orderItems = order.GetItems<OrderItem>();

      foreach (var entry in budgetTxn.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment)) {
        var orderItem = orderItems.Find(x => x.Id == entry.Id && x.GetEmpiriaType().Id == entry.EntityTypeId);

        orderItem.SetBudgetEntry(entry);

        orderItem.Save();
      }
    }


    private BudgetTransaction TryGetBaseTransaction(BudgetOperationType budgetOperation, Order order) {

      FixedList<BudgetTransaction> txns;

      if (order is ContractOrder) {
        txns = BudgetTransaction.GetFor(order.Contract);
        txns = FixedList<BudgetTransaction>.MergeDistinct(txns, BudgetTransaction.GetFor(order));
      } else {
        txns = BudgetTransaction.GetFor(order);
      }

      txns = txns.FindAll(x => x.IsClosed);

      switch (budgetOperation) {
        case BudgetOperationType.ApprovePayment:
          txns = txns.FindAll(x => x.OperationType == BudgetOperationType.Commit &&
                                   (order is ContractOrder && x.GetEntity().Equals(order.Contract) ||
                                   !(order is ContractOrder) && x.GetEntity().Equals(order)));
          break;
        default:
          throw Assertion.EnsureNoReachThisCode($"Unhandled budget transaction operation {budgetOperation}.");
      }

      if (txns.Count == 1) {
        return txns[0];
      } else if (txns.Count > 1) {
        Assertion.RequireFail("Se encontraron múltiples transacciones presupuestales base.");
      }

      return null;
    }

    #endregion Helpers

  }  // class BudgetExecutionAppServices

}  // namespace Empiria.Banobras.Budgeting.AppServices
