/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Sic.Connector.dll                          Pattern   : Service provider                        *
*  Type     : SicService                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements ICreditAccountService interface using Banobras SIC System services.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial.Adapters;

using Empiria.BanobrasIntegration.Sic.Adapters;
using Empiria.BanobrasIntegration.Sic.Data;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Implements ICreditAccountService interface using Banobras SIC System services.</summary>
  public class SicServices : ICreditAccountService {

    #region Constructors and parsers

    public SicServices() {
      // no-op
    }

    #endregion Constructors and parsers

    #region Methods

    public FixedList<ICreditEntryData> GetCreditsEntries(FixedList<string> creditIDs,
                                                         DateTime fromDate, DateTime toDate) {
      Assertion.Require(creditIDs, nameof(creditIDs));

      fromDate = ExecutionServer.DateMinValue;
      toDate = ExecutionServer.DateMaxValue;

      creditIDs = creditIDs.Select(x => EmpiriaString.Clean(x))
                           .ToFixedList()
                           .FindAll(x => EmpiriaString.IsInteger(x));

      FixedList<SicCreditEntry> entries = SicCreditDataService.GetCreditEntries(creditIDs,
                                                                                fromDate, toDate);

      return SicMapper.MapToCreditEntries(entries)
                      .Select(x => (ICreditEntryData) x)
                      .ToFixedList();
    }


    public ICreditSicData TryGetCreditSic(string creditNo) {
      Assertion.Require(creditNo, nameof(creditNo));

      SicCredit credit = SicCreditDataService.TryGetCredit(creditNo);

      Assertion.Require(credit, $"El número de crédito {creditNo} no existe");

      return SicMapper.MapToCreditSic(credit);
    }


    public ICreditAccountData TryGetCredit(string creditNo) {
      Assertion.Require(creditNo, nameof(creditNo));

      SicCredit credit = SicCreditDataService.TryGetCredit(creditNo);

      if (credit == null) {
        return null;
      }

      return SicMapper.MapToCredit(credit);
    }


    #endregion Methods

  } // class SicServices

} // namespace Empiria.BanobrasIntegration.Sic
