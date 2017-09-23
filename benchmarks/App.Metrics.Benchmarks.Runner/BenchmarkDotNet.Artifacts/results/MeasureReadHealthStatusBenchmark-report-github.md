``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312790 Hz, Resolution=301.8604 ns, Timer=TSC
dotnet cli version=2.0.0
  [Host] : .NET Core 4.6.00001.0, 64bit RyuJIT
  Core   : .NET Core 4.6.00001.0, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |     Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
 |----------- |---------:|----------:|----------:|-------:|----------:|
 | ReadHealth | 1.441 us | 0.0069 us | 0.0061 us | 0.1984 |     840 B |
