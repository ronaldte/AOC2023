
var cards = new Dictionary<int, int>();
int points = 0;

foreach (var line in File.ReadLines(@"input.txt"))
{
    var cardNumber = int.Parse(line[4..line.IndexOf(':')].Trim());
    cards.TryAdd(cardNumber, 1);

    var winningNumbers = ParseNumbers(line[(line.IndexOf(':') + 1)..line.IndexOf('|')]);
    var myNumbers = ParseNumbers(line[(line.IndexOf('|') + 1)..]);

    // Part 1 - count the number of matching numbers and double the winning for each pair
    points += (int)(1 * Math.Pow(2, winningNumbers.Intersect(myNumbers).Count() - 1));

    // Part 2 - for next number of matching numbers cards add one new card
    var matchingNumbers = winningNumbers.Intersect(myNumbers).Count();
    for (int i = cardNumber + 1; i <= cardNumber + matchingNumbers; i++)
    {
        cards.TryAdd(i, 1);
        // if we have multiple of the same cardnumber, all winnings are the same so just add the number
        // to all next cards
        cards[i] += cards[cardNumber];
    }
}

Console.WriteLine("Part 1: " + points);
Console.WriteLine("Part 2: " + cards.Select(pair => pair.Value).Sum());

static IEnumerable<int> ParseNumbers(string line) => line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);