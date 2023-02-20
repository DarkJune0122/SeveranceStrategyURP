using System.Collections.Generic;

namespace SeveranceStrategy
{
    public static class Constants
    {
        public static readonly HashSet<char> blockedSymbols = new()
        {
            ';', '/', '\\',
        };
    }
}
