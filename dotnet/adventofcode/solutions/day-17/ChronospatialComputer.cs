namespace adventofcode;

public class Register(long a, long b, long c)
{
    public long A { get; set; } = a;
    public long B { get; set; } = b;
    public long C { get; set; } = c;
    public List<long> Output { get; set; } = [];
}

public class Computer(Register register, int[] instructions)
{
    public Register Register { get; } = register;
    public int[] Instructions { get; } = instructions;
}

public class ChronospatialComputer : ExecutionMeasure, ISolution
{
    public int Day => 17;
    public string Puzzle => "Chronospatial Computer";

    private Lazy<Computer> Input;

    public ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute()
    {
        var resultPartOne = SolutionPartOne();
        var resultPartTwo = SolutionPartTwo();
        var millisecondsPartOne = MeasureExecutionTime(SolutionPartOne);
        var millisecondsPartTwo = MeasureExecutionTime(SolutionPartTwo);

        return ((resultPartOne.ToString(), millisecondsPartOne), (resultPartTwo.ToString(), millisecondsPartTwo));
    }

    public void SetInput(string inputSource)
    {
        Input = new(() =>
        {
            var lines = File.ReadAllLines(inputSource);

            int regA = int.Parse(lines[0].Split(':')[1].Trim());
            int regB = int.Parse(lines[1].Split(':')[1].Trim());
            int regC = int.Parse(lines[2].Split(':')[1].Trim());

            var instructions = lines[4].Split(':')[1]
                                       .Trim()
                                       .Split(',')
                                       .Select(int.Parse)
                                       .ToArray();

            return new Computer(
                new Register(regA, regB, regC), instructions);
        });
    }

    private Dictionary<int, Action<Register, int>> InstructionsMethods => new()
{
    { 0, (register, operand) => Adv(register, operand) },
    { 1, (register, operand) => Bxl(register, operand) },
    { 2, (register, operand) => Bst(register, operand) },
    { 4, (register, _) => Bxc(register) },
    { 5, (register, operand) => Out(register, operand) },
    { 6, (register, operand) => Bdv(register, operand) },
    { 7, (register, operand) => Cdv(register, operand) }
};

    private void Adv(Register register, int operand)
    {
        int divisor = (int)Math.Pow(2, ResolveOperand(register, operand));
        register.A /= divisor;
    }

    private void Bxl(Register register, int operand)
    {
        register.B ^= operand;
    }

    private void Bst(Register register, int operand)
    {
        register.B = ResolveOperand(register, operand) % 8;
    }

    private void Bxc(Register register)
    {
        register.B ^= register.C;
    }

    private void Out(Register register, int operand)
    {
        long output = ResolveOperand(register, operand) % 8;
        register.Output.Add(output);
    }

    private void Bdv(Register register, int operand)
    {
        int divisor = (int)Math.Pow(2, ResolveOperand(register, operand));
        register.B = register.A / divisor;
    }

    private void Cdv(Register register, int operand)
    {
        int divisor = (int)Math.Pow(2, ResolveOperand(register, operand));
        register.C = register.A / divisor;
    }

    private long ResolveOperand(Register register, int operand)
    {
        return operand switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => register.A,
            5 => register.B,
            6 => register.C,
        };
    }

    private string SolutionPartOne()
    {
        var input = Input.Value;

        var register = new Register(input.Register.A, input.Register.B, input.Register.C);

        for (int idx = 0; idx < input.Instructions.Length; idx += 2)
        {
            int opcode = input.Instructions[idx];
            int operand = input.Instructions[idx + 1];
            if (opcode == 3)
            {
                if (register.A != 0)
                {
                    idx = operand - 2;
                }
                continue;
            }

            InstructionsMethods[opcode](register, operand);
        }

        return string.Join(",", register.Output);
    }

    private string SolutionPartTwo()
    {
        return "/";
    }
}

