/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Procurement Services                 Component : Use cases Layer                      *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Use case interactor class            *
*  Type     : ProcurementBudgetingUseCases                  License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to integrate organization's procurement operations with the budgeting system.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Parties;

using Empiria.Orders;
using Empiria.Orders.Data;

using Empiria.Payments;
using Empiria.Payments.Adapters;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Banobras.Procurement.Adapters;

namespace Empiria.Banobras.Procurement.UseCases {

  /// <summary>Use cases used to integrate organization's procurement operations
  /// with the budgeting system.</summary>
  public class ProcurementBudgetingUseCases : UseCase {

    #region Constructors and parsers

    protected ProcurementBudgetingUseCases() {
      // no-op
    }

    static public ProcurementBudgetingUseCases UseCaseInteractor() {
      return CreateInstance<ProcurementBudgetingUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public BudgetTransactionDescriptorDto ApprovePayment(BudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var paymentOrder = PaymentOrder.Parse(fields.BaseObjectUID);

      var order = Order.Parse(paymentOrder.PayableEntity.UID);

      Assertion.Require(paymentOrder.Rules.CanApproveBudget(),
        $"No es posible solicitar la autorización presupuestal de pago para esta solicitud de pago, " +
        $"ya que su estado actual no permite ejecutar esta operación.");


      Assertion.Require(order.HasBudgetableItems,
        $"La(el) {order.OrderType.DisplayName} asociada(o) a esta solicitud de pago no tiene conceptos " +
        $"ligados a partidas presupuestales. No es posible solicitar la autorización presupuestal de pago.");

      order.Activate();

      order.Save();

      BudgetTransaction budgetTxn = CreateBudgetTransaction(order, BudgetOperationType.ApprovePayment);

      SendBudgetTransaction(budgetTxn, order);

      return BudgetTransactionMapper.MapToDescriptor(budgetTxn);
    }


    public BudgetTransactionDescriptorDto CommitBudget(BudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Order order = Order.Parse(fields.BaseObjectUID);

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

      Assertion.Require(bdgRequisitions.Count > 0,
        $"Esta(e) {order.OrderType.DisplayName} no tiene una suficiencia presupuestal cerrada, " +
        $"por lo que no es posible solicitar el compromiso presupuestal.");

      order.Activate();

      order.Save();

      BudgetTransaction budgetTxn = CreateBudgetTransaction(order, BudgetOperationType.Commit);

      SendBudgetTransaction(budgetTxn, order);

      return BudgetTransactionMapper.MapToDescriptor(budgetTxn);
    }


    public BudgetTransactionDescriptorDto RequestBudget(BudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Order order = Order.Parse(fields.BaseObjectUID);

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

      order.Activate();

      order.Save();

      BudgetTransaction budgetTxn = CreateBudgetTransaction(order, BudgetOperationType.Request);

      SendBudgetTransaction(budgetTxn, order);

      return BudgetTransactionMapper.MapToDescriptor(budgetTxn);
    }


    public FixedList<PayableEntityDto> SearchRelatedDocumentsForTransactionEdition(RelatedDocumentsQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = new Filter();

      var orgUnit = OrganizationalUnit.Parse(query.OrganizationalUnitUID);

      filter.AppendAnd($"ORDER_REQUESTED_BY_ID = {orgUnit.Id}");
      filter.AppendAnd($"ORDER_STATUS <> 'X'");

      if (query.Keywords.Length != 0) {
        filter.AppendAnd(SearchExpression.ParseAndLikeKeywords("ORDER_KEYWORDS", query.Keywords));
      }

      FixedList<Order> orders = OrdersData.Search<Order>(filter.ToString(), "ORDER_NO");

      return orders.Select(x => MapToPayableEntity(x))
                   .ToFixedList();
    }


    public BudgetValidationResultDto ValidateBudget(BudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      Order order = Order.Parse(fields.BaseObjectUID);

      var validator = new OrderBudgetTransactionValidator(order);

      try {
        validator.EnsureOrderHasAvailableBudget();

      } catch (AssertionFailsException ex) {
        return new BudgetValidationResultDto {
          Result = ex.Message
        };
      }

      return new BudgetValidationResultDto {
        Result = "Hay presupuesto disponible para todas las partidas de la requisición."
      };
    }

    #endregion Use cases

    #region Helpers

    internal BudgetTransaction CreateBudgetTransaction(Order order, BudgetOperationType operationType) {

      var bdgTxnType = BudgetTransactionType.GetFor(order.BudgetType, operationType);

      bool updateOrderItems = bdgTxnType.OperationType != BudgetOperationType.Exercise;

      var builder = new OrderBudgetTransactionBuilder(bdgTxnType, order, updateOrderItems);

      BudgetTransaction transaction = builder.Build();

      return transaction;
    }


    static private PayableEntityDto MapToPayableEntity(Order order) {
      return new PayableEntityDto {
        UID = order.UID.ToString(),
        Type = order.OrderType.MapToNamedEntity(),
        EntityNo = order.OrderNo,
        Name = order.Name,
        Description = order.Description,
        Budget = order.BaseBudget.MapToNamedEntity(),
        Currency = order.Currency.MapToNamedEntity(),
        Total = order.Total
      };
    }


    internal BudgetTransaction SendBudgetTransaction(BudgetTransaction transaction, Order order) {

      Assertion.Require(transaction.OperationType != BudgetOperationType.Exercise,
        "Cannot send a budget transaction of type 'Exercise'.");

      transaction.SendToAuthorization();

      transaction.Save();

      foreach (var item in order.GetItems<OrderItem>()
                                .FindAll(x => x.IsDirty)) {
        item.Save();
      }

      return transaction;
    }


    #endregion Helpers

  }  // class ProcurementBudgetingUseCases

}  // namespace Empiria.Banobras.Procurement.UseCases
