namespace Playground;

using DumpExt;
using System.Reflection;
using static _;

internal static class Program
{
    static void Main(string[] args)
    {
        Examples.Run();

        //Member<Student, int>(x => x.Id).Dump();
        return;


        var N = 3;

        sum(N).Dump();
        sumExpA(N).Dump();
        sumExp().DynamicInvoke(N).Dump();

        var sumN = For(ExpressionType.AddAssign, 0);
        var factorial = For(ExpressionType.MultiplyAssign, 1);

        sumN(4).Dump();
        factorial(3).Dump();

        For(ExpressionType.MultiplyAssign, 1)(5).Dump();
    }
}

public static class _
{
    public static int sum(int n)
    {
        int s = 0;

        for (int i = 1; i <= n; i++)
            s += i;

        return s;
    }


    public static int sumExpA(int i)
    {
        var result = 0;

        while (true)
        {
            if (i >= 1)
            {
                result += i--;
            }
            else
            {
                break;
            }

        }

        return result;
    }

    public static MemberInfo Member<T, TP>(Expression<Func<T, TP>> prop)
    {
        MemberExpression m = (MemberExpression)prop.Body;
        var name = m.Member.Name;
        return m.Member;
    }

    public static Func<T, T> @For<T>(ExpressionType @operator, T init)
    {
        var type = typeof(T);

        ParameterExpression result = Expression.Parameter(type, "result");

        // Creating an expression to hold a local variable.
        ParameterExpression i = Expression.Parameter(type, "i");

        // Creating a label to jump to from a loop.
        LabelTarget label = Expression.Label(type, "@break");

        // Creating a method body.
        GotoExpression ifFalse = Expression.Break(label, result);

        BinaryExpression ifTrue = Expression.MakeBinary(@operator, result, Expression.PostDecrementAssign(i));

        //BinaryExpression ifTrue = Expression.AddAssign(result,
        //                           Expression.PostDecrementAssign(i));

        BinaryExpression cond = Expression.GreaterThanOrEqual(i, Expression.Constant(1));

        ConditionalExpression ifThenElse = Expression.IfThenElse(cond, ifTrue, ifFalse);
        LoopExpression loopExpression = Expression.Loop(body: ifThenElse, @break: label);

        BinaryExpression initialExp = Expression.Assign(left: result, right: Expression.Constant(init));

        BlockExpression block = Expression.Block(
            new[] { result },
            initialExp,
            loopExpression
        );

        return Expression.Lambda<Func<T, T>>(block, i).Compile();
    }

    public static Delegate sumExp()
    {
        Type type = typeof(int);

        ParameterExpression result = Expression.Parameter(type, "result");

        // Creating an expression to hold a local variable.
        ParameterExpression i = Expression.Parameter(type, "i");

        // Creating a label to jump to from a loop.
        LabelTarget label = Expression.Label(type, "@break");

        // Creating a method body.
        GotoExpression ifFalse = Expression.Break(label, result);

        BinaryExpression ifTrue = Expression.AddAssign(result,
                                   Expression.PostDecrementAssign(i));
        BinaryExpression cond = Expression.GreaterThanOrEqual(i, Expression.Constant(1));

        ConditionalExpression ifThenElse = Expression.IfThenElse(cond, ifTrue, ifFalse);
        LoopExpression loopExpression = Expression.Loop(body: ifThenElse, @break: label);

        BinaryExpression initialExp = Expression.Assign(left: result, right: Expression.Constant(0));

        BlockExpression block = Expression.Block(
            new[] { result },
            initialExp,
            loopExpression
        );

        return Expression.Lambda(block, i).Compile();
    }
}
