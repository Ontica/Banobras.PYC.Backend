/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting External Interfaces       Component : Services                              *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Excel Importer                        *
*  Type     : BudgetTransactionImporter                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Service provider used to import budget transactions from Excel files.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.IO;

using Empiria.Commands;
using Empiria.Office;

using Empiria.Budgeting.Transactions;
using Empiria.Budgeting;

namespace Empiria.Banobras.Budgeting {

  /// <summary>Service provider used to import budget transactions from Excel files.</summary>
  internal class BudgetTransactionImporter {

    private readonly ImportBudgetTransactionCommand _command;
    private readonly FileInfo _excelFileInfo;

    internal BudgetTransactionImporter(ImportBudgetTransactionCommand command, FileInfo excelFileInfo) {
      Assertion.Require(command, nameof(command));
      Assertion.Require(excelFileInfo, nameof(excelFileInfo));

      _command = command;

      _excelFileInfo = excelFileInfo;
    }


    internal CommandResult<BudgetTransaction> Import() {

      //BudgetTransactionFields
      //BudgetEntryFields

      var excelFile = Spreadsheet.Open(_excelFileInfo.FullName);

      Tuple<int, int> entriesSection = GetEntriesSection(excelFile);

      var transactionName = excelFile.ReadCellValue<string>("A2");

      var entries = new List<ExcelBudgetEntry>(entriesSection.Item2 - entriesSection.Item1 + 1);

      for (int row = entriesSection.Item1; row <= entriesSection.Item2; row++) {

        var entry = ExcelBudgetEntry.CreateFrom(excelFile, row, transactionName);

        entry.Validate(Budget.Parse(_command.BudgetUID),
                       BudgetTransactionType.Parse(_command.TransactionTypeUID));

        entries.Add(entry);
      }

      var budgetTxn = BudgetTransaction.Empty;

      excelFile.Close();

      return BuildCommandResult(budgetTxn, transactionName, entries.ToFixedList());
    }


    private Tuple<int, int> GetEntriesSection(Spreadsheet excelFile) {
      int startRow = -1;
      int endRow = -1;

      int currentRow = 0;

      while (true) {

        currentRow++;

        if (currentRow > 20 && endRow == -1) {
          Assertion.RequireFail("El archivo Excel no tiene movimientos presupuestales.");
        }

        if (IsEntryRow(excelFile, currentRow)) {

          if (startRow == -1) {
            startRow = currentRow;
          }
          endRow = currentRow;

        } else if (startRow >= 1 && endRow >= 1) {

          return new Tuple<int, int>(startRow, endRow);

        }
      }
    }

    #region Helpers

    private CommandResult<BudgetTransaction> BuildCommandResult(BudgetTransaction budgetTxn,
                                                                string transactionName,
                                                                FixedList<ExcelBudgetEntry> entries) {

      var totals = new CommandTotals(transactionName, transactionName,
                                     entries.Count, budgetTxn.Entries.Count,
                                     entries.SelectFlat(x => x.Errors).Count);

      return new CommandResult<BudgetTransaction>(budgetTxn,
                                                  new CommandTotals[] { totals }.ToFixedList(),
                                                  errors: entries.SelectFlat(x => x.Errors).MapToNamedEntityList(false));
    }


    private bool IsEntryRow(Spreadsheet excelFile, int row) {

      if (row == 1) {
        return false;
      }

      if (excelFile.IsEmpty($"A{row}") ||
          excelFile.IsEmpty($"B{row}") ||
          excelFile.IsEmpty($"C{row}") ||
          excelFile.IsEmpty($"D{row}") ||
          excelFile.IsEmpty($"E{row}") ||
          excelFile.IsEmpty($"F{row}")) {
        return false;
      }

      if (excelFile.IsEmpty($"G{row}", true) &&
          excelFile.IsEmpty($"H{row}", true)) {
        return false;
      }

      if (excelFile.HasValue<decimal>($"G{row}") ||
          excelFile.HasValue<decimal>($"H{row}")) {
        return true;
      }

      return false;
    }

    #endregion Helpers

  }  // class BudgetTransactionImporter

}  // namespace Empiria.Banobras.Budgeting
