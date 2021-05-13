using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceColliderType : MonoBehaviour
{
    public enum Mode { Default, Grass, Wood, Car }
    public Mode terrainType;

    public string GetTerrainType()
    {
        string typeString = "";

        switch (terrainType)
        {
            case Mode.Default:
                typeString = "Default";
                break;
            case Mode.Grass:
                typeString = "Grass";
                break;
            case Mode.Wood:
                typeString = "Wood";
                break;
            case Mode.Car:
                typeString = "Car";
                break;
            default:
                typeString = "";
                break;
        }

        return typeString;
    }
}
