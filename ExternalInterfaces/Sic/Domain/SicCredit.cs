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
    public string CreditoNo {
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


    [DataField("MONEDA", ConvertFrom = typeof(int))]
    public int Moneda {
      get; set;
    } = 0;


    [DataField("MONEDA_DES")]
    public string MonedaDes {
      get; set;
    }


    [DataField("RFC")]
    public string Rfc {
      get; set;
    }


    [DataField("RFC_CONSE", ConvertFrom = typeof(int))]
    public int RfcConsecutivo {
      get; set;
    }


    [DataField("TIPO_CLIENTE")]
    public string TipoCliente {
      get; set;
    }


    [DataField("NOMBRE_CLIENTE")]
    public string NombreCliente {
      get; set;
    }


    [DataField("AREA_PROM")]
    public string AreaPromocion {
      get; set;
    }


    [DataField("AREA_PROM_DES")]
    public string AreaDescripcion {
      get; set;
    }


    [DataField("CLASIF_CRE", ConvertFrom = typeof(short))]
    public int ClasifCredito {
      get; set;
    } = 0;


    [DataField("CLASIF_CRE_DES")]
    public string ClasifCreditoDes {
      get; set;
    }


    [DataField("ETAPA", ConvertFrom = typeof(int))]
    public int EtapaCredito {
      get; set;
    } = 0;


    [DataField("ETAPA_DES")]
    public string EtapaCreditoDes {
      get; set;
    }


    [DataField("CTA_REGIS")]
    public string CtaRegistro {
      get; set;
    }


    [DataField("NUM_PASIVO")]
    public string CreditoPasivoNo {
      get; set;
    }


    [DataField("F_MAX_DISP", ConvertFrom = typeof(DateTime))]
    public DateTime FecMaxDisposicion {
      get; set;
    }


    [DataField("F_MAX_REFI", ConvertFrom = typeof(DateTime))]
    public DateTime FechaMaxRefinanciamiento {
      get; set;
    }


    [DataField("NUM_LINEA", ConvertFrom = typeof(short))]
    public int LineaCreditoNo {
      get; set;
    } = 0;


    [DataField("MON_NET_FIN", ConvertFrom = typeof(decimal))]
    public decimal MontoNetoFinanciar {
      get; set;
    }


    [DataField("TIPO_OBRA", ConvertFrom = typeof(short))]
    public int TipoObra {
      get; set;
    }


    [DataField("TIPO_OBRA_DES")]
    public string TipoObraDescripcion {
      get; set;
    }


    [DataField("MONTO_OBRA", ConvertFrom = typeof(decimal))]
    public decimal MontoObra {
      get; set;
    } = 0;


    [DataField("IMP_CRED", ConvertFrom = typeof(decimal))]
    public decimal MontoCredito {
      get; set;
    }


    [DataField("PFI_SALDO", ConvertFrom = typeof(decimal))]
    public decimal Saldo {
      get; set;
    }


    [DataField("PFI_PLAZO_INVERSION", ConvertFrom = typeof(short))]
    public int PlazoInversion {
      get; set;
    }


    [DataField("PFI_PLAZO_GRACIA_INT", ConvertFrom = typeof(short))]
    public int PlazoGraciaInt {
      get; set;
    }


    [DataField("PFI_PLAZO_AMORTIZACION", ConvertFrom = typeof(short))]
    public int PlazoAmortizacion {
      get; set;
    }


    [DataField("PFI_FECHA_AMORTIZACION", ConvertFrom = typeof(DateTime))]
    public DateTime FechaAmortizacion {
      get; set;
    }


    [DataField("PFI_PLAZO_DESEMBOLSO", ConvertFrom = typeof(short))]
    public int PlazoDesembolso {
      get; set;
    }

    [DataField("PFI_FECHA_DESEMBOLSO", ConvertFrom = typeof(DateTime))]
    public DateTime FechaDesembolso {
      get; set;
    }


    [DataField("PFI_TASA", ConvertFrom = typeof(short))]
    public int Tasa {
      get; set;
    }


    [DataField("PFI_FACTOR_TASA", ConvertFrom = typeof(short))]
    public int FactorTasa {
      get; set;
    }


    [DataField("PFI_TASA_PISO", ConvertFrom = typeof(short))]
    public int TasaPiso {
      get; set;
    }


    [DataField("PFI_TASA_TECHO", ConvertFrom = typeof(short))]
    public int TasaTecho {
      get; set;
    }


  } // class SicCredit

} // namespace Empiria.BanobrasIntegration.Sic
