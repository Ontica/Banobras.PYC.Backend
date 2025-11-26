/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC External Interfaces          Component : Adapters Layer                           *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Output Data Transfer Object              *
*  Type     : ExternalBudgetRequestDto                  License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Returns external budget request information for one or more budget accounts.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Banobras.Budgeting.Adapters {

  /// <summary>Returns external budget request information for one or more budget accounts.</summary>
  public class ExternalBudgetRequestDto {

    public string TransactionNo {
      get; internal set;
    }

    public string RequisitionNo {
      get; internal set;
    }

    public string OrgUnitCode {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Justification {
      get; internal set;
    }

    public DateTime AuthorizationDate {
      get; internal set;
    }

    public string AuthorizedBy {
      get; internal set;
    }

    public string ExternalControlID {
      get; internal set;
    }

    public dynamic Attributes {
      get; internal set;
    }

    public FixedList<ExternalBudgetRequestEntryDto> BudgetEntries {
      get; internal set;
    }

  }  // class ExternalBudgetRequestDto



  public class ExternalBudgetRequestEntryDto {

    public string BudgetAccountNo {
      get; internal set;
    }

    public int Month {
      get; internal set;
    }

    public decimal Amount {
      get; internal set;
    }

    public string ControlID {
      get; internal set;
    }

    public string ExternalControlEntryID {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Justification {
      get; internal set;
    }

  }  // class ExternalBudgetRequestEntryDto

}  // namespace Empiria.Banobras.Budgeting.Adapters
