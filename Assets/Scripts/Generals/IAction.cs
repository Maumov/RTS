using UnityEngine;
using System.Collections;
using System;


public interface IAction{

	Unit target {
		get;
		set;
	}

    Vector3 originPosition {
        get;
        set;
    }

    Vector3 destinationPosition {
        get;
        set;
    }

	void OriginPositionAction ();
	void DestinationPositionAcion();
}

[Serializable]
public class ItemSlot{
    public Item item;
    public int amount;
}
[Serializable]
public class Item{
    public int id;
    public string name;
}
