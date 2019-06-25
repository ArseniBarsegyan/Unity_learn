using System;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public int width = 10;
    public int height = 5;

    public TextAsset TextAsset;

    public List<string> GetTextFromFile(TextAsset textAsset)
    {
        var lines = new List<string>();

        if (textAsset != null)
        {
            string textData = textAsset.text;
            string[] delimiters = {"\r\n", "\n"};
            lines.AddRange(textData.Split(delimiters, StringSplitOptions.None));
            lines.Reverse();
        }

        else
        {
            Debug.LogWarning("MAPDATA GetTextFromFile Error: invalid text asset");
        }
        return lines;
    }

    public List<string> GetTextFromFile()
    {
        return GetTextFromFile(TextAsset);
    }

    public void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;
        
        foreach (var line in textLines)
        {
            if (line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    public int[,] MakeMap()
    {
        var lines = GetTextFromFile();
        SetDimensions(lines);

        int[,] map = new int[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (lines[y].Length > x)
                {
                    map[x, y] = (int)char.GetNumericValue(lines[y][x]);
                }
            }
        }

        return map;
    }
}
