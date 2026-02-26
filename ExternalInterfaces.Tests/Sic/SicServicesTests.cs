/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : PYC SIC Integration                              Component : Test cases                        *
*  Assembly : Banobras.PYC.ExternalInterfaces.Tests.dll        Pattern   : Unit tests                        *
*  Type     : SicServicesTests                                 License   : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : Unit tests for SicServices type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Adapters;

using Empiria.BanobrasIntegration.Sic;
using Empiria.BanobrasIntegration.Sic.Adapters;
using System.Threading.Tasks;
using System;

namespace Empiria.Sic.Tests.BanobrasIntegration {

  /// <summary>Unit tests for SicServices type.</summary>
  public class SicServicesTests {

    #region Facts

    [Fact]
    public void Should_Get_CreditAccountData_By_CreditNo() {
      var services = new SicServices();

      ICreditSicData sut = services.TryGetCreditSic("13789");

      Assert.NotNull(sut);
    }


    [Fact]
    public async Task Should_Get_AccountingTransacctions_Api() {
      var services = new SicApiService();

      AccountingEntriesQuery body = new AccountingEntriesQuery {
        CreditId = 13471,
        QueryDate = Convert.ToDateTime("2017-12-31")
      };


      FixedList<AccountingEntriesDto> sut = await services.GetMovements(body);

      Assert.NotNull(sut);
    }


    #endregion Facts

  }  // class SicServicesTests

}  // namespace Empiria.Sic.Tests.BanobrasIntegration
