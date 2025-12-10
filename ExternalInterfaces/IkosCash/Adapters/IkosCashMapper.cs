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
using Empiria.Payments.Processor;
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


    static internal SolicitudStatus MapToIkosSolicitudStatus(string idSolicitud) {
      return new SolicitudStatus {
        IdSolicitud = idSolicitud
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


    static internal IkosCashCancelTransactionPayload MapToCancelTransactionPayload(PaymentInstructionDto instruction) {
      throw new NotImplementedException();
    }


    static internal PaymentInstructionResultDto MapToPaymentResultDto(IkosCashTransactionResult result) {
      if (result.Code == 0) {
        return new PaymentInstructionResultDto {
          PaymentNo = result.IdSistemaExterno,
          ExternalRequestID = result.IdSolicitud,
          Status = PaymentInstructionStatus.InProcess,
          ExternalStatusName = GetStatusName('O'),
          ExternalResultText = result.ErrorMesage
        };
      }

      return new PaymentInstructionResultDto {
        PaymentNo = result.IdSistemaExterno,
        ExternalRequestID = "La solicitud a IkosCash falló",
        Status = PaymentInstructionStatus.Failed,
        ExternalStatusName = GetStatusName('K'),
        ExternalResultText = result.ErrorMesage
      };
    }


    static internal PaymentInstructionResultDto MapToPaymentResultDto(IkosCashCancelTransactionResult result) {
      throw new NotImplementedException();
    }


    static internal IkosCashTransactionPayload MapToTransactionPayload(PaymentInstructionDto instruction) {
      return new IkosCashTransactionPayload {
        Header = MapTransactionHeader(instruction),
        Payload = MapTransactionInnerPayload(instruction),
      };
    }

    static internal PaymentInstructionStatusDto MapToPaymentInstructionStatus(IkosStatusDto ikosStatus) {
      return new PaymentInstructionStatusDto {
        ExternalRequestID = ikosStatus.IdSolicitud,
        ExternalStatusName = GetStatusName(ikosStatus.Status),
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
          return PaymentInstructionStatus.InProcess;

        case 'K':
          return PaymentInstructionStatus.Failed;

        case 'L':
          return PaymentInstructionStatus.Payed;

        default:
          throw Assertion.EnsureNoReachThisCode($"Status desconocido de IkosCash: '{status}'");
      }
    }

    static private IkosCashTransactionHeader MapTransactionHeader(PaymentInstructionDto instruction) {
      return new IkosCashTransactionHeader {
        IdSistemaExterno = instruction.RequestUniqueNo,
        IdUsuario = IkosCashConstantValues.TRANSACTION_ID_USUARIO,
        IdDepartamento = 40,
        IdConcepto = 418, // ToDo Cambiar por costo financiero
        ClaveCliente = instruction.PaymentOrder.PayTo.Code,
        Cuenta = instruction.PaymentOrder.PaymentAccount.CLABE,
        FechaOperacion = instruction.RequestedTime,
        FechaValor = instruction.RequestedTime,
        Monto = instruction.PaymentOrder.Total,
        Referencia = instruction.ReferenceNo,
        ConceptoPago = "Pago Banobras, S.N.C.",
        Origen = IkosCashConstantValues.TRANSACTION_ORIGEN,
        SerieFirma = IkosCashConstantValues.SERIE_FIRMA,
      };

    }


    static private IkosCashTransactionInnerPayload MapTransactionInnerPayload(PaymentInstructionDto instruction) {
      string rfc = ((ITaxableParty) instruction.PaymentOrder.PayTo).TaxData.TaxCode;

      return new IkosCashTransactionInnerPayload {
        NomBen = instruction.PaymentOrder.PaymentAccount.HolderName,
        RfcBen = rfc,
        ClaveRastreo = "",
        CtaBen = "",
        Iva = 0,
        InstitucionBen = instruction.PaymentOrder.PaymentAccount.Institution.BrokerCode,
        TipoCtaBen = int.Parse(instruction.PaymentOrder.PaymentMethod.BrokerCode),
      };
    }


    #endregion Helpers

  } // class Mapper

}  //namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
