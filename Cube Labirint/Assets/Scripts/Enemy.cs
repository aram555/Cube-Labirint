using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float navTimer;
    public float damage;
    public bool isDamage;
    
    [Header("Enemy Type")]
    public EnemyTypes enemy;
    public enum EnemyTypes {Simpleton, Experienced, Deathly}
    [Header("EXP Pos")]
    public Transform firstPos;
    public Transform secondPos;
    [Header("Colors")]
    [SerializeField] Color destroyColor;
    [SerializeField] Color freezeColor;
    [SerializeField] Color selectColor;

    private bool damaged;
    private float damageTimer;
    NavMeshAgent agent;
    GameObject player;

    Renderer rend;
    Shader enemyLit;
    Shader holograph;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rend  = GetComponent<Renderer>();

        enemyLit  = Shader.Find("Universal Render Pipeline/Lit");
        holograph = Shader.Find("Shader Graphs/Holograph");

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
        if(isDamage && agent.enabled) {
            rend.material.shader = holograph;
            rend.material.SetColor("_MainColor", selectColor);
        }
        else if(agent.enabled) rend.material.shader = enemyLit;
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
        GetComponent<BoxCollider>().enabled  = set;
        GetComponent<NavMeshAgent>().enabled = set;
    }

    public void GetDamage(int Ammo) {
        Set(false);
        StartCoroutine(RestartEnemy());
        rend.material.shader = holograph;
        rend.material.SetColor("_MainColor", freezeColor);
    }
    public void Distract(int DistractAmmo) {
        GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");
        int random = Random.Range(0, cell.Length);

        agent.SetDestination(cell[random].transform.position);
    }
    
    public void DestroyEnemy(int DeathAmmo) {
        Set(false);
        rend.material.shader = holograph;
        rend.material.SetColor("_MainColor", destroyColor);
    }

    private void Restart() {
        Set(true);
        rend.material.shader = enemyLit;
    }

    IEnumerator RestartEnemy() {
        yield return new WaitForSeconds(10);
        Restart();
    }

    //Destinate Functions
    private IEnumerator Destinate() {
        while(true) {
            if(agent.enabled) {
                GameObject[] cell = GameObject.FindGameObjectsWithTag("Cell");
                int random = Random.Range(0, cell.Length);

                agent.SetDestination(cell[random].transform.position);

                yield return new WaitForSeconds(navTimer);
            } else yield return new WaitForSeconds(1);
        }
    }
    private IEnumerator Destinate(Transform firstDestinatePoints, Transform secondDestinatePoints) {
        while(true) {
            if(agent.enabled) {
                agent.SetDestination(firstDestinatePoints.position);
                yield return new WaitForSeconds(navTimer);

                agent.SetDestination(secondDestinatePoints.position);
                yield return new WaitForSeconds(navTimer);
            } else yield return new WaitForSeconds(1);
        }
    }
    private IEnumerator Destinate(GameObject Player) {
        while(true) {
            if(agent.enabled) {
                agent.SetDestination(Player.transform.position);

                yield return new WaitForSeconds(navTimer);
            } else yield return new WaitForSeconds(1);
        }
    }
}
