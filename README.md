# TinyTest
Tiny single file test "framework" for small projects because you don't always need full fledged testing frameworks.

### Usage
 - Create a console project (the test runner)
 - Copy the file [TinyTest.cs](https://github.com/Zaid-Ajaj/TinyTest/blob/master/TinyTest.cs) to the project
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

    // put at the end, call once
    Test.Report();
}
```
You can group tests using modules:
```csharp
Test.Module("Math tests");
Test.Case("Case 1", () => ... );
Test.Case("Case 2", () => ... );

Test.Module("Database tests");
Test.Case("Case 1", () => ... );
Test.Case("Case 2", () => ... );

Test.Report();
``` 

