using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Querys.Interface;

public interface IQueryService
{
    // Para listas
    //  var (data, message) = await _unitOfWork.Queries.QueryAsync<dynamic>(SqlQueries.GetAll);
    Task<(IEnumerable<T> Data, string Message)> QueryAsync<T>(string sql, object? parameters = null);



    // Para un solo registro
//    var(data, message) = await _queryService.QueryFirstOrDefaultAsync<Accident>(
//    SqlQueries.GetById,
//    new { Id = 123 }
//);
    Task<(T? Data, string Message)> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null);
}
