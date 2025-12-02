/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC External Interfaces          Component : Adapters Layer                           *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Output DTO                               *
*  Type     : OrgUnitDto                                License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Output DTO for organizational units.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Banobras.Budgeting.Adapters {

  /// <summary>Output DTO for organizational units.</summary>
  public class OrgUnitDto {

    public string Code {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

  } // class OrgUnitDto

}  // namespace Empiria.Banobras.Budgeting.Adapters
