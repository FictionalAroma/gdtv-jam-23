using CommonComponents;
using Hacking.Player;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HackingManager : MonoBehaviour
{
	public enum HackingMode
    {
		Easy,
		Normal,
		Hard
    }
	public HackingMode hackingMode;
	bool isTimeTrial;
	private int numberToKill = 5;
	float timeTrialTimer;
	float timeTrialTime;
	HackingPlayerController hackingPlayerController;
	[SerializeField] private TMP_Text toKillText;

	public void Start()
	{
		hackingPlayerController = FindObjectOfType<HackingPlayerController>();
		switch (hackingMode)
        {
			case HackingMode.Easy:
                {
					isTimeTrial = false;
					numberToKill = Random.Range(3, 10);
					break;
				}
				
			case HackingMode.Normal:
                {
					var randomizeMode = Random.Range(0, 2);
					if (randomizeMode == 0)
                    {
						isTimeTrial = true;
						timeTrialTime = Random.Range(10f, 15f);
                    }
					else
                    {
						isTimeTrial = false;
						numberToKill = Random.Range(5, 12);
                    }
					break;
                }
			case HackingMode.Hard:
                {
					isTimeTrial = true;
					numberToKill = Random.Range(10, 20);
					timeTrialTime = Random.Range(20f, 35f);
					break;
                }

        }

		toKillText.text = numberToKill.ToString();


	}
    private void Update()
	{
		timeTrialTimer -= Time.deltaTime;
        if (isTimeTrial&& hackingMode != HackingMode.Hard)
        {
			if (timeTrialTimer <=0)
            {
				PlayerWin(hackingPlayerController);
            }
        }
		else if (!isTimeTrial && hackingMode!= HackingMode.Hard)
        {
			if (numberToKill <= 0)
            {
				PlayerWin(hackingPlayerController);
            }
        }
		else if (hackingMode == HackingMode.Hard)
        {
			if (timeTrialTimer <=0 && numberToKill <=0)
            {
				PlayerWin(hackingPlayerController);
            }
        }
		if (hackingPlayerController.CurrentHP <= 0)
        {
			PlayerLose(hackingPlayerController);
        }
		
    }

	public void EnemyDead(Damagable damagable)
	{
		numberToKill--;
		toKillText.text = numberToKill.ToString("0");
	}


    public void PlayerWin(Damagable obj)
	{
		
	}

	public void PlayerLose(Damagable obj)
	{
		
	}
}
