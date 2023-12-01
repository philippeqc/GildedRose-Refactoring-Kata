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

# Reponse a Jonathan
Ca m'a bien chicoté son "Toi Philippe, qu'est-ce que tu aurais fait?".

Ajouté a GildedRoseTest
```csharp
protected Item GetSampleItem(string name = "foo", int sellIn = 10, int quality = 10)
{
    Item item = new Item { Name = name, SellIn = sellIn, Quality = quality };
    m_items.Add(item);

    return item;
}

protected IList<Item> m_items = new List<Item>();
protected GildedRose m_app;

[SetUp]
public void Setup()
{
    m_app = new GildedRose(m_items);
}
```

Permet de remplacer

```csharp
(GildedRose app, Item item) = Helper.GetAppWithSampleItem(name: name, quality: startQuality);
// Act
app.UpdateQuality();
```

par
```csharp
Item item = GetSampleItem(name: "Sulfuras");
// Act
m_app.UpdateQuality();
```
Notez `app` en `m_app`.
```csharp
```

```csharp
```

```csharp
```
