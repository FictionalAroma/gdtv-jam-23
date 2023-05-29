using CommonComponents.Interfaces;
using UnityEngine;

namespace CommonComponents
{
	public class Explosion : MonoBehaviour , IDamageDealer
	{
		public float Damage { get; set; }
		public SphereCollider sphereCollider;
		ParticleSystem[] explosionParticles;
		private void Start()
		{
			explosionParticles = GetComponentsInChildren<ParticleSystem>();
			foreach (var t in explosionParticles)
			{
				t.Play();
			}
			sphereCollider.radius = 4f;
			Destroy(this.gameObject, 1f);
		}

		private void OnDisable()
		{
			foreach (var particle in explosionParticles)
			{
				particle.Pause(true);
			}
		}

		private void OnEnable()
		{
			foreach (var particle in explosionParticles)
			{
				particle.Play(true);
			}
		}

	}
}
