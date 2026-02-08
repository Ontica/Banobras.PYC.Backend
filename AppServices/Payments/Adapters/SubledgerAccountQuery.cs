/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Payments Management                  Component : Adapters Layer                       *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Integration query interactor         *
*  Type     : SubledgerAccountQuery                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input query used to search suppliers subledger accounts.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Banobras.Payments.Adapters {

  /// <summary>Input query used to search suppliers subledger accounts.</summary>
  public class SubledgerAccountQuery {

    public string UID {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string TaxCode {
      get; set;
    } = string.Empty;

  }  // class SubledgerAccountQuery

}  // namespace Empiria.Banobras.Payments.Adapters
