/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Procurement Services                 Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Web Api Controller                   *
*  Type     : ProvisionsController                          License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web api used to handle expenses provisions.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Provisions.Adapters;

using Empiria.Banobras.Provisions.AppServices;

namespace Empiria.Banobras.Provisions.WebApi {

  /// <summary>Web api used to handle expenses provisions.</summary>
  public class ProvisionsController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/financial-management/provisions/search")]
    public CollectionModel SearchProvisions([FromBody] ProvisionsQuery query) {

      base.RequireBody(query);

      using (var services = ProvisionsAppServices.UseCaseInteractor()) {

        var orders = services.SearchProvisions(query);

        return new CollectionModel(base.Request, orders);
      }
    }

    #endregion Web Apis

  }  // class ProvisionsController

}  // namespace Empiria.Banobras.Provisions.WebApi
