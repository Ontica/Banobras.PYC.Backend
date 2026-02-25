/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Expenses Management                     Component : Tests cases                       *
*  Assembly : Banobras.PYC.AppServices.Tests.dll               Pattern   : Unit tests                        *
*  Type     : ExpensesAppServicesTests                         License   : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : Unit tests for ExpensesAppServices.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Banobras.Billing.Adapters;
using Empiria.Banobras.Billing.AppServices;

using Xunit;

namespace Empiria.Tests.Banobras.Billing {

  /// <summary>Unit tests for ExpensesAppServices.</summary>
  public class BillingAppServicesTests {

    #region Facts

    [Fact]
    public void Should_Get_Bill_Descriptor() {

      TestsCommonMethods.Authenticate();

      BillsQuery query = GetBillsQuery();

      var service = BillAppServices.UseCaseInteractor();

      FixedList<BillDescriptorDto> sut = service.SearchBills(query);

      Assert.NotNull(sut);
    }

    #endregion Facts

    #region Helpers

    private BillsQuery GetBillsQuery() {

      return new BillsQuery {
        BillTypeUID = "",
        BillCategoryUID = "",
        ManagedByUID = "",
        Keywords = "3F286DEE-DC25-11F0-8891-77A2C4B2D608",
        ConceptsKeywords = "",
        Tags = new string[] { },
        BillDateType = BillQueryDateType.None,
        //FromDate = new DateTime(2024, 10, 29),
        //ToDate = new DateTime(2024, 10, 31),
        Status = Empiria.Billing.BillStatus.Pending
      };

    }

    #endregion Helpers

  }  // class ExpensesAppServicesTests

}  // namespace Empiria.Tests.Banobras.Expenses
