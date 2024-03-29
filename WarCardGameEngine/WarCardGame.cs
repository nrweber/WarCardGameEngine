﻿using nic_weber.DeckOfCards;

namespace nic_weber.WarCardGameEngine;

/// <summary>
/// Two person card game. Each player will place one card
/// from their stack on the table. The player with the highest
/// card, picks up both cards and puts them on the bottom of 
/// their deck.
/// </summary>
public class WarCardGame
{
    public enum GameState
    {
        WaitingForBothPlayers,
        WaitingForPlayerOne,
        WaitingForPlayerTwo,
        PlayerOneWinsRound,
        PlayerTwoWinsRound,
        WarWaitingForBothPlayers,
        WarWaitingForPlayerOne,
        WarWaitingForPlayerTwo,
        PlayerOneWinsGame,
        PlayerOneWinsGameByWarRule,
        PlayerTwoWinsGame,
        PlayerTwoWinsGameByWarRule,
        DrawByWarRule,
    };



    /// <summary>
    /// Current state of the game
    /// </summary>
    public GameState State { get; private set; } = GameState.WaitingForBothPlayers;

    /// <summary>
    /// Number of cards left in player one's deck
    /// </summary>
    public int PlayerOneDeckSize 
    { 
        get 
        {
            return _playerOneDeck.CardsInDeck;
        }
    }

    /// <summary>
    /// Cards played by player one
    /// </summary>
    public IReadOnlyList<Card> PlayerOnePlayedCards { get => _playerOnePlayedCards.AsReadOnly(); }
    
    /// <summary>
    /// Number of cards left in player two's deck
    /// </summary>
    public int PlayerTwoDeckSize 
    { 
        get 
        {
            return _playerTwoDeck.CardsInDeck;
        }
    }

    /// <summary>
    /// Cards played by player two
    /// </summary>
    public IReadOnlyList<Card> PlayerTwoPlayedCards { get => _playerTwoPlayedCards.AsReadOnly(); }
    


    private StandardPokerDeck _playerOneDeck = new(StartingStates.Empty);
    private StandardPokerDeck _playerTwoDeck = new(StartingStates.Empty);
    private List<Card> _playerOnePlayedCards = new();
    private List<Card> _playerTwoPlayedCards = new();



    /// <summary>
    /// Start a new game of War with a standard 52 card deck.
    /// The deck is evenly devided between each player
    /// one card at a time.
    /// </summary>
    public WarCardGame()
    {
        var deck = new StandardPokerDeck(StartingStates.Standard52);
        deck.Shuffle();
        DealCards(deck);
    }


    /// <summary>
    /// Start a new game of War with the given deck.
    /// The deck is evenly divided between each player
    /// one card at a time. If there is an odd amount 
    /// of cards, the first player will receive an extra
    /// card.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if given deck does not contain enough cards.
    /// </exception>
    public WarCardGame(StandardPokerDeck deck)
    {
        if(deck.CardsInDeck <= 1)
            throw new ArgumentException("Deck needs to have at least 2 cards.");

        DealCards(deck);
    }


    /// <summary>
    /// Place top card from player one's deck on the played card list
    /// </summary>
    public void PlayerOneFlipCard()
    {
        switch(State)
        {
            case GameState.WaitingForBothPlayers:
                playerOnePlayCard();
                State = GameState.WaitingForPlayerTwo;
                break;
            case GameState.WaitingForPlayerOne:
                playerOnePlayCard();
                checkRound();
                break;
            case GameState.WarWaitingForBothPlayers:
                playerOnePlayCard();
                playerOnePlayCard();
                State = GameState.WarWaitingForPlayerTwo;
                break;
            case GameState.WarWaitingForPlayerOne:
                playerOnePlayCard();
                playerOnePlayCard();
                checkRound();
                break;


        }
    }


    /// <summary>
    /// Place top card from player two's deck on the played card list
    /// </summary>
    public void PlayerTwoFlipCard()
    {
        switch(State)
        {
            case GameState.WaitingForBothPlayers:
                playerTwoPlayCard();
                State = GameState.WaitingForPlayerOne;
                break;
            case GameState.WaitingForPlayerTwo:
                playerTwoPlayCard();
                checkRound();
                break;
            case GameState.WarWaitingForBothPlayers:
                playerTwoPlayCard();
                playerTwoPlayCard();
                State = GameState.WarWaitingForPlayerOne;
                break;
            case GameState.WarWaitingForPlayerTwo:
                playerTwoPlayCard();
                playerTwoPlayCard();
                checkRound();
                break;
        }
    }

    /// <summary>
    /// After a player has one the round, moves the played cards
    /// to the bottom of the winning players deck and starts
    /// a new round.
    /// </summary>
    public void ResetRound()
    {
        switch(State)
        {
            case GameState.PlayerOneWinsRound:
                {
                    //The cards must not be put back in the decks in a very
                    // set way. Otherwise, the game will get stuck in a loop.
                    // Take the Shuffles out and play through a game and you 
                    // will see the loop.
                    StandardPokerDeck tempDeck = new(StartingStates.Empty);
                    foreach(var c in _playerOnePlayedCards)
                        tempDeck.PushBottom(c);
                    foreach(var c in _playerTwoPlayedCards)
                        tempDeck.PushBottom(c);

                    tempDeck.Shuffle();

                    foreach(var c in tempDeck)
                        _playerOneDeck.PushBottom(c);
                    _playerOnePlayedCards.Clear();
                    _playerTwoPlayedCards.Clear();
                    State = GameState.WaitingForBothPlayers;
                    break; 
                }
            case GameState.PlayerTwoWinsRound: 
                {
                    StandardPokerDeck tempDeck = new(StartingStates.Empty);
                    foreach(var c in _playerOnePlayedCards)
                        tempDeck.PushBottom(c);
                    foreach(var c in _playerTwoPlayedCards)
                        tempDeck.PushBottom(c);
                   
                    tempDeck.Shuffle();

                    foreach(var c in tempDeck)
                        _playerTwoDeck.PushBottom(c);
                    _playerOnePlayedCards.Clear();
                    _playerTwoPlayedCards.Clear();
                    State = GameState.WaitingForBothPlayers;
                    break; 
                }
        }
         
    }


    private void DealCards(StandardPokerDeck deck)
    {
        while(deck.CardsInDeck > 1)
        {
            _playerOneDeck.PushBottom(deck.Pop()!);
            _playerTwoDeck.PushBottom(deck.Pop()!);
        }
        if(deck.CardsInDeck == 1)
        {
            _playerOneDeck.PushBottom(deck.Pop()!);
        }
    }


    private void playerOnePlayCard()
    {
        var card = _playerOneDeck.Pop();
        if(card is not null)
        {
            _playerOnePlayedCards.Add(card);
        }
    }


    private void playerTwoPlayCard()
    {
        var card = _playerTwoDeck.Pop();
        if(card is not null)
        {
            _playerTwoPlayedCards.Add(card);
        }
    }
   

    private void checkRound()
    {
        if(_playerTwoPlayedCards.Count == 0 || _playerOnePlayedCards.Count == 0)
            return;

        int p1 = CardValue(_playerOnePlayedCards.Last());
        int p2 = CardValue(_playerTwoPlayedCards.Last());

        if(p1 > p2)
        {
            if(_playerTwoDeck.CardsInDeck == 0)
            {
                State = GameState.PlayerOneWinsGame;
            }
            else
            {
                State = GameState.PlayerOneWinsRound;
            }
        }
        else if(p1 < p2)
        {
            if(_playerOneDeck.CardsInDeck == 0)
            {
                State = GameState.PlayerTwoWinsGame;
            }
            else
            {
                State = GameState.PlayerTwoWinsRound;
            }
        }
        else
        {
            if(_playerOneDeck.CardsInDeck < 2 && _playerTwoDeck.CardsInDeck < 2)
            {
                State = GameState.DrawByWarRule;
            }
            else if(_playerOneDeck.CardsInDeck >= 2 && _playerTwoDeck.CardsInDeck < 2)
            {
                State = GameState.PlayerOneWinsGameByWarRule;
            }
            else if(_playerOneDeck.CardsInDeck < 2 && _playerTwoDeck.CardsInDeck >= 2)
            {
                State = GameState.PlayerTwoWinsGameByWarRule;
            }
            else
            {
                State = GameState.WarWaitingForBothPlayers;
            }
        }
    }


    private int CardValue(Card card)
    {
        return card.Value switch
        {
            Values.Two => 2,
            Values.Three => 3,
            Values.Four => 4,
            Values.Five => 5,
            Values.Six => 6,
            Values.Seven => 7,
            Values.Eight => 8,
            Values.Nine => 9,
            Values.Ten => 10,
            Values.Jack => 11,
            Values.Queen => 12,
            Values.King => 13,
            Values.Ace => 14,
            _ => 0 //Jokers should not be in here but lets just make them low
        };
    }
}
