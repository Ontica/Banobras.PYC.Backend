/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : TokenDto                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to get ikos cash transactionstatus.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  public class AuthenticateDto {

    public string Token {
      get; set;
    }

    public string ErrorMesage {
      get; set;
    }

    public int Code {
      get; set;
    }


  } // class AuthenticateDto


  public class AuthenticateFields {

    public string Clave {
      get; set;
    }

    public string Llave {
      get; set;
    }

  } // class AuthenticateFields


} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters