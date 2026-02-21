/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting Management                 Component : Application services Layer           *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Application service                  *
*  Type     : BudgetStatusLogAppServices                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Application services for generate budget status logs.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Orders;
using Empiria.Orders.Contracts;

using Empiria.Payments;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

namespace Empiria.Banobras.Budgeting.AppServices {

  /// <summary>Application services for generate budget status logs.</summary>
  public class BudgetStatusAppServices : UseCase {

    #region Constructors and parsers

    protected BudgetStatusAppServices() {

    }

    static public BudgetStatusAppServices UseCaseInteractor() {
      return CreateInstance<BudgetStatusAppServices>();
    }

    #endregion Constructors and parsers

    #region Application services

    public int ExecuteLog() {
      int counter = LogBudgetCommitStatus();

      counter += LogBudgetablePaymentsStatus();

      return counter;
    }


    private int LogBudgetCommitStatus() {
      FixedList<BudgetTransaction> paymentTxns = GetPaymentBudgetTransactions();

      int counter = 0;

      foreach (var txn in paymentTxns) {

        var order = txn.GetEntity() as Order;
        var isContractOrder = order is ContractOrder;

        var commitTxns = BudgetTransaction.GetRelatedTo(txn)
                                          .FindAll(x => x.OperationType == BudgetOperationType.Commit &&
                                                        (x.InProcess || x.IsClosed) &&
                                                        ((!isContractOrder && x.GetEntity().Equals(order)) ||
                                                        (isContractOrder && x.GetEntity().Equals(order.Contract))));

        if (order.HasCrossedBeneficiaries()) {
          EmpiriaLog.Debug($"La orden {order.OrderNo} - Id = {order.Id} tiene múltiples beneficiarios. " +
                           $"Transacciones de compromiso {commitTxns.Count}. Es entregable de contrato: {isContractOrder}");
          counter++;
          continue;
        }

        string txnNo = $"{txn.TransactionNo} - Id = {txn.Id}";

        if (commitTxns.Count == 0) {
          EmpiriaLog.Debug($"Crear transacción de compromiso asociada a {txnNo}.");
          counter++;
          continue;
        }

        if (commitTxns.Count > 1) {
          EmpiriaLog.Debug($"Transacción {txnNo} tiene más de un compromiso asociado: " +
                          $"{string.Join(", ", commitTxns.Select(x => x.TransactionNo))}");
          counter++;
          continue;
        }

        if (commitTxns.Count == 1) {
          if (commitTxns[0].InProcess) {
            EmpiriaLog.Debug($"Cerrar transacción de compromiso {commitTxns[0].TransactionNo} asociada a {txnNo}.");
            counter++;
          }
          if (isContractOrder) {
            continue;
          }
          if (commitTxns[0].GetTotal() != txn.GetTotal()) {
            EmpiriaLog.Debug($"Los totales no coinciden. Compromiso {commitTxns[0].TransactionNo} asociada a {txnNo}.");
            counter++;
          }
          if (commitTxns[0].Entries.Count != txn.Entries.Count) {
            EmpiriaLog.Debug($"Las entradas no coinciden. Compromiso {commitTxns[0].TransactionNo} asociada a {txnNo}.");
            counter++;
          }
        }
      }

      return counter;
    }


    private int LogBudgetablePaymentsStatus() {
      FixedList<PaymentOrder> paymentOrders = GetPaymentOrders();

      int counter = 0;

      foreach (var po in paymentOrders) {

        Order order = po.PayableEntity as Order;

        var budgetTxns = BudgetTransaction.GetFor(order);

        string paymentOrderNo = $"{po.PaymentOrderNo} - Id = {po.Id}";
        string orderNo = $"{order.OrderNo} - Id = {order.Id}";

        if (order.GetTotal() != po.Total) {
          EmpiriaLog.Debug($"Los totales no coinciden entre la solicitud de pago {paymentOrderNo} " +
                           $"y su orden asociada {orderNo}.");
          counter++;
        }


        if (order.HasBudgetableItems && order.BudgetType.Equals(BudgetType.None)) {
          EmpiriaLog.Debug($"La orden {orderNo} tiene partidas prespuestales pero no es presupuestal.");
          counter++;
          continue;
        }

        if (order.Items.Count == 0 && budgetTxns.Count > 0) {
          EmpiriaLog.Debug($"La orden {orderNo} no tiene partidas, pero sí transacciones presupuestales.");
          counter++;
          continue;
        }

        if (order.Items.Count != 0 && budgetTxns.Count == 0) {
          EmpiriaLog.Debug($"La orden {orderNo} tiene partidas, pero no tiene transacciones presupuestales.");
          counter++;
          continue;
        }

        if ((order.HasBudgetableItems || !order.BudgetType.Equals(BudgetType.None)) && budgetTxns.Count == 0) {
          EmpiriaLog.Debug($"No hay transacciones de presupuesto asociadas a la solicitud de pago {paymentOrderNo}.");
          counter++;
          continue;
        }


        if ((!order.HasBudgetableItems || order.BudgetType.Equals(BudgetType.None)) && budgetTxns.Count != 0) {
          EmpiriaLog.Debug($"Existen transacciones de presupuesto para la " +
                           $"solicitud de pago {paymentOrderNo} que no es presupuestal.");
          counter++;
          continue;
        }

        if (!order.HasBudgetableItems && order.BudgetType.Equals(BudgetType.None) && budgetTxns.Count == 0) {
          continue;
        }

        var approvePayment = budgetTxns.FindAll(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                                (x.InProcess || x.IsClosed));

        if (approvePayment.Count == 0 && (po.Payed || po.HasActivePaymentInstruction)) {
          EmpiriaLog.Debug($"No hay transacciones de aprobación de pago asociadas a la solicitud de pago {paymentOrderNo}.");
          counter++;
          continue;
        }

        if (approvePayment.Count > 1) {
          EmpiriaLog.Debug($"Hay más de una transacción de aprobación de pago asociadas a la solicitud de pago {paymentOrderNo}.");
          counter++;
          continue;
        }
      }

      return counter;
    }

    #endregion Application services

    #region Helpers

    static private FixedList<BudgetTransaction> GetPaymentBudgetTransactions() {
      return BudgetTransaction.GetFullList<BudgetTransaction>()
                              .FindAll(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                            (x.InProcess || x.IsClosed));

    }


    static private FixedList<PaymentOrder> GetPaymentOrders() {
      return PaymentOrder.GetFullList<PaymentOrder>()
                         .FindAll(x => x.InProgress || x.Payed)
                         .ToFixedList();
    }

    #endregion Helpers

  }  // class BudgetStatusLogAppServices

}  // namespace Empiria.Banobras.Budgeting.AppServices
