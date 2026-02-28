/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Services                   Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Web Api Controller                   *
*  Type     : BudgetControlController                       License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to control budgeting periods and budget closing transactions.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions.UseCases;
using Empiria.Budgeting.Transactions.Adapters;

namespace Empiria.Banobras.Budgeting.WebApi {

  /// <summary>Web API used to control budgeting periods and budget closing transactions.</summary>
  public class BudgetControlController : WebApiController {

    #region Command Web Apis

    [HttpPost]
    [Route("v2/budgeting/planning/close-transactions")]
    public SingleObjectModel ClosePlanningTransactions() {

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
    [Route("v2/budgeting/planning/generate-transactions")]
    public SingleObjectModel GeneratePlanningTransactions() {

      using (var usecases = BudgetTransactionEditionUseCases.UseCaseInteractor()) {

        Budget budget = BaseObject.GetFullList<Budget>().Find(x => x.PlanningAutoGenerationTransactionTypes.Count != 0);

        FixedList<BudgetTransactionDescriptorDto> generated = usecases.GeneratePlanningTransactions(budget.UID);

        var result = new {
          Message = $"Se generaron {generated.Count} transacciones presupuestales para el presupuesto {budget.Name}."
        };

        return new SingleObjectModel(base.Request, result);
      }
    }


    #endregion Command Web Apis

  } // class BudgetControlController

} // namespace Empiria.Budgeting.WebApi
