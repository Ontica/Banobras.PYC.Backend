/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Payment Services                           Component : Integration Layer                       *
*  Assembly : Empiria.Financial.Integration.dll               Pattern   : Data Transfer Object                    *
*  Type     : PaymentOrderDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data structure with payment order data returned by an payment service provider.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  /// <summary>Data structure with payment order data returned by an payment service provider.</summary>
  public class PaymentDto {

    public string SystemCode {
      get; set;
    } = string.Empty;

    public string PaymentUID {
      get; set;
    } = string.Empty;

    public string ErrorMesage {
      get; set;
    } = string.Empty;

    public int ErrorCode {
      get; set;
    }


  }  // class PaymentDto



  /// <summary>DTO used to get IKOS payment instruction status.</summary>
  public class IkosStatusRequest {

    public string IdSolicitud {
      get; set;
    }

  }  // class IkosStatusRequest

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
