using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    SpriteRenderer rend;
    BoxCollider2D kollider;

    private void Start() {
        rend = GetComponent<SpriteRenderer>();
        kollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            transform.SetParent(other.gameObject.transform);
            rend.enabled = false;
            kollider.size *= 4; // so that it can collide with doors easily
        }

        if(other.gameObject.tag == "Door") {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
