/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIAL Services                     Component : Domain Layer                            *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Information Holder                      *
*  Type     : NominaDetalle                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds payroll data movements.                                                                  *          
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

using Empiria.Financial.Integration;
using Empiria.Financial.Rules;

namespace Empiria.BanobrasIntegration.Sial {

  /// <summary>Holds payroll data movements.</summary>
  public class NominaDetalle {

    static private readonly FixedList<FinancialRule> _budgetingRules =
                                              FinancialRuleCategory.ParseNamedKey("BUDGETING_ACCOUNTS")
                                                                   .GetFinancialRules();

    [DataField("BGM_NUM_VOL")]
    public int NoNomina {
      get; private set;
    }


    [DataField("BGM_FOLIO_VOL")]
    public int Folio {
      get; private set;
    }


    [DataField("BGM_AREA")]
    public string Area {
      get; private set;
    }


    [DataField("BGM_REG_CONTABLE")]
    private string _cuentaContable = string.Empty;

    public string CuentaContable {
      get {
        return IntegrationLibrary.FormatAccountNumber(_cuentaContable);
      }
    }


    [DataField("BGM_CVE_MOV", ConvertFrom = typeof(int))]
    public int TipoMovimiento {
      get; private set;
    }


    [DataField("BGM_IMPORTE", ConvertFrom = typeof(decimal))]
    public decimal Importe {
      get; private set;
    }


    public string CuentaPresupuestal {
      get {

        var rule = _budgetingRules.Find(x => x.DebitAccount == CuentaContable);

        Assertion.Require(rule,
                          $"No budgeting account rule found for payroll entry account '{CuentaContable}'.");

        return rule.CreditConcept;
      }
    }


    public string NoOperacion {
      get {
        return $"{NoNomina:D8}-{Folio:D4}";
      }
    }


    public string NombreCuentaContable {
      get {
        return "No determinado";
        // return IntegrationLibrary.GetAccountingAcctName(CuentaContable);
      }
    }


    public string NombreArea {
      get {
        Party orgUnit = OrganizationalUnit.TryParseWithID(Area);

        return orgUnit != null ? orgUnit.Name : $"El área '{Area}' no está registrada en el sistema PYC.";
      }
    }


    public string NombreCuentaPresupuestal {
      get {
        return "No determinado";
      }
    }

    internal string GetObservations() {
      return string.Empty;
    }

  } // class NominaDetalle

} // namespace Empiria.BanobrasIntegration.Sial
