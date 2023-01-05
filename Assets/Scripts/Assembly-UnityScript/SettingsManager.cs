using System;
using UnityEngine;

[Serializable]
public class SettingsManager : MonoBehaviour
{
	public GameObject soundOnButton;

	public GameObject soundOffButton;

	public GameObject noInvertButton;

	public GameObject invertButton;

	public GUITexture[] sensitivityBars;

	public SettingsManager()
	{
		sensitivityBars = new GUITexture[9];
	}

	public virtual void Start()
	{
		toggle(Global.gm.IsSoundOn(), soundOnButton, soundOffButton);
		toggle(!Global.gm.IsYAxisInverted(), noInvertButton, invertButton);
		DrawSensitivityBar();
	}

	public virtual void SoundPressed()
	{
		Global.gm.ToggleSound();
		toggle(Global.gm.IsSoundOn(), soundOnButton, soundOffButton);
	}

	public virtual void InvertYPressed()
	{
		Global.gm.ToggleYAxis();
		toggle(!Global.gm.IsYAxisInverted(), noInvertButton, invertButton);
	}

	private void toggle(bool isDefault, GameObject defaultButton, GameObject changedButton)
	{
		ButtonScript buttonScript = (ButtonScript)defaultButton.GetComponent(typeof(ButtonScript));
		ButtonScript buttonScript2 = (ButtonScript)changedButton.GetComponent(typeof(ButtonScript));
		if (isDefault)
		{
			defaultButton.GetComponent<GUITexture>().enabled = true;
			buttonScript.enabled = true;
			changedButton.GetComponent<GUITexture>().enabled = false;
			buttonScript2.enabled = false;
		}
		else
		{
			defaultButton.GetComponent<GUITexture>().enabled = false;
			buttonScript.enabled = false;
			changedButton.GetComponent<GUITexture>().enabled = true;
			buttonScript2.enabled = true;
		}
	}

	public virtual void PlusSensitivity()
	{
		Global.gm.IncreaseSensitivity();
		DrawSensitivityBar();
	}

	public virtual void MinusSensitivity()
	{
		Global.gm.DecreaseSensitivity();
		DrawSensitivityBar();
	}

	private void DrawSensitivityBar()
	{
		int sensitivityLevel = Global.gm.GetSensitivityLevel();
		for (int i = 0; i < 9; i++)
		{
			if (i <= sensitivityLevel - 1)
			{
				float a = 0.4f;
				Color color = sensitivityBars[i].color;
				float num = (color.a = a);
				Color color3 = (sensitivityBars[i].color = color);
				float r = 0.35f;
				Color color4 = sensitivityBars[i].color;
				float num2 = (color4.r = r);
				Color color6 = (sensitivityBars[i].color = color4);
				float g = 0f;
				Color color7 = sensitivityBars[i].color;
				float num3 = (color7.g = g);
				Color color9 = (sensitivityBars[i].color = color7);
				float b = 0f;
				Color color10 = sensitivityBars[i].color;
				float num4 = (color10.b = b);
				Color color12 = (sensitivityBars[i].color = color10);
			}
			else
			{
				float a2 = 0.3f;
				Color color13 = sensitivityBars[i].color;
				float num5 = (color13.a = a2);
				Color color15 = (sensitivityBars[i].color = color13);
				float r2 = 0.5f;
				Color color16 = sensitivityBars[i].color;
				float num6 = (color16.r = r2);
				Color color18 = (sensitivityBars[i].color = color16);
				float g2 = 0.5f;
				Color color19 = sensitivityBars[i].color;
				float num7 = (color19.g = g2);
				Color color21 = (sensitivityBars[i].color = color19);
				float b2 = 0.5f;
				Color color22 = sensitivityBars[i].color;
				float num8 = (color22.b = b2);
				Color color24 = (sensitivityBars[i].color = color22);
			}
		}
	}

	public virtual void Main()
	{
	}
}
