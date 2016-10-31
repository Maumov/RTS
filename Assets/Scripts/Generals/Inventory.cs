using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Inventory : MonoBehaviour
{
    List<ItemSlot> Slot = new List<ItemSlot>();

    public bool HaveItemSlot(ItemSlot ItemSlot)
    {
        foreach (ItemSlot r in Slot)
        {
            if (r.item.Equals(ItemSlot.item) && r.amount >= ItemSlot.amount)
            {
                return true;
            }
        }
        return false;
    }
    public bool HaveItem(Item item)
    {
        foreach (ItemSlot r in Slot)
        {
            if (r.item.Equals(item))
            {
                return true;
            }
        }
        return false;
    }
    public void ReceiveItemSlot(ItemSlot ItemSlot)
    {
        if (HaveItem(ItemSlot.item))
        {
            foreach (ItemSlot r in Slot)
            {
                if (r.item.Equals(ItemSlot.item))
                {
                    r.amount += ItemSlot.amount;
                    return;
                }
            }
        }
        else {
            Slot.Add(ItemSlot);
        }
    }

    public bool ConsumeItemSlot(ItemSlot ItemSlot)
    {
        if (HaveItemSlot(ItemSlot))
        {
            foreach (ItemSlot r in Slot)
            {
                if (r.item.Equals(ItemSlot.item))
                {
                    r.amount -= ItemSlot.amount;
					return true;
                }
            }
        }
		return false;
    }
}