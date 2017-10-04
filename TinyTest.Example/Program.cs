using System.Threading.Tasks;
using TinyTest;

namespace TinyTest.UI
{
    class Program
    {
        static int Add(int x, int y) => x + y;

        static void Main(string[] args)
        {
            Test.Case("Add(x, y) works", () =>
            {
                var result = Add(1, Add(2, Add(3, Add(4, 5))));
                Test.Equal(result, 15);
            });

            Test.CaseAsync("Async tests too!", async () =>
            {
                await Task.Delay(1000);
                Test.Equal(1, 1);
            });
        }
    }
}
