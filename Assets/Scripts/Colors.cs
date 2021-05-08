using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors
{
    public static Color ToColor(string name)
    {
        Color color;
        switch (name.ToLowerInvariant())
        {
            case "red":
                color = new Color(1f, 0f, 0f, 1f);
                break;
            case "blue":
                color = new Color(0f, 0f, 1f, 1f);
                break;
            case "green":
                color = new Color(0f, 1f, 0f, 1f);
                break;
            case "white":
                color = new Color(1f, 1f, 1f, 1f);
                break;
            case "black":
                color = new Color(0f, 0f, 0f, 1f);
                break;
            case "orange":
                color = new Color(1f, 0.65f, 0f, 1f);
                break;
            case "pink":
                color = new Color(1f, 0.4f, 0.7f, 1f);
                break;
            case "purple":
                color = new Color(0.5f, 0f, 0.5f, 1f);
                break;
            case "yellow":
                color = new Color(1f, 1f, 0f, 1f);
                break;
            default:
                Debug.Log("Could not parse color. Defaulting to white");
                color = new Color(1f, 1f, 1f, 1f);
                break;
        }
        return color;
    }
}
