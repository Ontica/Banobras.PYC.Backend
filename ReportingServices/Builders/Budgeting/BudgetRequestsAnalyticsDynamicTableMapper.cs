/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : BudgetRequestsAnalyticsDynamicTableMapper     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Maps a list of BudgetRequestsAnalyticsEntry to display them in dynamic tables.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.DynamicData;

namespace Empiria.Budgeting.Reporting {

  public class BudgetRequestsAnalyticsEntryDto {

    public string OrgUnit {
      get; internal set;
    }

    public string BudgetAccount {
      get; internal set;
    }

    public string BudgetBaseAccount {
      get; internal set;
    }

    public string BudgetProgram {
      get; internal set;
    }

    public string Budget {
      get; internal set;
    }

    public string ControlNo {
      get; internal set;
    }

    public string RequestTxnUID {
      get; internal set;
    }

    public string RequestTxnNo {
      get; internal set;
    }

    public string CommitTxnUID {
      get; internal set;
    }

    public string CommitTxnNo {
      get; internal set;
    }

    public string ApprovePaymentTxnUID {
      get; internal set;
    }

    public string ApprovePaymentTxnNo {
      get; internal set;
    }

    public string ExerciseTxnUID {
      get; internal set;
    }

    public string ExerciseTxnNo {
      get; internal set;
    }

    public decimal Amount {
      get; internal set;
    }

    public decimal Requested {
      get; internal set;
    }

    public decimal Committed {
      get; internal set;
    }

    public decimal ToPay {
      get; internal set;
    }

    public decimal Exercised {
      get; internal set;
    }

    public decimal ToExercise {
      get {
        return Requested + Committed + ToPay;
      }
    }

    public string Month {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string RequisitionNo {
      get; internal set;
    }

    public string ContractNo {
      get; internal set;
    }

    public string OrderNo {
      get; internal set;
    }

    public string OrderType {
      get; internal set;
    }

    public string PaymentOrderNo {
      get; internal set;
    }

    public string PayTo {
      get; internal set;
    }

    public string PaymentMethod {
      get; internal set;
    }

    public string PaymentAccount {
      get; internal set;
    }

    public string PaymentInstitution {
      get; internal set;
    }

    public DateTime? PaymentDate {
      get; internal set;
    }

    public string Debtor {
      get; internal set;
    }

    public string AccountingVoucher {
      get; internal set;
    }


  }  // class BudgetRequestsAnalyticsEntryDto



  /// <summary>Maps a list of BudgetRequestsAnalyticsEntry to display them in dynamic tables.</summary>
  public class BudgetRequestsAnalyticsDynamicTableMapper {

    private readonly FixedList<BudgetRequestsAnalyticsEntry> _entries;

    internal BudgetRequestsAnalyticsDynamicTableMapper(FixedList<BudgetRequestsAnalyticsEntry> entries) {
      _entries = entries;
    }


    internal FixedList<DataTableColumn> BuildColumns() {
      return new List<DataTableColumn>() {
        new DataTableColumn("orgUnit", "Área", "text"),
        new DataTableColumn("budgetAccount", "Partida", "text"),
        new DataTableColumn("budgetBaseAccount", "Capítulo", "text"),
        new DataTableColumn("budgetProgram", "Programa", "text"),
        new DataTableColumn("requestTxnNo", "Suficiencia", "text-link",
                            linkField: "requestTxnUID", action: "ViewBudgetTransaction"),
        new DataTableColumn("controlNo", "Núm Verif", "text-nowrap"),
        new DataTableColumn("amount", "Importe", "decimal"),
        new DataTableColumn("requested", "Apartado", "decimal"),
        new DataTableColumn("committed", "Comprometido", "decimal"),
        new DataTableColumn("toPay", "Por pagar", "decimal"),
        new DataTableColumn("exercised", "Ejercido", "decimal"),
        new DataTableColumn("toExercise", "Por ejercer", "decimal"),
        new DataTableColumn("commitTxnNo", "Compromiso", "text-link",
                            linkField: "commitTxnUID", action: "ViewBudgetTransaction"),
        new DataTableColumn("approvePaymentTxnNo", "Aut de pago", "text-link",
                            linkField: "approvePaymentTxnUID", action: "ViewBudgetTransaction"),
        new DataTableColumn("exerciseTxnNo", "Ejercicio", "text-link",
                            linkField: "exerciseTxnUID", action: "ViewBudgetTransaction"),
        new DataTableColumn("month", "Mes", "text"),
        new DataTableColumn("description", "Descripción", "text"),
        new DataTableColumn("requisitionNo", "Requisición", "text-nowrap"),
        new DataTableColumn("contractNo", "Contrato", "text"),
        new DataTableColumn("orderNo", "Orden", "text-nowrap"),
        new DataTableColumn("orderType", "Tipo", "text"),
        new DataTableColumn("paymentOrderNo", "Solicitud pago", "text-nowrap"),
        new DataTableColumn("payTo", "Pagar a", "text"),
        new DataTableColumn("paymentMethod", "Método de pago", "text"),
        new DataTableColumn("paymentAccount", "Cuenta de pago", "text-nowrap"),
        new DataTableColumn("paymentInstitution", "Institución", "text"),
        new DataTableColumn("paymentDate", "Fecha de pago", "date"),
        new DataTableColumn("debtor", "Deudor", "text"),
        new DataTableColumn("accountingVoucher", "Póliza contable", "text-nowrap"),
        new DataTableColumn("budget", "Presupuesto", "text"),
      }.ToFixedList();
    }


    internal FixedList<BudgetRequestsAnalyticsEntryDto> BuildEntries() {
      return _entries.Select(x => x.ControlNo.Contains("/") ?
                                        MapBuildExerciseEntry(x) : MapBuildRequestEntry(x))
                     .ToFixedList();
    }

    private BudgetRequestsAnalyticsEntryDto MapBuildExerciseEntry(BudgetRequestsAnalyticsEntry entry) {
      return new BudgetRequestsAnalyticsEntryDto {
        OrgUnit = entry.BudgetAccount.OrganizationalUnit.FullName,
        BudgetAccount = ((INamedEntity) entry.BudgetAccount).Name,
        BudgetBaseAccount = entry.BudgetAccount.StandardAccount.IsBaseAccount ? string.Empty :
                                                entry.BudgetAccount.StandardAccount.BaseAccount.StdAcctNo,
        BudgetProgram = entry.BudgetProgram.Code,
        RequestTxnUID = entry.RequestTxn.UID,
        RequestTxnNo = entry.RequestTxn.TransactionNo,
        CommitTxnUID = entry.CommitTxn.IsEmptyInstance ? string.Empty : entry.CommitTxn.UID,
        CommitTxnNo = entry.CommitTxn.IsEmptyInstance ? string.Empty : entry.CommitTxn.TransactionNo,
        ApprovePaymentTxnUID = entry.ApprovePaymentTxn.UID,
        ApprovePaymentTxnNo = entry.ApprovePaymentTxn.TransactionNo,
        ExerciseTxnUID = entry.ExerciseTxn.IsEmptyInstance ? string.Empty : entry.ExerciseTxn.UID,
        ExerciseTxnNo = entry.ExerciseTxn.IsEmptyInstance ? string.Empty : entry.ExerciseTxn.TransactionNo,
        ControlNo = entry.ControlNo,
        Month = EmpiriaString.MonthName(entry.Month),
        RequisitionNo = entry.RequestTxn.GetEntity().Data.BudgetableNo,
        ContractNo = entry.Contract.IsEmptyInstance ? string.Empty : entry.Contract.ContractNo,
        OrderNo = entry.PayableOrder.OrderNo,
        OrderType = !entry.Contract.IsEmptyInstance ? entry.Contract.Category.Name : entry.PayableOrder.Category.Name,
        PaymentOrderNo = entry.PaymentOrder.PaymentOrderNo,
        PayTo = entry.PaymentOrder.PayTo.Name,
        PaymentMethod = entry.PaymentOrder.PaymentMethod.Name,
        PaymentAccount = entry.PaymentOrder.PaymentMethod.AccountRelated ? entry.PaymentOrder.PaymentAccount.AccountNo : "No aplica",
        PaymentInstitution = entry.PaymentOrder.PaymentMethod.AccountRelated ? entry.PaymentOrder.PaymentAccount.Institution.Name : "No aplica",
        PaymentDate = entry.PaymentOrder.Payed ? entry.PaymentOrder.LastPaymentInstruction.LastUpdateTime : DateTime.MinValue,
        Debtor = entry.Debtor.IsEmptyInstance ? string.Empty : entry.Debtor.Name,
        AccountingVoucher = entry.AccountingVoucher,
        Description = entry.Description,
        Budget = entry.Budget.Name,
        Amount = entry.Amount,
        ToPay = entry.ToPay,
        Exercised = entry.Exercised
      };
    }

    private BudgetRequestsAnalyticsEntryDto MapBuildRequestEntry(BudgetRequestsAnalyticsEntry entry) {
      return new BudgetRequestsAnalyticsEntryDto {
        OrgUnit = entry.BudgetAccount.OrganizationalUnit.FullName,
        BudgetAccount = ((INamedEntity) entry.BudgetAccount).Name,
        BudgetBaseAccount = entry.BudgetAccount.StandardAccount.IsBaseAccount ? string.Empty :
                                                entry.BudgetAccount.StandardAccount.BaseAccount.StdAcctNo,
        BudgetProgram = entry.BudgetProgram.Code,
        ControlNo = entry.ControlNo,
        RequestTxnUID = entry.RequestTxn.UID,
        RequestTxnNo = entry.RequestTxn.TransactionNo,
        CommitTxnUID = entry.CommitTxn.IsEmptyInstance ? string.Empty : entry.CommitTxn.UID,
        CommitTxnNo = entry.CommitTxn.IsEmptyInstance ? string.Empty : entry.CommitTxn.TransactionNo,
        ApprovePaymentTxnUID = string.Empty,
        ApprovePaymentTxnNo = string.Empty,
        ExerciseTxnUID = string.Empty,
        ExerciseTxnNo = string.Empty,
        Month = EmpiriaString.MonthName(entry.Month),
        RequisitionNo = entry.Requisition.OrderNo,
        ContractNo = entry.Contract.IsEmptyInstance ? string.Empty : entry.Contract.ContractNo,
        OrderNo = string.Empty,
        OrderType = entry.Requisition.Category.Name,
        PaymentOrderNo = string.Empty,
        PayTo = string.Empty,
        PaymentMethod = string.Empty,
        PaymentAccount = string.Empty,
        PaymentInstitution = string.Empty,
        Debtor = string.Empty,
        AccountingVoucher = string.Empty,
        Description = entry.Description,
        Budget = entry.Budget.Name,
        Amount = entry.Amount,
        Requested = entry.Requested,
        Committed = entry.Committed,
        ToPay = entry.ToPay,
        Exercised = entry.Exercised
      };
    }

  }  // class BudgetRequestsAnalyticsBuilder

}  // namespace Empiria.Budgeting.Reporting
