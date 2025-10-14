/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Information Holder                      *
*  Type     : NominaDetalleEntry                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a Banobras SIAL payroll movements entry.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds information about a Banobras SIAL payroll movemenst entry.</summary>
  public class NominaDetalleEntry {

    [DataField("BGM_NUM_VOL")]
    public int NoNomina {
      get; set;
    }


    [DataField("BGM_FECHA_VOL")]
    public DateTime Fecha {
      get; set;
    }

    [DataField("BGM_REG_CONTABLE")]
    public string Cuenta {
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

    
    [DataField("BGM_CVE_MOV", ConvertFrom = typeof(int))]
    public int TipoMovimiento {
      get; set;
    }


    [DataField("BGM_IMPORTE", ConvertFrom = typeof(decimal))]
    public decimal Importe {
      get; set;
    }


  } // class NominaDetalleEntry

} // namespace Empiria.BanobrasIntegration.Sial
