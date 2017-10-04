using System;
using System.Threading.Tasks;

namespace TinyTest
{
    public class TestDefinition
    {
        public string Name { get; set; }
        public bool Failed { get; set; }
        public Exception Exception { get; set; } = null;
    }


    public static class Test
    {
        private static Action<string, Exception> onError = (name, ex) =>
        {
            Error($"Failed: {name}");
            Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        };


         
        private static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static void Fail(string msg = "")
        {
            if (!string.IsNullOrEmpty(msg))
            {
                throw new Exception(msg);
            }

            throw new Exception();
        }

        public static void Equal<T>(T x, T y) where T : IEquatable<T>
        {
            if (!x.Equals(y)) 
            {
                Fail();
            } 
        }


        public static void OnError(Action<string, Exception> handler)
        {
            if (handler != null)
            {
                onError = handler;
            }
        }

        public static void Case(string name, Action handler)
        {
            try
            {
                handler();
                Success($"Success: {name}");
            }
            catch(Exception ex)
            {
                onError(name, ex);
            }
        }

        public static Task CaseAsync(string name, Func<Task> handler)
        {
            try
            {
                var task = handler();
                task.Wait();
                Success($"Success: {name}");
                return task;
            }
            catch (Exception ex)
            {
                onError(name, ex);
                return Task.Run(() => { });
            }
        }
    }
}
