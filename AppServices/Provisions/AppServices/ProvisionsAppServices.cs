/* Banobras - PYC ********************************************************************************************
*                                                                                                            *
*  Module   : Banobras Expenses Management                     Component : Application services Layer        *
*  Assembly : Banobras.PYC.AppServices.dll                     Pattern   : Application service               *
*  Type     : ExpensesAppServices                              License   : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : Application services for expenses management.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Services;

using Empiria.Orders;
using Empiria.Orders.Adapters;

using Empiria.Payments.Adapters;

using Empiria.Provisions.Adapters;

namespace Empiria.Banobras.Provisions.AppServices {

  /// <summary>Application services for expenses management.</summary>
  public class ProvisionsAppServices : UseCase {

    #region Constructors and parsers

    protected ProvisionsAppServices() {
    }

    static public ProvisionsAppServices UseCaseInteractor() {
      return CreateInstance<ProvisionsAppServices>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FixedList<OrderDescriptor> SearchProvisions(ProvisionsQuery query) {

      string filter = query.MapToFilterString();
      string orderBy = query.MapToSortString();

      string sql = "SELECT * FROM OMS_ORDERS " +
                  $"WHERE {filter} " +
                  $"ORDER BY {orderBy}";

      var op = DataOperation.Parse(sql);

      var orders = DataReader.GetFixedList<Order>(op);

      return orders.Select(x => new OrderDescriptor(x))
                   .ToFixedList();
    }

    #endregion Use cases

  }  // class ProvisionsAppServices

}  // namespace Empiria.Banobras.Provisions.AppServices