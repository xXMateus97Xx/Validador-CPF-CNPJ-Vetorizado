using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
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
            "12CABCABCAB",
            "529982247AB",
            "529982247+.",
            "12.-...+./.",
            "123",
            "11111111111",
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCpf(string cpf)
    {
        if (cpf == null || cpf.Length != 11)
            return false;

        bool allEqual = true, allNumber = true;
        int i, j, sum;
        for (i = 0, j = 1; allEqual && allNumber && j < cpf.Length; i++, j++)
        {
            var c1 = cpf[i];
            var c2 = cpf[j];
            if (c1 != c2)
                allEqual = false;
            if (c1 < '0' || c1 > '9' || c2 < '0' || c2 > '9')
                allNumber = false;
        }

        if (allEqual || !allNumber)
            return false;

        sum = 0;

        for (i = 0, j = 100; i < cpf.Length - 2; i++, j -= 10)
            sum += (cpf[i] - '0') * j;

        sum %= 11;
        if (sum == 10)
            sum = 0;

        if (sum != cpf[cpf.Length - 2] - '0')
            return false;

        sum = 0;

        for (i = 0, j = 110; i < cpf.Length - 1; i++, j -= 10)
            sum += (cpf[i] - '0') * j;

        sum %= 11;
        if (sum == 10)
            sum = 0;

        return sum == cpf[cpf.Length - 1] - '0';
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCpfFast(string cpf)
    {
        if (!Avx2.IsSupported)
            throw new PlatformNotSupportedException("Avx2 not supported");

        if (cpf == null || cpf.Length != 11)
            return false;

        var cpfVec = Vector256.Create(cpf[0], cpf[1], cpf[2], cpf[3], cpf[4], cpf[5], cpf[6], cpf[7], cpf[8], cpf[9], cpf[10], 0, 0, 0, 0, 0).AsInt16();
        var charFilter = Avx2.ShiftRightLogical128BitLane(cpfVec, sizeof(short));
        charFilter = charFilter.WithElement(7, cpfVec.GetElement(8));
        charFilter = charFilter.WithElement(10, cpfVec.GetElement(10));
        var comparerResult = Avx2.CompareEqual(cpfVec, charFilter);

        var mask = Avx2.MoveMask(comparerResult.AsByte());
        if (mask == -1)
            return false;

        charFilter = Vector256.Create('9', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', 0, 0, 0, 0, 0).AsInt16();

        comparerResult = Avx2.CompareGreaterThan(cpfVec, charFilter);

        mask = Avx2.MoveMask(comparerResult.AsByte());
        if (mask != 0)
            return false;

        const short z = (short)'0' - 1;
        charFilter = Vector256.Create(z, z, z, z, z, z, z, z, z, z, z, -1, -1, -1, -1, -1).AsInt16();

        comparerResult = Avx2.CompareGreaterThan(cpfVec, charFilter);

        mask = Avx2.MoveMask(comparerResult.AsByte());
        if (mask != -1)
            return false;

        charFilter = Vector256.Create('0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 0, 0, 0, 0, 0).AsInt16();

        var nums = Avx2.Subtract(cpfVec, charFilter);

        var multipliers = Vector256.Create(100, 90, 80, 70, 60, 50, 40, 30, 20, 0, 0, 0, 0, 0, 0, 0);

        var multiply = Avx2.MultiplyLow(nums, multipliers);

        var zeros = Vector128<short>.Zero;

        var r = Ssse3.HorizontalAdd(multiply.GetLower(), multiply.GetUpper());
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);

        var sum = r.ToScalar();

        sum %= 11;
        if (sum == 10)
            sum = 0;

        if (sum != nums.GetElement(9))
            return false;

        multipliers = Vector256.Create(110, 100, 90, 80, 70, 60, 50, 40, 30, 20, 0, 0, 0, 0, 0, 0);

        multiply = Avx2.MultiplyLow(nums, multipliers);

        r = Ssse3.HorizontalAdd(multiply.GetLower(), multiply.GetUpper());
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);
        r = Ssse3.HorizontalAdd(r, zeros);

        sum = r.ToScalar();

        sum %= 11;
        if (sum == 10)
            sum = 0;

        return sum == nums.GetElement(10);
    }
}
