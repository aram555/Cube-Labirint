using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float HP;
    public float speed;

    Rigidbody rb;
    public MazeSpawner mazeSpawner;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0) Destroy(this.gameObject);
    }

    private void FixedUpdate() {
        Move();
    }

    void Move() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        rb.AddForce(new Vector3(x * speed, rb.velocity.y, z * speed));
    }

    private void OnCollisionEnter(Collision other) {
        Enemy enemy = other.collider.GetComponent<Enemy>();
        if(enemy) HP -= enemy.damage;

        if(other.collider.CompareTag("Finish")) {
            mazeSpawner.Create();
            transform.position = new Vector3(0, 0, 0);
        }
        
    }
}
