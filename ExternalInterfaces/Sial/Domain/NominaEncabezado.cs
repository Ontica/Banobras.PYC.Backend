/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Information Holder                      *
*  Type     : NominaEncabezado                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds payroll data.                                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds payroll data.</summary>
  public class NominaEncabezado {


    public string UID {
      get {
        return NoNomina.ToString();
      }
    }


    [DataField("BGME_NUM_VOL")]
    public int NoNomina {
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

    [DataField("BGME_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; set;
    }

  } // class NominaEncabezado

} // namespace Empiria.BanobrasIntegration.Sial
