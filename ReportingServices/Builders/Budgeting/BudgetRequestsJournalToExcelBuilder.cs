/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetRequestsJournalToExcelBuilder           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with budget requests entries.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Builds an Excel file with budget requests entries.</summary>
  internal class BudgetRequestsJournalToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public BudgetRequestsJournalToExcelBuilder(FileTemplateConfig templateConfig) {
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

          _excelFile.SetCell($"A{i}", entry.BudgetAccount.OrganizationalUnit.Code);
          _excelFile.SetCell($"B{i}", entry.BudgetAccount.Code);
          _excelFile.SetCell($"C{i}", entry.BudgetProgram.Code);
          _excelFile.SetCell($"D{i}", entry.ControlNo);
          _excelFile.SetCell($"E{i}", entry.Year);
          _excelFile.SetCell($"F{i}", entry.MonthName);
          if (entry.BalanceColumn.Equals(BalanceColumn.Requested)) {
            _excelFile.SetCell($"G{i}", entry.Deposit);
          } else if (entry.BalanceColumn.Equals(BalanceColumn.Commited)) {
            _excelFile.SetCell($"H{i}", entry.Deposit);
          } else if (entry.BalanceColumn.Equals(BalanceColumn.ToPay)) {
            _excelFile.SetCell($"I{i}", entry.Deposit);
          } else if (entry.BalanceColumn.Equals(BalanceColumn.Exercised)) {
            _excelFile.SetCell($"J{i}", entry.Deposit);
          }
          _excelFile.SetCell($"K{i}", txn.TransactionNo);
          _excelFile.SetCell($"L{i}", EmpiriaString.FirstWithValue(entry.Description,
                                                                   txn.Description,
                                                                   txn.Justification));
          _excelFile.SetCell($"M{i}", txn.RequestedDate.ToString("dd/MMM/yyyy"));
          _excelFile.SetCell($"N{i}", txn.RequestedBy.Name);
          _excelFile.SetCell($"O{i}", txn.AuthorizationDate.ToString("dd/MMM/yyyy"));
          _excelFile.SetCell($"P{i}", txn.AuthorizedBy.Name);
          _excelFile.SetCell($"Q{i}", entry.BudgetAccount.OrganizationalUnit.Name);
          _excelFile.SetCell($"R{i}", entry.BudgetAccount.Name);
          _excelFile.SetCell($"S{i}", entry.Budget.Name);
          _excelFile.SetCell($"T{i}", txn.Status.GetName());

          i++;
        }  // // foreach entry
      }  // foreach txn
    }

  } // class BudgetRequestsJournalToExcelBuilder

} // namespace Empiria.Budgeting.Reporting
