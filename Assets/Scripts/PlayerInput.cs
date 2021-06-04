using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Motor))]
public class PlayerInput : MonoBehaviour, IDroppable, ICantShoot
{

    Vector3 moveInput;
    Motor motor;

    public Gun handGun;
    public Gun gun;
    bool hasGun = false;
    int gunType;
    public Transform hand;
    bool canShoot = true;
    bool canMove = true;

    public GunPickup gunPickupPrefab;

    Camera mainCam;

    AudioSource audio;

    // Start is called before the first frame update
    void Awake()
    {
        motor = GetComponent<Motor>();
        audio = GetComponent<AudioSource>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        }
        else
        {
            moveInput = Vector3.zero;
        }

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
            if (canShoot)
            {
                if (hasGun)
                {
                    // putting this here because I don't want ranged enemies to click
                    // I guess they won't. They reload before they hit 0
                    // also, then I can just attach this to the player
                    // and don't need to swap audio clips around
                    if (gun.ShouldPlayClick())
                    {
                        if(!audio.isPlaying)
                            audio.Play();
                    }                        
                    gun.Fire();
                    
                }
                else
                {
                    handGun.Fire();
                }
            }
            else
            {
                // click sound
                // gun.ClickSound()
                // also called internally for out of ammo
                // is that confusing?
            }

        }
        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Q))
        {
            // drop with right click or Q(like minecraft)
            //FindObjectOfType<MapGrid>().MoveCircle(transform.position, 1.5f, false);
            DropGun();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(hasGun)
                gun.Reload();
            handGun.Reload();
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            Health h = GetComponent<Health>();
            if(h)
            {
                h.Reset();
            }
            Rigidbody rb = GetComponent<Rigidbody>();
            if(rb)
            {
                // might fix it if you're falling out of the world?
                rb.velocity = Vector3.zero;
            }
        }

    }

    /// <summary>
    /// Gives a gun. Returns false if already has a gun
    /// </summary>
    /// <param name="newGun"></param>
    /// <param name="gunType"></param>
    /// <returns></returns>
    public bool GiveGun(Gun newGun, int gunType)
    {
        if (!hasGun)
        {
            gun = Instantiate(newGun, hand.position, hand.rotation, hand);
            this.gunType = gunType;
            hasGun = true;
            return true;
        }
        return false;
    }

    public void DropGun()
    {
        if (hasGun)
        {
            hasGun = false;
            GunPickup copy = Instantiate(gunPickupPrefab, transform.position + Vector3.back * 2, transform.rotation);
            // prefab already has a gun
            // if I give them mine, it messes up when I delete mine
            // I wanted to do it this way so it can work if I have different guns
            // maybe I'll have to do
            //copy.gunType = gun.type
            // and then the prefab has all possible guns

            copy.SetGun(gunType);

            //copy.gun = gun; 
            Destroy(gun.gameObject);
        }
    }

    public void CanShoot(bool able)
    {
        canShoot = able;
    }

    public void CanMove(bool able)
    {
        canMove = able;
    }
}
