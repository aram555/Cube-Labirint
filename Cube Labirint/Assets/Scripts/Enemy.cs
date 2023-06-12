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

    private bool damaged;
    private float damageTimer;
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

    public void GetDamage(int Ammo) {
        if(Ammo <= 0) return;

        GetComponent<BoxCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

        StartCoroutine(RestartEnemy());
    }

    private void Restart() {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;
    }

    IEnumerator RestartEnemy() {
        yield return new WaitForSeconds(5);
        Restart();
    }

    private void Destinate() {
        if(!agent.enabled) return;
        GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");
        int random = Random.Range(0, cell.Length);

        agent.SetDestination(cell[random].transform.position);
    }
}
