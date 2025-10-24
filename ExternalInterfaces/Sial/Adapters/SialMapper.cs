/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Mapper class                            *
*  Type     : SialMapper                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping services for SIAL payrolls.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Mapping services for SIAL payrolls.</summary>
  static internal class SialMapper {

    #region Mappers

    static internal FixedList<SialPayrollDto> Map(FixedList<NominaEncabezado> payrolls) {
      return payrolls.Select(x => Map(x))
                     .ToFixedList();
    }


    static internal SialPayrollDto Map(NominaEncabezado payroll) {
      return new SialPayrollDto {
        UID = payroll.UID,
        PayrollNo = payroll.NoNomina.ToString(),
        PayrollDate = payroll.Fecha,
        Description = payroll.Descripcion,
        StatusName = payroll.Status.GetName()
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

    #endregion Mappers

  } // class SialMapper

}  //namespace Empiria.BanobrasIntegration.Sial.Adapters
