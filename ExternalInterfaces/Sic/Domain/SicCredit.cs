/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Services                      Component : Domain Layer                            *
*  Assembly : Sic.Connector.dll                          Pattern   : Information Holder                      *
*  Type     : SicCredit                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds credit account information from Banobras SIC System.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Holds credit account information from Banobras SIC System.</summary>
  public class SicCredit {

    [DataField("NUMERO")]
    public int CreditoNo {
      get; set;
    }


    [DataField("AUXILIAR")]
    public string Auxiliar {
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


    [DataField("PFI_AREA")]
    public string Area {
      get; set;
    }


    [DataField("CLAS_TIPO_CRED")]
    public string ClaveTipoCredito {
      get; set;
    }


    [DataField("ETAPA")]
    public int EtapaCredito {
      get; set;
    }


    [DataField("CTA_REGIS")]
    public string CtaRegistro {
      get; set;
    }


    [DataField("PFI_SALDO", ConvertFrom = typeof(decimal))]
    public decimal Saldo {
      get; set;
    }


    [DataField("PFI_PLAZO_INVERSION")]
    public int PlazoInversion {
      get; set;
    }


    [DataField("PFI_PLAZO_GRACIA_INT")]
    public int PlazoGracia {
      get; set;
    }


    [DataField("PFI_PLAZO_AMORTIZACION")]
    public int PlazoAmortiazacion {
      get; set;
    }


    [DataField("PFI_FECHA_DESEMBOLSO")]
    public DateTime FechaDesembolso {
      get; set;
    }


    [DataField("PFI_TASA", ConvertFrom = typeof(short))]
    public int Tasa {
      get; set;
    }


    [DataField("PFI_FACTOR_TASA")]
    public int FactorTasa {
      get; set;
    }


    [DataField("PFI_TASA_PISO")]
    public int TasaPiso {
      get; set;
    }


    [DataField("PFI_TASA_TECHO")]
    public int TasaTecho {
      get; set;
    }


    [DataField("CON_FTE_STD")]
    public string CuentaStandard {
      get; set;
    }

  } // class SicCredit

} // namespace Empiria.BanobrasIntegration.Sic
