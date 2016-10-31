using UnityEngine;
using System.Collections;

public class SelectionBox : MonoBehaviour {

    //public Texture2D selectionHighlight = null;
    public static Rect selection = new Rect(0,0,0,0);
    private Vector3 startClick = -Vector3.one;
    public UnityEngine.UI.Image SelectionImage = null;

    bool selecting = false;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        CreateSelection();
	}

    private void CreateSelection() {
        if (Input.GetMouseButtonDown(0)) {
            startClick = Input.mousePosition;
            selecting = true;
            
        } else if (Input.GetMouseButtonUp(0)) {
           
            selecting = false;
        }
        if (Input.GetMouseButton(0)) {
            
            selection = new Rect(startClick.x, InvertMouseY(startClick.y), Input.mousePosition.x - startClick.x, InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y));

            if (selection.width < 0)
            {
                selection.x += selection.width;
                selection.width = -selection.width;
            }
            if (selection.height < 0)
            {
                selection.y += selection.height;
                selection.height = -selection.height;

            }

        }
    }
    public static float InvertMouseY(float y) {
        return Screen.height - y;

    }

    public bool isSelecting() {
        return selecting;
    }

    private void OnGUI() {
        if (isSelecting())
        {
            Vector2 startposition = new Vector2(startClick.x - (Screen.width * 0.5f), startClick.y - (Screen.height * 0.5f));
            Vector2 currentposition = new Vector2(Input.mousePosition.x - (Screen.width * 0.5f), Input.mousePosition.y - (Screen.height * 0.5f));
            SelectionImage.rectTransform.localPosition = new Vector2((startposition.x + currentposition.x) * 0.5f, (startposition.y + currentposition.y) * 0.5f);
            SelectionImage.rectTransform.sizeDelta = new Vector2(Mathf.Abs(currentposition.x - startposition.x), Mathf.Abs(currentposition.y - startposition.y));
            SelectionImage.enabled = true;
        }
        else {
            SelectionImage.enabled = false;
        }
    }
}
