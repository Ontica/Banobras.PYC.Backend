/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Fields DTO                              *
*  Type     : IkosCashTransactionPayload                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Payload structure used to request IkosCash payments through web services.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Runtime.Remoting.Messaging;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {

  /// <summary>Payload structure used to request IkosCash payments through web services.</summary>
  internal class IkosCashTransactionPayload {

    public IkosCashTransactionHeader Header {
      get; internal set;
    }


    public IkosCashTransactionInnerPayload Payload {
      get; internal set;
    }


  } // class IkosCashTransactionPayload


  internal class IkosCashTransactionHeader {

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
      get; private set;
    }

    public string SerieFirma {
      get; internal set;
    }

    internal void SetFirma(string firma) {
      Assertion.Require(firma, nameof(firma));

      Firma = firma;
    }

  } // class IkosCashTransactionHeader


  internal class IkosCashTransactionInnerPayload {

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

  } // class IkosCashTransactionInnerPayload

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash
