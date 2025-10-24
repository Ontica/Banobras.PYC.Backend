/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Integration Layer                       *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Service provider                        *
*  Type     : SialService                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements Banobras SIAL System services.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Services;
using Empiria.StateEnums;

using Empiria.BanobrasIntegration.Sial.Adapters;
using Empiria.BanobrasIntegration.Sial.Data;

namespace Empiria.BanobrasIntegration.Sial {

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

    public FixedList<SialDetailEntryDto> GetPayrollsDetailEntries(DateTime payrollDate) {

      Assertion.Require(payrollDate, nameof(payrollDate));

      FixedList<NominaDetalleEntry> entries = SialDataService.GetDetalle(payrollDate);

      return SialMapper.MapToPayrollDetailEntries(entries);
    }


    public FixedList<SialPayrollDto> SearchPayrolls(SialPayrollsQuery query) {
      Assertion.Require(query, nameof(query));

      FixedList<NominaEncabezado> payrolls = query.Execute();

      return SialMapper.Map(payrolls);
    }


    public SialPayrollDto UpdatePayrollStatus(int payrollUID,
                                              EntityStatus newStatus) {

      Assertion.Require(newStatus == EntityStatus.Deleted ||
                        newStatus == EntityStatus.Closed ||
                        newStatus == EntityStatus.Pending, nameof(newStatus));

      SialDataService.UpdatePayrollStatus(payrollUID, newStatus);

      NominaEncabezado payroll = SialDataService.GetPayroll(payrollUID);

      return SialMapper.Map(payroll);
    }

    #endregion Methods

  } // class SialServices

} // namespace Empiria.BanobrasIntegration.Sial
