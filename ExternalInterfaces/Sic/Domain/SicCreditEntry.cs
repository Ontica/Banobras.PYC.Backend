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
using Empiria.BanobrasIntegration.Sic.Adapters;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Holds information about a Banobras SIC credit entry.</summary>
  public class SicCreditEntry {

    public int NoCredito {
      get; private set;
    }


    public string Auxiliar {
      get; private set;
    } = string.Empty;


    public DateTime FechaProceso {
      get; private set;
    }


    public string NumConcepto {
      get; private set;
    } = String.Empty;


    public string NombreConcepto {
      get; private set;
    }


    //[DataField("IMPORTE", ConvertFrom = typeof(decimal))]
    public decimal Importe {
      get; private set;
    }

    static internal SicCreditEntry MapToSicCreditEntry(MovtosDetalleDto movimiento, int idCredito) {

      return new SicCreditEntry {
        NoCredito = idCredito,
        FechaProceso = Convert.ToDateTime(movimiento.Fecha),
        NombreConcepto = movimiento.Concepto,
        Importe = movimiento.Importe,
      };

    }

  } // class SicCreditEntry

} // namespace Empiria.BanobrasIntegration.Sic
