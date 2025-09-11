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

namespace Empiria.Sic.Tests.BanobrasIntegration {

  /// <summary>Unit tests for SicServices type.</summary>
  public class SicServicesTests {

    #region Facts

    [Fact]
    public void Should_Get_CreditAccountData_By_CreditNo() {
      var services = new SicServices();

      ICreditAccountData sut = services.TryGetCredit("13924");

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class SicServicesTests

}  // namespace Empiria.Sic.Tests.BanobrasIntegration
