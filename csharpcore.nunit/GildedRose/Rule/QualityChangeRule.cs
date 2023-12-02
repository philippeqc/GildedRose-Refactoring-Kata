namespace GildedRoseKata.Rule;

public class QualityChangeRule : ItemRule
{
    private int _qualityRate;
    public QualityChangeRule(int qualityRate) =>
        _qualityRate = qualityRate;

    public void DoRule(Item item)
    {
        item.Quality += _qualityRate;
    }
}