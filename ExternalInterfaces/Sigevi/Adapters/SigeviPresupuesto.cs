/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIGEVI Integration                  Component : Adapters Layer                        *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll          Pattern   : Input Fields DTO                      *
*  Type     : SigeviPresupuesto                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields DTO used to request payments for travel expenses.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.BanobrasIntegration.Sigevi.Adapters {

  public class SigeviPresupuesto {

    public int Año {
      get; set;
    }

    public int Mes {
      get; set;
    }

    public string Area {
      get; set;
    } = string.Empty;


    public SigeviBudgetEntry[] Partidas {
      get; set;
    } = new SigeviBudgetEntry[0];

  }  // class SigeviPresupuesto



  public class SigeviBudgetEntry {

    public string Partida {
      get; set;
    } = string.Empty;


    public string Descripcion {
      get; set;
    } = string.Empty;


    public decimal Importe {
      get; set;
    }

  } // class SigeviBudgetEntry

}  // namespace Empiria.BanobrasIntegration.Sigevi.Adapters
