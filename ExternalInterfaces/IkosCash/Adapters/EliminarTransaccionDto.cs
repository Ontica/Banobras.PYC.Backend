/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : DeleteTransactionDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to delete ikos cash tranaction.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  public class EliminarTransaccionDto {

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

  } // class EliminarTransaccionDto


  public class EliminarTransaccionFields {

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

  } // class EliminarTransaccionDto

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
