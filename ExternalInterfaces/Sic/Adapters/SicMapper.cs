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

    static public FixedList<SicCreditDto> MapToCredits(FixedList<SicCredit> credits) {
      return credits.Select(x => MapToCredit(x))
                      .ToFixedList();
    }


    static internal SicCreditDto MapToCredit(SicCredit credit) {
      return new SicCreditDto {
        AccountNo = credit.CreditoNo.ToString(),
        CustomerNo = credit.ClienteNo.ToString(),
        CustomerName = EmpiriaString.Clean(credit.NombreCliente),
        SubledgerAccountNo = EmpiriaString.Clean(credit.Auxiliar),
      };
    }

    #endregion Internal Methods

  } // class SicMapper

}  //namespace Empiria.BanobrasIntegration.Sic.Adapters
