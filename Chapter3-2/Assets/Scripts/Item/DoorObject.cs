using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : MonoBehaviour, IInteractable
{
    public Animator Dooranim;
    public DoorData doorData;
    public string GetInteractPrompt()
    {
        string str = $"{doorData.DisplayName}\n{doorData.Description}";
        return str;
    }

    public void OnInteract()
    {
        Dooranim.SetBool("Open", true);
    }
}
