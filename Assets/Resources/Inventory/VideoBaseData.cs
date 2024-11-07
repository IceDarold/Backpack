using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class VideoBaseData : MonoBehaviour
{
    public List<Item> Items = new List<Item>();
}

[System.Serializable]
public class Item 
{
    public int Id;
    public string Name;
    public Sprite Image;
}

