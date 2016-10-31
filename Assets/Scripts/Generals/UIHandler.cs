using UnityEngine;
using System.Collections;

public class UIHandler : MonoBehaviour {

    public GameObject uI;

    void Start() {
        uI = GetComponentInChildren<Canvas>().gameObject;
        HideUI();
    }

    public void ShowUI() {
        uI.gameObject.SetActive(true);
    }
    public void HideUI() {
        uI.gameObject.SetActive(false);
    }
}
