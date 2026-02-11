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

      using (var appServices = BudgetExerciseAppServices.UseCaseInteractor()) {

        int exercisedCount = appServices.ExerciseBudget();

        return new SingleObjectModel(base.Request, new {
          Message = $"Se generaron {exercisedCount} transacciones de ejercicio presupuestal y ninguna póliza contable."
        });
      }
    }

    #endregion Web Apis

  }  // class BanobrasSicofinController

}  // namespace Empiria.BanobrasIntegration.Sicofin.WebApi
