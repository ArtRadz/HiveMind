using UnityEngine;

public static class HexGridUtil
{
    public static Vector2Int WorldToAxial(Vector3 world, float radius, Vector3 origin = default)
    {
        float x = world.x - origin.x;
        float y = world.y - origin.y;
        float qf = (Mathf.Sqrt(3f) / 3f * x - 1f / 3f * y) / radius;
        float rf = (2f / 3f * y) / radius;
        return CubeRound(qf, rf);
    }

    public static Vector3 AxialToWorld(int q, int r, float radius, Vector3 origin = default)
    {
        float x = radius * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) * 0.5f * r);
        float y = radius * (1.5f * r);
        return new Vector3(x + origin.x, y + origin.y, origin.z);
    }

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
        else if (dy > dz)     yi = -xi - zi;
        else                  zi = -xi - yi;
        return new Vector2Int(xi, zi);
    }

    public static MetaTile GetMetaTileUnderCursor(Camera cam, MapManager map, float radius, Vector3 origin = default)
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = -cam.transform.position.z;
        Vector3 world = cam.ScreenToWorldPoint(mouse);
        Vector2Int axial = WorldToAxial(world, radius, origin);
        GameObject go = map.MetaTiles[axial];
        return go.GetComponent<MetaTile>();
    }

    public static Vector3 GetRandomPointInCell(Vector2Int axial, float radius, Vector3 origin = default)
    {
        Vector3 center = AxialToWorld(axial.x, axial.y, radius, origin);
        float halfWidth = Mathf.Sqrt(3f) * radius * 0.5f;
        float halfHeight = radius;
        float dx = Random.Range(-halfWidth, halfWidth);
        float dy = Random.Range(-halfHeight, halfHeight);
        return new Vector3(center.x + dx, center.y + dy, center.z);
    }
}
