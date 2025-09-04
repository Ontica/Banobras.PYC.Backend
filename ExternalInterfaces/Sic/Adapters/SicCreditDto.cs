/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Sic Services                               Component : Adapters Layer                          *
*  Assembly : Sic.Connector.dll                          Pattern   : Output DTO                              *
*  Type     : SicCreditDto                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return SIC credit account data according to ICreditAccountData interface.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Adapters;

namespace Empiria.BanobrasIntegration.Sic.Adapters {

  /// <summary>Output DTO used to return SIC credit account data
  /// according to ICreditAccountData interface.</summary>
  public class SicCreditDto : ICreditAccountData {

    public string AccountNo {
      get; internal set;
    }

    public string SubledgerAccountNo {
      get; internal set;
    }

    public string CustomerNo {
      get; internal set;
    }

    public string CustomerName {
      get; internal set;
    }

  }  // class SicCreditDto

} // namespace Empiria.BanobrasIntegration.Sic.Adapters
