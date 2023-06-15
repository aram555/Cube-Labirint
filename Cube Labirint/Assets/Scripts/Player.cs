using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float HP;
    public float speed;
    [Header("Jump")]
    public float jumpSpeed;
    public float jumpDistance;
    public float jumpHeight;
    public Vector3 jumpPos;
    public bool jump;
    public bool jumping;
    [Header("Weapons")]
    public bool freeze;
    public bool destroy;
    public bool distracting;
    public int ammo;
    public int deathAmmo;
    public int distractingAmmo;

    Rigidbody rb;
    Camera cam;
    MazeSpawner mazeSpawner;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        mazeSpawner = GameObject.Find("Game Manager").GetComponent<MazeSpawner>();
        jumpPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0) Restart();

        //Fire
        if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
                if(jump) {
                    jumpPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    if(Vector3.Distance(transform.position, jumpPos) > jumpDistance) return;

                    jumping = true;
                    //jump = false;
                }

                if(hit.collider.CompareTag("Enemy")) {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if(freeze) {
                        if(ammo <= 0 || enemy.enemy != Enemy.EnemyTypes.Simpleton) return;

                        ammo--;
                        enemy.GetDamage(ammo);
                        freeze = false;
                    }
                    else if(destroy) {
                        if(deathAmmo <= 0 || enemy.enemy != Enemy.EnemyTypes.Simpleton) return;

                        deathAmmo--;
                        enemy.DestroyEnemy(deathAmmo);
                        destroy = false;
                    }
                    else if(distracting) {
                        if(distractingAmmo <= 0 || enemy.enemy != Enemy.EnemyTypes.Simpleton) return;

                        distractingAmmo--;
                        enemy.Distract(distractingAmmo);
                        distracting = false;
                    }
                }
            }
        }

        if(jumping) {
            Jump(jumpPos);
            if(Vector3.Distance(transform.position, jumpPos) <= 1.2f) jumping = false; 
        }
    }

    //Jump
    public void JumpButton() {
        jump = true;
    }
    private void Jump(Vector3 dir) {
        float posY = Mathf.Abs(dir.z - transform.position.z);
        Vector3 jumpPos = new Vector3(dir.x, jumpHeight + posY, dir.z);
        transform.position = Vector3.MoveTowards(transform.position, jumpPos, jumpSpeed * Time.deltaTime);
    }

    private void Restart() {
        SceneManager.LoadScene(0);
    }

    private void FixedUpdate() {
        Move();
    }
    void Move() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(x * speed, rb.velocity.y, z * speed);
    }

    private void OnCollisionEnter(Collision other) {
        Enemy enemy = other.collider.GetComponent<Enemy>();
        if(enemy) HP -= enemy.damage;

        if(other.collider.CompareTag("Finish")) {
            ammo = 10;
            mazeSpawner.Create();
            transform.position = new Vector3(0, 10, 0);
        }
        
    }

    //Weapons
    public void Freeze() {
        freeze = true;
        destroy = false;
    }
    public void DestroyEnemy() {
        destroy = true;
        freeze = false;
    }
}
