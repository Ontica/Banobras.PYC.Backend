/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Payments Management                 Component : Web Api Layer                         *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web api Controller                    *
*  Type     : PayeesController                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update payees.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.Payments.Adapters;

using Empiria.Banobras.Payments.AppServices;
using Empiria.Banobras.Payments.Adapters;

namespace Empiria.Banobras.Payments.WebApi {

  /// <summary>Web API used to retrieve and update suppliers.</summary>
  public class PayeesController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v8/procurement/suppliers/{payeeUID:guid}")]   // ToDo: remove this route in future versions
    [Route("v2/payments-management/payees/{payeeUID:guid}")]
    public SingleObjectModel GetPayee([FromUri] string payeeUID) {

      using (var usecases = PayeeAppServices.UseCaseInteractor()) {
        PayeeHolderDto payee = usecases.GetPayee(payeeUID);

        return new SingleObjectModel(base.Request, payee);
      }
    }


    [HttpGet]
    [Route("v8/procurement/suppliers/types")]  // ToDo: remove this route in future versions
    [Route("v2/payments-management/payees/types")]
    public CollectionModel GetPayeeTypes() {

      using (var usecases = PayeeAppServices.UseCaseInteractor()) {
        FixedList<NamedEntityDto> payeeTypes = usecases.GetPayeeTypes();

        return new CollectionModel(base.Request, payeeTypes);
      }
    }


    [HttpPost]
    [Route("v8/procurement/suppliers/search")]  // ToDo: remove this route in future versions
    [Route("v2/payments-management/payees/search")]
    public CollectionModel SearchPayees([FromBody] PayeesQuery query) {

      using (var usecases = PayeeAppServices.UseCaseInteractor()) {
        FixedList<PayeeDescriptor> payees = usecases.SearchPayees(query);

        return new CollectionModel(base.Request, payees);
      }
    }

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v8/procurement/suppliers")]  // ToDo: remove this route in future versions
    [Route("v2/payments-management/payees")]
    public async Task<SingleObjectModel> CreateSupplier([FromBody] PayeeFieldsExtended fields) {

      using (var usecases = PayeeAppServices.UseCaseInteractor()) {
        PayeeHolderDto payee = await usecases.AddPayee(fields);

        return new SingleObjectModel(base.Request, payee);
      }
    }


    [HttpDelete]
    [Route("v8/procurement/suppliers/{payeeUID:guid}")]  // ToDo: remove this route in future versions
    [Route("v2/payments-management/payees/{payeeUID:guid}")]
    public NoDataModel RemovePayee([FromUri] string payeeUID) {

      using (var usecases = PayeeAppServices.UseCaseInteractor()) {
        _ = usecases.RemovePayee(payeeUID);

        return new NoDataModel(base.Request);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v8/procurement/suppliers/{payeeUID:guid}")]
    [Route("v2/payments-management/payees/{payeeUID:guid}")]
    public async Task<SingleObjectModel> UpdateSupplier([FromUri] string payeeUID,
                                                        [FromBody] PayeeFieldsExtended fields) {

      fields.UID = payeeUID;

      using (var usecases = PayeeAppServices.UseCaseInteractor()) {
        PayeeHolderDto payee = await usecases.UpdatePayee(fields);

        return new SingleObjectModel(base.Request, payee);
      }
    }

    #endregion Command web apis

  }  // class PayeesController

}  // namespace Empiria.Banobras.Payments.WebApi
