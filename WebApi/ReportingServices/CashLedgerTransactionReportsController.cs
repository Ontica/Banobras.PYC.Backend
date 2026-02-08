/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Cash Flow Management                 Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Query Web Api Controller             *
*  Type     : CashLedgerTransactionReportsController        License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to retrieve and update transactions.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.Storage;
using Empiria.WebApi;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.CashLedger.UseCases;

using Empiria.CashFlow.Reporting;

namespace Empiria.Banobras.Reporting.WebApi {

  /// <summary>Web API used to retrieve and update cash ledger transactions.</summary>
  public class CashLedgerTransactionReportsController : WebApiController {

    #region Web apis

    [HttpGet]
    [Route("v1/cash-flow/cash-ledger/transactions/{id:long}/print")]
    public async Task<SingleObjectModel> GetCashTransactionAsPdfFile([FromUri] long id) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        CashTransactionHolderDto transaction = await usecases.GetTransaction(id);

        var reportingService = CashTransactionReportingService.ServiceInteractor();

        FileDto pdfFile = reportingService.ExportTransactionToPdf(transaction);

        return new SingleObjectModel(base.Request, pdfFile);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/bulk-operation/export-analysis")]
    public async Task<SingleObjectModel> ExportAccountsAnalysisToExcel([FromBody] BulkOperationCommand command) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionHolderDto> txns = await usecases.GetTransactions(command.GetConvertedIds());

        FixedList<CashEntryDto> entries = txns.SelectFlat(x => x.Entries);

        var reportingService = CashTransactionReportingService.ServiceInteractor();

        var result = new FileResultDto(
            reportingService.ExportAccountsAnalysisToExcel(entries),
            $"Se exportó el análisis de {txns.Count} pólizas a Excel."
        );

        base.SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/bulk-operation/export-entries")]
    public async Task<SingleObjectModel> ExportCashTransactionsToExcel([FromBody] BulkOperationCommand command) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionHolderDto> transactions = await usecases.GetTransactions(command.GetConvertedIds());

        var reportingService = CashTransactionReportingService.ServiceInteractor();

        var result = new FileResultDto(
            reportingService.ExportTransactionsToExcel(transactions),
            $"Se exportaron {transactions.Count} pólizas a Excel."
        );

        base.SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/transactions/bulk-operation/export-totals")]
    public async Task<SingleObjectModel> ExportCashTransactionsTotalsToExcel([FromBody] BulkOperationCommand command) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashTransactionHolderDto> transactions = await usecases.GetTransactions(command.GetConvertedIds());

        var reportingService = CashTransactionReportingService.ServiceInteractor();

        var result = new FileResultDto(
            reportingService.ExportTransactionsTotalsToExcel(transactions),
            $"Se exportaron {transactions.Count} pólizas a Excel."
        );

        base.SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v1/cash-flow/cash-ledger/entries/bulk-operation/export")]
    public async Task<SingleObjectModel> ExportCashEntriesToExcel([FromBody] BulkOperationCommand command) {

      using (var usecases = CashTransactionUseCases.UseCaseInteractor()) {
        FixedList<CashEntryExtendedDto> entries = await usecases.GetTransactionsEntries(command.GetConvertedIds());

        var reportingService = CashTransactionReportingService.ServiceInteractor();

        var result = new FileResultDto(
            reportingService.ExportTransactionsEntriesToExcel(entries),
            $"Se exportaron {entries.Count} movimientos a Excel."
        );

        base.SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }

    #endregion Web apis


    public class BulkOperationCommand {

      public string[] Items {
        get; set;
      }


      public FixedList<long> GetConvertedIds() {
        return Items.ToFixedList()
                    .Select(x => long.Parse(x))
                    .ToFixedList();
      }

    }  // class BulkOperationCommand


  }  // class CashLedgerTransactionReportsController

}  // namespace Empiria.Banobras.Reporting.WebApi
