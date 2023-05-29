using CommonComponents;
using UnityEngine;
using Random = UnityEngine.Random;

public class HackingMangaer : MonoBehaviour
{
	private int numberToKill = 5;

	public void Start()
	{
		numberToKill = Random.Range(5, 12);
	}


	public void PlayerWin(Damagable obj)
	{

	}

	public void PlayerLose(Damagable obj)
	{

	}
}
