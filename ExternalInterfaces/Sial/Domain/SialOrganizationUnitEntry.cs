/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Sial.Connector.dll                          Pattern  : Information Holder                      *
*  Type     : SialOrganizationUnitEntry                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a Banobras Sial organization unit entry.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.BanobrasIntegration.Sial.Data;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds information about a Banobras Sial organization unit entry.</summary>
  public class SialOrganizationUnitEntry {

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
    static public SialOrganizationUnitEntry GetOrganizationParent(string orgUnitId) {

      SialOrganizationUnitEntry orgUnit = SialDataService.GetOrganizationParent(orgUnitId);

      Assertion.Require(orgUnit, $"El área proporcionada {orgUnitId}, no existe");

      if (orgUnit.NoArea != string.Empty && orgUnit.NoAreaSupervision == string.Empty) {
        orgUnit.NoAreaSupervision = "Empty";
        orgUnit.AreaSupervision = "Empty";
      }

      return orgUnit;
    }

    #endregion Methods


  } // class SialOrganizationUnitEntry

} // namespace Empiria.BanobrasIntegration.Sial
