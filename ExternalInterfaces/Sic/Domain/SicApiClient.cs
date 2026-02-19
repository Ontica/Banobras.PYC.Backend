/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Sic.Connector.dll                          Pattern   : Data Transfer Object                    *
*  Type     : SicApiClient                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Client to consume sic web services.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Empiria.BanobrasIntegration.Sic.Adapters;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Client to consume sic web services.</summary>
  internal class SicApiClient {

    #region Fields

    private readonly HttpClient _httpClient = new HttpClient();

    #endregion Fields

    internal SicApiClient() {
      SetHttpClientProperties();
    }

    #region Methods

    internal async Task<FixedList<MovtosDetalleDto>> TryGetMovtosDetalle(MovtosEncabezadosDto body) {

      HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"movContables/getDetalleMovContables/", body);

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();

      jsonString = jsonString.Trim('"');

      return JsonConvert.DeserializeObject<FixedList<MovtosDetalleDto>>(jsonString);
    }

    #endregion Methods

    #region Helpers

    private void SetHttpClientProperties() {

      _httpClient.BaseAddress = new Uri(SicConstantValues.SIC_API_BASE_ADDRESS);

      _httpClient.Timeout = TimeSpan.FromSeconds(120);

      var headers = _httpClient.DefaultRequestHeaders;

      headers.Accept.Clear();

      headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    #endregion Helpers

  } // class SicApiClient

} // namespace Empiria.BanobrasIntegration.Sic
