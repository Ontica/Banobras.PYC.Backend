/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Data Layer                              *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Data Services                           *
*  Type     : SialDataService                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access for SIAL payroll data.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;
using Empiria.StateEnums;

namespace Empiria.BanobrasIntegration.Sial.Data {

  /// <summary>Provides data access for SIAL payroll data.</summary>
  static internal class SialDataService {

    static internal FixedList<NominaDetalleEntry> GetDetalle(DateTime payrollDate) {
      var sql = "SELECT * " +
                "FROM NOMINA_DETALLE " +
                "WHERE " +
                $"BGM_FECHA_VOL = {DataCommonMethods.FormatSqlDbDate(payrollDate)}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<NominaDetalleEntry>(op);
    }


    static internal NominaEncabezado GetPayroll(int payrollUID) {

      var sql = "SELECT * FROM NOMINA_ENCABEZADOS " +
                $"WHERE BGME_NUM_VOL = {payrollUID}";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<NominaEncabezado>(op);
    }


    static internal FixedList<NominaEncabezado> SearchPayrolls(EntityStatus status,
                                                               DateTime fromDate, DateTime toDate) {

      var sql = "SELECT * FROM NOMINA_ENCABEZADOS " +
          $"WHERE {DataCommonMethods.FormatSqlDbDate(fromDate)} <= BGME_FECHA_VOL AND " +
          $"BGME_FECHA_VOL < {DataCommonMethods.FormatSqlDbDate(toDate.AddDays(1))} AND " +
          $"BGME_STATUS = '{(char) status}' " +
          $"ORDER BY BGME_FECHA_VOL, BGME_NUM_VOL";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<NominaEncabezado>(op);
    }


    static internal void UpdatePayrollStatus(int payrollUID,
                                             EntityStatus status) {

      var sql = "UPDATE NOMINA_ENCABEZADOS " +
                $"SET BGME_STATUS = '{(char) status}' " +
                $"WHERE BGME_NUM_VOL = {payrollUID}";

      var op = DataOperation.Parse(sql);

      DataWriter.Execute(op);
    }

  }  // class SialDataService

}  // namespace Empiria.BanobrasIntegration.Sial.Data
