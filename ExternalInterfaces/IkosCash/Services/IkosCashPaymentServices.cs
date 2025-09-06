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

using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;
using Empiria.Payments.Processor.Adapters;

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


    PaymentInstructionResultDto IPaymentsBrokerService.CancelPaymentInstruction(PaymentInstructionDto instruction) {
      Assertion.Require(instruction, nameof(instruction));

      IkosCashCancelTransactionPayload cancelPayload = IkosCashMapper.MapToCancelTransactionPayload(instruction);

      IkosCashCancelTransactionResult result = _apiClient.CancelPaymentTransaction(cancelPayload)
                                                         .GetAwaiter()
                                                         .GetResult();

      return IkosCashMapper.MapToPaymentResultDto(result);
    }


    PaymentInstructionStatusDto IPaymentsBrokerService.GetPaymentInstructionStatus(string instructionUID) {
      SolicitudStatus statusRequest = IkosCashMapper.MapToIkosSolicitudStatus(instructionUID);

      var ikosStatus = _apiClient.GetPaymentTransactionStatus(statusRequest)
                                 .GetAwaiter()
                                 .GetResult();

      return IkosCashMapper.MapToPaymentInstructionStatus(ikosStatus);
    }


    PaymentInstructionResultDto IPaymentsBrokerService.SendPaymentInstruction(PaymentInstructionDto instruction) {
      Assertion.Require(instruction, nameof(instruction));

      IkosCashTransactionPayload transactionPayload = IkosCashMapper.MapToTransactionPayload(instruction);

      string cadenaOriginal = IkosCashMapper.GetCadenaOriginalFirma(transactionPayload);

      var certificateServices = new CertificateServices(IkosCashConstantValues.GET_PYC_CERTIFICATE());

      string firma = certificateServices.Sign(cadenaOriginal);

      transactionPayload.Header.SetFirma(firma);

      IkosCashTransactionResult result = _apiClient.SendPaymentTransaction(transactionPayload)
                                                   .GetAwaiter()
                                                   .GetResult();

      return IkosCashMapper.MapToPaymentResultDto(result);
    }

    #endregion Methods

  } // class PaymentService

} // namespace  Empiria.Payments.BanobrasIntegration.IkosCash
