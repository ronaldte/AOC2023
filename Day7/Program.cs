// Card, Bet
Dictionary<string, int> hands = File.ReadLines(@"input.txt")
    .Select(line => line.Split(' '))
    .ToDictionary(splitLine => splitLine[0], splitLine => int.Parse(splitLine[1]));

var cardValues = new Dictionary<char, int>()
{
    { 'A', 14 },
    { 'K', 13 },
    { 'Q', 12 },
    // Part 1
    //{ 'J', 11 },
    
    // Part 2
    {'J', 1 },

    { 'T', 10 },
    { '9', 9 },
    { '8', 8 },
    { '7', 7 },
    { '6', 6 },
    { '5', 5 },
    { '4', 4 },
    { '3', 3 },
    { '2', 2 },
};

var handValue = new List<(string, int)>();

foreach (var hand in hands)
{
    var cards = hand.Key.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

    // Part 2 - wildcard
    // Remove J cards and add it's count to the card with biggest count, ignore cards with 'JJJJJ'
    cards.TryGetValue('J', out int charCount);
    if(charCount > 0 && charCount < 5)
    {
        cards.Remove('J');
        var mostCommon = cards.OrderByDescending(letter => letter.Value).First().Key;
        cards[mostCommon] += charCount;
    }

    // High card = 1, ... Five of a kind = 7
    // Set hand value based on two rules, first number of different cards, if multiple options possible
    // multiply counts together (product)
    var product = cards.Values.Aggregate((product, current) => product * current);
    var value = cards.Values.Count switch
    {
        5 => 1,
        4 => 2,
        3 when product == 4 => 3,
        3 when product == 3 => 4,
        2 when product == 6 => 5,
        2 when product == 4 => 6,
        _ => 7,
    };

    handValue.Add((hand.Key, value));
}

// If hands have same value order them by higher card in order from start
// this is done by converting each card to it's numerical value, padded left so it's comparable 
// with double digits values => T (10) is bigger then 9 (09) with string comparison.
var ranks = handValue.OrderBy(h => h.Item2).ThenBy(hand =>
{
    var symbols = hand.Item1.Select(i => cardValues[i].ToString().PadLeft(2, '0')).ToList();
    return string.Join("", symbols);
}).Select(hand => hand.Item1).ToList();

// Final order sets it's ranks these ranks are then multiplied with bets and resulting in final winnings.
var winnings = 0;
for (int i = 0; i < hands.Count; i++)
{
    winnings += hands[ranks[i]] * (i+1);
}

Console.WriteLine(winnings);