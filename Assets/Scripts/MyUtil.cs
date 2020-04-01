using System.Collections;
using System.Collections.Generic;

public static class MyUtil
{
    public static void Shuffle(bool[] array, int seed)
    {
        int length = array.Length;
        System.Random prng = new System.Random(seed);
        for (int i = 0; i < length - 1; i++)
        {
            int rndi = prng.Next(i, length);
            bool temp = array[i];
            array[i] = array[rndi];
            array[rndi] = temp;
        }
    }

    public static void Shuffle2D(bool[,] map, int seed, int xl, int yl)
    {
        int length = xl * yl;
        bool[] array1D = new bool[length];
        array1D.Initialize();
        for (int x = 0; x < xl; x++)
        {
            for (int y = 0; y < yl; y++)
            {
                array1D[x * yl + y] = map[x, y];
            }
        }
        Shuffle(array1D, seed);
        for (int i = 0; i < array1D.Length; i++)
        {
            int row = (int)i / yl;
            int col = i % yl;
            map[row, col] = array1D[i];
        }
    }

    public static bool FloodFill(bool[,] map, int rowLength, int colLength)
    {


        return true;
    }
}
