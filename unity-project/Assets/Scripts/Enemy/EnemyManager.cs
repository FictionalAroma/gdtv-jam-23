﻿using System;
using CommonComponents;
using CommonComponents.Interfaces;
using Environment;
using UI;
using UnityEngine;

namespace Enemy
{
	[RequireComponent(typeof(EnemyMover))]
	[RequireComponent(typeof(SliderDisplay))]
	public class EnemyManager : Damagable
	{
		private EnemyMover _mover;
		private SliderDisplay _hpBar;

		protected override void Awake()
		{
			base.Awake();
			_hpBar = GetComponent<SliderDisplay>();
			HPChanged += _hpBar.SetValues;

			_mover = GetComponent<EnemyMover>();

			HPEmpty += OnDeath;
		}

		private void OnDeath()
		{
			Destroy(this.gameObject);
		}
	}
}