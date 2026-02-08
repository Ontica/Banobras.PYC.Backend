/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Procurement Services                 Component : Adapters Layer                       *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Output DTO                           *
*  Type     : BudgetValidationResultDto                     License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO that holds the result about a budget validation operation.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Banobras.Procurement.Adapters {

  /// <summary>Output DTO that holds the result about a budget validation operation.</summary>
  public class BudgetValidationResultDto {

    public string Result {
      get; internal set;
    }

  } // class BudgetValidationResultDto

}  // namespace Empiria.Banobras.Procurement.Adapters
