using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float ViewAngleForward = 135f;

    [Range(0,360)] public float viewAngle = 30f;
    public float viewRadius = 8f;

    public LayerMask targetMask;

    [SerializeField] SpriteRenderer rend;
    bool prevSpriteState = false;

    private void Update() {
        // keep the view cone pointing in the direction the sprite is facing
        if(rend.flipX != prevSpriteState) {
            prevSpriteState = rend.flipX;
            transform.eulerAngles = new Vector3(0, 0, -1 * transform.eulerAngles.z);
        }

        IsPlayerInView();
    }

    bool IsPlayerInView() {
        Collider2D player = Physics2D.OverlapCircle((Vector2)transform.position, viewRadius, LayerMask.GetMask("Player"));

        if(player != null) {
            Vector3 playerDir = (player.transform.position - transform.position).normalized;

            if(Vector3.Angle(transform.right, playerDir) < viewAngle / 2) {
                Debug.Log($"seen!");
            }
        }

        return false;
    }

    public Vector2 DirFromAngle(float angleDegrees, bool angleIsGlobal) {
        if(!angleIsGlobal) {
            angleDegrees += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Sin((angleDegrees) * Mathf.Deg2Rad), Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
