using Environment;
using Management;
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
    private void Start()
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
            LevelLoader.LoadNextScene();
        }
    }
	private void OnCollisionEnter(Collision other)
	{

		if (other.gameObject.CompareTag("Player"))
		{
			LevelLoader.LoadNextScene();
		}
	}

}
