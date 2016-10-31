using UnityEngine;
using System.Collections;
using Cubiquity;

public class CreateVoxelColoredCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		int width = 100;
		int height = 10;
		int depth = 100;

		ColoredCubesVolumeData data = VolumeData.CreateEmptyVolumeData<ColoredCubesVolumeData>(new Region(0, 0, 0, width, height, depth));

		QuantizedColor green = new QuantizedColor(0, 255, 0, 255);

		for(int z = 0; z < depth; z++){

			for (int y = 0; y < height; y++) {

				for (int x = 0; x < width; x++) {
					
					data.SetVoxel (x, y, z, green);

				}
			}
		}
		ColoredCubesVolume coloredCubesVolume = gameObject.AddComponent<ColoredCubesVolume>();

		coloredCubesVolume.data = data;		

		gameObject.AddComponent<ColoredCubesVolumeRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
