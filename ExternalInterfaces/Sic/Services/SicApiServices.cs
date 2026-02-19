/* Empiria Connector******************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Integration Layer                       *
*  Assembly : Sic.Connector.dll                          Pattern   : Service provider                        *
*  Type     : SicApiServices                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Implement accounting transactions of SIC from the API.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.BanobrasIntegration.Sic.Adapters;

namespace Empiria.BanobrasIntegration.Sic {

  /// <summary>Implements accounting entries of sic.</summary>
  public class SicApiService {

    private readonly SicApiClient _apiClient;

    #region Methods

    public SicApiService() {
      _apiClient = new SicApiClient();
    }


    internal async Task<FixedList<MovtosDetalleDto>> GetMovimientos(MovtosEncabezadosDto header) {

      var movimientos = await _apiClient.TryGetMovtosDetalle(header);

      return movimientos;

    }


    #endregion Methods

  } // class PaymentService

} // namespace Empiria.Payments.BanobrasIntegration.IkosCash
