using Management;
using Player;
using UI;

public class SingletonRepo
{
	private static SingletonRepo _instance = null;
	private PlayerController _playerObject;

	public static SingletonRepo Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SingletonRepo();
			}
			return _instance;
		}
	}

	public static PlayerUIManager PlayerUI
	{
		get
		{
			if (Instance._playerUI == null)
			{
				Instance._playerUI = PlayerUIManager.Instance;
			}

			return Instance._playerUI;
		}
	}

	private PlayerUIManager _playerUI = null;

	public static PlayerController PlayerObject { get => Instance._playerObject; set => Instance._playerObject = value; }

	public static GameStateManager StateManager => GameStateManager.Instance;
}

