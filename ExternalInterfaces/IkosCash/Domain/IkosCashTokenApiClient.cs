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
using Newtonsoft.Json;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {
  /// <summary>Client to get token from Ikos cash web services.</summary>
  internal class IkosCashTokenApiClient {

    #region Global Variables

    private readonly HttpClient client = new HttpClient();
    private readonly string baseAddress = "https://BNOTCASHNET-B.banobras.gob.mx:7111/api/";

    #endregion Global Variables

    internal IkosCashTokenApiClient() {
      SetHttpClientProperties();
    }

    #region Internal Methods

    internal async Task<string> GetToken() {

      AuthenticateFields fields = new AuthenticateFields {
        Clave = "BUSSIMEFIN(PYC)",
        Llave = "bussimefin20230613#"//ConfigurationData.Get<string>("PYC.KEY")
      };

      fields.Llave = GetSha256Hash(fields.Llave);

      HttpResponseMessage response = await client.PostAsJsonAsync("autenticar", fields);

      response.EnsureSuccessStatusCode();

      var jsonString = await response.Content.ReadAsStringAsync();
      var authenticate = JsonConvert.DeserializeObject<AuthenticateDto>(jsonString);
      
      return authenticate.Token;
    }


    #endregion Internal Methods

    #region Helpers

    private string GetSha256Hash(string rawData) {
      using (SHA256 sha256Hash = SHA256.Create()) {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return  Convert.ToBase64String(bytes);
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
