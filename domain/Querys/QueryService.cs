using Dapper;
using Domain.Querys.Interface;
using System.Data;

namespace Infraestructure.Queries;

public class QueryService : IQueryService
{
    private readonly IDbConnection _connection;

    public QueryService(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<(IEnumerable<T> Data, string Message)> QueryAsync<T>(string sql, object? parameters = null)
    {
        try
        {
            if (_connection.State == ConnectionState.Closed) _connection.Open();

            var data = await _connection.QueryAsync<T>(sql, parameters);
            return (data, "Success");
        }
        catch (Exception ex)
        {
            return (Enumerable.Empty<T>(), $"Error: {ex.Message}");
        }
    }

    public async Task<(T? Data, string Message)> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        try
        {
            if (_connection.State == ConnectionState.Closed) _connection.Open();

            var data = await _connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
            return (data, "Success");
        }
        catch (Exception ex)
        {
            return (default, $"Error: {ex.Message}");
        }
    }
}