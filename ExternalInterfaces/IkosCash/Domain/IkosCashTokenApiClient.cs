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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Security.Cryptography;
using System.Text;

using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {
  /// <summary>Client to get token from Ikos cash web services.</summary>
  internal class IkosCashTokenApiClient {

    #region Global Variables

    private readonly HttpClient client = new HttpClient();
    private readonly string baseAddress = "http://localhost:4111/api/";

    #endregion Global Variables

    internal IkosCashTokenApiClient() {
      SetHttpClientProperties();
    }

    #region Internal Methods

    internal async Task<string> GetToken() {

      AuthenticateFields fields = new AuthenticateFields {
        Clave = "",
        Llave = "",
      };

      fields.Llave = GetSha256Hash(fields.Llave);

      HttpResponseMessage response = await client.PostAsJsonAsync("autenticar", fields);

      response.EnsureSuccessStatusCode();

      var authenticate = await response.Content.ReadAsAsync<AuthenticateDto>();

      return authenticate.Token;
    }


    #endregion Internal Methods

    #region Helpers

    private string GetSha256Hash(string rawData) {
      using (SHA256 sha256Hash = SHA256.Create()) {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        string s = Convert.ToBase64String(bytes);

        return s;
      }
    }


    private void SetHttpClientProperties() {
      client.BaseAddress = new Uri(baseAddress);
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
    }

    #endregion Helpers


  } // class ApiClient

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash
