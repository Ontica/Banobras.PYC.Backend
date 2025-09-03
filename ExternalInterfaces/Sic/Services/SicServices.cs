/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Sic.Connector.dll                          Pattern   : Service provider                        *
*  Type     : SicService                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements interface using SIC services.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.BanobrasIntegration.Sic.Adapters;
using Empiria.BanobrasIntegration.Sic.Data;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Implements interface using SIC services.</summary>
  public class SicServices : Service {

    #region Constructors and parsers

    public SicServices() {
      // no-op
    }

    #endregion Constructors and parsers

    #region Methods

    public SitCreditDto TryGetCredit(string creditNo) {
      Assertion.Require(creditNo, nameof(creditNo));

      SicCredit credit = SicCreditDataService.TryGetCredit(creditNo);

      if (credit == null) {
        return null;
      } else {
        return SicMapper.MapToCredit(credit);
      }

    }

    #endregion Methods

  } // class SicServices

} // namespace  Empiria.BanobrasIntegration.Sic
