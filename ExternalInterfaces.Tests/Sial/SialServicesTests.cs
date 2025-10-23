/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                        Component : Test cases                        *
*  Assembly : Banobras.PYC.ExternalInterfaces.Tests.dll        Pattern   : Unit tests                        *
*  Type     : SialServicesTests                                License   : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : Unit tests for SialServices.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using System;

using Empiria.StateEnums;

using Empiria.BanobrasIntegration.Sial;
using Empiria.BanobrasIntegration.Sial.Adapters;

namespace Empiria.Tests.BanobrasIntegration.Sial {

  /// <summary>Unit tests for SialServices.</summary>
  public class SialServicesTests {

    #region Facts

    [Fact]
    public void Should_Search_Payrolls() {
      var services = SialServices.ServiceInteractor();

      var query = new SialPayrollsQuery {
        Status = EntityStatus.Pending,
        FromDate = DateTime.Parse("01-09-2025"),
        ToDate = DateTime.Parse("30-09-2025")
      };

      FixedList<SialPayrollDto> sut = services.SearchPayrolls(query);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Get_PayrollDataDetail() {
      var services = SialServices.ServiceInteractor();

      FixedList<SialDetailEntryDto> sut = services.GetPayrollsDetailEntries(DateTime.Parse("30/09/2025"));

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Update_ProcessStatus() {
      var services = SialServices.ServiceInteractor();

      services.UpdatePayrollStatus(9903739, EntityStatus.Deleted);
    }

    #endregion Facts

  }  // class SialServicesTests

}  // namespace Empiria.Tests.BanobrasIntegration.Sial
