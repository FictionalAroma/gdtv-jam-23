using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndObject : MonoBehaviour
{
    [SerializeField] ObjectiveDoor finalDoor;
    Collider _levelEndObjectCollider;
    private void Awake()
    {
        _levelEndObjectCollider = GetComponent<Collider>();
        _levelEndObjectCollider.enabled = false;
    }
    private void Update()
    {
        finalDoor.OnOpen += FinalDoorOpen;
    }

    public void FinalDoorOpen()
    {
        _levelEndObjectCollider.enabled = true;
    }
}
