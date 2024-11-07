using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceForBags : MonoBehaviour
{
    [SerializeField]
    BaseItemSettings settings;
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
        settings.PlaceForBags = this;


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
}
