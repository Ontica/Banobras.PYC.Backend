/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : DepartamentoDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to get concepts list from system Id                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;


namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {
    
  public class ResultadoTransaccionDto {

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


  } // class TransaccionDto


  public class TransaccionFields {

    public Header Header {
      get; set;
    }

    public Payload Payload {
      get; set;
    }

   

  } // class TransaccionFields


  public class Header {

    public string IdSistemaExterno {
      get; set;
    }

    public int IdUsuario {
      get; set;
    }

    public int IdDepartamento {
      get; set;
    }

    public int IdConcepto {
      get; set;
    }

    public string ClaveCliente {
      get; set;
    }

    public string Cuenta {
      get; set;
    }

    public DateTime FechaOperacion {
      get; set;
    }

    public DateTime FechaValor {
      get; set;
    }

    public decimal Monto {
      get; set;
    }

    public string Referencia {
      get; set;
    }

    public string ConceptoPago {
      get; set;
    }

    public string Origen {
      get; set;
    } = "O";

    public string Firma {
      get; set;
    }

    public string SerieFirma {
      get; set;
    }

  } // class Header


  public class Payload {

    public string InstitucionBen {
      get; set;
    }

    public string ClaveRastreo {
      get; set;
    }

    public string NomBen {
      get; set;
    }

    public string RfcBen {
      get; set;
    }

    public int TipoCtaBen {
      get; set;
    }

    public string CtaBen {
      get; set;
    }

    public decimal Iva {
      get; set;
    }
      
  } // class Payload

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
