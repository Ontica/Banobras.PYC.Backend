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

  } // class TransaccionDto


  public class TransaccionFields {

    public Header Header {
      get; set;
    } = new Header();


    public Payload Payload {
      get; set;
    } = new Payload();


    internal void SetFirma(string firma) {
      Assertion.Require(firma, nameof(firma));

      Header.Firma = firma;
    }

  } // class TransaccionFields


  public class Header {

    public string IdSistemaExterno {
      get; internal set;
    } = string.Empty;


    public int IdUsuario {
      get; internal set;
    }

    public int IdDepartamento {
      get; internal set;
    }

    public int IdConcepto {
      get; internal set;
    }

    public string ClaveCliente {
      get; internal set;
    }

    public string Cuenta {
      get; internal set;
    }

    public DateTime FechaOperacion {
      get; internal set;
    }

    public DateTime FechaValor {
      get; internal set;
    }

    public decimal Monto {
      get; internal set;
    }

    public string Referencia {
      get; internal set;
    }

    public string ConceptoPago {
      get; internal set;
    }

    public string Origen {
      get; internal set;
    } = "O";

    public string Firma {
      get; internal set;
    }

    public string SerieFirma {
      get; internal set;
    }

  } // class Header


  public class Payload {

    public string InstitucionBen {
      get; internal set;
    }

    public string ClaveRastreo {
      get; internal set;
    }

    public string NomBen {
      get; internal set;
    }

    public string RfcBen {
      get; internal set;
    }

    public int TipoCtaBen {
      get; internal set;
    }

    public string CtaBen {
      get; internal set;
    }

    public decimal Iva {
      get; internal set;
    }

  } // class Payload

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
