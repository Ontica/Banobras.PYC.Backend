/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIGEVI Integration                  Component : Adapters Layer                        *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll          Pattern   : Input Fields DTO                      *
*  Type     : SigeviSolicitudDotacionFields                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields DTO used to request advance payments for travel expenses.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Storage;

namespace Empiria.BanobrasIntegration.Sigevi.Adapters {

  /// <summary>Input fields DTO used to request advance payments for travel expenses.</summary>
  public class SigeviSolicitudDotacionFields {

    public string NoComision {
      get; set;
    } = string.Empty;


    public string Descripcion {
      get; set;
    } = string.Empty;


    public string NoExpediente {
      get; set;
    } = string.Empty;


    public string NoCuentaPago {
      get; set;
    } = string.Empty;


    public SigeviPresupuesto[] Presupuesto {
      get; set;
    } = new SigeviPresupuesto[0];


    public InputFile PDfSolicitudDotacion {
      get; set;
    }


    internal void EnsureValid() {
      Assertion.Require(NoComision, nameof(NoComision));
      Assertion.Require(NoExpediente, nameof(NoExpediente));
      Assertion.Require(NoCuentaPago, nameof(NoCuentaPago));
      Assertion.Require(Presupuesto, nameof(Presupuesto));
      Assertion.Require(Presupuesto.Length > 0, nameof(Presupuesto));
      Assertion.Require(PDfSolicitudDotacion, nameof(PDfSolicitudDotacion));
    }

  }  // class SigeviSolicitudDotacionFields

}  // namespace Empiria.BanobrasIntegration.Sigevi.Adapters
