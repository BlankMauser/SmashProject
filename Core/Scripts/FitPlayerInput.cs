using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Rewired;

public class FitPlayerInput : FitCharacterInput {

	public RayCastColliders character;
	private Player player; // The Rewired Player
	public int playerId = 0;
	public float X_Axis;
	public float Y_Axis;
	public float buffer_x;
	public float buffer_y;
	public int Init_Xdirection;
	public int MaxBuffer;
	public int FramesXNeutral = 0;
	public int FramesYNeutral = 0;
	public int FramesLPressed = 0;
	public int TechPenalty = 0;
	// Use this for initialization



	void Awake() {
				// Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
				player = ReInput.players.GetPlayer(playerId);

		}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
				GetInput();
				ProcessInput ();
	}

		public void GetInput() {
				
				// Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
				// whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.
				AttackButtonDown = player.GetButtonDown("Attack");
				AttackButtonHeld = player.GetButton("Attack");
				BulletButtonDown = player.GetButtonDown("Bullet");
				BulletButtonHeld = player.GetButton("Bullet");
				SpecialButtonDown = player.GetButtonDown("Special");
				SpecialButtonHeld = player.GetButton("Special");
				QAButtonDown = player.GetButtonDown("QA");
				QAButtonHeld = player.GetButton("QA");
				jumpButtonDown = player.GetButtonDown("Jump");
				TapjumpButtonDown = player.GetButtonDown("TapJump");
				jumpButtonHeld = player.GetButton("Jump");
				TapjumpButtonHeld = player.GetButton("TapJump");
				ShieldHardPress = player.GetButtonDown("ShieldLP") || player.GetButtonDown("ShieldRP");
				ShieldButtonDown = player.GetButtonDown("ShieldL") || player.GetButtonDown("ShieldR");
				ShieldButtonHeld = player.GetButton("ShieldL") || player.GetButton("ShieldR");

				CLeftDown = player.GetButtonDown("CLeft");
				CRightDown = player.GetButtonDown("CRight");
				CUpDown = player.GetButtonDown("CUp");
				CDownDown = player.GetButtonDown("CDown");

				UpDebugDown = player.GetButtonDown("UpDebug");

				x = player.GetAxis ("Move Left/Right");
				x_prev = player.GetAxisPrev ("Move Left/Right");
				X_Axis = x;
				y = player.GetAxis ("Move Up/Down");
				Y_Axis = y;
		}

		public void ProcessInput() {
				// Process movement
				if (Mathf.Abs (x) <= 0.18f) {
						FramesXNeutral = 0;
				} else {
						FramesXNeutral += 1;
				}

				if (Mathf.Abs (y) <= 0.18f) {
						FramesYNeutral = 0;
				} else {
						FramesYNeutral += 1;
				}
				
		if (TechPenalty > 0) {
			TechPenalty -= 1;
		}

				FramesLPressed += 1;

				if (Mathf.Abs(x) >= 0.06f) {
						SetAction (BufferedAction.WALKING);
						if (x > 0) {
								Init_Xdirection = 1;
						} else {
								Init_Xdirection = -1;
						}

				}

				if (Mathf.Abs(x) >= 0.7f) {
						if (FramesXNeutral <= 2) 
						{
						SetAction (BufferedAction.INIT_DASH, 2);
						} else 
						{
						SetAction (BufferedAction.WALKING);
						}
						if (x > 0) 
						{
								Init_Xdirection = 1;
						} else 
						{
								Init_Xdirection = -1;
						}			
				}

		if (jumpButtonDown || TapjumpButtonDown) {
			SetAction (BufferedAction.JUMP);
		}



				// Process Attack
				if (AttackButtonDown) {
			buffer_x = x;
			buffer_y = y;
						SetAction (BufferedAction.ATTACK, 1);
				}
			
		if (BulletButtonDown) {
			buffer_x = x;
			buffer_y = y;
			SetAction (BufferedAction.BULLET, 2);
		}

		if (SpecialButtonDown) {
			buffer_x = x;
			buffer_y = y;
			SetAction (BufferedAction.SPECIAL, 2);
		}

		if (QAButtonDown) {
			buffer_x = x;
			buffer_y = y;
			SetAction (BufferedAction.QA, 1);
		}

		//GOING TO BE REPLACED
		if (CUpDown) {
			buffer_x = 0;
			buffer_y = 1;
			SetAction (BufferedAction.ATTACK, 3);
		}

		if (CDownDown) {
			buffer_x = 0;
			buffer_y = -1;
			SetAction (BufferedAction.ATTACK, 3);
		}

		if (CLeftDown) {
			buffer_x = -1;
			buffer_y = 0;
			SetAction (BufferedAction.ATTACK, 3);
		}

		if (CRightDown) {
			buffer_x = 1;
			buffer_y = 0;
			SetAction (BufferedAction.ATTACK, 3);
		}

		if (UpDebugDown) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}


				if (ShieldButtonDown) {
			if (TechPenalty == 0) {
				FramesLPressed = 0;
				TechPenalty = 20;
			} else {
				TechPenalty = 20;
			}
						buffer_x = x;
						SetAction (BufferedAction.SHIELD, 3);
				}





				//Frames Since Neutral Limit
				if (FramesXNeutral >= 80) {
						FramesXNeutral = 80;
				}
				//Frames Since Neutral Limit
				if (FramesYNeutral >= 80) {
						FramesYNeutral = 80;
				}

		if (FramesLPressed >= 80) {
			FramesLPressed = 80;
		}

		}

		public void SetAction(BufferedAction action)
		{
				if (character.BfAction <= action) 
				{
						character.BfAction = action;
						character.BufferTimer = MaxBuffer;
				}
		}

		public void SetAction(BufferedAction action, int buffer)
		{
				if (character.BfAction <= action) 
				{
						character.BfAction = action;
						character.BufferTimer = buffer;
				}
		}

	public Cardinals ReturnAxis()
	{
		float AxAngle = Mathf.Round((Mathf.Atan2 (Y_Axis, X_Axis)) * Mathf.Rad2Deg);
		if (Mathf.Abs (X_Axis) > 0.18f || Mathf.Abs (Y_Axis) > 0.18f) {
			if (AxAngle >= -20 && AxAngle <= 20) {
				return Cardinals.Right;
			}
			if (AxAngle >= 21 && AxAngle <= 69) {
				return Cardinals.UpRight;
			}
			if (AxAngle >= 70 && AxAngle <= 110) {
				return Cardinals.Up;
			}
			if (AxAngle >= 111 && AxAngle <= 159) {
				return Cardinals.UpLeft;
			}
			if (AxAngle >= 160 || AxAngle <= -160) {
				return Cardinals.Left;
			}
			if (AxAngle <= -111 && AxAngle >= -159) {
				return Cardinals.DownLeft;
			}
			if (AxAngle <= -70 && AxAngle >= -110) {
				return Cardinals.Down;
			}
			if (AxAngle <= -21 && AxAngle >= -69) {
				return Cardinals.DownRight;
			}
		}
			
		return Cardinals.Center;
		
	}

	public Cardinals ReturnAxisAerial()
	{
		float AxAngle = Mathf.Round((Mathf.Atan2 (buffer_y, buffer_x)) * Mathf.Rad2Deg);
		if (Mathf.Abs (buffer_x) > 0.18f || Mathf.Abs (buffer_y) > 0.18f) {
			if (AxAngle >= -45 && AxAngle <= 45) {
				return Cardinals.Right;
			}
			if (AxAngle >= 46 && AxAngle <= 134) {
				return Cardinals.Up;
			}
			if (AxAngle >= 135 || AxAngle <= -135) {
				return Cardinals.Left;
			}
			if (AxAngle <= -46 && AxAngle >= -134) {
				return Cardinals.Down;
			}
		}

		return Cardinals.Center;

	}

}
