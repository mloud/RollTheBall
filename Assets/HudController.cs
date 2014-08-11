using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour 
{
	[SerializeField]
	TextMesh bonusText;

	[SerializeField]
	TextMesh timeText;

	[SerializeField]
	TextMesh movesText;


	private float Timer {get; set ;}

	void Start ()
	{
		Timer = 0;
	}

	void Update ()
	{

		if (Time.time > Timer)
		{
			Refresh();

			Timer = Time.time + 1.0f;
		}
	}


	void Refresh()
	{
		if (Game.Instance.CurrentState == Game.State.Running)
		{
			bonusText.text = Game.Instance.PlayerStatus.Bonus.ToString();
			movesText.text = Game.Instance.PlayerStatus.Moves.ToString();
			timeText.text = Game.Instance.PlayerStatus.GetLevelDurationFormated();
		}
	}
}
