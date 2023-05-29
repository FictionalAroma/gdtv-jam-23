using Player;
using UnityEngine;

namespace Enemy.Boss
{
	public class BossStateContext
	{

		public Animator animator => EnemyManager.enemyAnimator;
		public PlayerController PlayerCache { get; set; }
		public EnemyManager EnemyManager { get; set; }

		public BossMover Attacker { get; set; }
	}
}
