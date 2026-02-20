/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Expenses Management                 Component : Adapters Layer                        *
*  Assembly : Banobras.PYC.WebApi.dll                      Pattern   : Input Fields DTO                      *
*  Type     : TravelExpensesPaymentRequestFields           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields DTO used to request payments for travel expenses.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Storage;

namespace Empiria.Banobras.Expenses.Adapters {

  /// <summary>Input fields DTO used to request payments for travel expenses.</summary>
  public class TravelExpensesPaymentRequestFields {

    public string RequisitionUID {
      get; set;
    } = string.Empty;


    public string CommissionNo {
      get; set;
    } = string.Empty;


    public string CommissionerUID {
      get; set;
    } = string.Empty;


    public string PaymentAccountUID {
      get; set;
    } = string.Empty;


    public InputFile AuthorizationPdfFile {
      get; set;
    }


    internal void EnsureValid() {
      Assertion.Require(RequisitionUID, nameof(RequisitionUID));
      Assertion.Require(CommissionNo, nameof(CommissionNo));
      Assertion.Require(CommissionerUID, nameof(CommissionerUID));
      Assertion.Require(PaymentAccountUID, nameof(PaymentAccountUID));
      Assertion.Require(AuthorizationPdfFile, nameof(AuthorizationPdfFile));
    }

  }  // class TravelExpensesPaymentRequestFields

}  // namespace Empiria.Banobras.Expenses.Adapters
