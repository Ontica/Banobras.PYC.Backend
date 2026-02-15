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

using Empiria.DynamicData;
using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Budgeting.Reporting;

namespace Empiria.Banobras.Reporting.WebApi {

  /// <summary>Web API used to build budgeting journals and other budgeting reports.</summary>
  public class BudgetingReportsController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v2/financial-management/reports/budget-allocation-journal")]
    public SingleObjectModel AllocationJournalDataTable([FromBody] ReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        DynamicDto<BudgetAllocationJournalEntry> journal =
                                    service.AllocationJournalDynamicTable(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, journal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-allocation-journal/export")]
    public SingleObjectModel AllocationJournalToExcel([FromBody] ReportFields fields) {

      using (var reportingService = BudgetingReportingService.ServiceInteractor()) {

        FileDto journal = reportingService.AllocationJournalToExcel(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, journal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-exercise-journal")]
    public SingleObjectModel ExerciseJournalDataTable([FromBody] ReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        DynamicDto<BudgetExerciseJournalEntry> journal =
                                  service.ExerciseJournalDynamicTable(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, journal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-exercise-journal/export")]
    public SingleObjectModel ExerciseJournalToExcel([FromBody] ReportFields fields) {

      using (var reportingService = BudgetingReportingService.ServiceInteractor()) {

        FileDto journal = reportingService.ExerciseJournalToExcel(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, journal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-requests-journal")]
    public SingleObjectModel RequestsJournalDataTable([FromBody] ReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        DynamicDto<BudgetRequestsJournalEntry> journal =
                                    service.RequestsJournalDynamicTable(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, journal);
      }
    }

    #endregion Web apis

  }  // class BudgetingReportsController

}  // namespace Empiria.Banobras.Reporting.WebApi
