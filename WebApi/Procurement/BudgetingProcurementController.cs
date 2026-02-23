/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Procurement Services                 Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Web Api Controller                   *
*  Type     : BudgetingProcurementController                License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to retrieve and generate budget procurement transactions.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Orders;

using Empiria.Payments;
using Empiria.Payments.Adapters;

using Empiria.Banobras.Budgeting.AppServices;

using Empiria.Banobras.Procurement.Adapters;
using Empiria.Banobras.Procurement.UseCases;

namespace Empiria.Banobras.Procurement.WebApi {

  /// <summary>Web API used to retrieve and generate budget procurement transactions.</summary>
  public class ProcuremenBudgetingController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/procurement/approve-payment")]
    public SingleObjectModel ApprovePayment([FromBody] BudgetRequestFields fields) {

      using (var usecases = BudgetExecutionAppServices.UseCaseInteractor()) {

        var paymentOrder = PaymentOrder.Parse(fields.BaseObjectUID);

        BudgetTransaction approvePaymentTxn = usecases.ApprovePayment(paymentOrder);

        var dto = BudgetTransactionMapper.MapToDescriptor(approvePaymentTxn);

        return new SingleObjectModel(Request, dto);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/procurement/commit")]
    public SingleObjectModel CommitBudget([FromBody] BudgetRequestFields fields) {

      using (var usecases = BudgetExecutionAppServices.UseCaseInteractor()) {

        var order = Order.Parse(fields.BaseObjectUID);

        BudgetTransaction commitTxn = usecases.CommitBudget(order);

        var dto = BudgetTransactionMapper.MapToDescriptor(commitTxn);

        return new SingleObjectModel(Request, dto);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/procurement/request")]
    public SingleObjectModel RequestBudget([FromBody] BudgetRequestFields fields) {

      using (var usecases = BudgetExecutionAppServices.UseCaseInteractor()) {

        var order = Order.Parse(fields.BaseObjectUID);

        BudgetTransaction requestTxn = usecases.RequestBudget(order);

        var dto = BudgetTransactionMapper.MapToDescriptor(requestTxn);

        return new SingleObjectModel(Request, dto);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/related-documents/for-transaction-edition/search")]
    public CollectionModel SearchRelatedDocumentsForTransactionEdition([FromBody] RelatedDocumentsQuery query) {

      base.RequireBody(query);

      using (var usecases = ProcurementBudgetingUseCases.UseCaseInteractor()) {
        FixedList<PayableEntityDto> list = usecases.SearchRelatedDocumentsForTransactionEdition(query);

        return new CollectionModel(Request, list);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/procurement/validate-budget")]
    public SingleObjectModel ValidateBudget([FromBody] BudgetRequestFields fields) {

      using (var usecases = ProcurementBudgetingUseCases.UseCaseInteractor()) {
        BudgetValidationResultDto validationResult = usecases.ValidateBudget(fields);

        return new SingleObjectModel(Request, validationResult);
      }
    }

    #endregion Web Apis

  }  // class ProcuremenBudgetingController

}  // namespace Empiria.Banobras.Procurement.WebApi
