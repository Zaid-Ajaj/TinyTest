using System.Threading.Tasks;

namespace TinyTest.UI
{
    class Program
    {
        static int Add(int x, int y) => x + y;

        static int Main(string[] args)
        {
            Test.Module("Sample tests");

            Test.Case("Add(x, y) works", () =>
            {
                var result = Add(1, Add(2, 3));
                Test.Equal(result, 6);
            });

            Test.Case("Arrays equal", () =>
            {
                int[] xs = { 1, 2, 3, 4, 5 };
                int[] ys = { 1, 2, 3, 4 };

                Test.ArraysEqual(xs, ys);
            });

            Test.CaseAsync("Async tests too!", async () =>
            {
                await Task.Delay(1000);
                Test.Equal(1, 1);
            });

            return Test.Report();
        }
    }
}
