using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract class for character input, extend this to provide your own input.
/// </summary>
public abstract class FitCharacterInput : MonoBehaviour {

		/// <summary>
		/// The x movement. Use 1.0f or -1.0f to run. Smaller values to walk.
		/// </summary>
		virtual public float x{get; protected set;}

		/// <summary>
		/// The x movement previous frame.
		/// </summary>
		virtual public float x_prev{get; protected set;}

		/// <summary>
		/// The y movement. 1.0f = up, -1.0f = down.
		/// </summary>
		virtual public float y{get; protected set;}

		/// <summary>
		/// Is the jump button being held down.
		/// </summary>
		virtual public bool jumpButtonHeld{get; protected set;}

		/// <summary>
		/// Was the jump button pressed this frame.
		/// </summary>
		virtual public bool jumpButtonDown{get; protected set;}

	virtual public bool TapjumpButtonDown{get; protected set;}
	virtual public bool TapjumpButtonHeld{get; protected set;}

		virtual public bool AttackButtonDown{get; protected set;}
		virtual public bool AttackButtonHeld{get; protected set;}

		virtual public bool BulletButtonDown{get; protected set;}
		virtual public bool BulletButtonHeld{get; protected set;}

		virtual public bool SpecialButtonDown{get; protected set;}
		virtual public bool SpecialButtonHeld{get; protected set;}

		virtual public bool ShieldHardPress{get; protected set;}
		virtual public bool ShieldButtonDown{get; protected set;}
		virtual public bool ShieldButtonHeld{get; protected set;}

		virtual public bool QAButtonDown{get; protected set;}
		virtual public bool QAButtonHeld{get; protected set;}

	virtual public bool CLeftDown{get; protected set;}
	virtual public bool CRightDown{get; protected set;}
	virtual public bool CUpDown{get; protected set;}
	virtual public bool CDownDown{get; protected set;}

	virtual public bool UpDebugDown{get; protected set;}

		public bool NoInput;
		public bool NoJump;

		/// <summary>
		/// Stop jump from happening, useful for platforms that don't let you jump (or special jump behaviour).
		/// </summary>
		virtual public void CancelJump() {
				jumpButtonDown = false; 
				jumpButtonHeld = false;
		}
}