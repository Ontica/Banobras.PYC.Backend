/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Payment Services                           Component : Integration Layer                       *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Data Transfer Object                    *
*  Type     : PaymentOrderRequestDto                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to request of a payment order.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  /// <summary>DTO used to request of a payment.</summary>
  public class PaymentRequestDto {

    public string SystemCode {
      get; set;
    }

    public string Account {
      get; set;
    }

    public DateTime RequestedDate {
      get; set;
    }

    public DateTime DueDate {
      get; set;
    }

    public decimal Total {
      get; set;
    }

    public string Reference {
      get; set;
    }

    public string Description {
      get; set;
    }


  }  // class PaymentOrderRequestDto



}  // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
