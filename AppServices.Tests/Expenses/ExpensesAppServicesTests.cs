/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Expenses Management                     Component : Tests cases                       *
*  Assembly : Banobras.PYC.AppServices.Tests.dll               Pattern   : Unit tests                        *
*  Type     : ExpensesAppServicesTests                         License   : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : Unit tests for ExpensesAppServices.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;
using Empiria.Storage;

using Empiria.Orders;
using Empiria.Payments;

using Empiria.Payments.Adapters;

using Empiria.Banobras.Expenses.Adapters;
using Empiria.Banobras.Expenses.AppServices;

namespace Empiria.Tests.Banobras.Expenses {

  /// <summary>Unit tests for ExpensesAppServices.</summary>
  public class ExpensesAppServicesTests {

    #region Facts

    [Fact]
    public void Should_RequestPaymentForTravelExpenses() {
      var usecases = ExpensesAppServices.UseCaseInteractor();

      Requisition requisition = TestingConstants.TRAVEL_REQUISITION;
      Payee commissioner = TestingConstants.TRAVEL_COMMISSIONER;
      PaymentAccount account = commissioner.GetAccounts()[0];
      InputFile pdfFile = TestingConstants.TRAVEL_AUTHORIZATION_PDF_FILE;

      var fields = new TravelExpensesPaymentRequestFields {
        CommissionNo = "2026/234857/3456",
        RequisitionUID = requisition.UID,
        CommissionerUID = commissioner.UID,
        PaymentAccountUID = account.UID,
        AuthorizationPdfFile = pdfFile
      };

      PaymentOrderDto sut = usecases.RequestPaymentForTravelExpenses(fields);

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class ExpensesAppServicesTests

}  // namespace Empiria.Tests.Banobras.Expenses
