using UnityEngine;
using System.Collections;

public static class Noise {

	public enum NormalizeMode {Local, Global};

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode) {
		float[,] noiseMap = new float[mapWidth,mapHeight];
		
		//lacunarity means gap
		// if we wnat to get the same map again we will return seed

		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < octaves; i++) {	// because we want to generate maps diffirent locations of the map we kep offset values
			float offsetX = prng.Next (-100000, 100000) + offset.x;
			float offsetY = prng.Next (-100000, 100000) - offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= persistance;
		}

		if (scale <= 0) {	// scale can't be 0 because ve divide it
			scale = 0.0001f;
		}

		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;	// for zooming in the middle of the map while using noise scale
		float halfHeight = mapHeight / 2f;


		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				amplitude = 1;
				frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++) {
					float sampleX = (x-halfWidth + octaveOffsets[i].x) / scale * frequency; // higher the freq means higher apart the sample points
					float sampleY = (y-halfHeight + octaveOffsets[i].y) / scale * frequency;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;	// we did *2-1 because we want the value to be between -1 to 1
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;	// amp getting smaller and smaller                                    
					frequency *= lacunarity;	// freq getting bigger and bigger                                    
				}                                    
                                    
				if (noiseHeight > maxLocalNoiseHeight) {	// here we get the min and max points of our map                         
					maxLocalNoiseHeight = noiseHeight;                                    
				} else if (noiseHeight < minLocalNoiseHeight) {                                    
					minLocalNoiseHeight = noiseHeight;                                    
				}                                    
				noiseMap [x, y] = noiseHeight;
			}
		}
		

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				if (normalizeMode == NormalizeMode.Local) {		 // Normalizing means getting the values between 0 to 1 again
					noiseMap [x, y] = Mathf.InverseLerp (minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap [x, y]);	// this method returns 0 to 1 if its equal to the min LocalNoiseHeight it will return 0
				} else {
					float normalizedHeight = (noiseMap [x, y] + 1) / (maxPossibleHeight/0.9f);
					noiseMap [x, y] = Mathf.Clamp(normalizedHeight,0, int.MaxValue);
				}
			}
		}

		return noiseMap;
	}

}
