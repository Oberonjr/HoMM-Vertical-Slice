using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "Interactables/Resources")]
public class ResourceData : ScriptableObject
{
    public enum ResourceType
    {
        Gold,
        Wood,
        Ore,
        Crystal
    }

    public ResourceType Resource;
    public Sprite BigIcon;
    public Sprite SmallIcon;
}
