/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Tests Helpers                           *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Testing constants                       *
*  Type     : IkosCashConstantValues                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides constants values for Ikos Cash conector component.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Security.Cryptography.X509Certificates;

using Empiria.Json;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {

  /// <summary>Provides constants values for Ikos Cash conector component.</summary>
  static public class IkosCashConstantValues {

    static private readonly JsonObject _config;

    static IkosCashConstantValues() {
      _config = ConfigurationData.Get<JsonObject>("IkosCash.PYC");
    }


    static internal string TOKEN_API_BASE_ADDRESS => _config.Get<string>("TOKEN_API_BASE_ADDRESS");

    static internal string PAYMENTS_API_BASE_ADDRESS => _config.Get<string>("PAYMENTS_API_BASE_ADDRESS");

    static private string PYC_ASSIGNED_CERTIFICATE_PATH =>
                                              _config.Get<string>("PYC_ASSIGNED_CERTIFICATE_PATH");

    static private string PYC_ASSIGNED_CERTIFICATE_PASSWORD =>
                                              _config.Get<string>("PYC_ASSIGNED_CERTIFICATE_PASSWORD");

    static internal string PYC_ASSIGNED_PASSWORD => _config.Get<string>("PYC_ASSIGNED_PASSWORD");

    static internal string PYC_ASSIGNED_USER => _config.Get<string>("PYC_ASSIGNED_USER");

    static internal int TRANSACTION_ID_USUARIO => _config.Get<int>("TRANSACTION_ID_USUARIO");

    static internal string TRANSACTION_ORIGEN => _config.Get<string>("TRANSACTION_ORIGEN");

    static internal string SERIE_FIRMA => _config.Get<string>("SERIE_FIRMA");


    static internal X509Certificate2 GET_PYC_CERTIFICATE() {
      return new X509Certificate2(PYC_ASSIGNED_CERTIFICATE_PATH,
                                  PYC_ASSIGNED_CERTIFICATE_PASSWORD,
                                  X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet |
                                  X509KeyStorageFlags.Exportable);
    }

  }  // class IkosCashConstantValues

}  // namespace Empiria.Payments.BanobrasIntegration.IkosCash
