/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Sic.Connector.dll                          Pattern   : Data Transfer Object                    *
*  Type     : AccountingEntriesFieldsDto                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to get accounting entries list from system sic                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.BanobrasIntegration.Sic.Adapters {

    public class AccountingEntriesQuery {

        public int CreditId
        {
            get; set;
        }

        public DateTime QueryDate {
            get; set;
        }

    } // class MovtosEncabezadosDto


    public class AccountingEntriesDto {

        public DateTime DateMovement
        {
            get; internal set;
        }

        public string Concept
        {
            get; internal set;
        }

        public decimal Amount
        {
            get; internal set;
        }

    } // class MovtosDetalleDto

    static internal class AccountEntriesMapper {

      static internal MovtosEncabezadosDto MapToAccountEntries(AccountingEntriesQuery query) {
        return new MovtosEncabezadosDto {
          idCredito = query.CreditId,
          fecConsulta = query.QueryDate.Year.ToString() + query.QueryDate.Month.ToString() + query.QueryDate.Day.ToString(),
        };
      }

      static public FixedList<AccountingEntriesDto> MapToAccountEntriesDetail(FixedList<MovtosDetalleDto> entries) {
        return entries.Select(x => MapToAccountEntry(x))
                      .ToFixedList();
      }

      static internal AccountingEntriesDto MapToAccountEntry(MovtosDetalleDto entry) {
        return new AccountingEntriesDto {
          DateMovement = Convert.ToDateTime(entry.FECHA),
          Concept = entry.CONCEPTO,
          Amount = entry.IMPORTE
        };
      }

  } // AccountEntriesMapper

} // namespace Empiria.BanobrasIntegration.Sic.Adapters
