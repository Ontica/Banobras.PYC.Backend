/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Sic.Connector.dll                          Pattern   : Mapper class                            *
*  Type     : SicMapper                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapper service for SIC credits.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.BanobrasIntegration.Sic.Adapters {

  /// <summary>Mapper service for SIC credits.</summary>
  static internal class SicMapper {

    #region Internal Methods

    static public FixedList<SitCreditDto> MapToCredits(FixedList<SicCredit> credits) {
      return credits.Select(x => MapToCredit(x))
                      .ToFixedList();
    }


    static internal SitCreditDto MapToCredit(SicCredit credit) {
      return new SitCreditDto {
        CreditNo = credit.CreditoNo,
        ClientNo = credit.ClienteNo,
        ClientName = credit.NombreCliente,
        Auxiliar = credit.Auxiliar.Trim(),
      };
    }

    #endregion Internal Methods

  } // class SicMapper

}  //namespace Empiria.BanobrasIntegration.Sic.Adapters
