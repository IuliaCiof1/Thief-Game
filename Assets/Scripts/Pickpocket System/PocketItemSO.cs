using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="PocketItem")]
public class PocketItemSO : ScriptableObject
{
    public Sprite sprite;
    public int value;

    public bool canBeOwned;
    public InventoryItem ownedObject;
}
