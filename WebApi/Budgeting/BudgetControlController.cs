/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Services                   Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Web Api Controller                   *
*  Type     : BudgetControlController                       License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to control budgeting periods and budget closing transactions.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Web.Http;
using Empiria.Budgeting;
using Empiria.Budgeting.Explorer;
using Empiria.Budgeting.Explorer.UseCases;
using Empiria.Budgeting.Transactions.Adapters;
using Empiria.Budgeting.Transactions.UseCases;
using Empiria.WebApi;

namespace Empiria.Banobras.Budgeting.WebApi {

  /// <summary>Web API used to control budgeting periods and budget closing transactions.</summary>
  public class BudgetControlController : WebApiController {

    #region Query Web Apis

    [HttpGet]
    [Route("v2/budgeting/budget-control/{budgetUID:guid}/months")]
    public CollectionModel GetBudgetControlMonthsTable([FromUri] string budgetUID) {

      using (var usecases = BudgetExplorerUseCases.UseCaseInteractor()) {

        var budget = Budget.Parse(budgetUID);

        FixedList<BudgetDataInColumns> balances = usecases.GetMonthBalances(budget);

        var controlTable = balances.Select(x => new {
          x.Month,
          x.MonthName,
          x.Modified,
          x.Requested,
          x.Commited,
          x.ToPay,
          x.Exercised,
          x.ToExercise,
          x.Available,
          Actions = new {
            CanClose = x.Month < DateTime.Today.Month,
            CanOpen = false,
            CanGenerate = x.Available > 0 && x.Month < DateTime.Today.Month,
          }
        }).ToFixedList();

        return new CollectionModel(base.Request, controlTable, "BudgetControlMonthsTable");
      }
    }

    #endregion Query Web Apis

    #region Command Web Apis

    [HttpPost]
    [Route("v2/budgeting/budget-control/{budgetUID:guid}/months/{month:int}/close")]
    public SingleObjectModel CloseMonth([FromUri] int budgetUID, [FromUri] int month) {

      var budget = Budget.Parse(budgetUID);

      var result = new {
        Message = $"Se cerró {EmpiriaString.MonthName(month)} para el presupuesto {budget.Name}."
      };

      return new SingleObjectModel(base.Request, result);
    }


    [HttpPost]
    [Route("v2/budgeting/planning/{budgetUID:guid}/close-transactions")]
    public SingleObjectModel ClosePlanningTransactions([FromUri] string budgetUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

        Budget budget = BaseObject.GetFullList<Budget>().Find(x => x.PlanningAutoGenerationTransactionTypes.Count != 0);

        int closed = usecases.AutoCloseTransactions(budget);

        var result = new {
          Message = $"Se cerraron automáticamente {closed} transacciones presupuestales para el presupuesto {budget.Name}."
        };

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/budget-control/{budgetUID:guid}/months/{month:int}/generate-balance-transfers")]
    public SingleObjectModel GenerateBalanceTransferTransactions([FromUri] int budgetUID, [FromUri] int month) {

      var budget = Budget.Parse(budgetUID);

      var result = new {
        Message = $"Se generaron {0} transacciones presupuestales de cierre mensual correspondientes " +
                  $"al mes de {EmpiriaString.MonthName(month)} para el presupuesto {budget.Name}."
      };

      return new SingleObjectModel(base.Request, result);
    }


    [HttpPost]
    [Route("v2/budgeting/planning/{budgetUID:guid}/generate-transactions")]
    public SingleObjectModel GeneratePlanningTransactions([FromUri] string budgetUID) {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

        Budget budget = BaseObject.GetFullList<Budget>().Find(x => x.PlanningAutoGenerationTransactionTypes.Count != 0);

        FixedList<BudgetTransactionDescriptorDto> generated = usecases.GeneratePlanningTransactions(budget.UID);

        var result = new {
          Message = $"Se generaron {generated.Count} transacciones presupuestales para el presupuesto {budget.Name}."
        };

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/budget-control/{budgetUID:guid}/months/{month:int}/open")]
    public SingleObjectModel OpenMonth([FromUri] int budgetUID, [FromUri] int month) {

      var budget = Budget.Parse(budgetUID);

      var result = new {
        Message = $"Se abrió {EmpiriaString.MonthName(month)} para el presupuesto {budget.Name}."
      };

      return new SingleObjectModel(base.Request, result);
    }


    #endregion Command Web Apis

  } // class BudgetControlController

} // namespace Empiria.Budgeting.WebApi
