/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Provider implementation                 *
*  Type     : PaymentService                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements IPaymentService interface using Ikos Cash messages services.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;
using Empiria.Payments.Processor.Adapters;


namespace Empiria.Payments.BanobrasIntegration.IkosCash {
    /// <summary>Implements IPaymentService interface using Ikos Cash messages services.</summary>
    public class PaymentService : IPaymentService {

    private readonly IkosCashPaymentsApiClient _apiClient;

    #region Public methods

    public PaymentService() {
      _apiClient = new IkosCashPaymentsApiClient();
    }


    public async Task<IPaymentResult> AddPaymentTransaction(IPaymentInstruction paymentInstruction) {
      TransaccionFields transaccion = Mapper.MapPaymentInstructionToIcosCashTransaction(paymentInstruction);

      List<ResultadoTransaccionDto> pagos = await _apiClient.CratePaymentTransactions(transaccion);

      var paymentResult = Mapper.MapIcosCashTransactionToPaymentResulDTO(pagos);

      return paymentResult;
    }


    public async Task<PaymentStatusDto> GetPaymentsStatus(MinimalPaymentDto paymentTransaction) {
      var ikosStatus = await _apiClient.GetIkosCashTransactionStatus(paymentTransaction);

      return Mapper.MapToPaymentStatusDTO(ikosStatus);
    }


    public async Task<List<OrganizationUnitDto>> GetOrganizationUnitConcepts(int idSistema) {

      var ikosConceptos = await _apiClient.GetConcepts(idSistema);

      return Mapper.MapToOrganizationUnits(ikosConceptos);
    }


    #endregion Public methods

  } // class PaymentService

} // namespace  Empiria.Payments.BanobrasIntegration.IkosCash
