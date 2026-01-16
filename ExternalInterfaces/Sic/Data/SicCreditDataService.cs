/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIC Services                      Component : Data Layer                              *
*  Assembly : Sic.Connector.dll                          Pattern   : Data Service                            *
*  Type     : SicServices                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data access for SIC credits.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;

namespace Empiria.BanobrasIntegration.Sic.Data {

  /// <summary>Provides data access for SIC credits.</summary>
  static internal class SicCreditDataService {


    static internal FixedList<SicCreditEntry> GetCreditEntries(FixedList<string> creditIDs,
                                                               DateTime fromDate, DateTime toDate) {
      Assertion.Require(creditIDs, nameof(creditIDs));

      if (creditIDs.Count == 0) {
        return new FixedList<SicCreditEntry>();
      }

      var sql = "SELECT * FROM vw_aplica_pagos " +
                $"WHERE {DataCommonMethods.FormatSqlDbDate(fromDate)} <= FEC_PROCESO AND " +
                $"FEC_PROCESO < {DataCommonMethods.FormatSqlDbDate(toDate.AddDays(1))} AND " +
                $"Numero IN ({string.Join(", ", creditIDs)}) " +
                $"ORDER BY Numero, FEC_PROCESO, NUM_CONCEPTO, IMPORTE";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<SicCreditEntry>(op);
    }


    static internal FixedList<SicCredit> SearchAuxiliar(string auxiliar) {

      var sql = "SELECT * FROM vw_creditos_sic " +
                $"WHERE auxiliar = '{auxiliar}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObjectFixedList<SicCredit>(op);
    }


    static internal SicCredit TryGetCredit(string creditNo) {

      var sql = "SELECT * FROM vw_creditos_sic " +
                $"WHERE numero = '{creditNo}'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetPlainObject<SicCredit>(op, null);
    }


  }  // class SicCreditDataService

}  // namespace Empiria.BanobrasIntegration.Sic.Data
