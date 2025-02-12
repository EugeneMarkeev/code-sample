using Domain.Common;
using UnityEngine;

namespace Application.Utils
{
    public static class MathExtensions
    {
        public static Position2D ToPosition2D(this Vector2 vector)
        {
            return new Position2D(vector.x, vector.y);
        }

        public static Vector2 ToVector2(this Position2D position)
        {
            return new Vector2(position.X, position.Y);
        }
    }
}