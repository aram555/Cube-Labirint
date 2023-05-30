﻿using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    [Header("Maze")]
    public Cell CellPrefab;
    public Vector3 CellSize = new Vector3(1,1,0);
    [Header("Enemies and Dangerous")]
    public GameObject enemies;
    public int enemiesCount;

    public Maze maze;

    private void Start()
    {
        CreateMaze();
        CreateEnemies();
    }

    public void CreateMaze() {
        MazeGenerator generator = new MazeGenerator();
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
            }
        }
    }

    public void CreateEnemies() {
        for(int i = 0; i < enemiesCount; i++) {
            GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");
            int random = Random.Range(0, cell.Length);

            Instantiate(enemies, cell[random].transform.position, Quaternion.identity);

            print(cell[random]);
        }
    }
}