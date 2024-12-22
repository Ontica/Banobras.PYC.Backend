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

namespace Empiria.Payments.BanobrasIntegration.IkosCash  {

  /// <summary>Provides constants values for Ikos Cash conector component.</summary>
  static public class IkosCashConstantValues {

    static internal string TOKEN_API_BASE_ADDRESS =
                                  ConfigurationData.GetString("IkosCash.TOKEN_API_BASE_ADDRESS");

    static internal string PAYMENTS_API_BASE_ADDRESS =
                                  ConfigurationData.GetString("IkosCash.PAYMENTS_API_BASE_ADDRESS");

    static private string PYC_ASSIGNED_CERTIFICATE_PATH =
                              ConfigurationData.GetString("IkosCash.PYC_ASSIGNED_CERTIFICATE_PATH");

    static private string PYC_ASSIGNED_CERTIFICATE_PASSWORD =
                                  ConfigurationData.GetString("IkosCash.PYC_ASSIGNED_CERTIFICATE_PASSWORD");

    static internal string PYC_ASSIGNED_PASSWORD =
                              ConfigurationData.GetString("IkosCash.PYC_ASSIGNED_PASSWORD");

    static internal string PYC_ASSIGNED_USER =
                                  ConfigurationData.GetString("IkosCash.PYC_ASSIGNED_USER");

    static internal X509Certificate2 GET_PYC_CERTIFICATE() {
      return new X509Certificate2(PYC_ASSIGNED_CERTIFICATE_PATH,
                                  PYC_ASSIGNED_CERTIFICATE_PASSWORD,
                                  X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet |
                                  X509KeyStorageFlags.Exportable);
    }

  }  // class IkosCashConstantValues

}  // namespace Empiria.Payments.BanobrasIntegration.IkosCash
