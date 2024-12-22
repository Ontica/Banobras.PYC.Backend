/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Service provider                        *
*  Type     : IkosCashPaymentService                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements IPaymentService interface using Ikos Cash messages services.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Threading.Tasks;

using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {

    /// <summary>Implements IPaymentService interface using Ikos Cash messages services.</summary>
    public class IkosCashPaymentService : IPaymentService {

    private readonly IkosCashPaymentsApiClient _apiClient;

    #region Public methods

    public IkosCashPaymentService() {
      _apiClient = new IkosCashPaymentsApiClient();
    }


    public async Task<EliminarTransaccionDto> CancelPaymentTransaction(EliminarTransaccionFields paymentTransaction) {
      Assertion.Require(paymentTransaction, nameof(paymentTransaction));

      return await _apiClient.CancelPaymentTransaction(paymentTransaction);
    }


    public async Task<PaymentStatusResultDto> GetPaymentTransactionStatus(string idSolicitud) {
      var paymentRequest = Mapper.MapToIkosMinimalDto(idSolicitud);

      var ikosStatus = await _apiClient.GetPaymentTransactionStatus(paymentRequest);

      return Mapper.MapToPaymentStatusDTO(ikosStatus);
    }


    public async Task<List<OrganizationUnitDto>> GetOrganizationUnitConcepts(int idSistema) {

      var departamentos = await _apiClient.GetDepartamentos(idSistema);

      return Mapper.MapToOrganizationUnits(departamentos);
    }


    public async Task<ResultadoTransaccionDto> SendPaymentTransaction(TransaccionFields paymentTransaction) {
      Assertion.Require(paymentTransaction, nameof(paymentTransaction));

      string cadenaOriginal = Mapper.GetCadenaOriginalFirma(paymentTransaction);

      var certificateServices = new CertificateServices(IkosCashConstantValues.GET_PYC_CERTIFICATE());

      string firma = certificateServices.Sign(cadenaOriginal);

      paymentTransaction.SetFirma(firma);

      return await _apiClient.SendPaymentTransaction(paymentTransaction);
    }

    #endregion Public methods

  } // class PaymentService

} // namespace  Empiria.Payments.BanobrasIntegration.IkosCash
