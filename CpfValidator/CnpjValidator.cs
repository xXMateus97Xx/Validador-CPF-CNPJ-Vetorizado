using BenchmarkDotNet.Attributes;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Validators;

public class CnpjValidator
{
    public CnpjValidator()
    {
        Cnpjs = new string[]
        {
            "11444777000161", //Valid
            "11444777000165", //Last Invalid
            "11444777000101", //First Invalid
        };
    }

    [ParamsSource(nameof(Cnpjs))]
    public string Cnpj { get; set; }
    public string[] Cnpjs { get; set; }

    [Benchmark]
    public bool ScalarBenchmark()
    {
        return ValidadorCnpj(Cnpj);
    }

    [Benchmark]
    public bool SimdBenchmark()
    {
        return ValidadorCnpjFast(Cnpj);
    }

    [Benchmark]
    public bool SimdVectorBenchmark()
    {
        return ValidadorCnpjFast2(Cnpj);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCnpj(string cnpj)
    {
        var sum = 0;
        int i = 0;
        int j = 5;
        for (; i < 4; i++, j--)
            sum += (cnpj[i] - '0') * j;

        j = 9;

        for (; i < cnpj.Length - 2; i++, j--)
            sum += (cnpj[i] - '0') * j;

        var mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        if (mod != cnpj[cnpj.Length - 2] - '0')
            return false;

        i = 0;
        j = 6;
        sum = 0;
        for (; i < 5; i++, j--)
            sum += (cnpj[i] - '0') * j;

        j = 9;

        for (; i < cnpj.Length - 1; i++, j--)
            sum += (cnpj[i] - '0') * j;

        mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        return mod == cnpj[cnpj.Length - 1] - '0';
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCnpjFast(string cnpj)
    {
        if (!Avx2.IsSupported)
            throw new PlatformNotSupportedException("Avx2 not supported");

        if (cnpj.Length < 14)
            return false;

        var cpfVec = Vector256.Create(cnpj[0], cnpj[1], cnpj[2], cnpj[3], cnpj[4], cnpj[5], cnpj[6], cnpj[7],
                                      cnpj[8], cnpj[9], cnpj[10], cnpj[11], cnpj[12], cnpj[13], 0, 0).AsInt16();

        var charFilter = Vector256.Create('0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 0, 0).AsInt16();

        var zeros = Vector128<short>.Zero;

        var multipliers = Vector256.Create(5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0, 0);

        var nums = Avx2.Subtract(cpfVec, charFilter);

        var multiply = Avx2.MultiplyLow(nums, multipliers);

        var r = Ssse3.HorizontalAdd(multiply.GetLower(), multiply.GetUpper());
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);

        var sum = r.ToScalar();

        var mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        if (mod != nums.GetElement(12))
            return false;

        multipliers = Vector256.Create(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0);

        multiply = Avx2.MultiplyLow(nums, multipliers);

        r = Ssse3.HorizontalAdd(multiply.GetLower(), multiply.GetUpper());
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);

        sum = r.ToScalar();

        mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        return mod == nums.GetElement(13);
    }

    const short ZeroChar = (short)'0';
    private static readonly short[] CharFilter = new short[] { ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, ZeroChar, 0, 0 };
    private static readonly short[] Multipliers1 = new short[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0, 0 };
    private static readonly short[] Multipliers2 = new short[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0 };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCnpjFast2(string cnpj)
    {
        if (!Vector.IsHardwareAccelerated || Vector<short>.Count != 16)
            throw new PlatformNotSupportedException("Hardware accelaration not supported");

        Span<short> cnpjPtr = stackalloc short[Vector<short>.Count];

        var str = MemoryMarshal.Cast<char, short>(cnpj.AsSpan());
        str.CopyTo(cnpjPtr);

        var vec = new Vector<short>(cnpjPtr);

        var charFilter = new Vector<short>(CharFilter);

        var multipliers = new Vector<short>(Multipliers1);

        var nums = vec - charFilter;

        var multiply = nums * multipliers;

        var sum = Vector.Sum(multiply);

        var mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        if (mod != nums[12])
            return false;

        multipliers = new Vector<short>(Multipliers2);

        multiply = nums * multipliers;

        sum = Vector.Sum(multiply);

        mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        return mod == nums[13];
    }
}
