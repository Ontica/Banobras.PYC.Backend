/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Payments Management                  Component : Adapters Layer                       *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Output DTO                           *
*  Type     : PayeeHolderDto, PayeeDescriptor               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO used to return payees.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Billing.Adapters;

using Empiria.Financial.Adapters;

namespace Empiria.Banobras.Payments.Adapters {

  /// <summary>Holds full information for a payee.</summary>
  public class PayeeHolderDto {

    [Newtonsoft.Json.JsonProperty(PropertyName = "Supplier")]
    public PayeeDto Payee {
      get; internal set;
    }

    public FixedList<PaymentAccountDto> PaymentAccounts {
      get; internal set;
    }

    public BillsStructureDto Bills {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public BaseActions Actions {
      get; internal set;
    }

  }  // class PayeeHolderDto



  /// <summary>Output DTO with payee data.</summary>
  public class PayeeDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Type {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string TaxCode {
      get; internal set;
    }

    public string EmployeeNo {
      get; internal set;
    }

    public string SubledgerAccount {
      get; internal set;
    }

    public FixedList<string> Tags {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class PayeeDto


  /// <summary>Output DTO used to return minimal payees data to be used in lists.</summary>
  public class PayeeDescriptor {

    public string UID {
      get; internal set;
    }

    public string TypeName {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string TaxCode {
      get; internal set;
    }

    public string EmployeeNo {
      get; internal set;
    }

    public string SubledgerAccount {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // class PayeeDescriptor

}  // namespace Empiria.Banobras.Payments.Adapters
