namespace GildedRoseKata.Rule;

public class QualityConstantRule : ItemRule
{
    private int _qualityConstant;

    public QualityConstantRule(int qualityConstant)
    {
        _qualityConstant = qualityConstant;
    }
    public void DoRule(Item item)
    {
        item.Quality = _qualityConstant;
    }
}
