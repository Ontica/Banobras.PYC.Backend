/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Management                 Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Query Web Api Controller             *
*  Type     : BudgetingReportsController                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to build budgeting journals and other budgeting reports.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Budgeting.Reporting;

namespace Empiria.Banobras.Reporting.WebApi {

  /// <summary>Web API used to build budgeting journals and other budgeting reports.</summary>
  public class BudgetingReportsController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v2/financial-management/reports/budget-allocation-journal")]
    public SingleObjectModel BuildBudgetAllocationJournalReport([FromBody] ReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        var allocationJournal = service.BuildBudgetAllocationJournal(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, allocationJournal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-exercise-journal")]
    public SingleObjectModel BuildBudgetExerciseJournalReport([FromBody] ReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        var exerciseJournal = service.BuildBudgetExerciseJournal(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, exerciseJournal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-exercise-journal/export")]
    public SingleObjectModel ExportPaymentOrdersToExcel([FromBody] ReportFields fields) {

      using (var reportingService = BudgetingReportingService.ServiceInteractor()) {

        FileDto exerciseJournal = reportingService.ExportBudgetExerciseJournal(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, exerciseJournal);
      }
    }


    #endregion Web apis

  }  // class BudgetingReportsController

}  // namespace Empiria.Banobras.Reporting.WebApi
