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

    [HttpPost]
    [Route("v2/pyc/integration/sicofin/vouchers/generate")]
    public SingleObjectModel GenerateSicofinVouchers() {

      string message = "";

      using (var appServices = BudgetStatusAppServices.UseCaseInteractor()) {

        int logCount = appServices.ExecuteLog();

        message = $"El sistema encontró {logCount} transacciones por revisar.";
      }


      using (var appServices = BudgetExerciseAppServices.UseCaseInteractor()) {

        int exercisedCount = 0; // appServices.ExerciseBudget();

        message += $"Se generaron {exercisedCount} transacciones de ejercicio .";

        return new SingleObjectModel(base.Request, new {
          Message = message + $"No se generaron pólizas contables."
        });
      }
    }

    #endregion Web Apis

  }  // class BanobrasSicofinController

}  // namespace Empiria.BanobrasIntegration.Sicofin.WebApi
