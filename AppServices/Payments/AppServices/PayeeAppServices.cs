/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Payments Management                  Component : Application services Layer           *
*  Assembly : Banobras.PYC.AppServices.dll                  Pattern   : Application service                  *
*  Type     : PayeeAppServices                              License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Application services for payees management. A payee is someone who receives payments.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.Services;

using Empiria.Payments;
using Empiria.Payments.Adapters;
using Empiria.Payments.UseCases;

using Empiria.Banobras.Payments.Adapters;

namespace Empiria.Banobras.Payments.AppServices {

  /// <summary>Application services for payees management. A payee is someone who receives payments.</summary>
  public class PayeeAppServices : UseCase {

    #region Constructors and parsers

    protected PayeeAppServices() {

    }

    static public PayeeAppServices UseCaseInteractor() {
      return CreateInstance<PayeeAppServices>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<PayeeHolderDto> AddPayee(PayeeFieldsExtended fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      SubledgerAccountQuery query = BuildSubledgerAccountQuery(fields);

      await MatchPayeeSubledgerAccount(query);

      FixedList<Payee> sameTaxCode = GetPayeesByTaxCode(fields.TaxCode);

      if (sameTaxCode.Exists(x => x.SubledgerAccount == fields.SubledgerAccount)) {
        Assertion.RequireFail("Ya existe otro beneficiario con el mismo RFC y auxiliar contable.");
      }

      using (var usecase = PayeesUseCases.UseCaseInteractor()) {

        Payee payee = usecase.AddPayee(fields);

        return PayeeMapper.Map(payee);
      }
    }


    public PayeeHolderDto GetPayee(string payeeUID) {
      Assertion.Require(payeeUID, nameof(payeeUID));

      var payee = Payee.Parse(payeeUID);

      return PayeeMapper.Map(payee);
    }


    public FixedList<NamedEntityDto> GetPayeeTypes() {
      return PayeeType.GetPayeeTypes()
                      .MapToNamedEntityList(false);
    }


    public PayeeHolderDto RemovePayee(string payeeUID) {
      Assertion.Require(payeeUID, nameof(payeeUID));

      using (var usecase = PayeesUseCases.UseCaseInteractor()) {

        Payee payee = usecase.DeletePayee(payeeUID);

        return PayeeMapper.Map(payee);
      }
    }


    public FixedList<PayeeDescriptor> SearchPayees(PayeesQuery query) {
      Assertion.Require(query, nameof(query));

      using (var usecase = PayeesUseCases.UseCaseInteractor()) {

        FixedList<Payee> payees = usecase.SearchPayees(query);

        return PayeeMapper.Map(payees);
      }
    }


    public async Task<PayeeHolderDto> UpdatePayee(PayeeFieldsExtended fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      SubledgerAccountQuery query = BuildSubledgerAccountQuery(fields);

      await MatchPayeeSubledgerAccount(query);

      var payee = Payee.Parse(fields.UID);

      FixedList<Payee> sameTaxCode = GetPayeesByTaxCode(fields.TaxCode);

      if (sameTaxCode.Exists(x => x.UID != payee.UID &&
                                  x.SubledgerAccount == fields.SubledgerAccount)) {
        Assertion.RequireFail("Ya existe otro beneficiario con el mismo RFC y auxiliar contable.");
      }

      using (var usecase = PayeesUseCases.UseCaseInteractor()) {

        payee = usecase.UpdatePayee(payee, fields);

        return PayeeMapper.Map(payee);
      }
    }

    #endregion Use cases

    #region Helpers

    private SubledgerAccountQuery BuildSubledgerAccountQuery(PayeeFields fields) {
      return new SubledgerAccountQuery {
        UID = fields.SubledgerAccount,
        Name = fields.Name,
        TaxCode = fields.TaxCode
      };
    }


    private async Task MatchPayeeSubledgerAccount(SubledgerAccountQuery query) {
      Assertion.Require(query, nameof(query));

      await Task.CompletedTask;

      //var fields = new NamedEntityFields {
      //  UID = query.UID,
      //  Name = query.Name,
      //};

      //var financialAccountingServices = new AccountsServices();

      //FixedList<NamedEntityDto> subledgerAccounts = await
      //                              financialAccountingServices.SearchSuppliersSubledgerAccounts(fields);

      //subledgerAccounts = subledgerAccounts.FindAll(x => x.UID == query.UID);

      //if (subledgerAccounts.Count != 1) {
      //  Assertion.RequireFail("No encontré ningún auxiliar de proveedores en SICOFIN " +
      //                        "que coincida con datos similares a los de este proveedor. " +
      //                        "Es necesario registrar o revisar el auxiliar asociado a este proveedor en SICOFIN, " +
      //                        "y posteriormente volver a intentar ejecutar esta operación.");
      //}
    }


    private FixedList<Payee> GetPayeesByTaxCode(string taxCode) {
      Assertion.Require(taxCode, nameof(taxCode));

      taxCode = EmpiriaString.Clean(taxCode).ToUpper();

      return Payee.GetList<Payee>($"PARTY_CODE = '{taxCode}' AND PARTY_STATUS <> 'X'")
                  .ToFixedList();
    }

    #endregion Helpers

  }  // class PayeeUseCases

}  // namespace Empiria.Banobras.Payments.AppServices
