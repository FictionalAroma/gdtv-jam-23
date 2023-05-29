using Player;
using UnityEngine;

namespace Enemy.Boss
{
	public class BossStateContext
	{

		public Animator animator => Boss.BossAnimator;
		public PlayerController PlayerCache { get; set; }
		public BossManager Boss { get; set; }

		public BossMover Attacker { get; set; }
	}
}
