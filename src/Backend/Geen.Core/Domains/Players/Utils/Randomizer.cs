using System;
using System.Threading;

namespace Geen.Core.Domains.Players.Utils;

public static class Randomizer
{
    private static int _seed;

    private static readonly ThreadLocal<Random> _random = new(() => new Random(Interlocked.Increment(ref _seed)));

    public static Random RandomLocal => _random.Value;
}