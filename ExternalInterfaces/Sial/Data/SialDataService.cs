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

    static internal NominaEncabezado GetPayroll(int payrollUID) {

      var sql = "SELECT * FROM NOMINA_ENCABEZADOS " +
                $"WHERE BGME_NUM_VOL = {payrollUID}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<NominaEncabezado>(op);
    }


    static internal FixedList<NominaDetalle> GetPayrollEntries(int payrollUID) {
      var sql = "SELECT * " +
                "FROM NOMINA_DETALLE " +
                $"WHERE BGM_NUM_VOL = {payrollUID} " +
                "ORDER BY BGM_FOLIO_VOL";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<NominaDetalle>(op);
    }


    static internal FixedList<NominaEncabezado> SearchPayrolls(string filter) {
      Assertion.Require(filter, nameof(filter));

      var sql = "SELECT * FROM NOMINA_ENCABEZADOS " +
          $"WHERE {filter} " +
          $"ORDER BY BGME_FECHA_VOL, BGME_NUM_VOL";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<NominaEncabezado>(op);
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

  }  // class SialDataService

}  // namespace Empiria.BanobrasIntegration.Sial.Data
