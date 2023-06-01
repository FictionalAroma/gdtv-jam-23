using CommonComponents;
using TMPro;
using UnityEngine;

namespace Hacking
{
	public class HackingSpawner : MonoBehaviour
	{
		public GameObject[] thingsToSpawn;
		public Vector2 spawnPosition;
		public float spawnPosMaxY;
		public float spawnPosMinY;
		public float spawnPosMaxX;
		public float spawnPosMinX;
		public float spawnTimer;
		public float spawnTime;
		[SerializeField] private HackingManager mangager;

		void Start()
		{
			this.transform.position = spawnPosition;
		}


		// Update is called once per frame
		void Update()
		{
			if (spawnTimer <=0)
			{
				var enemy = Instantiate(thingsToSpawn[Random.Range(0, thingsToSpawn.Length)], new Vector3(Random.Range(spawnPosMinX, spawnPosMaxX), Random.Range(spawnPosMinY, spawnPosMaxY), 0), Quaternion.identity, mangager.transform);
				if (enemy.TryGetComponent<HackingEnemyController>(out var con))
				{
					con.HPEmpty += mangager.EnemyDead;
				}
				spawnTimer = spawnTime;
			}
			spawnTimer -= Time.deltaTime;


		}

	}
}
