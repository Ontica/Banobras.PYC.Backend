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


namespace Empiria.Payments.BanobrasIntegration.IkosCash {
    /// <summary>Implements IPaymentService interface using Ikos Cash messages services.</summary>
    public class PaymentService : IPaymentService {

    private readonly IkosCashPaymentsApiClient _apiClient;

    #region Public methods

    public PaymentService() {
      _apiClient = new IkosCashPaymentsApiClient();
    }


    public async Task<List<PaymentDto>> AddTransactions(List<PaymentRequestDto> paymentRequests) {
      List<TransaccionFields> transacciones = Mapper.MapPaymentRequestToIcosCashTransactions(paymentRequests);

      List<TransaccionDto> pagos = await _apiClient.CratePaymentTransactions(transacciones);

      var payments = Mapper.MapIcosCashTransactionToPaymentDTO(pagos);

      return payments;
    }


    public async Task<List<PaymentStatusDto>> GetPaymentsStatus(List<MinimalPaymentDto> payments) {
      var ikosStatus = await _apiClient.GetIkosCashTransactionStatus(payments);

      return Mapper.MapToPaymentStatusListDTO(ikosStatus);
    }


    public async Task<List<OrganizationUnitDto>> GetOrganizationUnitConcepts(int idSistema) {

      var ikosConceptos = await _apiClient.GetConcepts(idSistema);

      return Mapper.MapToOrganizationUnits(ikosConceptos);
    }


    #endregion Public methods

  } // class PaymentService

} // namespace  Empiria.Payments.BanobrasIntegration.IkosCash
