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
      Assertion.Require(fromDate, nameof(fromDate));
      Assertion.Require(toDate, nameof(toDate));

      fromDate = ExecutionServer.DateMinValue;
      toDate = ExecutionServer.DateMaxValue;

      creditIDs = creditIDs.Select(x => EmpiriaString.Clean(x))
                           .ToFixedList()
                           .FindAll(x => EmpiriaString.IsInteger(x));

      FixedList<SicCreditEntry> entries = await GetCreditsEntries(creditIDs, toDate);

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

    private async Task<FixedList<SicCreditEntry>> GetCreditsEntries(FixedList<string> creditIDs, DateTime toDate) {
      List<SicCreditEntry> sicCredits = new List<SicCreditEntry>();

      foreach (var credit in creditIDs) {
        var sicCreditEntries = await GetCreditEntries(Convert.ToInt32(credit), toDate);
        sicCredits.AddRange(sicCreditEntries);
      }

      return sicCredits.ToFixedList();
    }


    private async Task<List<SicCreditEntry>> GetCreditEntries(int creditId, DateTime toDate) {
      List<SicCreditEntry> sicCreditEntries = new List<SicCreditEntry>();

      var query = new MovtosEncabezadosDto {
        idCredito = creditId,
        fecConsulta = toDate.Year.ToString() + toDate.Month.ToString() + toDate.Day.ToString(),
      };

      FixedList<MovtosDetalleDto> movimientos = await _apiClient.TryGetMovtosDetalle(query);

      foreach (var movimiento in movimientos) {
        sicCreditEntries.Add(SicCreditEntry.MapToSicCreditEntry(movimiento, creditId));
      }

      return sicCreditEntries;
      ;
    }

    #endregion Helpers

  } // class SicServices

} // namespace Empiria.BanobrasIntegration.Sic
