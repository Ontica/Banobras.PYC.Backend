/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : IkosCashTransactionResult                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO that holds the result of a IkosCash payment result.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  /// <summary>DTO that holds the result of a IkosCash payment result.</summary>
  public class IkosCashTransactionResult {

    public string IdSistemaExterno {
      get; set;
    } = string.Empty;


    public string IdSolicitud {
      get; set;
    } = string.Empty;


    public string ErrorMesage {
      get; set;
    } = string.Empty;


    public int Code {
      get; set;
    } = -1;

  } // class IkosCashTransactionResult

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
