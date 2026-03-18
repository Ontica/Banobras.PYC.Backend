/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                      Component : Domain Layer                           *
*  Assembly : Sial.Connector.dll                          Pattern  : Information Holder                      *
*  Type     : EmpleadoSIAL                                License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : Holds information about a Banobras Sial employee entry.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds information about a Banobras Sial employee entry.</summary>
  public class EmpleadoSIAL {


    [DataField("ID_USUARIO", ConvertFrom = typeof(string))]
    public string NoEmpleado {
      get; private set;
    }


    [DataField("RFC")]
    public string RfcEmpleado {
      get; private set;
    }


    [DataField("CLAVE_AREA")]
    public string NoArea {
      get; private set;
    }


    [DataField("AREA")]
    public string Area {
      get; private set;
    }


    [DataField("NOMBRE")]
    public string Nombre {
      get; private set;
    }


    [DataField("APELLIDONOMBRE")]
    public string ApellidoNombre {
      get; private set;
    }


    [DataField("CLAVE_PUESTO")]
    public string NoPuesto {
      get; private set;
    }


    [DataField("PUESTO")]
    public string Puesto {
      get; private set;
    }


    [DataField("TABULADOR")]
    public string NoTabulador {
      get; private set;
    }


    [DataField("ID_CPN", ConvertFrom = typeof(string))]
    public int IdCpn {
      get; private set;
    }


    [DataField("DESC_CPN")]
    public string Cpn {
      get; private set;
    }

    public string Auxilar {
      get {
        return GetAuxilar();
      }
    }

    #region Helpers

    private string GetAuxilar() {
      string delegation = string.Empty;
      string constValues = "00000000";
      string employeAuxCode = "90";

      if (this.NoArea[0] == '2') {
        delegation = this.NoArea.Substring(1, 2);
      } else {
        delegation = "9";
      }

      var subLeadgerAccount = delegation + constValues + employeAuxCode + this.NoEmpleado.PadLeft(6, '0');

      return subLeadgerAccount;
    }

    #endregion Helpers

  } // class EmpleadoSIAL

} // namespace Empiria.BanobrasIntegration.Sial
