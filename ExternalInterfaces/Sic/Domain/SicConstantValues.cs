/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Tests Helpers                           *
*  Assembly : Sic.Connector.dll                          Pattern   : Testing constants                       *
*  Type     : SicConstantValues                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides constants values for sic conector component.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Provides constants values for sic conector component.</summary>
  static public class SicConstantValues {

    static private readonly JsonObject _config;

    static SicConstantValues() {
      _config = ConfigurationData.Get<JsonObject>("Sic.Pyc");
    }


    static internal string SIC_API_BASE_ADDRESS => _config.Get<string>("baseAddress");

 
  }  // class SicConstantValues

}  // namespace Empiria.BanobrasIntegration.Sic
