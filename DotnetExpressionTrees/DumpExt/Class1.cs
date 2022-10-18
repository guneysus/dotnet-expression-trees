using System.Runtime.CompilerServices;

namespace DumpExt
{
    public static class ext
    {
        public static void Dump(this object v, [CallerMemberName] string member = default)
        {
            Console.WriteLine(member);
            Console.WriteLine(string.Join(string.Empty, Enumerable.Range(1, member.Length).Select(x => '=')));
            Console.WriteLine(v);
            Console.WriteLine();

        }
    }
}