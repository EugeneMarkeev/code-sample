using System;

namespace Domain.Common
{
    [Serializable]
    public struct Position2D
    {
        public float X;
        public float Y;

        public Position2D(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}