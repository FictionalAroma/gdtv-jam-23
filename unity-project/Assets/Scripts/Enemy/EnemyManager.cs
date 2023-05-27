﻿using System;
using CommonComponents;
using CommonComponents.StateMachine;
using Enemy.States;
using UI;
using UnityEngine;

namespace Enemy
{
	[RequireComponent(typeof(SliderDisplay))]
	public class EnemyManager : Damagable
	{
		private EnemyStateMachine _stateMachine;
		private EnemyMover _mover;

		private SliderDisplay _hpBar;

		protected override void Awake()
		{
			_stateMachine = GetComponent<EnemyStateMachine>();


			base.Awake();

			_hpBar = GetComponent<SliderDisplay>();
			HPChangedEvent += _hpBar.SetValues;
			HPEmpty += OnDeath;

			_mover = GetComponent<EnemyMover>();

		}

		private void Start()
		{
			DamageTaken += _stateMachine.DamageTaken;
			_stateMachine.AddState(new EnemyIdleState(gameObject));
			_stateMachine.AddState(new EnemyAttackState(gameObject));


		}

		public void SetTarget(Transform newPos)
		{
			_mover.Target = newPos;

		}

		private void OnDeath(Damagable damagable)
		{
			Destroy(this.gameObject);
		}

	}

}