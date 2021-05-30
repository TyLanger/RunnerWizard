using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Motor))]
public class PlayerInput : MonoBehaviour
{

    Vector3 input;
    Motor motor;

    // Start is called before the first frame update
    void Awake()
    {
        motor = GetComponent<Motor>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        motor.MoveTo(input);

        if(Input.GetButtonDown("Jump"))
        {
            FindObjectOfType<MapGrid>().MoveSquare(transform.position, 4, true);
        }
    }
}
