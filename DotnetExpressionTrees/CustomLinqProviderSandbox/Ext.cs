public static class Ext
{
    public static void Visit(this Expression expression)
    {
        MyVisitor.New().Visit(expression);
    }
}


