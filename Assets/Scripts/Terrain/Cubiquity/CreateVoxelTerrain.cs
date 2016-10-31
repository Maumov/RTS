using UnityEngine;
using System.Collections;
using Cubiquity;

public class CreateVoxelTerrain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Dimensions of our volume.
		int width = 100;
		int height = 10;
		int depth = 100;

		TerrainVolumeData volumeData = VolumeData.CreateEmptyVolumeData<TerrainVolumeData>(new Region(0, 0, 0, width, height, depth));

		// Let's keep the allocation outside of the loop.
		MaterialSet materialSet = new MaterialSet();
		float simplexNoiseValue = 0f;
		simplexNoiseValue += 1.0f; 
		simplexNoiseValue *= 127.5f;
		materialSet.weights[0] = (byte)simplexNoiseValue;
		for (int z = 0; z < depth; z++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					volumeData.SetVoxel(x, y, z, materialSet);
				}
			}
		}
		//Add the required volume component.
		TerrainVolume terrainVolume = gameObject.AddComponent<TerrainVolume>();

		// Set the provided data.
		terrainVolume.data = volumeData;

		// Add the renderer
		gameObject.AddComponent<TerrainVolumeRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
