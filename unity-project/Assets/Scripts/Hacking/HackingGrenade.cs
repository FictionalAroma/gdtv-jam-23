using CommonComponents.Interfaces;
using UnityEngine;

namespace Hacking
{
	public class HackingGrenade : MonoBehaviour, IDamageDealer
	{
		[field: SerializeField] public float Damage { get; private set; }
		[SerializeField] Animator grenadeAnimator;
		public float explosionRadius;
		public float grenadeTimer;
		public float grenadeTime;
		CircleCollider2D circleCollider2D;

		private void Start()
		{
			grenadeAnimator = GetComponent<Animator>();
			grenadeTimer = grenadeTime;
			circleCollider2D = GetComponent<CircleCollider2D>();
		}

		// Update is called once per frame
		void Update()
		{
			grenadeTimer -= Time.deltaTime;
			if (grenadeTimer<= 0)
			{
				ExplodeGrenade();
			}
		}
		private void ExplodeGrenade()
		{
			circleCollider2D.radius = explosionRadius;
			grenadeAnimator.SetTrigger("isExploding");
			Destroy(this.gameObject, 4);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			ExplodeGrenade();
		}
	}
}
