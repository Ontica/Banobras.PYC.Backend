/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetRequestsJournalToExcelBuilder           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with a given list of budget requests analytics entries.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Builds an Excel file with a given list of budget requests analytics entries.</summary>
  internal class BudgetRequestsAnalyticsToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public BudgetRequestsAnalyticsToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<BudgetRequestsAnalyticsEntry> entries) {
      Assertion.Require(entries, nameof(entries));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(entries);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(FixedList<BudgetRequestsAnalyticsEntry> entries) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var entry in entries) {

        _excelFile.SetCell($"A{i}", entry.BudgetAccount.OrganizationalUnit.Code);
        _excelFile.SetCell($"B{i}", entry.BudgetAccount.Code);

        if (!entry.BudgetAccount.StandardAccount.IsBaseAccount) {
          _excelFile.SetCell($"C{i}", entry.BudgetAccount.StandardAccount.BaseAccount.StdAcctNo);
        }

        _excelFile.SetCell($"D{i}", entry.BudgetProgram.Code);
        _excelFile.SetCell($"E{i}", entry.RequestTxn.TransactionNo);
        _excelFile.SetCell($"F{i}", entry.ControlNo);
        _excelFile.SetCell($"G{i}", entry.Amount);
        _excelFile.SetCell($"H{i}", entry.Requested);
        _excelFile.SetCell($"I{i}", entry.Committed);
        _excelFile.SetCell($"J{i}", entry.ToPay);
        _excelFile.SetCell($"K{i}", entry.Exercised);
        _excelFile.SetCell($"L{i}", entry.ToExercise);
        _excelFile.SetCell($"M{i}", entry.Description);
        if (!entry.CommitTxn.IsEmptyInstance) {
          _excelFile.SetCell($"N{i}", entry.CommitTxn.TransactionNo);
        }
        if (!entry.ApprovePaymentTxn.IsEmptyInstance) {
          _excelFile.SetCell($"O{i}", entry.ApprovePaymentTxn.TransactionNo);
        }
        if (!entry.ExerciseTxn.IsEmptyInstance) {
          _excelFile.SetCell($"P{i}", entry.ExerciseTxn.TransactionNo);
        }
        _excelFile.SetCell($"Q{i}", entry.Requisition.OrderNo);
        if (!entry.Contract.IsEmptyInstance) {
          _excelFile.SetCell($"R{i}", entry.Contract.ContractNo);
        }
        if (!entry.PayableOrder.IsEmptyInstance) {
          _excelFile.SetCell($"S{i}", entry.PayableOrder.OrderNo);
        }
        if (!entry.Contract.IsEmptyInstance) {
          _excelFile.SetCell($"T{i}", entry.Contract.Category.Name);
        } else if (!entry.PayableOrder.IsEmptyInstance) {
          _excelFile.SetCell($"T{i}", entry.PayableOrder.Category.Name);
        } else {
          _excelFile.SetCell($"T{i}", entry.Requisition.Category.Name);
        }

        if (!entry.PaymentOrder.IsEmptyInstance) {
          _excelFile.SetCell($"U{i}", entry.PaymentOrder.PaymentOrderNo);
          _excelFile.SetCell($"V{i}", entry.PaymentOrder.PayTo.Name);
          _excelFile.SetCell($"W{i}", entry.PaymentOrder.PaymentMethod.Name);
          _excelFile.SetCell($"X{i}", entry.PaymentOrder.PaymentAccount.AccountNo);
          _excelFile.SetCell($"Y{i}", entry.PaymentOrder.PaymentAccount.Institution.Name);
          _excelFile.SetCell($"Z{i}", entry.Debtor.Name);
          _excelFile.SetCell($"AA{i}", entry.AccountingVoucher);
        }

        _excelFile.SetCell($"AB{i}", entry.BudgetAccount.OrganizationalUnit.Name);
        _excelFile.SetCell($"AC{i}", entry.BudgetAccount.Name);
        _excelFile.SetCell($"AD{i}", entry.Budget.Name);

        i++;

      }  // foreach entry

    }

  } // class BudgetRequestsAnalyticsToExcelBuilder

} // namespace Empiria.Budgeting.Reporting
