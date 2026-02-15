/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Procurement Services                 Component : Domain Layer                         *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Service provider                     *
*  Type     : OrderBudgetTransactionBuilder                 License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a budget transaction from a products order.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Parties;
using Empiria.StateEnums;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

using Empiria.Orders;

namespace Empiria.Banobras.Procurement {

  /// <summary>Builds a budget transaction from a products order.</summary>
  internal class OrderBudgetTransactionBuilder {

    private readonly BudgetTransactionType _transactionType;
    private readonly Order _order;
    private readonly bool _updateOrderItems;

    private BudgetTransaction _transaction;

    public OrderBudgetTransactionBuilder(BudgetTransactionType transactionType,
                                         Order order,
                                         bool updateOrderItems) {

      Assertion.Require(transactionType, nameof(transactionType));
      Assertion.Require(order, nameof(order));
      Assertion.Require(order.GetItems<OrderItem>().Count > 0,
                        "Order has no items.");

      _transactionType = transactionType;
      _order = order;
      _updateOrderItems = updateOrderItems;
    }


    internal BudgetTransaction Build() {
      _transaction = BuildTransaction();

      BuildEntries();

      Assertion.Require(_transaction.Entries.Count > 0,
                        "No es posible generar la transacción presupuestal debido " +
                        "a que la orden de compra o requisición no cuenta " +
                        "con conceptos pendientes de autorizar.");

      return _transaction;
    }

    #region Helpers

    private void BuildDoubleEntries(OrderItem orderEntry,
                                    BalanceColumn withdrawalColumn,
                                    BalanceColumn depositColumn) {

      var fields = BuildEntryFields(orderEntry, depositColumn, true);

      BudgetEntry depositEntry = _transaction.AddEntry(fields);

      if (_updateOrderItems) {
        orderEntry.SetBudgetEntry(depositEntry);
      }

      fields = BuildEntryFields(orderEntry, withdrawalColumn, false);

      _transaction.AddEntry(fields);
    }


    private void BuildEntries() {

      FixedList<OrderItem> orderItems;

      if (_transaction.OperationType == BudgetOperationType.Request) {
        orderItems = _order.GetItems<OrderItem>()
                           .FindAll(x => x.BudgetEntry.IsEmptyInstance ||
                                         x.BudgetEntry.Status == TransactionStatus.Rejected);
      } else {
        orderItems = _order.GetItems<OrderItem>();
      }

      foreach (var item in orderItems) {

        if (_transaction.OperationType == BudgetOperationType.Request) {
          BuildDoubleEntries(item, BalanceColumn.Available, BalanceColumn.Requested);

        } else if (_transaction.OperationType == BudgetOperationType.Commit && !_order.HasCrossedBeneficiaries()) {
          BuildDoubleEntries(item, BalanceColumn.Requested, BalanceColumn.Commited);

        } else if (_transaction.OperationType == BudgetOperationType.Commit && _order.HasCrossedBeneficiaries()) {
          throw new NotImplementedException("Las transacciones de compromiso presupuestal " +
                                            "sobre requisiciones multiáreas no están disponibles.");

        } else if (_transaction.OperationType == BudgetOperationType.ApprovePayment) {
          BuildDoubleEntries(item, BalanceColumn.Commited, BalanceColumn.ToPay);

        } else if (_transaction.OperationType == BudgetOperationType.Exercise) {
          BuildDoubleEntries(item, BalanceColumn.ToPay, BalanceColumn.Exercised);

        } else {
          throw Assertion.EnsureNoReachThisCode($"Budget transaction entries rule is undefined: " +
                                                $"{_transaction.TransactionType.DisplayName}");
        }
      }
    }


    private BudgetEntryFields BuildEntryFields(OrderItem orderEntry,
                                               BalanceColumn balanceColumn,
                                               bool isDeposit) {

      DateTime budgetingDate = DetermineEntryBudgetingDate(orderEntry, balanceColumn, isDeposit);

      string relatedEntryUID = string.Empty;

      if (!orderEntry.RequisitionItem.IsEmptyInstance) {
        relatedEntryUID = orderEntry.RequisitionItem.BudgetEntry.UID;
      }

      return new BudgetEntryFields {
        BudgetUID = orderEntry.Budget.UID,
        BudgetAccountUID = orderEntry.BudgetAccount.UID,
        BalanceColumnUID = balanceColumn.UID,
        Description = orderEntry.Description,
        Justification = orderEntry.Justification,
        ProductUID = orderEntry.Product.UID,
        ProductCode = orderEntry.ProductCode,
        ProductName = orderEntry.ProductName,
        ProductUnitUID = orderEntry.ProductUnit.UID,
        OriginCountryUID = orderEntry.OriginCountry.UID,
        ProductQty = orderEntry.Quantity,
        ProjectUID = orderEntry.Project.UID,
        PartyUID = orderEntry.RequestedBy.UID,
        Year = budgetingDate.Year,
        Month = budgetingDate.Month,
        Day = budgetingDate.Day,
        EntityTypeId = orderEntry.GetEmpiriaType().Id,
        EntityId = orderEntry.Id,
        RelatedEntryUID = relatedEntryUID,
        ExchangeRate = _order.ExchangeRate,
        CurrencyUID = orderEntry.Currency.UID,
        CurrencyAmount = orderEntry.Subtotal,
        Amount = Math.Round((isDeposit ? orderEntry.Subtotal : -1 * orderEntry.Subtotal) * _order.ExchangeRate, 2)
      };
    }


    private BudgetTransaction BuildTransaction() {

      BudgetTransactionFields fields = BuildTransactionFields();

      var budget = Budget.Parse(fields.BaseBudgetUID);

      var transaction = new BudgetTransaction(_transactionType, budget, _order);

      transaction.Update(fields);

      return transaction;
    }


    private BudgetTransactionFields BuildTransactionFields() {

      OperationSource operationSource;

      if (_transactionType.OperationType == BudgetOperationType.ApprovePayment ||
          _transactionType.OperationType == BudgetOperationType.Exercise) {
        operationSource = OperationSource.ParseNamedKey("SISTEMA_DE_PAGOS");
      } else {
        operationSource = OperationSource.ParseNamedKey("SISTEMA_DE_ADQUISICIONES");
      }

      return new BudgetTransactionFields {
        TransactionTypeUID = _transactionType.UID,
        BaseEntityTypeUID = _order.GetEmpiriaType().UID,
        BaseEntityUID = _order.UID,
        Justification = _order.Justification,
        Description = _order.Description,
        BaseBudgetUID = _order.BaseBudget.UID,
        BasePartyUID = _order.RequestedBy.UID,
        CurrencyUID = _order.Currency.UID,
        ExchangeRate = _order.ExchangeRate,
        OperationSourceUID = operationSource.UID,
        RequestedByUID = Party.ParseWithContact(ExecutionServer.CurrentContact).UID,
        ApplicationDate = DateTime.Today
      };
    }

    private DateTime DetermineEntryBudgetingDate(OrderItem orderEntry,
                                                 BalanceColumn balanceColumn,
                                                 bool isDeposit) {

      switch (_transactionType.OperationType) {
        case BudgetOperationType.Request:

          return DetermineBudgetRequestDate(orderEntry);

        case BudgetOperationType.Commit:

          return DetermineBudgetCommitRequestDate(orderEntry, isDeposit);

        case BudgetOperationType.ApprovePayment:

          return DetermineApprovePaymentRequestDate(orderEntry, isDeposit);

        case BudgetOperationType.Exercise:

          return DetermineExcerciseRequestDate(orderEntry);

        default:
          throw Assertion.EnsureNoReachThisCode($"Budget entry budgeting date rule is undefined: " +
                                                $"{_transaction.TransactionType.DisplayName}");
      }
    }


    private DateTime DetermineApprovePaymentRequestDate(OrderItem orderEntry, bool isDeposit) {
      if (isDeposit) {
        return DateTime.Today.Date;
      }

      DateTime budgetRequisitionDate = !orderEntry.RequisitionItem.BudgetEntry.IsEmptyInstance ?
                                             orderEntry.RequisitionItem.BudgetEntry.Date : DateTime.Today;

      return new DateTime(budgetRequisitionDate.Year, budgetRequisitionDate.Month, 1);
    }


    private DateTime DetermineBudgetCommitRequestDate(OrderItem entry, bool isDeposit) {

      DateTime budgetRequisitionDate = !entry.RequisitionItem.BudgetEntry.IsEmptyInstance ?
                                                          entry.RequisitionItem.BudgetEntry.Date : DateTime.Today;

      if (isDeposit && budgetRequisitionDate.Year == DateTime.Today.Year) {
        return new DateTime(entry.Budget.Year, Math.Max(DateTime.Today.Month, budgetRequisitionDate.Month), 1);

      } else {
        return new DateTime(budgetRequisitionDate.Year, budgetRequisitionDate.Month, 1);
      }
    }


    private DateTime DetermineBudgetRequestDate(OrderItem entry) {
      DateTime entryStartDate = entry.Order.StartDate;

      if (entryStartDate.Year == entry.Budget.Year) {

        return new DateTime(entry.Budget.Year, entryStartDate.Month, 1);

      } else {

        return new DateTime(entry.Budget.Year, 1, 1);

      }
    }


    private DateTime DetermineExcerciseRequestDate(OrderItem entry) {

      DateTime budgetRequisitionDate = !entry.RequisitionItem.BudgetEntry.IsEmptyInstance ?
                                             entry.RequisitionItem.BudgetEntry.Date : DateTime.Today;

      return new DateTime(budgetRequisitionDate.Year, budgetRequisitionDate.Month, 1);
    }

    #endregion Helpers

  }  // class OrderBudgetTransactionBuilder

}  // namespace Empiria.Banobras.Procurement
