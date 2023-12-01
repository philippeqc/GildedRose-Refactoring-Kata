using System;
using System.Collections.Generic;

namespace GildedRoseKata;

public class Sulfuras
{
    private Item m_item;
    public Sulfuras(Item item) { m_item = item; }
    public void UpdateQuality() { }
}

public class AgedBrie
{
    private Item m_item;
    public AgedBrie(Item item) { m_item = item; }
    public void UpdateQuality()
    {
        if (m_item.Quality < 50)
        {
            m_item.Quality += 1;
        }
    }
}

public class GildedRose
{
    private readonly IList<Item> _items;

    public GildedRose(IList<Item> items)
    {
        _items = items;
    }

    public void UpdateQuality()
    {
        foreach (var item in _items)
        {
            UpdateItemQuality(item);
        }
    }

    public void UpdateItemQuality(Item item)
    {
        if (item.Name == "Sulfuras, Hand of Ragnaros")
        {
            Sulfuras sulfuras = new Sulfuras(item);
            sulfuras.UpdateQuality();
            return;
        }

        item.SellIn -= 1;

        if (item.Name == "Aged Brie")
        {
            AgedBrie agedBrie = new AgedBrie(item);
            agedBrie.UpdateQuality();
            return;
        }

        if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
        {
            if (item.SellIn <= 0)
            {
                item.Quality = 0;
            }
            else if (item.SellIn <= 5)
            {
                item.Quality += 3;
            }
            else if (item.SellIn <= 10)
            {
                item.Quality += 2;
            }
            else
            {
                item.Quality += 1;
            }
            return;
        }

        if(item.Name == "Conjured") {
            if (item.SellIn <= 0)
            {
                item.Quality = Math.Max(item.Quality - 4, 0);
            }
            else
            {
                item.Quality = Math.Max(item.Quality - 2, 0);
            }
            return;
        }

        if (item.SellIn <= 0)
        {
            item.Quality = Math.Max(item.Quality - 2, 0);
        }
        else
        {
            item.Quality = Math.Max(item.Quality - 1, 0);
        }
        return;
    }
}