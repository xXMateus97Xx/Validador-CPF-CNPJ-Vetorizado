namespace Validators;

public class Program
{
    public static void Main(string[] args)
    {
#if DEBUG
        Console.WriteLine(CpfValidator.ValidadorCpfFast("52998224725"));
        Console.WriteLine(CpfValidator.ValidadorCpfFastNewApi("52998224725"));
        Console.WriteLine(CnpjValidator.ValidadorCnpjFast("11444777000161"));
        Console.WriteLine(CnpjValidator.ValidadorCnpjFastNewApi("11444777000161"));
#else
        BenchmarkDotNet.Running.BenchmarkSwitcher.FromTypes([typeof(CpfValidator), typeof(CnpjValidator)]).Run();
#endif

        Console.ReadLine();
    }
}
