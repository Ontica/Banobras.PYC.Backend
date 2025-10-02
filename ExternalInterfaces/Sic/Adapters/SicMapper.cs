/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Services                      Component : Adapters Layer                          *
*  Assembly : Sic.Connector.dll                          Pattern   : Mapper class                            *
*  Type     : SicMapper                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapper service for SIC credits.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;
using Empiria.Parties;

namespace Empiria.BanobrasIntegration.Sic.Adapters {

  /// <summary>Mapper service for SIC credits.</summary>
  static internal class SicMapper {

    #region Internal Methods

    static internal FixedList<SicCreditEntryDto> MapToCreditEntries(FixedList<SicCreditEntry> entries) {
      return entries.Select(x => new SicCreditEntryDto {
        AccountNo = x.NoCredito.ToString(),
        SubledgerAccountNo = EmpiriaString.Clean(x.Auxiliar),
        ApplicationDate = x.FechaProceso,
        OperationTypeNo = x.NumConcepto.ToString(),
        OperationName = EmpiriaString.Clean(x.NombreConcepto),
        Amount = x.Importe
      }).ToFixedList();
    }

    static public FixedList<SicCreditDto> MapToCredits(FixedList<SicCredit> credits) {
      return credits.Select(x => MapToCredit(x))
                      .ToFixedList();
    }


    static internal SicCreditDto MapToCredit(SicCredit credit) {
      return new SicCreditDto {
        AccountNo = credit.CreditoNo.ToString(),
        Borrower = EmpiriaString.Clean(credit.NombreCliente),
        SubledgerAccountNo = EmpiriaString.Clean(credit.Auxiliar),
        CreditStage = credit.EtapaCredito.ToString(),
        CreditType = credit.ClaveTipoCredito,
        ExternalCreditNo = credit.CreditoNo.ToString(),
        Currency = MapCurrency(credit.Moneda),
        Area = MapArea(credit.Area),
        StandardAccount = credit.CuentaStandard,
        CurrentBalance = credit.Saldo,
        InvestmentTerm = credit.PlazoInversion,
        GracePeriod = credit.PlazoGracia,
        RepaymentTerm = credit.PlazoAmortiazacion,
        RepaymentDate = credit.FechaDesembolso,
        InterestRate = credit.Tasa,
        InterestRateFactor = credit.FactorTasa,
        InterestRateFloor = credit.TasaPiso,
        InterestRateCeiling = credit.TasaTecho
      };
    }

    #endregion Internal Methods

    #region Helpers

    static private Currency MapCurrency(int moneda) {
      var currencies = Currency.GetList();
      string currencyCode = moneda.ToString("D2");
      var currency = currencies.Find(x => x.Code == currencyCode);

      if (currency == null) {
        return Currency.Default;
      }

      return currency;
    }

    static private OrganizationalUnit MapArea(string area) {
      var units = OrganizationalUnit.GetList<OrganizationalUnit>();
      var unit = units.Find(x => x.Code == area);

      if (unit == null) {
        return OrganizationalUnit.Empty;
      }

      return unit;
    }

    #endregion Helpers

  } // class SicMapper

}  //namespace Empiria.BanobrasIntegration.Sic.Adapters
