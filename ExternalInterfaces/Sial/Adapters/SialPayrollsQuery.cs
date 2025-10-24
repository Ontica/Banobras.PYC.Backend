/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces            Pattern   : Output DTO                              *
*  Type     : SialPayrollsQuery                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input query used to return SIAL payrolls.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;
using Empiria.StateEnums;

using Empiria.BanobrasIntegration.Sial.Data;

namespace Empiria.BanobrasIntegration.Sial.Adapters {

  /// <summary>Input query used to search SIAL payrolls.</summary>
  public class SialPayrollsQuery {

    public DateTime FromDate {
      get; set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToDate {
      get; set;
    } = ExecutionServer.DateMaxValue;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;

  }  // class SialPayrollsQuery



  /// <summary>Extension methods for SialPayrollsQuery.</summary>
  static internal class SialPayrollsQueryExtensions {

    static internal FixedList<NominaEncabezado> Execute(this SialPayrollsQuery query) {
      string dateRangeFilter = BuildDateRangeFilter(query.FromDate, query.ToDate);
      string statusFilter = BuildStatusFilter(query.Status);

      var filter = new Filter(dateRangeFilter);
      filter.AppendAnd(statusFilter);

      return SialDataService.SearchPayrolls(filter.ToString());
    }

    #region Helpers

    static private string BuildDateRangeFilter(DateTime fromDate, DateTime toDate) {
      return $"{DataCommonMethods.FormatSqlDbDate(fromDate)} <= BGME_FECHA_VOL AND " +
             $"BGME_FECHA_VOL < {DataCommonMethods.FormatSqlDbDate(toDate.AddDays(1))}";
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return string.Empty;
      }

      return $"BGME_STATUS = '{(char) status}'";
    }

    #endregion Helpers

  }

} // namespace Empiria.BanobrasIntegration.Sial.Adapters
