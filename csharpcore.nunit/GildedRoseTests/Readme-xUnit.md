Literal and constant (expected value)  should be first argument on Assert.Equal, AssertNotEqual, Assert.StrictEqual, or Assert.NotStrictEqual

Assert.Equal, AssertNotEqual, Assert.StrictEqual, or Assert.NotStrictEqual
Assert.NotNull();

Assert.True(str.Contains(word)) => Assert.Contains(word, str)
Assert.False(str.Contains(word)) => Assert.DoesNotContain(word, str)
Assert.True(str.StartsWith(word)) => Assert.StartsWith(word, str)
Assert.True(str.EndsWith(word)) => Assert.EndsWith(word, str)
Assert.True(string.Equals("foo bar baz", result)); => Assert.Equal("foo bar baz", result);

## collection is empty
https://xunit.net/xunit.analyzers/rules/xUnit2011
```c#
var result = new[] { 1 };
// Assert.Collection(result);
Assert.Empty(result);
// or
Assert.Collection(
    result,
    value => Assert.Equal(1, value)
);
```

## value exists in a collection
```c#
var result = new[] { "Hello" };
// Assert.True(result.Any(value => value.Length == 5));
Assert.Contains(result, value => value.Length == 5);
```

## collection size (0,1, not empty)
Use Assert.Empty, Assert.NotEmpty, or Assert.Single
```c#
var result = new[] { "Hello" };
// Assert.Equal(1, result.Count());
Assert.Single(result);
```

Assert.IsType<string>(result)
Assert.IsNotType
Assert.IsAssignableFrom

// Match regular expression
Assert.Matches("foo (.*?) baz", result);

Assert.Same 
Assert.NotSame

Dont use
`Assert.Equals`

# Theory
```c#
public class Calculator
{
    public int Add(int value1, int value2)
    {
        return value1 + value2;
    }
}
```

```c#
[Theory]
[InlineData(1, 2, 3)]
[InlineData(-4, -6, -10)]
[InlineData(-2, 2, 0)]
[InlineData(int.MinValue, -1, int.MaxValue)]
public void CanAddTheory(int value1, int value2, int expected)
{
    var calculator = new Calculator();

    var result = calculator.Add(value1, value2);

    Assert.Equal(expected, result);
}
```

# Using a dedicated data class with [ClassData]
```c#
[Theory]
[ClassData(typeof(CalculatorTestData))]
public void CanAddTheoryClassData(int value1, int value2, int expected)
{
    ...
}

public class CalculatorTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 1, 2, 3 };
        yield return new object[] { -4, -6, -10 };
        yield return new object[] { -2, 2, 0 };
        yield return new object[] { int.MinValue, -1, int.MaxValue };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

# Using generator properties with the [MemberData] properties
```c#
public class CalculatorTests
{
    [Theory]
    [MemberData(nameof(Data))]
    public void CanAddTheoryMemberDataProperty(int value1, int value2, int expected)
    {
        ...
    }

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { 1, 2, 3 },
            new object[] { -4, -6, -10 },
            new object[] { -2, 2, 0 },
            new object[] { int.MinValue, -1, int.MaxValue },
        };
}
```

# Loading data from a method on the test class

```c#
public class CalculatorTests
{
    [Theory]
    [MemberData(nameof(GetData), parameters: 3)]
    public void CanAddTheoryMemberDataMethod(int value1, int value2, int expected)
    {
        ...
    }

    public static IEnumerable<object[]> GetData(int numTests)
    {
        var allData = new List<object[]>
        {
            new object[] { 1, 2, 3 },
            new object[] { -4, -6, -10 },
            new object[] { -2, 2, 0 },
            new object[] { int.MinValue, -1, int.MaxValue },
        };

        return allData.Take(numTests);
    }
}
```

# Loading data from a property or method on a different class
```c#
public class CalculatorTests
{
    [Theory]
    [MemberData(nameof(CalculatorData.Data), MemberType= typeof(CalculatorData))]
    public void CanAddTheoryMemberDataMethod(int value1, int value2, int expected)
    {
        ...
    }
}

public class CalculatorData
{
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { 1, 2, 3 },
            new object[] { -4, -6, -10 },
            new object[] { -2, 2, 0 },
            new object[] { int.MinValue, -1, int.MaxValue },
        };
}
```



# Skip

## Base
use either
- `[Fact (Skip = "specific reason")]`
- `[Theory (Skip = "specific reason")]`


## Skip based on platform detection

### Option 1 - Most intrusive, compile time platform detection

In the VS Solution, define another configuration that defines a precompiler flag MONOWIN (just so that it's explicitly a flag the says that it is for code compiled on Windows for use on Mono).

Then define an attribute that will make the test ignored when compiled for Mono:

```c#
public class IgnoreOnMonoFactAttribute : FactAttribute {
#if MONOWIN
    public IgnoreOnMonoFactAttribute() {
        Skip = "Ignored on Mono";
    }
#endif
}
```

### Option 2 - somewhat intrusive - runtime platform detection
Here is a similar solution to option1, except no separate configuration is required:

```c#
public class IgnoreOnMonoFactAttribute : FactAttribute {
    public IgnoreOnMonoFactAttribute() {
        if(IsRunningOnMono()) {
            Skip = "Ignored on Mono";
        }
    }
    /// <summary>
    /// Determine if runtime is Mono.
    /// Taken from http://stackoverflow.com/questions/721161
    /// </summary>
    /// <returns>True if being executed in Mono, false otherwise.</returns>
    public static bool IsRunningOnMono() {
        return Type.GetType("Mono.Runtime") != null;
    }
}
```

Note 1
xUnit runner will run a method twice if it is marked with [Fact] and [IgnoreOnMonoFact]. (CodeRush doesn't do that, in this case I assume xUnit is correct). This means that any tests methods must have [Fact] replaced with [IgnoreOnMonoFact]

Note 2
CodeRush test runner still ran the [IgnoreOnMonoFact] test, but it did ignore the [Fact(Skip="reason")] test. I assume it is due to CodeRush reflecting xUnit and not actually running it with the aid of xUnit libraries. This works fine with xUnit runner.

## SkippableFact

Add Nuget Package SkippableFact (https://www.nuget.org/packages/Xunit.SkippableFact/)

```c#
[SkippableFact]
public void SomeTestForWindowsOnly()
{
    Skip.IfNot(Environment.IsWindows);

    // Test Windows only functionality.
}

[SkippableTheory]
```

Skip.IfNot(Environment.IsWindows);
Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

# Trait

```c#
[Fact, Trait("type","unit")]
public void MyUnitTest(){
  // given 
  // when
  // then
}

[Fact, Trait("type","http")]
public void MyHttpIntegrationTest(){
  // given 
  // when do things over HTTP
  // then
}
```
usage `dotnet test --filter type=unit`