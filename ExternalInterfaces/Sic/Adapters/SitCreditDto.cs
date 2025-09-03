/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Sic Services                               Component : Integration Layer                       *
*  Assembly : Sic.Connector.dll                          Pattern   : Data Transfer Object                    *
*  Type     : SitCreditDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data structure with credit data returned.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.BanobrasIntegration.Sic.Adapters {

  /// <summary>Data structure with credit data returned.</summary>
  public class SitCreditDto {

    public int CreditNo {
      get; set;
    }


    public string Auxiliar {
      get; set;
    } = string.Empty;


    public int ClientNo {
      get; set;
    }


    public string ClientName {
      get; set;
    } = string.Empty;

  }  // class SitCreditDto

} // namespace Empiria.BanobrasIntegration.Sic.Adapters
