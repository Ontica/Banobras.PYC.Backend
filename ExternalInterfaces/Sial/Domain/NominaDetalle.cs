/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Information Holder                      *
*  Type     : NominaDetalle                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds payroll data movements.                                                                  *          
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using Empiria.Financial.Integration;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds payroll data movements.</summary>
  public class NominaDetalle {

    [DataField("BGM_NUM_VOL")]
    public int NoNomina {
      get; private set;
    }


    [DataField("BGM_FOLIO_VOL")]
    public int Folio {
      get; private set;
    }


    [DataField("BGM_AREA")]
    public string Area {
      get; private set;
    }


    [DataField("BGM_REG_CONTABLE")]
    private string _cuentaContable = string.Empty;

    public string CuentaContable {
      get {
        return IntegrationLibrary.FormatAccountNumber(_cuentaContable);
      }
    }


    [DataField("BGM_CVE_MOV", ConvertFrom = typeof(int))]
    public int TipoMovimiento {
      get; private set;
    }


    [DataField("BGM_IMPORTE", ConvertFrom = typeof(decimal))]
    public decimal Importe {
      get; private set;
    }


    public string NoOperacion {
      get {
        return $"{NoNomina:D8}-{Folio:D4}";
      }
    }

  } // class NominaDetalle

} // namespace Empiria.BanobrasIntegration.Sial
