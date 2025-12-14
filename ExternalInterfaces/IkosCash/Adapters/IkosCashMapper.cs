/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Mapper class                            *
*  Type     : IkosCashMapper                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapper service for IkosCash payment services.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  /// <summary>Mapper service for IkosCash payment services.</summary>
  static internal class IkosCashMapper {

    #region Internal Methods

    static internal string BuildCadenaOriginalFirma(IkosCashTransactionPayload request) {

      return request.Header.Referencia + ";" +
             request.Header.IdUsuario.ToString() + ";" +
             request.Header.Cuenta + ";" +
             request.Header.FechaValor.ToString("yyyyMMdd") + ";" +
             request.Header.Monto.ToString("0.00") + ";" +
             request.Header.IdSistemaExterno + ";" +
             request.Header.ClaveCliente + ";" +
             request.Header.FechaOperacion.ToString("yyyyMMdd") + ";" +
             request.Header.IdDepartamento + ";" +
             request.Header.IdConcepto + ";";
      //request.Payload.InstitucionBen + ";" +
      //request.Payload.NomBen + ";" +
      //request.Payload.RfcBen + ";" +
      //request.Payload.TipoCtaBen + ";" +
      //request.Payload.CtaBen + ";" +
      //request.Payload.Iva.ToString("0.00");
    }


    static internal BrokerResponseDto MapToBrokerResponseDto(IkosCashInstructionResult result) {

      const char SUCCESSFUL_STATUS = 'O';
      const char FAILED_STATUS = 'K';

      var brokerResponse = new BrokerResponseDto {
        BrokerInstructionNo = result.IdSolicitud,
        PaymentInstructionNo = result.IdSistemaExterno,
        BrokerMessage = result.ErrorMesage,
      };

      if (result.Successful) {
        brokerResponse.BrokerStatusText = GetStatusName(SUCCESSFUL_STATUS);
        brokerResponse.Status = PaymentInstructionStatus.Requested;
      } else {
        brokerResponse.BrokerStatusText = GetStatusName(FAILED_STATUS);
        brokerResponse.Status = PaymentInstructionStatus.Failed;
      }

      return brokerResponse;
    }


    static internal BrokerResponseDto MapToBrokerResponseDto(IkosCashCancelTransactionResult result) {
      throw new NotImplementedException();
    }


    static internal BrokerResponseDto MapToBrokerResponseDto(BrokerRequestDto brokerRequest, IkosStatusDto ikosStatus) {
      return new BrokerResponseDto {
        BrokerInstructionNo = ikosStatus.IdSolicitud,
        BrokerMessage = string.Empty,
        PaymentInstructionNo = brokerRequest.PaymentInstructionNo,
        BrokerStatusText = GetStatusName(ikosStatus.Status),
        Status = MapStatus(ikosStatus.Status)
      };
    }


    static internal IkosCashCancelTransactionPayload MapToIkosCashCancelTransactionPayload(BrokerRequestDto brokerRequest) {
      throw new NotImplementedException();
    }


    static internal IkosCashTransactionPayload MapToIkosCashTransactionPayload(BrokerRequestDto brokerRequest) {
      return new IkosCashTransactionPayload {
        Header = MapTransactionHeader(brokerRequest),
        Payload = MapTransactionInnerPayload(brokerRequest)
      };
    }


    static internal IkosStatusRequest MapToIkosCashSolicitudStatus(BrokerRequestDto brokerRequest) {
      return new IkosStatusRequest {
        IdSolicitud = brokerRequest.PaymentInstructionNo
      };
    }


    static internal List<OrganizationUnitDto> MapToOrganizationUnits(List<DepartamentoDto> departamentos) {
      var organizationUnits = new List<OrganizationUnitDto>(departamentos.Count);

      foreach (var departamento in departamentos) {
        organizationUnits.Add(MapToOrganizationUnit(departamento));
      }

      return organizationUnits;
    }


    static internal OrganizationUnitDto MapToOrganizationUnit(DepartamentoDto departamento) {
      return new OrganizationUnitDto {
        Id = departamento.IdDepartamento,
        Code = departamento.Clave,
        Name = departamento.Nombre,
        Descripcion = departamento.Descripcion,
        Concepts = MapToOrganizationUnitConcepts(departamento.Conceptos)
      };
    }


    static internal List<OrganizationUnitConceptDto> MapToOrganizationUnitConcepts(List<ConceptoDto> conceptos) {
      var organizationUnitConcepts = new List<OrganizationUnitConceptDto>(conceptos.Count);

      foreach (var concepto in conceptos) {
        organizationUnitConcepts.Add(MapToOrganizationUnitConcept(concepto));
      }

      return organizationUnitConcepts;
    }


    static internal OrganizationUnitConceptDto MapToOrganizationUnitConcept(ConceptoDto concepto) {
      return new OrganizationUnitConceptDto {
        Id = concepto.IdConcepto,
        Code = concepto.Clave,
        Name = concepto.Nombre,
        Descripcion = concepto.Descripcion
      };
    }


    #endregion Internal Methods

    #region Helpers

    static private string GetStatusName(char status) {

      switch (status) {

        case 'O':
          return "Autorizado, en proceso";

        case 'P':
          return "Requiere autorización por rompimiento de límites";

        case 'K':
          return "Cancelado o rechazado";

        case 'L':
          return "Pago realizado";

        case 'C':
          return "Captura manual, en proceso";

        case 'V':
          return "Validado, en proceso";

        default:
          return $"Status desconocido de IkosCash: '{status}'";
      }
    }


    static private PaymentInstructionStatus MapStatus(char status) {
      switch (status) {

        case 'O':
        case 'P':
        case 'C':
        case 'V':
          return PaymentInstructionStatus.InProgress;

        case 'K':
          return PaymentInstructionStatus.Failed;

        case 'L':
          return PaymentInstructionStatus.Payed;

        default:
          throw Assertion.EnsureNoReachThisCode($"Status desconocido de IkosCash: '{status}'");
      }
    }


    static private IkosCashTransactionHeader MapTransactionHeader(BrokerRequestDto brokerRequest) {
      return new IkosCashTransactionHeader {
        IdUsuario = IkosCashConstantValues.TRANSACTION_ID_USUARIO,
        Origen = IkosCashConstantValues.TRANSACTION_ORIGEN,
        SerieFirma = IkosCashConstantValues.SERIE_FIRMA,

        IdSistemaExterno = brokerRequest.PaymentInstructionNo,
        IdDepartamento = brokerRequest.PaymentRequesterId,
        IdConcepto = brokerRequest.PaymentWithdrawalAccountId,
        ClaveCliente = brokerRequest.BeneficiaryTaxCode,
        Cuenta = brokerRequest.BeneficiaryAccountNo,
        FechaOperacion = brokerRequest.ProgrammedDate,
        FechaValor = brokerRequest.EffectiveDate,
        Monto = brokerRequest.PaymentTotal,
        Referencia = brokerRequest.PaymentReferenceNo,
        ConceptoPago = brokerRequest.PaymentDescription
      };

    }


    static private IkosCashTransactionInnerPayload MapTransactionInnerPayload(BrokerRequestDto brokerRequest) {

      return new IkosCashTransactionInnerPayload {
        NomBen = brokerRequest.BeneficiaryName,
        RfcBen = brokerRequest.BeneficiaryTaxCode,
        ClaveRastreo = "",
        CtaBen = "",
        Iva = 0,
        InstitucionBen = brokerRequest.BeneficiaryBankCode,
        TipoCtaBen = int.Parse(brokerRequest.BeneficiaryAccountTypeCode)
      };
    }


    #endregion Helpers

  } // class Mapper

}  //namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
