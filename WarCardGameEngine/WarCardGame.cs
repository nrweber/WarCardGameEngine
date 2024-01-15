using nic_weber.DeckOfCards;

namespace nic_weber.WarCardGameEngine;

/// <summary>
/// Two person card game. Each player will place one card
/// from their stack on the table. The player with the highest
/// card, picks up both cards and puts them on the bottom of 
/// their deck.
/// </summary>
public class WarCardGame
{
    private readonly StandardPokerDeck _deck;

    /// <summary>
    /// Start a new game of War with the given deck.
    /// The deck is evenly devided between each player
    /// one card at a time. If there is an odd amount 
    /// of cards, the first player will recieve an extra
    /// card.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown if given deck does not contain enough cards.
    /// </exception>
    public WarCardGame(StandardPokerDeck deck)
    {
        if(deck.CardsInDeck == 0)
            throw new ArgumentException("Deck needs to have at least 2 cards.");

        _deck = deck;
    }
}
