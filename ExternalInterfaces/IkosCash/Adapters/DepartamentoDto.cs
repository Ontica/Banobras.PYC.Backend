/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : DepartamentoDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to get concepts list from system Id                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  public class DepartamentoDto {

    public int IdDepartamento {
      get; set;
    }

    public string Clave {
      get; set;
    }

    public string Nombre {
      get; set;
    }

    public string Descripcion {
      get; set;
    }

    public List<ConceptoDto> Conceptos {
      get; set;
    }   

  } // class DepartamentoDto


  public class ConceptoDto {

    public int IdConcepto {
      get; set;
    }

    public string Clave {
      get; set;
    }

    public string Nombre {
      get; set;
    }

    public string Descripcion {
      get; set;
    }

  } // class ConceptoDto

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
