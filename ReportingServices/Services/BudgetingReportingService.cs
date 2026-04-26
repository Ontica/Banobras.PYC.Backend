/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetingReportingService                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate budgeting reports.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;
using System.Threading.Tasks;

using Empiria.DynamicData;
using Empiria.Office;
using Empiria.Services;
using Empiria.Storage;

using Empiria.Budgeting.Transactions;

using Empiria.Banobras.Reporting;
using Empiria.Banobras.Reporting.Builders.Budgeting;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Provides services used to generate budgeting reports.</summary>
  public class BudgetingReportingService : Service {

    #region Constructors and parsers

    private BudgetingReportingService() {
      // no-op
    }

    static public BudgetingReportingService ServiceInteractor() {
      return CreateInstance<BudgetingReportingService>();
    }


    #endregion Constructors and parsers

    #region Services

    public DynamicDto<BudgetAllocationJournalEntry> AllocationJournalDynamicTable(PYCReportFields fields) {

      FixedList<BudgetTransaction> transactions = GetAllocationJournalTransactions(fields);

      var builder = new BudgetAllocationJournalBuilder(transactions);

      var columns = builder.BuildColumns();
      var entries = builder.BuildEntries();

      return new DynamicDto<BudgetAllocationJournalEntry>(columns, entries);
    }


    public FileDto AllocationJournalToExcel(PYCReportFields fields) {
      var templateUID = $"{GetType().Name}.BudgetAllocationJournal";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var builder = new BudgetAllocationJournalToExcelBuilder(templateConfig);

      FixedList<BudgetTransaction> transactions = GetAllocationJournalTransactions(fields);

      ExcelFile excelFile = builder.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }


    public DynamicDto<BudgetExerciseJournalEntry> ExerciseJournalDynamicTable(PYCReportFields fields) {

      FixedList<BudgetTransaction> transactions = GetExerciseJournalTransactions(fields);

      var builder = new BudgetExerciseJournalBuilder(transactions);

      var columns = builder.BuildColumns();
      var entries = builder.BuildEntries();

      return new DynamicDto<BudgetExerciseJournalEntry>(columns, entries);
    }


    public FileDto ExerciseJournalToExcel(PYCReportFields fields) {
      var templateUID = $"{GetType().Name}.BudgetExerciseJournal";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var builder = new BudgetExerciseJournalToExcelBuilder(templateConfig);

      FixedList<BudgetTransaction> transactions = GetExerciseJournalTransactions(fields);

      ExcelFile excelFile = builder.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }


    public DynamicDto<BudgetRequestsAnalyticsEntryDto> RequestsAnalyticsDynamicTable(PYCReportFields fields) {

      FixedList<BudgetTransaction> transactions = GetRequestsJournalTransactions(fields);

      var builder = new BudgetRequestsAnalyticsBuilder(transactions);

      var entries = builder.BuildEntries();

      var mapper = new BudgetRequestsAnalyticsDynamicTableMapper(entries);

      var columns = mapper.BuildColumns();
      var mappedEntries = mapper.BuildEntries();

      return new DynamicDto<BudgetRequestsAnalyticsEntryDto>(columns, mappedEntries);
    }



    public FileDto RequestsAnalyticsToExcel(PYCReportFields fields) {
      var templateUID = $"{GetType().Name}.BudgetRequestsAnalytics";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      FixedList<BudgetTransaction> transactions = GetRequestsJournalTransactions(fields);

      var analyticsBuilder = new BudgetRequestsAnalyticsBuilder(transactions);

      var entries = analyticsBuilder.BuildEntries();

      var excelBuilder = new BudgetRequestsAnalyticsToExcelBuilder(templateConfig);

      ExcelFile excelFile = excelBuilder.CreateExcelFile(entries);

      return excelFile.ToFileDto();
    }


    public DynamicDto<BudgetRequestsJournalEntry> RequestsJournalDynamicTable(PYCReportFields fields) {

      FixedList<BudgetTransaction> transactions = GetRequestsJournalTransactions(fields);

      var builder = new BudgetRequestsJournalBuilder(transactions);

      var columns = builder.BuildColumns();
      var entries = builder.BuildEntries();

      return new DynamicDto<BudgetRequestsJournalEntry>(columns, entries);
    }


    public FileDto RequestsJournalToExcel(PYCReportFields fields) {
      var templateUID = $"{GetType().Name}.BudgetRequestsJournal";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var builder = new BudgetRequestsJournalToExcelBuilder(templateConfig);

      FixedList<BudgetTransaction> transactions = GetRequestsJournalTransactions(fields);

      ExcelFile excelFile = builder.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }

    #endregion Services

    #region Helpers

    static private FixedList<BudgetTransaction> GetAllocationJournalTransactions(PYCReportFields fields) {
      return BudgetTransaction.GetFullList<BudgetTransaction>()
                              .FindAll(x => x.BaseBudget.BudgetType.Equals(fields.BudgetType) &&
                                            (x.OperationType == BudgetOperationType.Authorize ||
                                             x.OperationType == BudgetOperationType.Expand ||
                                             x.OperationType == BudgetOperationType.Reduce ||
                                             x.OperationType == BudgetOperationType.Modify) &&
                                            (x.InProcess || x.IsClosed) &&
                                            (fields.FromDate <= x.ApplicationDate && x.ApplicationDate < fields.ToDate.AddDays(1)))
                              .OrderBy(x => x.BaseParty.Code)
                              .ThenBy(x => x.ApplicationDate)
                              .ThenByDescending(x => x.BaseBudget.Name)
                              .ThenBy(x => x.TransactionNo)
                              .ToFixedList();
    }


    static private FixedList<BudgetTransaction> GetExerciseJournalTransactions(PYCReportFields fields) {
      return BudgetTransaction.GetFullList<BudgetTransaction>()
                              .FindAll(x => x.BaseBudget.BudgetType.Equals(fields.BudgetType) &&
                                           (x.OperationType == BudgetOperationType.ApprovePayment ||
                                             x.OperationType == BudgetOperationType.Exercise) &&
                                            (x.InProcess || x.IsClosed) &&
                                            (fields.FromDate <= x.ApplicationDate && x.ApplicationDate < fields.ToDate.AddDays(1)))
                              .OrderBy(x => x.BaseParty.Code)
                              .ThenBy(x => x.ApplicationDate)
                              .ThenByDescending(x => x.BaseBudget.Name)
                              .ThenBy(x => x.TransactionNo)
                              .ToFixedList();
    }


    static private FixedList<BudgetTransaction> GetRequestsJournalTransactions(PYCReportFields fields) {
      var fromDate = new DateTime(fields.ToDate.Year, 1, 1);
      var toDate = fields.ToDate;

      return BudgetTransaction.GetFullList<BudgetTransaction>()
                              .FindAll(x => x.BaseBudget.BudgetType.Equals(fields.BudgetType) &&
                                           (x.OperationType == BudgetOperationType.Request ||
                                             x.OperationType == BudgetOperationType.Commit ||
                                             x.OperationType == BudgetOperationType.ApprovePayment ||
                                             x.OperationType == BudgetOperationType.Exercise) &&
                                            (x.InProcess || x.IsClosed) &&
                                            (fromDate <= x.ApplicationDate && x.ApplicationDate < toDate.AddDays(1)))
                              .OrderBy(x => x.BaseParty.Code)
                              .ThenBy(x => x.ApplicationDate)
                              .ThenByDescending(x => x.BaseBudget.Name)
                              .ThenBy(x => x.TransactionNo)
                              .ToFixedList();
    }


    public async Task<DynamicDto<ExerciseReconciliationDto>> GetBudgetExerciseAccountingReconciliation(PYCReportFields fields) {

      var reconciliator = new BudgetExerciseAccountingReconciliator(fields.FromDate, fields.ToDate);

      FixedList<ExerciseReconciliationDto> reconciliation = await reconciliator.Build();

      var columns = new DataTableColumn[] {
        new DataTableColumn("budgetTransactionNo", "Transaccion", "text-nowrap"),
        new DataTableColumn("budgetTransactionOrgUnit", "Área", "text-nowrap"),
        new DataTableColumn("budgetAccountName", "Partida", "text-nowrap"),
        new DataTableColumn("budgetControlNumber", "Num verif", "text-nowrap"),
        new DataTableColumn("budgetExercise", "Ejercido", "decimal"),
        new DataTableColumn("accountingVoucherNo", "Póliza contable", "text-nowrap"),
        new DataTableColumn("accountNumber", "Cuenta contable", "text-nowrap"),
        new DataTableColumn("accountName", "Nombre de la cuenta contable", "text") { Size = "lg" },
        new DataTableColumn("verificationNumber", "Num verif (contable)", "text") { Size = "sm"} ,
        new DataTableColumn("debit", "Cargos", "decimal"),
        new DataTableColumn("credit", "Abonos", "decimal"),

      }.ToFixedList();

      return new DynamicDto<ExerciseReconciliationDto>(columns, reconciliation);
    }

    #endregion Helpers

  } // class BudgetingReportingService

} // namespace Empiria.Budgeting.Reporting
