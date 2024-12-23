/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : IkosCashCancelTransactionResult            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO that holds the result of an IkosCash transaction cancelation.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


namespace Empiria.Payments.BanobrasIntegration.IkosCash {

  /// <summary>DTO that holds the result of an IkosCash transaction cancelation.</summary>
  public class IkosCashCancelTransactionResult {

    public string IdSistemaExterno {
      get; set;
    }

    public string IdSolicitud {
      get; set;
    }

    public string ErrorMesage {
      get; set;
    }

    public int Code {
      get; set;
    }

  } // class IkosCashCancelTransactionResult

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
