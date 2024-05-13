using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.BenchMark.Core;
public class BenchMarkService
{
    ApplicationDbContext context = new();
    [Benchmark(Baseline = true)]
    public async Task ToListAsync()
    {
        await context.shoppingCarts.ToListAsync();
    }

    [Benchmark]
    public async Task ToList()
    {
        context.shoppingCarts.ToList();
    }
}
