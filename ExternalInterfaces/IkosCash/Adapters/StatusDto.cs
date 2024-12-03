/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : DepartamentoDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to get ikos cash transactionstatus.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  public class IkosStatusDto {

    public string IdSolicitud {
      get; set;
    }

    public string Status {
      get; set;
    }

   
  } // class DepartamentoDto


} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters