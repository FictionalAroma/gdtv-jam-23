using Player;
using UnityEngine;

namespace Enemy.Boss
{
	public class BossStateContext
	{

		public Animator animator => EnemyManager.BossAnimator;
		public PlayerController PlayerCache { get; set; }
		public BossManager EnemyManager { get; set; }

		public BossMover Attacker { get; set; }
	}
}
