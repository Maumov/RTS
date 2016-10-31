using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
	float vertical,horizontal,mousewheel;
	public float zoomMultiplier;
	public float xyMultiplier;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		GetInputs ();
		transform.position += transform.forward * mousewheel + new Vector3(horizontal,0f,vertical).normalized ;
    }
	void GetInputs(){
		vertical = Input.GetAxisRaw ("Vertical") * xyMultiplier;
		horizontal = Input.GetAxisRaw ("Horizontal") * xyMultiplier;
		mousewheel = Input.GetAxis ("Mouse ScrollWheel") * zoomMultiplier;

	}
}
