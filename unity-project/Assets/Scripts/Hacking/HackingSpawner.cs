using CommonComponents;
using TMPro;
using UnityEngine;

namespace Hacking
{
	public class HackingSpawner : Damagable
	{
		public GameObject[] thingsToSpawn;
		public int timesLeftToSpawn;
		public Vector2 spawnPosition;
		public float spawnPosMaxY;
		public float spawnPosMinY;
		public float spawnPosMaxX;
		public float spawnPosMinX;
		public float spawnTimer;
		public float spawnTime;
		public bool isTimeTrial;
		[SerializeField] private TMP_Text hpText;
		[SerializeField] private HackingMangaer mangager;

		void Start()
		{
			this.transform.position = spawnPosition;
			HPChangedEvent += OnHPChangedEvent;
			HPEmpty += mangager.PlayerWin;
		}

		private void OnHPChangedEvent(float changeby, float newhp)
		{
			hpText.text = newhp.ToString("0");
		}

		// Update is called once per frame
		void Update()
		{
			if (spawnTimer <=0)
			{
				if (!isTimeTrial)
				{
					if (timesLeftToSpawn < 0)
					{
						var enemy = Instantiate(thingsToSpawn[Random.Range(0, thingsToSpawn.Length)], new Vector3(Random.Range(spawnPosMinX, spawnPosMaxX), Random.Range(spawnPosMinY, spawnPosMaxY), 0), Quaternion.identity);
						if (enemy.TryGetComponent<HackingEnemyController>(out var con))
						{
							con.HPEmpty += EnemyDestoryed;
						}
						spawnTimer = spawnTime;
					}
				}
				else
				{
					Instantiate(thingsToSpawn[Random.Range(0, thingsToSpawn.Length)], new Vector3(Random.Range(spawnPosMinX, spawnPosMaxX), Random.Range(spawnPosMinY, spawnPosMaxY), 0), Quaternion.identity);
					spawnTimer = spawnTime;
				}
            
            
			}
			spawnTimer -= Time.deltaTime;


		}

		private void EnemyDestoryed(Damagable obj)
		{
			OnDamageTaken(1);
		}
	}
}
