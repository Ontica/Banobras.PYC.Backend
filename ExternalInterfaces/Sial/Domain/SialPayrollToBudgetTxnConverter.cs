/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Integration                  Component : Integration Layer                       *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Service provider                        *
*  Type     : SialPayrollToBudgetTxnConverter            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implements Banobras SIAL System services.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

using Empiria.Financial;
using Empiria.Financial.Rules;

using Empiria.Budgeting;

using Empiria.Banobras.Budgeting.Adapters;

using Empiria.BanobrasIntegration.Sial.Data;

namespace Empiria.BanobrasIntegration.Sial {

  internal class SialPayrollToBudgetTxnConverter {

    static private readonly FixedList<FinancialRule> _budgetingRules =
                                          FinancialRuleCategory.ParseNamedKey("BUDGETING_ACCOUNTS")
                                                               .GetFinancialRules();

    private readonly int _payrollID;

    internal SialPayrollToBudgetTxnConverter(int payrollID) {
      _payrollID = payrollID;
    }

    internal BudgetingTransactionDto Convert() {
      NominaSIAL payroll = SialDataService.GetPayroll(_payrollID);

      return new BudgetingTransactionDto {
        Description = $"{payroll.Descripcion} ({payroll.NumeroNomina})",
        Entries = payroll.Entradas().Select(x => Convert(payroll, x)).ToFixedList()
      };
    }

    #region Helpers

    private BudgetingEntryDto Convert(NominaSIAL payroll, EntradaNominaSIAL entry) {

      var dto = new BudgetingEntryDto {
        Year = payroll.Fecha.Year,
        Month = payroll.Fecha.Month,
        Day = payroll.Fecha.Day,
        OrgUnitCode = entry.Area,
        OperationNo = entry.NoOperacion,
        Amount = entry.Importe,
        AccountingAcctNo = entry.CuentaContable,
        AccountingAcctName = string.Empty
      };

      dto.BudgetAccountNo = GetCuentaPresupuestal(entry.CuentaContable);
      dto.BudgetAccount = GetBudgetAccount(entry.Area, dto.BudgetAccountNo);
      dto.BudgetAccountName = GetNombreCuentaPresupuestal(dto.BudgetAccount, dto.BudgetAccountNo);

      var areaSial = AreaSIAL.TryGetArea(entry.Area);

      if (areaSial == null) {
        dto.OrgUnitCode = "No registrada";
        dto.OrgUnitName = $"El área {entry.Area} no está registrada en el sistema SIAL.";
        dto.Observations = "Favor de revisar el área origen";

        return dto;
      } else {
        dto.OrgUnitName = areaSial.Nombre;
      }

      return dto;
    }


    static private string GetNombreCuentaPresupuestal(BudgetAccount budgetAccount, string noCuentaPresupuestal) {
      if (!budgetAccount.IsEmptyInstance) {
        return budgetAccount.Name;
      }

      if (string.IsNullOrWhiteSpace(noCuentaPresupuestal)) {
        return "Cuenta presupuestal no proporcionada";
      }

      var budgetType = BudgetType.GetList().Find(x => x.Name.Contains("GastoCorriente"));

      if (budgetType == null) {
        return "Catálogo de cuentas prespuestales no registrado";
      }

      var stdAccount = StandardAccount.TryParseAccountNo(budgetType.StandardAccountType, noCuentaPresupuestal);

      if (stdAccount == null) {
        return $"La cuenta presupuestal '{noCuentaPresupuestal}' no está registrada en el sistema.";
      }

      return $"El área no maneja la cuenta presupuestal '{noCuentaPresupuestal}'.";
    }


    static private string GetCuentaPresupuestal(string cuentaContable) {
      var rule = _budgetingRules.Find(x => x.DebitAccount == cuentaContable);

      if (rule == null) {
        return string.Empty;
      }

      return rule.CreditConcept;
    }


    static private BudgetAccount GetBudgetAccount(string claveArea, string cuentaPresupuestal) {
      if (string.IsNullOrWhiteSpace(claveArea) || string.IsNullOrWhiteSpace(cuentaPresupuestal)) {
        return BudgetAccount.Empty;
      }

      while (true) {
        OrganizationalUnit orgUnit = GetFirstRegisteredOrgUnit(claveArea);

        var account = BudgetAccount.TryParse(orgUnit, cuentaPresupuestal);

        if (account != null) {
          return account;
        }

        AreaSIAL parent = AreaSIAL.TryGetArea(claveArea);

        if (parent == null) {
          return BudgetAccount.Empty;
        }

        claveArea = parent.ClaveAreaSuperior;
      }
    }


    static private OrganizationalUnit GetFirstRegisteredOrgUnit(string claveArea) {

      if (string.IsNullOrWhiteSpace(claveArea)) {
        return OrganizationalUnit.Empty;
      }

      OrganizationalUnit orgUnit = null;

      while (true) {

        orgUnit = OrganizationalUnit.TryParseWithID(claveArea);

        if (orgUnit != null) {
          return orgUnit;
        }

        AreaSIAL areaSIAL = AreaSIAL.TryGetArea(claveArea);

        if (areaSIAL == null) {
          return OrganizationalUnit.Empty;
        }

        areaSIAL = AreaSIAL.TryGetArea(areaSIAL.ClaveAreaSuperior);

        if (areaSIAL == null) {
          return OrganizationalUnit.Empty;
        }

        claveArea = areaSIAL.Clave;
      }
    }

    #endregion Helpers

  }  // class SialPayrollToBudgetTxnConverter

}  // namespace Empiria.BanobrasIntegration.Sial
