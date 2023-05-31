using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float navTimer;
    public float damage;
    private float newNavTimer;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Destinate();

        newNavTimer = navTimer;
    }

    // Update is called once per frame
    void Update()
    {
        navTimer -= Time.deltaTime;
        if(navTimer <= 0) {
            Destinate();
            navTimer = newNavTimer;
        }
    }

    private void Destinate() {
        GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");
        int random = Random.Range(0, cell.Length);

        agent.SetDestination(cell[random].transform.position);
    }
}
