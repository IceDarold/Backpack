using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VideoInventory : MonoBehaviour
{
    public VideoBaseData Data;

    public List<ItemInventory> Items = new List<ItemInventory>();

    public GameObject GameObjShow;

    public GameObject InventoryMainObject;
    public int MaxCount;

    public Camera camera;
    public EventSystem EV;

    public int currentID;
    public ItemInventory currentItem;

    public RectTransform movingObject;
    public Vector3 offset;


    public void AddItem(int id, Item item, int count)
    {
        Items[id].Id = item.Id;
        Items[id].Count = count;
        Items[id].ItemGameObject.GetComponent<Image>().sprite = item.Image;

        if(count > 1 && item.Id != 0)
        {
            Items[id].ItemGameObject.GetComponentInChildren<Text>().text = count.ToString();
        }
        else
        {
            Items[id].ItemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddInventoryItem(int id, ItemInventory invitem)
    {
        Items[id].Id = invitem.Id;
        Items[id].Count = invitem.Count;
        //Items[id].ItemGameObject.GetComponent<Image>().sprite = Data.items[invitem.Id].image;

        if (invitem.Count > 1 && invitem.Id != 0)
        {
            Items[id].ItemGameObject.GetComponentInChildren<Text>().text = invitem.Count.ToString();
        }
        else
        {
            Items[id].ItemGameObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddGrathivs()
    {
        for(int i = 0;  i < MaxCount; i++)
        {
            GameObject newitem = Instantiate(GameObjShow, InventoryMainObject.transform) as GameObject;

            newitem.name = i.ToString();

            ItemInventory ii = new ItemInventory();
            ii.ItemGameObject = newitem;

            RectTransform rt = newitem.GetComponent<RectTransform>();
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            newitem.GetComponentInChildren<RectTransform>().localScale = Vector3.one;

            Button tempButton = newitem.GetComponent <Button>();

            Items.Add(ii);
        }
    }

    public void SelectObject()
    {
        if(currentID == -1)
        {

        }
        else
        {

        }
    }

    public void MoveObject()
    {
        Vector3 pos = Input.mousePosition + offset;
        pos.z = InventoryMainObject.GetComponent<RectTransform>().position.z;
        movingObject.position = camera.ScreenToWorldPoint(pos);
    }

    public void CopyInventoryItem()
    {

    }
}

[System.Serializable]
public class ItemInventory 
{
    public GameObject ItemGameObject;
    public int Id;

    public int Count;
}
