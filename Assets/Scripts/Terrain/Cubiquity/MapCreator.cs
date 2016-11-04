using UnityEngine;
using System.Collections;
using Cubiquity;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour {
	public string textureLocation;
	//public int VoxelThresHold = 128;
	public int maxHeight = 1;
 	public int width = 100;
	public int depth = 100;
	public GameObject cubesHolder, terrainHolder;

	TerrainVolume terrainVolume;
	ColoredCubesVolume coloredCubesVolume;
	public int range = 3;
	public int weight = 128;
	public Material terrainMaterial;
	QuantizedColor color;
	public float loadPercentage;
	// Use this for initialization
	void Start () {
		StartCoroutine(CreateMap ());
	}
	
	// Update is called once per frame
	void Update () {
//		RemoveVolume ();
//		AddVolume ();
//		ChangeWeight ();
	}
	IEnumerator CreateMap(){
		int[,] map = LoadImage(textureLocation);
		ColoredCubesVolumeData cubesData = VolumeData.CreateEmptyVolumeData<ColoredCubesVolumeData>(new Region(0, 0, 0, width, maxHeight, depth ));
		color = new QuantizedColor(255, 255, 255, 255);

//		coloredCubesVolume = cubesHolder.AddComponent<ColoredCubesVolume>();
//		coloredCubesVolume.data = cubesData;		
//		cubesHolder.AddComponent<ColoredCubesVolumeRenderer>();
//		cubesHolder.AddComponent <ColoredCubesVolumeCollider>();

		for (int z = 0; z < depth; z++)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < (int)(((float)map[x,z]/255f) * maxHeight); y++)
				{
					//if(map[x,z] > VoxelThresHold){
					cubesData.SetVoxel(x, y, z, color);	
					//cubesData.SetVoxel(x, y, z, color);	
					//}
				}
			}
			loadPercentage = (float)z/(float)depth;
			yield return null;
		}
		Debug.Log("Finish");
		cubesData.CommitChanges();
		coloredCubesVolume = cubesHolder.AddComponent<ColoredCubesVolume>();
		coloredCubesVolume.data = cubesData;		
		cubesHolder.AddComponent<ColoredCubesVolumeRenderer>();
		cubesHolder.AddComponent <ColoredCubesVolumeCollider>();
		CreateTerrain ();

		cubesHolder.transform.localScale = new Vector3(3f,3f,3f);
		transform.localScale *= 1f/3f;

	}
	void CreateTerrain(){
		TerrainVolumeData terrainData = VolumeData.CreateEmptyVolumeData<TerrainVolumeData>(new Region(0, 0, 0, (width * 3) + 1, (maxHeight * 3) + 1, (depth * 3) + 1));
		MaterialSet materialSet = new MaterialSet();
		float simplexNoiseValue = 1f;

		for (int z = 0; z <= depth; z += 1)
		{
			for (int x = 0; x <= width; x += 1)
			{
				Vector3 pos = new Vector3 (x ,0f, z);

				if(TerrainVoxelPosition (ref pos))
				{	
					materialSet.weights[3] = (byte)128;
					//Center
					terrainData.SetVoxel((x*3)+1,(int) pos.y+0,(z*3)+1, materialSet);
					terrainData.SetVoxel((x*3)+1,(int) pos.y+0,(z*3)+2, materialSet);
					terrainData.SetVoxel((x*3)+2,(int) pos.y+0,(z*3)+1, materialSet);
					terrainData.SetVoxel((x*3)+2,(int) pos.y+0,(z*3)+2, materialSet);

					materialSet.weights[3] = (byte)128;
//					//Z Up
					Vector3 pos2= new Vector3 (x ,0f, z - 1);
					TerrainVoxelPosition (ref pos2);


					terrainData.SetVoxel((x*3)+0,(int) pos2.y,(z*3)+0, materialSet);

					terrainData.SetVoxel((x*3)+1,(int) pos2.y+0,(z*3)+0, materialSet);

					terrainData.SetVoxel((x*3)+2,(int) pos2.y+0,(z*3)+0, materialSet);

					terrainData.SetVoxel((x*3)+3,(int) pos2.y+0,(z*3)+0, materialSet);

//					//Z Down
//					Vector3 pos3= new Vector3 (x + 1 ,0f, z);
//					TerrainVoxelPosition (ref pos3);
//					terrainData.SetVoxel((x*3)+0,(int) pos3.y+0,(z*3)+3, materialSet);
//					terrainData.SetVoxel((x*3)+2,(int) pos3.y+0,(z*3)+3, materialSet);
//					terrainData.SetVoxel((x*3)+3,(int) pos3.y+0,(z*3)+3, materialSet);
//					//X Up
//					Vector3 pos4= new Vector3 (x - 1 ,0f, z);
//					TerrainVoxelPosition (ref pos4);
//					terrainData.SetVoxel((x*3)+3,(int) pos4.y+0,(z*3)+1, materialSet);
//					terrainData.SetVoxel((x*3)+3,(int) pos4.y+0,(z*3)+2, materialSet);
//					//X Down
//					Vector3 pos5= new Vector3 (x + 1 ,0f, z);
//					TerrainVoxelPosition (ref pos5);
//					terrainData.SetVoxel((x*3)+0,(int) pos5.y+0,(z*3)+1, materialSet);
//					terrainData.SetVoxel((x*3)+0,(int) pos5.y+0,(z*3)+2, materialSet);
//

				}

			}
		}


		terrainVolume = terrainHolder.AddComponent<TerrainVolume>();
		terrainVolume.data = terrainData;
		TerrainVolumeRenderer tr = terrainHolder.AddComponent<TerrainVolumeRenderer>();
		tr.material = terrainMaterial;
		terrainHolder.AddComponent <TerrainVolumeCollider>();

	}

	bool TerrainVoxelPosition(ref Vector3 voxelPosition){
		Ray ray = new Ray ();
		ray.direction = Vector3.down;

		ray.origin = new Vector3 (voxelPosition.x, transform.position.y + maxHeight + 1f, voxelPosition.z);
		PickVoxelResult pickResult;
		bool hit1 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, maxHeight + 5f, out pickResult);
		if(hit1){
			voxelPosition = new Vector3(pickResult.worldSpacePos.x,pickResult.worldSpacePos.y,pickResult.worldSpacePos.z);
			return true;
		}else{
			return false;
		}
		
	}




	void RemoveVolume(){
		if(Input.GetKeyUp(KeyCode.Alpha2))
		{
			// Build a ray based on the current mouse position
			Vector2 mousePos = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));


			// Perform the raycasting. If there's a hit the position will be stored in these ints.
			PickVoxelResult pickResult;
			bool hit = Picking.PickFirstSolidVoxel(coloredCubesVolume, ray, 1000.0f, out pickResult);

			// If we hit a solid voxel then create an explosion at this point.
			if(hit)
			{					
				
				DestroyVoxels(pickResult.volumeSpacePos.x, pickResult.volumeSpacePos.y, pickResult.volumeSpacePos.z, range);

			}
		}
	}
	void AddVolume(){
		if(Input.GetKeyUp(KeyCode.Alpha1))
		{
			// Build a ray based on the current mouse position
			Vector2 mousePos = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));


			// Perform the raycasting. If there's a hit the position will be stored in these ints.
			PickVoxelResult pickResult;
			bool hit = Picking.PickLastEmptyVoxel(coloredCubesVolume, ray, 1000.0f, out pickResult);

			// If we hit a solid voxel then create an explosion at this point.
			if(hit)
			{					
				AddVoxels(pickResult.volumeSpacePos.x, pickResult.volumeSpacePos.y, pickResult.volumeSpacePos.z, range);
			}
		}
	}
	void ChangeWeight (){
		if(Input.GetKey(KeyCode.Alpha3))
		{
			// Build a ray based on the current mouse position
			Vector2 mousePos = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));


			// Perform the raycasting. If there's a hit the position will be stored in these ints.
			PickVoxelResult pickResult;
			bool hit = Picking.PickFirstSolidVoxel(coloredCubesVolume, ray, 1000.0f, out pickResult);

			// If we hit a solid voxel then create an explosion at this point.
			if(hit)
			{					
				AddWeight(pickResult.volumeSpacePos.x, pickResult.volumeSpacePos.y, pickResult.volumeSpacePos.z);
			}
		}
		if(Input.GetKey(KeyCode.Alpha4))
		{
			// Build a ray based on the current mouse position
			Vector2 mousePos = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));


			// Perform the raycasting. If there's a hit the position will be stored in these ints.
			PickVoxelResult pickResult;
			bool hit = Picking.PickFirstSolidVoxel(coloredCubesVolume, ray, 1000.0f, out pickResult);

			// If we hit a solid voxel then create an explosion at this point.
			if(hit)
			{					
				RemoveWeight(pickResult.volumeSpacePos.x, pickResult.volumeSpacePos.y, pickResult.volumeSpacePos.z);
			}
		}
	}
	void AddVoxels(int xPos, int yPos, int zPos, int range){
		MaterialSet materialSet = new MaterialSet();
		materialSet.weights[3] = (byte)weight;
		int rangeSquared = range * range;
		List<Vector3i> voxelsToDelete = new List<Vector3i>();

		for(int z = zPos - range; z <= zPos + range; z++) 
		{
			for(int y = yPos - range; y <= yPos + range; y++)
			{
				for(int x = xPos - range; x <= xPos + range; x++)
				{			
//					// Compute the distance from the current voxel to the center of our explosion.
//					int xDistance = x - xPos;
//					int yDistance = y - yPos;
//					int zDistance = z - zPos;
//
//					// Working with squared distances avoids costly square root operations.
//					int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;

					// We're iterating over a cubic region, but we want our explosion to be spherical. Therefore 
					// we only further consider voxels which are within the required range of our explosion center. 
					// The corners of the cubic region we are iterating over will fail the following test.
//					if(distSquared < rangeSquared)
//					{							
						Vector3i voxel = new Vector3i(x, y, z);
						voxelsToDelete.Add(voxel);

//					}
				}
			}
		}

		foreach (Vector3i voxel in voxelsToDelete) // Loop through List with foreach
		{
			coloredCubesVolume.data.SetVoxel(voxel.x, voxel.y, voxel.z, color);
			terrainVolume.data.SetVoxel (voxel.x,voxel.y,voxel.z,materialSet);
		}
		//CreateTerrain ();
	}
	void AddWeight(int xPos, int yPos, int zPos){
		int a = terrainVolume.data.GetVoxel (xPos, yPos, zPos).weights [3];
		a = a + 1; 
		if(a <=300){
			MaterialSet ms = new MaterialSet();
			ms.weights [3] = (byte)a;
			terrainVolume.data.SetVoxel (xPos, yPos, zPos,ms);	
		}
	}

	void RemoveWeight(int xPos, int yPos, int zPos){
		int a = terrainVolume.data.GetVoxel (xPos, yPos, zPos).weights [3];
		byte b = (byte)(a - 1); 
		if(b >=(byte)0){
			MaterialSet ms = new MaterialSet();
			ms.weights [3] = b;
			terrainVolume.data.SetVoxel (xPos, yPos, zPos,ms);	
		}
	}

	void DestroyVoxels(int xPos, int yPos, int zPos, int range)
	{
		MaterialSet emptyMaterialSet = new MaterialSet();

		int rangeSquared = range * range;

		List<Vector3i> voxelsToDelete = new List<Vector3i>();

		for(int z = zPos - range; z < zPos + range; z++) 
		{
			for(int y = yPos - range; y < yPos + range; y++)
			{
				for(int x = xPos - range; x < xPos + range; x++)
				{			
					// Compute the distance from the current voxel to the center of our explosion.
					int xDistance = x - xPos;
					int yDistance = y - yPos;
					int zDistance = z - zPos;

					// Working with squared distances avoids costly square root operations.
					int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;

					// We're iterating over a cubic region, but we want our explosion to be spherical. Therefore 
					// we only further consider voxels which are within the required range of our explosion center. 
					// The corners of the cubic region we are iterating over will fail the following test.
					if(distSquared < rangeSquared)
					{							
						Vector3i voxel = new Vector3i(x, y, z);
						voxelsToDelete.Add(voxel);

					}
				}
			}
		}

		foreach (Vector3i voxel in voxelsToDelete) // Loop through List with foreach
		{
			coloredCubesVolume.data.SetVoxel(voxel.x, voxel.y, voxel.z, new QuantizedColor(0,0,0,0));
			terrainVolume.data.SetVoxel(voxel.x, voxel.y, voxel.z, emptyMaterialSet);
		}
	}

	int[,] LoadImage(string texture){
		byte[] bytes= System.IO.File.ReadAllBytes(Application.dataPath + "/StreamingAssets/" + texture);

		Texture2D levelMap = new Texture2D(2,2);
		levelMap.LoadImage(bytes);

		//get the raw pixels from the level imagemap
		Color32[] allPixels = levelMap.GetPixels32();
		width = levelMap.width;
		depth = levelMap.height;

		int[,] map = new int[width,depth];
		for(int x = 0; x < width; x++){
			for(int z = 0; z < depth; z++){
				map[x,z] = allPixels[(z * width) + x].r;
			}
		}
		return map;
	}
}
