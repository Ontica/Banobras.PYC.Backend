/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Sial.Connector.dll                          Pattern  : Information Holder                      *
*  Type     : SialOrganizationUnitEntry                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a Banobras Sial organization unit entry.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds information about a Banobras Sial organization unit entry.</summary>
  public class SialOrganizationUnitEntry {

    [DataField("CLAVE_AREA")]
    public string NoArea {
      get; private set;
    }


    [DataField("AREA")]
    public string Descripcion {
      get; private set;
    }


    [DataField("CLAVE_AREA_SUP")]
    public string NoAreaSupervision {
      get; private set;
    }


    [DataField("AREA_SUP")]
    public string AreaSupervision {
      get; private set;
    }


    [DataField("NIVEL_CLAVE", ConvertFrom = typeof(int))]
    public int noNivel {
      get; private set;
    }


  } // class SialOrganizationUnitEntry

} // namespace Empiria.BanobrasIntegration.Sial
