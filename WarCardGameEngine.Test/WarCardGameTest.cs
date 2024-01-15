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
    
    [Fact]
    public void ThrowsArgumentExceptionIfDeckContainsOneCard()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        Assert.Throws<ArgumentException>(() => new WarCardGame(testDeck));
    }
    
    [Fact]
    public void DefaultConstructor_EachPlayerHas26Cards_GameStateIsWaitingForBothPlayers()
    {
        WarCardGame game = new();

        Assert.Equal(26, game.PlayerOneDeckSize);
        Assert.Equal(26, game.PlayerTwoDeckSize);

        Assert.Equal(WarCardGame.GameState.WaitingForBothPlayers, game.State);
    }

    [Fact]
    public void PlayerOneFlipCard_CardIsPutIntoPlayList_GameStateIsWaitingOnPlayerTwo()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Heart, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();

        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Equal(new Card(Suits.Club, Values.Ace), game.PlayerOnePlayedCards[0]);
        Assert.Equal(WarCardGame.GameState.WaitingForPlayerTwo, game.State);
    }

    [Fact]
    public void PlayerOneFlipCard_SecondCallWhileWaitingForPlayerTwoDoesNothing()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Heart, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));

        WarCardGame game = new(testDeck);

        //First Call
        game.PlayerOneFlipCard();

        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Equal(new Card(Suits.Club, Values.Ace), game.PlayerOnePlayedCards[0]);
        Assert.Equal(WarCardGame.GameState.WaitingForPlayerTwo, game.State);
       
        //Second Call
        game.PlayerOneFlipCard();

        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Equal(WarCardGame.GameState.WaitingForPlayerTwo, game.State);
    }
    
    [Fact]
    public void PlayerTwoFlipCard_CardIsPutIntoPlayList_GameStateIsWaitingOnPlayerOne()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Heart, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));

        WarCardGame game = new(testDeck);
        game.PlayerTwoFlipCard();

        Assert.Single(game.PlayerTwoPlayedCards);
        Assert.Equal(new Card(Suits.Spade, Values.Ace), game.PlayerTwoPlayedCards[0]);
        Assert.Equal(WarCardGame.GameState.WaitingForPlayerOne, game.State);
    }

    [Fact]
    public void PlayerTwoFlipCard_SecondCallWhileWaitingForPlayerOneDoesNothing()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Heart, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));

        WarCardGame game = new(testDeck);

        // First Call
        game.PlayerTwoFlipCard();

        Assert.Single(game.PlayerTwoPlayedCards);
        Assert.Equal(new Card(Suits.Spade, Values.Ace), game.PlayerTwoPlayedCards[0]);
        Assert.Equal(WarCardGame.GameState.WaitingForPlayerOne, game.State);
        
        // Second Call
        game.PlayerTwoFlipCard();

        Assert.Single(game.PlayerTwoPlayedCards);
        Assert.Equal(WarCardGame.GameState.WaitingForPlayerOne, game.State);
    }
 
    [Fact]
    public void PlayerOneFlipCard_CanBeCalledAfterPlayerTwoFlipCard()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));

        WarCardGame game = new(testDeck);
        game.PlayerTwoFlipCard();

        game.PlayerOneFlipCard();

        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Equal(new Card(Suits.Club, Values.Ace), game.PlayerOnePlayedCards[0]);
    }

    [Fact]
    public void PlayerTwoFlipCard_CanBeCalledAfterPlayerOneFlipCard()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();

        game.PlayerTwoFlipCard();

        Assert.Single(game.PlayerTwoPlayedCards);
        Assert.Equal(new Card(Suits.Club, Values.Two), game.PlayerTwoPlayedCards[0]);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AfterBothPlay_PlayerOneHigher_GameStateIsPlayerOneWinsRound(bool oneFirst)
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));

        WarCardGame game = new(testDeck);
        if(oneFirst)
        {
            game.PlayerOneFlipCard();
            game.PlayerTwoFlipCard();
        }
        else
        {
            game.PlayerTwoFlipCard();
            game.PlayerOneFlipCard();
        }

        Assert.Equal(WarCardGame.GameState.PlayerOneWinsRound, game.State);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AfterBothPlay_PlayerTwoHigher_GameStateIsPlayerOneWinsRound(bool oneFirst)
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));

        WarCardGame game = new(testDeck);
        if(oneFirst)
        {
            game.PlayerOneFlipCard();
            game.PlayerTwoFlipCard();
        }
        else
        {
            game.PlayerTwoFlipCard();
            game.PlayerOneFlipCard();
        }

        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsRound, game.State);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AfterBothPlay_CardsMatch_GameStateIsWar(bool oneFirst)
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));

        WarCardGame game = new(testDeck);
        if(oneFirst)
        {
            game.PlayerOneFlipCard();
            game.PlayerTwoFlipCard();
        }
        else
        {
            game.PlayerTwoFlipCard();
            game.PlayerOneFlipCard();
        }

        Assert.Equal(WarCardGame.GameState.WarWaitingForBothPlayers, game.State);
    }

    [Fact]
    public void AfterPlayerOneWinsRound_PlayerOneFlipCard_and_PlayerTwoFlipCard_DoNothing()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();

        Assert.Equal(WarCardGame.GameState.PlayerOneWinsRound, game.State);
        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Single(game.PlayerTwoPlayedCards);

        // Try to flip another card
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        Assert.Equal(WarCardGame.GameState.PlayerOneWinsRound, game.State);
        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Single(game.PlayerTwoPlayedCards);
    }
    
    [Fact]
    public void AfterPlayerTwoWinsRound_PlayerOneFlipCard_and_PlayerTwoFlipCard_DoNothing()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();

        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsRound, game.State);
        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Single(game.PlayerTwoPlayedCards);

        // Try to flip another card
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsRound, game.State);
        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Single(game.PlayerTwoPlayedCards);
    }

    [Fact]
    public void AfterPlayerOneWinsRound_RestRound_MovesAllPlayedCardsToPlayerOne_StateBacktoWaitingForBothPlayers()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();

        Assert.Equal(WarCardGame.GameState.PlayerOneWinsRound, game.State);
        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Single(game.PlayerTwoPlayedCards);

        game.ResetRound();

        Assert.Equal(WarCardGame.GameState.WaitingForBothPlayers, game.State);
        Assert.Empty(game.PlayerOnePlayedCards);
        Assert.Empty(game.PlayerTwoPlayedCards);
        Assert.Equal(3, game.PlayerOneDeckSize);
        Assert.Equal(1, game.PlayerTwoDeckSize);
    }

    [Fact]
    public void AfterPlayerTwoWinsRound_RestRound_MovesAllPlayedCardsToPlayerTwo_StateBacktoWaitingForBothPlayers()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();

        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsRound, game.State);
        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Single(game.PlayerTwoPlayedCards);

        game.ResetRound();

        Assert.Equal(WarCardGame.GameState.WaitingForBothPlayers, game.State);
        Assert.Empty(game.PlayerOnePlayedCards);
        Assert.Empty(game.PlayerTwoPlayedCards);
        Assert.Equal(1, game.PlayerOneDeckSize);
        Assert.Equal(3, game.PlayerTwoDeckSize);
    }

    [Fact]
    public void WarWaitingOnBothPlayers_PlayerOneFlipCard_TwoMoreCardsAreFlipped_StateGoesToWarWaitingForPlayerTwo()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Heart, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Two));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Three));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Four));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Five));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();

        Assert.Single(game.PlayerOnePlayedCards);
        Assert.Single(game.PlayerTwoPlayedCards);
        Assert.Equal(WarCardGame.GameState.WarWaitingForBothPlayers, game.State);
        
        game.PlayerOneFlipCard();
        Assert.Equal(3, game.PlayerOnePlayedCards.Count);
    }
}
