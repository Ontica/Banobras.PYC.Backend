/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                           Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : PaymentsBillsToExcelBuilder                   License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with payment orders and its associated bills.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Billing;
using Empiria.Office;
using Empiria.Storage;

namespace Empiria.Payments.Reporting {

  /// <summary>Builds an Excel file with payment orders and its associated bills.</summary>
  internal class PaymentsBillsToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public PaymentsBillsToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(DateTime fromDate, DateTime toDate) {

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(fromDate, toDate);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(DateTime fromDate, DateTime toDate) {

      var paymentOrders = GetPaymentOrders(fromDate, toDate);

      int i = _templateConfig.FirstRowIndex;

      foreach (var po in paymentOrders) {

        var bills = Bill.GetListFor(po.PayableEntity);

        foreach (var bill in bills) {

          _excelFile.SetCell($"A{i}", po.PaymentOrderNo);
          _excelFile.SetCell($"B{i}", po.RequestedBy.Code);
          _excelFile.SetCell($"C{i}", po.RequestedBy.Name);
          _excelFile.SetCell($"E{i}", po.PaymentType.Name);
          _excelFile.SetCell($"D{i}", po.PaymentMethod.Name);
          _excelFile.SetCell($"F{i}", po.PayTo.Name);
          _excelFile.SetCell($"G{i}", po.PayableEntity.EntityNo);
          _excelFile.SetCell($"H{i}", po.PayableEntity.GetEmpiriaType().DisplayName);
          _excelFile.SetCell($"I{i}", po.PayableEntity.Name);
          _excelFile.SetCell($"J{i}", po.Currency.ISOCode);
          _excelFile.SetCell($"K{i}", po.Total);
          _excelFile.SetCell($"L{i}", po.PaymentAccount.Institution.Name);
          _excelFile.SetCell($"M{i}", po.PaymentAccount.AccountNo);
          _excelFile.SetCell($"N{i}", po.ReferenceNumber);
          _excelFile.SetCell($"O{i}", po.Debtor.Name);
          _excelFile.SetCell($"P{i}", po.PostedBy.Name);
          _excelFile.SetCell($"Q{i}", po.PostingTime.ToString("dd/MMM/yyyy HH:mm"));
          _excelFile.SetCell($"R{i}", po.Status.GetName());

          if (po.Payed) {
            _excelFile.SetCell($"S{i}", po.LastPaymentInstruction.LastUpdateTime.ToString("dd/MMM/yyyy HH:mm"));
            _excelFile.SetCell($"T{i}", po.LastPaymentInstruction.BrokerInstructionNo);
          }
          _excelFile.SetCell($"U{i}", po.PayableEntity.Budget.Name);
          _excelFile.SetCell($"V{i}", bill.BillCategory.Name);
          _excelFile.SetCell($"W{i}", bill.BillNo);
          _excelFile.SetCell($"X{i}", bill.Name);
          _excelFile.SetCell($"Y{i}", bill.Subtotal);
          _excelFile.SetCell($"Z{i}", bill.Discount);
          _excelFile.SetCell($"AA{i}", bill.Taxes);
          _excelFile.SetCell($"AB{i}", bill.Total);

          i++;

        }  // foreach orderItem

      } // foreach paymentOrder

    }


    private FixedList<PaymentOrder> GetPaymentOrders(DateTime fromDate, DateTime toDate) {
      return PaymentOrder.GetList<PaymentOrder>()
                         .FindAll(x => (x.InProgress || x.Payed) &&
                                       fromDate <= x.PostingTime && x.PostingTime < toDate.AddDays(1))
                         .ToFixedList()
                         .Sort((x, y) => x.PostingTime.CompareTo(y.PostingTime));
    }

  } // class PaymentsBillsToExcelBuilder

} // namespace Empiria.Payments.Reporting
