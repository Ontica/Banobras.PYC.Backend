/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces            Pattern   : Output DTO                              *
*  Type     : SialEmployeeDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a SIAL employee.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Output DTO used to return a SIAL employee.</summary>
  public class SialEmployeeDto {

    public string Code {
      get; internal set;
    }


    public string RFC {
      get; internal set;
    }


    public string Name {
      get; internal set;
    }


    public string OrganizationUnitCode {
      get; internal set;
    }


    public string OrganizationUnit {
      get; internal set;
    }


    public string PositionCode {
      get; internal set;
    }


    public string Position {
      get; internal set;
    }


    public string Category {
      get; internal set;
    }


    public int CpnCode {
      get; internal set;
    }


    public string Cpn {
      get; internal set;
    }


    public string SubLeadgerAccount {
      get; internal set;
    }

  }  // class SialEmployeeDto

} // namespace Empiria.BanobrasIntegration.Sial.Adapters
