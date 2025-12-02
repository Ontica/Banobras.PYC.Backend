/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC External Interfaces          Component : Use cases Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Use case interactor class                *
*  Type     : ExternalBudgetingUseCases                 License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Use cases for budget integration with other Banobras' systems.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;
using Empiria.Banobras.Budgeting.Adapters;
using Empiria.Budgeting;
using Empiria.Json;
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


    public FixedList<OrgUnitDto> GetBudgetingOrganizationalUnits() {
      var orgUnits = BaseObject.GetFullList<BudgetAccount>()
                               .SelectDistinct(x => x.OrganizationalUnit)
                               .OrderBy(x => x.Code);

      return orgUnits.Select(x => ToOrgUnitDto(x))
                     .ToFixedList();
    }


    public ExternalBudgetRequestDto RequestBudget(ExternalBudgetRequestFields fields) {
      Assertion.Require(fields, nameof(fields));

      var orgUnit = Party.TryParseWithID(fields.OrgUnitCode);

      Assertion.Require(fields.OrgUnitCode, $"Se requiere el valor de la clave del área (orgUnitCode).");

      Assertion.Require(orgUnit != null && orgUnit is OrganizationalUnit,
        $"El área solicitada no está registrada en el sistema PYC: '{fields.OrgUnitCode}'");

      Assertion.Require(2025 <= fields.Year && fields.Year <= 2026,
        $"El año presupuestal solicitado ({fields.Year}) no está disponible en el sistema PYC.");

      Assertion.Require(!ExecutionServer.IsMinOrMaxDate(fields.StartDate),
        $"Se requiere proporcionar la fecha de inicio para la solicitud.");

      Assertion.Require(!ExecutionServer.IsMinOrMaxDate(fields.EndDate),
        $"Se requiere proporcionar la fecha de fin para la solicitud.");

      Assertion.Require(fields.StartDate <= fields.EndDate,
        $"La fecha de inicio de la solicitud no puede ser mayor que la fecha de su finalización.");

      Assertion.Require(fields.RequisitionNo,
        $"Se requiere proporcionar el número de requisición para la solicitud.");

      Assertion.Require(fields.Description,
        $"Se requiere proporcionar la descripción de la solicitud.");

      Assertion.Require(fields.Justification,
        $"Se requiere proporcionar la justificación de la solicitud.");

      Assertion.Require(fields.ExternalControlID,
        $"Se requiere proporcionar el identificador externo único para la solicitud.");

      Assertion.Require(fields.BudgetEntries.Length > 0,
        $"Se requiere al menos una entrada para la solicitud presupuestaria.");


      for (int i = 0; i < fields.BudgetEntries.Count(); i++) {
        var entry = fields.BudgetEntries[i];

        Assertion.Require(entry, $"La entrada de presupuesto en la posición {i} tiene un valor nulo.");

        Assertion.Require(1 <= entry.Month && entry.Month <= 12,
          $"El valor del mes (month) en la entrada de presupuesto en la posición {i} no es válido: {entry.Month}");

        Assertion.Require(entry.BudgetAccountNo,
          $"Se requiere el número de cuenta presupuestal (budgetAccountNo) en la entrada de presupuesto en la posición {i}.");

        Assertion.Require(entry.Amount > 0,
          $"Se requiere un monto (amount) mayor que cero en la entrada de presupuesto en la posición {i}.");

        Assertion.Require(entry.ExternalEntryControlID,
          $"Se requiere el identificador externo único de la partida que se está solicitando en la entrada de presupuesto en la posición {i}.");

        var budgetAccount = BudgetAccount.TryParse(entry.BudgetAccountNo);

        Assertion.Require(budgetAccount, $"La partida presupuestal '{entry.BudgetAccountNo}' en la posición {i} no está registrada en el sistema PYC.");
      }

      int randomValue = EmpiriaMath.GetRandom(1, 12);

      Assertion.Ensure(randomValue != 3,
          "Error interno [DE PRUEBA] al generar la solicitud presupuestaria. Por favor intente de nuevo.");

      Assertion.Require(randomValue != 7,
          $"No hay suficiente presupuesto disponible para la partida " +
          $"{fields.BudgetEntries[0].BudgetAccountNo} en el mes {fields.BudgetEntries[0].Month}.");

      return new ExternalBudgetRequestDto {
        TransactionNo = $"SUF-GC-{fields.Year}-{EmpiriaMath.GetRandom(1, 20000).ToString("00000")}",
        RequisitionNo = fields.RequisitionNo,
        OrgUnitCode = fields.OrgUnitCode,
        Year = fields.Year,
        StartDate = fields.StartDate,
        EndDate = fields.EndDate,
        Description = fields.Description,
        Justification = fields.Justification,
        AuthorizationDate = DateTime.Now,
        AuthorizedBy = "Sistema PYC",
        ExternalControlID = fields.ExternalControlID,
        Attributes = JsonObject.Parse(fields.Attributes).ToObject(),
        BudgetEntries = fields.BudgetEntries.Select(entry => new ExternalBudgetRequestEntryDto {
          BudgetAccountNo = entry.BudgetAccountNo,
          Month = entry.Month,
          Amount = entry.Amount,
          ControlID = $"{fields.Year.ToString().Substring(2, 2)}-{EmpiriaMath.GetRandom(1, 20000).ToString("00000")}",
          ExternalControlEntryID = entry.ExternalEntryControlID,
          Description = entry.Description,
          Justification = entry.Justification
        }).ToFixedList()
      };
    }

    #endregion Use cases

    #region Helpers

    private OrgUnitDto ToOrgUnitDto(OrganizationalUnit x) {
      return new OrgUnitDto {
        Code = x.Code,
        Name = x.Name
      };
    }

    #endregion Helpers

  }  // class ExternalBudgetingUseCases

}  // namespace Empiria.Banobras.Budgeting.UseCases
