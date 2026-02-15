/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : BudgetExerciseJournalToExcelBuilder           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with budget exercise entries.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Payments;

using Empiria.Budgeting.Transactions;

namespace Empiria.Budgeting.Reporting {

  /// <summary>Builds an Excel file with budget exercise entries.</summary>
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
          _excelFile.SetCell($"C{i}", entry.BudgetProgram.Code);
          _excelFile.SetCell($"D{i}", entry.ControlNo);
          _excelFile.SetCell($"E{i}", entry.Year);
          _excelFile.SetCell($"F{i}", entry.MonthName);

          if (txn.OperationType == BudgetOperationType.Exercise) {
            _excelFile.SetCell($"G{i}", 0);
            _excelFile.SetCell($"H{i}", entry.Amount);
          } else {
            _excelFile.SetCell($"G{i}", entry.Amount);
            _excelFile.SetCell($"H{i}", 0);
          }
          _excelFile.SetCell($"I{i}", txn.TransactionNo);
          _excelFile.SetCell($"J{i}", EmpiriaString.FirstWithValue(entry.Description,
                                                                   txn.Description,
                                                                   txn.Justification));
          _excelFile.SetCell($"K{i}", string.Empty);  // txnAutpago

          if (!paymentOrder.IsEmptyInstance && paymentOrder.Payed) {
            _excelFile.SetCell($"L{i}", paymentOrder.PaymentOrderNo);
            _excelFile.SetCell($"M{i}", paymentOrder.PaymentMethod.Name);
            _excelFile.SetCell($"N{i}", paymentOrder.PayTo.Name);
            _excelFile.SetCell($"O{i}", paymentOrder.PayTo.Code);
            _excelFile.SetCell($"P{i}", paymentOrder.LastPaymentInstruction.LastUpdateTime.ToString("dd/MMM/yyyy HH:mm"));

          } else if (!paymentOrder.IsEmptyInstance) {
            _excelFile.SetCell($"L{i}", paymentOrder.PaymentOrderNo);
            _excelFile.SetCell($"M{i}", paymentOrder.PaymentMethod.Name);
            _excelFile.SetCell($"N{i}", paymentOrder.PayTo.Name);
            _excelFile.SetCell($"O{i}", paymentOrder.PayTo.Code);
            _excelFile.SetCell($"P{i}", "Por pagar");

          } else if (paymentOrder.IsEmptyInstance) {
            _excelFile.SetCell($"L{i}", "Por determinar");
            _excelFile.SetCell($"M{i}", string.Empty);
            _excelFile.SetCell($"N{i}", string.Empty);
            _excelFile.SetCell($"O{i}", string.Empty);
            _excelFile.SetCell($"P{i}", string.Empty);
          }

          // ToDo: Accounting data

          _excelFile.SetCell($"U{i}", txn.RequestedDate.ToString("dd/MMM/yyyy"));
          _excelFile.SetCell($"V{i}", txn.RequestedBy.Name);
          _excelFile.SetCell($"W{i}", txn.AuthorizationDate.ToString("dd/MMM/yyyy"));
          _excelFile.SetCell($"X{i}", txn.AuthorizedBy.Name);
          _excelFile.SetCell($"Y{i}", entry.BudgetAccount.OrganizationalUnit.Name);
          _excelFile.SetCell($"Z{i}", entry.BudgetAccount.Name);
          _excelFile.SetCell($"AA{i}", entry.Budget.Name);
          _excelFile.SetCell($"AB{i}", txn.Status.GetName());

          i++;
        }  // // foreach entry
      }  // foreach txn
    }

  } // class BudgetExerciseJournalToExcelBuilder

} // namespace Empiria.Budgeting.Reporting
