using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBag : BaseItem
{
    [Serializable]
    public class ListBag
    {
        public List<BagForItem> Bags;
    }
    [SerializeField]
    private List<ListBag> BagsForItem;

    void Update()
    {
        if (isButtonPressed)
        {
            if (isButtonMoving)
            {
                Vector3 newPosition = Input.mousePosition + settings.OffsetFromMOuse;
                rb.MovePosition(newPosition);
                lastMousePosition = Input.mousePosition;

                canBeInstalled = CheckBags();
                CreateArrayForColorCells();

                ColorAndDeColorCells();
            }
            else
            {
                timeAfterDown += Time.deltaTime;
                if (Vector3.Distance(lastMousePosition, Input.mousePosition) >= settings.MinDistanceForMove || timeAfterDown >= settings.timeAfterDown)
                {
                    for (int i = 0; i < BagsForItem.Count; i++)
                    {
                        for (int j = 0; j < BagsForItem[i].Bags.Count; j++)
                        {
                            if (BagsForItem[i].Bags[j].IsBusy) return;
                        }
                    }
                    CheckRotation();

                    rb.freezeRotation = true;

                    timeAfterDown = 0;
                    isButtonMoving = true;

                    transform.SetParent(settings.ItemParent);

                    if (isLoñk) RemoveToBag();
                }
            }


        }
    }
    protected override void CheckRotation()
    {
        //rb.MoveRotation(rb.rotation + (rb.rotation % 90 >= 45 ? (90 - rb.rotation % 90) : (-rb.rotation % 90)));
        rb.rotation %= 360;
        if (rb.rotation < 0) rb.rotation += 360;
        rb.rotation = rb.rotation + (rb.rotation % 90 >= 45 ? (90 - rb.rotation % 90) : (-rb.rotation % 90));

        if (Math.Abs(lastRotation - rb.rotation) == 90)
        {
            bool[,] newForm = new bool[form.GetLength(1), form.GetLength(0)];
            for (int i = 0; i < newForm.GetLength(0); i++)
            {
                for (int j = 0; j < newForm.GetLength(1); j++)
                {
                    newForm[i, j] = form[j, i];
                }
            }
            form = newForm;
        }
        else if (Math.Abs(lastRotation - rb.rotation) == 270)
        {
            bool[,] newForm = new bool[form.GetLength(1), form.GetLength(0)];
            for (int i = 0; i < newForm.GetLength(0); i++)
            {
                for (int j = 0; j < newForm.GetLength(1); j++)
                {
                    newForm[i, j] = form[j, form.GetLength(1) - i - 1];
                }
            }
            form = newForm;
        }
        else if (Math.Abs(lastRotation - rb.rotation) == 180)
        {
            bool[,] newForm = new bool[form.GetLength(0), form.GetLength(1)];
            for (int i = 0; i < newForm.GetLength(0); i++)
            {
                for (int j = 0; j < newForm.GetLength(1); j++)
                {
                    newForm[i, j] = form[form.GetLength(0) - i - 1, j];
                }
            }
            form = newForm;
        }

        lastRotation = rb.rotation;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        rb.freezeRotation = false;
        transform.SetParent(settings.BagParent);

        if (isButtonMoving)
        {
            GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;

            if (canBeInstalled)
            {
                installationLocations = lastBags.ToList();
                InstalInBag();
            }
            else
            {
                GetComponentInParent<Rigidbody2D>().AddForce((Input.mousePosition - lastMousePosition) * settings.ForceMagnitude, ForceMode2D.Impulse);
            }


            ColorAndDeColorCells();
        }

        isButtonPressed = false;
        isButtonMoving = false;
    }
    protected override bool CheckBags()
    {
        for (int y = 0; y < form.GetLength(0); y++)
        {
            for (int x = 0; x < form.GetLength(1); x++)
            {
                Vector3 pos = gameObject.GetComponent<RectTransform>().transform.position +
                    new Vector3((x - form.GetLength(1) / 2f + 0.5f) * settings.PlaceForBags.SideX, (y - form.GetLength(0) / 2f + 0.5f) * settings.PlaceForBags.SideY, 0);


                RectTransform PlaceForBagsRT = settings.PlaceForBags.gameObject.GetComponent<RectTransform>();

                if (pos.x < PlaceForBagsRT.transform.position.x - PlaceForBagsRT.rect.width / 2 || pos.x > PlaceForBagsRT.transform.position.x + PlaceForBagsRT.rect.width / 2
                    || pos.y < PlaceForBagsRT.transform.position.y - PlaceForBagsRT.rect.height / 2 || pos.y > PlaceForBagsRT.transform.position.y + PlaceForBagsRT.rect.height / 2)
                {
                    return false;
                }

                int column = (int)((pos.x - PlaceForBagsRT.position.x + PlaceForBagsRT.rect.width / 2) / settings.PlaceForBags.SideX);
                int row = (int)((pos.y - PlaceForBagsRT.position.y + PlaceForBagsRT.rect.height / 2) / settings.PlaceForBags.SideY);

                if (settings.PlaceForBags.BagsForBag[row, column].IsBusy)
                {
                    return false;
                }
            }
        }

        return true;
    }

    protected override void CreateArrayForColorCells()
    {
        for (int y = 0; y < form.GetLength(0); y++)
        {
            for (int x = 0; x < form.GetLength(1); x++)
            {
                Vector3 pos = gameObject.GetComponent<RectTransform>().transform.position +
                    new Vector3((x - form.GetLength(1) / 2f + 0.5f) * settings.PlaceForBags.SideX, (y - form.GetLength(0) / 2f + 0.5f) * settings.PlaceForBags.SideY, 0);

                RectTransform PlaceForBagsRT = settings.PlaceForBags.gameObject.GetComponent<RectTransform>();

                if (pos.x < PlaceForBagsRT.transform.position.x - PlaceForBagsRT.rect.width / 2 || pos.x > PlaceForBagsRT.transform.position.x + PlaceForBagsRT.rect.width / 2
                    || pos.y < PlaceForBagsRT.transform.position.y - PlaceForBagsRT.rect.height / 2 || pos.y > PlaceForBagsRT.transform.position.y + PlaceForBagsRT.rect.height / 2)
                {
                    continue;
                }


                int column = (int)((pos.x - PlaceForBagsRT.position.x + PlaceForBagsRT.rect.width / 2) / settings.PlaceForBags.SideX);
                int row = (int)((pos.y - PlaceForBagsRT.position.y + PlaceForBagsRT.rect.height / 2) / settings.PlaceForBags.SideY);


                newBags.Add(new Vector3(row, column, Convert.ToInt32(canBeInstalled)));
            }
        }
    }

    protected override void ColorAndDeColorCells()
    {
        foreach (var e in lastBags)
        {
            bool a = false;
            foreach (var newe in newBags)
            {
                if (e == newe)
                {
                    a = true;
                    break;
                }
            }

            if (a) continue;

            if (e.z == 1)
            {
                settings.PlaceForBags.BagsForBag[(int)e.x, (int)e.y].ChangeEnablingImageActivity(false);
            }
            else if (e.z == 0)
            {
                settings.PlaceForBags.BagsForBag[(int)e.x, (int)e.y].ChangeBanningImageActivity(false);
            }
        }

        foreach (var e in newBags)
        {
            bool a = false;
            foreach (var laste in lastBags)
            {
                if (e == laste)
                {
                    a = true;
                    break;
                }
            }

            if (a) continue;

            if (e.z == 1)
            {
                settings.PlaceForBags.BagsForBag[(int)e.x, (int)e.y].ChangeEnablingImageActivity(true);
            }
            else if (e.z == 0)
            {
                settings.PlaceForBags.BagsForBag[(int)e.x, (int)e.y].ChangeBanningImageActivity(true);
            }
        }

        lastBags = newBags;
        newBags = new List<Vector3>(0);
    }

    public override void RemoveToBag()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
        isLoñk = false;

        for (int i = 0; i < installationLocations.Count; i++)
        {
            settings.PlaceForBags.BagsForBag[(int)installationLocations[i].x, (int)installationLocations[i].y].IsBusy = false; 
        }

        /*
        foreach(var item in BagsForItem)
        {
            foreach (BagForItem bag in item.Bags)
            {
                if (bag.item != null) bag.item.RemoveToBag();
            }
        }*/
    }

    protected override void InstalInBag()
    {
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        isLoñk = true;

        Vector3 pos = settings.PlaceForBags.BagsForBag[(int)installationLocations[0].x, (int)installationLocations[0].y].GetComponent<RectTransform>().transform.position
                    + new Vector3((form.GetLength(1) / 2f - 0.5f) * settings.PlaceForBags.SideX, (form.GetLength(0) / 2f - 0.5f) * settings.PlaceForBags.SideY, 0);
        pos.z = 1;

        GetComponent<RectTransform>().transform.position = pos;

        int count = 0;
        for(int i = 0; i < BagsForItem.Count; i++)
        {
            for(int j = 0; j < BagsForItem[i].Bags.Count; j++)
            {
                settings.PlaceForBags.BagsForBag[(int)installationLocations[count].x, (int)installationLocations[count].y].IsBusy = true;
                settings.PlaceForBags.BagsForBag[(int)installationLocations[count].x, (int)installationLocations[count].y].BagForItem = BagsForItem[i].Bags[j];

                count++;
            }
        }
    }
}
