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

    static internal FixedList<SialPayrollDto> Map(FixedList<NominaSIAL> payrolls) {
      return payrolls.Select(x => Map(x))
                     .ToFixedList();
    }


    static internal SialPayrollDto Map(NominaSIAL payroll) {
      return new SialPayrollDto {
        UID = payroll.UID,
        PayrollNo = payroll.NumeroNomina.ToString(),
        PayrollDate = payroll.Fecha,
        Description = payroll.Descripcion,
        StatusName = payroll.Status.GetName()
      };
    }


    static internal SialDetailDto MapToPayrollDetail(EntradaNominaSIAL payroll) {
      return new SialDetailDto {
        PayrollNo = payroll.NoNomina,
        AreaNo = payroll.Area,
        AccountNo = payroll.CuentaContable,
        TypeMovement = payroll.TipoMovimiento,
        Amount = payroll.Importe
      };
    }

    static internal FixedList<SialDetailEntryDto> MapToPayrollDetailEntries(FixedList<EntradaNominaSIAL> entries) {
      return entries.Select(x => new SialDetailEntryDto {
        PayrollNo = x.NoNomina.ToString(),
        AreaNo = x.Area,
        AccountNo = x.CuentaContable.ToString(),
        TypeMovement = x.TipoMovimiento,
        Amount = x.Importe,
      }).ToFixedList();
    }


    static internal SialEmployeeDto MapToEmployeeDto(EmpleadoSIAL empleado) {
      return new SialEmployeeDto {
        Code = empleado.NoEmpleado.ToString(),
        RFC = empleado.RfcEmpleado,
        Name = EmpiriaString.Clean(empleado.Nombre),
        OrganizationUnitCode = empleado.NoArea,
        OrganizationUnit = empleado.Area,
        PositionCode = empleado.NoPuesto,
        Position = EmpiriaString.Clean(empleado.Puesto),
        Category = empleado.NoTabulador,
        CpnCode = empleado.IdCpn,
        Cpn = empleado.Cpn,
        SubLeadgerAccount = empleado.Auxilar
      };
    }


    internal static FixedList<SialEmployeeDto> MapEmployeesDto(FixedList<EmpleadoSIAL> entries) {
      return entries.Select(x => MapToEmployeeDto(x)).ToFixedList();
    }

    #endregion Mappers

  } // class SialMapper

}  //namespace Empiria.BanobrasIntegration.Sial.Adapters
