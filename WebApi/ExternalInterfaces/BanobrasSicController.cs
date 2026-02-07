/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Integration                     Component : Web Api                               *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web Api Controller                    *
*  Type     : BanobrasSicController                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API for Sic System integration.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Financial.Adapters;

namespace Empiria.BanobrasIntegration.Sic.WebApi {

  /// <summary>Web API for Sic System integration.</summary>
  public class BanobrasSicController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v2/pyc/integration/sic/credits/{creditNo}/export")]
    public SingleObjectModel ExportCreditToBudgetingInterface([FromUri] string creditNo) {

      var services = new SicServices();

      ICreditSicData credit = services.TryGetCreditSic(creditNo);

      return new SingleObjectModel(base.Request, credit);


    }  // SicServices


    #endregion Web Apis

  }  // class BanobrasSicController

}  // namespace Empiria.BanobrasIntegration.Sic.WebApi
