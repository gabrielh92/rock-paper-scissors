using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shouriken : MonoBehaviour
{
    [SerializeField] float force = 25f;

    Rigidbody2D rb;
    Animator anim;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void Fly(Vector2 at) {
        var dir = at - (Vector2)transform.position;
        dir.Normalize();
        rb.AddForce(dir * force, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Wall") {
            rb.velocity = Vector2.zero;
            anim.SetBool("Hit", true);
            // todo emit event
            if (Events != null) {
                Events((Vector2)transform.position);
            }
        }
    }

    public delegate void PublishShourikenHit(Vector2 shourikenPos);
    public static event PublishShourikenHit Events;


}
