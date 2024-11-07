using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BaseItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Serializable]
    public class ForForm
    {
        public List<bool> LineForm;
    }

    [SerializeField]
    protected List<ForForm> transientForm;

    protected bool[,] form; // Array is not filled in from the inspector

    protected BaseItemSettings settings;

    public bool isButtonPressed = false;

    protected Rigidbody2D rb;
    protected bool isButtonMoving = false;
    protected Vector3 lastMousePosition;
    protected float timeAfterDown = 0;
    protected bool canBeInstalled = false;
    protected List<Vector3> lastBags = new List<Vector3>(0);    // z == 1 => Enabling;  z == 0 => Banning; z == -1 => BagForBag-Banning 
    protected List<Vector3> newBags = new List<Vector3>(0);       // ^^^ 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        form = new bool[transientForm.Count, transientForm[0].LineForm.Count];
        for(int i = 0; i < form.GetLength(0); i++)
        {
            for(int j = 0; j < form.GetLength(1); j++)
            {
                form[i, j] = transientForm[i].LineForm[j];
            }
        }


        if (settings == null)
        {
            settings = Resources.Load<BaseItemSettings>("Inventory/Data/BaseItemSettings");

        }
    }
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
                CreateArrayForColorCells(canBeInstalled);

                ColorAndDeColorCells();
            }
            else
            {
                timeAfterDown += Time.deltaTime;
                if(Vector3.Distance(lastMousePosition, Input.mousePosition) >= settings.MinDistanceForMove || timeAfterDown >= settings.timeAfterDown)
                {
                    rb.MoveRotation(rb.rotation + (rb.rotation % 90 >= 45 ? (90 - rb.rotation) : (-rb.rotation)));
                    Debug.Log((rb.rotation % 90 >= 45 ? (90 - rb.rotation % 90) : (-rb.rotation % 90)));
                    rb.freezeRotation = true;

                    timeAfterDown = 0;
                    isButtonMoving = true;

                    RemoveToBag();
                }
            }


        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
        lastMousePosition = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rb.freezeRotation = false;

        ColorAndDeColorCells();

        GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;

        if (canBeInstalled)
        {
            InstalInBag();
        }
        else
        {
            GetComponentInParent<Rigidbody2D>().AddForce((Input.mousePosition - lastMousePosition) * settings.ForceMagnitude, ForceMode2D.Impulse);
        }

        isButtonPressed = false;
        isButtonMoving = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //GetInfo
    }

    protected virtual bool CheckBags()
    {
        for(int x = 0; x < form.GetLength(1); x++)
        {
            for(int y = 0; y < form.GetLength(0); y++)
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

                if (!settings.PlaceForBags.BagsForBag[row, column].IsBusy)
                {
                    return false;
                }
            }
        }
        
        return true;
    }

    protected virtual void CreateArrayForColorCells(bool isRed)
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

                if (settings.PlaceForBags.BagsForBag[row, column].BagForItem == null)
                {
                    newBags.Add(new Vector3(row, column, -1));
                    continue;
                }

                if (isRed)
                {
                    newBags.Add(new Vector3(row, column, 0));
                }
                else
                {
                    newBags.Add(new Vector3(row, column, 1));
                }
            }
        }
    }

    protected virtual void ColorAndDeColorCells()
    {
        foreach (var e in lastBags) {
            bool a = false;
            foreach (var newe in newBags)
            {
                if(e == newe)
                {
                    a = true;
                    break;
                }
            }

            if (a) continue;

            if (e.z == 1)
            {
                settings.PlaceForBags.BagsForBag[(int)e.x, (int)e.y].BagForItem.ChangeEnablingImageActivity(false);
            }
            else if(e.z == 0) {
                settings.PlaceForBags.BagsForBag[(int)e.x, (int)e.y].BagForItem.ChangeBanningImageActivity(false);
            }
            else
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
                settings.PlaceForBags.BagsForBag[(int)e.x, (int)e.y].BagForItem.ChangeEnablingImageActivity(true);
            }
            else if (e.z == 0)
            {
                settings.PlaceForBags.BagsForBag[(int)e.x, (int)e.y].BagForItem.ChangeBanningImageActivity(true);
            }
            else
            {
                settings.PlaceForBags.BagsForBag[(int)e.x, (int)e.y].ChangeBanningImageActivity(true);
            }
        }

        lastBags = newBags;
        newBags = new List<Vector3>(0);
    }

    protected void RemoveToBag()
    {

    }

    protected void InstalInBag()
    {

    }
}
