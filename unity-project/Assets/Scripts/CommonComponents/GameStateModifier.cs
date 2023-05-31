using System;
using System.Collections.Generic;
using CommonComponents.Interfaces;
using UnityEngine;

namespace CommonComponents
{
    public class GameStateModifier : MonoBehaviour, IGameStateSubscriber
	{
		[SerializeField] private List<GameState> activeStateList;
		private MonoBehaviour[] _behaviourList;
		private void Start()
		{
			SingletonRepo.StateManager.Subscribe(this);
			_behaviourList = GetComponents<MonoBehaviour>();
		}

		private void OnDestroy()
		{
			SingletonRepo.StateManager.Unsubscribe(this);
		}

		public void OnStateChange(GameState newState)
		{
			bool enable = activeStateList.Contains(newState);
			foreach (var mb in _behaviourList)
			{
				mb.enabled = enable;
			}
		}
	}
}