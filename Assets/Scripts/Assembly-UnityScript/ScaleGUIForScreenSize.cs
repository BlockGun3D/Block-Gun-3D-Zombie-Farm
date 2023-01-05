using System;
using UnityEngine;

[Serializable]
public class ScaleGUIForScreenSize : MonoBehaviour
{
	public bool scaleToLargerRatio;

	public bool largeScreenAdjust;

	public float fontScale;

	private int fontSize;

	public ScaleGUIForScreenSize()
	{
		fontScale = 1f;
	}

	public virtual void Start()
	{
		float num = 1f;
		float num2 = (float)Screen.width / Global.defaultScreen.x;
		float num3 = (float)Screen.height / Global.defaultScreen.y;
		if (!(num2 >= num3))
		{
			ScaleGUI((!scaleToLargerRatio) ? num2 : num3);
		}
		else
		{
			ScaleGUI((!scaleToLargerRatio) ? num3 : num2);
		}
	}

	private void ScaleGUI(float ratio)
	{
		if (largeScreenAdjust && !((float)Screen.currentResolution.width / Screen.dpi <= 7f))
		{
			float x = ((transform.position.x <= 0.5f) ? (transform.position.x * 0.66f) : (transform.position.x + (1f - transform.position.x) * 0.33f));
			Vector3 position = transform.position;
			float num = (position.x = x);
			Vector3 vector2 = (transform.position = position);
			ratio *= 0.66f;
		}
		if ((bool)GetComponent<GUITexture>())
		{
			float num2 = GetComponent<GUITexture>().pixelInset.x * ratio;
			Rect pixelInset = GetComponent<GUITexture>().pixelInset;
			float num4 = (pixelInset.x = num2);
			Rect rect2 = (GetComponent<GUITexture>().pixelInset = pixelInset);
			float num5 = GetComponent<GUITexture>().pixelInset.y * ratio;
			Rect pixelInset2 = GetComponent<GUITexture>().pixelInset;
			float num7 = (pixelInset2.y = num5);
			Rect rect4 = (GetComponent<GUITexture>().pixelInset = pixelInset2);
			float num8 = GetComponent<GUITexture>().pixelInset.width * ratio;
			Rect pixelInset3 = GetComponent<GUITexture>().pixelInset;
			float num10 = (pixelInset3.width = num8);
			Rect rect6 = (GetComponent<GUITexture>().pixelInset = pixelInset3);
			float num11 = GetComponent<GUITexture>().pixelInset.height * ratio;
			Rect pixelInset4 = GetComponent<GUITexture>().pixelInset;
			float num13 = (pixelInset4.height = num11);
			Rect rect8 = (GetComponent<GUITexture>().pixelInset = pixelInset4);
		}
		if ((bool)GetComponent<GUIText>())
		{
			float x2 = GetComponent<GUIText>().pixelOffset.x * ratio;
			Vector2 pixelOffset = GetComponent<GUIText>().pixelOffset;
			float num14 = (pixelOffset.x = x2);
			Vector2 vector4 = (GetComponent<GUIText>().pixelOffset = pixelOffset);
			float y = GetComponent<GUIText>().pixelOffset.y * ratio;
			Vector2 pixelOffset2 = GetComponent<GUIText>().pixelOffset;
			float num15 = (pixelOffset2.y = y);
			Vector2 vector6 = (GetComponent<GUIText>().pixelOffset = pixelOffset2);
			GetComponent<GUIText>().fontSize = (int)((float)GetComponent<GUIText>().fontSize * ratio);
			fontSize = GetComponent<GUIText>().fontSize;
		}
	}

	public virtual void Update()
	{
		if ((bool)GetComponent<GUIText>())
		{
			GetComponent<GUIText>().fontSize = (int)((float)fontSize * fontScale);
		}
	}

	public virtual void Main()
	{
	}
}
