using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float HP;
    [SerializeField] private float speed;
    [Header("Joystick")]
    [SerializeField] private Joystick joystick;
    [Header("Teleport")]
    [SerializeField] private int cost;
    [SerializeField] private float distance;
    [SerializeField] private bool teleport;
    [Header("Weapons")]
    [SerializeField] private bool freeze;
    [SerializeField] private bool destroy;
    [SerializeField] private bool distracting;
    [SerializeField] private int ammo;
    [SerializeField] private int deathAmmo;
    [SerializeField] private int distractingAmmo;
    [Header("Spec Weapons")]
    [SerializeField] private bool destroyWall;
    [SerializeField] private int destroyCount;
    [Header("Particles")]
    [SerializeField] private Color freezeColor;
    [SerializeField] private Color destroyColor;
    [SerializeField] private Color distractColor;
    [SerializeField] private GameObject hitPartcle;
    [SerializeField] private GameObject destroyhWallParticle;

    Rigidbody rb;
    Camera cam;
    MazeSpawner mazeSpawner;
    GameObject[] enemys;
    // Start is called before the first frame update
    void Start()
    {
        rb              = GetComponent<Rigidbody>();
        cam             = Camera.main;
        mazeSpawner     = GameObject.Find("Game Manager").GetComponent<MazeSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) mazeSpawner.Create();
        if (HP <= 0) Restart();

        //Fire and Teleport
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
                if (teleport) {
                    if (hit.collider.GetComponent<Cell>() && Vector3.Distance(transform.position, hit.point) < distance) {
                        transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
                        teleport = false;
                    }
                    else return;
                }

                if (destroyWall && destroyCount >= 1) {
                    if (hit.collider.name == "Left" || hit.collider.name == "Bottom") {
                        Instantiate(destroyhWallParticle, hit.point, Quaternion.Euler(-90, 0, 0));
                        Destroy(hit.collider.gameObject);
                        destroyWall = false;
                        destroyCount--;
                    }
                }
                
                if (hit.collider.CompareTag("Enemy")) {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (freeze) {
                        if (ammo <= 0 || enemy.enemy != Enemy.EnemyTypes.Simpleton) return;

                        var main = hitPartcle.GetComponent<ParticleSystem>().main;
                        main.startColor = freezeColor;
                        Instantiate(hitPartcle, hit.point, Quaternion.Euler(-90, 0, 0));

                        ammo--;
                        enemy.GetDamage(ammo);
                        SetEnemySpeed(enemys, 5, false);
                        freeze = false;
                    }
                    else if (destroy) {
                        if (deathAmmo <= 0 || enemy.enemy == Enemy.EnemyTypes.Deathly) return;

                        var main = hitPartcle.GetComponent<ParticleSystem>().main;
                        main.startColor = destroyColor;
                        Instantiate(hitPartcle, hit.point, Quaternion.Euler(-90, 0, 0));

                        deathAmmo--;
                        enemy.DestroyEnemy(deathAmmo);
                        SetEnemySpeed(enemys, 5, false);
                        destroy = false;
                    }
                    else if (distracting) {
                        if (distractingAmmo <= 0 || enemy.enemy == Enemy.EnemyTypes.Deathly) return;

                        var main = hitPartcle.GetComponent<ParticleSystem>().main;
                        main.startColor = distractColor;
                        Instantiate(hitPartcle, hit.point, Quaternion.Euler(-90, 0, 0));

                        distractingAmmo--;
                        enemy.Distract(distractingAmmo);
                        SetEnemySpeed(enemys, 5, false);
                        distracting = false;
                    }
                }
            }
        }
    }
    private void FixedUpdate() {
        Move();
    }
    void Move() {
        float x = joystick.Horizontal;
        float z = joystick.Vertical;

        rb.velocity = new Vector3(z * speed, rb.velocity.y, -x * speed);
    }

    //Buttons
    public void Teleport() {
        teleport = true;
    }
    public void DestroyWall() {
        destroyWall = true;
    }
    public void ResetPos() {
        transform.position = new Vector3(0, 3, 0);
    }

    private void Restart() {
        SceneManager.LoadScene(0);
    }
    private void OnCollisionEnter(Collision other) {
        Enemy enemy   = other.collider.GetComponent<Enemy>();
        if(enemy) HP -= enemy.damage;

        if(other.collider.CompareTag("Finish")) {
            ammo = 10;
            mazeSpawner.Create();
            ResetPos();
        }

        if(other.collider.CompareTag("Ground")) ResetPos();
    }

    private GameObject[] GetEnemy(GameObject[] enemy) {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");

        return enemy;
    }
    private void SetEnemySpeed(GameObject[] e, int n, bool set) {
        foreach(GameObject E in e) {
            E.GetComponent<NavMeshAgent>().speed = n;
            E.GetComponent<Enemy>().isDamage = set;
        }
    }
    //Weapons
    public void Freeze() {
        enemys = GetEnemy(enemys);
        SetEnemySpeed(enemys, 0, true);

        freeze      = true;
        destroy     = false;
        distracting = false;
    }
    public void DestroyEnemy() {
        enemys = GetEnemy(enemys);
        SetEnemySpeed(enemys, 0, true);

        destroy     = true;
        freeze      = false;
        distracting = false;
    }
    public void Distract() {
        enemys = GetEnemy(enemys);
        SetEnemySpeed(enemys, 0, true);

        destroy     = false;
        freeze      = false;
        distracting = true;
    }
}
