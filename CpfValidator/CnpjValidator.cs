using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Validators;

public class CnpjValidator
{
    public CnpjValidator()
    {
        Cnpjs =
        [
            "11444777000161", //Valid
            "11444777000165", //Last Invalid
            "11444777000101", //First Invalid
            "29ZXCHB7000175", //Valid
            "29ZXCHB7000179", //Last Invalid
            "29ZXCHB7000135", //First Invalid
            "321",
            "21ABCDFERGEsdf",
            "21./+...++/...",
            "214657898456.+",
            "214657898456LH",
        ];
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

        int i, j, sum;
        for (i = 0; i < cnpj.Length; i++)
        {
            var c1 = cnpj[i];
            if (c1 < '0' || (c1 > '9' && c1 < 'A') || c1 > 'Z')
                return false;
        }

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

        var cnpjVec = Vector256.Create(cnpj[0], cnpj[1], cnpj[2], cnpj[3], cnpj[4], cnpj[5], cnpj[6], cnpj[7],
                                      cnpj[8], cnpj[9], cnpj[10], cnpj[11], cnpj[12], cnpj[13], 0, 0).AsInt16();


        var charFilter = Vector256.Create('Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', '9', '9', 0, 0).AsInt16();

        var comparerResult = Avx2.CompareGreaterThan(cnpjVec, charFilter);

        var mask = Avx2.MoveMask(comparerResult.AsByte());
        if (mask != 0)
            return false;

        charFilter = Vector256.Create('9', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', '9', 0, 0).AsInt16();

        comparerResult = Avx2.CompareGreaterThan(cnpjVec, charFilter);

        const short a = (short)'A' - 1;
        charFilter = Vector256.Create(a, a, a, a, a, a, a, a, a, a, a, a, a, a, -1, 1).AsInt16();

        comparerResult = Avx2.And(comparerResult, ~Avx2.CompareGreaterThan(cnpjVec, charFilter));

        mask = Avx2.MoveMask(comparerResult.AsByte());
        if (mask != 0)
            return false;

        const short z = (short)'0' - 1;
        charFilter = Vector256.Create(z, z, z, z, z, z, z, z, z, z, z, z, z, z, -1, -1);

        comparerResult = Avx2.CompareGreaterThan(cnpjVec, charFilter);

        mask = Avx2.MoveMask(comparerResult.AsByte());
        if (mask != -1)
            return false;

        var multipliers = Vector256.Create(5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0, 0);

        charFilter = Vector256.Create('0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', 0, 0).AsInt16();

        var nums = Avx2.Subtract(cnpjVec, charFilter);

        var multiply = Avx2.MultiplyLow(nums, multipliers);

        var zeros = Vector128<short>.Zero;

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

        var cnpjVec = Vector256.Create(cnpj[0], cnpj[1], cnpj[2], cnpj[3], cnpj[4], cnpj[5], cnpj[6], cnpj[7],
                                     cnpj[8], cnpj[9], cnpj[10], cnpj[11], cnpj[12], cnpj[13], ushort.MaxValue - 1, ushort.MaxValue - 1);

        const ushort min = '0';
        const ushort betweenMin = '9';
        const ushort betweenMax = 'A' - 1;
        const ushort max = 'Z';

        var charFilter = Vector256.Create(max, max, max, max, max, max, max, max, max, max, max, max, betweenMin, betweenMin, ushort.MaxValue - 1, ushort.MaxValue - 1);

        var comparerResult = Vector256.GreaterThanAny(cnpjVec, charFilter);

        if (comparerResult)
            return false;

        charFilter = Vector256.Create(betweenMin, betweenMin, betweenMin, betweenMin, betweenMin, betweenMin,
            betweenMin, betweenMin, betweenMin, betweenMin, betweenMin, betweenMin, betweenMin, betweenMin, ushort.MaxValue, ushort.MaxValue);

        var minResult = Vector256.GreaterThan(cnpjVec, charFilter);

        charFilter = Vector256.Create(betweenMax, betweenMax, betweenMax, betweenMax, betweenMax, betweenMax,
            betweenMax, betweenMax, betweenMax, betweenMax, betweenMax, betweenMax, betweenMax, betweenMax, ushort.MaxValue, ushort.MaxValue);

        var maxResult = Vector256.LessThanOrEqual(cnpjVec, charFilter);

        comparerResult = Vector256.EqualsAny(minResult, maxResult);

        if (comparerResult)
            return false;

        charFilter = Vector256.Create(min, min, min, min, min, min, min, min, min, min, min, min, min, min, ushort.MaxValue - 1, ushort.MaxValue - 1);

        comparerResult = Vector256.LessThanAny(cnpjVec, charFilter);

        if (comparerResult)
            return false;

        var multipliers = Vector256.Create((ushort)5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0, 0);

        var nums = cnpjVec - charFilter;

        var multiply = nums * multipliers;

        var sum = Vector256.Sum(multiply);

        var mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        if (mod != nums.GetElement(12))
            return false;

        multipliers = Vector256.Create((ushort)6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2, 0, 0, 0);

        multiply = nums * multipliers;

        sum = Vector256.Sum(multiply);

        mod = sum % 11;
        mod = mod < 2 ? 0 : 11 - mod;

        return mod == nums.GetElement(13);
    }
}
