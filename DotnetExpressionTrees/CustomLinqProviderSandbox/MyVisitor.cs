using System.Diagnostics.CodeAnalysis;

public class MyVisitor : ExpressionVisitor
{
    public static ExpressionVisitor New() => new MyVisitor();

    protected override Expression VisitMember(MemberExpression node)
    {
        return base.VisitMember(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        return base.VisitConstant(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return base.VisitParameter(node);
    }

    [return: NotNullIfNotNull("node")]
    public override Expression? Visit(Expression? node)
    {
        return base.Visit(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        return base.VisitBinary(node);
    }

    protected override ElementInit VisitElementInit(ElementInit node)
    {
        return base.VisitElementInit(node);
    }

    protected override Expression VisitExtension(Expression node)
    {
        return base.VisitExtension(node);
    }

    protected override Expression VisitIndex(IndexExpression node)
    {
        return base.VisitIndex(node);
    }

    protected override Expression VisitDefault(DefaultExpression node)
    {
        return base.VisitDefault(node);
    }

    protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
    {
        return base.VisitMemberAssignment(node);
    }

    protected override MemberBinding VisitMemberBinding(MemberBinding node)
    {
        return base.VisitMemberBinding(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        return base.VisitMethodCall(node);
    }

    protected override Expression VisitNew(NewExpression node)
    {
        return base.VisitNew(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        return base.VisitUnary(node);
    }

    protected override Expression VisitMemberInit(MemberInitExpression node)
    {
        return base.VisitMemberInit(node);
    }

    protected override Expression VisitBlock(BlockExpression node)
    {
        return base.VisitBlock(node);
    }

    protected override Expression VisitConditional(ConditionalExpression node)
    {
        return base.VisitConditional(node);
    }

    protected override Expression VisitInvocation(InvocationExpression node)
    {
        return base.VisitInvocation(node);
    }

    protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
    {
        return base.VisitMemberListBinding(node);
    }

    protected override Expression VisitLoop(LoopExpression node)
    {
        return base.VisitLoop(node);
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        return base.VisitLambda(node);
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
    {
        return base.VisitMemberMemberBinding(node);
    }

    protected override Expression VisitListInit(ListInitExpression node)
    {
        return base.VisitListInit(node);
    }

    protected override Expression VisitNewArray(NewArrayExpression node)
    {
        return base.VisitNewArray(node);
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
        return base.VisitTypeBinary(node);
    }
}



