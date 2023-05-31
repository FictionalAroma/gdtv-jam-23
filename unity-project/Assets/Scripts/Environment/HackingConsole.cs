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
			ActiveState = true;
			if (hackingSceneDifficulty != HackingDifficulty.None)
			{
				LevelLoader.LoadHacking(hackingSceneDifficulty);

			}

			OnActivate?.Invoke();
		}

		public void Subscribe(Action act) => OnActivate += act;
	}
}