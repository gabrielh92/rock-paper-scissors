using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
    Blaster,
    Pistol,
    MachineGun
}

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponType weaponType;

    [SerializeField] Vector2 bulletSize;
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxBulletDistance;

    [SerializeField] float shotRecoil;
    [SerializeField] float enemyRecoil;

    [SerializeField] float shotCooldown_ms;
    [SerializeField] bool canHoldShots = false;

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] Vector2 holdingPos;
    [SerializeField] float playerTrackSpeed = 3f;

    Rigidbody2D rb;
    SpriteRenderer rend;

    bool pickedUp = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            transform.SetParent(other.gameObject.transform);
            GetComponent<BoxCollider2D>().enabled = false;
            pickedUp = true;
        }
    }

    private void Update() {
        // Move weapon to player's hands
        if(pickedUp && Vector2.Distance(transform.localPosition, holdingPos) > 0.01f) {
            float time = Time.deltaTime;
            // uses a unit time function (I think?) to give the floaty feelin
            // time = time * time * (3f - 2f * time);
            transform.localPosition = Vector2.Lerp(transform.localPosition, holdingPos, time * playerTrackSpeed);
        }
    }

    public void LookAt(Vector2 pos) {
        // point the gun at the mouse pos
        Vector2 lookDir = pos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        rend.flipY = Mathf.Abs(angle) > 90;
    }

    public void Fire(Vector2 at) {
        Debug.Log("Fire!");
    }

    public Vector2 ClampCrosshair(Vector2 pos) {
        return Vector2.Lerp(transform.position, pos, maxBulletDistance);
    }
}
