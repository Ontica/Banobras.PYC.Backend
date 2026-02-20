/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIGEVI Integration               Component : Adapters Layer                           *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Input Query                              *
*  Type     : SigeviAvailableBudgetQuery                License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Query used to retrieve available budget information for one or more accounts.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using Empiria.Budgeting;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.BanobrasIntegration.Sigevi.Adapters {

  public class SigeviAvailableBudgetQuery {

    public int Año {
      get; set;
    }

    public int Mes {
      get; set;
    }

    public string Area {
      get; set;
    } = string.Empty;


    public string[] Partidas {
      get; set;
    } = new string[0];


    internal void EnsureValid() {
      Assertion.Require(Año >= 2026,
        $"El año presupuestal solicitado ({Año}) no está disponible en el sistema PYC.");

      Assertion.Require(1 <= Mes && Mes <= 12, $"El valor del mes no es válido: {Mes}");

      Assertion.Require(Area, $"Se requiere proporcionar el número de área.");

      var orgUnit = Party.TryParseWithID(Area);

      Assertion.Require(orgUnit != null && orgUnit is OrganizationalUnit,
          $"El área solicitada no está registrada en el sistema PYC: '{Area}'");

      Assertion.Require(Partidas.Length > 0, $"Se requiere proporcionar al menos una partida presupuestal.");

    }


    internal Budget GetBudget() {
      var budget = Budget.GetList()
                         .Find(x => x.Year == Año && x.BudgetType.Name.Contains("GastoCorriente"));

      Assertion.Require(budget, $"No se ha registrado el presupuesto para el año {Año} del gasto corriente");

      return budget;
    }


    internal FixedList<BudgetAccount> GetBudgetAccounts() {
      var list = new List<BudgetAccount>(Partidas.Length);

      var orgUnit = (OrganizationalUnit) Party.TryParseWithID(Area);

      foreach (var partida in Partidas) {
        var account = BudgetAccount.TryParse(orgUnit, partida);

        Assertion.Require(account,
          $"La partida presupuestal '{partida}' no está registrada en el sistema PYC para el área '{Area}'.");

        Assertion.Require(account.Status == EntityStatus.Active,
          $"La partida presupuestal '{partida}' no está activa para el área '{Area}'");

        Assertion.Require(account.BudgetType.Name.Contains("GastoCorriente"),
          $"La partida presupuestal '{partida}' no pertenece al gasto corriente.");

        list.Add(account);
      }

      return list.ToFixedList();
    }

  }  // class SigeviAvailableBudgetQuery

}  // namespace Empiria.BanobrasIntegration.Sigevi.Adapters
