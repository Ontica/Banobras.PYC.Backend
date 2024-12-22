﻿/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Provider implementation                 *
*  Type     : PaymentService                             License   : Please read LICENSE.txt file            *
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

    public async Task<string> GetToken() {
      return await _apiClient.GetClientToken();
    }

    public string GetFirma(TransaccionFields paymentTransaction) {
      return  Mapper.GetFirma(paymentTransaction); 
    }

    public async Task<ResultadoTransaccionDto> AddPaymentTransaction(TransaccionFields paymentTransaction) {
      var firma = Mapper.GetFirma(paymentTransaction);
      paymentTransaction.Header.Firma = firma;

      List<TransaccionFields> paymentTransactions = new List<TransaccionFields>();
      paymentTransactions.Add(paymentTransaction);
     
      List<ResultadoTransaccionDto> pagos = await _apiClient.CreateTransactions(paymentTransactions);
           
      return pagos[0];
    }


    public async Task<EliminarTransaccionDto> DeletePaymentTransaction(EliminarTransaccionFields paymentTransaction) {
      List<EliminarTransaccionFields> paymentTransactions = new List<EliminarTransaccionFields>();
      paymentTransactions.Add(paymentTransaction);

      List<EliminarTransaccionDto> pagos = await _apiClient.DeleteTransaction(paymentTransactions);

      return pagos[0];
    }


    public async Task<PaymentStatusResultDto> GetPaymentsStatus(string Idsolicitud) {
      var paymentRequest = Mapper.MapToIkosMinimalDto(Idsolicitud);
      List<SolicitudField> solicitudes = new List<SolicitudField>();
      solicitudes.Add(paymentRequest);
 
      var ikosStatus = await _apiClient.GetTransactionStatus(solicitudes);

      return Mapper.MapToPaymentStatusDTO(ikosStatus);
    }


    public async Task<List<OrganizationUnitDto>> GetOrganizationUnitConcepts(int idSistema) {

      var ikosConceptos = await _apiClient.GetConcepts(idSistema);

      return Mapper.MapToOrganizationUnits(ikosConceptos);
    }


    #endregion Public methods

  } // class PaymentService

} // namespace  Empiria.Payments.BanobrasIntegration.IkosCash
