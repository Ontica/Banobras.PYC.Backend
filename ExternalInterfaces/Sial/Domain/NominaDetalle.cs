/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Information Holder                      *
*  Type     : NominaDetalle                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds payroll data movements.                                                                  *          
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds payroll data movements.</summary>
  public class NominaDetalle {

    [DataField("BGM_NUM_VOL")]
    public int NoNomina {
      get; set;
    }

    [DataField("BGM_FECHA_VOL")]
    public DateTime Fecha {
      get; set;
    }

    [DataField("BGM_AREA")]
    public string Area {
      get; set;
    }

    [DataField("BGM_CONCEPTO")]
    public int Concepto {
      get; set;
    }

    [DataField("BGM_REG_CONTABLE")]
    public string Cuenta {
      get; set;
    }

    [DataField("BGM_SECTOR")]
    public string Sector {
      get; set;
    }

    [DataField("BGM_NUM_AUX")]
    public string Auxiliar {
      get; set;
    }

    [DataField("BGM_CVE_MOV")]
    public int TipoMovimiento {
      get; set;
    }

    [DataField("BGM_IMPORTE", ConvertFrom = typeof(decimal))]
    public decimal Importe {
      get; set;
    }


  } // class NominaDetalle

} // namespace Empiria.BanobrasIntegration.Sial
