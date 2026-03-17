/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Services Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Excel file builder                      *
*  Type     : SialPayrollToExcelBuilder                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Exports a SIAL payroll to an Excel file according to Banobras budget transaction layout.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;

using Empiria.Office;
using Empiria.Storage;

using Empiria.Banobras.Budgeting.Adapters;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Exports a SIAL payroll to an Excel file according to Banobras budget transaction layout.</summary>
  internal class SialPayrollToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    internal SialPayrollToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(BudgetingTransactionDto budgetingTxn) {
      Assertion.Require(budgetingTxn, nameof(budgetingTxn));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader(budgetingTxn);

      FillOut(budgetingTxn);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader(BudgetingTransactionDto txn) {
      _excelFile.SetCell("A2", "191000");
      _excelFile.SetCell("B2", "DIRECCIÓN DE RECURSOS HUMANOS");

      _excelFile.SetCell("A3", txn.Description);
      _excelFile.SetCell("G3", DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"));
    }


    private void FillOut(BudgetingTransactionDto txn) {
      int i = 6;

      var entries = txn.Entries.OrderBy(x => x.BudgetAccount.OrganizationalUnit.Code)
                               .ThenBy(x => x.BudgetAccount.Code)
                               .ThenBy(x => x.OrgUnitCode);

      foreach (var entry in entries) {
        _excelFile.SetCell($"A{i}", entry.Year);
        _excelFile.SetCell($"B{i}", entry.Month);
        _excelFile.SetCell($"C{i}", entry.Day);

        if (!entry.BudgetAccount.IsEmptyInstance) {
          _excelFile.SetCell($"D{i}", entry.BudgetAccount.OrganizationalUnit.Code);
          _excelFile.SetCell($"E{i}", entry.BudgetAccount.Code);
          _excelFile.SetCell($"K{i}", entry.BudgetAccount.OrganizationalUnit.Name);
          _excelFile.SetCell($"L{i}", entry.BudgetAccount.Name);

        } else {
          _excelFile.SetCell($"D{i}", entry.OrgUnitCode);
          _excelFile.SetCell($"E{i}", entry.BudgetAccountNo);
          _excelFile.SetCell($"K{i}", entry.OrgUnitName);
          _excelFile.SetCell($"L{i}", entry.BudgetAccountName);
        }

        _excelFile.SetCell($"F{i}", "Ejercido");

        if (entry.Amount > 0) {
          _excelFile.SetCell($"G{i}", entry.Amount);
        } else {
          _excelFile.SetCell($"H{i}", Math.Abs(entry.Amount));
        }

        _excelFile.SetCell($"I{i}", string.Empty);  // isAdjustment
        _excelFile.SetCell($"J{i}", string.Empty);  // NumVerificacion

        _excelFile.SetCell($"M{i}", entry.OrgUnitCode);
        _excelFile.SetCell($"N{i}", entry.OrgUnitName);

        _excelFile.SetCell($"O{i}", entry.OperationNo);

        _excelFile.SetCell($"P{i}", entry.AccountingAcctNo);
        _excelFile.SetCell($"Q{i}", entry.AccountingAcctName);

        _excelFile.SetCell($"R{i}", $"Nómina correspondiente al área ({entry.OrgUnitCode}) {entry.OrgUnitName}");

        i++;
      }
    }

  }  // class SialPayrollToExcelBuilder

}  // namespace Empiria.BanobrasIntegration.Sial
