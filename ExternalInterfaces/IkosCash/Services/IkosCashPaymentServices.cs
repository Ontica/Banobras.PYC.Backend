/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Service provider                        *
*  Type     : IkosCashPaymentService                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements IPaymentService interface using IkosCash messages services.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using System.Threading.Tasks;

using Empiria.Payments.Processor;
using Empiria.Payments.Processor.Adapters;

using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {

  /// <summary>Implements IPaymentService interface using IkosCash messages services.</summary>
  public class IkosCashPaymentService : IPaymentsBrokerService {

    private readonly IkosCashPaymentsApiClient _apiClient;

    #region Methods

    public IkosCashPaymentService() {
      _apiClient = new IkosCashPaymentsApiClient();
    }


    internal async Task<List<OrganizationUnitDto>> GetOrganizationUnitConcepts(int idSistema) {

      var departamentos = await _apiClient.GetDepartamentos(idSistema);

      return IkosCashMapper.MapToOrganizationUnits(departamentos);
    }


    async Task<BrokerResponseDto> IPaymentsBrokerService.CancelPaymentInstruction(BrokerRequestDto brokerRequest) {
      Assertion.Require(brokerRequest, nameof(brokerRequest));

      IkosCashCancelTransactionPayload cancelPayload = IkosCashMapper.MapToCancelTransactionPayload(brokerRequest);

      IkosCashCancelTransactionResult result = await _apiClient.CancelPaymentTransaction(cancelPayload);

      return IkosCashMapper.MapToBrokerResponseDto(result);
    }


    async Task<BrokerResponseDto> IPaymentsBrokerService.RequestPaymentStatus(BrokerRequestDto brokerRequest) {
      IkosStatusRequest ikosStatusRequest = IkosCashMapper.MapToIkosSolicitudStatus(brokerRequest);

      IkosStatusDto ikosStatus = await _apiClient.RequestPaymentStatus(ikosStatusRequest);

      return IkosCashMapper.MapToBrokerResponseDto(brokerRequest, ikosStatus);
    }


    async Task<BrokerResponseDto> IPaymentsBrokerService.SendPaymentInstruction(BrokerRequestDto instruction) {
      Assertion.Require(instruction, nameof(instruction));

      PaymentOrder paymentOrder = instruction.PaymentOrder;

      Assertion.Require(!paymentOrder.IsEmptyInstance, nameof(paymentOrder));
      Assertion.Require(paymentOrder.PaymentInstructions.CanCreateNewInstruction(),
                        "No se puede enviar la instrucción de pago debido a que " +
                        $"la orden de pago está en estado {paymentOrder.Status.GetName()}.");

      IkosCashTransactionPayload payload = IkosCashMapper.MapToIkosCashTransactionPayload(instruction);

      SetElectronicSign(payload);

      IkosCashInstructionResult result = await _apiClient.SendPaymentTransaction(payload);

      return IkosCashMapper.MapToBrokerResponseDto(result);
    }

    #endregion Methods

    #region Helpers

    private void SetElectronicSign(IkosCashTransactionPayload payload) {
      string cadenaOriginal = IkosCashMapper.GetCadenaOriginalFirma(payload);

      var certificateServices = new CertificateServices(IkosCashConstantValues.GET_PYC_CERTIFICATE());

      string esign = certificateServices.Sign(cadenaOriginal);

      payload.Header.SetFirma(esign);
    }

    #endregion Helpers

  } // class PaymentService

} // namespace  Empiria.Payments.BanobrasIntegration.IkosCash
