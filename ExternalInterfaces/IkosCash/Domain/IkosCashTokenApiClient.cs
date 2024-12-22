/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : ApiClientToken                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Client to get token from Ikos cash web services.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;

using System.Security.Cryptography;
using System.Text;

using Newtonsoft.Json;

using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {

  /// <summary>Client to get token from Ikos cash web services.</summary>
  internal class IkosCashTokenApiClient {

    #region Fields

    private readonly HttpClient _httpClient = new HttpClient();

    #endregion Fields

    internal IkosCashTokenApiClient() {
      SetHttpClientProperties();
    }

    #region Methods

    internal async Task<string> GetToken() {

      var fields = new AuthenticateFields {
        Clave = IkosCashConstantValues.PYC_ASSIGNED_USER,
        Llave = GetSha256Hash(IkosCashConstantValues.PYC_ASSIGNED_PASSWORD)
      };

      HttpResponseMessage response = await _httpClient.PostAsJsonAsync("autenticar", fields);

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();
      var authenticate = JsonConvert.DeserializeObject<AuthenticateDto>(jsonString);

      return authenticate.Token;
    }


    #endregion Methods

    #region Helpers

    private string GetSha256Hash(string rawData) {

      using (SHA256 sha256Hash = SHA256.Create()) {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        return Convert.ToBase64String(bytes);
      }
    }


    private void SetHttpClientProperties() {
      _httpClient.BaseAddress = new Uri(IkosCashConstantValues.TOKEN_API_BASE_ADDRESS);

      var headers = _httpClient.DefaultRequestHeaders;

      headers.Accept.Clear();
      headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    #endregion Helpers


  } // class ApiClient

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash
