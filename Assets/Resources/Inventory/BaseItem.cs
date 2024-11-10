using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

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
    protected List<Vector3> installationLocations = new List<Vector3>(0);
    protected float lastRotation = 0f;
    protected bool isLoñk = false;

    void Awake()
    {
        GetComponent<RectTransform>().transform.position = new Vector3(GetComponent<RectTransform>().transform.position.x, GetComponent<RectTransform>().transform.position.y, 2f);

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
                CreateArrayForColorCells();

                ColorAndDeColorCells();
            }
            else
            {
                timeAfterDown += Time.deltaTime;
                if(Vector3.Distance(lastMousePosition, Input.mousePosition) >= settings.MinDistanceForMove || timeAfterDown >= settings.timeAfterDown)
                {
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

    /// <summary>
    /// Use only at the very beginning in the start() method, z - rotation
    /// </summary>
    /// <param name="location"></param>
    public void InstallInThisLocations(List<Vector3> location)
    {
        rb.rotation = location[0].z;
        CheckRotation();

        foreach (Vector3 loc in location)
        {
            installationLocations.Add(new Vector3(loc.y, loc.x, 1));
        }

        InstalInBag();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonPressed = true;
        lastMousePosition = Input.mousePosition;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        rb.freezeRotation = false;
        transform.SetParent(settings.EquipmentParent);

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

    public void OnPointerClick(PointerEventData eventData)
    {
        //GetInfo
    }

    protected virtual void CheckRotation()
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

    protected virtual bool CheckBags()
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

                if (!settings.PlaceForBags.BagsForBag[row, column].IsBusy)
                {
                    return false;
                }
                else if(settings.PlaceForBags.BagsForBag[row, column].BagForItem.IsBusy)
                {
                    return false;
                }
            }
        }
        
        return true;
    }

    protected virtual void CreateArrayForColorCells()
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

                if (!settings.PlaceForBags.BagsForBag[row, column].IsBusy)
                {
                    newBags.Add(new Vector3(row, column, -1));
                    continue;
                }
                if (canBeInstalled)
                {
                    newBags.Add(new Vector3(row, column, 1));
                }
                else
                {
                    newBags.Add(new Vector3(row, column, 0));
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

    public virtual void RemoveToBag()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
        isLoñk = false;

        foreach (Vector3 vec in installationLocations)
        {
            if (vec.z != 1) Debug.Log("______________________________________________+++++++++++++++++++++++++++++++++++_________________");

            settings.PlaceForBags.BagsForBag[(int)vec.x, (int)vec.y].BagForItem.IsBusy = false;
            settings.PlaceForBags.BagsForBag[(int)vec.x, (int)vec.y].BagForItem.item = null;
        }
    }

    protected virtual void InstalInBag()
    {
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        isLoñk = true;

;       Vector3 pos = settings.PlaceForBags.BagsForBag[(int)installationLocations[0].x, (int)installationLocations[0].y].GetComponent<RectTransform>().transform.position
                    + new Vector3((form.GetLength(1) / 2f - 0.5f) * settings.PlaceForBags.SideX, (form.GetLength(0) / 2f - 0.5f) * settings.PlaceForBags.SideY, 0);

        GetComponent<RectTransform>().transform.position = pos;

        foreach(Vector3 vec in installationLocations)
        {
            if (vec.z != 1) Debug.Log("______________________________________________+++++++++++++++++++++++++++++++++++_________________");

            settings.PlaceForBags.BagsForBag[(int)vec.x, (int)vec.y].BagForItem.IsBusy = true;
            settings.PlaceForBags.BagsForBag[(int)vec.x, (int)vec.y].BagForItem.item = this;
        }
    }
}
