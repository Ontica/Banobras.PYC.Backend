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

using Empiria.Parties;
using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {

  /// <summary>Mapper service for IkosCash payment services.</summary>
  static internal class IkosCashMapper {

    #region Internal Methods

    static internal List<OrganizationUnitDto> MapToOrganizationUnits(List<DepartamentoDto> departamentos) {
      List<OrganizationUnitDto> organizationUnits = new List<OrganizationUnitDto>();

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
      List<OrganizationUnitConceptDto> organizationUnitConcepts = new List<OrganizationUnitConceptDto>();

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


    static internal IkosStatusRequest MapToIkosSolicitudStatus(BrokerRequestDto brokerRequest) {
      return new IkosStatusRequest {
        IdSolicitud = brokerRequest.RequestUniqueNo
      };
    }

    static internal string GetCadenaOriginalFirma(IkosCashTransactionPayload request) {

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


    static internal IkosCashCancelTransactionPayload MapToCancelTransactionPayload(BrokerRequestDto brokerRequest) {
      throw new NotImplementedException();
    }


    static internal BrokerResponseDto MapToBrokerResponseDto(IkosCashInstructionResult result) {

      const char SUCCESSFUL_STATUS = 'O';
      const char FAILED_STATUS = 'K';

      if (result.Successful) {
        return new BrokerResponseDto {
          BrokerInstructionNo = result.IdSolicitud,

          InstructionNo = result.IdSistemaExterno,
          BrokerStatusText = GetStatusName(SUCCESSFUL_STATUS),
          BrokerMessage = result.ErrorMesage,
          Status = PaymentInstructionStatus.Requested,
        };
      }

      return new BrokerResponseDto {
        BrokerInstructionNo = result.IdSolicitud,
        InstructionNo = result.IdSistemaExterno,
        Status = PaymentInstructionStatus.Failed,
        BrokerStatusText = GetStatusName(FAILED_STATUS),
        BrokerMessage = result.ErrorMesage
      };
    }


    static internal BrokerResponseDto MapToBrokerResponseDto(IkosCashCancelTransactionResult result) {
      throw new NotImplementedException();
    }


    static internal IkosCashTransactionPayload MapToIkosCashTransactionPayload(BrokerRequestDto brokerRequest) {
      return new IkosCashTransactionPayload {
        Header = MapTransactionHeader(brokerRequest),
        Payload = MapTransactionInnerPayload(brokerRequest)
      };
    }


    static internal BrokerResponseDto MapToBrokerResponseDto(BrokerRequestDto brokerRequest, IkosStatusDto ikosStatus) {
      return new BrokerResponseDto {
        BrokerInstructionNo = ikosStatus.IdSolicitud,
        BrokerMessage = string.Empty,
        InstructionNo = brokerRequest.RequestUniqueNo,
        BrokerStatusText = GetStatusName(ikosStatus.Status),
        Status = MapStatus(ikosStatus.Status)
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
        IdSistemaExterno = brokerRequest.RequestUniqueNo,
        IdUsuario = IkosCashConstantValues.TRANSACTION_ID_USUARIO,
        IdDepartamento = 40,
        IdConcepto = 418, // ToDo Cambiar por costo financiero
        ClaveCliente = brokerRequest.PaymentOrder.PayTo.Code,
        Cuenta = brokerRequest.PaymentOrder.PaymentAccount.CLABE,
        FechaOperacion = brokerRequest.RequestedTime,
        FechaValor = brokerRequest.RequestedTime,
        Monto = brokerRequest.PaymentOrder.Total,
        Referencia = brokerRequest.ReferenceNo,
        ConceptoPago = "Pago Banobras, S.N.C.",
        Origen = IkosCashConstantValues.TRANSACTION_ORIGEN,
        SerieFirma = IkosCashConstantValues.SERIE_FIRMA,
      };

    }


    static private IkosCashTransactionInnerPayload MapTransactionInnerPayload(BrokerRequestDto brokerRequest) {
      string rfc = ((ITaxableParty) brokerRequest.PaymentOrder.PayTo).TaxData.TaxCode;

      return new IkosCashTransactionInnerPayload {
        NomBen = brokerRequest.PaymentOrder.PaymentAccount.HolderName,
        RfcBen = rfc,
        ClaveRastreo = "",
        CtaBen = "",
        Iva = 0,
        InstitucionBen = brokerRequest.PaymentOrder.PaymentAccount.Institution.BrokerCode,
        TipoCtaBen = int.Parse(brokerRequest.PaymentOrder.PaymentMethod.BrokerCode),
      };
    }


    #endregion Helpers

  } // class Mapper

}  //namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
