# TestCase

Rouler le même test plusieurs fois avec des données différentes.

Sur `QualityIsNeverMoreThanFifty_AsATestCase`

```csharp
[TestCase("Some item", 75, 74)]
[TestCase("Aged Brie", 75, 75)]
public void QualityIsNeverMoreThanFifty_AsATestCase(string name, int startQuality, int endQuality)
```

# Alternative avec `ExpectedResult`

Rend le résultat attendu un attribut distinct. Le test doit retourner la valeur finale.

```csharp
[TestCase("Some item", 75, ExpectedResult = 74)]
[TestCase("Aged Brie", 75, ExpectedResult = 75)]
public int QualityIsNeverMoreThanFifty_AsATestCaseWithExpectedResult(string name, int startQuality)
{
    ...

    // Assert
    return item.Quality;
```

```csharp
```

```csharp
```

```csharp
```
