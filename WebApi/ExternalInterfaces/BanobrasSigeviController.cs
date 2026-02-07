/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIGEVI Integration                  Component : Web Api                               *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web Api Controller                    *
*  Type     : BanobrasSigeviController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web services invoked by Banobras' SIGEVI system.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.BanobrasIntegration.Sigevi.Adapters;
using Empiria.BanobrasIntegration.Sigevi.Services;

namespace Empiria.BanobrasIntegration.Sigevi.WebApi {

  /// <summary>Web services invoked by Banobras' SIGEVI system.</summary>
  public class BanobrasSigeviController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/pyc/integration/sigevi/available-budget")]
    public SingleObjectModel AvailableBudget([FromBody] AvailableBudgetQuery query) {

      using (var usecases = SigeviIntegrationServices.ServiceInteractor()) {
        AvailableBudgetDto available = usecases.AvailableBudget(query);

        return new SingleObjectModel(base.Request, available);
      }
    }


    [HttpGet]
    [Route("v2/pyc/integration/sigevi/organizational-units")]
    public CollectionModel GetBudgetingOrganizationalUnits() {

      using (var usecases = SigeviIntegrationServices.ServiceInteractor()) {
        FixedList<OrgUnitDto> orgUnits = usecases.GetBudgetingOrganizationalUnits();

        return new CollectionModel(base.Request, orgUnits);
      }
    }


    [HttpPost]
    [Route("v2/pyc/integration/sigevi/request-budget")]
    public SingleObjectModel RequestBudget([FromBody] ExternalBudgetRequestFields fields) {

      using (var usecases = SigeviIntegrationServices.ServiceInteractor()) {
        ExternalBudgetRequestDto requestedBudget = usecases.RequestBudget(fields);

        return new SingleObjectModel(base.Request, requestedBudget);
      }
    }

    #endregion Web Apis

  }  // class BanobrasSigeviController

}  // namespace Empiria.BanobrasIntegration.Sigevi.WebApi
