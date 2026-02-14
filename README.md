# Validador CPF e CNPJ Vetorizado

Validador de CPF e CNPJ utilizando Vector e Intrinsícos do C#

# Benchmark Validação CPF

```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.202
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2


```
| Method              | Cpf         | Mean       | Error     | StdDev    |
|-------------------- |------------ |-----------:|----------:|----------:|
| **ScalarBenchmark**     | **11111111111** | **11.2815 ns** | **0.0422 ns** | **0.0374 ns** |
| SimdBenchmark       | 11111111111 |  4.0383 ns | 0.0100 ns | 0.0093 ns |
| SimdBenchmarkNewApi | 11111111111 |  4.2008 ns | 0.0155 ns | 0.0145 ns |
| **ScalarBenchmark**     | **12.-...+./.** |  **7.4764 ns** | **0.0457 ns** | **0.0382 ns** |
| SimdBenchmark       | 12.-...+./. |  4.4925 ns | 0.0103 ns | 0.0086 ns |
| SimdBenchmarkNewApi | 12.-...+./. |  4.4647 ns | 0.0117 ns | 0.0109 ns |
| **ScalarBenchmark**     | **123**         |  **0.0105 ns** | **0.0031 ns** | **0.0026 ns** |
| SimdBenchmark       | 123         |  0.0113 ns | 0.0014 ns | 0.0013 ns |
| SimdBenchmarkNewApi | 123         |  0.0328 ns | 0.0133 ns | 0.0111 ns |
| **ScalarBenchmark**     | **12CABCABCAB** |  **7.5368 ns** | **0.1326 ns** | **0.2708 ns** |
| SimdBenchmark       | 12CABCABCAB |  4.3473 ns | 0.0131 ns | 0.0123 ns |
| SimdBenchmarkNewApi | 12CABCABCAB |  3.9773 ns | 0.0113 ns | 0.0100 ns |
| **ScalarBenchmark**     | **529982247+.** |  **7.4957 ns** | **0.0176 ns** | **0.0156 ns** |
| SimdBenchmark       | 529982247+. |  4.5261 ns | 0.0116 ns | 0.0109 ns |
| SimdBenchmarkNewApi | 529982247+. |  4.4692 ns | 0.0160 ns | 0.0150 ns |
| **ScalarBenchmark**     | **52998224715** |  **7.4560 ns** | **0.0085 ns** | **0.0076 ns** |
| SimdBenchmark       | 52998224715 |  7.5229 ns | 0.0263 ns | 0.0246 ns |
| SimdBenchmarkNewApi | 52998224715 |  6.6008 ns | 0.0080 ns | 0.0075 ns |
| **ScalarBenchmark**     | **52998224721** | **13.9770 ns** | **0.0140 ns** | **0.0117 ns** |
| SimdBenchmark       | 52998224721 |  9.8002 ns | 0.0162 ns | 0.0151 ns |
| SimdBenchmarkNewApi | 52998224721 |  8.6598 ns | 0.0176 ns | 0.0156 ns |
| **ScalarBenchmark**     | **52998224725** | **14.0157 ns** | **0.0236 ns** | **0.0185 ns** |
| SimdBenchmark       | 52998224725 |  9.8090 ns | 0.0204 ns | 0.0191 ns |
| SimdBenchmarkNewApi | 52998224725 |  8.6687 ns | 0.0161 ns | 0.0143 ns |
| **ScalarBenchmark**     | **529982247AB** |  **7.4749 ns** | **0.0369 ns** | **0.0345 ns** |
| SimdBenchmark       | 529982247AB |  4.2994 ns | 0.0165 ns | 0.0154 ns |
| SimdBenchmarkNewApi | 529982247AB |  3.9535 ns | 0.0214 ns | 0.0200 ns |

52998224725 é um CPF válido

52998224715 possui o 1º dígito verificador inválido

52998224721 possui o 2º dígito verificador inválido

# Benchmark validação CNPJ

```

BenchmarkDotNet v0.15.6, Windows 11 (10.0.26200.7840)
AMD Ryzen 5 3600 3.59GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3


```
| Method              | Cnpj           | Mean       | Error     | StdDev    |
|-------------------- |--------------- |-----------:|----------:|----------:|
| **ScalarBenchmark**     | **11444777000101** | **14.8630 ns** | **0.0170 ns** | **0.0142 ns** |
| SimdBenchmark       | 11444777000101 |  5.8854 ns | 0.0074 ns | 0.0069 ns |
| SimdBenchmarkNewApi | 11444777000101 |  5.8880 ns | 0.0062 ns | 0.0058 ns |
| **ScalarBenchmark**     | **11444777000161** | **23.0970 ns** | **0.0284 ns** | **0.0222 ns** |
| SimdBenchmark       | 11444777000161 |  8.8741 ns | 0.0100 ns | 0.0089 ns |
| SimdBenchmarkNewApi | 11444777000161 |  8.8733 ns | 0.0059 ns | 0.0052 ns |
| **ScalarBenchmark**     | **11444777000165** | **22.8371 ns** | **0.0236 ns** | **0.0221 ns** |
| SimdBenchmark       | 11444777000165 |  8.8757 ns | 0.0092 ns | 0.0086 ns |
| SimdBenchmarkNewApi | 11444777000165 |  8.8789 ns | 0.0122 ns | 0.0114 ns |
| **ScalarBenchmark**     | **21./+...++/...** |  **1.6488 ns** | **0.0035 ns** | **0.0031 ns** |
| SimdBenchmark       | 21./+...++/... |  2.9104 ns | 0.0036 ns | 0.0033 ns |
| SimdBenchmarkNewApi | 21./+...++/... |  3.0365 ns | 0.0236 ns | 0.0221 ns |
| **ScalarBenchmark**     | **214657898456.+** |  **7.4003 ns** | **0.0420 ns** | **0.0393 ns** |
| SimdBenchmark       | 214657898456.+ |  2.9452 ns | 0.0052 ns | 0.0049 ns |
| SimdBenchmarkNewApi | 214657898456.+ |  2.9452 ns | 0.0044 ns | 0.0039 ns |
| **ScalarBenchmark**     | **214657898456LH** | **15.5847 ns** | **0.1082 ns** | **0.1012 ns** |
| SimdBenchmark       | 214657898456LH |  2.5583 ns | 0.0070 ns | 0.0065 ns |
| SimdBenchmarkNewApi | 214657898456LH |  2.5616 ns | 0.0070 ns | 0.0065 ns |
| **ScalarBenchmark**     | **21ABCDFERGEsdf** |  **7.1287 ns** | **0.0286 ns** | **0.0239 ns** |
| SimdBenchmark       | 21ABCDFERGEsdf |  2.5660 ns | 0.0025 ns | 0.0021 ns |
| SimdBenchmarkNewApi | 21ABCDFERGEsdf |  2.5662 ns | 0.0042 ns | 0.0037 ns |
| **ScalarBenchmark**     | **29ZXCHB7000135** | **15.8101 ns** | **0.3454 ns** | **0.3231 ns** |
| SimdBenchmark       | 29ZXCHB7000135 |  5.8912 ns | 0.0181 ns | 0.0170 ns |
| SimdBenchmarkNewApi | 29ZXCHB7000135 |  5.8874 ns | 0.0131 ns | 0.0122 ns |
| **ScalarBenchmark**     | **29ZXCHB7000175** | **24.0046 ns** | **0.0729 ns** | **0.0682 ns** |
| SimdBenchmark       | 29ZXCHB7000175 |  8.8712 ns | 0.0086 ns | 0.0077 ns |
| SimdBenchmarkNewApi | 29ZXCHB7000175 |  8.8788 ns | 0.0112 ns | 0.0104 ns |
| **ScalarBenchmark**     | **29ZXCHB7000179** | **23.9195 ns** | **0.0678 ns** | **0.0634 ns** |
| SimdBenchmark       | 29ZXCHB7000179 |  8.8793 ns | 0.0115 ns | 0.0108 ns |
| SimdBenchmarkNewApi | 29ZXCHB7000179 |  8.8716 ns | 0.0091 ns | 0.0081 ns |
| **ScalarBenchmark**     | **321**            |  **0.0089 ns** | **0.0016 ns** | **0.0014 ns** |
| SimdBenchmark       | 321            |  0.0108 ns | 0.0014 ns | 0.0013 ns |
| SimdBenchmarkNewApi | 321            |  0.0796 ns | 0.0014 ns | 0.0013 ns |

11444777000161 e 29ZXCHB7000175 são CNPJs válidos

11444777000101 e 29ZXCHB7000135 possuem o 1º dígito verificador inválido

11444777000165 e 29ZXCHB7000179 possuem o 2º dígito verificador inválido
