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

    public int LogBudgetCommitStatus() {
      FixedList<BudgetTransaction> paymentTxns = GetPaymentBudgetTransactions();

      int counter = 0;

      foreach (var txn in paymentTxns) {

        var order = (Order) txn.GetEntity();
        var isContractOrder = order is ContractOrder;

        var commitTxns = BudgetTransaction.GetRelatedTo(txn)
                                          .FindAll(x => x.OperationType == BudgetOperationType.Commit &&
                                                        (x.InProcess || x.IsClosed) &&
                                                        ((!isContractOrder && x.GetEntity().Equals(order)) ||
                                                        (isContractOrder && x.GetEntity().Equals(order.Contract))));

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

    #endregion Application services

    #region Helpers

    private FixedList<BudgetTransaction> GetPaymentBudgetTransactions() {
      return BudgetTransaction.GetFullList<BudgetTransaction>()
                              .FindAll(x => x.OperationType == BudgetOperationType.ApprovePayment &&
                                            (x.InProcess || x.IsClosed));

    }

    #endregion Helpers

  }  // class BudgetStatusLogAppServices

}  // namespace Empiria.Banobras.Budgeting.AppServices
