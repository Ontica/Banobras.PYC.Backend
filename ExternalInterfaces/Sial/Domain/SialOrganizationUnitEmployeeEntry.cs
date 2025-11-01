/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Sial.Connector.dll                          Pattern  : Information Holder                      *
*  Type     : SialOrganizationUnitEmployeeEntry          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a Banobras Sial organization unit employee entry.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds information about a Banobras Sial organization unit employee entry.</summary>
  public class SialOrganizationUnitEmployeeEntry {

    
    [DataField("ID_USUARIO", ConvertFrom = typeof(string))]
    public string NoEmpleado {
      get; private set;
    } 

    [DataField("CLAVE_AREA")]
    public string NoArea {
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

        
  } // class SialOrganizationUnitEmployeeEntry

} // namespace Empiria.BanobrasIntegration.Sial
