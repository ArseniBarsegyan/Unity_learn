using UnityEngine;

public class MapData : MonoBehaviour
{
    public int Width = 10;
    public int Height = 5;

    public int[,] MakeMap()
    {
        int[,] map = new int[Width, Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                map[x, y] = 0;
            }
        }

        map[1, 0] = 1;
        map[1, 1] = 1;
        map[1, 2] = 1;

        map[4, 1] = 1;
        map[4, 2] = 1;
        map[4, 3] = 1;
        map[4, 4] = 1;
        map[5, 1] = 1;
        map[6, 1] = 1;

        map[9, 0] = 1;
        map[9, 1] = 1;
        map[9, 2] = 1;
        map[9, 3] = 1;

        return map;
    }
}
