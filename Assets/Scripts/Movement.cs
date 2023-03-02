using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float runSpeed = 4.5f;
    [SerializeField] float rollSpeed = 16f;
    [SerializeField] float rollCoolDown_s = 3f;

    Rigidbody2D rb;
    SpriteRenderer rend;
    Animator animator;
    Camera cam;

    Vector2 movement;
    Vector2 mousePos;
    bool canRoll = true;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // input
    private void Update() {
        bool rollTrigger = canRoll && (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1));
        animator.SetBool("Roll", rollTrigger);
        if(rollTrigger) {
            StartCoroutine(TimeRollCooldown());
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        rend.flipX = movement.x < 0;
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    // movement
    private void FixedUpdate() {
        Vector2 velocity = (animator.GetCurrentAnimatorStateInfo(0).IsName("Roll")) ? movement * rollSpeed : movement * runSpeed;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private IEnumerator TimeRollCooldown() {
        canRoll = false;
        yield return new WaitForSeconds(rollCoolDown_s);
        canRoll = true;
    }
}
