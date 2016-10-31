using UnityEngine;
using System.Collections;
using System;

//https://www.youtube.com/watch?v=EWQpo4sjuxw
public class BuildBlockMesh : MonoBehaviour {

	public GameObject newBlock;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown (0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Physics.Raycast (ray,out hit,1000f)){
				Vector3 blockPos = hit.point + hit.normal / 2f;

				blockPos.x = (float)Math.Round (blockPos.x,MidpointRounding.AwayFromZero);
				blockPos.y = (float)Math.Round (blockPos.y,MidpointRounding.AwayFromZero);
				blockPos.z = (float)Math.Round (blockPos.z,MidpointRounding.AwayFromZero);

				GameObject block = (GameObject)Instantiate (newBlock,blockPos,Quaternion.identity);
				block.transform.parent = this.transform;
				Combine (block);
			}
		}
	}
	void Combine(GameObject block){
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		Destroy (this.gameObject.GetComponent<MeshCollider>());
		int i = 0;
		Debug.Log (meshFilters.Length);
		while (i < meshFilters.Length) {
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].gameObject.active = false;
			i++;
		}
		transform.GetComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine,true);
		transform.GetComponent<MeshFilter>().mesh.RecalculateBounds ();
		transform.GetComponent<MeshFilter>().mesh.RecalculateNormals ();
		transform.GetComponent<MeshFilter>().mesh.Optimize ();

		this.gameObject.AddComponent<MeshCollider>();
		transform.gameObject.active = true;
		Destroy (block);
	}
}
