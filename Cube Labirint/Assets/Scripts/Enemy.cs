using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float navTimer;
    public float damage;
    
    [Header("Enemy Type")]
    public EnemyTypes enemy;
    public enum EnemyTypes {Simpleton, Experienced, Deathly}
    [Header("EXP Pos")]
    public Transform firstPos;
    public Transform secondPos;

    private bool damaged;
    private float damageTimer;
    NavMeshAgent agent;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        switch(enemy) {
            case EnemyTypes.Simpleton:
                Simpleton();
                break;
            case EnemyTypes.Experienced:
                Experienced();
                break;
            case EnemyTypes.Deathly:
                Deathly();
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //Enemy Type
    private void Simpleton() {
        StartCoroutine(Destinate());
    }
    private void Experienced() {
        GameObject[] destinatePoints = GameObject.FindGameObjectsWithTag("Cell");
        int first  = Random.Range(0, destinatePoints.Length);
        int second = Random.Range(0, destinatePoints.Length);

        firstPos  = destinatePoints[first].transform;
        secondPos = destinatePoints[second].transform;

        StartCoroutine(Destinate(firstPos, secondPos));
    }
    private void Deathly() {
        player = GameObject.Find("Player");
        StartCoroutine (Destinate(player));
    }

    //Restart and Damage

    private void Set(bool set) {
        GetComponent<BoxCollider>().enabled = set;
        GetComponent<NavMeshAgent>().enabled = set;
    }

    public void GetDamage(int Ammo) {
        Set(false);
        StartCoroutine(RestartEnemy());
    }

    public void Distract(int DistractAmmo) {
        GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");
        int random = Random.Range(0, cell.Length);

        agent.SetDestination(cell[random].transform.position);
    }
    
    public void DestroyEnemy(int DeathAmmo) {
        Set(false);
    }

    private void Restart() {
        Set(true);
    }

    IEnumerator RestartEnemy() {
        yield return new WaitForSeconds(5);
        Restart();
    }

    //Destinate Functions
    private IEnumerator Destinate() {
        while(true) {
            if(!agent.enabled) break;
            GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");
            int random = Random.Range(0, cell.Length);

            agent.SetDestination(cell[random].transform.position);

            yield return new WaitForSeconds(navTimer);
        }
    }
    private IEnumerator Destinate(Transform firstDestinatePoints, Transform secondDestinatePoints) {
        while(true) {
            if(!agent.enabled) break;

            agent.SetDestination(firstDestinatePoints.position);
            yield return new WaitForSeconds(navTimer);

            agent.SetDestination(secondDestinatePoints.position);
            yield return new WaitForSeconds(navTimer);
        }
    }
    private IEnumerator Destinate(GameObject Player) {
        while(true) {
            if(!agent.enabled) break;
            agent.SetDestination(Player.transform.position);

            yield return new WaitForSeconds(navTimer);
        }
    }
}
