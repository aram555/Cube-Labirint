using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeSpawner : MonoBehaviour
{
    [Header("Maze")]
    public Cell CellPrefab;
    public Vector3 CellSize = new Vector3(1,1,0);
    public float timer;
    [Header("NavMesh")]
    public NavMeshSurface[] surfaces;
    public bool surface;

    [Header("Enemies and Dangerous")]
    public GameObject enemies;
    public int enemiesCount;
    
    [Header("BackGround and Walls")]
    [SerializeField] private Color[] colors;
    [SerializeField] private Material Gradient;
    [SerializeField] private Material Ground;
    [SerializeField] private Material Walls;

    public Maze maze;

    private void Start()
    {
        Create();
    }

    private void Update() {
        if(surface) {
            ReloadSurface();
            surface = false;
        }
    }

    public void Create() {
        surface = true;
        SetColor();
        StartCoroutine(CreateMaze());
        //ReloadSurface();
        //CreateEnemies();
    }

    public void ReloadSurface() {
        for (int i = 0; i < surfaces.Length; i++) {
            surfaces[i].BuildNavMesh();
        }
    }

    public void SetColor() {
        int randomOne = Random.Range(0, colors.Length);
        int randomTwo = Random.Range(0, colors.Length);
        Gradient.SetColor("_Top", colors[randomOne]);
        Gradient.SetColor("_Bottom", colors[randomTwo]);
        Walls.color  = colors[randomOne];
        Ground.color = colors[randomTwo];
    }

    public IEnumerator CreateMaze() {
        enemiesCount = Random.Range(1, 5);
        GameObject[] enem = GameObject.FindGameObjectsWithTag("Enemy");
        if(enem.Length > 0) {
            foreach(GameObject e in enem) {
                Destroy(e);
            }
        }

        GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");
        if(cell.Length > 0) {
            foreach(GameObject c in cell) {
                Destroy(c);
            }
        }

        MazeGenerator generator = new MazeGenerator();
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, (y * CellSize.y) + 10, y * CellSize.z), Quaternion.identity);

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
                yield return new WaitForSeconds(timer);
            }
            yield return new WaitForSeconds(timer);
        }
        yield return new WaitForSeconds(1);
        ReloadSurface();
        CreateEnemies();
    }


    public void CreateEnemies() {
        for(int i = 0; i < enemiesCount; i++) {
            GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");
            int random = Random.Range(0, cell.Length);

            Instantiate(enemies, cell[random].transform.position, Quaternion.identity);
        }
    }
}