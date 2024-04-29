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

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.202
  [Host]     : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2


```
| Method              | Cnpj           | Mean       | Error     | StdDev    |
|-------------------- |--------------- |-----------:|----------:|----------:|
| **ScalarBenchmark**     | **11444777000101** | **15.2186 ns** | **0.2116 ns** | **0.1979 ns** |
| SimdBenchmark       | 11444777000101 |  6.2986 ns | 0.0265 ns | 0.0248 ns |
| SimdBenchmarkNewApi | 11444777000101 |  6.3187 ns | 0.0237 ns | 0.0221 ns |
| **ScalarBenchmark**     | **11444777000161** | **23.9060 ns** | **0.1136 ns** | **0.1063 ns** |
| SimdBenchmark       | 11444777000161 |  8.7168 ns | 0.0163 ns | 0.0153 ns |
| SimdBenchmarkNewApi | 11444777000161 |  8.7205 ns | 0.0156 ns | 0.0138 ns |
| **ScalarBenchmark**     | **11444777000165** | **23.9006 ns** | **0.0770 ns** | **0.0683 ns** |
| SimdBenchmark       | 11444777000165 |  8.7007 ns | 0.0353 ns | 0.0330 ns |
| SimdBenchmarkNewApi | 11444777000165 |  8.7110 ns | 0.0276 ns | 0.0258 ns |
| **ScalarBenchmark**     | **21./+...++/...** |  **2.6684 ns** | **0.0438 ns** | **0.0409 ns** |
| SimdBenchmark       | 21./+...++/... |  2.8441 ns | 0.0134 ns | 0.0126 ns |
| SimdBenchmarkNewApi | 21./+...++/... |  2.9228 ns | 0.0356 ns | 0.0297 ns |
| **ScalarBenchmark**     | **214657898456.+** |  **7.9028 ns** | **0.0301 ns** | **0.0282 ns** |
| SimdBenchmark       | 214657898456.+ |  2.8303 ns | 0.0138 ns | 0.0129 ns |
| SimdBenchmarkNewApi | 214657898456.+ |  2.8730 ns | 0.0112 ns | 0.0104 ns |
| **ScalarBenchmark**     | **214657898456LH** |  **9.7942 ns** | **0.1618 ns** | **0.1513 ns** |
| SimdBenchmark       | 214657898456LH |  2.7782 ns | 0.0078 ns | 0.0065 ns |
| SimdBenchmarkNewApi | 214657898456LH |  2.8014 ns | 0.0119 ns | 0.0111 ns |
| **ScalarBenchmark**     | **21ABCDFERGEsdf** |  **3.3375 ns** | **0.0149 ns** | **0.0125 ns** |
| SimdBenchmark       | 21ABCDFERGEsdf |  2.7973 ns | 0.0103 ns | 0.0096 ns |
| SimdBenchmarkNewApi | 21ABCDFERGEsdf |  2.8004 ns | 0.0105 ns | 0.0098 ns |
| **ScalarBenchmark**     | **321**            |  **0.0109 ns** | **0.0037 ns** | **0.0034 ns** |
| SimdBenchmark       | 321            |  0.2792 ns | 0.0189 ns | 0.0168 ns |
| SimdBenchmarkNewApi | 321            |  0.2569 ns | 0.0313 ns | 0.0262 ns |

11444777000161 é um CNPJ válido

11444777000101 possui o 1º dígito verificador inválido

11444777000165 possui o 2º dígito verificador inválido
