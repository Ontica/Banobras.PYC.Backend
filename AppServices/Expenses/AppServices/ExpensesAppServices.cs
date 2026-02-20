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

using Empiria.Budgeting.Transactions;

using Empiria.Orders;

using Empiria.Payments;
using Empiria.Payments.Adapters;
using Empiria.Payments.UseCases;

using Empiria.Banobras.Expenses.Adapters;

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


    public PaymentOrderDto RequestPaymentForTravelExpenses(TravelExpensesPaymentRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      var travelRequisition = Requisition.Parse(fields.RequisitionUID);

      Assertion.Require(travelRequisition.Category.PlaysRole("travel-expenses"),
                        "Requisition is not for travel expenses.");

      Assertion.Require(travelRequisition.Rules.CanRequestBudget(),
                        "Budget cannot be requested for this requisition.");

      if (BudgetTransaction.GetFor(travelRequisition)
                           .FindAll(x => x.InProcess || x.IsClosed)
                           .Count > 0) {
        Assertion.RequireFail("There are already budget transactions in process or closed for this requisition.");
      }

      var paymentOrder = PaymentOrder.Parse(EmpiriaMath.GetRandom(1, 50));

      return PaymentOrderMapper.MapToDto(paymentOrder);
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
