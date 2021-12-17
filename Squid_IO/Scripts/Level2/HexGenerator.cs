using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HexGenerator : MonoBehaviour
{
    [SerializeField] Transform hexPrefab;
    [SerializeField] Transform spikePrefab;
    [SerializeField] List<Material> materials;

    public int gridWidth = 11;
    public int gridHeight = 11;

    float hexWidth = 1.732f * 3;
    float hexHeight = 2f * 3;
    public float gap = 0f;

    Vector3 startPos;

    private void Start()
    {
        AddGap();
        CalcStartPos();
        CreateSpikeGrid(-1);
        CreateGrid(0);
        CreateGrid(1);
        CreateGrid(2);
    }

    private void AddGap()
    {
        hexWidth += gap;
        hexHeight += gap;
    }

    private void CalcStartPos()
    {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = hexWidth / 2;

        float x = -hexWidth * (gridWidth / 2) - offset;
        float z = hexWidth * 0.75f * (gridHeight / 2);

        startPos = new Vector3(x, 0, z);
    }

    private void CreateGrid(int layer)
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                Transform hex = Instantiate(hexPrefab);
                hex.gameObject.GetComponent<MeshRenderer>().material = materials[UnityEngine.Random.Range(0, materials.Count)];
                Vector2 gridPos = new Vector2(j, i);
                hex.position = CalcWorldPos(gridPos, layer);
                hex.parent = this.transform;
                hex.name = "Hexagon " + j + "|" + i + "|" + layer;
            }
        }
    }

    private void CreateSpikeGrid(int layer)
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                Transform hex = Instantiate(spikePrefab);
                Vector2 gridPos = new Vector2(j, i);
                hex.position = CalcWorldPos(gridPos, layer);
                hex.parent = this.transform;
                hex.name = "Spike " + j + "|" + i;
            }
        }
    }

    private Vector3 CalcWorldPos(Vector2 gridPos, int layer)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z + gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, layer * 40, z);
    }
}
