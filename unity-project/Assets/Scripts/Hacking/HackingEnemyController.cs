using CommonComponents;
using CommonComponents.Interfaces;
using Hacking.Player;
using UnityEngine;

namespace Hacking
{
	public class HackingEnemyController : Damagable, IDamageDealer
	{
		[field: SerializeField] public float Damage { get; private set; }
    
		public Vector2 moveVector;
		public float moveThrust;
		HackingPlayerController hackingPlayerScrip;
		public float searchTimer;
		public float searchTime;
		new Rigidbody2D rigidbody2D;
		public HackingBullet hackingBullet;   
		void Start()
		{
			searchTimer = searchTime;
			hackingPlayerScrip = FindObjectOfType<HackingPlayerController>();
			rigidbody2D = GetComponent<Rigidbody2D>();

			HPEmpty += OnDead;
		}

		private void OnDead(Damagable obj)
		{
			Destroy(gameObject, 0.2f);
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<Collider2D>().enabled = false;
		}

		// Update is called once per frame
		void Update()
		{
			if (searchTimer<=0)
			{
				MoveAndShootTarget();
           
			}
			else
			{
				SearchTarget();
			}
		}

		private void MoveAndShootTarget()
		{
			moveVector = hackingPlayerScrip.transform.position - this.transform.position;
			rigidbody2D.AddForce(moveVector.normalized * (moveThrust * Time.deltaTime), ForceMode2D.Impulse);
			var bullet = Instantiate(hackingBullet, transform.position, Quaternion.identity);
			bullet.moveVector = moveVector.normalized;
			searchTimer = searchTime;
		}

		private void SearchTarget()
		{
			searchTimer -= Time.deltaTime;
		}
	}
}
