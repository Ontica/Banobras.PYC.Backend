/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : PYC SIAL Integration                             Component : Test cases                        *
*  Assembly : Banobras.PYC.ExternalInterfaces.Tests.dll        Pattern   : Unit tests                        *
*  Type     : SicServicesTests                                 License   : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : Unit tests for SialServices type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Financial.Adapters;

using Empiria.BanobrasIntegration.Sial;
using Empiria.BanobrasIntegration.Sial.Adapters;
using System;

namespace Empiria.Sial.Tests.BanobrasIntegration {

  /// <summary>Unit tests for SialServices type.</summary>
  public class SialServicesTests {

    #region Facts

    [Fact]
    public void Should_Get_PayrollData() {
      var services = new SialServices();

      FixedList<SialHeaderDto> sut = services.GetPayrollsEntries('P', DateTime.Parse("01-09-2025"), DateTime.Parse("30-09-2025"));

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Get_PayrollDataDetail() {
      var services = new SialServices();

      FixedList<SialDetailEntryDto> sut = services.GetPayrollsDetailEntries(DateTime.Parse("30/09/2025"));

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class SialServicesTests

}  // namespace Empiria.Sial.Tests.BanobrasIntegration
