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
using Empiria.Services;

using Empiria.Orders;
using Empiria.Orders.Contracts;

using Empiria.Payments;

using Empiria.Budgeting.Transactions;

using Empiria.Banobras.Procurement;

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


    public BudgetTransaction CommitBudget(Order order, DateTime? applicationDate = null) {
      Assertion.Require(order, nameof(order));

      applicationDate = applicationDate ?? DateTime.Today.Date;

      Assertion.Require(order.Rules.CanCommitBudget(),
          $"No es posible solicitar el compromiso presupuestal para esta(e) {order.OrderType.DisplayName}, " +
          $"ya que su estado actual no permite ejecutar esta operación.");

      Assertion.Require(!order.Requisition.IsEmptyInstance && !(order is Requisition),
          $"Esta(e) {order.OrderType.DisplayName} no tiene asociada una requisición, por lo que " +
          $"no es posible solicitar el compromiso presupuestal.");

      Assertion.Require(order.HasBudgetableItems,
          $"Esta(e) {order.OrderType.DisplayName} no tiene conceptos asociados " +
          $"a partidas presupuestales. No es posible solicitar el compromiso presupuestal.");

      var bdgRequisitions = BudgetTransaction.GetFor(order.Requisition)
                                             .FindAll(x => x.OperationType == BudgetOperationType.Request &&
                                                           x.IsClosed);

      // ToDO: Per item
      if (bdgRequisitions.Count == 0) {
        Assertion.RequireFail($"Esta(e) {order.OrderType.DisplayName} no tiene una suficiencia presupuestal cerrada, " +
                              $"por lo que no es posible solicitar el compromiso presupuestal.");

      } else if (bdgRequisitions.Count > 1) {
        Assertion.RequireFail($"Se encontraron múltiples solicitudes presupuestales cerradas para la requisición " +
                              $"asociada a esta(e) {order.OrderType.DisplayName}. No es posible solicitar el compromiso presupuestal.");
      }

      var builder = new BudgetTransactionBuilder(order, CommonData.SISTEMA_DE_ADQUISICIONES,
                                                 applicationDate.Value);

      // Allow multiple requests but only one commit
      BudgetTransaction commitTxn = builder.Build(BudgetOperationType.Commit, bdgRequisitions[0]);

      order.Activate();

      order.Save();

      SendBudgetTransaction(commitTxn, order);

      return commitTxn;
    }


    public BudgetTransaction ExerciseBudget(PaymentOrder paymentOrder,
                                            BudgetTransaction paymentApproval,
                                            DateTime? exerciseDate = null) {

      Assertion.Require(paymentOrder, nameof(paymentOrder));
      Assertion.Require(paymentApproval, nameof(paymentApproval));

      exerciseDate = exerciseDate ?? DateTime.Today.Date;


      var builder = new BudgetTransactionBuilder((IBudgetable) paymentOrder.PayableEntity,
                                                 CommonData.SISTEMA_DE_CONTROL_PRESUPUESTAL,
                                                 exerciseDate.Value,
                                                 paymentOrder.ExchangeRate);

      BudgetTransaction exerciseTxn = builder.Build(BudgetOperationType.Exercise, paymentApproval);

      exerciseTxn.SetExerciseData(paymentOrder, CommonData.GERENCIA_DE_PAGOS);

      exerciseTxn.Close();

      exerciseTxn.Save();

      return exerciseTxn;
    }


    public BudgetTransaction RequestBudget(Order order) {
      Assertion.Require(order.Rules.CanRequestBudget(),
           $"No es posible solicitar la suficiencia presupuestal para esta requisición, " +
           $"ya que su estado actual no permite ejecutar esta operación.");

      Assertion.Require(order.Requisition.IsEmptyInstance && order is Requisition,
          $"Esta requisición tiene información incorrecta. " +
          $"No es posible solicitar la suficiencia presupuestal.");

      Assertion.Require(order.HasBudgetableItems,
          "Esta requisición no tiene conceptos asociados a partidas presupuestales. " +
          "No es posible solicitar la suficiencia presupuestal.");

      var validator = new OrderBudgetTransactionValidator(order);

      validator.EnsureOrderHasAvailableBudget();

      var builder = new BudgetTransactionBuilder(order, CommonData.SISTEMA_DE_ADQUISICIONES, DateTime.Today);

      BudgetTransaction requestTxn = builder.Build(BudgetOperationType.Request);

      order.Activate();

      order.Save();

      SendBudgetTransaction(requestTxn, order);

      return requestTxn;
    }

    #endregion Application services

    #region Helpers

    private void SendBudgetTransaction(BudgetTransaction budgetTxn, Order order) {

      budgetTxn.SendToAuthorization();

      budgetTxn.Save();

      var orderItems = order.GetItems<OrderItem>();

      foreach (var entry in budgetTxn.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment)) {
        var orderItem = orderItems.Find(x => x.Id == entry.EntityId && x.GetEmpiriaType().Id == entry.EntityTypeId);

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
