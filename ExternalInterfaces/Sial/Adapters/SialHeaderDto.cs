/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Sial Services                     Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces            Pattern   : Output DTO                              *
*  Type     : SialHeaderDto                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return SIAL payroll data                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Financial;
using Empiria.Financial.Adapters;
using Empiria.Parties;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Output DTO used to return SIAL payroll data</summary>
  public class SialHeaderDto {

    public int PayrollNo {
      get; internal set;
    }

    public DateTime PayrollDate {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

  }  // class SialHeaderDto

} // namespace Empiria.BanobrasIntegration.Sial.Adapters
