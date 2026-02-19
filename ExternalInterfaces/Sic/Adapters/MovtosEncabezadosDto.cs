/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Sic.Connector.dll                          Pattern   : Data Transfer Object                    *
*  Type     : MovtosEncabezadosDto                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to get accounting entries list from system sic                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.BanobrasIntegration.Sic.Adapters {

    public class MovtosEncabezadosDto
    {

        public int idCredito
        {
            get; set;
        }

        public string fecConsulta {
            get; set;
        }

    } // class MovtosEncabezadosDto


    public class MovtosDetalleDto
    {

        public string FECHA
        {
            get; set;
        }

        public string CONCEPTO
        {
            get; set;
        }

        public decimal IMPORTE
        {
            get; set;
        }

    } // class MovtosDetalleDto

} // namespace Empiria.BanobrasIntegration.Sic.Adapters
