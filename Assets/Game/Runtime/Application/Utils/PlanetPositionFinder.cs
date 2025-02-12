using System.Collections.Generic;
using UnityEngine;

namespace Application.Utils
{
    public class PlanetPositionFinder
    {
        public static Vector2 FindValidRelativePosition(List<Vector2> existingPositions, float planetRadiusRelative,
            float minDistanceFactor = 1f, int maxAttempts = 2000)
        {
            var safeRect = new Rect(planetRadiusRelative * 1.1f, planetRadiusRelative * 1.1f,
                1 - planetRadiusRelative * 1.4f, 1 - planetRadiusRelative * 1.4f);

            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                var candidate = new Vector2(
                    Random.Range(safeRect.x, safeRect.width),
                    Random.Range(safeRect.y, safeRect.height));

                var isValid = true;
                foreach (var pos in existingPositions)
                    if (Vector2.Distance(candidate, pos) < planetRadiusRelative * 2 * minDistanceFactor)
                    {
                        isValid = false;
                        break;
                    }

                if (isValid) return candidate;
            }

            return Vector2.one*0.5f;
        }
    }
}