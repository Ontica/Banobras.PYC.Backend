/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIGEVI Integration               Component : Adapters Layer                           *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Output Data Transfer Object              *
*  Type     : AvailableBudgetDto                        License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Returns available budget information for one or more budget accounts.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.BanobrasIntegration.Sigevi.Adapters {

  /// <summary>Returns available budget information for one or more budget accounts.</summary>
  public class AvailableBudgetDto {

    public int Year {
      get; internal set;
    }

    public int Month {
      get; internal set;
    }

    public string OrgUnitCode {
      get; internal set;
    }

    public string BudgetAccountNo {
      get; internal set;
    }

    public decimal Available {
      get; internal set;
    }

  }  // class AvailableBudgetDto

}  // namespace Empiria.BanobrasIntegration.Sigevi.Adapters
