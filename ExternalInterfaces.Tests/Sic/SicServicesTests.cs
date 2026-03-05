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
using System.Collections.Generic;
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
      SicApiClient _apiClient = _apiClient = new SicApiClient();

      var body = new MovtosEncabezadosDto {
        idCredito = 9564,
        fecConsulta = "20110531"
      };

      FixedList<MovtosDetalleDto> sut = await _apiClient.TryGetMovtosDetalle(body);

      Assert.NotNull(sut);
    }


    [Fact]
    public async Task Should_Get_CreditAccountEntries_By_CreditsNo() {
      var services = new SicServices();

      var credits = new List<string>() { "15300" };
      DateTime fromDate = Convert.ToDateTime("2024-01-01");
      DateTime toDate = Convert.ToDateTime("2024-02-02");

      FixedList<ICreditEntryData> sut = await services.GetCreditsEntries(credits.ToFixedList(), fromDate, toDate);

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class SicServicesTests

}  // namespace Empiria.Sic.Tests.BanobrasIntegration
