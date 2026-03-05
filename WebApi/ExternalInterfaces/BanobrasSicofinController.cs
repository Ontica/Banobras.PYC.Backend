/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                    Component : Web Api                               *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web Api Controller                    *
*  Type     : BanobrasSicofinController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API for PYC-SICOFIN integration.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Banobras.Budgeting.AppServices;

namespace Empiria.BanobrasIntegration.Sicofin.WebApi {

  /// <summary>Web API for PYC-SICOFIN integration.</summary>
  public class BanobrasSicofinController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/pyc/analyze-budget-data")]
    public SingleObjectModel AnalyzeBudgetData() {

      using (var appServices = BudgetStatusAppServices.UseCaseInteractor()) {

        int logCount = appServices.ExecuteAnalysis();

        return new SingleObjectModel(base.Request, new {
          Message = $"El sistema encontró {logCount} transacciones con uno o más datos por actualizar."
        });
      }
    }


    [HttpPost]
    [Route("v2/pyc/integration/sicofin/vouchers/generate")]
    public SingleObjectModel GenerateSicofinVouchers() {

      using (var appServices = BudgetExerciseAppServices.UseCaseInteractor()) {

        int cleanCount = appServices.CleanBudget();

        string message = $"Se hicieron {cleanCount} ajustes a transacciones prespuestales. ";

        int exercisedCount = appServices.ExerciseBudget();

        message += $"Se generaron {exercisedCount} transacciones de ejercicio presupuestal. ";

        return new SingleObjectModel(base.Request, new {
          Message = message + $"No se generaron pólizas contables."
        });
      }
    }

    #endregion Web Apis

  }  // class BanobrasSicofinController

}  // namespace Empiria.BanobrasIntegration.Sicofin.WebApi
