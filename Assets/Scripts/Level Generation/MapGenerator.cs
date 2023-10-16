using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [Range(0, 100)]
    [SerializeField] int randomFillPercent;
    [SerializeField] string seed;
    [SerializeField] bool useRandomSeed;
    [SerializeField] int smoothRecursions;
    int[,] map;

    private void Start() {
        GenerateMap();
        RandomFillMap();

        for (int i = 0; i < smoothRecursions; i++)
            SmoothMap();
    }

    private void GenerateMap() {
        map = new int[width, height];
    }

    private void RandomFillMap() {
        if (useRandomSeed) {
            seed = Time.time.ToString();
        }

        System.Random prng = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    map[x, y] = 1;
                else
                    map[x, y] = (prng.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }
    }

    private void SmoothMap() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                int neighbourWallCount = GetSurroundingWallCount(x, y);
                
                if (neighbourWallCount > 4) map[x, y] = 1;
                else if (neighbourWallCount < 4) map[x, y] = 0;
            }
        }
    }

    private int GetSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX < 0 || neighbourX >= width || neighbourY < 0 || neighbourY >= height) {
                    wallCount++;
                    continue;
                }
                if (neighbourX == gridX || neighbourY == gridY) continue;
                wallCount += map[neighbourX, neighbourY];
            }
        }

        return wallCount;
    }

    private void OnDrawGizmos() {
        if (map != null) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Gizmos.color = (map[x,y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0, -height/2 + y + 0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}