/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Services                   Component : Web Api Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                       Pattern   : Web Api Controller                   *
*  Type     : BudgetTransactionsReportsController           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Web API used to retrieve and edit budget transactions.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;
using System.Web.Http;

using Empiria.Storage;

using Empiria.WebApi;

using Empiria.Budgeting.Transactions;

using Empiria.Budgeting.Reporting;

namespace Empiria.Banobras.Reporting.WebApi {

  /// <summary>Web API used to retrieve and edit budget transactions.</summary>
  public class BudgetTransactionsReportsController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/budgeting/transactions/bulk-operation/export-entries-grouped")]
    public SingleObjectModel ExportGroupedEntriesToExcel([FromBody] BulkOperationCommand command) {

      FixedList<BudgetTransactionByYear> transactions =
            command.Items.Select(x => new BudgetTransactionByYear(BudgetTransaction.Parse(x)))
                         .ToFixedList()
                         .Sort((x, y) => x.Transaction.TransactionNo.CompareTo(y.Transaction.TransactionNo));

      using (var reportingService = BudgetTransactionReportingService.ServiceInteractor()) {

        var result = new FileResultDto(
            reportingService.ExportGroupedEntriesToExcel(transactions),
            $"Se exportaron {transactions.Count} transacciones presupuestales a Excel."
        );

        SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpPost]
    [Route("v2/budgeting/transactions/bulk-operation/export-entries-ungrouped")]
    public SingleObjectModel ExportUngroupedEntriesToExcel([FromBody] BulkOperationCommand command) {

      FixedList<BudgetTransaction> transactions =
            command.Items.Select(x => BudgetTransaction.Parse(x))
                         .ToFixedList()
                         .Sort((x, y) => x.TransactionNo.CompareTo(y.TransactionNo));

      using (var reportingService = BudgetTransactionReportingService.ServiceInteractor()) {

        var result = new FileResultDto(
            reportingService.ExportUngroupedEntriesToExcel(transactions),
            $"Se exportaron {transactions.Count} transacciones presupuestales a Excel."
        );

        SetOperation(result.Message);

        return new SingleObjectModel(base.Request, result);
      }
    }


    [HttpGet]
    [Route("v2/budgeting/transactions/{transactionUID:guid}/print")]
    public SingleObjectModel PrintTransaction([FromUri] string transactionUID) {

      using (var reportingService = BudgetTransactionReportingService.ServiceInteractor()) {


        var transaction = BudgetTransaction.Parse(transactionUID);

        FileDto file = reportingService.ExportTransactionVoucherToPdf(transaction);

        return new SingleObjectModel(base.Request, file);
      }
    }

    #endregion Web Apis

  }  // class BudgetTransactionsReportsController

}  // namespace Empiria.Banobras.Reporting.WebApi
