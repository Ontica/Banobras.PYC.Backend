/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces            Pattern   : Output DTO                              *
*  Type     : OrganizationUnitEntryDto                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a SIAL organization unit.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Adapters;
using Empiria.Parties;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Output DTO used to return a SIAL organization unit.</summary>
  public class SialOrganizationUnitEntryDto : ISialOrganizationUnitData {
     
    public string OrganizationUnitNo {
      get; internal set;
    }

    public string OrganizationUnitName {
         get; internal set;
    }

    public string SupervisingUnitNo {  
      get; internal set;
    }

    public string SupervisingUnitName {
      get; internal set;
    }

    public int HierarchicalLevel {
      get; internal set;
    }

  }  // class OrganizationUnitEntryDto

} // namespace Empiria.BanobrasIntegration.Sial.Adapters
