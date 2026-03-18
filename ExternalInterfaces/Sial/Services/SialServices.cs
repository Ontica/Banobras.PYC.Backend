/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Integration Layer                       *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Service provider                        *
*  Type     : SialService                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements Banobras SIAL System services.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Office;
using Empiria.Services;
using Empiria.StateEnums;
using Empiria.Storage;

using Empiria.Financial.Adapters;

using Empiria.Banobras.Budgeting.Adapters;

using Empiria.BanobrasIntegration.Sial.Adapters;
using Empiria.BanobrasIntegration.Sial.Data;

namespace Empiria.BanobrasIntegration.Sial.Services {

  /// <summary>Implements Banobras SIAL System services.</summary>
  public class SialServices : Service {

    #region Constructors and parsers

    protected SialServices() {
      // no-op
    }

    static public SialServices ServiceInteractor() {
      return Service.CreateInstance<SialServices>();
    }

    #endregion Constructors and parsers

    #region Methods

    public BudgetingTransactionDto ConvertPayrollToBudgetTransaction(int payrollUID) {

      var converter = new SialPayrollToBudgetTxnConverter(payrollUID);

      return converter.Convert();
    }


    public FileDto ExportPayrollToExcel(BudgetingTransactionDto transaction) {
      Assertion.Require(transaction, nameof(transaction));

      var templateUID = $"{this.GetType().Name}.ExportBudgetingTransactionDto";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new SialPayrollToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(transaction);

      return excelFile.ToFileDto();
    }


    public FixedList<SialPayrollDto> SearchPayrolls(SialPayrollsQuery query) {
      Assertion.Require(query, nameof(query));

      FixedList<NominaSIAL> payrolls = query.Execute();

      return SialMapper.Map(payrolls);
    }


    public SialPayrollDto UpdatePayrollStatus(int payrollUID,
                                              EntityStatus newStatus) {

      Assertion.Require(newStatus == EntityStatus.Deleted ||
                        newStatus == EntityStatus.Closed ||
                        newStatus == EntityStatus.Pending, nameof(newStatus));

      SialDataService.UpdatePayrollStatus(payrollUID, newStatus);

      NominaSIAL payroll = SialDataService.GetPayroll(payrollUID);

      return SialMapper.Map(payroll);
    }


    public FixedList<SialEmployeeDto> GetOrganizationUnitEmployeesEntries() {

      FixedList<EmpleadoSIAL> entries = SialDataService.GetEmployees();

      return SialMapper.MapEmployeesDto(entries)
                      .ToFixedList();
    }


    public SialEmployeeDto TryGetEmployeeNo(string employeeNo) {
      Assertion.Require(employeeNo, nameof(employeeNo));

      EmpleadoSIAL employee = SialDataService.TryGetEmployeeNo(employeeNo);

      if (employee == null) {
        return null;
      }

      return SialMapper.MapToEmployeeDto(employee);
    }


    public SialEmployeeDto TryGetEmployeeRfc(string employeeRfc) {
      Assertion.Require(employeeRfc, nameof(employeeRfc));

      EmpleadoSIAL employee = SialDataService.TryGetEmployeeRfc(employeeRfc);

      if (employee == null) {
        return null;
      }

      return SialMapper.MapToEmployeeDto(employee);
    }

    #endregion Methods

  } // class SialServices

} // namespace Empiria.BanobrasIntegration.Sial.Services
