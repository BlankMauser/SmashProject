using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FSMHelper;

public class FitPalante : RayCastColliders {

		// keep an instance of our state machine
		private FitAnimatorStateMachine m_PlayerSM = null;
		public TextAnchor HUDalignment;

		void Start ()
		{
				// create the state machine and start it
				m_PlayerSM = new FitAnimatorStateMachine(this.gameObject);
				m_PlayerSM.StartSM();
		}

				void Update ()
				{
						// update the state machine very frame
						m_PlayerSM.UpdateSM();
						// this is how you can print the current active state tree to the log for debugging
						if (Input.GetButtonDown("Fire2"))
						{
								m_PlayerSM.PrintActiveStateTree();
						}


				}

		void LateUpdate () {
				frameTime = maxFrameTime;
				if (FitAnima.enabled == true) 
				{
						CheckDirection ();
						MoveInXDirection ();
						MoveInYDirection ();
						UpdateECB ();
						if (BufferTimer == 0) {
								ClearBuffer ();
						} else {
								BufferTimer -= 1;
						}
				}
				// update the state machine very frame
				m_PlayerSM.LateUpdateSM();

		}



		void OnGUI()
		{
				int w = Screen.width, h = Screen.height;

				GUIStyle style = new GUIStyle();

				Rect rect = new Rect(0, 0, w, h * 2 / 100);
				style.alignment = HUDalignment;
				style.fontSize = h * 2 / 100;
				style.normal.textColor = new Color (0.5f, 0.0f, 0.0f, 1.0f);
				string text = state.ToString() + "   " + previousState.ToString();
				GUI.Label(rect, text, style);
		}

		void OnDestroy()
		{
				// stop the state machine to ensure all the Exit() gets called
				if (m_PlayerSM != null)
				{
						m_PlayerSM.StopSM();
						m_PlayerSM = null;
				}
		}


}


