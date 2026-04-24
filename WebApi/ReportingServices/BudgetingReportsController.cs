/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Management                 Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Query Web Api Controller             *
*  Type     : BudgetingReportsController                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to build budgeting journals and other budgeting reports.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.DynamicData;
using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Budgeting.Reporting;

using Empiria.Banobras.Reporting.Builders.Budgeting;

namespace Empiria.Banobras.Reporting.WebApi {

  /// <summary>Web API used to build budgeting journals and other budgeting reports.</summary>
  public class BudgetingReportsController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v2/financial-management/reports/budget-allocation-journal")]
    public SingleObjectModel AllocationJournalDataTable([FromBody] PYCReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        DynamicDto<BudgetAllocationJournalEntry> journal =
                                    service.AllocationJournalDynamicTable(fields);

        return new SingleObjectModel(base.Request, journal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-allocation-journal/export")]
    public SingleObjectModel AllocationJournalToExcel([FromBody] PYCReportFields fields) {

      using (var reportingService = BudgetingReportingService.ServiceInteractor()) {

        FileDto journal = reportingService.AllocationJournalToExcel(fields);

        return new SingleObjectModel(base.Request, journal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-exercise-journal")]
    public SingleObjectModel ExerciseJournalDataTable([FromBody] PYCReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        DynamicDto<BudgetExerciseJournalEntry> journal =
                                  service.ExerciseJournalDynamicTable(fields);

        return new SingleObjectModel(base.Request, journal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-exercise-accounting-reconciliation")]
    public async Task<SingleObjectModel> ExerciseAccountingReconciliation([FromBody] PYCReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        DynamicDto<ExerciseReconciliationDto> reconciliation =
                    await service.GetBudgetExerciseAccountingReconciliation(fields);

        return new SingleObjectModel(base.Request, reconciliation);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-exercise-accounting-reconciliation/export")]
    public async Task<SingleObjectModel> ExerciseAccountingReconciliationToExcel([FromBody] PYCReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        DynamicDto<ExerciseReconciliationDto> reconciliation =
                    await service.GetBudgetExerciseAccountingReconciliation(fields);

        return new SingleObjectModel(base.Request, reconciliation);
      }
    }

    [HttpPost]
    [Route("v2/financial-management/reports/budget-exercise-journal/export")]
    public SingleObjectModel ExerciseJournalToExcel([FromBody] PYCReportFields fields) {

      using (var reportingService = BudgetingReportingService.ServiceInteractor()) {

        FileDto journal = reportingService.ExerciseJournalToExcel(fields);

        return new SingleObjectModel(base.Request, journal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-requests-analytics")]
    public SingleObjectModel RequestsAnalyticsDataTable([FromBody] PYCReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        DynamicDto<BudgetRequestsAnalyticsEntryDto> entries =
                                    service.RequestsAnalyticsDynamicTable(fields);

        return new SingleObjectModel(base.Request, entries);
      }
    }



    [HttpPost]
    [Route("v2/financial-management/reports/budget-requests-analytics/export")]
    public SingleObjectModel RequestsAnalyticsToExcel([FromBody] PYCReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        FileDto excelFile = service.RequestsAnalyticsToExcel(fields);

        return new SingleObjectModel(base.Request, excelFile);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-requests-journal")]
    public SingleObjectModel RequestsJournalDataTable([FromBody] PYCReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        DynamicDto<BudgetRequestsJournalEntry> journal = service.RequestsJournalDynamicTable(fields);

        return new SingleObjectModel(base.Request, journal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-requests-journal/export")]
    public SingleObjectModel RequestsJournalToExcel([FromBody] PYCReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        FileDto journal = service.RequestsJournalToExcel(fields);

        return new SingleObjectModel(base.Request, journal);
      }
    }

    #endregion Web apis

  }  // class BudgetingReportsController

}  // namespace Empiria.Banobras.Reporting.WebApi
