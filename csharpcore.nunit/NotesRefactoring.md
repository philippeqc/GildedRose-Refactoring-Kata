# Remove index
Work on `item` not `_items[i]`

```csharp
for (var i = 0; i < _items.Count; i++)
{
    if (item.Name != "Aged Brie" && item.Name != "Backstage passes to a TAFKAL80ETC concert")
```

# Work on single item
extract to new method `UpdateItemQuality` 