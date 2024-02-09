var lines = File.ReadAllText(@"input.txt").Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

// Part 1
// var seeds = lines[0][6..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

// Part 2
var seedIntervals = SplitSeedIntervals(lines.First());

foreach (var line in lines[1..])
{
    // (source range start, destionation range start), how many elements are there 
    Dictionary<(long, long), long> transformRules = ParseRuleIntervals(line);

    // Sometimes value changes in the interval, to calculate correct value throughout, we have to split the interval
    var nextSeeds = new List<(long, long)>();
    var queue = new Queue<(long, long)>(seedIntervals);

    // no intersect = no value change, split until there is no intersection between rules and seed intervals
    while (queue.Count > 0)
    {
        var seedRange = queue.Dequeue();
        foreach (var map in transformRules)
        {
            // Check if intervals overlap and, otherwise value doesn't change
            if (seedRange.Item1 <= map.Key.Item2 && map.Key.Item1 <= seedRange.Item2)
            {
                // Make sure interval start is smaller then its end
                var overlapRange = (Math.Max(map.Key.Item1, seedRange.Item1), Math.Min(map.Key.Item2, seedRange.Item2));
                // If the intervals are the same, we are done, otherwise we need to split the interval
                if (overlapRange != seedRange)
                {                   
                    queue.Enqueue(overlapRange.Item1 > seedRange.Item1 ? (seedRange.Item1, overlapRange.Item1-1) : (overlapRange.Item2+1, seedRange.Item2));
                    seedRange = overlapRange;
                }
            }
        }
        // Interval has same value from start to the end, becomes new interval for next iteration
        nextSeeds.Add(seedRange);
    }

    // Part1
    //for (int i = 0; i < seeds.Length; i++)
    //{
    //    seeds[i] += valueParser.FirstOrDefault(x => seeds[i] >= x.Key.Item1 && seeds[i] < x.Key.Item2, new KeyValuePair<(long, long), long>((0, 0), 0)).Value;
    //}

    // Part 2
    seedIntervals.Clear();
    // calculate shift -> going from source to destionation is just linear function -> compute difference and add the difference to my seeds
    // shift both start and end -> this way we projected source interval to destination interval
    foreach (var seed in nextSeeds)
    {
        var shift = transformRules.FirstOrDefault(x => seed.Item1 >= x.Key.Item1 && seed.Item1 < x.Key.Item2, new KeyValuePair<(long, long), long>((0, 0), 0)).Value;
        seedIntervals.Add((seed.Item1 + shift, seed.Item2 + shift));
    }

    transformRules.Clear();
}

// Part 1
// Console.WriteLine(seeds.Min());

// Part 2 split intervals mean the lowest value is on either end, flatting the sequence results in list of all number, our result is the
// lowerst number
Console.WriteLine(seedIntervals.SelectMany(t => new long[] { t.Item1, t.Item2 }).ToList().Min());


static IList<(long, long)> SplitSeedIntervals(string line)
{
    var seedNumbers = ParseNumbers(line[6..]);

    // instead of storring how many numbers there are in range, we store start and the end of the range.
    var seeds = new List<(long, long)>();
    for (int i = 0; i < seedNumbers.Count; i += 2)
    {
        seeds.Add((seedNumbers[i], seedNumbers[i] + seedNumbers[i + 1] - 1));
    }
    return seeds;
}

static Dictionary<(long, long), long> ParseRuleIntervals(string line)
{
    var transformRules = new Dictionary<(long, long), long>();
    var intervals = line[(line.IndexOf(':') + 1)..].Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
    foreach (var interval in intervals)
    {
        var range = ParseNumbers(interval);
        transformRules.Add((range[1], range[1] + range[2]), range[0] - range[1]);
    }
    return transformRules;
}

static IList<long> ParseNumbers(string line) => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();