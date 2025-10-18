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
using Empiria.StateEnums;
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

    [HttpPut, HttpPatch]
    [Route("v2/external-interfaces/sial/{status}/{payrollNo}")]

    public SingleObjectModel UpdateProcessPayroll(EntityStatus status, int payrollNo,
                                                  [FromBody] SialHeaderQuery query) {

      query.Status = status;

      _service.UpdateProcessStatus(status, payrollNo);  

      return new SingleObjectModel(this.Request, query.Status.GetName());
    }
    

    #endregion Sial importers

  }  // class BanobrasSialImportersController

}  // namespace Empiria.PYC.WebApi.BanobrasIntegration
