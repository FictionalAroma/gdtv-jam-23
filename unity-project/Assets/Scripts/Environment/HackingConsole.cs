using System;
using System.ComponentModel;
using CommonComponents;
using CommonComponents.Interfaces;
using Management;

namespace Environment
{
	public enum HackingDifficulty
	{
		None,
		Easy,
		Medium,
		Hard,
	}

	public class HackingConsole : Interactable
	{

		public bool ActiveState { get; set; }
		
		private event Action OnActivate;
		public HackingDifficulty hackingSceneDifficulty = HackingDifficulty.Easy;
		public override void Action(InteractableActor actor)
		{
			if (hackingSceneDifficulty != HackingDifficulty.None)
			{
				LevelLoader.LoadHacking(this);
			}
			else
			{
				TurnOnConsole();
			}

		}

		public void TurnOnConsole()
		{
			ActiveState = true;
			OnActivate?.Invoke();

		}

		public void Subscribe(Action act) => OnActivate += act;
	}
}