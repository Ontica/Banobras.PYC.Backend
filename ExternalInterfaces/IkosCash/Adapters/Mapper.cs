/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Mapper class                            *
*  Type     : Mapper                                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data Transfer Objects mapper.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Collections.Generic;


namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters {
  public class Mapper {

    #region Global Variables

    #endregion

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


    static public string GetFirma(TransaccionFields transaccion) {
      string dataToSign = GetCadenaFirma(transaccion);

      return Signer.Sign(dataToSign);
    }


    static public string GetCadenaFirma(TransaccionFields transaccion) {
      string signatureString = transaccion.Header.Referencia + ";" +
                               transaccion.Header.IdUsuario.ToString() + ";" +
                               transaccion.Header.Cuenta + ";" +
                               transaccion.Header.FechaValor.ToString("yyyyMMdd") + ";" +
                               transaccion.Header.Monto.ToString("0.00") + ";" +
                               transaccion.Header.IdSistemaExterno + ";" +
                               transaccion.Header.ClaveCliente + ";" +
                               transaccion.Header.FechaOperacion.ToString("yyyyMMdd") + ";" +
                               transaccion.Header.IdDepartamento + ";" +
                               transaccion.Header.IdConcepto + ";";
                              //transaccion.Payload.InstitucionBen + ";" +
                              //transaccion.Payload.NomBen + ";" +
                              //transaccion.Payload.RfcBen + ";" +
                              //transaccion.Payload.TipoCtaBen + ";" +
                              //transaccion.Payload.CtaBen + ";" +
                              //transaccion.Payload.Iva.ToString("0.00");

      return signatureString;
    }

    static internal PaymentStatusResultDto MapToPaymentStatusDTO(List<IkosStatusDto> statusList) {

      return new PaymentStatusResultDto {
        IdSolicitud = statusList[0].IdSolicitud,
        Status = statusList[0].Status,
        StatusName = GetStatusName(statusList[0].Status)
      };

    }

    #endregion Internal Methods

    #region Helpers

    static private string GetStatusName(char status) {
      switch (status) {
        case 'O':
          return "Authorized";
        case 'P':
          return "Requiere autorización por rompimiento de límites";
        case 'K':
          return "Canceled";
        case 'L':
          return "Liquidated";
        case 'C':
          return "Manual Capture";
        case 'V':
          return "Validated";
        default:
          return "Undefined";
      }

    }

    #endregion Helpers


  } // class Mapper

}  //namespace Empiria.Payments.BanobrasIntegration.IkosCash.Adapters
