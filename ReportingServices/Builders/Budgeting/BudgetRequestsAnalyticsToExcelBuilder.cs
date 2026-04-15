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
using Empiria.Payments;
using Empiria.StateEnums;
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
        _excelFile.SetCell($"B{i}", entry.BudgetAccount.OrganizationalUnit.Name);
        _excelFile.SetCell($"C{i}", entry.BudgetAccount.Code);
        _excelFile.SetCell($"D{i}", entry.BudgetAccount.StandardAccount.Description);

        if (!entry.BudgetAccount.StandardAccount.IsBaseAccount) {
          _excelFile.SetCell($"E{i}", entry.BudgetAccount.StandardAccount.BaseAccount.StdAcctNo);
        }

        _excelFile.SetCell($"F{i}", entry.BudgetProgram.Code);
        _excelFile.SetCell($"G{i}", entry.RequestTxn.TransactionNo);
        _excelFile.SetCell($"H{i}", entry.RequestTxn.ApplicationDate);
        _excelFile.SetCell($"I{i}", entry.ControlNo);
        if (!entry.ApprovePaymentTxn.IsEmptyInstance) {
          _excelFile.SetCell($"J{i}", entry.ApprovePaymentTxn.Status.GetName());
        } else {
          _excelFile.SetCell($"J{i}", entry.RequestTxn.Status.GetName());
        }
        _excelFile.SetCell($"K{i}", entry.Amount);
        _excelFile.SetCell($"L{i}", entry.Requested);
        _excelFile.SetCell($"M{i}", entry.Committed);
        _excelFile.SetCell($"N{i}", entry.ToPay);
        _excelFile.SetCell($"O{i}", entry.Exercised);
        _excelFile.SetCell($"P{i}", entry.ToExercise);
        _excelFile.SetCell($"Q{i}", entry.Description);


        if (!entry.CommitTxn.IsEmptyInstance) {
          _excelFile.SetCell($"R{i}", entry.CommitTxn.TransactionNo);
          _excelFile.SetCell($"S{i}", entry.CommitTxn.ApplicationDate);
        }

        if (!entry.ApprovePaymentTxn.IsEmptyInstance) {
          _excelFile.SetCell($"T{i}", entry.ApprovePaymentTxn.TransactionNo);
          _excelFile.SetCell($"U{i}", entry.ApprovePaymentTxn.ApplicationDate);
        }

        if (!entry.ExerciseTxn.IsEmptyInstance) {
          _excelFile.SetCell($"V{i}", entry.ExerciseTxn.TransactionNo);
        }

        _excelFile.SetCell($"W{i}", entry.Requisition.OrderNo);

        if (!entry.Contract.IsEmptyInstance) {
          _excelFile.SetCell($"X{i}", entry.Contract.ContractNo);
        }
        if (!entry.PayableOrder.IsEmptyInstance) {
          _excelFile.SetCell($"Y{i}", entry.PayableOrder.OrderNo);
        }

        if (!entry.Contract.IsEmptyInstance) {
          _excelFile.SetCell($"Z{i}", entry.Contract.Category.Name);
        } else if (!entry.PayableOrder.IsEmptyInstance) {
          _excelFile.SetCell($"Z{i}", entry.PayableOrder.Category.Name);
        } else {
          _excelFile.SetCell($"Z{i}", entry.Requisition.Category.Name);
        }

        if (!entry.PaymentOrder.IsEmptyInstance) {
          _excelFile.SetCell($"AA{i}", entry.PaymentOrder.PaymentOrderNo);
          _excelFile.SetCell($"AB{i}", entry.PaymentOrder.PayTo.Name);
          _excelFile.SetCell($"AC{i}", ((Payee) entry.PaymentOrder.PayTo).TaxCode);
          _excelFile.SetCell($"AD{i}", entry.PaymentOrder.PaymentMethod.Name);
          _excelFile.SetCell($"AE{i}", entry.PaymentOrder.PaymentAccount.AccountNo);
          _excelFile.SetCell($"AF{i}", entry.PaymentOrder.PaymentAccount.Institution.Name);
          _excelFile.SetCell($"AG{i}", entry.PaymentOrder.Payed ?
                  entry.PaymentOrder.LastPaymentInstruction.LastUpdateTime.ToString("dd/MMM/yyyy HH:mm") : string.Empty);
          _excelFile.SetCell($"AH{i}", entry.Debtor.Name);
          _excelFile.SetCell($"AI{i}", entry.Debtor.EmployeeNo);
          _excelFile.SetCell($"AJ{i}", entry.AccountingVoucher);
        }

        _excelFile.SetCell($"AK{i}", entry.Budget.Name);

        i++;

      }  // foreach entry

    }

  } // class BudgetRequestsAnalyticsToExcelBuilder

} // namespace Empiria.Budgeting.Reporting
