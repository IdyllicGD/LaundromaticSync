using UnityEngine;
using System;

public class LSapi : MonoBehaviour
{
    public static LSapi instance;

    readonly System.Random random = new();

    void Start()
    {
        instance = this;
    }
    public int ReturnRandomInt(int min, int max)
    {
        int rnd = random.Next(min, max);
        return rnd;
    }
}
