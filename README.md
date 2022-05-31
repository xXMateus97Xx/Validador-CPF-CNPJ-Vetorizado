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
| **ScalarBenchmark** | **11111111111** | **12.2788 ns** | **0.1627 ns** | **0.1522 ns** |
|   SimdBenchmark | 11111111111 |  4.0253 ns | 0.0082 ns | 0.0076 ns |
| **ScalarBenchmark** | **12.-...+./.** |  **8.0388 ns** | **0.0063 ns** | **0.0053 ns** |
|   SimdBenchmark | 12.-...+./. |  4.6106 ns | 0.0041 ns | 0.0039 ns |
| **ScalarBenchmark** |         **123** |  **0.5976 ns** | **0.0010 ns** | **0.0009 ns** |
|   SimdBenchmark |         123 |  0.2309 ns | 0.0009 ns | 0.0007 ns |
| **ScalarBenchmark** | **12CABCABCAB** |  **8.0657 ns** | **0.0148 ns** | **0.0131 ns** |
|   SimdBenchmark | 12CABCABCAB |  4.2983 ns | 0.0024 ns | 0.0023 ns |
| **ScalarBenchmark** | **529982247+.** |  **8.0620 ns** | **0.0063 ns** | **0.0056 ns** |
|   SimdBenchmark | 529982247+. |  4.5280 ns | 0.0023 ns | 0.0018 ns |
| **ScalarBenchmark** | **52998224715** |  **8.0887 ns** | **0.0687 ns** | **0.0574 ns** |
|   SimdBenchmark | 52998224715 |  7.1959 ns | 0.0159 ns | 0.0141 ns |
| **ScalarBenchmark** | **52998224721** | **15.2807 ns** | **0.0120 ns** | **0.0094 ns** |
|   SimdBenchmark | 52998224721 |  9.7119 ns | 0.0112 ns | 0.0094 ns |
| **ScalarBenchmark** | **52998224725** | **15.2820 ns** | **0.0097 ns** | **0.0086 ns** |
|   SimdBenchmark | 52998224725 |  9.6975 ns | 0.0057 ns | 0.0051 ns |
| **ScalarBenchmark** | **529982247AB** |  **8.0649 ns** | **0.0091 ns** | **0.0076 ns** |
|   SimdBenchmark | 529982247AB |  4.3026 ns | 0.0022 ns | 0.0021 ns |

52998224725 é um CPF válido

52998224715 possui o 1º dígito verificador inválido

52998224721 possui o 2º dpigito verificador inválido

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
| **ScalarBenchmark** | **11444777000101** |  **8.5167 ns** | **0.0041 ns** | **0.0032 ns** |
|   SimdBenchmark | 11444777000101 |  6.2884 ns | 0.0114 ns | 0.0107 ns |
| **ScalarBenchmark** | **11444777000161** | **17.9328 ns** | **0.0047 ns** | **0.0039 ns** |
|   SimdBenchmark | 11444777000161 |  8.2461 ns | 0.0084 ns | 0.0070 ns |
| **ScalarBenchmark** | **11444777000165** | **18.0171 ns** | **0.0208 ns** | **0.0162 ns** |
|   SimdBenchmark | 11444777000165 |  8.2844 ns | 0.0154 ns | 0.0144 ns |
| **ScalarBenchmark** | **21./+...++/...** |  **8.4377 ns** | **0.0132 ns** | **0.0111 ns** |
|   SimdBenchmark | 21./+...++/... |  2.7177 ns | 0.0031 ns | 0.0026 ns |
| **ScalarBenchmark** | **214657898456.+** |  **8.5554 ns** | **0.0037 ns** | **0.0031 ns** |
|   SimdBenchmark | 214657898456.+ |  2.8779 ns | 0.0023 ns | 0.0019 ns |
| **ScalarBenchmark** | **214657898456LH** |  **8.5672 ns** | **0.0070 ns** | **0.0054 ns** |
|   SimdBenchmark | 214657898456LH |  2.6197 ns | 0.0057 ns | 0.0050 ns |
| **ScalarBenchmark** | **21ABCDFERGEsdf** |  **8.5765 ns** | **0.0116 ns** | **0.0103 ns** |
|   SimdBenchmark | 21ABCDFERGEsdf |  2.6156 ns | 0.0042 ns | 0.0035 ns |
| **ScalarBenchmark** |            **321** |  **0.4653 ns** | **0.0013 ns** | **0.0012 ns** |
|   SimdBenchmark |            321 |  0.2380 ns | 0.0009 ns | 0.0007 ns |

11444777000161 é um CNPJ válido

11444777000101 possui o 1º dígito verificador inválido

11444777000165 possui o 2º dígito verificador inválido
