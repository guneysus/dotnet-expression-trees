public static class Db
{
    public static Expression<Func<Student, TResult>> Query<TResult>(Expression<Func<Student, TResult>> exp)
    {
        return exp;
    }

    public static Expression<Func<Student, bool>> Where(Expression<Func<Student, bool>> predicate)
    {
        return predicate;
    }
}


