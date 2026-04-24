/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : PYC Reporting services                        Component : Adapters Layer                       *
*  Assembly : Banobras.PYC.ReportingServices.dll            Pattern   : Input fields                         *
*  Type     : PYCReportFields                               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input fields to build PYC reports.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Budgeting;

namespace Empiria.Banobras.Reporting {

  /// <summary>Input fields to build PYC reports.</summary>
  public class PYCReportFields {

    public string BudgetTypeUID {
      get; set;
    } = string.Empty;


    public DateTime FromDate {
      get; set;
    } = DateTime.Today;


    public DateTime ToDate {
      get; set;
    } = DateTime.Today;


    internal BudgetType BudgetType {
      get {
        return BudgetType.Parse(this.BudgetTypeUID);
      }
    }

  }  // class PYCReportFields

}  // namespace Empiria.Banobras.Reporting
