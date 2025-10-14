/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Information Holder                      *
*  Type     : NominaEncabezadoEntry                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a Banobras SIAL payroll entry.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds information about a Banobras SIAL payroll entry.</summary>
  public class NominaEncabezadoEntry {

    [DataField("BGME_NUM_VOL")]
    public string NoNomina {
      get; set;
    }


    [DataField("BGME_FECHA_VOL")]
    public DateTime Fecha {
      get; set;
    }


    [DataField("BGME_DESCRIP")]
    public string Descripcion {
      get; set;
    }

  } // class NominaEncabezadoEntry

} // namespace Empiria.BanobrasIntegration.Sial
