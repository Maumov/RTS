using UnityEngine;
using System.Collections;
using System.Reflection.Emit;
[RequireComponent(typeof(UIHandler)) ]
[RequireComponent(typeof(SelectionControl))]
public class Unit : Stats
{	
   
	[Header ("Visuals")]
	public Color color;
	public MeshRenderer meshRenderer;
	public Texture texture;

	[Header ("UI")]
	UIHandler uIHandler;
    // Use this for initialization
    void Start()
    {
		uIHandler = GetComponent<UIHandler> ();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Notified() {
		
    }

}
