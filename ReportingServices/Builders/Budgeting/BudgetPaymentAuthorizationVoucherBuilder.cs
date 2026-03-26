/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                           Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Html builder                         *
*  Type     : BudgetPaymentAuthorizationVoucherBuilder      License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a PDF file with a budget payment authorization voucher.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Text;

using Empiria.Financial;
using Empiria.Office;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Billing;

using Empiria.Orders;
using Empiria.Orders.Contracts;

using Empiria.Payments;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Builds a PDF file with a budget payment authorization voucher.</summary>
  internal class BudgetPaymentAuthorizationVoucherBuilder : HtmlBuilder {

    private readonly BudgetTransaction _budgetTxn;
    private readonly PaymentOrder _paymentOrder;
    private readonly Order _order;
    private readonly FixedList<Bill> _bills;

    private decimal _orderDiscounts = 0;
    private decimal _orderPenalties = 0;

    public BudgetPaymentAuthorizationVoucherBuilder(BudgetTransaction budgetTxn,
                                                    FileTemplateConfig templateConfig) : base(templateConfig) {
      Assertion.Require(budgetTxn, nameof(budgetTxn));

      var paymentOrder = PaymentOrder.Parse(budgetTxn.PayableId);

      _budgetTxn = budgetTxn;
      _paymentOrder = paymentOrder;
      _order = _paymentOrder.PayableEntity as Order;

      _bills = Bill.GetListFor(_paymentOrder.PayableEntity);
    }

    #region Builders

    protected override void Build() {

      BuildHeader();
      BuildBudgetEntries();
      BuildBudgetTotals();
      BuildBills();
      BuildBillsTotals();
      BuildFooter();
      HideUnneededParts();
      PatchText();

      SignIf(_budgetTxn.IsClosed);
    }


    private void BuildBills() {
      string TEMPLATE = GetSection("BILLS.TEMPLATE");

      var entriesHtml = new StringBuilder();

      foreach (var bill in _bills) {
        var entryHtml = new StringBuilder(TEMPLATE);

        entryHtml.Replace("{{BILL_NO}}", BuildBillNumber(bill));
        entryHtml.Replace("{{BILL_TYPE}}", BuildBillType(bill));
        entryHtml.Replace("{{DESCRIPTION}}", bill.Name);
        entryHtml.Replace("{{ISSUE_DATE}}", bill.IssueDate.ToString("dd/MMM/yyyy"));
        entryHtml.Replace("{{POSTING_DATE}}", bill.PostingTime.ToString("dd/MMM/yyyy"));
        entryHtml.Replace("{{SUBTOTAL}}", bill.Subtotal.ToString("C2"));
        entryHtml.Replace("{{DISCOUNT}}", bill.Discount.ToString("C2"));
        entryHtml.Replace("{{TOTAL}}", (bill.Subtotal - bill.Discount).ToString("C2"));

        entriesHtml.Append(entryHtml);
      }

      SetSection("BILLS.TEMPLATE", entriesHtml.ToString());
    }


    private void BuildBudgetEntries() {

      if (_order.BudgetType == BudgetType.None) {

        Set("{{BUDGET_ENTRIES_TABLE_TITLE}}", "ESTE PAGO NO ESTÁ RELACIONADO CON EL CONTROL PRESUPUESTAL");
        Hide("{{CLASS_HIDE_BUDGET_CONCEPTS}}");

        return;

      } else if (_budgetTxn.IsEmptyInstance) {

        SetWarning("{{BUDGET_ENTRIES_TABLE_TITLE}}", "ESTE PAGO AÚN NO CUENTA CON APROBACIÓN PRESUPUESTAL");
        Hide("{{CLASS_HIDE_BUDGET_CONCEPTS}}");

        return;

      }

      SetIf("{{BUDGET_ENTRIES_TABLE_TITLE}}", _budgetTxn.IsClosed,
                                              $"CONTROL PRESUPUESTAL",
                                              Warning("LA APROBACIÓN PRESUPUESTAL ESTÁ EN PROCESO"));

      Remove("{{CLASS_HIDE_BUDGET_CONCEPTS}}");

      string TEMPLATE = base.GetSection("TRANSACTION_ENTRY.TEMPLATE");

      var entriesHtml = new StringBuilder();

      foreach (var budgetEntry in _budgetTxn.Entries.FindAll(x => x.Deposit > 0 && x.NotAdjustment)) {

        var orderItem = OrderItem.Parse(budgetEntry.EntityId);

        var entryHtml = new StringBuilder(TEMPLATE);

        entryHtml.Replace("{{BUDGET_ACCOUNT_CODE}}", budgetEntry.BudgetAccount.Code);
        entryHtml.Replace("{{BUDGET_ACCOUNT_PARTY}}", budgetEntry.BudgetAccount.OrganizationalUnit.Code);

        entryHtml.Replace("{{DESCRIPTION}}", budgetEntry.ProductName);
        entryHtml.Replace("{{ORIGIN_COUNTRY}}", budgetEntry.OriginCountry.CountryISOCode);
        entryHtml.Replace("{{CONTROL_NO}}", budgetEntry.ControlNo);
        entryHtml.Replace("{{PROGRAM}}", budgetEntry.BudgetProgram.Code);
        entryHtml.Replace("{{PRODUCT_CODE}}", budgetEntry.ProductCode);
        entryHtml.Replace("{{PRODUCT_UNIT}}", budgetEntry.ProductUnit.Name);
        entryHtml.Replace("{{SUBTOTAL}}", (budgetEntry.CurrencyAmount - orderItem.DiscountsTotal).ToString("C2"));
        entryHtml.Replace("{{DISCOUNT}}", orderItem.Discount.ToString("C2"));
        entryHtml.Replace("{{PENALTIES}}", orderItem.PenaltyDiscount.ToString("C2"));
        entryHtml.Replace("{{TOTAL}}", budgetEntry.Amount.ToString("C2"));

        _orderDiscounts += orderItem.Discount;
        _orderPenalties += orderItem.PenaltyDiscount;

        entriesHtml.Append(entryHtml);
      }

      SetSection("TRANSACTION_ENTRY.TEMPLATE", entriesHtml.ToString());
    }


    private void BuildFooter() {
      HideIf("{{CLASS_HIDE_SIGN}}", _paymentOrder.Total <= 4000000m);
    }


    private void BuildHeader() {

      string title = _budgetTxn.OperationType == BudgetOperationType.Exercise ? "Ejercicio presupuestal" : "Relación de pagos";

      if (!_budgetTxn.IsClosed) {
        title = $"{title}{Space(2)}({_budgetTxn.Status.GetName()})";
      }

      SetIf("{{REPORT.TITLE}}", _budgetTxn.IsClosed, title, Warning(title));

      Set("{{TRANSACTION.TRANSACTION_NO}}", _budgetTxn.TransactionNo);
      Set("{{TRANSACTION.REQUESTED_BY}}", _budgetTxn.RequestedBy.Name);
      Set("{{TRANSACTION.REQUESTED_TIME}}", _budgetTxn.RequestedDate.ToString("dd/MMM/yyyy HH:mm"));

      Set("{{BUDGET_NAME}}", _budgetTxn.BaseBudget.Name.ToUpperInvariant());
      Set("{{BUDGET.TOTAL}}", _budgetTxn.GetTotal());

      Set("{{ORDER.ORDER_NO}}", _order.OrderNo);
      Set("{{ORDER.NAME}}", _order.Name);
      Set("{{ORDER.TYPE}}", _order.OrderType.DisplayName);
      Set("{{ORDER.CATEGORY}}", BuildOrderCategory());
      Set("{{ORDER.REQUESTED_BY}}", _paymentOrder.PostedBy.Name);

      Set("{{PAYMENT_ORDER.NO}}", _paymentOrder.PaymentOrderNo);
      Set("{{PAYMENT_ORDER.TOTAL}}", _paymentOrder.Total);

      Set("{{PAYMENT_ORDER.PAYMENT_TYPE}}", _paymentOrder.PaymentType.Name);

      Set("{{PAYMENT_ORDER.REQUEST_TIME}}", _paymentOrder.PostingTime.ToString("dd/MMM/yyyy HH:mm"));
      Set("{{PAYMENT_ORDER.REQUESTED_BY}}", $"{_paymentOrder.RequestedBy.Name} ({_paymentOrder.RequestedBy.Code}) ");


      var payTo = (Payee) _paymentOrder.PayTo;

      Set("{{PAYMENT.PAY_TO}}", $"{payTo.Name}{Space(2)}( {payTo.SubledgerAccount} )");

      if (_budgetTxn.OperationType == BudgetOperationType.Exercise && _paymentOrder.Payed) {
        Set("{{PAYMENT.PAYMENT_METHOD}}", $"{_paymentOrder.PaymentMethod.Name}{Space(2)}" +
                                          $"{_paymentOrder.LastPaymentInstruction.LastUpdateTime.ToString("dd/MMM/yyyy HH:mm")}");
      } else {
        Set("{{PAYMENT.PAYMENT_METHOD}}", $"{_paymentOrder.PaymentMethod.Name}");
      }

      Set("{{PAYMENT.BUDGET}}", _paymentOrder.PayableEntity.Budget.Name);
      Set("{{PAYMENT.DESCRIPTION}}", _paymentOrder.Description);

      Set("{{PAYMENT.TOTAL}}", _paymentOrder.Total);
      Set("{{PAYMENT.CURRENCY}}", _paymentOrder.Currency.ISOCode);

      if (!_paymentOrder.PaymentAccount.IsEmptyInstance) {
        Set("{{PAYMENT.INSTITUTION}}", _paymentOrder.PaymentAccount.Institution.Name);
        Set("{{PAYMENT.ACCOUNT_NO}}", $"{Space(4)} {Normal("No:")} {_paymentOrder.PaymentAccount.AccountNo}");
        SetIf("{{PAYMENT.REFERENCE_NUMBER}}", _paymentOrder.ReferenceNumber.Length != 0,
                                            $"{Space(2)} {Normal("Referencia:")} {_paymentOrder.ReferenceNumber}");
      } else {
        Set("{{PAYMENT.INSTITUTION}}", "No aplica");
        Remove("{{PAYMENT.ACCOUNT_NO}}");
        Remove("{{PAYMENT.REFERENCE_NUMBER}}");
      }

      Set("{{REQUISITION.NO}}", _order.Requisition.OrderNo);
      Set("{{REQUISITION.TOTAL}}", _order.Requisition.Total);

      Set("{{CONTRACT_DATA}}", BuildContractData());

      if (!_paymentOrder.Debtor.IsEmptyInstance) {
        var debtor = (Payee) _paymentOrder.Debtor;

        Set("{{PAYMENT.DEBTOR}}", $"{debtor.Name} ({debtor.SubledgerAccount})");
      } else {
        Set("{{PAYMENT.DEBTOR}}", "No aplica");
      }

      SetIf("{{CURRENCY_CODE}}", _budgetTxn.Currency.Equals(Currency.Default), string.Empty, _budgetTxn.Currency.ISOCode);
      Set("{{EXCHANGE_RATE}}", _budgetTxn.ExchangeRate.ToString("C4"));

      BuildBudgetRequests();

      Set("{{JUSTIFICATION}}", EmpiriaString.FirstWithValue(_budgetTxn.Justification, _order.Justification,
                                                            _order.Requisition.Justification));
      Set("{{STATUS}}", _budgetTxn.Status.GetName());
      SetWarning("{{REJECTED_REASON}}", _budgetTxn.RejectedReason);
    }


    private void BuildBillsTotals() {

      var billsTotals = new BillsTotals(_bills);

      Set("{{BILLS.SUBTOTAL}}", billsTotals.Subtotal.ToString("C2"));

      string TEMPLATE = base.GetSection("TAXES.TEMPLATE");

      var totalsHtml = new StringBuilder();

      foreach (var taxItem in billsTotals.TaxItems) {

        var totalHtml = TEMPLATE.Replace("{{TAX_TYPE}}", taxItem.TaxName);
        totalHtml = totalHtml.Replace("{{TAX_TOTAL}}", taxItem.Total.ToString("C2"));

        totalsHtml.Append(totalHtml);
      }

      SetSection("TAXES.TEMPLATE", totalsHtml.ToString());

      Set("{{BILLS.TOTAL}}", billsTotals.Total);
    }

    private void BuildBudgetTotals() {
      Set("{{BUDGET.SUBTOTAL}}", _order.Subtotal + _order.Items.DiscountsTotal);
      Set("{{BUDGET.DISCOUNT_TOTAL}}", _order.Items.Discount);
      Set("{{BUDGET.PENALTIES_TOTAL}}", _order.Items.Penalties);
      Set("{{BUDGET.TOTAL}}", _order.Subtotal);
    }

    #endregion Builders

    #region Helpers

    private string BuildBillNumber(Bill bill) {
      string serial = string.Empty;

      if (bill.SchemaData.Serie.Length != 0) {
        serial = Normal("Serie: ") + bill.SchemaData.Serie;
      }
      if (bill.SchemaData.Folio.Length != 0) {
        if (serial.Length != 0) {
          serial += Space(2);
        }
        serial += Normal("Folio: ") + bill.SchemaData.Folio;
      }

      if (serial.Length != 0) {
        serial = $"<br />{serial}";
      }

      return $"{bill.BillNo}{serial}";
    }


    private string BuildBillType(Bill bill) {
      string billType = bill.BillCategory.Name;

      if (bill.BillCategory.IsCFDI && bill.PaymentMethod.Length != 0) {
        billType += $"<br/>{Strong(bill.PaymentMethod)}";
      }

      return billType;
    }


    private void BuildBudgetRequests() {
      if (_budgetTxn.IsEmptyInstance) {
        Set("{{PAYMENT.BUDGET_REQUESTS}}", "No aplica");
      }

      var bdgRequests = BudgetTransaction.GetRelatedTo(_budgetTxn)
                                         .FindAll(x => x.OperationType == BudgetOperationType.Request && x.IsClosed)
                                         .Select(x => x.TransactionNo)
                                         .ToFixedList();

      Set("{{PAYMENT.BUDGET_REQUESTS}}", string.Join(", ", bdgRequests));

      var bdgCommits = BudgetTransaction.GetFor(_order is ContractOrder ? _order.Contract : _order)
                                        .FindAll(x => x.OperationType == BudgetOperationType.Commit && x.IsClosed)
                                        .Select(x => x.TransactionNo)
                                        .ToFixedList();

      Set("{{PAYMENT.BUDGET_COMMIT}}", string.Join(", ", bdgCommits));
    }


    private string BuildContractData() {
      if (_order.Contract.IsEmptyInstance) {
        return "No aplica";
      }

      var contract = _order.Contract;

      return $"{contract.OrderNo}{Space(3)}" +
             $"Vigencia: {contract.StartDate:dd/MMM/yyyy} - {contract.EndDate:dd/MMM/yyyy}{Space(3)}" +
             $"Importe: {contract.Total:C2}";
    }

    private string BuildOrderCategory() {
      if (!_order.Category.IsEmptyInstance) {
        return _order.Category.Name;
      } else if (!_order.Contract.IsEmptyInstance) {
        return _order.Contract.Category.Name;
      } else {
        return _order.Requisition.Category.Name;
      }
    }


    private void HideUnneededParts() {

      if (_orderDiscounts == 0) {
        Hide("{{HIDE_DISCOUNT_COLUMN}}");
      }

      if (_orderPenalties == 0) {
        Hide("{{HIDE_PENALTIES_COLUMN}}");
      }

      if (_budgetTxn.Currency.Equals(Currency.Default)) {
        Hide("{{HIDE_EXCHANGE_RATE}}");
      }

      if (_orderDiscounts == 0 && _orderPenalties == 0 &&
          _budgetTxn.Currency.Equals(Currency.Default)) {
        Hide("{{HIDE_SUBTOTAL_COLUMN}}");
      }

      if (!_budgetTxn.InProcess && !_budgetTxn.IsClosed) {
        Hide("{{HIDE_ALL_SIGNS}}");
      }

      if (_budgetTxn.OperationType == BudgetOperationType.Exercise) {
        Hide("{{HIDE_ALL_SIGNS}}");
      }
    }


    private void PatchText() {
      if (_budgetTxn.OperationType != BudgetOperationType.Exercise) {
        return;
      }

      Set("Pagar a:", "Pagado a:");
      Set("TOTAL A PAGAR", "TOTAL PAGADO");
    }

    #endregion Helpers

  }  // class BudgetPaymentAuthorizationVoucherBuilder

}  // namespace Empiria.Budgeting.Reporting
