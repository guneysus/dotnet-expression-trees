using System.Reflection;

namespace Playground;
using DumpExt;
internal static class Examples
{
    static void Add1()
    {
        BinaryExpression body = Expression.Add(Expression.Constant(3), Expression.Constant(1));
        LambdaExpression lambda = Expression.Lambda(body);
        Delegate _delegate = lambda.Compile();
        _delegate.DynamicInvoke().Dump();
    }

    static void AddAssign1()
    {
        ParameterExpression p = Expression.Variable(typeof(int), "p");
        BinaryExpression body = Expression.AddAssign(p, Expression.Constant(10));
        LambdaExpression lambda = Expression.Lambda(body, p);
        var _delegate = lambda.Compile();
        _delegate.DynamicInvoke(9).Dump();
    }
    static void And1()
    {
        ParameterExpression a = Expression.Variable(typeof(bool), "a");

        BinaryExpression body = Expression.And(a, Expression.Constant(true));
        LambdaExpression lambda = Expression.Lambda(body, a);
        var _delegate = lambda.Compile();

        _delegate.DynamicInvoke(true).Dump();
        _delegate.DynamicInvoke(false).Dump();
    }

    static void AndAlso1()
    {
        ParameterExpression a = Expression.Variable(typeof(bool), "a");

        BinaryExpression body = Expression.AndAlso(a, Expression.Constant(true));
        LambdaExpression lambda = Expression.Lambda(body, a);
        var _delegate = lambda.Compile();

        _delegate.DynamicInvoke(true).Dump();
        _delegate.DynamicInvoke(false).Dump();
    }

    static void AndAssign1()
    {
        ParameterExpression a = Expression.Variable(typeof(bool), "a");

        BinaryExpression body = Expression.AndAssign(a, Expression.Constant(true));
        LambdaExpression lambda = Expression.Lambda(body, a);
        var _delegate = lambda.Compile();

        _delegate.DynamicInvoke(true).Dump();
        _delegate.DynamicInvoke(false).Dump();
    }


    static void ArrayAccess2()
    {
        ParameterExpression arr = Expression.Variable(typeof(int[,]), "arr");
        ParameterExpression i = Expression.Variable(typeof(int), "i");
        ParameterExpression j = Expression.Variable(typeof(int), "j");

        var body = Expression.ArrayAccess(arr, i, j);
        LambdaExpression lambda = Expression.Lambda(body, arr, i, j);
        var _delegate = lambda.Compile();

        _delegate.DynamicInvoke(new int[,]
            { {10,  20,   30}, {100, 200, 300}}, 0, 2).Dump();
    }


    static void ArrayAccess1()
    {
        ParameterExpression p = Expression.Variable(typeof(int[]), "arr");
        ParameterExpression i = Expression.Variable(typeof(int), "i");

        var body = Expression.ArrayAccess(p, i);
        LambdaExpression lambda = Expression.Lambda(body, p, i);
        var _delegate = lambda.Compile();

        _delegate.DynamicInvoke(new int[] { 100, 200, 300 }, 2).Dump();
    }

    static void ArrayLength1()
    {
        ParameterExpression p = Expression.Variable(typeof(int[]), "arr");

        var body = Expression.ArrayLength(p);
        LambdaExpression lambda = Expression.Lambda(body, p);
        var _delegate = lambda.Compile();

        _delegate.DynamicInvoke(new int[] { 1, 2, 3, 4, 5 }).Dump();
    }

    static void AssignProperty1()
    {
        ParameterExpression p = Expression.Parameter(typeof(Student), "p");
        ParameterExpression value = Expression.Parameter(typeof(int), "value");

        var prop = Expression.Property(p, "Id");
        var body = Expression.Assign(prop, value);
        LambdaExpression lambda = Expression.Lambda(body, p, value);
        var _delegate = lambda.Compile();

        var s = new Student();
        _delegate.DynamicInvoke(s, 100);
        s.Id.Dump();
    }

    static void Bind1()
    {
        var idValueParam = Expression.Parameter(typeof(int), "Id");

        var newExpr = Expression.New(typeof(Student));
        System.Reflection.MemberInfo m = typeof(Student).GetProperty("Id");

        var bindExprs = new[]
            {
                Expression.Bind(m, idValueParam)
              // You can bind more properties here if you like.
        };

        var body = Expression.MemberInit(newExpr, bindExprs);
        var lambda = Expression.Lambda(body, idValueParam);
        var _delegate = lambda.Compile();
        _delegate.DynamicInvoke(100).Dump();
    }

    static void MemberAssign1()
    {
        var student = Expression.Parameter(typeof(Student), "student");
        var value = Expression.Parameter(typeof(int), "Id");
        var m = typeof(Student).GetProperty("Id");

        var memberExp = Expression.MakeMemberAccess(student, m);
        var assign = Expression.Assign(memberExp, value);


        var lambda = Expression.Lambda(assign, student, value);
        var _delegate = lambda.Compile();
        Student instance = new Student() { Id = 99 };
        _delegate.DynamicInvoke(instance, 100).Dump();
    }


    static void And2()
    {
        ParameterExpression a = Expression.Variable(typeof(bool), "a");
        ParameterExpression b = Expression.Variable(typeof(bool), "b");

        BinaryExpression body = Expression.And(a, b);
        LambdaExpression lambda = Expression.Lambda(body, a, b);
        var _delegate = lambda.Compile();

        _delegate.DynamicInvoke(true, true).Dump();
        _delegate.DynamicInvoke(true, false).Dump();
        _delegate.DynamicInvoke(false, true).Dump();
        _delegate.DynamicInvoke(false, false).Dump();
    }

    public static void Run()
    {
        Add1();
        AddAssign1();
        And1();
        And2();
        AndAlso1();
        AndAssign1();
        ArrayAccess1();
        ArrayAccess2();
        ArrayLength1();
        AssignProperty1();
        Bind1();
        MemberAssign1();
        Block1();
        Label1();
        Call1();
        Coalesce1();
    }

    private static void Coalesce1()
    {
        var a = Expression.Parameter(typeof(string), "a");
        var b = Expression.Parameter(typeof(string), "b");
        Expression.Lambda(
            Expression.Coalesce(a, b)
        , a, b).Compile().DynamicInvoke("Hello", null).Dump();
    }

    private static void Call1()
    {
        Expression.Lambda(
        Expression.Call(
            null,
            typeof(Console).GetMethod("WriteLine", new Type[] {
                typeof(string)
            }), Expression.Constant("Hello World"))).Compile().DynamicInvoke().Dump();
    }

    private static void Label1()
    {
        // Add the following directive to the file:
        // using System.Linq.Expressions;

        // Creating a parameter expression.
        ParameterExpression value = Expression.Parameter(typeof(int), "value");

        // Creating an expression to hold a local variable.
        ParameterExpression result = Expression.Parameter(typeof(int), "result");

        // Creating a label to jump to from a loop.
        LabelTarget label = Expression.Label(typeof(int));

        // Creating a method body.
        BlockExpression block = Expression.Block(
            new[] { result },
            Expression.Assign(result, Expression.Constant(1)),
                Expression.Loop(
                   Expression.IfThenElse(
                       Expression.GreaterThan(value, Expression.Constant(1)),
                       Expression.MultiplyAssign(result,
                           Expression.PostDecrementAssign(value)),
                       Expression.Break(label, result)
                   ),
               label
            )
        );

        // Compile and run an expression tree.
        int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);

        factorial.Dump();

        // This code example produces the following output:
        //
        // 120
    }

    private static void Block1()
    {
        var p = Expression.Parameter(typeof(int), "p");
        var a = Expression.Parameter(typeof(int), "a");

        var add = Expression.AddAssign(p, a);

        Expression.Lambda(Expression.Block(add, add, add), p, a).Compile().DynamicInvoke(10, 2).Dump();
    }
}

