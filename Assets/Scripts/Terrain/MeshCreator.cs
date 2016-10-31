using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
public class MeshCreator : MonoBehaviour {
	public enum Side
	{
		Top,Bottom,Left,Right,Front,Back
	}

	public GameObject target,targetResult;
	public float width = 5f;
	public float height = 5f;
	public float lenght = 5f;
	public float squareSize = 1f;
	// Use this for initialization
	void Start () {
		Test ();
	}
	// Update is called once per frame
	void Update () {
	
	}
	void Test(){
		Vector3[] verts = GenerateVertices (width, width, width,Side.Front);
		int[] tris = GenerateTriangles (verts, width, width);
		Vector3[] normals = GenerateNormals (verts, Side.Front);
		MeshFilter meshFilter = targetResult.GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.normals = normals;
		for(int i = 0; i < mesh.vertices.Length;i++){
			GameObject ob = (GameObject)Instantiate (target,mesh.vertices[i],Quaternion.identity);
			ob.name = i.ToString ();
		}
	}
	Vector3[] GenerateVertices(float width,float height,float length,Side side){
		List<Vector3> vertices = new List<Vector3> (); 
		for(int i = 0; i< height ; i++){
			for(int j = 0; j< width ; j++){
				switch (side) {
				case Side.Back:
					vertices.Add (new Vector3(i ,j ,length-1));	
					break;
				case Side.Bottom:
					vertices.Add (new Vector3(i ,0f, j ));	
					break;
				case Side.Front:
					vertices.Add (new Vector3(i,j,0f));	
					break;
				case Side.Left:
					vertices.Add (new Vector3(0f,j,i));	
					break;
				case Side.Right:
					vertices.Add (new Vector3(length-1,j,i));	
					break;
				case Side.Top:
					vertices.Add (new Vector3(i,length-1,j));	
					break;

				}
			}
		}
		return vertices.ToArray ();
	}
	int[] GenerateTriangles(Vector3[] vertices , float width, float height){
		List<int> tri = new List<int> ();
		for(int i = 0; i < width-1; i ++){
			for(int j = 0; j < height-1; j++){
				tri.Add ((i * (int)height) + (j) );
				tri.Add (((i+1) * (int)height) + (j) );
				tri.Add ((i * (int)height) + (j) + 1);

				tri.Add (((i+1) * (int)height) + (j) );
				tri.Add (((i+1) * (int)height) + (j) + 1);
				tri.Add ((i * (int)height) + (j) + 1);
			}
		}
		return tri.ToArray ();
	}
	Vector3[] GenerateNormals(Vector3[] vertices,Side side){
		List<Vector3> normals = new List<Vector3> (); 
		Vector3 normalDirection = new Vector3();
		switch (side) {
			
		case Side.Top:
			normalDirection = Vector3.up;
			break;
		case Side.Bottom:
			normalDirection = Vector3.down;
			break;
		case Side.Left:
			normalDirection = Vector3.left;
			break;
		case Side.Right:
			normalDirection = Vector3.right;
			break;
		case Side.Front:
			normalDirection = Vector3.forward;
			break;
		case Side.Back:
			normalDirection = Vector3.back;
			break;
		}
		for(int i = 0; i < vertices.Length; i++){
			normals.Add (normalDirection);
		}
		return normals.ToArray ();

	}
	void GenerateQuad(){
		MeshFilter mf = target.GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		mf.mesh = mesh;

		Vector3[] vertices = new Vector3[4];

		vertices[0] = new Vector3(0, 0, 0);
		vertices[1] = new Vector3(width, 0, 0);
		vertices[2] = new Vector3(0, height, 0);
		vertices[3] = new Vector3(width, height, 0);

		mesh.vertices = vertices;

		int[] tri = new int[6];

		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;

		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;

		mesh.triangles = tri;

		Vector3[] normals = new Vector3[4];

		normals[0] = -Vector3.forward;
		normals[1] = -Vector3.forward;
		normals[2] = -Vector3.forward;
		normals[3] = -Vector3.forward;

		mesh.normals = normals;

		Vector2[] uv = new Vector2[4];

		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);

		mesh.uv = uv;
	}
}
