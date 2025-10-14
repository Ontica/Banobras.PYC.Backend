/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Service provider                        *
*  Type     : SialService                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements Banobras SIAL System services.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial.Adapters;

using Empiria.BanobrasIntegration.Sial.Adapters;
using Empiria.BanobrasIntegration.Sial.Data;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Implements Banobras SIAL System services.</summary>
  public class SialServices {

    #region Constructors and parsers

    public SialServices() {
      // no-op
    }

    #endregion Constructors and parsers

    #region Methods

    public FixedList<SialHeaderDto> GetPayrollsEntries(char status, DateTime fromDate, DateTime toDate) {

      Assertion.Require(status, nameof(status));
      Assertion.Require(fromDate, nameof(fromDate));
      Assertion.Require(toDate, nameof(toDate));

      FixedList<NominaEncabezado> entries = SialDataService.GetEncabezados(status, fromDate, toDate);

      return SialMapper.MapToPayrollEntries(entries)
                      .Select(x => (SialHeaderDto) x)
                      .ToFixedList();
    }

    public FixedList<SialDetailEntryDto> GetPayrollsDetailEntries(DateTime payrollDate) {

      Assertion.Require(payrollDate, nameof(payrollDate));

      FixedList<NominaDetalleEntry> entries = SialDataService.GetDetalle(payrollDate);

      return SialMapper.MapToPayrollDetailEntries(entries)
                      .Select(x => (SialDetailEntryDto) x)
                      .ToFixedList();
    }


    #endregion Methods

  } // class SialServices

} // namespace Empiria.BanobrasIntegration.Sial
