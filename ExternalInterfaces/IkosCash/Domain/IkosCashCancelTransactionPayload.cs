/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : IkosCashCancelTransactionPayload           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to cancel IkosCash payment instructions.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


namespace Empiria.Payments.BanobrasIntegration.IkosCash {

  /// <summary>DTO used to cancel IkosCash payment instructions.</summary>
  public class IkosCashCancelTransactionPayload {

    public string IdSolicitud {
      get; set;
    }

    public decimal Importe {
      get; set;
    }

    public string IdSistemaExterno {
      get; set;
    }

    public int Usuario {
      get; set;
    }

    public int IdMotivo {
      get; set;
    }

    public string Descripcion {
      get; set;
    }

  } // class IkosCashCancelTransactionPayload

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
