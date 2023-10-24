using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshGenerator))]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [Range(0, 100)]
    [SerializeField] int randomFillPercent;
    [Range(0.5f, 5f)]
    [SerializeField] float squareSize;
    [SerializeField] string seed;
    [SerializeField] bool useRandomSeed;
    [SerializeField] int smoothRecursions;
    int[,] map;

    MeshGenerator meshGenerator;

    public event EventHandler OnLevelGenerationComplete;

    private void Start() {
        meshGenerator = GetComponent<MeshGenerator>();

        GameController.Instance.OnLevelComplete += CreateNewWorld;

        CreateNewWorld();
    }

    private void CreateNewWorld(object e, EventArgs data) => CreateNewWorld();

    private void CreateNewWorld() {
            meshGenerator.Clear();
            
            GenerateMap();
            RandomFillMap();

            for (int i = 0; i < smoothRecursions; i++)
                SmoothMap();

            meshGenerator.GenerateMesh(map, squareSize);

            OnLevelGenerationComplete?.Invoke(this, EventArgs.Empty);
    }

    private void GenerateMap() {
        map = new int[width, height];
    }

    private void RandomFillMap() {
        if (useRandomSeed) {
            seed = System.DateTime.Now.ToString();
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
        
        for (int x = gridX - 1; x <= gridX + 1; x++)
        for (int y = gridY - 1; y <= gridY + 1; y++) {
            if (x < 0 || x >= width || y < 0 || y >= height) {
                wallCount++;
                continue;
            }
            if (x == gridX && y == gridY) continue;
            wallCount += map[x, y];
        }

        return wallCount;
    }

    // private void OnDrawGizmos() {
    //     if (map != null) {
    //         for (int x = 0; x < width; x++) {
    //             for (int y = 0; y < height; y++) {
    //                 Gizmos.color = (map[x,y] == 1) ? Color.black : Color.white;
    //                 Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0, -height/2 + y + 0.5f);
    //                 Gizmos.DrawCube(pos, Vector3.one);
    //             }
    //         }
    //     }
    // }
}
