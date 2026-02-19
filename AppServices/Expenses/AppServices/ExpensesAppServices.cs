/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Expenses Management                     Component : Application services Layer        *
*  Assembly : Banobras.PYC.AppServices.dll                     Pattern   : Application service               *
*  Type     : ExpensesAppServices                              License   : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : Application services for expenses management.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.StateEnums;

using Empiria.Payments;
using Empiria.Payments.Adapters;
using Empiria.Payments.UseCases;

namespace Empiria.Banobras.Expenses.AppServices {

  /// <summary>Application services for expenses management.</summary>
  public class ExpensesAppServices : UseCase {

    #region Constructors and parsers

    protected ExpensesAppServices() {
    }

    static public ExpensesAppServices UseCaseInteractor() {
      return CreateInstance<ExpensesAppServices>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<PaymentAccountDto> GetCommissionerPaymentAccounts(string commissionerUID) {
      Assertion.Require(commissionerUID, nameof(commissionerUID));

      var commissioner = Payee.Parse(commissionerUID);

      FixedList<PaymentAccount> accounts = commissioner.GetAccounts();

      return PaymentAccountDto.Map(accounts);
    }


    public FixedList<NamedEntityDto> SearchCommissioners(string keywords) {

      keywords = keywords ?? string.Empty;

      var query = new PayeesQuery {
        Keywords = keywords,
        Status = EntityStatus.Active
      };

      FixedList<Payee> commissioners = PaymentAccountService.SearchPayees(query);

      return commissioners.MapToNamedEntityList();
    }

    #endregion Use cases

  }  // class ExpensesAppServices

}  // namespace Empiria.Banobras.Expenses.AppServices
