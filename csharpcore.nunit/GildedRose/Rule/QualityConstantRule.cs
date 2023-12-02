namespace GildedRoseKata.Rule;

public class QualityConstantRule : ItemRule
{
    private int _qualityConstant;
    private int _fromSellIn;
    
    public QualityConstantRule(int qualityConstant, int fromSellIn = int.MaxValue)
    {
        _qualityConstant = qualityConstant;
        _fromSellIn = fromSellIn;
    }
    public void DoRule(Item item)
    {
        if (item.SellIn <= _fromSellIn)
            item.Quality = _qualityConstant;
    }
}
