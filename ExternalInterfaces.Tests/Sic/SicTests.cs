/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : PYC SIC Integration                              Component : Test cases                        *
*  Assembly : Banobras.PYC.ExternalInterfaces.Tests.dll        Pattern   : Unit tests                        *
*  Type     : SicTests                                         License   : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : PYC - Sic Credit services Test.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.BanobrasIntegration.Sic;

namespace Empiria.Sic.Tests.BanobrasIntegration {

  /// <summary>PYC - Sic Credit services Test.</summary>
  public class SicTests {

    #region Facts

    [Fact]
    public void Should_Get_CreditInfo_By_CreditNo() {
      var services = new SicServices();

      var credit = services.TryGetCredit("15634");

      Assert.NotNull(credit);
    }

    #endregion Facts

  }  // class SicTests

}  // namespace Empiria.Sic.Tests.BanobrasIntegration
