/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Services                      Component : Adapters Layer                          *
*  Assembly : Sic.Connector.dll                          Pattern   : Output DTO                              *
*  Type     : SicCreditDto                               License   : Please read LICENSE.txt file            *
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
  public class SicCreditDto : ICreditAccountData {

    public string AccountNo {
      get; internal set;
    }

    public OrganizationalUnit OrganizationalUnit {
      get; internal set;
    }

    public string SubledgerAccountNo {
      get; internal set;
    }

    public string Borrower {
      get; internal set;
    }

    public string CreditStage {
      get; internal set;
    }

    public string CreditType {
      get; internal set;
    }

    public string ExternalCreditNo {
      get; internal set;
    }

    public OrganizationalUnit Area {
      get; internal set;
    }

    public Currency Currency {
      get; internal set;
    }

    public string StandardAccount {
      get; internal set;
    }

    public decimal CurrentBalance {
      get; internal set;
    }

    public int InvestmentTerm {
      get; internal set;
    }

    public int GracePeriod {
      get; internal set;
    }

    public int RepaymentTerm {
      get; internal set;
    }

    public DateTime RepaymentDate {
      get; internal set;
    }

    public decimal InterestRate {
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
