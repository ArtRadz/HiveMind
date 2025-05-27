using UnityEngine;

public static class HexGridUtil
{
    // --------------------------------------------------------------------------------–
    // Public API
    // --------------------------------------------------------------------------------–

    /// <param name="world">World-space point (X,Y) or (X,Z).</param>
    /// <param name="radius">Hex radius (centre → flat edge).</param>
    /// <param name="origin">Optional board origin in world space.</param>
    /// <param name="useXZ">True if your game uses X-Z plane instead of X-Y.</param>
    public static Vector2Int WorldToAxial(
        Vector3 world,
        float   radius,
        Vector3 origin   = default,
        bool    useXZ    = false)
    {
        // Translate so (0,0) grid sits at 'origin'
        float x = world.x - origin.x;
        float y = (useXZ ? world.z : world.y) - (useXZ ? origin.z : origin.y);

        // --- fractional axial coords (flat-top) -------------------------------
        float qf = (Mathf.Sqrt(3f) / 3f * x - 1f / 3f * y) / radius;
        float rf = (                    2f / 3f * y)        / radius;

        // --- snap to nearest integer hex using cube-round ---------------------
        return CubeRound(qf, rf);
    }

    /// <summary>Axial (q,r) → Unity world position.</summary>
    public static Vector3 AxialToWorld(
        int     q,
        int     r,
        float   radius,
        Vector3 origin   = default,
        bool    useXZ    = false)
    {
        float x = radius * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) * 0.5f * r);
        float y = radius * (1.5f * r);

        return useXZ
            ? new Vector3(x + origin.x, origin.y, y + origin.z) // X-Z plane
            : new Vector3(x + origin.x, y + origin.y, origin.z); // X-Y plane
    }

    // --------------------------------------------------------------------------------–
    // Internals
    // --------------------------------------------------------------------------------–

    /// <summary>Rounds fractional cube coords to the nearest legal axial tile.</summary>
    private static Vector2Int CubeRound(float qf, float rf)
    {
        float xf = qf;
        float zf = rf;
        float yf = -xf - zf;

        int xi = Mathf.RoundToInt(xf);
        int yi = Mathf.RoundToInt(yf);
        int zi = Mathf.RoundToInt(zf);

        float dx = Mathf.Abs(xi - xf);
        float dy = Mathf.Abs(yi - yf);
        float dz = Mathf.Abs(zi - zf);

        if (dx > dy && dx > dz) xi = -yi - zi;
        else if (dy > dz)       yi = -xi - zi;
        else                    zi = -xi - yi;

        // axial uses (q = x, r = z)
        return new Vector2Int(xi, zi);
    }
}