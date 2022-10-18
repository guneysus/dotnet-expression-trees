namespace FluentExpressionTrees;

public class FluentExpressionBuilder
{
    private Dictionary<string, ParameterExpression> _parameters = new();

    private List<Expression> _expressions = new();
    protected FluentExpressionBuilder DefineParameter<T>(string name)
    {
        this._parameters.Add(name, Expression.Parameter(typeof(T)));
        return this;
    }

    public static FluentExpressionBuilder New()
    {
        return new FluentExpressionBuilder();
    }

    public FluentExpressionBuilder Parameter<T>(string name)
    {
        return this.DefineParameter<T>(name);
    }

    public FluentExpressionBuilder Binary(ExpressionType expressionType, string left, string right)
    {
        var binary = Expression.MakeBinary(expressionType, this._parameters[left], this._parameters[right]);

        _expressions.Add(binary);

        return this;
    }

    public FluentExpressionBuilder @__For<T>(T init, Expression<Predicate<T>> predicate, Action<T> func)
    {
        Expression<Action<T>> bind = (x) => func(x);
        Expression callExpr = Expression.Invoke(bind, Expression.Constant(5));
        this._expressions.Add(callExpr);

        return this;
    }

    public Delegate Build()
    {
        return Expression.Lambda(Expression.Block(_expressions), _parameters.Values).Compile();
    }
}





