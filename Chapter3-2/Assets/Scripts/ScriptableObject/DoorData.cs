using UnityEngine;

[CreateAssetMenu(fileName = "DoorButton", menuName = "New Button")]
public class DoorData : ScriptableObject
{
    [Header("Info")]
    public string DisplayName;
    public string Description;
}
