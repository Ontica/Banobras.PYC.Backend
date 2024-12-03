/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Payment Services                           Component : Integration Layer                       *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Data Transfer Object                    *
*  Type     : PaymentStatusDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Dto to read payment status.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  /// <summary>Dto to read payment status.</summary>
  public class PaymentStatusDto {

    public string PaymentUID {
      get; set;
    } = string.Empty;


    public string Status {
      get; set;
    } = string.Empty;



  }  // class PaymentStatusDto

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
