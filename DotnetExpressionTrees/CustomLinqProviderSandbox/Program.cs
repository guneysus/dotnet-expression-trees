namespace CustomLinqProviderSandbox;

internal static class Program
{
    static void Main(string[] args)
    {
        // Enumerable.Range(1,10).AsQueryable().DistinctBy()

        Db.Where(x => x.Id == 10 && x != null && x.Name.Contains("foo") && x.Age >= 18 || x.Id < 0).Visit();
        return;

        Db.Query(x => x.Id).Visit();
        Db.Query(x => x.Id == 10).Visit();
        Db.Query(x => x.Id is int).Visit();
        Db.Query(x => x.Name != "Ahmed").Visit();
        Db.Query(x => x.Age > 10).Visit();
        Db.Query(x => new int[1] { x.Id }).Visit();
        Db.Query(x => new object()).Visit();
        Db.Query(x => new Dictionary<int, string>()).Visit();
        Db.Query(x => x.GetTitle()).Visit();
        Db.Query<string>(x => default).Visit();

        Console.WriteLine("Hello, World!");
    }
}
