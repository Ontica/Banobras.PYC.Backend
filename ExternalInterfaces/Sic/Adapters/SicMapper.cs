/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Services                      Component : Adapters Layer                          *
*  Assembly : Sic.Connector.dll                          Pattern   : Mapper class                            *
*  Type     : SicMapper                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapper service for SIC credits.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Globalization;
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

    static public FixedList<SicCreditAccountDto> MapToCredits(FixedList<SicCredit> credits) {
      return credits.Select(x => MapToCredit(x))
                      .ToFixedList();
    }


    static internal SicCreditAccountDto MapToCredit(SicCredit credit) {
      return new SicCreditAccountDto {
        CreditNo = credit.CreditoNo,
        SubledgerAccountNo = EmpiriaString.Clean(credit.Auxiliar),
        BaseInterestRate = credit.TasaBaseInt,
        BaseMorInterestRate = credit.TasaBaseInt,
        PreviousCredits = credit.CreditoAnterior,
        OrganizationalUnit = MapArea(credit.AreaPromocion),
        Currency = MapCurrency(credit.Moneda),
        FederalTaxPayersReg = credit.Rfc,
        FederalTaxPayersRegNo = credit.RfcConsecutivo,
        CustomerType = credit.TipoCliente,
        CustomerName = credit.NombreCliente,
        CreditType = credit.ClasifCreditoDes,
        CreditStage = credit.EtapaCreditoDes,
        StandardAccount = credit.CtaRegistro,
        ExternalCreditNo = credit.CreditoPasivoNo,
        MaxAvailabilityDate = credit.FecMaxDisposicion,
        MaxRefinancingDate = credit.FechaMaxRefinanciamiento,
        LineCreditNo = credit.LineaCreditoNo,
        NetFinancedAmount = credit.MontoNetoFinanciar,
        ConstructionBuilding = credit.TipoObraDescripcion,
        ConstructionBuildingCost = credit.MontoObra,
        LoanAmount = credit.MontoCredito,
        CurrentBalance = credit.Saldo,
        InvestmentTerm = credit.PlazoInversion,
        InterestGracePeriod = credit.PlazoGraciaInt,
        DisbursementPeriod = credit.PlazoDesembolso,
        DisbursementDate = credit.FechaDesembolso,
        RepaymentTerm = credit.PlazoAmortizacion,
        RepaymentDate = credit.FechaAmortizacion,
        InterestRate = credit.Tasa,
        InterestRateFactor = credit.FactorTasa,
        InterestRateFloor = credit.TasaPiso,
        InterestRateCeiling = credit.TasaTecho
      };
    }


    static internal SicCreditDto MapToCreditSic(SicCredit credit) {
      OrganizationalUnit orgUnit = MapArea(credit.AreaPromocion);
      Currency currency = MapCurrency(credit.Moneda);
      return new SicCreditDto {
        CreditNo = credit.CreditoNo,
        SubledgerAccountNo = EmpiriaString.Clean(credit.Auxiliar),
        BaseInterestRate = credit.TasaBaseInt,
        BaseMorInterestRate = credit.TasaBaseInt,
        PreviousCredits = credit.CreditoAnterior,
        Currency = currency.Name,
        CurrencyNo = credit.Moneda.ToString(),
        FederalTaxPayersReg = credit.Rfc,
        FederalTaxPayersRegNo = credit.RfcConsecutivo,
        CustomerType = credit.TipoCliente,
        CustomerName = credit.NombreCliente,
        OrganizationUnitNo = orgUnit.Code,
        OrganizationUnit = orgUnit.Name,
        CreditType = credit.ClasifCreditoDes,
        CreditStage = credit.EtapaCreditoDes,
        StandardAccount = credit.CtaRegistro,
        ExternalCreditNo = credit.CreditoPasivoNo,
        MaxAvailabilityDate = credit.FecMaxDisposicion,
        MaxRefinancingDate = credit.FechaMaxRefinanciamiento,
        LineCreditNo = credit.LineaCreditoNo,
        NetFinancedAmount = credit.MontoNetoFinanciar,
        ConstructionBuilding = credit.TipoObraDescripcion,
        ConstructionBuildingCost = credit.MontoObra,
        LoanAmount = credit.MontoCredito,
        CurrentBalance = credit.Saldo,
        InvestmentTerm = credit.PlazoInversion,
        InterestGracePeriod = credit.PlazoGraciaInt,
        DisbursementPeriod = credit.PlazoDesembolso,
        DisbursementDate = credit.FechaDesembolso,
        RepaymentTerm = credit.PlazoAmortizacion,
        RepaymentDate = credit.FechaAmortizacion,
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
