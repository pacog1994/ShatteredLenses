using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ItemDatabase : MonoBehaviour {

   
    public const int numItemSlots = 4; //number of item slots
    public Image[] itemImages = new Image[numItemSlots]; //The item image
    public Item[] items = new Item[numItemSlots];

    //Add items from inventory
    public void AddItem(Item itemToAdd)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i]  == null)
            {
                items[i] = itemToAdd;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                return;
            }
        }
    }
    //Removes item from inventory
    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                return;
            }
        }
    }

    //Find by id in database
    public bool FindItem(Item itemToFind)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == itemToFind)
            {
                return true;
            }
        }
        return false;
       
    }
    //Getter in database
    public Item getItem(Item itemToGet)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == itemToGet)
            {
                return items[i];
            }
        }
        return null;
    }
    //Clear
    public void Clear()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
            }
        }
        return;
    }
}
