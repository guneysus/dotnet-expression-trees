using System.Collections;

namespace CustomLinqProviderSandbox;

public sealed class DatabaseTable<TEntity> : IQueryable<TEntity>
{
    public Expression Expression => Expression.Constant(this);

    public Type ElementType => throw new NotImplementedException();

    public IQueryProvider Provider => throw new NotImplementedException();

    public IEnumerator<TEntity> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
