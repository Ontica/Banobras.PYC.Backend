/* Empiria Connector******************************************************************************************
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

    public FixedList<SialPayrollDto> SearchPayrolls(SialPayrollsQuery query) {
      Assertion.Require(query, nameof(query));

      FixedList<NominaEncabezado> entries = SialDataService.GetEncabezados(query.Status,
                                                                           query.FromDate, query.ToDate);

      return SialMapper.Map(entries)
                       .ToFixedList();
    }


    public FixedList<SialDetailEntryDto> GetPayrollsDetailEntries(DateTime payrollDate) {

      Assertion.Require(payrollDate, nameof(payrollDate));

      FixedList<NominaDetalleEntry> entries = SialDataService.GetDetalle(payrollDate);

      return SialMapper.MapToPayrollDetailEntries(entries)
                      .Select(x => (SialDetailEntryDto) x)
                      .ToFixedList();
    }


    public void UpdateProcessStatus(EntityStatus status, int payrollNumber) {
      Assertion.Require(status, nameof(status));
      Assertion.Require(payrollNumber, nameof(payrollNumber));

      SialDataService.UpdateProcessStatus(status, payrollNumber);

    }

    #endregion Methods

  } // class SialServices

} // namespace Empiria.BanobrasIntegration.Sial
