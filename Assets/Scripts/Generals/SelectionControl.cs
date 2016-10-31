using UnityEngine;
using System.Collections;

public enum Category { Unit, Building, Resource}

public class SelectionControl : MonoBehaviour
{
    GroupHandler groupHandler;
    public Category category = Category.Unit;
    public bool highlighted = false;
    public bool  selected = false;
    void Start() {
        groupHandler = GameObject.FindGameObjectWithTag("Esentials").GetComponent<GroupHandler>();
        switch (category)
        {
            case Category.Unit:
                groupHandler.Units.Add(gameObject);
                break;
            case Category.Building:
                groupHandler.Buildings.Add(gameObject);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Unit>().meshRenderer.isVisible)
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = SelectionBox.InvertMouseY(camPos.y);
            if (Input.GetMouseButtonUp(0))
            {
                selected = SelectionBox.selection.Contains(camPos);
                if (selected) {
                    Selected();
                }
                
            }
            else if (Input.GetMouseButton(0))
            {
                highlighted = SelectionBox.selection.Contains(camPos);
                if (highlighted) {
                    Highlight();
                }
                
            } 
        }
       
    }
    public bool IsHighLighted()
    {
        return highlighted;
    }

    public bool IsSelected()
    {
        return selected;
    }
    void Highlight()
    {

    }
    void Selected()
    {
        switch (category) {
            case Category.Unit:
                groupHandler.UnitsSelected.Add(gameObject);
                groupHandler.Selected.Add(gameObject);
                break;
            case Category.Building:
                groupHandler.UnitsSelected.Add(gameObject);
                groupHandler.Selected.Add(gameObject);
                break;
        }
        
    }

}