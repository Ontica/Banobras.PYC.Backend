/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : IkosCashInstructionResult                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO that holds an IkosCash payment instruction result.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  /// <summary>DTO that holds an IkosCash payment instruction result.</summary>
  public class IkosCashInstructionResult {

    public string IdSistemaExterno {
      get; set;
    } = string.Empty;


    public string IdSolicitud {
      get; set;
    } = string.Empty;


    public string ErrorMesage {
      get; set;
    } = string.Empty;


    public int Code {
      get; set;
    } = -1;


    public bool Successful {
      get {
        return Code == 0;
      }
    }

    public bool Failed {
      get {
        return Code != 0;
      }
    }

  } // class IkosCashInstructionResult

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
