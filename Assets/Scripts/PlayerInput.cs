using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Motor))]
public class PlayerInput : MonoBehaviour
{

    Vector3 moveInput;
    Motor motor;

    public Gun gun;

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
            transform.LookAt(new Vector3(lookPoint.x, transform.position.y, lookPoint.z));
        }

        if (Input.GetButtonDown("Jump"))
        {
            FindObjectOfType<MapGrid>().MoveCircle(transform.position, 2.5f, true);
        }
        if(Input.GetButton("Fire1"))
        {
            gun.Fire();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            FindObjectOfType<MapGrid>().MoveCircle(transform.position, 2.5f, false);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            gun.Reload();
        }
    }
}
