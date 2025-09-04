/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Sic.Connector.dll                          Pattern   : Data Transfer Object                    *
*  Type     : SicCredit                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to get credits for SIC.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>DTO used to get credits for SIC.</summary>
  public class SicCredit {

    [DataField("NUMERO")]
    public int CreditoNo {
      get; set;
    }


    [DataField("AUXILIAR")]
    public string Auxiliar {
      get; set;
    }


    [DataField("NUM_CLIENTE")]
    public int ClienteNo {
      get; set;
    }


    [DataField("TBASE_INTE")]
    public string TasaBaseInt {
      get; set;
    }


    [DataField("TBASE_MORA")]
    public string TasaBaseIntMor {
      get; set;
    }


    [DataField("NUMANT")]
    public string CreditoAnterior {
      get; set;
    }


    [DataField("MONEDA")]
    public int Moneda {
      get; set;
    }


    [DataField("RFC")]
    public string Rfc {
      get; set;
    }


    [DataField("TIPO_CLIENTE")]
    public string TipoCliente {
      get; set;
    }


    [DataField("CLIENTE")]
    public string NombreCliente {
      get; set;
    }

  } // class SicCredit

} // namespace Empiria.BanobrasIntegration.Sic.Adapters
