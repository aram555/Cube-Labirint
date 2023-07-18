using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeSpawner : MonoBehaviour
{
    [Header("Maze")]
    public GameObject CellPrefab;
    public Vector3 CellSize = new Vector3(1,1,0);
    public float timer;
    [Header("NavMesh")]
    public NavMeshSurface[] surfaces;
    public bool surface;

    [Header("Enemies and Dangerous")]
    public GameObject enemies;
    public int enemiesCount;
    
    [Header("BackGround and Walls")]
    [SerializeField] private GameObject backgroundParticle;
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
        StartCoroutine(CreateMaze());
        //SetColor();
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

        if(randomTwo == randomOne) randomTwo = Random.Range(0, colors.Length);
        
        Gradient.SetColor("_Top", colors[randomOne]);
        Gradient.SetColor("_Bottom", colors[randomTwo]);

        Walls.color  = colors[randomOne];
        Ground.color = colors[randomTwo];

        ParticleSystem.MainModule backP = backgroundParticle.GetComponent<ParticleSystem>().main;
        backP.startColor = new ParticleSystem.MinMaxGradient(colors[randomOne], colors[randomTwo]);
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
                c.GetComponent<WallTraffic>().Traffic(false, true);
                yield return new WaitForSeconds(timer);
            }
            yield return new WaitForSeconds(1);
        }
        SetColor();

        MazeGenerator generator = new MazeGenerator();
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                GameObject C = Instantiate(CellPrefab, new Vector3(x * CellSize.x, (y * CellSize.y), y * CellSize.z), Quaternion.identity);
                Cell c = C.transform.GetChild(0).GetComponent<Cell>();

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
                c.GetComponent<WallTraffic>().Traffic(true, false);
                yield return new WaitForSeconds(timer);
            }
            yield return new WaitForSeconds(timer);
        }
        yield return new WaitForSeconds(2);
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