using CoreLibrary.Interface.Repositories;
using Domain.Exceptions;
using infrastructure.Extensions;
using infrastructure.Setting;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

namespace infrastructure.Repositories.RepositoryAsync;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ServiceContext _dbContext;
    protected readonly DbSet<TEntity> _entities;

    public GenericRepository(ServiceContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _entities = _dbContext.Set<TEntity>();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        ValidateNullEntity<TEntity>.IsNullEntity(entity);

        await _entities.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        ValidateNullEntity<TEntity>.IsNullEntity(entity);

        _entities.Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task BulkInsertAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any()) return;

     
        var entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
        var tableName = entityType.GetTableName();
        var schema = entityType.GetSchema() ?? "public";

        
        var properties = entityType.GetProperties()
            .Where(p => !p.IsPrimaryKey() || !p.ValueGenerated.HasFlag(Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd))
            .ToList();

        var columnNames = properties.Select(p => p.GetColumnName()).ToList();
        var propertyInfos = properties.Select(p => p.PropertyInfo).ToList();

        
        var columnsSql = string.Join(", ", columnNames.Select(c => $"\"{c}\""));
        var copySql = $"COPY \"{schema}\".\"{tableName}\" ({columnsSql}) FROM STDIN (FORMAT BINARY)";

        
        var connection = (NpgsqlConnection)_dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open) await connection.OpenAsync();

        using (var writer = await connection.BeginBinaryImportAsync(copySql))
        {
            foreach (var entity in entities)
            {
                await writer.StartRowAsync();

                foreach (var propInfo in propertyInfos)
                {
                    var value = propInfo?.GetValue(entity);
                    await writer.WriteAsync(value);
                }
            }
            await writer.CompleteAsync();
        }
    }
}