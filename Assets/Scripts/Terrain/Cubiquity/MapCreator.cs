using UnityEngine;
using System.Collections;
using Cubiquity;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour {
	public int width = 100;
	public int height = 10;
	public int depth = 100;
	public GameObject cubesHolder, terrainHolder;

	TerrainVolume terrainVolume;
	ColoredCubesVolume coloredCubesVolume;
	public int range = 3;
	public int weight = 128;
	public Material terrainMaterial;
	QuantizedColor color;
	// Use this for initialization
	void Start () {
		CreateMap ();
	}
	
	// Update is called once per frame
	void Update () {
		RemoveVolume ();
		AddVolume ();
		ChangeWeight ();
	}
	void CreateMap(){

		ColoredCubesVolumeData cubesData = VolumeData.CreateEmptyVolumeData<ColoredCubesVolumeData>(new Region(0, 0, 0, width, height, depth));
		MaterialSet materialSet = new MaterialSet();
		float simplexNoiseValue = 0f;
		simplexNoiseValue += 2f; 
		simplexNoiseValue *= 127.5f;
		materialSet.weights[3] = (byte)simplexNoiseValue;
		color = new QuantizedColor(255, 255, 255, 255);

		for (int z = 0; z <= depth; z++)
		{
			for (int y = 0; y < 2; y++)
			{
				for (int x = 0; x <= width; x++)
				{
					
					cubesData.SetVoxel(x, y, z, color);
				}
			}
		}

		coloredCubesVolume = cubesHolder.AddComponent<ColoredCubesVolume>();
		coloredCubesVolume.data = cubesData;		
		cubesHolder.AddComponent<ColoredCubesVolumeRenderer>();
		cubesHolder.AddComponent <ColoredCubesVolumeCollider>();
		CreateTerrain ();
	}
	void CreateTerrain(){
		TerrainVolumeData terrainData = VolumeData.CreateEmptyVolumeData<TerrainVolumeData>(new Region(0, 0, 0, width, height, depth));
		MaterialSet materialSet = new MaterialSet();
		float simplexNoiseValue = 1f;


		for (int z = 0; z <= depth; z++)
		{
			for (int x = 0; x <= width; x++)
			{
				
				Vector3 pos = new Vector3 (x,0f,z);

				if(TerrainVoxelPosition (ref pos, ref simplexNoiseValue))
				{					
					simplexNoiseValue *= 127.5f;
					materialSet.weights[3] = (byte)simplexNoiseValue;
					terrainData.SetVoxel((int)pos.x,(int) pos.y,(int) pos.z, materialSet);
				}

			}
		}


		terrainVolume = terrainHolder.AddComponent<TerrainVolume>();
		terrainVolume.data = terrainData;
		TerrainVolumeRenderer tr = terrainHolder.AddComponent<TerrainVolumeRenderer>();
		tr.material = terrainMaterial;
		terrainHolder.AddComponent <TerrainVolumeCollider>();

	}

	bool TerrainVoxelPosition( ref Vector3 voxelPosition ,ref float weight){
		Ray ray = new Ray ();
		ray.direction = Vector3.down;

		ray.origin = new Vector3 (voxelPosition.x, transform.position.y + height + 1f, voxelPosition.z);
		PickVoxelResult pickResult1;
		bool hit1 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, 1000.0f, out pickResult1);
		if(hit1){
			ray.origin = new Vector3 (voxelPosition.x+1, transform.position.y + height + 1f, voxelPosition.z);
			PickVoxelResult pickResult2;
			bool hit2 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, 1000.0f, out pickResult2);

			ray.origin = new Vector3 (voxelPosition.x-1,transform.position.y + height + 1f, voxelPosition.z);
			PickVoxelResult pickResult3;
			bool hit3 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, 1000.0f, out pickResult3);

			ray.origin = new Vector3 (voxelPosition.x, transform.position.y + height + 1f, voxelPosition.z +1);
			PickVoxelResult pickResult4;
			bool hit4 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, 1000.0f, out pickResult4);

			ray.origin = new Vector3 (voxelPosition.x, transform.position.y + height + 1f, voxelPosition.z -1);
			PickVoxelResult pickResult5;
			bool hit5 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, 1000.0f, out pickResult5);

			//
			// Diagonales Ignoradas
			//
//			ray.origin = new Vector3 (voxelPosition.x, transform.position.y + height + 1f, voxelPosition.z);
//			PickVoxelResult pickResult6;
//			bool hit6 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, 1000.0f, out pickResult6);
//
//			ray.origin = new Vector3 (voxelPosition.x, transform.position.y + height + 1f, voxelPosition.z);
//			PickVoxelResult pickResult7;
//			bool hit7 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, 1000.0f, out pickResult7);
//
//			ray.origin = new Vector3 (voxelPosition.x, transform.position.y + height + 1f, voxelPosition.z);
//			PickVoxelResult pickResult8;
//			bool hit8 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, 1000.0f, out pickResult8);
//
//			ray.origin = new Vector3 (voxelPosition.x, transform.position.y + height + 1f, voxelPosition.z);
//			PickVoxelResult pickResult9;
//			bool hit9 = Picking.PickFirstSolidVoxel (coloredCubesVolume, ray, 1000.0f, out pickResult9);	
		}
		return true;
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
}
