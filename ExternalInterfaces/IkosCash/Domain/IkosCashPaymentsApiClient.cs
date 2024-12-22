/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : ApiClient                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Client to consume Ikos cash web services.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;
using Newtonsoft.Json;


namespace Empiria.Payments.BanobrasIntegration.IkosCash {
  /// <summary>Client to consume Ikos cash web services.</summary>
  internal class IkosCashPaymentsApiClient {

    #region Global Variables

    private readonly HttpClient client = new HttpClient();
    private readonly string baseAddress = ConstantValues.API_BASE_ADDRESS;

    #endregion Global Variables

    internal IkosCashPaymentsApiClient() {
      SetHttpClientProperties();
    }

    #region Internal Methods


    internal async Task<List<ResultadoTransaccionDto>> CreateTransactions(List<TransaccionFields> paymentTransactions) {

      HttpResponseMessage response = await client.PostAsJsonAsync("recepcion/message", paymentTransactions);

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<List<ResultadoTransaccionDto>>(jsonString);
    }


    internal async Task<List<EliminarTransaccionDto>> DeleteTransaction(List<EliminarTransaccionFields> fields) {

      HttpResponseMessage response = await client.PostAsJsonAsync("delete/message", fields);

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<List<EliminarTransaccionDto>>(jsonString);
    }


    internal async Task<List<DepartamentoDto>> GetConcepts(int idSistema) {
      HttpResponseMessage response = await client.GetAsync(baseAddress + $"catalogos/departamento/{idSistema}");

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<List<DepartamentoDto>>(jsonString);
    }


    internal async Task<List<IkosStatusDto>> GetTransactionStatus(List<SolicitudField> solicitudes) {
      HttpResponseMessage response = await client.PostAsJsonAsync("operacion/status", solicitudes);

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();

      return JsonConvert.DeserializeObject<List<IkosStatusDto>>(jsonString);
    }


    #endregion Internal Methods

    #region Private Methods

    private void SetHttpClientProperties() {
      //string token = await GetClientToken();
      client.BaseAddress = new Uri(baseAddress);
      client.DefaultRequestHeaders.Accept.Clear();

      client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

      client.DefaultRequestHeaders.Add("TOKEN", ConstantValues.TOKEN);
     
    }

    internal async Task<string> GetClientToken() {
      IkosCashTokenApiClient clientToken = new IkosCashTokenApiClient();
      return await clientToken.GetToken();
    }

    #endregion Private Variables & Methods


  } // class ApiClient

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash
