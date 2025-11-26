/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC External Interfaces          Component : Adapters Layer                           *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Input Fields DTO                         *
*  Type     : ExternalBudgetRequestFields               License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Input fields DTO to request budget for one or more accounts.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Banobras.Budgeting.Adapters {

  /// <summary>Input fields DTO to request budget for one or more accounts.</summary>
  public class ExternalBudgetRequestFields {

    public string OrgUnitCode {
      get; set;
    } = string.Empty;


    public string RequisitionNo {
      get; set;
    } = string.Empty;


    public int Year {
      get; set;
    }

    public DateTime StartDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public DateTime EndDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public string ExternalControlID {
      get; set;
    } = string.Empty;


    public object Attributes {
      get; set;
    }


    public BudgetEntryFields[] BudgetEntries {
      get; set;
    } = new BudgetEntryFields[0];

  }  // class ExternalBudgetRequestFields


  public class BudgetEntryFields {

    public string BudgetAccountNo {
      get; set;
    } = string.Empty;


    public int Month {
      get; set;
    }

    public decimal Amount {
      get; set;
    }

    public string ExternalEntryControlID {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;

  }  // class BudgetEntryFields

}  // namespace Empiria.Banobras.Budgeting.Adapters
