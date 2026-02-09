/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Payments Management                  Component : Adapters Layer                       *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Mapper                               *
*  Type     : PayeeMapper                                   License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Maps payees to their DTOs.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;
using Empiria.StateEnums;

using Empiria.Billing;
using Empiria.Billing.Adapters;

using Empiria.Payments;
using Empiria.Payments.Adapters;

namespace Empiria.Banobras.Payments.Adapters {

  /// <summary>Maps payees to their DTOs.</summary>
  static public class PayeeMapper {

    static public FixedList<PayeeDescriptor> Map(FixedList<Payee> payees) {

      return payees.Select(x => MapToDescriptor(x))
                   .ToFixedList();
    }


    static public PayeeHolderDto Map(Payee payee) {

      var bills = Bill.GetListFor(payee);

      return new PayeeHolderDto {
        Payee = MapToDto(payee),
        PaymentAccounts = PaymentAccountDto.Map(payee.GetAccounts()),
        Bills = BillMapper.MapToBillStructure(bills),
        Documents = DocumentServices.GetAllEntityDocuments(payee),
        History = HistoryServices.GetEntityHistory(payee),
        Actions = new BaseActions() {
          CanUpdate = true,
          CanEditDocuments = true,
          CanDelete = true
        }
      };
    }

    #region Helpers

    static private PayeeDescriptor MapToDescriptor(Payee payee) {
      return new PayeeDescriptor {
        UID = payee.UID,
        Name = payee.Name,
        TypeName = payee.PayeeType.Name,
        TaxCode = payee.Code,
        EmployeeNo = payee.EmployeeNo,
        SubledgerAccount = payee.SubledgerAccount,
        StatusName = payee.Status.GetName()
      };
    }


    static private PayeeDto MapToDto(Payee payee) {
      return new PayeeDto {
        UID = payee.UID,
        Name = payee.Name,
        Type = payee.PayeeType.MapToNamedEntity(),
        TaxCode = payee.Code,
        EmployeeNo = payee.EmployeeNo,
        Tags = payee.Tags,
        SubledgerAccount = payee.SubledgerAccount,
        Status = payee.Status.MapToDto(),
      };
    }

    #endregion Helpers

  }  // class PayeeMapper

}  // namespace Empiria.Banobras.Payments.Adapters
