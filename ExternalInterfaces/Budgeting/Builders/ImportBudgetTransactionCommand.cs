/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting External Interfaces       Component : Adapters Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Command                               *
*  Type     : ImportBudgetTransactionCommand               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Command data used to import budget transactions.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Banobras.Budgeting {

  /// <summary>Command data used to import budget transactions.</summary>
  public class ImportBudgetTransactionCommand {

    public bool DryRun {
      get; set;
    } = true;


    public string TransactionTypeUID {
      get; set;
    }

    public string BudgetTypeUID {
      get; set;
    }

    public string BudgetUID {
      get; set;
    }

    public string OperationSourceUID {
      get; set;
    }

    public DateTime ApplicationDate {
      get; set;
    }

    public string Justification {
      get; set;
    }

  }  // class ImportBudgetTransactionCommand

}  // namespace Empiria.Banobras.Budgeting
