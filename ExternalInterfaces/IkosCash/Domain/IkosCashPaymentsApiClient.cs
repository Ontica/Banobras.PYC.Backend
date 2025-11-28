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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {

  /// <summary>Client to consume Ikos cash web services.</summary>
  internal class IkosCashPaymentsApiClient {

    #region Fields

    private readonly HttpClient _httpClient = new HttpClient();

    #endregion Fields

    internal IkosCashPaymentsApiClient() {
      SetHttpClientProperties();
    }

    #region Methods

    internal async Task<IkosCashCancelTransactionResult> CancelPaymentTransaction(IkosCashCancelTransactionPayload fields) {
      Assertion.Require(fields, nameof(fields));

      await EnsureAccessTokenIsCreated();

      HttpResponseMessage response = await _httpClient.PostAsJsonAsync("delete/message",
                                                                       new IkosCashCancelTransactionPayload[1] { fields });

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();

      IkosCashCancelTransactionResult[] result = JsonConvert.DeserializeObject<IkosCashCancelTransactionResult[]>(jsonString);

      EnsureIkosCashSuccessResponse(result);

      return result[0];
    }


    internal async Task<List<DepartamentoDto>> GetDepartamentos(int idSistema) {

      await EnsureAccessTokenIsCreated();

      HttpResponseMessage response = await _httpClient.GetAsync($"catalogos/departamento/{idSistema}");

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();

      return JsonConvert.DeserializeObject<List<DepartamentoDto>>(jsonString);
    }


    internal async Task<IkosStatusDto> GetPaymentTransactionStatus(SolicitudStatus solicitud) {
      Assertion.Require(solicitud, nameof(solicitud));

      await EnsureAccessTokenIsCreated();

      HttpResponseMessage response = await _httpClient.PostAsJsonAsync("operacion/status",
                                                                        new SolicitudStatus[1] { solicitud });

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();

      IkosStatusDto[] result = JsonConvert.DeserializeObject<IkosStatusDto[]>(jsonString);

      EnsureIkosCashSuccessResponse(result);

      return result[0];
    }


    internal async Task<IkosCashTransactionResult> SendPaymentTransaction(IkosCashTransactionPayload paymentTransaction) {
      Assertion.Require(paymentTransaction, nameof(paymentTransaction));

      await EnsureAccessTokenIsCreated();

      HttpResponseMessage response = await _httpClient.PostAsJsonAsync("recepcion/message",
                                                                  new IkosCashTransactionPayload[1] { paymentTransaction });

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();

      IkosCashTransactionResult[] result = JsonConvert.DeserializeObject<IkosCashTransactionResult[]>(jsonString);

      EnsureIkosCashSuccessResponse(result);

      return result[0];
    }

    private void EnsureIkosCashSuccessResponse(Array result) {
      Assertion.Require(result != null && result.Length > 0,
                        "Ocurrió un problema al leer el regreso de la llamada a IKosCash. La respuesta regresó vacía.");
    }

    #endregion Methods

    #region Helpers

    private async Task EnsureAccessTokenIsCreated() {

      if (_httpClient.DefaultRequestHeaders.Contains("Token")) {
        return;
      }

      var tokenClient = new IkosCashTokenApiClient();

      _httpClient.DefaultRequestHeaders.Add("Token", await tokenClient.GetToken());
    }


    private void SetHttpClientProperties() {

      _httpClient.BaseAddress = new Uri(IkosCashConstantValues.PAYMENTS_API_BASE_ADDRESS);

      _httpClient.Timeout = TimeSpan.FromSeconds(10);

      var headers = _httpClient.DefaultRequestHeaders;

      headers.Accept.Clear();

      headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    #endregion Helpers

  } // class ApiClient

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash
