using nic_weber.DeckOfCards;

namespace nic_weber.WarCardGameEngine.Test;

public class UnitTest1
{
    [Fact]
    public void ThrowsArgumentExceptionIfDeckContainsZeroCards()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        Assert.Throws<ArgumentException>(() => new WarCardGame(testDeck));
    }
}
