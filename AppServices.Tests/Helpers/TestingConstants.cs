/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Application Services                    Component : Tests cases                       *
*  Assembly : Banobras.PYC.AppServices.Tests.dll               Pattern   : Testing constants                 *
*  Type     : TestingConstants                                 License   : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : Constants used for aplication services tests.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.IO;

using Empiria.Orders;
using Empiria.Payments;
using Empiria.Storage;

namespace Empiria.Tests.Banobras {

  /// <summary>Constants used for aplication services tests.</summary>
  internal class TestingConstants {

    static internal readonly Requisition TRAVEL_REQUISITION =
                  Requisition.Parse(ConfigurationData.GetString("TESTS_REQUISITION_UID"));

    static internal readonly Payee TRAVEL_COMMISSIONER =
                  Payee.Parse(ConfigurationData.GetString("TESTS_PAYEE_UID"));

    static internal readonly InputFile TRAVEL_AUTHORIZATION_PDF_FILE =
                  new InputFile(new FileInfo(ConfigurationData.GetString("TESTS_PDF_FILE_PATH")));

  }  // class TestingConstants

}  // namespace Empiria.Tests.Banobras
