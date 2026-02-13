/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetExerciseJournalToExcelBuilder           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with the results of a budget explorer request.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Budgeting.Transactions;
using Empiria.Payments;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Builds an Excel file with the results of a budget explorer request.</summary>
  internal class BudgetExerciseJournalToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public BudgetExerciseJournalToExcelBuilder(FileTemplateConfig templateConfig) {
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

        PaymentOrder paymentOrder = PaymentOrder.Parse(txn.PayableId);

        foreach (var entry in txn.Entries.FindAll(x => x.Deposit > 0)) {
          _excelFile.SetCell($"A{i}", entry.BudgetAccount.OrganizationalUnit.Code);
          _excelFile.SetCell($"B{i}", entry.BudgetAccount.Code);
          _excelFile.SetCell($"C{i}", entry.BudgetAccount.OrganizationalUnit.Name);
          _excelFile.SetCell($"D{i}", entry.BudgetAccount.Name);
          _excelFile.SetCell($"E{i}", entry.BudgetProgram.Code);
          _excelFile.SetCell($"F{i}", entry.Budget.Name);
          _excelFile.SetCell($"G{i}", entry.ControlNo);
          _excelFile.SetCell($"H{i}", txn.TransactionNo);
          _excelFile.SetCell($"I{i}", string.Empty);  // txnAutpago
          _excelFile.SetCell($"J{i}", entry.MonthName);
          if (txn.OperationType == BudgetOperationType.Exercise) {
            _excelFile.SetCell($"K{i}", 0);
            _excelFile.SetCell($"L{i}", entry.Amount);
          } else {
            _excelFile.SetCell($"K{i}", entry.Amount);
            _excelFile.SetCell($"L{i}", 0);
          }
          _excelFile.SetCell($"M{i}", entry.Description);
          if (!paymentOrder.IsEmptyInstance && paymentOrder.Payed) {
            _excelFile.SetCell($"N{i}", paymentOrder.PaymentOrderNo);
            _excelFile.SetCell($"O{i}", paymentOrder.PaymentMethod.Name);
            _excelFile.SetCell($"P{i}", paymentOrder.PayTo.Name);
            _excelFile.SetCell($"Q{i}", paymentOrder.PayTo.Code);
            _excelFile.SetCell($"R{i}", paymentOrder.LastPaymentInstruction.LastUpdateTime.ToString("dd/MMM/yyyy HH:mm"));
          } else if (!paymentOrder.IsEmptyInstance) {
            _excelFile.SetCell($"N{i}", paymentOrder.PaymentOrderNo);
            _excelFile.SetCell($"O{i}", paymentOrder.PaymentMethod.Name);
            _excelFile.SetCell($"P{i}", paymentOrder.PayTo.Name);
            _excelFile.SetCell($"Q{i}", paymentOrder.PayTo.Code);
            _excelFile.SetCell($"R{i}", "Por pagar");
          } else if (paymentOrder.IsEmptyInstance) {
            _excelFile.SetCell($"N{i}", "Por determinar");
            _excelFile.SetCell($"O{i}", string.Empty);
            _excelFile.SetCell($"P{i}", string.Empty);
            _excelFile.SetCell($"Q{i}", string.Empty);
            _excelFile.SetCell($"R{i}", string.Empty);
          }
          _excelFile.SetCell($"W{i}", txn.Status.GetName());
          _excelFile.SetCell($"X{i}", txn.Justification);

          i++;

        }
      }
    }

  } // class BudgetExplorerResultBuilder

} // namespace Empiria.Budgeting.Reporting
