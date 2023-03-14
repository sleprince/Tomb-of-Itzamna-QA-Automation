using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public Rigidbody lDoor, rDoor;

    public bool itsATrap;

    private void Start()
    {
        lDoor.useGravity = false;
        lDoor.isKinematic = true;
        rDoor.useGravity = false;
        rDoor.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (itsATrap)
        {
            lDoor.useGravity = true;
            lDoor.isKinematic = false;
            rDoor.useGravity = true;
            rDoor.isKinematic = false;
        }
    }
}
