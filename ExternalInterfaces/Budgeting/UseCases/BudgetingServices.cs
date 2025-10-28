/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                    Component : Web Api                               *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web Api Controller                    *
*  Type     : BanobrasSialController                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API for Sial System integration.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;
using Empiria.Storage;

using Empiria.Banobras.Budgeting.Adapters;

namespace Empiria.Banobras.Budgeting.Services {

  public class BudgetingServices : Service {

    #region Constructors and parsers

    protected BudgetingServices() {
      // no-op
    }

    static public BudgetingServices ServiceInteractor() {
      return Service.CreateInstance<BudgetingServices>();
    }

    #endregion Constructors and parsers

    public FileDto ExportToExcel(BudgetingTransactionDto budgetingTxn) {
      Assertion.Require(budgetingTxn, nameof(budgetingTxn));

      throw new System.NotImplementedException();
    }

  }  // class BudgetingServices

} // namespace Empiria.Banobras.Budgeting.Services
