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

using Empiria.BanobrasIntegration.Sial.Data;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds information about a Banobras Sial organization unit entry.</summary>
  public class SialOrganizationUnitEntry {

    static private Lazy<FixedList<SialOrganizationUnitEntry>> _entries =
                new Lazy<FixedList<SialOrganizationUnitEntry>>(() => SialDataService.GetOrganizationUnitEntries());

    #region Constructors and parsers

    protected SialOrganizationUnitEntry() {
      // Require by Empiria FrameWork
    }

    #endregion Constructors and parsers

    #region Properties

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

    #endregion Properties

    #region Methods

    static public SialOrganizationUnitEntry TryGetOrganization(string noArea) {
      noArea = EmpiriaString.Clean(noArea);

      if (string.IsNullOrWhiteSpace(noArea)) {
        return null;
      }

      return _entries.Value.Find(x => x.NoArea == noArea);
    }

    #endregion Methods

  } // class SialOrganizationUnitEntry

} // namespace Empiria.BanobrasIntegration.Sial
