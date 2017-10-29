using System.Linq;
using System.Threading.Tasks;

namespace TinyTest.UI
{
    class Program
    {
        static int Add(int x, int y) => x + y;

        static int Main(string[] args)
        {
            Test.Module("Simple Tests");

            Test.Case("Add(x, y) works", () =>
            {
                var result = Enumerable.Range(1, 100).Aggregate(Add);
                Test.Equal(result, 5050);
            });

            Test.CaseAsync("Async tests too!", async () =>
            {
                await Task.Delay(200);
                Test.Equal(1, 1);
            });

            return Test.Report();
        }
    }
}
