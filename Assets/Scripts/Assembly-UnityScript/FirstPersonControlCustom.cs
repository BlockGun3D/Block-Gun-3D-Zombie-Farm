using System;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(CharacterController))]
public class FirstPersonControlCustom : MonoBehaviour
{
	public JoystickCustom moveTouchPad;

	public JoystickCustom rotateTouchPad;

	public Transform cameraPivot;

	public float forwardSpeed;

	public float backwardSpeed;

	public float sidestepSpeed;

	public float jumpSpeed;

	public float inAirMultiplier;

	public Vector2 rotationSpeed;

	public float tiltPositiveYAxis;

	public float tiltNegativeYAxis;

	public float tiltXAxisMinimum;

	public bool invertYAxis;

	public GUITexture jumpButton;

	private Transform thisTransform;

	private CharacterController character;

	private Vector3 cameraVelocity;

	private Vector3 velocity;

	private bool canJump;

	private GameManager gm;

	public FirstPersonControlCustom()
	{
		forwardSpeed = 4f;
		backwardSpeed = 1f;
		sidestepSpeed = 1f;
		jumpSpeed = 8f;
		inAirMultiplier = 0.25f;
		rotationSpeed = new Vector2(50f, 25f);
		tiltPositiveYAxis = 0.6f;
		tiltNegativeYAxis = 0.4f;
		tiltXAxisMinimum = 0.1f;
		canJump = true;
	}

	public virtual void Start()
	{
		gm = GameManager.GetInstance();
		thisTransform = (Transform)GetComponent(typeof(Transform));
		character = (CharacterController)GetComponent(typeof(CharacterController));
		GameObject gameObject = GameObject.Find("PlayerSpawn");
		if ((bool)gameObject)
		{
			thisTransform.position = gameObject.transform.position;
		}
	}

	public virtual void OnEndGame()
	{
		moveTouchPad.Disable();
		if ((bool)rotateTouchPad)
		{
			rotateTouchPad.Disable();
		}
		enabled = false;
	}

	public virtual void Update()
	{
		GameState gameState = gm.GetGameState();
		if (gameState == GameState.PLAYING || gameState == GameState.TUTORIAL_LOOK || gameState == GameState.TUTORIAL_MOVE)
		{
			UpdateGameplay();
		}
	}

	public virtual Vector2 updateKeyboardControls()
	{
		int num = default(int);
		int num2 = default(int);
		if (Input.GetKey("w"))
		{
			num = 1;
		}
		if (Input.GetKey("s"))
		{
			num = -1;
		}
		if (Input.GetKey("a"))
		{
			num2 = -1;
		}
		if (Input.GetKey("d"))
		{
			num2 = 1;
		}
		return new Vector2(num2, num);
	}

	private bool locked;

	public virtual void UpdateGameplay()
	{
		if (!Application.isMobilePlatform)
		{
			if (!locked)
			{
				Screen.lockCursor = true;
				locked = true;
			}
			moveTouchPad.position = updateKeyboardControls();
			if (Screen.lockCursor)
			{
				rotateTouchPad.position = new Vector2(Input.GetAxis("Mouse X") * Global.gm.GetSensitivityValue() / 20f, Input.GetAxis("Mouse Y") * Global.gm.GetSensitivityValue() / 20f);
			}
		}
		Vector3 motion = thisTransform.TransformDirection(new Vector3(moveTouchPad.position.x, 0f, moveTouchPad.position.y));
		motion.y = 0f;
		motion.Normalize();
		Vector2 vector = new Vector2(Mathf.Abs(moveTouchPad.position.x), Mathf.Abs(moveTouchPad.position.y));
		if (!(vector.y <= vector.x))
		{
			if (!(moveTouchPad.position.y <= 0f))
			{
				motion *= forwardSpeed * vector.y;
			}
			else
			{
				motion *= backwardSpeed * vector.y;
			}
		}
		else
		{
			motion *= sidestepSpeed * vector.x;
		}
		if (character.isGrounded)
		{
			bool flag = false;
			JoystickCustom joystickCustom = null;
			joystickCustom = ((!rotateTouchPad) ? moveTouchPad : rotateTouchPad);
			if (!joystickCustom.IsFingerDown())
			{
				canJump = true;
			}
			if (Application.isMobilePlatform && canJump && StaticFuncs.TestButtonTouchBegan(jumpButton) || !Application.isMobilePlatform && canJump && Input.GetKey(KeyCode.Space))
			{
				flag = true;
				canJump = false;
			}
			if (flag)
			{
				velocity = character.velocity;
				velocity.y = jumpSpeed;
			}
		}
		else
		{
			velocity.y += Physics.gravity.y * Time.deltaTime;
			motion.x *= inAirMultiplier;
			motion.z *= inAirMultiplier;
		}
		motion += velocity;
		motion += Physics.gravity;
		motion *= Time.deltaTime;
		character.Move(motion);
		if (character.isGrounded)
		{
			velocity = Vector3.zero;
		}
		Vector2 vector2 = Vector2.zero;
		if ((bool)rotateTouchPad)
		{
			vector2 = rotateTouchPad.position;
		}
		else
		{
			Vector3 acceleration = Input.acceleration;
			float num = Mathf.Abs(acceleration.x);
			if (!(acceleration.z >= 0f) && !(acceleration.x >= 0f))
			{
				if (!(num < tiltPositiveYAxis))
				{
					vector2.y = (num - tiltPositiveYAxis) / (1f - tiltPositiveYAxis);
				}
				else if (!(num > tiltNegativeYAxis))
				{
					vector2.y = (0f - (tiltNegativeYAxis - num)) / tiltNegativeYAxis;
				}
			}
			if (!(Mathf.Abs(acceleration.y) < tiltXAxisMinimum))
			{
				vector2.x = (0f - (acceleration.y - tiltXAxisMinimum)) / (1f - tiltXAxisMinimum);
			}
		}
		vector2.x *= rotationSpeed.x * Global.gm.GetSensitivityValue();
		vector2.y *= rotationSpeed.y * Global.gm.GetSensitivityValue();
		vector2.x = Mathf.Clamp(vector2.x * Time.deltaTime, -30f, 30f);
		vector2.y = Mathf.Clamp(vector2.y * Time.deltaTime, -30f, 30f);
		thisTransform.Rotate(0f, vector2.x, 0f, Space.World);
		if (invertYAxis)
		{
			cameraPivot.Rotate(vector2.y, 0f, 0f);
		}
		else
		{
			cameraPivot.Rotate(0f - vector2.y, 0f, 0f);
		}
	}

	public virtual void Main()
	{
	}
}
