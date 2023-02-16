using System.Collections.Generic;

namespace SeveranceStrategy.Maintance
{
    public interface IAttributeHolder<TAttribute> where TAttribute : AttributeAlias
    {
        public Dictionary<string, TAttribute> Attributes { get; }
    }
}
