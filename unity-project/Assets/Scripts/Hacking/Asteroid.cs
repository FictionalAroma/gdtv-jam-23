using CommonComponents;
using CommonComponents.Interfaces;
using UnityEngine;

namespace Hacking
{
	public class Asteroid : Damagable, IDamageDealer
	{
		[field: SerializeField] public float Damage { get; private set; }
		[SerializeField] float asteroidPower;
		public Vector2 moveVector;
		new Rigidbody2D rigidbody2D;
		void Start()
		{
			moveVector = new Vector2(Random.Range(0f,1f), Random.Range(0f,1f));
			rigidbody2D = GetComponent<Rigidbody2D>();
			rigidbody2D.AddForce(moveVector*asteroidPower, ForceMode2D.Impulse);

		}

   
	}
}
