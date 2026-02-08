/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Payments Management                  Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Query Web Api Controller             *
*  Type     : PaymentOrderReportsController                 License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to generate payment order reports and documents.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;
using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Payments;

using Empiria.Payments.Reporting;

namespace Empiria.Banobras.Reporting.WebApi {

  /// <summary>Web API used to generate payment order reports and documents.</summary>
  public class PaymentOrderReportsController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v2/payments-management/payment-orders/bulk-operation/export")]
    public SingleObjectModel ExportPaymentOrdersToExcel([FromBody] BulkOperationCommand command) {

      FixedList<PaymentOrder> paymentOrders = command.Items.Select(x => PaymentOrder.Parse(x))
                                                           .ToFixedList();

      using (var reportingService = PaymentsReportingService.ServiceInteractor()) {

        var result = new FileResultDto(
            reportingService.ExportPaymentOrdersToExcel(paymentOrders),
            $"Se exportaron {paymentOrders.Count} solicitudes de pago a Excel."
        );

        SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpGet]
    [Route("v2/payments-management/payment-orders/{paymentOrderUID:guid}/print")]
    public SingleObjectModel PrintPaymentOrder([FromUri] string paymentOrderUID) {

      var paymentOrder = PaymentOrder.Parse(paymentOrderUID);

      using (var reportingService = PaymentsReportingService.ServiceInteractor()) {

        FileDto file = reportingService.ExportPaymentOrderToPdf(paymentOrder);

        return new SingleObjectModel(base.Request, file);
      }
    }

    #endregion Web apis

  }  // class PaymentOrderReportsController

}  // namespace Empiria.Banobras.Reporting.WebApi
