/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC External Interfaces           Component : Adapters Layer                          *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll        Pattern   : Data Transfer Object                    *
*  Type     : BudgetingTransactionDto                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO with budgeting transaction data.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Banobras.Budgeting.Adapters {

  /// <summary>DTO with budgeting transaction data.</summary>
  public class BudgetingTransactionDto {

    public string Description {
      get; set;
    }


    public FixedList<BudgetingEntryDto> Entries {
      get; set;
    }

  }  // class BudgetingTransactionDto



  public class BudgetingEntryDto {

    public int Year {
      get; set;
    }

    public int Month {
      get; set;
    }

    public int Day {
      get; set;
    }

    public string OperationNo {
      get; set;
    }

    public string OrgUnitCode {
      get; set;
    }

    public string BudgetAccountNo {
      get; set;
    }

    public decimal Amount {
      get; set;
    }

    public string OrgUnitName {
      get; internal set;
    }

    public string BudgetAccountName {
      get; internal set;
    }

    public string AccountingAcctNo {
      get; internal set;
    }

    public string AccountingAcctName {
      get; internal set;
    }

    public string Observations {
      get; internal set;
    }

  }  // class BudgetingEntryDto

}  // namespace Empiria.Banobras.Budgeting.Adapters
