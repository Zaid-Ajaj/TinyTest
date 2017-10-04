using System.Threading.Tasks;

namespace TinyTest.UI
{
    class Program
    {
        static int Add(int x, int y) => x + y;

        static void Main(string[] args)
        {
            Test.Case("Add(x, y) works", () =>
            {
                var result = Add(1, Add(2, 3));
                Test.Equal(result, 6);
            });

            Test.CaseAsync("Async tests too!", async () =>
            {
                await Task.Delay(1000);
                Test.Equal(1, 1);
            });

            Test.Report();
        }
    }
}
