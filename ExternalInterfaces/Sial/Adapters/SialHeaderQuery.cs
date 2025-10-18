/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Sial Services                     Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces            Pattern   : Output DTO                              *
*  Type     : SialHeaderQuery                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query used to return SIAL payroll data.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.StateEnums;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Input query used to return SIAL payroll data.</summary>
  public class SialHeaderQuery {

    public int PayrollNo {
      get; set;
    }

    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;

    public DateTime FromDate {
      get; set;
    }

    public DateTime ToDate {
      get; set;
    }

  }  // class SialHeaderQuery

} // namespace Empiria.BanobrasIntegration.Sial.Adapters
