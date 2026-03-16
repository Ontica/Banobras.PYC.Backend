/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting External Interfaces       Component : Services                              *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Excel Importer                        *
*  Type     : BudgetTransactionImporter                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Service provider used to import budget transactions from Excel files.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.IO;

using Empiria.Commands;
using Empiria.Office;
using Empiria.Parties;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

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

      BudgetTransaction budgetTxn = ReadBudgetTransaction(excelFile);

      FixedList<ExcelBudgetEntry> entries = ReadBudgetEntries(excelFile, budgetTxn);

      excelFile.Close();

      return BuildCommandResult(budgetTxn, entries);
    }

    private BudgetTransaction ReadBudgetTransaction(Spreadsheet excelFile) {

      string orgUnitCode = excelFile.ReadCellValue("A2", string.Empty);

      Assertion.Require(orgUnitCode,
                        "El archivo Excel necesita tener el número de área en la celda A2.");

      var orgUnit = OrganizationalUnit.TryParseWithID(orgUnitCode);

      Assertion.Require(orgUnit, $"Celda A3: El área {orgUnitCode} no está registrada en el sistema.");

      string description = excelFile.ReadCellValue("A3", string.Empty);

      Assertion.Require(description,
                        "El archivo Excel necesita tener la " +
                        "descripción de la transacción presupuestal en la celda A3.");


      var txn = new BudgetTransaction(BudgetTransactionType.Parse(_command.TransactionTypeUID),
                                      Budget.Parse(_command.BudgetUID));

      var fields = new BudgetTransactionFields {
        BasePartyUID = orgUnit.UID,
        Description = description,
        AllowsOverdrafts = true,
        ApplicationDate = _command.ApplicationDate,
        OperationSourceUID = _command.OperationSourceUID,
        Justification = _command.Justification,
      };

      txn.Update(fields);

      return txn;
    }


    private FixedList<ExcelBudgetEntry> ReadBudgetEntries(Spreadsheet excelFile,
                                                          BudgetTransaction budgetTxn) {

      var entriesSection = GetEntriesSection(excelFile);

      var entries = new List<ExcelBudgetEntry>(entriesSection.EndRow - entriesSection.StartRow + 1);

      for (int row = entriesSection.StartRow; row <= entriesSection.EndRow; row++) {

        var entry = ExcelBudgetEntry.CreateFrom(excelFile, row, budgetTxn.Description);

        entry.Validate(Budget.Parse(_command.BudgetUID),
                       BudgetTransactionType.Parse(_command.TransactionTypeUID));

        entries.Add(entry);
      }

      return entries.ToFixedList();
    }

    #region Helpers

    private CommandResult<BudgetTransaction> BuildCommandResult(BudgetTransaction budgetTxn,
                                                                FixedList<ExcelBudgetEntry> entries) {

      var totals = new CommandTotals(budgetTxn.Description, budgetTxn.Description,
                                     entries.Count, budgetTxn.Entries.Count,
                                     entries.SelectFlat(x => x.Errors).Count);

      return new CommandResult<BudgetTransaction>(budgetTxn,
                                                  new CommandTotals[] { totals }.ToFixedList(),
                                                  errors: entries.SelectFlat(x => x.Errors).MapToNamedEntityList(false));
    }


    private (int StartRow, int EndRow) GetEntriesSection(Spreadsheet excelFile) {
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

          return (startRow, endRow);
        }
      }
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
