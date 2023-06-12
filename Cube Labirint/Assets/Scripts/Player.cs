using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float HP;
    public float speed;
    public int ammo;

    Rigidbody rb;
    Camera cam;
    public MazeSpawner mazeSpawner;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0) Restart();

        if(Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) {
                if(hit.collider.CompareTag("Enemy")) {
                    ammo--;
                    
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    enemy.GetDamage(ammo);
                }
            }
        }
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
            mazeSpawner.Create();
            transform.position = new Vector3(0, 10, 0);
        }
        
    }
}
