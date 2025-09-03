/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Sic.Connector.dll                          Pattern   : Service provider                        *
*  Type     : SicService                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements interface using SIC services.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.BanobrasIntegration.Sic.Adapters;
using Empiria.BanobrasIntegration.Sic.Data;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Implements interface using SIC services.</summary>
  public class SicServices {

   
    #region Methods

    FixedList<CreditResultDto> GetCreditResult (string  auxiliar) {
      Assertion.Require(auxiliar, nameof(auxiliar));

      FixedList<SicCreditResult> creditResult = SicCreditDataService.SearchAuxiliar(auxiliar);

      return SicMapper.MapToCredits (creditResult);
    }


    #endregion Methods

  } // class SicServices

} // namespace  Empiria.BanobrasIntegration.Sic
