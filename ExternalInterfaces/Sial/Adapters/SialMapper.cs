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
        AreaNo = payroll.Area,
        AccountNo = payroll.CuentaContable,
        TypeMovement = payroll.TipoMovimiento,
        Amount = payroll.Importe
      };
    }

    static internal FixedList<SialDetailEntryDto> MapToPayrollDetailEntries(FixedList<NominaDetalle> entries) {
      return entries.Select(x => new SialDetailEntryDto {
        PayrollNo = x.NoNomina.ToString(),
        AreaNo = x.Area,
        AccountNo = x.CuentaContable.ToString(),
        TypeMovement = x.TipoMovimiento,
        Amount = x.Importe,
      }).ToFixedList();
    }


    static internal FixedList<SialOrganizationUnitEntryDto> MapToOrganizationUnitEntries(FixedList<SialOrganizationUnitEntry> entries) {
      return entries.Select(x => new SialOrganizationUnitEntryDto {
        OrganizationUnitNo = x.NoArea,
        OrganizationUnitName = EmpiriaString.Clean(x.Descripcion),
        SupervisingUnitNo = x.NoAreaSupervision,
        SupervisingUnitName = EmpiriaString.Clean(x.AreaSupervision),
        HierarchicalLevel = x.noNivel
      }).ToFixedList();
    }


    static internal SialOrganizationUnitEmployeeEntryDto MapToOrganizationUnitEmployeeEntries(SialOrganizationUnitEmployeeEntry employee) {
      return new SialOrganizationUnitEmployeeEntryDto {
        EmployeeNo = employee.NoEmpleado.ToString(),
        FedTaxpayersReg = employee.RfcEmpleado,
        Name = EmpiriaString.Clean(employee.Nombre),
        LastName = EmpiriaString.Clean(employee.ApellidoNombre),
        OrganizationUnitNo = employee.NoArea,
        JobNo = employee.NoPuesto,
        JobTitle = EmpiriaString.Clean(employee.Puesto),
        JobCategoryNo = employee.NoTabulador,
      };
    }

    internal static FixedList<SialOrganizationUnitEmployeeEntryDto> MapToOrganizationUnitEmployeesEntries(FixedList<SialOrganizationUnitEmployeeEntry> entries) {
      return entries.Select(x => new SialOrganizationUnitEmployeeEntryDto {
        EmployeeNo = x.NoEmpleado.ToString(),
        Name = EmpiriaString.Clean(x.Nombre),
        LastName = EmpiriaString.Clean(x.ApellidoNombre),
        OrganizationUnitNo = x.NoArea,
        JobNo = x.NoPuesto,
        JobTitle = EmpiriaString.Clean(x.Puesto),
        JobCategoryNo = x.NoTabulador,
      }).ToFixedList();
    }

    #endregion Mappers

    } // class SialMapper

  }  //namespace Empiria.BanobrasIntegration.Sial.Adapters
