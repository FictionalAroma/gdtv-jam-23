using Environment;
using Management;
using UnityEngine;

public class LevelEndObject : MonoBehaviour
{
    [SerializeField] ObjectiveDoor finalDoor;
    Collider _levelEndObjectCollider;
    LevelLoader levelLoader;
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelLoader.LoadNextScene();
        }
    }
}
