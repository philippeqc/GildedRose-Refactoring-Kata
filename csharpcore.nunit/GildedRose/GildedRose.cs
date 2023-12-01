using System;
using System.Collections.Generic;

namespace GildedRoseKata;

public class SellInDegradeRule {
    private Item m_item;
    public SellInDegradeRule(Item item) {m_item = item;}
    public void UpdateSellIn() {
        m_item.SellIn -= 1;
    }
}

public interface IItemType
{
    void UpdateQuality();
}

public class Sulfuras : IItemType
{
    private Item m_item;
    public Sulfuras(Item item) { m_item = item; }
    public void UpdateQuality() { }
}

public class AgedBrie : IItemType
{
    private Item m_item;
    private SellInDegradeRule m_sellInDegradeRule;
    public AgedBrie(Item item) { m_item = item; m_sellInDegradeRule = new SellInDegradeRule(m_item);}
    public void UpdateQuality()
    {
        m_sellInDegradeRule.UpdateSellIn();
        if (m_item.Quality < 50)
        {
            m_item.Quality += 1;
        }
    }
}

public class BackstagePass : IItemType
{
    private Item m_item;
    private SellInDegradeRule m_sellInDegradeRule;
    public BackstagePass(Item item) { m_item = item; m_sellInDegradeRule = new SellInDegradeRule(m_item);}
    public void UpdateQuality()
    {
        m_sellInDegradeRule.UpdateSellIn();
        if (m_item.SellIn <= 0)
        {
            m_item.Quality = 0;
        }
        else if (m_item.SellIn <= 5)
        {
            m_item.Quality += 3;
        }
        else if (m_item.SellIn <= 10)
        {
            m_item.Quality += 2;
        }
        else
        {
            m_item.Quality += 1;
        }
    }
}

public class Conjured : IItemType
{
    private Item m_item;
    private SellInDegradeRule m_sellInDegradeRule;
    public Conjured(Item item) { m_item = item; m_sellInDegradeRule = new SellInDegradeRule(m_item);}
    public void UpdateQuality()
    {
        m_sellInDegradeRule.UpdateSellIn();
        if (m_item.SellIn <= 0)
        {
            m_item.Quality = Math.Max(m_item.Quality - 4, 0);
        }
        else
        {
            m_item.Quality = Math.Max(m_item.Quality - 2, 0);
        }
    }
}

public class Nonspecific : IItemType
{
    private Item m_item;
    private SellInDegradeRule m_sellInDegradeRule;
    public Nonspecific(Item item) { m_item = item; m_sellInDegradeRule = new SellInDegradeRule(m_item);}
    public void UpdateQuality()
    {
        m_sellInDegradeRule.UpdateSellIn();
        if (m_item.SellIn <= 0)
        {
            m_item.Quality = Math.Max(m_item.Quality - 2, 0);
        }
        else
        {
            m_item.Quality = Math.Max(m_item.Quality - 1, 0);
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
        IItemType itemType;

        if (item.Name == "Sulfuras, Hand of Ragnaros")
        {
            itemType = new Sulfuras(item);
            itemType.UpdateQuality();
            return;
        }

        if (item.Name == "Aged Brie")
        {
            itemType = new AgedBrie(item);
        }
        else if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
        {
            itemType = new BackstagePass(item);
        }
        else if (item.Name == "Conjured")
        {
            itemType = new Conjured(item);
        }
        else
        {
            itemType = new Nonspecific(item);
        }
        itemType.UpdateQuality();
    }
}