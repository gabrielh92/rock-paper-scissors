using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShourikenHear : MonoBehaviour
{
    [SerializeField] float shourikenHearingRange = 8f;
    [SerializeField] float delayBeforeReaction_s = 1f;
    [SerializeField] float investigationTimeout_s = 6f;
    [SerializeField] float investigationDistance = 0.5f;
    [SerializeField] float moveSpeed = 3f;

    FieldOfView fov;
    SpriteRenderer rend;
    Rigidbody2D rb;
    Animator anim;
    Patrol patrol;

    Vector3 originalPos;
    Vector3 shourikenPos;
    float investigationTimer = 0f;

    enum AlertState {
        Nada,
        Heard,
        HeadingOver,
        Investigating,
        HeadingBack,
    }

    AlertState alertState;

    private void OnEnable() {
        Shouriken.Events += ShourikenHit;
    }

    private void OnDisable() {
        Shouriken.Events -= ShourikenHit;
    }

    private void Start() {
        fov = GetComponentInChildren<FieldOfView>();
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        patrol = GetComponent<Patrol>();
    }

    private void Update() {
        switch(alertState) {
            case AlertState.Heard:
            originalPos = transform.position;
            if(patrol != null) {
                patrol.shouldPatrol = false;
            }

            // look at shouriken hit
            fov.transform.LookAt(shourikenPos, Vector3.forward);
            fov.transform.Rotate(new Vector3(0, 0, fov.ViewAngleForward));

            // flip renderer if needed
            rend.flipX = (fov.transform.eulerAngles.z > 180);
            break;

            case AlertState.HeadingOver:
            // todo fix lol
            transform.position = Vector3.Lerp(transform.position, shourikenPos, moveSpeed * Time.deltaTime);
            if(Vector2.Distance((Vector2)transform.position, shourikenPos) < investigationDistance) {
                alertState = AlertState.Investigating;
                investigationTimer = Time.deltaTime;
                anim.SetBool("Investigate", true);
            }
            break;

            case AlertState.HeadingBack:
            // todo fix lol
            transform.position = Vector3.Lerp(transform.position, originalPos, moveSpeed * Time.deltaTime);
            if(Vector2.Distance((Vector2)transform.position, (Vector2)originalPos) < Mathf.Epsilon) {
                if(patrol != null) {
                    patrol.shouldPatrol = true;
                }

                alertState = AlertState.Nada;
            }
            break;

            case AlertState.Investigating:
            if(1000 * (Time.deltaTime - investigationTimer) > investigationTimeout_s) {
                alertState = AlertState.HeadingBack;
                anim.SetBool("Investigate", false);
            }
            break;

            case AlertState.Nada:
            default:
            break;
        }
    }

    void ShourikenHit(Vector2 _shourikenPos) {
        if(Vector2.Distance(_shourikenPos, fov.transform.position) < shourikenHearingRange) {
            shourikenPos = _shourikenPos;
            StartCoroutine(ShourikenHit());

            // check if there's direct line of sight
            // var hit = Physics2D.Raycast(transform.position, (Vector2)transform.position - shourikenPos);
            // if(hit.transform.tag == "Shouriken" || hit.transform.tag == "Player") {
                // StartCoroutine(ShourikenHit());
            // }
        }
    }

    private IEnumerator ShourikenHit() {
        alertState = AlertState.Heard;
        yield return new WaitForSeconds(delayBeforeReaction_s);
        alertState = AlertState.HeadingOver;
    }

    private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(transform.position, shourikenHearingRange);
    }

}
