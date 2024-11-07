using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

public class BagForBag : MonoBehaviour
{
    [SerializeField]
    private Image EnablingImage; 
    [SerializeField]
    private Image BanningImage;

    public bool IsBusy = false;
    public BagForItem BagForItem;
    
    public void ChangeEnablingImageActivity(bool ativity)
    {
        EnablingImage.enabled = ativity;
    }

    public void ChangeBanningImageActivity(bool ativity)
    {
        BanningImage.enabled = ativity;
    }
}
