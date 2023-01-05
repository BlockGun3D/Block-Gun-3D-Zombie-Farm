using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(GUITexture))]
public class JoystickCustom : MonoBehaviour
{
	[NonSerialized]
	private static Joystick[] joysticks;

	[NonSerialized]
	private static bool enumeratedJoysticks;

	[NonSerialized]
	private static float tapTimeDelta = 0.3f;

	public bool touchPad;

	public Rect touchZone;

	public Vector2 deadZone;

	public bool normalize;

	public Vector2 position;

	public int tapCount;

	public bool absolutePos;

	private int lastFingerId;

	private float tapTimeWindow;

	private Vector2 fingerDownPos;

	private Vector2 lastPosition;

	private float fingerDownTime;

	private float firstDeltaTime;

	private GUITexture gui;

	private Rect defaultRect;

	private BoundaryMatt guiBoundary;

	private Vector2 guiTouchOffset;

	private Vector2 guiCenter;

	private GameManager gm;

	public JoystickCustom()
	{
		deadZone = Vector2.zero;
		lastFingerId = -1;
		firstDeltaTime = 0.5f;
		guiBoundary = new BoundaryMatt();
	}

	public virtual void Start()
	{
		gm = GameManager.GetInstance();
		gui = (GUITexture)GetComponent(typeof(GUITexture));
		defaultRect = gui.pixelInset;
		defaultRect.x += transform.position.x * (float)Screen.width;
		defaultRect.y += transform.position.y * (float)Screen.height;
		float x = 0f;
		Vector3 vector = transform.position;
		float num = (vector.x = x);
		Vector3 vector3 = (transform.position = vector);
		float y = 0f;
		Vector3 vector4 = transform.position;
		float num2 = (vector4.y = y);
		Vector3 vector6 = (transform.position = vector4);
		if (touchPad)
		{
			if ((bool)gui.texture)
			{
				touchZone = defaultRect;
			}
			return;
		}
		guiTouchOffset.x = defaultRect.width * 0.5f;
		guiTouchOffset.y = defaultRect.height * 0.5f;
		guiCenter.x = defaultRect.x + guiTouchOffset.x;
		guiCenter.y = defaultRect.y + guiTouchOffset.y;
		guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
		guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
		guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
		guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
	}

	public virtual void Disable()
	{
		gameObject.SetActive(false);
		enumeratedJoysticks = false;
	}

	public virtual void ResetJoystick()
	{
		gui.pixelInset = defaultRect;
		lastFingerId = -1;
		position = Vector2.zero;
		fingerDownPos = Vector2.zero;
		lastPosition = Vector2.zero;
		if (touchPad)
		{
			int num = 0;
			Color color = gui.color;
			float num2 = (color.a = num);
			Color color3 = (gui.color = color);
		}
	}

	public virtual bool IsFingerDown()
	{
		return lastFingerId != -1;
	}

	public virtual void LatchedFinger(int fingerId)
	{
		if (lastFingerId == fingerId)
		{
			ResetJoystick();
		}
	}

	public virtual void Update()
	{
		GameState gameState = gm.GetGameState();
		if (gameState == GameState.PLAYING || gameState == GameState.TUTORIAL_LOOK || gameState == GameState.TUTORIAL_MOVE)
		{
			UpdateGameplay();
		}
	}

	public virtual void UpdateGameplay()
	{
		if (!enumeratedJoysticks)
		{
			joysticks = ((Joystick[])UnityEngine.Object.FindObjectsOfType(typeof(Joystick))) as Joystick[];
			enumeratedJoysticks = true;
		}
		int touchCount = Input2.touchCount;
		if (!(tapTimeWindow <= 0f))
		{
			tapTimeWindow -= Time.deltaTime;
		}
		else
		{
			tapCount = 0;
		}
		if (touchCount == 0)
		{
			ResetJoystick();
		}
		else
		{
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input2.GetTouch(i);
				Vector2 vector = touch.position - guiTouchOffset;
				bool flag = false;
				if (touchPad)
				{
					if (touchZone.Contains(touch.position))
					{
						flag = true;
					}
				}
				else if (gui.HitTest(touch.position))
				{
					flag = true;
				}
				if (flag && (lastFingerId == -1 || lastFingerId != touch.fingerId))
				{
					if (touchPad)
					{
						int num = 0;
						Color color = gui.color;
						float num2 = (color.a = num);
						Color color3 = (gui.color = color);
						lastFingerId = touch.fingerId;
						fingerDownPos = touch.position;
						fingerDownTime = Time.time;
					}
					lastFingerId = touch.fingerId;
					if (!(tapTimeWindow <= 0f))
					{
						tapCount++;
					}
					else
					{
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
					}
					int j = 0;
					Joystick[] array = joysticks;
					for (int length = array.Length; j < length; j++)
					{
						if (array[j] != this)
						{
							array[j].LatchedFinger(touch.fingerId);
						}
					}
				}
				if (lastFingerId != touch.fingerId)
				{
					continue;
				}
				if (touch.tapCount > tapCount)
				{
					tapCount = touch.tapCount;
				}
				if (touchPad)
				{
					if (absolutePos)
					{
						position.x = touch.position.x - fingerDownPos.x;
						position.y = touch.position.y - fingerDownPos.y;
						Vector2 vector2 = lastPosition;
						lastPosition = position;
						position.x = Mathf.Clamp((position.x - vector2.x) / (touchZone.width / 2f), -1f, 1f);
						position.y = Mathf.Clamp((position.y - vector2.y) / (touchZone.height / 2f), -1f, 1f);
					}
					else
					{
						position.x = Mathf.Clamp((touch.position.x - fingerDownPos.x) / (touchZone.width / 2f), -1f, 1f);
						position.y = Mathf.Clamp((touch.position.y - fingerDownPos.y) / (touchZone.height / 2f), -1f, 1f);
					}
				}
				else
				{
					float num3 = Mathf.Clamp(vector.x, guiBoundary.min.x, guiBoundary.max.x);
					Rect pixelInset = gui.pixelInset;
					float num5 = (pixelInset.x = num3);
					Rect rect2 = (gui.pixelInset = pixelInset);
					float num6 = Mathf.Clamp(vector.y, guiBoundary.min.y, guiBoundary.max.y);
					Rect pixelInset2 = gui.pixelInset;
					float num8 = (pixelInset2.y = num6);
					Rect rect4 = (gui.pixelInset = pixelInset2);
				}
				if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					ResetJoystick();
				}
			}
		}
		if (!touchPad)
		{
			position.x = (gui.pixelInset.x + guiTouchOffset.x - guiCenter.x) / guiTouchOffset.x;
			position.y = (gui.pixelInset.y + guiTouchOffset.y - guiCenter.y) / guiTouchOffset.y;
		}
		float num9 = Mathf.Abs(position.x);
		float num10 = Mathf.Abs(position.y);
		if (!(num9 >= deadZone.x))
		{
			position.x = 0f;
		}
		else if (normalize)
		{
			position.x = Mathf.Sign(position.x) * (num9 - deadZone.x) / (1f - deadZone.x);
		}
		if (!(num10 >= deadZone.y))
		{
			position.y = 0f;
		}
		else if (normalize)
		{
			position.y = Mathf.Sign(position.y) * (num10 - deadZone.y) / (1f - deadZone.y);
		}
	}

	public virtual void Main()
	{
	}
}
