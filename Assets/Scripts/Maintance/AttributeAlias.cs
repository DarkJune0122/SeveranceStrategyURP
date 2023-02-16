using System;

namespace SeveranceStrategy.Maintance
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AttributeAlias : Attribute
    {
        public readonly string alias;

        // This is a positional argument
        public AttributeAlias(IAttributeHolder<AttributeAlias> aliasHolder, string alias)
        {
            this.alias = alias;
            aliasHolder.Attributes.Add(alias, this);

            throw new NotImplementedException("A");
        }
    }
}
