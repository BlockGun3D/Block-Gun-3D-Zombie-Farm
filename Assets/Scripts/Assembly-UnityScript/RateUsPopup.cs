using System;
using UnityEngine;

[Serializable]
public class RateUsPopup : MonoBehaviour
{
	public GUITexture[] stars;

	public Color inactiveColor;

	public GameObject FeedbackPopup;

	public GameObject RateusPopup;

	private int starPressed;

	private Color normColor;

	public RateUsPopup()
	{
		stars = new GUITexture[5];
		starPressed = 1;
	}

	public virtual void Start()
	{
		normColor = stars[0].color;
		UpdateStarUI();
	}

	public virtual void Update()
	{
	}

	public virtual void Star1Pressed()
	{
		starPressed = 1;
		UpdateStarUI();
	}

	public virtual void Star2Pressed()
	{
		starPressed = 2;
		UpdateStarUI();
	}

	public virtual void Star3Pressed()
	{
		starPressed = 3;
		UpdateStarUI();
	}

	public virtual void Star4Pressed()
	{
		starPressed = 4;
		UpdateStarUI();
	}

	public virtual void Star5Pressed()
	{
		starPressed = 5;
		UpdateStarUI();
	}

	public virtual void UpdateStarUI()
	{
		for (int i = 0; i < starPressed; i++)
		{
			stars[i].color = normColor;
		}
		for (int i = starPressed; i < 5; i++)
		{
			stars[i].color = inactiveColor;
		}
	}

	public virtual void RatePressed()
	{
		if (starPressed >= 4)
		{
			RateusPopup.SetActive(true);
		}
		else
		{
			FeedbackPopup.SetActive(true);
		}
		Global.gm.pressedRate = 1;
	}

	public virtual void RateAtStorePressed()
	{
		gameObject.SendMessage("GoToAppUrl");
		Global.gm.pressedRate = 1;
	}

	public virtual void Main()
	{
	}
}
