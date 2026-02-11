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

using Empiria.Budgeting.Transactions.Adapters;

using Empiria.Payments.Adapters;

using Empiria.Banobras.Procurement.Adapters;
using Empiria.Banobras.Procurement.UseCases;

namespace Empiria.Banobras.Procurement.WebApi {

  /// <summary>Web API used to retrieve and generate budget procurement transactions.</summary>
  public class ProcuremenBudgetingController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/procurement/approve-payment")]
    public SingleObjectModel ApprovePayment([FromBody] BudgetRequestFields fields) {

      using (var usecases = ProcurementBudgetingUseCases.UseCaseInteractor()) {
        BudgetTransactionDescriptorDto transaction = usecases.ApprovePayment(fields);

        return new SingleObjectModel(Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/procurement/commit")]
    public SingleObjectModel CommitBudget([FromBody] BudgetRequestFields fields) {

      using (var usecases = ProcurementBudgetingUseCases.UseCaseInteractor()) {
        BudgetTransactionDescriptorDto transaction = usecases.CommitBudget(fields);

        return new SingleObjectModel(Request, transaction);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/procurement/request")]
    public SingleObjectModel RequestBudget([FromBody] BudgetRequestFields fields) {

      using (var usecases = ProcurementBudgetingUseCases.UseCaseInteractor()) {
        BudgetTransactionDescriptorDto transaction = usecases.RequestBudget(fields);

        return new SingleObjectModel(Request, transaction);
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
