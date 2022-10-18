namespace FluentExpressionTrees.Console;

using System;
using DumpExt;

internal static class Program
{
    static void Main(string[] args)
    {
        Loop(i: () => 1,
            until: i => i <= 5,
            step: i => i + 1,
            body: (i, result) => result + i,
            result: () => 0).Dump();

        Loop(i: () => 1,
            until: i => i <= 5,
            step: i => i + 1,
            body: (i, result) => result * i,
            result: () => 1).Dump();

        Loop(i: () => 1,
            until: i => i <= 6,
            step: i => i + 1,
            body: (i, result) => result * i,
            result: () => 1).Dump();

        Loop(i: 1,
            @while: i => i <= 6,
            step: i => i + 1,
            body: (i, result) => result * i,
            result: 1).Dump();


        var list = new List<int>();
        Loop(i: 1,
            @while: i => i <= 10,
            step: i => i + 1,
            body: i => list.Add(i),
            result: 1);

        Loop(i: 0,
            @while: i => i <= 100,
            step: i => i + 10,
            body: i => Console.WriteLine(i),
            result: 1);


        int result = 0;
        Loop(i: 0, @while: i => i <= 10, step: i => i + 1, body: i => result += i);


        var fact = Loop(i: 1,
             @while: i => i <= 5,
             step: i => i + 1,
             body: (result, i) => result * i,
             result: 1,
             compile: true);


        return;

        For<int>(init: i => 1,
            until: i => i <= 6,
            i => i + 1,
            @operator: ExpressionType.MultiplyAssign).Dump();

        For<int>(init: i => 5,
            until: i => i >= 1,
            i => i - 1,
            @operator: ExpressionType.MultiplyAssign).Dump();

        For<int>(init: i => 1,
            until: i => i <= 5,
            i => i + 1,
            @operator: ExpressionType.AddAssign).Dump();

        //forRev();

    }

    private static void forRev()
    {
        var type = typeof(int);

        ParameterExpression result = Expression.Parameter(type, "result");

        // Creating an expression to hold a local variable.
        ParameterExpression i = Expression.Parameter(type, "i");

        // Creating a label to jump to from a loop.
        LabelTarget label = Expression.Label(type, "@break");

        ConditionalExpression ifThenElse = Expression
            .IfThenElse(test: Expression.GreaterThanOrEqual(i, Expression.Constant(1)),
            ifTrue: Expression.MakeBinary(ExpressionType.MultiplyAssign, result, Expression.PostDecrementAssign(i)),
            ifFalse: Expression.Break(label, result));

        LoopExpression loopExpression = Expression.Loop(body: ifThenElse, @break: label);

        BinaryExpression initialExp = Expression.Assign(left: result, right: Expression.Constant(1));

        BlockExpression block = Expression.Block(
            new[] { result },
            initialExp,
            loopExpression
        );

        var l = Expression.Lambda<Func<int, int>>(block, i).Compile();

        var r = l.DynamicInvoke(5);
    }



    private static object Loop<T>(
        Expression<Func<T>> i,
        Expression<Predicate<T>> until,
        Expression<Func<T, T>> step,
        Expression<Func<T, T, T>> body,
        Expression<Func<T>> result

        )
    {
        var type = typeof(T);

        var iInitialValue = invoke(i);
        ParameterExpression resultExp = Expression.Variable(type, "result");

        // Creating an expression to hold a local variable.
        ParameterExpression iExp = Expression.Variable(type, "i");

        // Creating a label to jump to from a loop.
        LabelTarget labelExp = Expression.Label(type, "@break");

        Expression testExp = wrap(until, iExp);
        Expression stepExp = wrap(step, iExp);

        var stepAndAssignExp = Expression.Assign(iExp, stepExp);
        var bodyExp = Expression.Assign(resultExp, wrap<T>(body, iExp, resultExp));

        var ifTrueExp = Expression.Block(
            bodyExp,
            stepAndAssignExp
        );

        ConditionalExpression ifThenElse = Expression.IfThenElse(
            test: testExp,
            ifTrue: ifTrueExp,
            ifFalse: Expression.Break(labelExp, resultExp));

        LoopExpression loopExp = Expression.Loop(body: ifThenElse, @break: labelExp);
        BinaryExpression initIExp = Expression.Assign(left: iExp, right: Expression.Constant(iInitialValue));
        BinaryExpression initResultExp = Expression.Assign(left: resultExp, right: Expression.Constant(invoke(result)));

        BlockExpression blockExp = Expression.Block(
            new[] { resultExp },
            initIExp, initResultExp,
            loopExp
        );

        var lambdaExp = Expression.Lambda<Func<T, T>>(blockExp, iExp).Compile();

        var returnValue = lambdaExp.DynamicInvoke(default(T));

        return (T)returnValue;

    }

    private static object Loop<T>(
        T i,
        Expression<Predicate<T>> @while,
        Expression<Func<T, T>> step,
        Expression<Func<T, T, T>> body,
        T result

        )
    {
        var type = typeof(T);

        ParameterExpression resultExp = Expression.Variable(type, "result");

        // Creating an expression to hold a local variable.
        ParameterExpression iExp = Expression.Variable(type, "i");

        // Creating a label to jump to from a loop.
        LabelTarget labelExp = Expression.Label(type, "@break");

        Expression testExp = wrap(@while, iExp);
        Expression stepInvoke = wrap(step, iExp);

        var stepExp = Expression.Assign(iExp, stepInvoke);
        var bodyExp = Expression.Assign(resultExp, wrap<T>(body, iExp, resultExp));

        var ifTrueExp = Expression.Block(
            bodyExp,
            stepExp
        );

        ConditionalExpression ifThenElseExp = Expression.IfThenElse(
            test: testExp,
            ifTrue: ifTrueExp,
            ifFalse: Expression.Break(labelExp, resultExp));

        LoopExpression loopExp = Expression.Loop(body: ifThenElseExp, @break: labelExp);
        BinaryExpression initIExp = Expression.Assign(left: iExp, right: Expression.Constant(i));
        BinaryExpression initResultExp = Expression.Assign(left: resultExp, right: Expression.Constant(result));

        BlockExpression blockExp = Expression.Block(
            new[] { resultExp },
            initIExp, initResultExp,
            loopExp
        );

        var lambdaExp = Expression.Lambda<Func<T, T>>(blockExp, iExp).Compile();

        var returnValue = lambdaExp.DynamicInvoke(default(T));

        return (T)returnValue;

    }

    /// <summary>
    /// TODO: Benchmark
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="i"></param>
    /// <param name="while"></param>
    /// <param name="step"></param>
    /// <param name="body"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private static object Loop<T>(
        T i,
        Expression<Predicate<T>> @while,
        Expression<Func<T, T>> step,
        Expression<Action<T>> body,
        T result
        )
    {
        var type = typeof(T);

        ParameterExpression resultExp = Expression.Variable(type, "result");

        // Creating an expression to hold a local variable.
        ParameterExpression iExp = Expression.Variable(type, "i");

        // Creating a label to jump to from a loop.
        LabelTarget labelExp = Expression.Label(type, "@break");

        Expression testExp = wrap(@while, iExp);
        Expression stepInvoke = wrap(step, iExp);

        var stepExp = Expression.Assign(iExp, stepInvoke);
        var bodyExp = wrap(body, iExp);

        var ifTrueExp = Expression.Block(
            bodyExp,
            stepExp
        );

        ConditionalExpression ifThenElseExp = Expression.IfThenElse(
            test: testExp,
            ifTrue: ifTrueExp,
            ifFalse: Expression.Break(labelExp, resultExp));

        LoopExpression loopExp = Expression.Loop(body: ifThenElseExp, @break: labelExp);
        BinaryExpression initIExp = Expression.Assign(left: iExp, right: Expression.Constant(i));
        BinaryExpression initResultExp = Expression.Assign(left: resultExp, right: Expression.Constant(result));

        BlockExpression blockExp = Expression.Block(
            new[] { resultExp },
            initIExp, initResultExp,
            loopExp
        );

        var lambdaExp = Expression.Lambda<Func<T, T>>(blockExp, iExp).Compile();

        var returnValue = lambdaExp.DynamicInvoke(default(T));

        return (T)returnValue;
    }

    private static void Loop<T>(
        T i,
        Expression<Predicate<T>> @while,
        Expression<Func<T, T>> step,
        Action<T> body
        )
    {
        Predicate<T> predicate = @while.Compile(true);
        Func<T, T> func = step.Compile(true);

        while (predicate(i))
        {
            body(i);
            i = func(i);
        }

    }

    private static T Loop<T>(
        T i,
        Expression<Predicate<T>> @while,
        Expression<Func<T, T>> step,
        Expression<Func<T, T, T>> body,
        T result,
        bool compile
        )
    {
        Predicate<T> predicate = @while.Compile(compile);
        Func<T, T> stepFunc = step.Compile(compile);
        Func<T, T, T> bodyFunc = body.Compile(compile);

        while (predicate(i))
        {
            result = bodyFunc(result, i);
            i = stepFunc(i);
        }

        return result;
    }


    private static T invoke<T>(Expression<Func<T>> init)
    {
        var type = typeof(T); ;


        var iInitialFunc = Expression.Lambda<Func<T>>(init.Body).Compile();
        T iInitialVariable = iInitialFunc.Invoke();
        return iInitialVariable;
    }

    private static T For<T>(Expression<Func<T, T>> init,
        Expression<Predicate<T>> until,
        Expression<Func<T, T>> stepExp,
        ExpressionType @operator)
    {
        var type = typeof(T);

        var iInitialFunc = Expression.Lambda<Func<T, T>>(init.Body, Expression.Parameter(type)).Compile();
        var iInitialVariable = iInitialFunc.Invoke(default);

        ParameterExpression result = Expression.Variable(type, "result");

        // Creating an expression to hold a local variable.
        ParameterExpression i = Expression.Variable(type, "i");

        // Creating a label to jump to from a loop.
        LabelTarget label = Expression.Label(type, "@break");

        //var test = until;
        Expression test = wrap(until, i);
        // Expression test = Expression.LessThanOrEqual(i, Expression.Constant(5));

        //var ifTrue = Expression.MakeBinary(@operator, result, Expression.PostIncrementAssign(i)); // result *= i++;;

        Expression stepInvoke = wrap(stepExp, i);

        var stepAndAssign = Expression.Assign(i, stepInvoke);

        var ifTrue = Expression.Block(
            Expression.MakeBinary(@operator, result, i),
            //Expression.PreIncrementAssign(i),
            stepAndAssign
        );

        /*
         if cond {
            result *= i++;
         }

        if cond {
            result *= i;
            i = i++;
        }
         */
        ConditionalExpression ifThenElse = Expression.IfThenElse(
            test: test,
            ifTrue: ifTrue,
            ifFalse: Expression.Break(label, result));

        LoopExpression loopExpression = Expression.Loop(body: ifThenElse, @break: label);

        BinaryExpression initI = Expression.Assign(left: i, right: Expression.Constant(iInitialVariable));
        BinaryExpression initResult = Expression.Assign(left: result, right: Expression.Constant(1));

        BlockExpression block = Expression.Block(
            new[] { result },
            initI, initResult,
            loopExpression
        );

        var l = Expression.Lambda<Func<T, T>>(block, i).Compile();

        var r = l.DynamicInvoke(default(T));

        return (T)r;
    }

    private static Expression wrap<T>(Expression<Func<T, T>> stepExp, ParameterExpression i)
    {
        Expression<Func<T, T>> bind = (x) => stepExp.Compile()(x);
        Expression wrapped = Expression.Invoke(bind, i);
        return wrapped;
    }

    private static Expression wrap<T>(Expression<Action<T>> stepExp, ParameterExpression i)
    {
        Expression<Action<T>> bind = (x) => stepExp.Compile()(x);
        Expression wrapped = Expression.Invoke(bind, i);
        return wrapped;
    }


    private static Expression wrap<T>(Expression<Predicate<T>> exp, ParameterExpression i)
    {
        Expression<Predicate<T>> bind = (x) => exp.Compile()(x);
        Expression wrapped = Expression.Invoke(bind, i);
        return wrapped;
    }

    private static Expression wrap<T>(Expression<Func<T, T, T>> exp,
        ParameterExpression arg0,
        ParameterExpression arg1)
    {
        Expression<Func<T, T, T>> bind = (i, result) => exp.Compile()(i, result);
        Expression wrapped = Expression.Invoke(bind, arg0, arg1);
        return wrapped;
    }



    public static Expression writeLineExp()
    {
        var arg = Expression.Parameter(typeof(object), "i");
        var writeln = Expression.Call(typeof(Console).GetMethod("WriteLine",
          new[] {
              typeof(object)
          }), arg);

        return writeln;
    }


}