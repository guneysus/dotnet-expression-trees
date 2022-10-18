using Xunit.Abstractions;

namespace FluentExpressionTrees.Tests;

public class UnitTest
{
    public UnitTest(ITestOutputHelper output)
    {
        Output = output;
    }

    public ITestOutputHelper Output { get; }

    //[Fact]
    public void Test1()
    {
        var result = FluentExpressionBuilder
            .New()
            .Parameter<int>("a")
            .Parameter<int>("b")
            .Binary(ExpressionType.Multiply, "a", "b")
            .Build()
            .DynamicInvoke(3, 5);

        Assert.Equal(expected: 8, actual: result);
    }

}