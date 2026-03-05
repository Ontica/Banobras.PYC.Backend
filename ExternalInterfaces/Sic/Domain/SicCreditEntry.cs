/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Services                      Component : Domain Layer                            *
*  Assembly : Sic.Connector.dll                          Pattern   : Information Holder                      *
*  Type     : SicCreditEntry                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information about a Banobras SIC credit entry.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;

using Empiria.BanobrasIntegration.Sic.Adapters;


namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Holds information about a Banobras SIC credit entry.</summary>
  public class SicCreditEntry {

    public int NoCredito {
      get; private set;
    }


    public string NomCredito {
      get; private set;
    } = String.Empty;


    public string Auxiliar {
      get; private set;
    } = string.Empty;


    public DateTime FechaProceso {
      get; private set;
    }


    public string NombreConcepto {
      get; private set;
    }


    public decimal Importe {
      get; private set;
    }


    #region Methods


    static internal FixedList<SicCreditEntry> MapToSicCreditsEntry(FixedList<MovtosDetalleDto> movimientos, int idCredito) {
      return movimientos.Select(x => MapToSicCreditEntry(x, idCredito))
                       .ToFixedList();
    }


    static internal SicCreditEntry MapToSicCreditEntry(MovtosDetalleDto movimiento, int idCredito) {
      FinancialAccount creditAccount = GetCreditAccount(idCredito.ToString());

      return new SicCreditEntry {
        NoCredito = idCredito,
        NomCredito = creditAccount.Description,
        FechaProceso = Convert.ToDateTime(movimiento.Fecha),
        NombreConcepto = movimiento.Concepto,
        Auxiliar = creditAccount.SubledgerAccountNo,
        Importe = movimiento.Importe,
      };

    }

    #endregion Methods

    #region Helpers
    static FinancialAccount GetCreditAccount(string idCredito) {
      var financialAccount = FinancialAccount.TryParseWithAccountNo(FinancialAccountType.CreditAccount, idCredito);

      if (financialAccount is null) {
        return FinancialAccount.Empty;
      }

      return financialAccount;
    }

    #endregion Helpers


  } // class SicCreditEntry

} // namespace Empiria.BanobrasIntegration.Sic
