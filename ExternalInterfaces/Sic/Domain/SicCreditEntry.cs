/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Services                      Component : Domain Layer                            *
*  Assembly : Sic.Connector.dll                          Pattern   : Information Holder                      *
*  Type     : SicCreditEntry                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a Banobras SIC credit entry.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Holds information about a Banobras SIC credit entry.</summary>
  public class SicCreditEntry {

    [DataField("NUMERO")]
    public int NoCredito {
      get; private set;
    }


    [DataField("AUXILIAR")]
    public string Auxiliar {
      get; private set;
    }


    [DataField("FEC_PROCESO")]
    public DateTime FechaProceso {
      get; private set;
    }


    [DataField("NUM_CONCEPTO")]
    public string NumConcepto {
      get; private set;
    }


    [DataField("CONCEPTO")]
    public string NombreConcepto {
      get; private set;
    }


    [DataField("IMPORTE", ConvertFrom = typeof(decimal))]
    public decimal Importe {
      get; private set;
    }

  } // class SicCreditEntry

} // namespace Empiria.BanobrasIntegration.Sic
