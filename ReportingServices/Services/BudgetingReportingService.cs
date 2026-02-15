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

using Empiria.DynamicData;
using Empiria.Office;
using Empiria.Services;
using Empiria.Storage;

using Empiria.Budgeting.Transactions;

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

    public DynamicDto<BudgetAllocationJournalEntry> AllocationJournalDynamicTable(DateTime fromDate, DateTime toDate) {

      FixedList<BudgetTransaction> transactions = GetAllocationJournalTransactions(fromDate, toDate);

      var builder = new BudgetAllocationJournalBuilder(transactions);

      var columns = builder.BuildColumns();
      var entries = builder.BuildEntries();

      return new DynamicDto<BudgetAllocationJournalEntry>(columns, entries);
    }


    public FileDto AllocationJournalToExcel(DateTime fromDate, DateTime toDate) {
      var templateUID = $"{GetType().Name}.BudgetAllocationJournal";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var builder = new BudgetAllocationJournalToExcelBuilder(templateConfig);

      FixedList<BudgetTransaction> transactions = GetAllocationJournalTransactions(fromDate, toDate);

      ExcelFile excelFile = builder.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }


    public DynamicDto<BudgetExerciseJournalEntry> ExerciseJournalDynamicTable(DateTime fromDate, DateTime toDate) {

      FixedList<BudgetTransaction> transactions = GetExerciseJournalTransactions(fromDate, toDate);

      var builder = new BudgetExerciseJournalBuilder(transactions);

      var columns = builder.BuildColumns();
      var entries = builder.BuildEntries();

      return new DynamicDto<BudgetExerciseJournalEntry>(columns, entries);
    }


    public FileDto ExerciseJournalToExcel(DateTime fromDate, DateTime toDate) {
      var templateUID = $"{GetType().Name}.BudgetExerciseJournal";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var builder = new BudgetExerciseJournalToExcelBuilder(templateConfig);

      FixedList<BudgetTransaction> transactions = GetExerciseJournalTransactions(fromDate, toDate);

      ExcelFile excelFile = builder.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }


    public DynamicDto<BudgetRequestsJournalEntry> RequestsJournalDynamicTable(DateTime fromDate, DateTime toDate) {

      FixedList<BudgetTransaction> transactions = GetRequestsJournalTransactions(fromDate, toDate);

      var builder = new BudgetRequestsJournalBuilder(transactions);

      var columns = builder.BuildColumns();
      var entries = builder.BuildEntries();

      return new DynamicDto<BudgetRequestsJournalEntry>(columns, entries);
    }


    public FileDto RequestsJournalToExcel(DateTime fromDate, DateTime toDate) {
      var templateUID = $"{GetType().Name}.BudgetRequestsJournal";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var builder = new BudgetRequestsJournalToExcelBuilder(templateConfig);

      FixedList<BudgetTransaction> transactions = GetRequestsJournalTransactions(fromDate, toDate);

      ExcelFile excelFile = builder.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }

    #endregion Services

    #region Helpers

    static private FixedList<BudgetTransaction> GetAllocationJournalTransactions(DateTime fromDate, DateTime toDate) {
      return BudgetTransaction.GetFullList<BudgetTransaction>()
                              .FindAll(x => (x.OperationType == BudgetOperationType.Authorize ||
                                             x.OperationType == BudgetOperationType.Expand ||
                                             x.OperationType == BudgetOperationType.Modify) &&
                                            (x.InProcess || x.IsClosed) &&
                                            (fromDate <= x.ApplicationDate && x.ApplicationDate < toDate.AddDays(1)))
                              .OrderBy(x => x.BaseParty.Code)
                              .ThenBy(x => x.ApplicationDate)
                              .ThenByDescending(x => x.BaseBudget.Name)
                              .ThenBy(x => x.TransactionNo)
                              .ToFixedList();
    }


    static private FixedList<BudgetTransaction> GetExerciseJournalTransactions(DateTime fromDate, DateTime toDate) {
      return BudgetTransaction.GetFullList<BudgetTransaction>()
                              .FindAll(x => (x.OperationType == BudgetOperationType.ApprovePayment ||
                                             x.OperationType == BudgetOperationType.Exercise) &&
                                            (x.InProcess || x.IsClosed) &&
                                            (fromDate <= x.ApplicationDate && x.ApplicationDate < toDate.AddDays(1)))
                              .OrderBy(x => x.BaseParty.Code)
                              .ThenBy(x => x.ApplicationDate)
                              .ThenByDescending(x => x.BaseBudget.Name)
                              .ThenBy(x => x.TransactionNo)
                              .ToFixedList();
    }


    static private FixedList<BudgetTransaction> GetRequestsJournalTransactions(DateTime fromDate, DateTime toDate) {
      return BudgetTransaction.GetFullList<BudgetTransaction>()
                              .FindAll(x => (x.OperationType == BudgetOperationType.Request ||
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

    #endregion Helpers

  } // class BudgetingReportingService

} // namespace Empiria.Budgeting.Reporting
