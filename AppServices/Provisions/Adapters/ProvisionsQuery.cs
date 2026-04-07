/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Procurement Services                 Component : Adapters Layer                       *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Query DTO                            *
*  Type     : ProvisionsQuery                               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Query data transfer object used to search expenses provisions.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Provisions.Adapters {

  /// <summary>Query data transfer object used to search expenses provisions.</summary>
  public class ProvisionsQuery {

    public string OrderTypeUID {
      get; set;
    } = string.Empty;


    public string ForUseInOrderTypeUID {
      get; set;
    } = string.Empty;


    public string RequestedByUID {
      get; set;
    } = string.Empty;


    public string OrderNo {
      get; set;
    } = string.Empty;


    public string CategoryUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public string ProviderUID {
      get; set;
    } = string.Empty;


    public string BudgetTypeUID {
      get; set;
    } = string.Empty;


    public string BudgetUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public Priority Priority {
      get; set;
    } = Priority.All;


    public TransactionStatus Status {
      get; set;
    } = TransactionStatus.All;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class ProvisionsQuery

} // namespace Empiria.Provisions.Adapters
