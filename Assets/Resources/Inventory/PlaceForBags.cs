using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlaceForBags;

public class PlaceForBags : MonoBehaviour
{
    [Serializable]
    public class ItemPosition
    {
        //Very important!!! fill in from left to right, from bottom to top. z - rotation
        public List<Vector3> IP;
    }
    [Tooltip("Very important!!! fill in from left to right, from bottom to top. z - rotation")]
    public List<ItemPosition> InitialItemPositions;
    public List<BaseItem> InitialItems;


    [SerializeField]
    private BaseItemSettings settings;
    [SerializeField]
    private Transform EquipmentParent;
    [SerializeField]
    private Transform BagParent;
    [SerializeField]
    private Transform ItemParent;
    [SerializeField]
    public int lenx = 1;
    [SerializeField]
    public int leny = 1;

    [SerializeField]
    private GameObject BagForBagPrefab;

    [SerializeField]
    public BagForBag[,] BagsForBag;

    public float SideX;
    public float SideY;

    private void Awake()
    {
        SetBaseItemSettings();

        BagsForBag = new BagForBag[leny, lenx];

        this.SideX = this.GetComponent<RectTransform>().rect.width / lenx;
        SideY = this.GetComponent<RectTransform>().rect.height / leny;

        BagForBagPrefab.transform.localScale = new Vector3(SideX, SideY, 0);

        for (int i = 0; i < leny; i++)
        {
            for(int j = 0; j < lenx; j++)
            {
                BagsForBag[i, j] = Instantiate(BagForBagPrefab, new Vector3(this.SideX / 2 + j * this.SideX, this.GetComponent<RectTransform>().rect.height / 2 + i * SideY, 0), Quaternion.identity, transform).GetComponent<BagForBag>();
            }
        }
    }

    private void Start()
    {
        InstallItems(InitialItemPositions, InitialItems);
    }
    private void InstallItems(List<ItemPosition> itemPositions, List<BaseItem> items)
    {
        for (int i = 0;i < itemPositions.Count;i++)
        {
            items[i].InstallInThisLocations(itemPositions[i].IP);
        }
    }

    private void SetBaseItemSettings()
    {
        settings.PlaceForBags = this;
        settings.EquipmentParent = EquipmentParent;
        settings.BagParent = BagParent;
        settings.ItemParent = ItemParent;
    }
}
