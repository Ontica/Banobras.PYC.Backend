/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces            Pattern   : Output DTO                              *
*  Type     : SialPayrollDto                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return a SIAL payroll.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Output DTO used to return SIAL payroll.</summary>
  public class SialPayrollDto {

    public string UID {
      get; internal set;
    }

    public string PayrollNo {
      get; internal set;
    }

    public DateTime PayrollDate {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // class SialPayrollDto

} // namespace Empiria.BanobrasIntegration.Sial.Adapters
