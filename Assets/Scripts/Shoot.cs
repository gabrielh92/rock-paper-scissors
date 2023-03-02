using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject shourikenPrefab;

    Rigidbody2D rb;
    Camera cam;
    Weapon currentWeapon;

    Vector2 mousePos;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        currentWeapon = null;
    }

    private void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        crosshair.transform.position = (currentWeapon != null) ?
            currentWeapon.ClampCrosshair(mousePos) : shourikenClampCrosshair(mousePos);

        // throw shouriken

        if(currentWeapon != null) {
            if(Input.GetMouseButtonDown(0)) {
                currentWeapon.Fire(mousePos);
            }
        } else {
            if(Input.GetMouseButtonDown(0)) {
                throwShouriken(mousePos);
            }
        }
    }

    private void FixedUpdate() {
        if(currentWeapon != null) {
            currentWeapon.LookAt(mousePos);
        }
    }

    private void throwShouriken(Vector2 pos) {
        Vector3 ipos = Vector2.Lerp(transform.position, pos, .75f);;
        ipos.z = -0.01f;
        var shouriken = GameObject.Instantiate(shourikenPrefab, ipos ,Quaternion.identity);
        Debug.DrawLine(ipos, pos, Color.white, Mathf.Infinity);
        shouriken.GetComponent<Shouriken>().Fly(pos);
    }

    private Vector2 shourikenClampCrosshair(Vector2 pos) {
        return Vector2.Lerp(transform.position, pos, 0.95f);
    }

    // ** Weapon pickup system
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Weapon") {
            currentWeapon = other.GetComponent<Weapon>();

        }
    }
}
