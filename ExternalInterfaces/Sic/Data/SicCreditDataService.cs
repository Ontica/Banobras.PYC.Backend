/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Sic Connector                              Component : Data Layer                              *
*  Assembly : Sic.Connector.dll                          Pattern   : Data Service                            *
*  Type     : SicServices                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access for SIC credits.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.BanobrasIntegration.Sic.Data {

  /// <summary>Provides data access for SIC credits.</summary>
  static internal class SicCreditDataService {

    static internal FixedList<SicCreditResult> SearchAuxiliar (string auxiliar) {

      var sql = "SELECT * FROM vw_creditos_sic " +
                $"WHERE auxiliar = '{auxiliar}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<SicCreditResult>(op);
    }

  }  // class SicCreditDataService

}  // namespace Empiria.BanobrasIntegration.Sic.Data
