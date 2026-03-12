/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting External Interfaces       Component : Exporters                             *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Excel Builder                         *
*  Type     : BudgetingTransactionExcelExporter            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Exports budgeting transaction data to an Excel file.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;

using Empiria.Banobras.Budgeting.Adapters;

namespace Empiria.Banobras.Budgeting.Exporters {

  /// <summary>Exports budgeting transaction data to an Excel file.</summary>
  internal class BudgetingTransactionExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    internal BudgetingTransactionExcelBuilder(FileTemplateConfig templateConfig) {
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
      _excelFile.SetCell(_templateConfig.TitleCell, txn.Description);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell, DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss"));
    }

    private void FillOut(BudgetingTransactionDto txn) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var entry in txn.Entries) {
        _excelFile.SetCell($"A{i}", entry.Year);
        _excelFile.SetCell($"B{i}", entry.Month);
        _excelFile.SetCell($"C{i}", entry.Day);

        if (!entry.BudgetAccount.IsEmptyInstance) {
          _excelFile.SetCell($"D{i}", entry.BudgetAccount.OrganizationalUnit.Code);
          _excelFile.SetCell($"H{i}", entry.BudgetAccount.OrganizationalUnit.Name);
          _excelFile.SetCell($"E{i}", entry.BudgetAccount.Code);
          _excelFile.SetCell($"I{i}", entry.BudgetAccount.Name);
        } else if (!entry.OrgUnit.IsEmptyInstance) {
          _excelFile.SetCell($"D{i}", entry.OrgUnit.Code);
          _excelFile.SetCell($"H{i}", entry.OrgUnit.Name);
        } else {
          _excelFile.SetCell($"D{i}", entry.OrgUnitCode);
          _excelFile.SetCell($"H{i}", $"El área {entry.OrgUnitCode} no existe en el sistema SIAL ni en PYC.");
        }

        if (entry.BudgetAccount.IsEmptyInstance) {
          _excelFile.SetCell($"E{i}", entry.BudgetAccountNo);
          _excelFile.SetCell($"I{i}", $"La relación área/partida o " +
                                      $"la partida {entry.BudgetAccountNo} no existe en el sistema PYC.");
        }

        _excelFile.SetCell($"F{i}", entry.Amount);
        _excelFile.SetCell($"G{i}", entry.OperationNo);
        _excelFile.SetCell($"J{i}", entry.AccountingAcctNo);
        _excelFile.SetCell($"K{i}", entry.AccountingAcctName);

        _excelFile.SetCell($"L{i}", $"Área original en el archivo SIAL: '{entry.OrgUnitCode}'");

        i++;
      }
    }

  }  // class BudgetingTransactionExcelExporter

}  // namespace Empiria.Banobras.Budgeting.Exporters
