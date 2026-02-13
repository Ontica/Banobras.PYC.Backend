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
    [Route("v2/financial-management/reports/budget-exercise-journal")]
    public SingleObjectModel BuildBudgetExerciseJournalReport([FromBody] ReportFields fields) {

      using (var service = BudgetingReportingService.ServiceInteractor()) {

        var budgetExerciseJournal = service.BuildBudgetExerciseJournal(fields.FromDate, fields.ToDate);

        return new SingleObjectModel(base.Request, budgetExerciseJournal);
      }
    }


    [HttpPost]
    [Route("v2/financial-management/reports/budget-exercise-journal/export")]
    public SingleObjectModel ExportPaymentOrdersToExcel([FromBody] ReportFields fields) {

      using (var reportingService = BudgetingReportingService.ServiceInteractor()) {

        var result = new FileResultDto(
            reportingService.ExportBudgetExerciseJournal(fields.FromDate, fields.ToDate),
            $"Se exportaron los movimientos del ejercicio presupuestal a Excel."
        );

        SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result.File);
      }
    }


    #endregion Web apis

  }  // class BudgetingReportsController

}  // namespace Empiria.Banobras.Reporting.WebApi
