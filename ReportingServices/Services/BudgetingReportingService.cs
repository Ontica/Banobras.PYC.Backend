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

    public DynamicDto<BudgetExerciseJournalEntry> BuildBudgetExerciseJournal(DateTime fromDate, DateTime toDate) {

      FixedList<BudgetTransaction> transactions = GetJournalTransactions(fromDate, toDate);

      var builder = new BudgetExerciseJournalBuilder(transactions);

      var columns = builder.BuildColumns();
      var entries = builder.BuildEntries();

      return new DynamicDto<BudgetExerciseJournalEntry>(columns, entries);
    }


    public FileDto ExportBudgetExerciseJournal(DateTime fromDate, DateTime toDate) {
      var templateUID = $"{GetType().Name}.ExportBudgetExerciseJournalToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetExerciseJournalToExcelBuilder(templateConfig);

      FixedList<BudgetTransaction> transactions = GetJournalTransactions(fromDate, toDate);

      ExcelFile excelFile = exporter.CreateExcelFile(transactions);

      return excelFile.ToFileDto();
    }

    #endregion Services

    #region Helpers

    static private FixedList<BudgetTransaction> GetJournalTransactions(DateTime fromDate, DateTime toDate) {
      return BudgetTransaction.GetFullList<BudgetTransaction>()
                              .FindAll(x => (x.OperationType == BudgetOperationType.Exercise ||
                                            (x.OperationType == BudgetOperationType.ApprovePayment && x.PayableId <= 0)) &&
                                            (x.InProcess || x.IsClosed) &&
                                            (fromDate <= x.ApplicationDate && x.ApplicationDate < toDate.AddDays(1)));
    }


    #endregion Helpers

  } // class BudgetingReportingService

} // namespace Empiria.Budgeting.Reporting
