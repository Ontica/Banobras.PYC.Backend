/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Use cases Layer                         *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Use case interactor class               *
*  Type     : BillUseCases                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage billing.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Billing;
using Empiria.Billing.UseCases;
using Empiria.Banobras.Billing.Adapters;

namespace Empiria.Banobras.Billing.AppServices {

  /// <summary>Use cases to manage billing.</summary>
  public class BillAppServices : UseCase {

    #region Constructors and parsers

    protected BillAppServices() {

    }

    static public BillAppServices UseCaseInteractor() {
      return CreateInstance<BillAppServices>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public BillHolderDto GetBill(string billUID) {
      Assertion.Require(billUID, nameof(billUID));

      Bill bill = Bill.Parse(billUID);

      return BillMapper.Map(bill);
    }


    public FixedList<BillDescriptorDto> SearchBills(BillsQuery query) {
      Assertion.Require(query, nameof(query));

      var filter = query.MapToFilterString();
      var sort = query.MapToSortString();

      using (var usecases = BillUseCases.UseCaseInteractor()) {
        
        FixedList<Bill> bills = usecases.SearchBills_(filter, sort);

        return BillMapper.MapToBillListDto(bills);
      }
    }

    #endregion Use cases

    #region Private methods

    #endregion Private methods

  } // class BillUseCases

} // namespace Empiria.Billing.UseCases
