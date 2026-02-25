/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                      Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : BillController                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive and update payable bills.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;
using Empiria.Banobras.Billing.Adapters;
using Empiria.Banobras.Billing.AppServices;
using Empiria.Billing.UseCases;
using Empiria.WebApi;

namespace Empiria.Banobras.Billing.WebApi {

  /// <summary>Web API used to retrive and update payable bills.</summary>
  public class BillController : WebApiController {

    #region Web apis

    //[HttpGet]
    //[Route("v2/billing-management/bills/{billUID:guid}")]
    //public SingleObjectModel GetBill([FromUri] string billUID) {

    //  using (var usecases = BillUseCases.UseCaseInteractor()) {

    //    BillHolderDto bill = usecases.GetBill(billUID);

    //    return new SingleObjectModel(base.Request, bill);
    //  }
    //}


    [HttpPost]
    [Route("v2/billing-management/bills/search")]
    public CollectionModel SearchBills([FromBody] BillsQuery query) {

      using (var service = BillAppServices.UseCaseInteractor()) {
        FixedList<BillDescriptorDto> bills = service.SearchBills(query);

        return new CollectionModel(this.Request, bills);
      }
    }

    #endregion Web apis

  } // class BillController

} // namespace Empiria.Billing.WebApi
