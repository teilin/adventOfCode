using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day04 : ISolver
{
    string[] game, numbers;
    int[] owned_numbers, winning_numbers;
    Dictionary<int, int> repeats = new Dictionary<int, int>();
    Dictionary<int, int> games_record = new Dictionary<int, int>();
    int CountThemAll() => repeats.Sum(x => x.Value);

    int StringToInt(string s) => int.Parse(s);

    int CountGame(int[] owned, int[] winning){
        int additional_cards = 0;
        foreach(int o in owned){
            if(!winning.Contains(o)) continue;
            additional_cards++;
        }
        return additional_cards;
    }

    void Scratch(){
        foreach(KeyValuePair<int, int> g in games_record){
            for(int i = 1; i <= g.Value; i++)
            if(repeats.ContainsKey(g.Key + i))
                repeats[g.Key + i] += repeats[g.Key];
        }
    }

    public Task Solve(Input input)
    {
        // var game = ParseInput(input.inputs);

        // var totalPoints = 0.0;
        // foreach(var card in game.GetCards)
        // {
        //     if(card.NumberOfWinningCards > 0) totalPoints += Math.Pow(2, card.NumberOfWinningCards - 1);
        // }
        // Console.WriteLine($"Part 1 -> {totalPoints}");

        var lines = input.inputs.ToArray();
        for(int i = 0; i < lines.Length; i++){
            repeats.Add(i+1, 0);
            repeats[i+1] += 1;
            game = lines[i].Split(':');
            numbers = game[1].Replace("  ", " ").Split(" | ");
            owned_numbers = Array.ConvertAll(numbers[0].Substring(1).Split(' '), new Converter<string, int>(StringToInt));
            winning_numbers = Array.ConvertAll(numbers[1].Split(' '), new Converter<string, int>(StringToInt));
            games_record[i+1] = CountGame(owned_numbers, winning_numbers);
        }

        Scratch();
        Console.WriteLine(CountThemAll());

        return Task.CompletedTask;
    }

    private Game ParseInput(IList<string> inputs)
    {
        var game = new Game();
        foreach(var line in inputs)
        {
            var tmp = line.Substring(5).Split(": ", StringSplitOptions.RemoveEmptyEntries);
            var cardId = int.Parse(tmp[0]);
            var decs = tmp[1].Split(" | ", StringSplitOptions.RemoveEmptyEntries);
            var winning = decs[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
            var onhand = decs[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
            game.AddCard(new Card(cardId, winning, onhand));
        }
        return game;
    }

    private sealed class Game
    {
        private readonly IList<Card> Cards = new List<Card>();

        public void AddCard(Card card) => Cards.Add(card);
        public IList<Card> GetCards => Cards;

        public int GetNumberOfCards()
        {
            var numbers = new int[Cards.Count];
            for(var i=0;i<numbers.Length;i++) numbers[i]=1;
            foreach(var card in Cards)
            {
                for(var i=0;i<card.NumberOfWinningCards;i++)
                {
                    numbers[card.Id+i] += numbers[card.Id];
                }
            }
            return numbers.Sum();
        }

        public long CountCards()
        {
            long[] numbers = new long[Cards.Count];

            for (int i = 0; i < Cards.Count; i++)
            {
                numbers[i] = 1;
            }

            foreach (var card in Cards)
            {
                var noMatches = card.GetNoScratchcardsForCard();
                for (int i = 1; i <= noMatches; i++)
                {
                    numbers[card.Id + i] += numbers[card.Id];
                }
            }

            var noTotalCards = numbers.Sum();

            return noTotalCards;
        }
    }

    private sealed class Card
    {
        private int id;
        private readonly IList<int> winning = new List<int>();
        private readonly IList<int> onhand = new List<int>();
        public Card(int id, IList<int> winning, IList<int> onhand)
        {
            this.id = id;
            this.winning = winning;
            this.onhand = onhand;
        }
        public int Id => id;

        public int NumberOfWinningCards => onhand.Intersect(winning).Count();

        public long GetNoScratchcardsForCard()
        {
            long noMatchingCards = 0;

            foreach (var i in winning)
            {
                foreach (var j in onhand)
                {
                    if (i == j)
                    {
                        noMatchingCards++;
                    }
                }
            }

            return noMatchingCards;
        }
    }
}