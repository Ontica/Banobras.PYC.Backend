/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Procurement Services                 Component : Use cases Layer                      *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Use case interactor class            *
*  Type     : OrderPaymentUseCases                          License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Use cases used to request procurement order payments.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.StateEnums;

using Empiria.Billing;

using Empiria.Orders;
using Empiria.Orders.Adapters;

using Empiria.Payments;

namespace Empiria.Banobras.Procurement.UseCases {

  /// <summary>Use cases used to integrate organization's procurement operations
  /// with the payments system.</summary>
  public class OrderPaymentUseCases : UseCase {

    #region Constructors and parsers

    protected OrderPaymentUseCases() {
      // no-op
    }

    static public OrderPaymentUseCases UseCaseInteractor() {
      return CreateInstance<OrderPaymentUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public PayableOrderHolderDto RequestPayment(string orderUID, PaymentOrderFields fields) {
      Assertion.Require(orderUID, nameof(orderUID));
      Assertion.Require(fields, nameof(fields));

      var order = PayableOrder.Parse(orderUID);

      var bills = Bill.GetListFor(order);

      Assertion.Require(bills.Count > 0, "No se han agregado los comprobantes.");

      var billsTotals = new BillsTotals(bills);

      Assertion.Require(billsTotals.Total > 0, "El importe total de los comprobantes debe ser mayor a cero.");

      if (order.Items.Count == 0 && order.Taxes.ControlConcepts.Count == 0) {
        Assertion.RequireFail("No se han cargado los conceptos.");
      }

      decimal orderTotals = order.Subtotal + order.Taxes.ControlConceptsTotal;
      decimal billed = billsTotals.Subtotal - billsTotals.Discounts + billsTotals.BudgetableTaxesTotal;

      if (order.Category.PlaysRole("travel-expenses")) {
        Assertion.Require(billed >= orderTotals,
                         "El importe de los comprobantes no puede ser menor al de los conceptos.");

      } else {
        Assertion.Require(orderTotals == billed,
                          $"El importe de los comprobantes ({billed:C2}) no coincide " +
                          $"con el importe de los conceptos ({orderTotals:C2}).");
      }

      var paymentType = PaymentType.Parse(fields.PaymentTypeUID);

      decimal paymentTotal = order.GetTotal();

      var paymentOrder = new PaymentOrder(paymentType, order.Provider, order, paymentTotal);

      var paymentMethod = paymentOrder.PaymentMethod;

      fields.PayableEntityTypeUID = order.GetEmpiriaType().UID;
      fields.PayableEntityUID = order.UID;
      fields.RequestedByUID = order.RequestedBy.UID;
      fields.Description = order.Name;
      fields.Observations = fields.Description;

      fields.PayToUID = order.Provider.UID;
      fields.CurrencyUID = order.Currency.UID;
      fields.Total = paymentTotal;

      paymentOrder.Update(fields);

      paymentOrder.Save();

      if (order.Status == EntityStatus.Pending) {
        order.Activate();
        order.Save();
      }

      return PayableOrderMapper.Map(order);
    }

    #endregion Use cases

  }  // class PaymentsProcurementUseCases

}  // namespace Empiria.Banobras.Procurement.UseCases
