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
using System.Collections.Generic;
using System.Threading.Tasks;
using Empiria.BanobrasIntegration.Sic.Adapters;
using Empiria.BanobrasIntegration.Sic.Data;
using Empiria.Financial.Adapters;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Implements ICreditAccountService interface using Banobras SIC System services.</summary>
  public class SicServices : ICreditAccountService {

    #region Fields

    private readonly SicApiClient _apiClient;

    #endregion Fields

    #region Constructors and parsers

    public SicServices() {
      _apiClient = new SicApiClient();
      // no-op
    }

    #endregion Constructors and parsers

    #region Methods

    public async Task<FixedList<ICreditEntryData>> GetCreditsEntries(FixedList<string> creditIDs,
                                                         DateTime fromDate, DateTime toDate) {
      Assertion.Require(creditIDs, nameof(creditIDs));

      creditIDs = creditIDs.Select(x => EmpiriaString.Clean(x))
                           .ToFixedList()
                           .FindAll(x => EmpiriaString.IsInteger(x));

      FixedList<SicCreditEntry> entries = await GetCredits(creditIDs, fromDate, toDate);

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


    #region Helpers 

    private async Task<FixedList<SicCreditEntry>> GetCredits(FixedList<string> creditIDs, DateTime fromDate, DateTime toDate) {
      List<SicCreditEntry> sicCredits = new List<SicCreditEntry>();

      foreach (var credit in creditIDs) {
        var sicMovemnts = await GetCreditEntries(Convert.ToInt32(credit), fromDate, toDate);

        var slipMovmentsList = SlipCreditMovments(sicMovemnts, fromDate, toDate);
        var entries = SicCreditEntry.MapToSicCreditsEntry(slipMovmentsList, Convert.ToInt32(credit));

        sicCredits.AddRange(entries);
      }

      return sicCredits.ToFixedList();
    }


    private async Task<FixedList<MovtosDetalleDto>> GetCreditEntries(int creditId, DateTime fromDate, DateTime toDate) {
      List<SicCreditEntry> sicCreditEntries = new List<SicCreditEntry>();

      var query = new MovtosEncabezadosDto {
        idCredito = creditId,
        fecConsulta = toDate.Year.ToString() + toDate.Month.ToString("D2") + toDate.Day.ToString("D2"),
      };

      return await _apiClient.TryGetMovtosDetalle(query);
    }


    private FixedList<MovtosDetalleDto> SlipCreditMovments(FixedList<MovtosDetalleDto> movimientos, DateTime fromDate, DateTime toDate) {
      var slipMovimientos = movimientos.FindAll(x => Convert.ToDateTime(x.Fecha) >= fromDate && Convert.ToDateTime(x.Fecha) <= toDate);

      return slipMovimientos;
    }

    #endregion Helpers

  } // class SicServices

} // namespace Empiria.BanobrasIntegration.Sic
