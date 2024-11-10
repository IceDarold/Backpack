using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class BagForBag : MonoBehaviour
{
    [SerializeField]
    protected Image EnablingImage; 
    [SerializeField]
    protected Image BanningImage;

    public BagForItem BagForItem;

    public bool IsBusy = false;
    
    public void ChangeEnablingImageActivity(bool ativity)
    {
        EnablingImage.enabled = ativity;
    }

    public void ChangeBanningImageActivity(bool ativity)
    {
        BanningImage.enabled = ativity;
    }
}
