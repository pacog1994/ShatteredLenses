using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Item")]
public class Item : ScriptableObject {

    [TextArea]
    public string description;
    public string itemName = "New Item";
    public Sprite sprite;
    
}
