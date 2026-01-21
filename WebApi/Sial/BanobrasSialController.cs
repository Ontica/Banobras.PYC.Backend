/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                    Component : Web Api                               *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Web Api Controller                    *
*  Type     : BanobrasSialController                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API for Sial System integration.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.StateEnums;
using Empiria.Storage;
using Empiria.WebApi;

using Empiria.Banobras.Budgeting.Adapters;
using Empiria.Banobras.Budgeting.Services;

using Empiria.BanobrasIntegration.Sial.Adapters;
using Empiria.BanobrasIntegration.Sial.Services;
using Empiria.Financial.Adapters;


namespace Empiria.PYC.WebApi.BanobrasIntegration {

  /// <summary>Web API for Sial System integration.</summary>
  public class BanobrasSialController : WebApiController {

    #region Web Apis

    [HttpPost]
    [Route("v2/pyc/integration/sial/payrolls/{payrollUID:int}/export")]
    public SingleObjectModel ExportPayrollToBudgetingInterface([FromUri] int payrollUID) {


      using (var payrollServices = SialServices.ServiceInteractor()) {
        BudgetingTransactionDto payrollTxn =
                         payrollServices.ConvertPayrollToBudgetingInterface(payrollUID);

        using (var budgetingServices = BudgetingServices.ServiceInteractor()) {
          FileDto excelFile = budgetingServices.ExportToExcel(payrollTxn);

          return new SingleObjectModel(base.Request, excelFile);
        }  // budgetingServices
      }  // payrollServices
    }


    [HttpPost]
    [Route("v2/pyc/integration/sial/payrolls")]
    public CollectionModel SearchPayrolls([FromBody] SialPayrollsQuery query) {

      using (var services = SialServices.ServiceInteractor()) {
        FixedList<SialPayrollDto> payrolls = services.SearchPayrolls(query);

        return new CollectionModel(base.Request, payrolls);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v2/pyc/integration/sial/payrolls/{payrollUID:int}/update-status/{newStatus}")]
    public SingleObjectModel UpdatePayrollStatus([FromUri] int payrollUID,
                                                 [FromUri] EntityStatus newStatus) {

      using (var services = SialServices.ServiceInteractor()) {
        SialPayrollDto payroll = services.UpdatePayrollStatus(payrollUID, newStatus);

        return new SingleObjectModel(base.Request, payroll);
      }
    }

    [HttpGet]
    [Route("v2/pyc/integration/sial/employees/{employeeNo}/export")]
    public SingleObjectModel WorkerPayrollNo([FromUri] string employeeNo) {
      var services = SialServices.ServiceInteractor();

      ISialOrganizationUnitEmployeesData payrollNo = services.TryGetEmployeeNo(employeeNo);

      return new SingleObjectModel(base.Request, payrollNo);
    }

    #endregion Web Apis

  }  // class BanobrasSialController

}  // namespace Empiria.PYC.WebApi.BanobrasIntegration
