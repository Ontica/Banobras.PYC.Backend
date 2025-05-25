/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC Core                          Component : Common Types                            *
*  Assembly : Banobras.PYC.Core.dll                      Pattern   : Application data preloader              *
*  Type     : Preloader                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Application data preloader for Banobras' PYC System.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Threading.Tasks;

using Empiria.Contacts;
using Empiria.Parties;

using Empiria.Budgeting;
using Empiria.Financial;
using Empiria.Financial.Projects;

namespace Empiria.Banobras.PYC {

  /// <summary>Application data preloader for Banobras' PYC System.</summary>
  static public class Preloader {

    static private bool _alreadyExecuted = false;

    static public void Preload() {
      if (_alreadyExecuted) {
        return;
      }

      var task = new Task(() => {
        DoPreload();
      });

      task.Start();
    }


    static private void DoPreload() {

      _alreadyExecuted = true;

      try {
        EmpiriaLog.Info($"Banobras PYC application preloading starts at {DateTime.Now}.");

        _ = BaseObject.GetFullList<Contact>();
        _ = BaseObject.GetFullList<Party>();
        _ = BaseObject.GetFullList<CommonStorage>();
        _ = BaseObject.GetFullList<StandardAccount>();
        _ = BaseObject.GetFullList<FinancialProject>();
        _ = BaseObject.GetFullList<FinancialAccount>();
        _ = BaseObject.GetFullList<BudgetAccountSegment>();
        _ = BaseObject.GetFullList<BudgetAccount>();

        EmpiriaLog.Info($"Banobras PYC application preloading ends at {DateTime.Now}.");
      } catch {
        //  no-op
      }
    }

  }  // class Preloader

} // namespace Empiria.Banobras.PYC
