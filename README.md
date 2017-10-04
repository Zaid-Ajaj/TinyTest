# TinyTest
Tiny single file test "framework" for small projects because you don't always need full fledged testing frameworks.

### Usage
 - Create a console project (the test runner)
 - Copy the file [Test.cs](https://github.com/Zaid-Ajaj/TinyTest/blob/master/Test.cs) to the project
 - Start writing tests
 
 ```csharp
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
 ```
