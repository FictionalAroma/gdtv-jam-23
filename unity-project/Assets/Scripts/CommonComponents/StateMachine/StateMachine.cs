using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommonComponents.StateMachine
{
	public abstract class StateMachine<E, T> : MonoBehaviour where E : Enum 
																where T : BaseState<E>
	{
		protected readonly Dictionary<E, T> StateDictionary = new Dictionary<E, T>();
		protected BaseState<E> CurrentState = null;

		protected virtual void Awake()
		{

		}

		public virtual bool AddState(T newState)
		{
            CurrentState ??= newState;
			return StateDictionary.TryAdd(newState.State, newState);
		}

		public void Update()
		{
			if (CurrentState != null)
			{
				var newStateEnum = CurrentState.Tick();
				TrySwapState(newStateEnum);
			}

		}
		public virtual T TrySwapState(E newStateEnum)
		{
			if (!newStateEnum.Equals(CurrentState.State))
			{
				return SwapState(newStateEnum);
			}
			return (T)CurrentState;

		}

		public virtual T SwapState(E newStateEnum)
		{
			CurrentState.Deactivate();
			CurrentState = StateDictionary[newStateEnum];
			CurrentState.Activate();
			return (T)CurrentState;
		}

	}

}