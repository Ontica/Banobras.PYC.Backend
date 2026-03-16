/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras Budgeting External Interfaces       Component : Services                              *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Excel Importer                        *
*  Type     : ExcelBudgetEntry                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds buget entry data extracted from an Excel file row accoring to Banobras layout.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.Financial;
using Empiria.Office;
using Empiria.Parties;

using Empiria.Budgeting;
using Empiria.Budgeting.Transactions;

namespace Empiria.Banobras.Budgeting {

  /// <summary>Holds buget entry data extracted from an Excel file row accoring to Banobras layout.</summary>
  internal class ExcelBudgetEntry {

    private readonly List<NamedEntity> _errors = new List<NamedEntity>();

    private ExcelBudgetEntry(string transactionName) {
      TransactionName = transactionName;
    }

    static internal ExcelBudgetEntry CreateFrom(Spreadsheet excelFile, int row, string transactionName) {
      Assertion.Require(excelFile, nameof(excelFile));
      Assertion.Require(row > 0, nameof(row));
      Assertion.Require(transactionName, nameof(transactionName));

      var entry = new ExcelBudgetEntry(transactionName);

      entry.Load(excelFile, row);

      return entry;
    }

    public string TransactionName {
      get;
    }

    public int Row {
      get; private set;
    }

    public int Año {
      get; private set;
    }

    public int Mes {
      get; private set;
    }

    public int Día {
      get; private set;
    }

    public string Area {
      get; private set;
    }

    public string Partida {
      get; private set;
    }

    public string Movimiento {
      get; private set;
    }

    public decimal Ampliaciones {
      get; private set;
    }

    public decimal Reducciones {
      get; private set;
    }

    public bool EsAjuste {
      get; private set;
    }

    public string NumVerificacion {
      get; private set;
    }

    public string Descripcion {
      get; private set;
    }

    internal FixedList<NamedEntity> Errors {
      get {
        return this._errors.ToFixedList();
      }
    }

    #region Methods

    private void Load(Spreadsheet excelFile, int row) {
      Row = row;

      Año = excelFile.ReadCellValue<int>($"A{Row}");
      Mes = excelFile.ReadCellValue<int>($"B{Row}");
      Día = excelFile.ReadCellValue($"C{Row}", 1);
      Area = excelFile.ReadCellValue($"D{Row}", string.Empty);
      Partida = excelFile.ReadCellValue($"E{Row}", string.Empty);
      Movimiento = excelFile.ReadCellValue($"F{Row}", string.Empty);
      Ampliaciones = excelFile.ReadCellValue($"G{Row}", 0m);
      Reducciones = excelFile.ReadCellValue($"H{Row}", 0m);
      EsAjuste = excelFile.ReadCellValue($"I{Row}", false);
      NumVerificacion = excelFile.ReadCellValue($"J{Row}", string.Empty);
      Descripcion = excelFile.ReadCellValue($"K{Row}", string.Empty);
    }


    internal void Validate(Budget budget, BudgetTransactionType transactionType) {
      if (Año != budget.Year) {
        AddError($"El año del movimiento ({Año}) no coincide con el " +
                 $"año del presupuesto ({budget.Name}).", "A");
      }

      if (!(1 <= Mes && Mes <= 12)) {
        AddError($"El mes del movimiento ({Mes}) no es un valor válido.", "B");
      }

      if (Día < 1 || Día > DateTime.DaysInMonth(Año, Mes)) {
        AddError($"El día del movimiento ({Día}) no es un valor válido para el mes {Mes} del año {Año}.");
      }

      if (Area.Length == 0) {
        AddError($"El área del movimiento no puede estar vacía.");
      }

      if (Partida.Length == 0) {
        AddError($"La partida del movimiento no puede estar vacía.");
      }

      if (Movimiento.Length == 0) {
        AddError($"El tipo de movimiento no puede estar vacío.");
      }

      if (Ampliaciones < 0) {
        AddError($"La columna de ampliaciones tiene un valor negativo ({Ampliaciones.ToString("C2")}).", "G");
      }

      if (Reducciones < 0) {
        AddError($"La columna de reducciones tiene un valor negativo ({Reducciones.ToString("C2")}).", "H");
      }

      if (Ampliaciones == 0 && Reducciones == 0) {
        AddError($"Tanto la columna de ampliaciones como la de reducciones tienen importes iguales a cero.");
      }

      var orgUnit = OrganizationalUnit.TryParseWithID(Area);

      if (orgUnit == null) {
        AddError($"El área del movimiento ({Area}) no está registrada en el sistema.", "D");
      }

      if (Errors.Count > 0) {
        return;
      }

      var stdAccount = StandardAccount.TryParseAccountNo(budget.BudgetType.StandardAccountType, Partida);

      if (stdAccount == null) {
        AddError($"La partida '{Partida}' no existe para el " +
                 $"tipo de presupuesto {budget.BudgetType.DisplayName}.");
        return;
      }

      var budgetAccount = BudgetAccount.TryParse(orgUnit, Partida);

      if (budgetAccount == null) {

        AddError($"El área {Area} no tiene asignada la partida presupuestal {Partida}");

      } else if (!budgetAccount.BudgetType.Equals(budget.BudgetType)) {

        AddError($"La partida presupuestal {Partida} no pertenece al " +
                 $"tipo de presupuesto {budget.BudgetType.DisplayName}.");

      }
    }

    #endregion Methods

    #region Helpers

    private void AddError(string msg, string column = "") {
      msg = $"{GetLocation(column)}: {msg}";

      var error = new NamedEntity(TransactionName, msg);

      _errors.Add(error);
    }


    private string GetLocation(string column) {
      if (column.Length != 0) {
        return $"Celda {column}{this.Row}";
      } else {
        return $"Fila {this.Row}";
      }
    }

    #endregion Helpers

  }  // class ExcelBudgetEntry

}  // namespace Empiria.Banobras.Budgeting
