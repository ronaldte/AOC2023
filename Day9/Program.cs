var extrapolatedValuesSum = 0;

foreach(var line in File.ReadLines(@"input.txt"))
{
    // Part 1 - call normally
    //var numbers = line.Split(' ').Select(int.Parse).ToArray();
    
    // Part 2 - just reverse the order and use the same algorithm
    var numbers = line.Split(' ').Select(int.Parse).Reverse().ToArray();
    extrapolatedValuesSum += ExtrapolateSequence(numbers);
}

Console.WriteLine(extrapolatedValuesSum);

// Recursive method to find the next element, call untill all numbers are 0, then sum with the last number.
static int ExtrapolateSequence(int[] sequence)
{
    if(sequence.All(value => value == 0))
    {
        return 0;
    }

    // Calculate new differences 
    var nextSequence = new int[sequence.Length - 1];
    for(int i = 0; i < sequence.Length - 1; i++)
    {
        nextSequence[i] = sequence[i + 1] - sequence[i];
    }

    return sequence.Last() + ExtrapolateSequence(nextSequence);
}