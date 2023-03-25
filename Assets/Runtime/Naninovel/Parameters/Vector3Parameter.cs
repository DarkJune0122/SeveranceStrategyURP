/*using Naninovel;
using System;
using UnityEngine;

namespace SeveranceStrategy.Naninovel.Parameters
{
    /// <summary>
    /// Represents a serializable <see cref="Command"/> parameter with <see cref="string"/> value.
    /// </summary>
    [Serializable]
    public sealed class Vector3Parameter : CommandParameter<Vector3>
    {
        public static implicit operator Vector3Parameter(Vector3 vector) => new() { Value = vector };
        public static implicit operator Vector3(Vector3Parameter param) => param is null || !param.HasValue ? Vector3.zero : param.Value;

        protected override Vector3 ParseValueText(string valueText, out string errors)
        {
            string[] values = valueText.Split(Core.Environment.VectorValuesSeparator);
            if (values.Length != 3)
            {
                errors = $"Provided string have more values than it's possible to handle";
            }
            else errors = null;

            Vector3 position = new();
            if (!float.TryParse(values[0], out position.x)) errors += $"Unable to parse given string into {nameof(Vector3)} X value: {values[0]}";
            if (!float.TryParse(values[1], out position.y)) errors += $"Unable to parse given string into {nameof(Vector3)} Y value: {values[1]}";
            if (!float.TryParse(values[2], out position.z)) errors += $"Unable to parse given string into {nameof(Vector3)} Z value: {values[2]}";
            return position;
        }
    }
}
*/