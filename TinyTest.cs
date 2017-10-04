using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TinyTest
{
    public enum TestResult
    {
        Succeeded, Failed, Errored
    }

    public class TestDefinition
    {
        public string Name { get; set; }
        public TestResult Result { get; set; }
        public long RunTime { get; set; }
        public Exception Exception { get; set; } = null;
    }

    public class TestModule
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public List<TestDefinition> Tests { get; set; }
    }

    public interface ITestReporter
    {
        void Report(IEnumerable<TestModule> modules); 
    }


    public class ConsoleReporter : ITestReporter
    {
        private void LogTests(IEnumerable<TestDefinition> tests, bool indent = false)
        {
            foreach (var test in tests)
            {
                var indentation = indent ? "    " : "";
                if (test.Result == TestResult.Succeeded)
                {
                    SuccessLog($"{test.Name} | Passed in {test.RunTime} ms");
                }

                if (test.Result == TestResult.Errored)
                {
                    var ex = test.Exception;
                    ErrorLog($"{test.Name} | Errored in {test.RunTime} ms");
                    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                }

                if (test.Result == TestResult.Failed)
                {
                    FailLog($"{test.Name} | Failed in {test.RunTime} ms");
                }
            }
        }

        private static void ErrorLog(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static void SuccessLog(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static void FailLog(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public void Report(IEnumerable<TestModule> modules)
        {
            var defaultOnly = modules.Count() == 1 && modules.First().IsDefault;

            if (defaultOnly)
            {
                var defaultModule = modules.First();
                LogTests(defaultModule.Tests);
                return;
            }

            foreach (var module in modules)
            {
                Console.WriteLine($"Module: {module.Name}");
                LogTests(module.Tests, true);
                Console.WriteLine();
            }
        }
    }

    public static class Test
    {
        class TestFailureException : Exception
        {
            public TestFailureException() { }
            public TestFailureException(string msg) : base(msg)
            {

            }
        }

        public static void Module(string name)
        {
            if (testModules.Count == 1 && testModules[0].IsDefault)
            {
                testModules[0] = new TestModule
                {
                    Name = name,
                    Tests = testModules[0].Tests,
                    IsDefault = false
                };
            }
            else
            {
                testModules.Add(new TestModule
                {
                    Name = name,
                    Tests = new List<TestDefinition>(),
                    IsDefault = false
                });

            }
        }

        


        static List<TestModule> testModules = new List<TestModule>()
        {
            new TestModule
            {
                Name = "Default",
                IsDefault = true,
                Tests = new List<TestDefinition>()
            }
        };

        private static Action<string, Exception> onError = (name, ex) => 
        {

        };

        public static void ReportUsing(ITestReporter reporter)
        {
            reporter.Report(testModules);
        }

        public static void Report()
        {
            ReportUsing(new ConsoleReporter());
        }

        public static void Fail(string msg = "")
        {
            if (!string.IsNullOrEmpty(msg))
            {
                throw new TestFailureException(msg);
            }

            throw new TestFailureException();
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
            var testResult = new TestDefinition();
            testResult.Name = name;
            var stopwatch = Stopwatch.StartNew();
            try
            {
                handler();
            }
            catch(TestFailureException)
            {
                testResult.Result = TestResult.Failed;
            }
            catch(Exception ex)
            {
                testResult.Result = TestResult.Errored;
                testResult.Exception = ex;
                onError(name, ex);
            }
            finally
            {
                stopwatch.Stop();
                testResult.RunTime = stopwatch.ElapsedMilliseconds;
            }

            var lastModule = testModules.Count - 1;
            testModules[lastModule].Tests.Add(testResult);
        }

        public static Task CaseAsync(string name, Func<Task> handler)
        {
            var testResult = new TestDefinition();
            testResult.Name = name;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var task = handler();
                task.Wait();
                testResult.Result = TestResult.Succeeded;
            }
            catch (TestFailureException)
            {
                testResult.Result = TestResult.Failed;
            }
            catch (Exception ex)
            {
                testResult.Result = TestResult.Errored;
                testResult.Exception = ex;
                onError(name, ex);
            }
            finally
            {
                stopwatch.Stop();
                testResult.RunTime = stopwatch.ElapsedMilliseconds;
            }

            var lastModule = testModules.Count - 1;
            testModules[lastModule].Tests.Add(testResult);

            return Task.Run(() => { });
        }
    }
}
