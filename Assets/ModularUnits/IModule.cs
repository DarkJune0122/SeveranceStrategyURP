using System.Runtime.CompilerServices;
using UnityEngine;

namespace Dark.ModularUnits
{
    public interface IModule
    {
        /// <summary>
        /// Parent of this module. Should return Null if this is root module. (See also <seealso cref="IsRootModule"/>)
        /// </summary>
        public IModule Parent { get; }
        public bool IsRootModule
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Parent == null;
        }

        /// <summary>
        /// Array of available modules. Can be Null.
        /// </summary>
        public IModule[] Modules { get; }
        /// <summary>
        /// Local unscaled position, relative to it's parent <see cref="Parent"/>
        /// </summary>
        public Vector2 RelativePosition { get; set; }
        /// <summary>
        /// Position, relative to root module. Currently this is not baked value.
        /// </summary>
        public Vector2 TotalPosition
        {
            get
            {
                if (IsRootModule)
                    return RelativePosition;

                // Iterates over all parent modules to determine total position.
                Vector2 resultPosition = RelativePosition;
                IModule higherModule = Parent;
                do
                {
                    resultPosition += higherModule.RelativePosition;
                    higherModule = higherModule.Parent;
                }
                while (higherModule == null);

                return resultPosition;
            }

            set
            {
                if (IsRootModule)
                {
                    RelativePosition = value;
                    return;
                }

                RelativePosition = value - Parent.TotalPosition;
            }
        }
    }
}
