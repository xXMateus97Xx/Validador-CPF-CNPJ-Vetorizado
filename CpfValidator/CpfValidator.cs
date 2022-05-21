using BenchmarkDotNet.Attributes;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Validators;

public class CpfValidator
{
    public CpfValidator()
    {
        Cpfs = new string[]
        {
            "52998224725", //Valid
            "52998224721", //Last Invalid
            "52998224715", //First Invalid
        };
    }

    [ParamsSource(nameof(Cpfs))]
    public string Cpf { get; set; }
    public string[] Cpfs { get; set; }

    [Benchmark]
    public bool ScalarBenchmark()
    {
        return ValidadorCpf(Cpf);
    }

    [Benchmark]
    public bool SimdBenchmark()
    {
        return ValidadorCpfFast(Cpf);
    }

    [Benchmark]
    public bool SimdVectorBenchmark()
    {
        return ValidadorCpfFast2(Cpf);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCpf(string cpf)
    {
        var sum = 0;

        for (int i = 0, j = 100; i < cpf.Length - 2; i++, j -= 10)
            sum += (cpf[i] - '0') * j;

        if (sum % 11 != cpf[cpf.Length - 2] - '0')
            return false;

        sum = 0;

        for (int i = 0, j = 110; i < cpf.Length - 1; i++, j -= 10)
            sum += (cpf[i] - '0') * j;

        return sum % 11 == cpf[cpf.Length - 1] - '0';
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCpfFast(string cpf)
    {
        Span<short> cpfPtr = stackalloc short[16];

        var str = MemoryMarshal.Cast<char, short>(cpf.AsSpan());
        str.CopyTo(cpfPtr);

        var vec = new Vector<short>(cpfPtr);

        var cpfVec = vec.AsVector256();

        var charFilter = Vector256.Create('0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 0, 0, 0, 0, 0).AsInt16();

        var zeros = Vector128<short>.Zero;

        var multipliers = Vector256.Create(100, 90, 80, 70, 60, 50, 40, 30, 20, 0, 0, 0, 0, 0, 0, 0);

        var nums = Avx2.Subtract(cpfVec, charFilter);

        var multiply = Avx2.MultiplyLow(nums, multipliers);

        var r = Ssse3.HorizontalAdd(multiply.GetLower(), multiply.GetUpper());
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);

        var sum = r.ToScalar();

        if (sum % 11 != nums.GetElement(9))
            return false;

        multipliers = Vector256.Create(110, 100, 90, 80, 70, 60, 50, 40, 30, 20, 0, 0, 0, 0, 0, 0);

        multiply = Avx2.MultiplyLow(nums, multipliers);

        r = Ssse3.HorizontalAdd(multiply.GetLower(), multiply.GetUpper());
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);

        sum = r.ToScalar();

        return sum % 11 == nums.GetElement(10);
    }

    const short ZeroChar = (short)'0';
    private static readonly short[] CharFilter = new short[] { ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, 0, 0, 0, 0, 0 };
    private static readonly short[] Multipliers1 = new short[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 0, 0, 0, 0, 0, 0, 0 };
    private static readonly short[] Multipliers2 = new short[] { 110, 100, 90, 80, 70, 60, 50, 40, 30, 20, 0, 0, 0, 0, 0, 0 };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCpfFast2(string cpf)
    {
        Span<short> cpfPtr = stackalloc short[16];

        var str = MemoryMarshal.Cast<char, short>(cpf.AsSpan());
        str.CopyTo(cpfPtr);

        var vec = new Vector<short>(cpfPtr);

        var charFilter = new Vector<short>(CharFilter);

        var multipliers = new Vector<short>(Multipliers1);

        var nums = vec - charFilter;

        var multiply = nums * multipliers;

        var sum = Vector.Sum(multiply);

        if (sum % 11 != nums[9])
            return false;

        multipliers = new Vector<short>(Multipliers2);

        multiply = nums * multipliers;

        sum = Vector.Sum(multiply);

        return sum % 11 == nums[10];
    }
}
