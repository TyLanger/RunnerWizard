using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Motor))]
public class PlayerInput : MonoBehaviour
{

    Vector3 moveInput;
    Motor motor;

    public Gun gun;
    bool hasGun = false;
    public Transform hand;

    public GunPickup gunPickupPrefab;

    Camera mainCam;

    // Start is called before the first frame update
    void Awake()
    {
        motor = GetComponent<Motor>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        motor.MoveTo(moveInput.normalized);

        // look at mouse
        Ray CameraRay = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane eyePlane = new Plane(Vector3.up, Vector3.zero);

        if (eyePlane.Raycast(CameraRay, out float cameraDist))
        {
            Vector3 lookPoint = CameraRay.GetPoint(cameraDist);
            motor.SetLookAt(new Vector3(lookPoint.x, transform.position.y, lookPoint.z));
        }

        if (Input.GetButtonDown("Jump"))
        {
            //FindObjectOfType<MapGrid>().MoveCircle(transform.position, 1.5f, true);
        }
        if(Input.GetButton("Fire1"))
        {
            if(hasGun)
                gun.Fire();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            //FindObjectOfType<MapGrid>().MoveCircle(transform.position, 1.5f, false);
            DropGun();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(hasGun)
                gun.Reload();
        }
    }

    public void GiveGun(Gun newGun)
    {
        gun = Instantiate(newGun, hand.position, hand.rotation, hand);
        hasGun = true;
    }

    public void DropGun()
    {
        hasGun = false;
        GunPickup copy = Instantiate(gunPickupPrefab, transform.position + Vector3.back * 2, transform.rotation);
        // prefab already has a gun
        // if I give them mine, it messes up when I delete mine
        // I wanted to do it this way so it can work if I have different guns
        // maybe I'll have to do
        //copy.gunType = gun.type
        // and then the prefab has all possible guns

        //copy.gun = gun; 
        Destroy(gun.gameObject);
    }
}
