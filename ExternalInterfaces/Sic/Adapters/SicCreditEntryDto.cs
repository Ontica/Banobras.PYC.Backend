/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Services                      Component : Adapters Layer                          *
*  Assembly : Sic.Connector.dll                          Pattern   : Output DTO                              *
*  Type     : SicCreditEntryDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for SIC credit entry according to ICreditEntryData interface.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial.Adapters;

namespace Empiria.BanobrasIntegration.Sic.Adapters {

  /// <summary>Output DTO for SIC credit entry data according to ICreditEntryData interface.</summary>
  public class SicCreditEntryDto : ICreditEntryData {

    public string AccountNo {
      get; internal set;
    }

    public string SubledgerAccountNo {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public string OperationTypeNo {
      get; internal set;
    }

    public string OperationName {
      get; internal set;
    }

    public decimal Amount {
      get; internal set;
    }

  }  // class SicCreditEntryDto

} // namespace Empiria.BanobrasIntegration.Sic.Adapters
