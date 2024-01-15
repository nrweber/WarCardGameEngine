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
        testDeck.PushBottom(new Card(Suits.Club, Values.Six));
        testDeck.PushBottom(new Card(Suits.Club, Values.Seven));

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
        Assert.Equal(WarCardGame.GameState.WarWaitingForPlayerTwo, game.State);
    }
    
    [Fact]
    public void WarWaitingOnBothPlayers_PlayerTwoFlipCard_TwoMoreCardsAreFlipped_StateGoesToWarWaitingForPlayerOne()
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
        
        game.PlayerTwoFlipCard();
        Assert.Equal(3, game.PlayerTwoPlayedCards.Count);
        Assert.Equal(WarCardGame.GameState.WarWaitingForPlayerOne, game.State);
    }

    [Fact]
    public void WarWaitingOnPlayerOne_PlayerOneFlipCardCanStillBeCalled()
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

        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.WarWaitingForPlayerOne, game.State);
        
        game.PlayerOneFlipCard();
        Assert.Equal(3, game.PlayerOnePlayedCards.Count);
    }

    [Fact]
    public void WarWaitingOnPlayerTwo_PlayerTwoFlipCardCanStillBeCalled()
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

        game.PlayerOneFlipCard();
        Assert.Equal(WarCardGame.GameState.WarWaitingForPlayerTwo, game.State);
        
        game.PlayerTwoFlipCard();
        Assert.Equal(3, game.PlayerTwoPlayedCards.Count);
    }

    [Fact]
    public void AfterWar_PlayerOneWinsRound_PlayerOneFlipCard_and_PlayerTwoFlipCard_DoNothing_AlsoResetRoundGivesAllCardsToPlayerTwo()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Heart, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ten));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Three));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Four));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Five));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();

        Assert.Equal(WarCardGame.GameState.PlayerOneWinsRound, game.State);
        Assert.Equal(3, game.PlayerOnePlayedCards.Count);
        Assert.Equal(3, game.PlayerTwoPlayedCards.Count);

        // Try to flip another card
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        Assert.Equal(WarCardGame.GameState.PlayerOneWinsRound, game.State);
        Assert.Equal(3, game.PlayerOnePlayedCards.Count);
        Assert.Equal(3, game.PlayerTwoPlayedCards.Count);
        
        // Give cards to player one
        game.ResetRound();
        Assert.Equal(0, game.PlayerOnePlayedCards.Count);
        Assert.Equal(0, game.PlayerTwoPlayedCards.Count);
        Assert.Equal(7, game.PlayerOneDeckSize);
        Assert.Equal(1, game.PlayerTwoDeckSize);
    }
    
    [Fact]
    public void AfterWar_PlayerTwoWinsRound_PlayerOneFlipCard_and_PlayerTwoFlipCard_DoNothing_AlsoResetRoundGivesAllCardsToPlayerTwo()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Heart, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Three));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ten));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Four));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Five));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();

        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsRound, game.State);
        Assert.Equal(3, game.PlayerOnePlayedCards.Count);
        Assert.Equal(3, game.PlayerTwoPlayedCards.Count);

        // Try to flip another card
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsRound, game.State);
        Assert.Equal(3, game.PlayerOnePlayedCards.Count);
        Assert.Equal(3, game.PlayerTwoPlayedCards.Count);

        // Give cards to player Two
        game.ResetRound();
        Assert.Equal(0, game.PlayerOnePlayedCards.Count);
        Assert.Equal(0, game.PlayerTwoPlayedCards.Count);
        Assert.Equal(1, game.PlayerOneDeckSize);
        Assert.Equal(7, game.PlayerTwoDeckSize);
    }
    
    [Fact]
    public void NeverEndingWar()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Three));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.Four));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Four));
        testDeck.PushBottom(new Card(Suits.Club, Values.Five));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Five));
        testDeck.PushBottom(new Card(Suits.Club, Values.Six));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Six));
        testDeck.PushBottom(new Card(Suits.Club, Values.Seven));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Seven));
        testDeck.PushBottom(new Card(Suits.Club, Values.Eight));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Eight));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        Assert.Equal(WarCardGame.GameState.WarWaitingForBothPlayers, game.State);
        Assert.Equal(1, game.PlayerOnePlayedCards.Count);
        Assert.Equal(1, game.PlayerTwoPlayedCards.Count);
        
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();

        Assert.Equal(WarCardGame.GameState.WarWaitingForBothPlayers, game.State);
        Assert.Equal(3, game.PlayerOnePlayedCards.Count);
        Assert.Equal(3, game.PlayerTwoPlayedCards.Count);
        
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();

        Assert.Equal(WarCardGame.GameState.WarWaitingForBothPlayers, game.State);
        Assert.Equal(5, game.PlayerOnePlayedCards.Count);
        Assert.Equal(5, game.PlayerTwoPlayedCards.Count);
    }

    [Fact]
    public void PlayerOneWinsAfterNormalRoundBecauasePlayerTwoIsOutOfCards()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        Assert.Equal(WarCardGame.GameState.PlayerOneWinsGame, game.State);
    }
    
    [Fact]
    public void PlayerTwoWinsAfterNormalRoundBecauasePlayerOneIsOutOfCards()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Spade, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Three));
        testDeck.PushBottom(new Card(Suits.Club, Values.King));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsRound, game.State);
        game.ResetRound();
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsGame, game.State);
    }

    [Fact]
    public void PlayerOneWinsAfterWarBecauasePlayerTwoIsOutOfCards()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.King));
        testDeck.PushBottom(new Card(Suits.Spade, Values.King));
        testDeck.PushBottom(new Card(Suits.Club, Values.Queen));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Two));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.WarWaitingForBothPlayers, game.State);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        Assert.Equal(WarCardGame.GameState.PlayerOneWinsGame, game.State);
    }

    [Fact]
    public void PlayerTwoWinsAfterWarBecauasePlayerOneIsOutOfCards()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.King));
        testDeck.PushBottom(new Card(Suits.Club, Values.King));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Queen));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.WarWaitingForBothPlayers, game.State);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        
        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsGame, game.State);
    }

    [Fact]
    public void PlayerOneWinsByWarRuleBeforeWarBecauasePlayerTwoIsDownToZeroCards()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerOneWinsRound, game.State);
        game.ResetRound();
        
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerOneWinsGameByWarRule, game.State);
        Assert.Equal(0, game.PlayerTwoDeckSize);
    }
    
    [Fact]
    public void PlayerOneWinsByWarRuleBeforeWarBecauasePlayerTwoIsDownToOneCard()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Two));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Two));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerOneWinsRound, game.State);
        game.ResetRound();
        
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerOneWinsGameByWarRule, game.State);
        Assert.Equal(1, game.PlayerTwoDeckSize);
    }


    [Fact]
    public void PlayerTwoWinsByWarRuleBeforeWarBecauasePlayerOneIsDownToZeroCards()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Two));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsRound, game.State);
        game.ResetRound();
        
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsGameByWarRule, game.State);
        Assert.Equal(0, game.PlayerOneDeckSize);
    }
    
    [Fact]
    public void PlayerTwoWinsByWarRuleBeforeWarBecauasePlayerOneIsDownToOneCard()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Two));
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Heart, Values.Ace));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsRound, game.State);
        game.ResetRound();
        
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.PlayerTwoWinsGameByWarRule, game.State);
        Assert.Equal(1, game.PlayerOneDeckSize);
    }
    
    [Fact]
    public void GameDrwaByWarRuleBeforeWarBecauaseBothPlayersHaveZeroCards()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.DrawByWarRule, game.State);
        Assert.Equal(0, game.PlayerOneDeckSize);
        Assert.Equal(0, game.PlayerTwoDeckSize);
    }
    
    [Fact]
    public void GameDrwaByWarRuleBeforeWarBecauaseBothPlayersHaveOneCard()
    {
        StandardPokerDeck testDeck =  new(StartingStates.Empty);
        testDeck.PushBottom(new Card(Suits.Diamond, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Club, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Spade, Values.Ace));
        testDeck.PushBottom(new Card(Suits.Heart, Values.Ace));

        WarCardGame game = new(testDeck);
        game.PlayerOneFlipCard();
        game.PlayerTwoFlipCard();
        Assert.Equal(WarCardGame.GameState.DrawByWarRule, game.State);
        Assert.Equal(1, game.PlayerOneDeckSize);
        Assert.Equal(1, game.PlayerTwoDeckSize);
    }
}
