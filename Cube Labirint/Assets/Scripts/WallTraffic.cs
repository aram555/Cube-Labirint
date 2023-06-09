using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTraffic : MonoBehaviour
{
    [Header("Spped and ground")]
    public float speed;
    public bool isGround;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGround) GoBottom();
    }

    private void GoBottom() {
        transform.Translate(0, -speed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.collider.CompareTag("Ground")) {
            isGround = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}
