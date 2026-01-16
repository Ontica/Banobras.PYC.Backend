/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Services                      Component : Adapters Layer                          *
*  Assembly : Sic.Connector.dll                          Pattern   : Output DTO                              *
*  Type     : SicCreditAccountDto                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return SIC credit account data according to ICreditAccountData interface.   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Financial;
using Empiria.Financial.Adapters;
using Empiria.Parties;

namespace Empiria.BanobrasIntegration.Sic.Adapters {

  /// <summary>Output DTO used to return SIC credit account data
  /// according to ICreditAccountData interface.</summary>
  public class SicCreditAccountDto : ICreditAccountData {

    public string CreditNo {
      get; internal set;
    }


    public string SubledgerAccountNo {
      get; internal set;
    }


    public string BaseInterestRate {
      get; internal set;
    }


    public string BaseMorInterestRate {
      get; internal set;
    }


    public string PreviousCredits {
      get; internal set;
    }


    public Currency Currency {
      get; internal set;
    }


    public string FederalTaxPayersReg {
      get; internal set;
    }


    public int FederalTaxPayersRegNo {
      get; internal set;
    }


    public string CustomerType {
      get; internal set;
    }


    public string CustomerName {
      get; internal set;
    }


    public OrganizationalUnit OrganizationalUnit {
      get; internal set;
    }


    public string CreditType {
      get; internal set;
    }


    public string CreditStage {
      get; internal set;
    }


    public string StandardAccount {
      get; internal set;
    }


    public string ExternalCreditNo {
      get; internal set;
    }


    public DateTime MaxAvailabilityDate {
      get; internal set;
    }


    public DateTime MaxRefinancingDate {
      get; internal set;
    }


    public int LineCreditNo {
      get; internal set;
    }


    public decimal NetFinancedAmount {
      get; internal set;
    }


    public string ConstructionBuilding {
      get; internal set;
    }


    public decimal ConstructionBuildingCost {
      get; internal set;
    }


    public decimal LoanAmount {
      get; internal set;
    }


    public decimal CurrentBalance {
      get; internal set;
    }


    public int InvestmentTerm {
      get; internal set;
    }


    public int DisbursementPeriod {
      get; internal set;
    }

    
    public DateTime DisbursementDate {
      get; internal set;
    }


    public int RepaymentTerm {
      get; internal set;
    }


    public DateTime RepaymentDate {
      get; internal set;
    }

    public int InterestRate {
      get; internal set;
    }

    public int InterestGracePeriod {
      get; internal set;
    }

    public decimal InterestRateFactor {
      get; internal set;
    }


    public decimal InterestRateFloor {
      get; internal set;
    }

    public decimal InterestRateCeiling {
      get; internal set;
    }

  }  // class SicCreditDto

} // namespace Empiria.BanobrasIntegration.Sic.Adapters
