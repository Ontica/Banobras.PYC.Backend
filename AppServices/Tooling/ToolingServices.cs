/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Explorer                          Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.Explorer.dll              Pattern   : Service provider                        *
*  Type     : CashFlowSearchServices                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services used to retrieve cash flow related data and entities.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using Empiria.BanobrasIntegration.Sic;
using Empiria.DynamicData;
using Empiria.Financial.Adapters;
using Empiria.Services;

namespace Empiria.Banobras.Tooling.AppServices {

  /// <summary>Provides services used to retrieve cash flow related data and entities.</summary>
  public class ToolingServices : Service {

    #region Constructors and parsers

    protected ToolingServices() {
      // no-op
    }

    static public ToolingServices ServiceInteractor() {
      return CreateInstance<ToolingServices>();
    }

    #endregion Constructors and parsers

    #region Services

    public async Task<DynamicDto<ICreditEntryData>> SearchExternalCreditEntries(RecordsSearchQuery query) {
      Assertion.Require(query, nameof(query));

      var sicServices = new SicServices();

      FixedList<ICreditEntryData> entries = await sicServices.GetCreditsEntries(query.Keywords.ToFixedList(),
                                                                     query.FromDate,
                                                                      query.ToDate);
      var columns = new DataTableColumn[] {
        new DataTableColumn("accountNo", "No crédito", "text"),
        new DataTableColumn("accountName", "Acreditado", "text"),
        new DataTableColumn("subledgerAccountNo", "Auxiliar", "text"),
        new DataTableColumn("applicationDate", "Fecha", "date"),
        new DataTableColumn("operationName", "Concepto", "text"),
        new DataTableColumn("amount", "Importe", "decimal"),
      }.ToFixedList();

      return new DynamicDto<ICreditEntryData>(query, columns, entries);
    }

    #endregion Services

  }  // class ToolingServices

}  // namespace Empiria.CashFlow.Explorer.UseCases
