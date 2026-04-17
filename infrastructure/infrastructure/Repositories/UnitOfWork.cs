using CoreLibrary.Interface.Repositories;
using Domain.Querys.Interface;
using infrastructure.Repositories.RepositoryAsync;
using infrastructure.Setting;
using System.Collections;
using Microsoft.EntityFrameworkCore.Storage; // Necesario
using Microsoft.EntityFrameworkCore;        // Necesario

namespace infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ServiceContext _context;
    private readonly IQueryService _queries;
    private Hashtable _repositories;
    private IDbContextTransaction _currentTransaction; // Para trackear la transacción real

    public UnitOfWork(ServiceContext context, IQueryService queries)
    {
        _context = context;
        _queries = queries;
    }

    public IQueryService Queries => _queries;

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories == null) _repositories = new Hashtable();
        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = new GenericRepository<TEntity>(_context);
            _repositories.Add(type, repositoryInstance);
        }
        return (IGenericRepository<TEntity>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);


    public async Task BeginTransactionAsync()
    {
   
        var strategy = _context.Database.CreateExecutionStrategy();


        await strategy.ExecuteAsync(async () =>
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        });
    }

    public async Task CommitnAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
            }
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
    }
}