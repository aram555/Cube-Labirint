using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTraffic : MonoBehaviour
{
    [Header("Booleans")]
    [SerializeField] private bool start;
    [SerializeField] private bool end;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(start) anim.Play("Start");
        else if(end) anim.Play("End");
        else anim.Play("Idle");
    }

    public void StartTraffic() {
        start = false;
    }
    public void DestroyWall() {
        Destroy(this.transform.parent.gameObject);
    }

    public void Traffic(bool startTraffic, bool endTraffic) {
        start = startTraffic;
        end   = endTraffic;
    }
}
