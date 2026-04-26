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

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Payments.Adapters;

using Empiria.Banobras.Expenses.AppServices;

using Empiria.BanobrasIntegration.Sigevi.Adapters;
using Empiria.BanobrasIntegration.Sigevi.Services;

namespace Empiria.BanobrasIntegration.Sigevi.WebApi {

  /// <summary>Web services invoked by Banobras' SIGEVI system.</summary>
  public class BanobrasSigeviController : WebApiController {

    #region Query web apis

    [HttpPost]
    [Route("v2/pyc/integration/sigevi/presupuesto-disponible")]
    public SingleObjectModel AvailableBudget([FromBody] SigeviAvailableBudgetQuery query) {

      using (var usecases = SigeviIntegrationServices.ServiceInteractor()) {
        SigeviAvailableBudgetDto available = usecases.AvailableBudget(query);

        return new SingleObjectModel(base.Request, available);
      }
    }


    [HttpGet]
    [Route("v2/pyc/integration/sigevi/areas-presupuestales")]
    public CollectionModel GetBudgetingOrganizationalUnits() {

      using (var usecases = SigeviIntegrationServices.ServiceInteractor()) {
        FixedList<AreaDto> areas = usecases.GetBudgetingOrganizationalUnits();

        return new CollectionModel(base.Request, areas);
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

    #endregion Query web apis

    #region Command web apis

    [HttpPost]
    [Route("v2/pyc/integration/sigevi/solicitar-dotacion-viaticos")]
    public SingleObjectModel SolicitarDotacionViaticos() {

      var fields = GetFormDataFromHttpRequest<SigeviSolicitudDotacionFields>("request");

      InputFile file = base.GetInputFileFromHttpRequest();

      fields.PDfSolicitudDotacion = file;

      using (var usecases = ExpensesAppServices.UseCaseInteractor()) {
        PaymentOrderDto paymentOrder = usecases.RequestPaymentForTravelExpenses(null);

        var message = new MessageFields() {
          Message = $"Se solicitó el pago de la comisión {fields.NoComision}. " +
                    $"El número de solicitud de pago correspondiente es: {paymentOrder.PaymentOrderNo}"
        };

        return new SingleObjectModel(base.Request, message);
      }
    }

    #endregion Command web apis

  }  // class BanobrasSigeviController

}  // namespace Empiria.BanobrasIntegration.Sigevi.WebApi
