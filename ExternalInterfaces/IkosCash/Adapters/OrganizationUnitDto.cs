/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Mapper class                            *
*  Type     : Mapper                                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data Transfer Objects mapper.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;


namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {
  /// <summary>Data structure.</summary>
  public class OrganizationUnitDto {

    public int Id {
      get; set;
    }

    public string Code {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string Descripcion {
      get; set;
    }


    public IEnumerable<OrganizationUnitConceptDto> Concepts {
      get; set;
    }


  }  // class PaymentSystemDto


  public class OrganizationUnitConceptDto {

    public int Id {
      get; set;
    }


    public string Code {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string Descripcion {
      get; set;
    }


  }  // class ConceptDto


} // namespace IkosCashConnector.Integration.PaymentServices
