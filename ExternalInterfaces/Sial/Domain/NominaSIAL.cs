/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Information Holder                      *
*  Type     : NominaSIAL                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Representa una nómina en el sistema SIAL de Banobras.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.BanobrasIntegration.Sial.Data;
using Empiria.StateEnums;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Representa una nómina en el sistema SIAL de Banobras.</summary>
  public class NominaSIAL {

    public string UID {
      get {
        return NumeroNomina.ToString();
      }
    }


    [DataField("BGME_NUM_VOL")]
    public int NumeroNomina {
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

    public FixedList<EntradaNominaSIAL> Entradas() {
      return SialDataService.GetPayrollEntries(NumeroNomina);
    }

  } // class NominaSIAL

} // namespace Empiria.BanobrasIntegration.Sial
