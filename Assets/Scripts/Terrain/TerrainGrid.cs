using UnityEngine;
using System.Collections;
using UnityEditor;

public class TerrainGrid : MonoBehaviour {

	public GameObject prefab;
	public GameObject quad;
	public Transform mapContainer;
	public int x = 10,y = 10,z = 1;


	void Start(){
		StartCoroutine (GenerateWorld ());
	}

	IEnumerator GenerateWorld(){
		yield return StartCoroutine (GenerateFront ());
		yield return StartCoroutine (GenerateBack ());
		yield return StartCoroutine (GenerateLeft ());
		yield return StartCoroutine (GenerateRight ());
		yield return StartCoroutine (GenerateTop ());
		yield return StartCoroutine (GenerateBottom ());
		CombineMeshes ();
		yield return null;
	}
	IEnumerator GenerateFront(){
		for(int i = 0; i < x; i++){
			for(int j = 0; j < y; j++){
				Quaternion rotation = Quaternion.Euler (0f,0f,0f);
				GameObject go = (GameObject)Instantiate (prefab,mapContainer.position + new Vector3(0.5f,0.5f,0f) + new Vector3(i,j,0f),rotation);
				go.transform.parent = mapContainer;
				go.name = "Front " + (i + j);

			}
			yield return null;
		}
		yield return null;
	}
	IEnumerator GenerateBack(){
		for(int i = 0; i < x; i++){
			for(int j = 0; j < y; j++){
				Quaternion rotation = Quaternion.Euler (0f,180f,0f);
				GameObject go = (GameObject)Instantiate (prefab,mapContainer.position + new Vector3(0.5f,0.5f,0f) + new Vector3(i,j,z),rotation);
				go.transform.parent = mapContainer;
				go.name = "Back " + (i + j);

			}
			yield return null;
		}
		yield return null;
	}
	IEnumerator GenerateLeft(){
		for(int i = 0; i < y; i++){
			for(int j = 0; j < z; j++){
				Quaternion rotation = Quaternion.Euler (0f,90f,0f);
				GameObject go = (GameObject)Instantiate (prefab,mapContainer.position + new Vector3(0f,0.5f,0.5f) + new Vector3(0,i,j),rotation);
				go.transform.parent = mapContainer;
				go.name = "Left " + (i + j);

			}
			yield return null;
		}
		yield return null;
	}
	IEnumerator GenerateRight(){
		for(int i = 0; i < y; i++){
			for(int j = 0; j < z; j++){
				Quaternion rotation = Quaternion.Euler (0f,-90f,0f);
				GameObject go = (GameObject)Instantiate (prefab,mapContainer.position + new Vector3(0f,0.5f,0.5f) + new Vector3(x,i,j),rotation);
				go.transform.parent = mapContainer;
				go.name = "Right " + (i + j);

			}
			yield return null;
		}
		yield return null;
	}
	IEnumerator GenerateTop(){
		for(int i = 0; i < x; i++){
			for(int j = 0; j < z; j++){
				Quaternion rotation = Quaternion.Euler (90f,0f,0f);
				GameObject go = (GameObject)Instantiate (prefab,mapContainer.position + new Vector3(0.5f,0f,0.5f) + new Vector3(i,y,j),rotation);
				go.transform.parent = mapContainer;
				go.name = "Top " + (i + j);
			}
			yield return null;
		}
		yield return null;
	}
	IEnumerator GenerateBottom(){
		for(int i = 0; i < x; i++){
			for(int j = 0; j < z; j++){
				Quaternion rotation = Quaternion.Euler (-90f,0f,0f);
				GameObject go = (GameObject)Instantiate (prefab,mapContainer.position + new Vector3(0.5f,0f,0.5f) + new Vector3(i,0f,j),rotation);
				go.transform.parent = mapContainer;
				go.name = "Bottom " + (i + j);
			}
			yield return null;
		}
		yield return null;
	}
	void CombineMeshes(){
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		//Destroy (this.gameObject.GetComponent<MeshCollider>());
		int i = 0;
		Debug.Log (meshFilters.Length);
		while (i < meshFilters.Length) {
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].gameObject.active = false;
			if(meshFilters[i].name != this.name){
				Destroy (meshFilters[i].gameObject);	
			}
			i++;
		}
		transform.GetComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine,true);
		transform.GetComponent<MeshFilter>().mesh.RecalculateBounds ();
		transform.GetComponent<MeshFilter>().mesh.RecalculateNormals ();
		transform.GetComponent<MeshFilter>().mesh.Optimize ();

		this.gameObject.AddComponent<MeshCollider>();
		transform.gameObject.active = true;
	}
}
