using nic_weber.WarCardGameEngine;


WarCardGame game = new();


int roundCount = 0;
while(true)
{
    roundCount++;
    Console.Clear();
    Console.WriteLine($"Round {roundCount}");
    Console.WriteLine($"Player 1 Deck Count: {game.PlayerOneDeckSize}");
    Console.WriteLine($"Player 2 Deck Count: {game.PlayerTwoDeckSize}");
    Console.WriteLine("---------------------");
    Console.Write($"Player 1 Board: ");
    foreach(var card in game.PlayerOnePlayedCards)
    {
        Console.Write($"{card.Suit}{card.Value},");
    }
    Console.WriteLine();
    Console.Write($"Player 2 Board: ");
    foreach(var card in game.PlayerTwoPlayedCards)
    {
        Console.Write($"{card.Suit}{card.Value},");
    }
    Console.WriteLine();
    Console.WriteLine("---------------------");
    Console.WriteLine($"Status: {game.State}");

    Console.ReadKey();

    game.ResetRound();
    game.PlayerOneFlipCard();
    game.PlayerTwoFlipCard();
}





