var input = File.ReadAllLines(@"input.txt");
var instruction = input[0];
var currentInstructionId = 0;
var nodes = input[2..]
    .Select(line => line.Split(new string[] { "= (", ",", ")" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    .ToDictionary(split => split[0], split => (split[1], split[2]));

var currentNodes = nodes.Where(node => node.Key.EndsWith('A')).Select(node => node.Key).ToArray();

// Store number of steps it took from start to finish state for each start node
var steps = new ulong[currentNodes.Length];

for (var i = 0; i < currentNodes.Length; i++)
{
    var node = currentNodes[i];
    currentInstructionId = 0;
    
    // Iterate current node till it's end state
    while (!currentNodes[i].EndsWith('Z'))
    {
        // load its possible left and rigth state and select new based on current instruction
        var (left, right) = nodes[currentNodes[i]];
        currentNodes[i] = instruction[currentInstructionId++ % instruction.Length] == 'L' ? left : right;
    }

    steps[i] = (ulong)currentInstructionId;
}

// Doing this with all start nodes at the same time would run for long time, instead find how many steps it took to find each finish state
// and calculate least common multiple (LCM).
static ulong gcd(ulong a, ulong b)
{
    while (b != 0)
    {
        ulong temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

Console.WriteLine(steps.Aggregate((x, y) => x * y / gcd(x, y)));