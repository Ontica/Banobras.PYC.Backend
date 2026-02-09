/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Payments Management                 Component : Web Api Layer                         *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web api Controller                    *
*  Type     : PaymentAccountsController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update payee's payment accounts.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments;
using Empiria.Payments.Adapters;
using Empiria.Payments.UseCases;

using Empiria.Banobras.Payments.Adapters;

namespace Empiria.Banobras.Payments.WebApi {

  /// <summary>Web API used to retrieve and update payee's payment accounts.</summary>
  public class PaymentAccountsController : WebApiController {

    #region Web apis

    [HttpPost]
    [Route("v8/procurement/suppliers/{payeeUID:guid}/payment-accounts")]   // ToDo: Remove this route in future versions
    [Route("v2/payments-management/payees/{payeeUID:guid}/payment-accounts")]
    public SingleObjectModel AddPaymentAccount([FromUri] string payeeUID,
                                               [FromBody] PaymentAccountFields fields) {

      var payee = Payee.Parse(payeeUID);

      _ = PaymentAccountService.AddPaymentAccount(payee, fields);

      PayeeHolderDto payeeDto = PayeeMapper.Map(payee);

      return new SingleObjectModel(base.Request, payeeDto);
    }


    [HttpGet]
    [Route("v8/financial/financial-institutions")]      // ToDo: Remove this route in future versions
    [Route("v2/payments-management/financial-institutions")]
    public CollectionModel GetFinancialInstitutions() {

      FixedList<FinancialInstitution> institutions = FinancialInstitution.GetList();

      return new CollectionModel(Request, institutions.MapToNamedEntityList());
    }


    [HttpGet]
    [Route("v8/procurement/suppliers/{payeeUID:guid}/payment-accounts")]  // ToDo: Remove this route in future versions
    [Route("v2/payments-management/payees/{payeeUID:guid}/payment-accounts")]
    public CollectionModel GetPaymentAccounts([FromUri] string payeeUID) {

      var payee = Payee.Parse(payeeUID);

      FixedList<PaymentAccountDto> accounts = PaymentAccountDto.Map(payee.GetAccounts());

      return new CollectionModel(base.Request, accounts);
    }


    [HttpGet]
    [Route("v8/financial/payment-account-types")]     // ToDo: Remove this route in future versions
    [Route("v2/payments-management/payment-account-types")]
    public CollectionModel GetPaymentAccountTypes() {

      FixedList<PaymentAccountType> accountTypes = PaymentAccountType.GetList();

      return new CollectionModel(Request, accountTypes.MapToNamedEntityList());
    }


    [HttpGet]
    [Route("v8/financial/payment-methods")]   // ToDo: Remove this route in future versions
    [Route("v2/payments-management/payment-methods")]
    public CollectionModel GetPaymentMethods() {

      FixedList<PaymentMethod> paymentMethods = PaymentMethod.GetList();

      FixedList<PaymentMethodDto> dtos = PaymentMethodDto.Map(paymentMethods);

      return new CollectionModel(Request, dtos);
    }


    [HttpDelete]
    [Route("v8/procurement/suppliers/{payeeUID:guid}/payment-accounts/{accountUID:guid}")] // ToDo: Remove this route in future versions
    [Route("v2/payments-management/payees/{payeeUID:guid}/payment-accounts/{accountUID:guid}")]
    public SingleObjectModel RemovePaymentAccount([FromUri] string payeeUID,
                                                  [FromUri] string accountUID) {

      var payee = Payee.Parse(payeeUID);
      var account = PaymentAccount.Parse(accountUID);

      _ = PaymentAccountService.RemovePaymentAccount(payee, account);

      PayeeHolderDto payeeDto = PayeeMapper.Map(payee);

      return new SingleObjectModel(base.Request, payeeDto);
    }


    [HttpPut, HttpPatch]
    [Route("v8/procurement/suppliers/{payeeUID:guid}/payment-accounts/{accountUID:guid}")]  // ToDo: Remove this route in future versions
    [Route("v2/payments-management/payees/{payeeUID:guid}/payment-accounts/{accountUID:guid}")]
    public SingleObjectModel UpdatePaymentAccount([FromUri] string payeeUID,
                                                  [FromUri] string accountUID,
                                                  [FromBody] PaymentAccountFields fields) {

      var payee = Payee.Parse(payeeUID);

      fields.UID = accountUID;

      _ = PaymentAccountService.UpdatePaymentAccount(payee, fields);

      PayeeHolderDto payeeDto = PayeeMapper.Map(payee);

      return new SingleObjectModel(base.Request, payeeDto);
    }

    #endregion Web apis

  }  // class PaymentAccountsController

}  //namespace Empiria.Banobras.Payments.WebApi
