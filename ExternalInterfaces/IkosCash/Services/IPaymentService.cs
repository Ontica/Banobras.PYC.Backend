﻿/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Payment Services                           Component : Integration Layer                       *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Service integration interface           *
*  Type     : IPaymentService                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Defines the services that must be implemented by a payment service provider.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using Empiria.Payments.BanobrasIntegration.IkosCash.Adapters;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {

    /// <summary>Defines the services that must be implemented by a payment service provider.</summary>
    public interface IPaymentService {

    Task<List<PaymentDto>> AddTransactions(List<PaymentRequestDto> paymentRequests);

    Task<List<PaymentStatusDto>> GetPaymentsStatus(List<MinimalPaymentDto> payments);

    Task<List<OrganizationUnitDto>> GetOrganizationUnitConcepts(int idSistema);

  }  // interface IPaymentService

}  // namespace Empiria.Payments.BanobrasIntegration.IkosCash