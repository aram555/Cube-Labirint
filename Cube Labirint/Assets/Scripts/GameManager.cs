using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public NavMeshSurface[] surfaces;
    public Transform finish;
    // Start is called before the first frame update

    MazeSpawner mazeSpawner;
    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        mazeSpawner = new MazeSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadSurface() {
        for (int i = 0; i < surfaces.Length; i++) {
            surfaces[i].BuildNavMesh();
        }
    }

    public void SpawnFinish(Vector2Int finishPos) {
        finish.position = new Vector3(finishPos.x*2, finish.position.y, finishPos.y*2);
        print("rjiwajrioaw");
    }
}