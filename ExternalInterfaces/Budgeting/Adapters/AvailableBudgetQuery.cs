/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC External Interfaces          Component : Adapters Layer                           *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Input Query                              *
*  Type     : AvailableBudgetQuery                      License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Query used to retrieve available budget information for one or more accounts.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Banobras.Budgeting.Adapters {

  public class AvailableBudgetQuery {

    public int Year {
      get; set;
    }

    public int Month {
      get; set;
    }

    public string OrgUnitCode {
      get; set;
    } = string.Empty;


    public string BudgetAccountNo {
      get; set;
    } = string.Empty;

  }  // class AvailableBudgetQuery

}  // namespace Empiria.Banobras.Budgeting.Adapters
