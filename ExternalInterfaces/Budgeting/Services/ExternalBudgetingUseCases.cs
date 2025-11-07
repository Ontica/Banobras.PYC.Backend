/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC External Interfaces          Component : Use cases Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Use case interactor class                *
*  Type     : ExternalBudgetingUseCases                 License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Use cases for budget integration with other Banobras' systems.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Banobras.Budgeting.Adapters;
using Empiria.Budgeting;
using Empiria.Parties;
using Empiria.Services;

namespace Empiria.Banobras.Budgeting.UseCases {

  /// <summary>Use cases for budget integration with other Banobras' systems.</summary>
  public class ExternalBudgetingUseCases : UseCase {

    #region Constructors and parsers

    protected ExternalBudgetingUseCases() {
      // no-op
    }

    static public ExternalBudgetingUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<ExternalBudgetingUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public AvailableBudgetDto AvailableBudget(AvailableBudgetQuery query) {
      Assertion.Require(query, nameof(query));
      Assertion.Require(2025 <= query.Year && query.Year <= 2026,
        $"El año presupuestal solicitado ({query.Year}) no está disponible en el sistema PYC.");
      Assertion.Require(1 <= query.Month && query.Month <= 12,
        $"El valor del mes (month) no es válido: {query.Month}");
      Assertion.Require(query.OrgUnitCode, $"Se requiere el valor de la clave del área (orgUnitCode).");

      var orgUnit = Party.TryParseWithID(query.OrgUnitCode);

      Assertion.Require(orgUnit != null && orgUnit is OrganizationalUnit,
        $"El área solicitada no está registrada en el sistema PYC: '{query.OrgUnitCode}'");

      Assertion.Require(query.BudgetAccountNo, $"Se requiere el valor de la partida presupuestal (budgetAccountNo).");

      var budgetAccount = BudgetAccount.TryParse(query.BudgetAccountNo);

      Assertion.Require(budgetAccount, $"La partida presupuestal '{query.BudgetAccountNo}' no está registrada en el sistema PYC.");

      return new AvailableBudgetDto {
        Year = query.Year,
        Month = query.Month,
        OrgUnitCode = query.OrgUnitCode,
        BudgetAccountNo = query.BudgetAccountNo,
        Available = EmpiriaMath.GetRandom(1, 30000)
      };
    }

    #endregion Use cases

  }  // class ExternalBudgetingUseCases

}  // namespace Empiria.Banobras.Budgeting.UseCases
