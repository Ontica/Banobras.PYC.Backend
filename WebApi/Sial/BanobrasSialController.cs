/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                    Component : Web Api                               *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web Api Controller                    *
*  Type     : BanobrasSialController                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API for Sial System integration.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.StateEnums;
using Empiria.WebApi;

using Empiria.BanobrasIntegration.Sial;
using Empiria.BanobrasIntegration.Sial.Adapters;

namespace Empiria.PYC.WebApi.BanobrasIntegration {

  /// <summary>Web API for Sial System integration.</summary>
  public class BanobrasSialController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/pyc/integration/sial/payrolls")]
    public CollectionModel SearchPayrolls([FromBody] SialPayrollsQuery query) {

      using (var services = SialServices.ServiceInteractor()) {
        FixedList<SialPayrollDto> payrolls = services.SearchPayrolls(query);

        return new CollectionModel(base.Request, payrolls);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/external-interfaces/sial/{status}/{payrollNo}")]
    public SingleObjectModel UpdateProcessPayroll(EntityStatus status, int payrollNo,
                                                  [FromBody] SialPayrollsQuery query) {

      query.Status = status;

      using (var services = SialServices.ServiceInteractor()) {
        services.UpdateProcessStatus(query.Status, payrollNo);

        return new SingleObjectModel(this.Request, query.Status.GetName());
      }
    }

    #endregion Web Apis

  }  // class BanobrasSialController

}  // namespace Empiria.PYC.WebApi.BanobrasIntegration
