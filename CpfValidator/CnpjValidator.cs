using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
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
            "321",
            "21ABCDFERGEsdf",
            "21./+...++/...",
            "214657898456.+",
            "214657898456LH",
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
    public bool SimdBenchmarkNewApi()
    {
        return ValidadorCnpjFast(Cnpj);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCnpj(string cnpj)
    {
        if (cnpj == null || cnpj.Length != 14)
            return false;

        var allNumber = true;
        int i, j, sum;
        for (i = 0; allNumber && i < cnpj.Length; i++)
        {
            var c1 = cnpj[i];
            if (c1 < '0' || c1 > '9')
                allNumber = false;
        }

        if (!allNumber)
            return false;

        sum = 0;
        for (i = 0, j = 5; i < 4; i++, j--)
            sum += (cnpj[i] - '0') * j;

        for (j = 9; i < cnpj.Length - 2; i++, j--)
            sum += (cnpj[i] - '0') * j;

        var mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        if (mod != cnpj[cnpj.Length - 2] - '0')
            return false;

        sum = 0;
        for (i = 0, j = 6; i < 5; i++, j--)
            sum += (cnpj[i] - '0') * j;

        for (j = 9; i < cnpj.Length - 1; i++, j--)
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

        if (cnpj == null || cnpj.Length != 14)
            return false;

        var cpfVec = Vector256.Create(cnpj[0], cnpj[1], cnpj[2], cnpj[3], cnpj[4], cnpj[5], cnpj[6], cnpj[7],
                                      cnpj[8], cnpj[9], cnpj[10], cnpj[11], cnpj[12], cnpj[13], 0, 0).AsInt16();

        var charFilter = Vector256.Create('9', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', 0, 0).AsInt16();

        var comparerResult = Avx2.CompareGreaterThan(cpfVec, charFilter);

        var mask = Avx2.MoveMask(comparerResult.AsByte());
        if (mask != 0)
            return false;

        const short z = (short)'0' - 1;
        charFilter = Vector256.Create(z, z, z, z, z, z, z, z, z, z, z, z, z, z, -1, -1).AsInt16();

        comparerResult = Avx2.CompareGreaterThan(cpfVec, charFilter);

        mask = Avx2.MoveMask(comparerResult.AsByte());
        if (mask != -1)
            return false;

        var zeros = Vector128<short>.Zero;

        var multipliers = Vector256.Create(5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0, 0);

        charFilter = Vector256.Create('0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 0, 0).AsInt16();

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ValidadorCnpjFastNewApi(string cnpj)
    {
        if (!Vector256.IsHardwareAccelerated)
            throw new PlatformNotSupportedException("Vec256 not supported");

        if (cnpj == null || cnpj.Length != 14)
            return false;

        var cpfVec = Vector256.Create(cnpj[0], cnpj[1], cnpj[2], cnpj[3], cnpj[4], cnpj[5], cnpj[6], cnpj[7],
                                     cnpj[8], cnpj[9], cnpj[10], cnpj[11], cnpj[12], cnpj[13], 0, 0).AsInt16();

        var charFilter = Vector256.Create("9\09\09\09\09\09\09\09\09\09\09\09\09\09\0\0\0\0\0"u8).AsInt16();

        var comparerResult = Vector256.GreaterThanAny(cpfVec, charFilter);

        if (comparerResult)
            return false;

        charFilter = Vector256.Create("0\00\00\00\00\00\00\00\00\00\00\00\00\00\0\0\0\0\0"u8).AsInt16();

        comparerResult = Vector256.LessThanAny(cpfVec, charFilter);

        if (comparerResult)
            return false;

        var multipliers = Vector256.Create(5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0, 0);

        var nums = cpfVec - charFilter;

        var multiply = nums * multipliers;

        var sum = Vector256.Sum(multiply);

        var mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        if (mod != nums.GetElement(12))
            return false;

        multipliers = Vector256.Create(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0);

        multiply = nums * multipliers;

        sum = Vector256.Sum(multiply);

        mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        return mod == nums.GetElement(13);
    }
}
