using UnityEngine;
using System.Collections;

public class RightClick : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1)) {
            Notify(Input.mousePosition);
        }
        
    }
    void Notify(Vector3 pos) {

    }
}
