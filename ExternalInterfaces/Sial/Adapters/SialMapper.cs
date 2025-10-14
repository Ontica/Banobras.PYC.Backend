/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Mapper class                            *
*  Type     : SicMapper                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapper service for SIAL payrolls.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.BanobrasIntegration.Sial;
using Empiria.BanobrasIntegration.Sial.Adapters;
using Empiria.BanobrasIntegration.Sic.Adapters;
using Empiria.Financial;
using Empiria.Parties;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Mapper service for SIAL payrolls.</summary>
  static internal class SialMapper {

    #region Internal Methods

    static internal FixedList<SialHeaderDto> MapToPayrollEntries(FixedList<NominaEncabezado> entries) {
      return entries.Select(x => new SialHeaderDto {
        PayrollNo = x.NoNomina,
        PayrollDate = x.Fecha,
        Description = x.Descripcion,
      }).ToFixedList();
    }

    static public FixedList<SialHeaderDto> MapToPayrolls(FixedList<NominaEncabezado> payrolls) {
      return payrolls.Select(x => MapToPayroll(x))
                      .ToFixedList();
    }

    static internal SialHeaderDto MapToPayroll(NominaEncabezado payroll) {
      return new SialHeaderDto {
        PayrollNo = payroll.NoNomina,
        PayrollDate = payroll.Fecha,
        Description = payroll.Descripcion
      };
    }

    static internal SialDetailDto MapToPayrollDetail(NominaDetalle payroll) {
      return new SialDetailDto {
        PayrollNo = payroll.NoNomina,
        PayrollDate = payroll.Fecha,
        AreaNo = payroll.Area,
        AccountNo = payroll.Cuenta,
        SectorNo = payroll.Sector,
        SubledgerAccountNo = payroll.Auxiliar,
        TypeMovement = payroll.TipoMovimiento,
        Amount = payroll.Importe
      };
    }

    static internal FixedList<SialDetailEntryDto> MapToPayrollDetailEntries(FixedList<NominaDetalleEntry> entries) {
      return entries.Select(x => new SialDetailEntryDto {
        PayrollNo = x.NoNomina.ToString(),
        PayrollDate = x.Fecha,
        AreaNo = x.Area,
        ConceptNo = x.Concepto,
        AccountNo = x.Cuenta.ToString(),
        TypeMovement = x.TipoMovimiento,
        Amount = x.Importe,
      }).ToFixedList();
    }


    #endregion Internal Methods

  } // class SialMapper

}  //namespace Empiria.BanobrasIntegration.Sial.Adapters
