/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting External Interfaces       Component : Services Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Services interactor                   *
*  Type     : BudgetingServices                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides Banobras budgeting-related services.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Office;
using Empiria.Services;
using Empiria.Storage;

using Empiria.Banobras.Budgeting.Adapters;
using Empiria.Banobras.Budgeting.Exporters;

namespace Empiria.Banobras.Budgeting.Services {

  /// <summary>Provides Banobras budgeting-related services.</summary>
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

      var templateUID = $"{this.GetType().Name}.ExportBudgetingTransactionDto";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new BudgetingTransactionExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(budgetingTxn);

      return excelFile.ToFileDto();
    }

  }  // class BudgetingServices

} // namespace Empiria.Banobras.Budgeting.Services
