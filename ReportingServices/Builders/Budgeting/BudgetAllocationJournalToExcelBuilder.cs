/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetAllocationJournalToExcelBuilder         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with budget allocation entries.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Payments;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Builds an Excel file with budget allocation entries.</summary>
  internal class BudgetAllocationJournalToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public BudgetAllocationJournalToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<BudgetTransaction> transactions) {
      Assertion.Require(transactions, nameof(transactions));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(transactions);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(FixedList<BudgetTransaction> transactions) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var txn in transactions) {

        foreach (var entry in txn.Entries) {

          decimal amount = entry.Deposit > 0 ? entry.Deposit : entry.Withdrawal * -1;

          _excelFile.SetCell($"A{i}", entry.BudgetAccount.OrganizationalUnit.Code);
          _excelFile.SetCell($"B{i}", entry.BudgetAccount.Code);
          _excelFile.SetCell($"C{i}", entry.BudgetAccount.OrganizationalUnit.Name);
          _excelFile.SetCell($"D{i}", entry.BudgetAccount.Name);
          _excelFile.SetCell($"E{i}", entry.BudgetProgram.Code);
          _excelFile.SetCell($"F{i}", entry.Budget.Name);
          _excelFile.SetCell($"G{i}", txn.TransactionNo);
          _excelFile.SetCell($"H{i}", entry.MonthName);

          if (entry.BalanceColumn.Equals(BalanceColumn.Authorized)) {
            _excelFile.SetCell($"I{i}", amount);
          } else if (entry.BalanceColumn.Equals(BalanceColumn.Expanded)) {
            _excelFile.SetCell($"J{i}", amount);
          } else if (entry.BalanceColumn.Equals(BalanceColumn.Reduced)) {
            _excelFile.SetCell($"K{i}", amount);
          }

          _excelFile.SetCell($"L{i}", txn.Justification);
          _excelFile.SetCell($"M{i}", txn.Status.GetName());

          i++;
        }  // // foreach entry
      }  // foreach txn
    }

  } // class BudgetAllocationJournalToExcelBuilder

} // namespace Empiria.Budgeting.Reporting
