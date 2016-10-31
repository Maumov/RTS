using UnityEngine;
using System.Collections;

public class CameraMeshTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxisRaw ("Horizontal") > 0){
			transform.RotateAround (Vector3.zero,Vector3.up,50f * Time.deltaTime);	
		}
		if(Input.GetAxisRaw ("Horizontal") < 0){
			transform.RotateAround (Vector3.zero,Vector3.up,-50f * Time.deltaTime);
		}

	}
}
