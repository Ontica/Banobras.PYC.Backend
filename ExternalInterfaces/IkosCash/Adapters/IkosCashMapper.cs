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


    static internal SolicitudField MapToIkosMinimalDto(string Idsolicitud) {
      return new SolicitudField {
        IdSolicitud = Idsolicitud
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


    static internal PaymentResultDto MapToPaymentResultDto(IkosCashTransactionResult result) {
      if (result.Code == 0) {
        return new PaymentResultDto {
          PaymentNo = result.IdSistemaExterno,
          RequestID = result.IdSolicitud,
          Status = 'O',
          StatusName = GetStatusName('0'),
          Text = result.ErrorMesage,
          Failed = false,
        };
        } else {
          return new PaymentResultDto {
            PaymentNo = result.IdSistemaExterno,
            RequestID = result.IdSolicitud,
            Status = 'K',
            StatusName = GetStatusName('K'),
            Text = result.ErrorMesage,
            Failed = false,
          };
      }

    }
    

    static internal PaymentResultDto MapToPaymentResultDto(IkosCashCancelTransactionResult result) {
      throw new NotImplementedException();
    }


    static internal IkosCashTransactionPayload MapToTransactionPayload(PaymentInstructionDto instruction) {
      return new IkosCashTransactionPayload {
         Header = MapTransactionHeader(instruction),
         Payload = MapTransactionInnerPayload(instruction),
      };
    }

    static internal PaymentResultDto MapToPaymentResultDto(IkosStatusDto ikosStatus) {
      return new PaymentResultDto {
        RequestID = ikosStatus.IdSolicitud,
        Status = ikosStatus.Status,
        StatusName = GetStatusName(ikosStatus.Status)
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


    static private IkosCashTransactionHeader MapTransactionHeader(PaymentInstructionDto instruction) {
      return new IkosCashTransactionHeader {
        Referencia = instruction.ReferenceNo,
        IdSistemaExterno = instruction.RequestUniqueNo,
        IdUsuario = IkosCashConstantValues.TRANSACTION_ID_USUARIO,
        FechaOperacion = instruction.RequestedTime,
        FechaValor = instruction.RequestedTime,
        Monto = instruction.PaymentOrder.Total,
        Cuenta = instruction.PaymentOrder.PaymentAccount.CLABE,
        Origen = IkosCashConstantValues.TRANSACTION_ORIGEN,
        SerieFirma = IkosCashConstantValues.SERIE_FIRMA,

        ClaveCliente = "",
        ConceptoPago = "",
        IdConcepto = 1,
        IdDepartamento = 2
      };
    }


    static private IkosCashTransactionInnerPayload MapTransactionInnerPayload(PaymentInstructionDto instruction) {
      string rfc = "";

      if (instruction.PaymentOrder.PayTo is Organization organization) {
        rfc = organization.TaxData.TaxCode;

      } else if (instruction.PaymentOrder.PayTo is Person person) {
        rfc = person.TaxData.TaxCode;
      } else {
        throw Assertion.EnsureNoReachThisCode($"Unhandled PaymentOrder.PayTo type: " +
                                              $"{instruction.PaymentOrder.PayTo.PartyType.DisplayName}");
      }

      return new IkosCashTransactionInnerPayload {
        NomBen = instruction.PaymentOrder.PaymentAccount.HolderName,
        RfcBen = rfc,
        ClaveRastreo = "",
        CtaBen = "",
        Iva = 0,

        InstitucionBen = "",
        TipoCtaBen = 40,
      };
    }


    #endregion Helpers

  } // class Mapper

}  //namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
