/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces            Pattern   : Output DTO                              *
*  Type     : SialPayrollsQuery                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query used to return SIAL payrolls.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.StateEnums;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Input query used to search SIAL payrolls.</summary>
  public class SialPayrollsQuery {

    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.Pending;

  }  // class SialPayrollsQuery

} // namespace Empiria.BanobrasIntegration.Sial.Adapters
