/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Expenses Management                 Component : Web Api Layer                         *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web api Controller                    *
*  Type     : ExpensesController                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used for expenses management.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Adapters;

using Empiria.Banobras.Expenses.AppServices;

namespace Empiria.Banobras.Expenses.WebApi {

  /// <summary>Web API used for expenses management.</summary>
  public class ExpensesController : WebApiController {

    #region Web apis

    [HttpGet]
    [Route("v2/expenses-management/commissioners/{commissionerUID:guid}/payment-accounts")]
    public CollectionModel GetCommissionerPaymentAccounts([FromUri] string commissionerUID) {

      using (var usecases = ExpensesAppServices.UseCaseInteractor()) {

        FixedList<PaymentAccountDto> accounts = usecases.GetCommissionerPaymentAccounts(commissionerUID);

        return new CollectionModel(base.Request, accounts);
      }
    }


    [HttpGet]
    [Route("v2/expenses-management/commissioners/search")]
    public CollectionModel SearchCommissioners([FromUri] string keywords = null) {

      keywords = keywords ?? string.Empty;

      using (var usecases = ExpensesAppServices.UseCaseInteractor()) {

        FixedList<NamedEntityDto> commissioners = usecases.SearchCommissioners(keywords);

        return new CollectionModel(base.Request, commissioners);
      }
    }

    #endregion Web apis

  }  // class ExpensesController

}  // namespace Empiria.Banobras.Expenses.WebApi
