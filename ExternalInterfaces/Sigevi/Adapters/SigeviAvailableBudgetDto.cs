/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIGEVI Integration               Component : Adapters Layer                           *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Output Data Transfer Object              *
*  Type     : SigeviAvailableBudgetDto                  License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Returns available budget information for one or more budget accounts.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Budgeting;
using Empiria.Budgeting.Explorer;

namespace Empiria.BanobrasIntegration.Sigevi.Adapters {

  /// <summary>Returns available budget information for one or more budget accounts.</summary>
  public class SigeviAvailableBudgetDto {

    internal SigeviAvailableBudgetDto(SigeviAvailableBudgetQuery query,
                                      FixedList<BudgetDataInColumns> availableBudget) {

      Año = query.Año;
      Mes = query.Mes;
      Area = query.Area;
      Disponibilidad = BuildDisponibilidad(query.GetBudgetAccounts(), availableBudget);
    }

    public int Año {
      get;
    }

    public int Mes {
      get;
    }

    public string Area {
      get;
    }

    public FixedList<SigeviAvailableBudgetEntry> Disponibilidad {
      get;
    }


    static private FixedList<SigeviAvailableBudgetEntry> BuildDisponibilidad(FixedList<BudgetAccount> budgetAccounts,
                                                                             FixedList<BudgetDataInColumns> availableBudget) {

      var list = new List<SigeviAvailableBudgetEntry>(budgetAccounts.Count);

      foreach (var budgetAccount in budgetAccounts) {

        var budgetData = availableBudget.Find(x => x.BudgetAccount.Equals(budgetAccount));

        decimal available = (budgetData != null) ? Math.Max(budgetData.Available, 0) : 0m;

        list.Add(new SigeviAvailableBudgetEntry {
          Partida = budgetAccount.AccountNo,
          Disponible = available
        });
      }

      return list.ToFixedList();
    }

  }  // class SigeviAvailableBudgetDto



  public class SigeviAvailableBudgetEntry {

    public string Partida {
      get; internal set;
    }

    public decimal Disponible {
      get; internal set;
    }

  } // class SigeviAvailableBudgetEntry

}  // namespace Empiria.BanobrasIntegration.Sigevi.Adapters
