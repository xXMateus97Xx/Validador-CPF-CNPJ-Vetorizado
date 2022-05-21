namespace Validators;

public class Program
{
    public static void Main(string[] args)
    {
#if DEBUG
        Console.WriteLine(CpfValidator.ValidadorCpfFast2("52998224725"));
        Console.WriteLine(CnpjValidator.ValidadorCnpjFast("11444777000161"));
#else
        BenchmarkDotNet.Running.BenchmarkSwitcher.FromTypes(new[] { typeof(CpfValidator), typeof(CnpjValidator) }).Run();
#endif

        Console.ReadLine();
    }
}
