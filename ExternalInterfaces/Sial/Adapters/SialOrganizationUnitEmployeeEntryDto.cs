/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces            Pattern   : Output DTO                              *
*  Type     : SialOrganizationUnitEmployeeEntryDto       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a SIAL organization unit employee.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Adapters;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Output DTO used to return a SIAL organization unit employee.</summary>
  public class SialOrganizationUnitEmployeeEntryDto : ISialOrganizationUnitEmployeesData {

    public string EmployeeNo {
      get; internal set;
    }

    public string FedTaxpayersReg {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string LastName {
      get; internal set;
    }

    public string OrganizationUnitNo {
         get; internal set;
    }

    public string JobNo {
      get; internal set;
    }

    public string JobTitle {
      get; internal set;
    }

    public string JobCategoryNo {
      get; internal set;
    }


  }  // class SialOrganizationUnitEmployeeEntryDto

} // namespace Empiria.BanobrasIntegration.Sial.Adapters
