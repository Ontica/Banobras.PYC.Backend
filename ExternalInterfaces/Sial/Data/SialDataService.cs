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

namespace Empiria.BanobrasIntegration.Sial.Data {

  /// <summary>Provides data access for SIAL payroll data.</summary>
  static internal class SialDataService {

    static internal FixedList<NominaEncabezado> GetEncabezados() {
      var sql = "SELECT * " +
                "FROM NOMINA_ENCABEZADOS";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<NominaEncabezado>(op);
    }

  }  // class SialDataService

}  // namespace Empiria.BanobrasIntegration.Sial.Data
