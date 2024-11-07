using UnityEngine;

[CreateAssetMenu(fileName = "BaseItemSettings", menuName = "Settings/BaseItemSettings")]
public class BaseItemSettings : ScriptableObject
{
    public PlaceForBags PlaceForBags;
    public float MinDistanceForMove = 0.1f;
    public Vector3 OffsetFromMOuse = new Vector3(0, 80, 0);
    public float ForceMagnitude = 100f;
    public float timeAfterDown = 1.5f;
}
