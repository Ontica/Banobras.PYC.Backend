/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                      Component : Domain Layer                           *
*  Assembly : Sial.Connector.dll                          Pattern  : Information Holder                      *
*  Type     : AreaSIAL                                    License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : Contiene información sobre un área organizacional en el sistema SIAL de Banobras.              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.BanobrasIntegration.Sial.Data;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Contiene información sobre un área organizacional en el sistema SIAL de Banobras.</summary>
  public class AreaSIAL {

    static private Lazy<FixedList<AreaSIAL>> _entries =
                new Lazy<FixedList<AreaSIAL>>(() => SialDataService.GetAreasSIAL());

    #region Constructors and parsers

    protected AreaSIAL() {
      // Require by Empiria FrameWork
    }

    static public FixedList<AreaSIAL> GetListaAreas() {
      return _entries.Value;
    }

    static public AreaSIAL TryGetArea(string claveArea) {
      claveArea = EmpiriaString.Clean(claveArea);

      if (string.IsNullOrWhiteSpace(claveArea)) {
        return null;
      }

      return _entries.Value.Find(x => x.Clave == claveArea);
    }


    static public AreaSIAL TryGetAreaSuperiorSIAL(string claveArea) {
      Assertion.Require(claveArea, "Requiero un número de área");

      AreaSIAL areaSIAL = TryGetArea(claveArea);

      Assertion.Require(areaSIAL, $"El área {claveArea} no existe en SIAL.");

      return TryGetArea(areaSIAL.ClaveAreaSuperior);
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("CLAVE_AREA")]
    public string Clave {
      get; private set;
    }


    [DataField("AREA")]
    public string Nombre {
      get; private set;
    }


    [DataField("CLAVE_AREA_SUP")]
    private string _claveAreaSuperior = string.Empty;

    public string ClaveAreaSuperior {
      get {
        if (_claveAreaSuperior == "162111") {
          return "162100";
        }
        return _claveAreaSuperior;
      }
    }


    [DataField("AREA_SUP")]
    public string NombreAreaSuperior {
      get; private set;
    }


    public EmpleadoSIAL Responsable {
      get {
        return GetResponsable();
      }
    }


    #endregion Properties

    #region Helpers

    private EmpleadoSIAL GetResponsable() {
      string areaSIAL = this.Clave;

      while (true) {
        var responsible = SialDataService.GetResponsableArea(areaSIAL);

        if (responsible != null) {
          return responsible;
        }

        areaSIAL = AreaSIAL.TryGetAreaSuperiorSIAL(areaSIAL).Clave;
      }
    }

    #endregion Helpers

  } // class AreaSIAL

} // namespace Empiria.BanobrasIntegration.Sial
