using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;

public class PlayerStatus
{
	public int Bonus { get; set; }
	public int Moves { get; set; }
	public float LevelStartTime { get; set;}

	private StringBuilder strBuilder;


	public float GetLevelDuration()
	{
		return Time.time - LevelStartTime;
	}

	public string GetLevelDurationFormated()
	{
		StringBuilder strBuilder = new StringBuilder();

		TimeSpan span = TimeSpan.FromSeconds(GetLevelDuration());

		int minutes = span.Minutes;
		int seconds = span.Seconds;

		if (minutes < 10)
			strBuilder.Append("0");
		strBuilder.Append(minutes.ToString());
		strBuilder.Append(":");
		if (seconds < 10)
			strBuilder.Append("0");
		strBuilder.Append(seconds.ToString());

		return strBuilder.ToString(); 
	}


}
