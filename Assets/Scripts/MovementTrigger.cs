using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTrigger : MonoBehaviour
{
    GameScript script;

    private void Start()
    {
        script = FindObjectOfType<GameScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInput player = other.GetComponent<PlayerInput>();
        if(player)
        {
            script.TriggerTouched();
            Destroy(gameObject);
        }
    }
}
