/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Sial Services                     Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces            Pattern   : Output DTO                              *
*  Type     : SialDetailDto                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return SIAL payroll data detail                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Financial;
using Empiria.Financial.Adapters;
using Empiria.Parties;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Output DTO used to return SIAL payroll data detail</summary>
  public class SialDetailDto {

    public int PayrollNo {
      get; internal set;
    }

    public DateTime PayrollDate {
      get; internal set;
    }

    public string AreaNo {
      get; internal set;
    }

    public int ConceptNo {
      get; internal set;
    }

    public string AccountNo {
      get; internal set;
    }

    public string SectorNo {
      get; internal set;
    }

    public string SubledgerAccountNo {
      get; internal set;
    }

    public int TypeMovement{
      get; internal set;
    }

    public decimal Amount {
      get; internal set;
    }

  }  // class SialDetailDto

} // namespace Empiria.BanobrasIntegration.Sial.Adapters
