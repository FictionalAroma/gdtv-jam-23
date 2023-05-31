using System;
using CommonComponents;
using CommonComponents.Interfaces;
using Management;

namespace Environment
{
	public class HackingConsole : Interactable
	{
		public bool ActiveState { get; set; }
		
		private event Action OnActivate;
		public string hackingSceneDifficulty;
		public override void Action(InteractableActor actor)
		{
			ActiveState = true;
			LevelLoader.LoadHacking(this);
			OnActivate?.Invoke();
		}

		public void Subscribe(Action act) => OnActivate += act;
	}
}