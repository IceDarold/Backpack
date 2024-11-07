using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBag : BaseItem
{
    private BagForItem[] BagsForItem;

    protected override bool CheckBags()
    {
        for (int x = 0; x < form.GetLength(1); x++)
        {
            for (int y = 0; y < form.GetLength(0); y++)
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

    protected override void CreateArrayForColorCells(bool isRed)
    {
        for (int x = 0; x < form.GetLength(1); x++)
        {
            for (int y = 0; y < form.GetLength(0); y++)
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


                newBags.Add(new Vector3(row, column, Convert.ToInt32(isRed)));
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
}
