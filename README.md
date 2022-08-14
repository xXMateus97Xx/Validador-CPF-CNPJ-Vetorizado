# Validador CPF e CNPJ Vetorizado

Validador de CPF e CNPJ utilizando Vector e Intrinsícos do C#

# Benchmark Validação CPF

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|          Method |         Cpf |       Mean |     Error |    StdDev |
|---------------- |------------ |-----------:|----------:|----------:|
| **ScalarBenchmark** | **11111111111** | **12.1232 ns** | **0.0568 ns** | **0.0503 ns** |
|   SimdBenchmark | 11111111111 |  4.0264 ns | 0.0060 ns | 0.0050 ns |
| **ScalarBenchmark** | **12.-...+./.** |  **8.2147 ns** | **0.0169 ns** | **0.0149 ns** |
|   SimdBenchmark | 12.-...+./. |  4.5436 ns | 0.0211 ns | 0.0198 ns |
| **ScalarBenchmark** |         **123** |  **0.6122 ns** | **0.0028 ns** | **0.0025 ns** |
|   SimdBenchmark |         123 |  0.4724 ns | 0.0020 ns | 0.0018 ns |
| **ScalarBenchmark** | **12CABCABCAB** |  **8.1860 ns** | **0.0112 ns** | **0.0099 ns** |
|   SimdBenchmark | 12CABCABCAB |  4.2979 ns | 0.0108 ns | 0.0101 ns |
| **ScalarBenchmark** | **529982247+.** |  **8.2161 ns** | **0.0353 ns** | **0.0330 ns** |
|   SimdBenchmark | 529982247+. |  4.5369 ns | 0.0103 ns | 0.0091 ns |
| **ScalarBenchmark** | **52998224715** |  **8.3093 ns** | **0.0711 ns** | **0.0555 ns** |
|   SimdBenchmark | 52998224715 |  6.8496 ns | 0.0658 ns | 0.0615 ns |
| **ScalarBenchmark** | **52998224721** | **15.5790 ns** | **0.1482 ns** | **0.1313 ns** |
|   SimdBenchmark | 52998224721 |  9.8622 ns | 0.0534 ns | 0.0474 ns |
| **ScalarBenchmark** | **52998224725** | **15.6126 ns** | **0.1619 ns** | **0.1514 ns** |
|   SimdBenchmark | 52998224725 |  9.8357 ns | 0.0241 ns | 0.0214 ns |
| **ScalarBenchmark** | **529982247AB** |  **8.2452 ns** | **0.0289 ns** | **0.0241 ns** |
|   SimdBenchmark | 529982247AB |  4.3122 ns | 0.0062 ns | 0.0052 ns |

52998224725 é um CPF válido

52998224715 possui o 1º dígito verificador inválido

52998224721 possui o 2º dígito verificador inválido

# Benchmark validação CNPJ

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


```
|          Method |           Cnpj |       Mean |     Error |    StdDev |
|---------------- |--------------- |-----------:|----------:|----------:|
| **ScalarBenchmark** | **11444777000101** | **16.9780 ns** | **0.1704 ns** | **0.1594 ns** |
|   SimdBenchmark | 11444777000101 |  6.0525 ns | 0.0071 ns | 0.0067 ns |
| **ScalarBenchmark** | **11444777000161** | **26.5910 ns** | **0.1867 ns** | **0.1747 ns** |
|   SimdBenchmark | 11444777000161 |  8.2820 ns | 0.0097 ns | 0.0076 ns |
| **ScalarBenchmark** | **11444777000165** | **26.4659 ns** | **0.0258 ns** | **0.0229 ns** |
|   SimdBenchmark | 11444777000165 |  8.2897 ns | 0.0234 ns | 0.0219 ns |
| **ScalarBenchmark** | **21./+...++/...** |  **2.8657 ns** | **0.0440 ns** | **0.0412 ns** |
|   SimdBenchmark | 21./+...++/... |  2.7306 ns | 0.0050 ns | 0.0046 ns |
| **ScalarBenchmark** | **214657898456.+** |  **8.8697 ns** | **0.0980 ns** | **0.0916 ns** |
|   SimdBenchmark | 214657898456.+ |  2.8886 ns | 0.0068 ns | 0.0063 ns |
| **ScalarBenchmark** | **214657898456LH** |  **8.5899 ns** | **0.1417 ns** | **0.1256 ns** |
|   SimdBenchmark | 214657898456LH |  2.6268 ns | 0.0076 ns | 0.0067 ns |
| **ScalarBenchmark** | **21ABCDFERGEsdf** |  **2.7215 ns** | **0.0674 ns** | **0.0597 ns** |
|   SimdBenchmark | 21ABCDFERGEsdf |  2.6164 ns | 0.0155 ns | 0.0145 ns |
| **ScalarBenchmark** |            **321** |  **0.4685 ns** | **0.0097 ns** | **0.0086 ns** |
|   SimdBenchmark |            321 |  0.2702 ns | 0.0043 ns | 0.0034 ns |

11444777000161 é um CNPJ válido

11444777000101 possui o 1º dígito verificador inválido

11444777000165 possui o 2º dígito verificador inválido
