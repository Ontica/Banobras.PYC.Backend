/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Integration Services                Component : Voucher importation                   *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Command Controller                    *
*  Type     : BanobrasSialImportersController              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API that imports sial from Banobras PYC .                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;
using Empiria.BanobrasIntegration.Sial;
using Empiria.BanobrasIntegration.Sial.Adapters;
using Empiria.Financial.Adapters;
using Empiria.Storage;
using Empiria.WebApi;

namespace Empiria.PYC.WebApi.BanobrasIntegration {

  /// <summary>Web API that imports sial from Banobras PYC.</summary>
  public class BanobrasSialImportersController : WebApiController {
    
    private readonly SialServices _service = new SialServices();

    #region Sial importers

    [HttpPost]
    [Route("v2/external-interfaces/sial/SialPayrolls")]
    public SingleObjectModel ImportSialPayrolls([FromBody] SialHeaderQuery query  ) {

      var result = _service.GetPayrollsEntries(query.Status, query.FromDate, query.ToDate);

      return new SingleObjectModel(base.Request, result);

    }

    #endregion Sial importers

  }  // class BanobrasSialImportersController

}  // namespace Empiria.PYC.WebApi.BanobrasIntegration
