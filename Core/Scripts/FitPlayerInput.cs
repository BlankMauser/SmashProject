using UnityEngine;
using System.Collections;
using Rewired;

public class FitPlayerInput : FitCharacterInput {

	public RayCastColliders character;
	private Player player; // The Rewired Player
	public int playerId = 0;
	public float X_Axis;
	public float Y_Axis;
	public int Init_Xdirection;
	public int MaxBuffer;
	public int FramesXNeutral = 0;
	public int FramesYNeutral = 0;
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
				jumpButtonDown = player.GetButtonDown("Jump");
				jumpButtonHeld = player.GetButton("Jump");
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

				if (Mathf.Abs(x) >= 0.06f && character.BfAction <= BufferedAction.WALKING) {
						character.BfAction = BufferedAction.WALKING;
						character.BufferTimer = MaxBuffer;
						if (x > 0) {
								Init_Xdirection = 1;
						} else {
								Init_Xdirection = -1;
						}

				}

				if (Mathf.Abs(x) >= 0.7f) {
						character.BfAction = BufferedAction.INIT_DASH;
						if (FramesXNeutral > 5) {
						character.BfAction = BufferedAction.WALKING;
						}
						character.BufferTimer = MaxBuffer;
						if (x > 0) {
								Init_Xdirection = 1;
						} else {
								Init_Xdirection = -1;
						}			
				}



				// Process Attack
				if (AttackButtonDown) {
						character.BfAction = BufferedAction.JAB;
						character.BufferTimer = MaxBuffer;
				}

				if (jumpButtonDown) {
						character.BfAction = BufferedAction.JUMP;
						character.BufferTimer = MaxBuffer;
				}






				//Frames Since Neutral Limit
				if (FramesXNeutral >= 300) {
						FramesXNeutral = 300;
				}
				//Frames Since Neutral Limit
				if (FramesYNeutral >= 300) {
						FramesYNeutral = 300;
				}
		}
}
