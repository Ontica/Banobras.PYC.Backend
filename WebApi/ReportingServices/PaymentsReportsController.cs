/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Payments Management                  Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Query Web Api Controller             *
*  Type     : PaymentsReportsController                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to build payments reports and export them to Excel.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Reporting;

namespace Empiria.Banobras.Reporting.WebApi {

  /// <summary>Web API used to build payments reports and export them to Excel.</summary>
  public class PaymentsReportsController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v2/financial-management/reports/payments-bills")]
    public SingleObjectModel BuildPaymentsBillsReport([FromBody] PYCReportFields fields) {

      using (var service = PaymentsReportingService.ServiceInteractor()) {

        var paymentsBills = service.BuildPaymentsBillsReport(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, paymentsBills);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/payments-bills/export")]
    public SingleObjectModel BuildPaymentsBillsExcelReport([FromBody] PYCReportFields fields) {

      using (var service = PaymentsReportingService.ServiceInteractor()) {

        var excelFileDto = service.ExportPaymentsBillsReportToExcel(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, excelFileDto);
      }
    }



    [HttpPost]
    [Route("v2/financial-management/reports/payments-concepts")]
    public SingleObjectModel BuildPaymentsConceptsReport([FromBody] PYCReportFields fields) {

      using (var service = PaymentsReportingService.ServiceInteractor()) {

        var paymentsBills = service.BuildPaymentsConceptsReport(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, paymentsBills);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/payments-concepts/export")]
    public SingleObjectModel BuildPaymentsConceptsExcelReport([FromBody] PYCReportFields fields) {

      using (var service = PaymentsReportingService.ServiceInteractor()) {

        var excelFileDto = service.ExportPaymentConceptsToExcel(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, excelFileDto);
      }
    }

    #endregion Web apis

  }  // class PaymentsReportsController

}  // namespace Empiria.Banobras.Reporting.WebApi
