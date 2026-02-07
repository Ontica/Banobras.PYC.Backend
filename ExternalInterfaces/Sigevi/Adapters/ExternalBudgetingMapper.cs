/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Banobras SIGEVI Integration               Component : Adapters Layer                           *
*  Assembly : Banobras.PYC.ExternalInterfaces.dll       Pattern   : Mapper                                   *
*  Type     : ExternalBudgetingMapper                   License   : Please read LICENSE.txt file             *
*                                                                                                            *
*  Summary  : Maps budgeting structures for other Banobras' systems.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Budgeting.Explorer.Adapters;

namespace Empiria.BanobrasIntegration.Sigevi.Adapters {

  static internal class ExternalBudgetingMapper {

    static internal FixedList<AvailableBudgetDto> MapBudgetExplorerResult(BudgetExplorerResultDto result) {
      throw new NotImplementedException();
    }

    static internal BudgetExplorerQuery MapToBudgetExplorerQuery(AvailableBudgetQuery query) {
      throw new NotImplementedException();
    }

  }  // class ExternalBudgetingMapper

}  // namespace Empiria.BanobrasIntegration.Sigevi.Adapters
