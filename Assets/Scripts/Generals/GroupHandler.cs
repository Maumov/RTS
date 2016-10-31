using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GroupHandler : MonoBehaviour {

    public List<GameObject> Buildings;
    public List<GameObject> Units;
    public List<GameObject> UnitsSelected;
    public List<GameObject> BuildingsSelected;
    public List<GameObject> Selected;

    public void Notified() {
        
    }
    void NotifySelected() {
        foreach (GameObject g in Selected) {
            g.GetComponent<Unit>().Notified();
        }
    }
}
