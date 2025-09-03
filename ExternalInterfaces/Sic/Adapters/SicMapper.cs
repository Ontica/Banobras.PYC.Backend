/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Sic.Connector.dll                          Pattern   : Mapper class                            *
*  Type     : SicMapper                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapper service for SIC credits.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Parties;
using Empiria.Payments.Processor;
using Empiria.Payments.Processor.Adapters;

namespace Empiria.BanobrasIntegration.Sic.Adapters {

  /// <summary>Mapper service for SIC credits.</summary>
  static internal class SicMapper {

    #region Internal Methods

    static public FixedList<CreditResultDto> MapToCredits(FixedList<SicCreditResult> credits) {
      return credits.Select(x => MapToCredit (x))
                      .ToFixedList();
    }

    static internal CreditResultDto MapToCredit (SicCreditResult credit) {
      return new CreditResultDto {
        CreditNo = credit.CreditoNo,
        ClientNo = credit.ClienteNo,
        ClientName = credit.NombreCliente,
        Auxiliar = credit.Auxiliar,        
      };
    }

    #endregion Internal Methods

  } // class SicMapper

}  //namespace Empiria.BanobrasIntegration.Sic.Adapters
