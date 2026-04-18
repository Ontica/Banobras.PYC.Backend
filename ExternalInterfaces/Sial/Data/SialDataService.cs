/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Data Layer                              *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Data Services                           *
*  Type     : SialDataService                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access for SIAL payroll data.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

using Empiria.StateEnums;

namespace Empiria.BanobrasIntegration.Sial.Data {

  /// <summary>Provides data access for SIAL payroll data.</summary>
  static internal class SialDataService {

    static internal AreaSIAL GetAreaSIAL(string claveArea) {

      var sql = "SELECT * FROM VW_AREAS_SIAL " +
                $"WHERE CLAVE_AREA ='{claveArea}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<AreaSIAL>(op, null);
    }


    static internal FixedList<AreaSIAL> GetAreasSIAL() {

      var sql = "SELECT * FROM VW_AREAS_SIAL " +
                $"ORDER BY CLAVE_AREA";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<AreaSIAL>(op);
    }


    static internal NominaSIAL GetPayroll(int payrollUID) {

      EnsurePayrollsStatusNotNull();

      var sql = "SELECT * FROM NOMINA_ENCABEZADOS " +
                $"WHERE BGME_NUM_VOL = {payrollUID}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<NominaSIAL>(op);
    }


    static internal FixedList<EntradaNominaSIAL> GetPayrollEntries(int payrollUID) {
      var sql = "SELECT * " +
                "FROM NOMINA_DETALLE " +
                $"WHERE BGM_NUM_VOL = {payrollUID} " +
                "ORDER BY BGM_FOLIO_VOL";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<EntradaNominaSIAL>(op);
    }


    static internal FixedList<NominaSIAL> SearchPayrolls(string filter) {
      Assertion.Require(filter, nameof(filter));

      EnsurePayrollsStatusNotNull();

      var sql = "SELECT * FROM NOMINA_ENCABEZADOS " +
          $"WHERE {filter} " +
          $"ORDER BY BGME_FECHA_VOL, BGME_NUM_VOL";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<NominaSIAL>(op);
    }


    static internal void UpdatePayrollStatus(int payrollUID,
                                             EntityStatus status) {

      Assertion.Require(status != EntityStatus.All, nameof(status));

      var sql = "UPDATE NOMINA_ENCABEZADOS " +
               $"SET BGME_STATUS = '{(char) status}' " +
               $"WHERE BGME_NUM_VOL = {payrollUID}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }


    static internal EmpleadoSIAL TryGetEmployeeNo(string employeeNo) {

      var sql = "SELECT * FROM INTRAN.INT_V_USUARIOS_PYC@INTRAN " +
                $"WHERE ID_USUARIO = {employeeNo}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<EmpleadoSIAL>(op, null);
    }

    static internal EmpleadoSIAL TryGetEmployeeRfc(string employeeRfc) {

      var sql = "SELECT * FROM INTRAN.INT_V_USUARIOS_PYC@INTRAN " +
                $"WHERE RFC = '{employeeRfc}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<EmpleadoSIAL>(op, null);
    }


    internal static FixedList<EmpleadoSIAL> GetEmployees() {

      var sql = "SELECT * FROM INTRAN.INT_V_USUARIOS_PYC@INTRAN";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<EmpleadoSIAL>(op);
    }


    internal static EmpleadoSIAL GetResponsableArea(string areaSIAL) {
      const string inicialesResponsable = "00";

      var sql = "SELECT * FROM " +
                "(SELECT * FROM INTRAN.INT_V_USUARIOS_PYC@INTRAN " +
                $"WHERE CLAVE_AREA = '{areaSIAL}' " +
                $"AND SUBSTR(CLAVE_PUESTO, 1, 2) = '{inicialesResponsable}') " +
                "WHERE ROWNUM = 1 ";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<EmpleadoSIAL>(op, null);
    }



    static private void EnsurePayrollsStatusNotNull() {

      var sql = "UPDATE NOMINA_ENCABEZADOS " +
               $"SET BGME_STATUS = 'P' " +
               $"WHERE BGME_STATUS IS NULL";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }

  }  // class SialDataService

}  // namespace Empiria.BanobrasIntegration.Sial.Data
