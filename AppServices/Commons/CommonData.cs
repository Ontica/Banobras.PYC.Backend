/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras PYC Application Services          Component : Common Types                            *
*  Assembly : Banobras.PYC.AppServices.dll               Pattern   : Static constants and variables          *
*  Type     : CommonData                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds constants and variables common to Banobras PYC application services.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Banobras {

  /// <summary>Holds constants and variables common to Banobras PYC application services.</summary>
  static internal class CommonData {

    static public readonly int BATCH_SIZE = 250;

    static public readonly OrganizationalUnit GERENCIA_DE_CONTROL_PRESUPUESTAL =
                                                          OrganizationalUnit.Parse(102);

    static public readonly OrganizationalUnit GERENCIA_DE_PAGOS =
                                                          OrganizationalUnit.Parse(145);

    static public readonly OperationSource SISTEMA_DE_ADQUISICIONES =
                                OperationSource.ParseNamedKey("SISTEMA_DE_ADQUISICIONES");

    static public readonly OperationSource SISTEMA_DE_PAGOS =
                                OperationSource.ParseNamedKey("SISTEMA_DE_PAGOS");

    static public readonly OperationSource SISTEMA_DE_CONTROL_PRESUPUESTAL =
                                OperationSource.ParseNamedKey("SISTEMA_DE_CONTROL_PRESUPUESTAL");

  }  // class CommonData

}  // namespace Empiria.Banobras
