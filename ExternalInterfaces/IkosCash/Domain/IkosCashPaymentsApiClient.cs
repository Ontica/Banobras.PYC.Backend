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
    private readonly string baseAddress = "http://localhost:4110/api/";

    #endregion Global Variables

    internal IkosCashPaymentsApiClient() {
      SetHttpClientProperties();
    }

    #region Internal Methods

    internal async Task<List<ResultadoTransaccionDto>> CratePaymentTransactions(TransaccionFields transaction) {
      List<TransaccionFields> transaccionFields = new List<TransaccionFields>();

      HttpResponseMessage response = await client.PostAsJsonAsync("recepción/message", transaccionFields);

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();

      return JsonConvert.DeserializeObject<List<ResultadoTransaccionDto>>(jsonString);
    }


    internal async Task<EliminarTransaccionDto> DeleteTransaction(EliminarTransaccionFields fields) {

      HttpResponseMessage response = await client.PostAsJsonAsync("delete/message", fields);

      if (response.IsSuccessStatusCode) {
        return await response.Content.ReadAsAsync<EliminarTransaccionDto>();
      } else {
        throw new Exception($"Can not find transaction with id={fields.IdSolicitud}");
      }

    }


    internal async Task<List<DepartamentoDto>> GetConcepts(int idSistema) {
      HttpResponseMessage response = await client.GetAsync(baseAddress + $"catalogos/departamento/{idSistema}");

      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsAsync<List<DepartamentoDto>>();
    }


    internal async Task<List<IkosStatusDto>> GetIkosCashTransactionStatus(MinimalPaymentRequestDto payment) {
      List<MinimalPaymentRequestDto> payments = new List<MinimalPaymentRequestDto>();

      HttpResponseMessage response = await client.PostAsJsonAsync("operacion/status", payments);

      response.EnsureSuccessStatusCode();     

      var jsonString = await response.Content.ReadAsStringAsync();

      return JsonConvert.DeserializeObject<List<IkosStatusDto>>(jsonString);
    }


    #endregion Internal Methods

    #region Private Methods

    private async void SetHttpClientProperties() {
      string token = await GetClientToken();
      client.BaseAddress = new Uri(baseAddress);
      client.DefaultRequestHeaders.Accept.Clear();

      client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

      client.DefaultRequestHeaders.Add("TOKEN", token);
    }

    private async Task<string> GetClientToken() {
      IkosCashTokenApiClient clientToken = new IkosCashTokenApiClient();
      return await clientToken.GetToken();
    }

    #endregion Private Variables & Methods


  } // class ApiClient

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash
