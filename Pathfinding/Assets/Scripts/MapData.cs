using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    public int width = 10;
    public int height = 5;

    public TextAsset TextAsset;
    public Texture2D TextureMap;
    public string ResourcePath = "Mapdata";

    void Start()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if (TextureMap == null)
        {
            TextureMap = Resources.Load<Texture2D>(ResourcePath + "/" + levelName);
        }

        if (TextAsset == null)
        {
            TextAsset = Resources.Load<TextAsset>(ResourcePath + "/" + levelName);
        }
    }

    public List<string> GetMapFromTextFile(TextAsset textAsset)
    {
        var lines = new List<string>();

        if (textAsset != null)
        {
            string textData = textAsset.text;
            string[] delimiters = {"\r\n", "\n"};
            lines.AddRange(textData.Split(delimiters, StringSplitOptions.None));
            lines.Reverse();
        }
        return lines;
    }

    public List<string> GetMapFromTextFile()
    {
        return GetMapFromTextFile(TextAsset);
    }

    public List<string> GetMapFromTexture(Texture2D texture)
    {
        var lines = new List<string>();

        if (texture != null)
        {
            for (int y = 0; y < texture.height; y++)
            {
                string newLine = string.Empty;

                for (int x = 0; x < texture.width; x++)
                {
                    if (texture.GetPixel(x, y) == Color.black)
                    {
                        newLine += '1';
                    }
                    else if (texture.GetPixel(x, y) == Color.white)
                    {
                        newLine += '0';
                    }
                    else
                    {
                        newLine += ' ';
                    }
                }

                lines.Add(newLine);
            }
        }
        
        return lines;
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
        var lines = new List<string>();

        if (TextureMap != null)
        {
            lines = GetMapFromTexture(TextureMap);
        }
        else
        {
            lines = GetMapFromTextFile();
        }

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
