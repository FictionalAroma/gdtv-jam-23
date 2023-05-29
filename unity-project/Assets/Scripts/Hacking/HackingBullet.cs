using CommonComponents.Interfaces;
using UnityEngine;

namespace Hacking
{
	public class HackingBullet : MonoBehaviour, IDamageDealer
	{
		[field: SerializeField] public float Damage { get; private set; }
		[SerializeField] float bulletSpeed;
		public Vector2 moveVector;
    
		// Update is called once per frame
		void Update()
		{
			Vector2 newPosition = transform.position;
			newPosition += moveVector*Time.deltaTime*bulletSpeed;
			transform.position = newPosition;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			Destroy(this.gameObject);
		}
	}
}
