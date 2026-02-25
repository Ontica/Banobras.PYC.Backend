/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Mapping class                           *
*  Type     : BillMapper                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for bill.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Linq;
using Empiria.Billing;
using Empiria.Documents;
using Empiria.History;
using Empiria.Orders;
using Empiria.StateEnums;

namespace Empiria.Banobras.Billing.Adapters {

  /// <summary>Mapping methods for bill.</summary>
  static public class BillMapper {

    #region Public methods

    static internal FixedList<BillDescriptorDto> MapToBillListDto(FixedList<Bill> bills) {
      return bills.Select((x) => MapToBillDescriptorDto(x))
                  .ToFixedList();
    }

    #endregion Public methods

    #region Helpers

    static private BaseActions MapActions() {
      return new BaseActions {
        CanEditDocuments = true
      };
    }


    static private BillDescriptorDto MapToBillDescriptorDto(Bill bill) {

      var baseEntity = Order.Parse(bill.PayableEntityId);

      return new BillDescriptorDto() {
        UID = bill.UID,
        BillNo = bill.BillNo,
        BillTypeName = bill.BillType.DisplayName,
        IssuedByName = bill.IssuedBy.Name,
        IssuedToName = bill.IssuedTo.Name,
        CategoryName = bill.BillCategory.Name,
        BaseEntityNo = baseEntity.OrderNo,
        BaseEntityTypeName = baseEntity.OrderType.Name,
        BaseEntityName = baseEntity.Name,
        Total = bill.Total,
        IssueDate = bill.IssueDate,
        StatusName = bill.Status.GetName()
      };
    }

    #endregion Helpers

  } // class BillMapper

} // namespace Empiria.Billing.Adapters
