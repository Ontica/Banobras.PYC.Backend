/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC External Interfaces             Component : Web Api                               *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web Api Controller                    *
*  Type     : ExternalBudgetingController                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used for budget integration with other Banobras' systems.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Banobras.Budgeting.Adapters;
using Empiria.Banobras.Budgeting.UseCases;

namespace Empiria.Banobras.Budgeting.WebApi {

  /// <summary>Web API used for budget integration with other Banobras' systems.</summary>
  public class ExternalBudgetingController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/sigevi/integration/budgeting/available-budget")]
    public SingleObjectModel AvailableBudget([FromBody] AvailableBudgetQuery query) {

      using (var usecases = ExternalBudgetingUseCases.UseCaseInteractor()) {
        AvailableBudgetDto available = usecases.AvailableBudget(query);

        return new SingleObjectModel(base.Request, available);
      }
    }

    #endregion Web Apis

  }  // class ExternalBudgetingController

}  // namespace Empiria.Banobras.Budgeting.WebApi
