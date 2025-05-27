using UnityEngine;

/// <summary>
/// Flat-top hex grid helpers (XY plane).
///   • World <--> axial (q,r)
///   • Stateless: caller supplies radius & origin every time.
/// </summary>
public static class HexGridUtil
{
    /* ────────────────────────────────────────────────────────────────
     *  World (XY) ➜ Axial
     * ──────────────────────────────────────────────────────────────── */
    public static Vector2Int WorldToAxial(
        Vector3 world,
        float   radius,
        Vector3 origin = default)          // (0,0,0) if you built the grid at world-zero
    {
        // shift into board-local space
        float x = world.x - origin.x;
        float y = world.y - origin.y;

        // fractional axial coords (flat-top)
        float qf = (Mathf.Sqrt(3f) / 3f *  x - 1f / 3f * y) / radius;
        float rf = (                           2f / 3f * y) / radius;

        return CubeRound(qf, rf);              // snap to nearest hex
    }

    /* ────────────────────────────────────────────────────────────────
     *  Axial ➜ World (XY)
     * ──────────────────────────────────────────────────────────────── */
    public static Vector3 AxialToWorld(
        int     q,
        int     r,
        float   radius,
        Vector3 origin = default)
    {
        float x = radius * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) * 0.5f * r);
        float y = radius * (1.5f * r);

        return new Vector3(x + origin.x, y + origin.y, origin.z);
    }

    /* ────────────────────────────────────────────────────────────────
     *  Internal helper – cube rounding
     * ──────────────────────────────────────────────────────────────── */
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
