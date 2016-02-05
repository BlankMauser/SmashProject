using UnityEngine;
using System.Collections;

public class RayCastColliders : MonoBehaviour {

	/// <summary>
	/// feet colliders. These colliders push the characters upwards.
	/// </summary>
	public RaycastDiamond[] ECBfeet;
	/// <summary>
	/// The head colliders. These characters push the character down when the head is hit.
	/// </summary>
	public RaycastDiamond[] ECBhead;
	/// <summary>
	/// sides. These colliders push the character left and right when they hit obstacles.
	/// </summary>
	public RaycastDiamond[] ECBsides;


		// ECB Positions
		/// <summary>
		/// Attach to ECB Top.
		/// </summary>
		public Transform TOPTransform;
		/// <summary>
		/// Attach to ECB Bottom.
		/// </summary>
		public Transform BOTTransform;
		/// <summary>
		/// Attach to ECB Left.
		/// </summary>
		public Transform ECBLeft;
		/// <summary>
		/// Attach to ECB Right.
		/// </summary>
		public Transform ECBRight;
		public Vector3 PreviousTop;
		public Vector3 PreviousLeft;
		public Vector3 PreviousRight;
		public Vector3 PreviousBottom;
		public Vector3 CurrentTop;
		public Vector3 CurrentLeft;
		public Vector3 CurrentRight;
		public Vector3 CurrentBottom;

	/// <summary>
	/// Movement attributes.
	/// </summary>
	public MovementAttributes movement;
	/// <summary>
	/// The jump details. Controls how the character moves in the air.
	/// </summary>
	public JumpAttributes jump;
	/// <summary>
	/// The slopes details. Controls how the character handles sloped platforms.
	/// </summary>
	public SlopeDetails slopes;
	public FitPlayerInput Inputter;
	/// <summary>
	/// The layer of normal objects that the chracter cannot pass through.
	/// </summary>
	public int backgroundLayer;
	/// <summary>
	/// The layer of object which the chracter can jump through but can not fall through.
	/// </summary>
	public int passThroughLayer;
	/// <summary>
	/// The layer of objects the character can climb.
	/// </summary>
	public int climableLayer;
	/// <summary>
	/// How far to look ahead when considering if this character is grounded.
	/// </summary>
	public float groundedLookAhead = 0.25f;

	public FitAnimator Animator;

	public Animation anima;

	public StunType stun;

	public Vector3 velocity;

	public float C_Drag;
	
	public int localXdir;

	/// <summary>
	/// X Axis of Controller.
	/// </summary>
	public int x_direction;
	/// <summary>
	/// Direction character is facing.
	/// </summary>
	public int x_facing;

	public bool ApplyFriction = true;
	public int BufferTimer;

	
	public CharacterState state;
	public CharacterState potentialState;
	public CharacterState previousState;


	public Vector2 Velocity {
				get { return velocity; }	
				set { velocity = value; }
	}

//	public CharacterState State{
//			get { return state; }
//			private set {
//					if (!characterStateForced && (int) value > (int)potentialState) potentialState = value;
//			}
//	}

		public BufferedAction BfAction;

//	public void ForceSetCharacterState(CharacterState state) {
//			potentialState = state;
//			characterStateForced = true;
//	}

	public bool IsGrounded(float offset){
				foreach (RaycastDiamond foot in ECBfeet) {
						if (foot.IsColliding(backgroundLayer | 1 << passThroughLayer, offset)) return true;
				}
				return false;
	}

	

	/// <summary>
	/// The maximum time a frame can take. Smaller values allow your character to move faster, larger values will tend to make lag
	/// less apparent. Static so it is the same across all components.
	/// WARNING: If this is too large your character can fall through the ground, or move through small platforms.
	/// </summary>
	public static float maxFrameTime = 0.0166667f;


	/// <summary>
	/// Gets the frame time.
	/// </summary>
	/// <value>
	/// The frame time.
	/// </value>
	public static float FrameTime {
			get { return Mathf.Min (Time.deltaTime, maxFrameTime);	}
	}

		/// <summary>
		/// Switches the colliders direction. This is a default implementation
		/// a direction checker may choose to do something different. This is primarily
		/// for assymetrical characters.
		/// </summary>
		public void SwitchColliders() {	
				// Head Colliders
				foreach (RaycastDiamond c in ECBhead) {
						c.offset = new Vector3 (c.offset.x * -1, c.offset.y, c.offset.z);
				}
				// Feet Colliders
				foreach (RaycastDiamond c in ECBfeet) {
						c.offset = new Vector3 (c.offset.x * -1, c.offset.y, c.offset.z);
				}
				// Side Colliders
				foreach (RaycastDiamond c in ECBsides) {
						c.offset = new Vector3 (c.offset.x * -1, c.offset.y, c.offset.z);
						if (c.direction == RC_Direction.LEFT) {
								c.direction = RC_Direction.RIGHT;
						} else if (c.direction == RC_Direction.RIGHT) {
								c.direction = RC_Direction.LEFT;
						}
						RaycastDiamond tempCollider = highestSideColliders [0];
						highestSideColliders [0] = highestSideColliders [1];
						highestSideColliders [1] = tempCollider;
				}
		}

		#region private variables

	public float frameTime;
	private int groundedFeet;
	private float fallThroughTimer = 0.0f;
	private PlatformPass myParent;
	private Transform myTransform;
	private bool characterStateForced = false;
	private float currentDrag = 0.0f;
	private RaycastDiamond[] highestSideColliders;

		#endregion

	// Events sent for animations. Listen to these to animate your character.
	


	public delegate void CharacterControllerEventDelegate(CharacterState state, CharacterState previousState);
	public event CharacterControllerEventDelegate CharacterAnimationEvent;

		#region Unity Hooks

	void Awake() {

			myTransform = transform;	
			velocity = Vector3.zero;
			currentDrag = movement.friction;

				// Assign default transforms
				if (ECBfeet != null) {
						foreach (RaycastDiamond c in ECBfeet) {
								if (c.BOTTransform == null) c.BOTTransform = transform;
						}
				}
				if (ECBhead != null) {
						foreach (RaycastDiamond c in ECBhead) {
								if (c.BOTTransform == null) c.BOTTransform = transform;	
						}
				}
				if (ECBsides != null) {
						float[ ]highestSideColliderHeight = new float[]{-9999.0f, -9999.0f};
						highestSideColliders = new RaycastDiamond[2];
						foreach (RaycastDiamond c in ECBsides) {
								if (c.BOTTransform == null) c.BOTTransform = transform;	
								if (c.direction == RC_Direction.RIGHT && c.offset.y > highestSideColliderHeight[0]) {
										highestSideColliders[0] = c;
										highestSideColliderHeight[0] = c.offset.y;
								} else if (c.direction == RC_Direction.LEFT && c.offset.y > highestSideColliderHeight[1]) {
										highestSideColliders[1] = c;
										highestSideColliderHeight[1] = c.offset.y;
								}
						}
				}

	}

	// Use this for initialization
	void Start () {
				PreviousTop = TOPTransform.position;
				PreviousLeft = ECBLeft.position;
				PreviousRight = ECBRight.position;
				PreviousBottom = BOTTransform.position;
				CurrentTop = TOPTransform.position;
				CurrentLeft = ECBLeft.position;
				CurrentRight = ECBRight.position;
				CurrentBottom = BOTTransform.position;
	}
				
		void LateUpdate () {
			frameTime = maxFrameTime;
				CheckDirection ();
				MoveInXDirection();
				MoveInYDirection();
				UpdateECB ();
				if (BufferTimer == 0) {
						ClearBuffer ();
				} else {
						BufferTimer -= 1;
				}

		}

		protected void MoveInXDirection()	{

				if (ApplyFriction == true) {
						float newVelocity = (Mathf.Abs (velocity.x) - C_Drag);
						if (newVelocity < 0) {
								newVelocity = 0;
						}
						if (velocity.x > 0) {
								localXdir = 1;
						} else if (velocity.x < 0) {
								localXdir = -1;
						} else if (velocity.x == 0) {
								localXdir = x_facing;
						}
						newVelocity = newVelocity * localXdir;
						velocity.x = newVelocity;
				}

				if ((myParent == null || !myParent.overrideX) && velocity.x > movement.skinSize || velocity.x * -1 > movement.skinSize) {
						myTransform.Translate (velocity.x * frameTime, 0.0f, 0.0f);		
				}

				float forceSide = 0.0f;

				for (int i = 0; i < ECBsides.Length; i++) {
						RaycastHit hitSides;
						float additionalDistance = 0.0f;

						hitSides = ECBsides [i].GetCollision (backgroundLayer, additionalDistance);

								// Hit something ...
								if (hitSides.collider != null) {

										// Check for platforms, but only if we are within collider distance + skinSize
								if (hitSides.distance <= ECBsides [i].DistanceToECB + movement.skinSize) {
												Platform platform = hitSides.collider.gameObject.GetComponent<Platform> ();
												if (platform != null) {
												}
										}
								}

								// Stop movement, but only if we are within collider distance
						if (hitSides.distance <= ECBsides [i].DistanceToECB) {
								float tmpForceSide = (hitSides.normal * (ECBsides [i].DistanceToECB - hitSides.distance)).x;
										if (tmpForceSide > Mathf.Abs (forceSide) || tmpForceSide * -1 > Mathf.Abs (forceSide)) {
												forceSide = tmpForceSide;
												//	TODO Remove this after adequate testing break;
										}
								}
						
				}

				// Move
				if (forceSide > movement.skinSize) {	
						myTransform.Translate(Mathf.Max(velocity.x * frameTime * -1, forceSide), 0.0f, 0.0f);		
				} else if (-1 * forceSide > movement.skinSize) {		
						myTransform.Translate(Mathf.Min(velocity.x * frameTime * -1, forceSide), 0.0f, 0.0f);	
				}
				if ((forceSide > 0 && velocity.x < 0) || (forceSide < 0 && velocity.x > 0)) {
						velocity.x = 0.0f;
				}


		}

		protected void MoveInYDirection()	{


				// Apply velocity
				if ((myParent == null || !myParent.overrideY) && (velocity.y > movement.skinSize || velocity.y * -1 > movement.skinSize)) {
						myTransform.Translate (0.0f, velocity.y * frameTime, 0.0f, Space.World);
				}


				// Fall/Stop
				bool hasHitFeet = false;
				bool hasHitHead = false;
				bool AboveECB = false;
				float maxForce = 0.0f;
				float slope = 0.0f;
				int slopeCount = -1; 
				GameObject hitGameObject = null;
				float lastHitDistance = -1;
				float lastHitX = 0.0f;

				foreach (RaycastDiamond feetCollider in ECBfeet) {
						RaycastHit hitFeet = new RaycastHit ();
						RaycastHit hitLadder = new RaycastHit ();
						RaycastHit hitGround = new RaycastHit ();
						float closest = float.MaxValue;
						float closestLadder = float.MaxValue;

						RaycastHit[] feetCollisions = feetCollider.GetAllCollision (backgroundLayer | (fallThroughTimer <= 0.0f ? 1 << passThroughLayer : 0) | (fallThroughTimer <= 0.0f ? 1 << climableLayer : 0), slopes.slopeLookAhead);						
						// Get closest collision
						foreach (RaycastHit collision in feetCollisions) {
								// If we got a ladder also keep reference to ground
								if (collision.collider != null && collision.collider.gameObject.layer == climableLayer) {
										if (collision.distance < closestLadder) {
												hitLadder = collision;	
												closestLadder = collision.distance;
										}
								} else if (collision.distance < closest) {
										hitFeet = collision;
										closest = collision.distance;
								}
								if (collision.collider.gameObject.layer != climableLayer)
										groundedFeet++;
						}

						// If ladder is closest collider
						if (hitLadder.collider != null && hitFeet.collider != null && hitLadder.distance < closest) {
								// Only assign ground if its a true hit, not a slope look ahead hit
								if (hitFeet.distance <= feetCollider.DistanceToECB && hitFeet.collider.gameObject.layer != climableLayer) {
										hitGround = hitFeet;
										hasHitFeet = true;
								}
								hitFeet = hitLadder;
						}

						// If only hitting a ladder
						if (hitLadder.collider != null && hitFeet.collider == null) {
								hitFeet = hitLadder;	
						}


						float force = (hitFeet.normal * (feetCollider.DistanceToECB - hitFeet.distance)).y;	
						// Standing on a something that has an action when you stand on it
						if (hitFeet.collider != null) {
								PlatformPass platform = hitFeet.collider.gameObject.GetComponent<PlatformPass> ();
								if (platform != null) {
										// ECB is above
										if (platform.myTransform.position.y > CurrentBottom.y) {
												AboveECB = true;
										}
								}
//								if (platform != null && feetCollider.DistanceToECB >= hitFeet.distance) {
//										Transform parentPlatform = platform.ParentOnStand (this);
//										if (parentPlatform != null) {
//												// Normal parenting (moving platforms etc)
//												myParent = platform;
//												if (myTransform.parent != parentPlatform) {
//														myTransform.parent = parentPlatform;
//												}
//												hitGameObject = hitFeet.collider.gameObject;
//										}
//								}


										// Calculate slope
										if (slopes.allowSlopes) {
												if (lastHitDistance < 0.0f) {
														lastHitDistance = hitFeet.distance;
														lastHitX = feetCollider.offset.x;
														if (slopeCount == -1)
																slopeCount = 0;
												} else {
														slope += Mathf.Atan ((lastHitDistance - hitFeet.distance) / (feetCollider.offset.x - lastHitX)) * Mathf.Rad2Deg;
														slopeCount++;
														lastHitDistance = hitFeet.distance;
														lastHitX = feetCollider.offset.x;
												}
										}
								

								// If we are hitting our feet on the ground we can't climb down a ladder
								if (hitLadder.collider == null && hitFeet.distance <= feetCollider.DistanceToECB) {
										maxForce = 0.0f;
								}
								// Get force to apply		
								if (force > maxForce && hitLadder.collider == null) {
										hasHitFeet = true;
										maxForce = force;
										hitGameObject = hitFeet.collider.gameObject;
								}


						}
				}

				if (hasHitFeet) {
						if (PreviousBottom.y >= CurrentBottom.y && AboveECB == false) {
								myTransform.Translate (0.0f, maxForce, 0.0f, Space.World);	
								velocity.y = 0.0f;
						}
				}

				// Apply rotation from slopes
				if (slopes.allowSlopes) {
						float actualSlope = (myTransform.localEulerAngles.z % 360.0f) + (slope / (float)slopeCount);
						if (slopeCount > 0 && (!(actualSlope > slopes.maxRotation && actualSlope < 360.0f - slopes.maxRotation))) {
								myTransform.Rotate (0.0f, 0.0f, slopes.rotationSpeed * (slope / (float)slopeCount));
						} else if (slopeCount == -1) {
								myTransform.localRotation = Quaternion.RotateTowards (myTransform.localRotation, Quaternion.identity, slopes.rotationSpeed * 10.0f);
						}
						if ((actualSlope > slopes.maxRotation && actualSlope < 360.0f - slopes.maxRotation) || IsGrounded(groundedLookAhead) == false) {
								myTransform.localRotation = Quaternion.Euler (0, 0, 0);
						}
				}

				// Hitting Head
				if (velocity.y > 0.0f || (myParent != null && myParent.velocity.y > 0.0f)) {
						maxForce = 0.0f;
						foreach (RaycastDiamond headCollider in ECBhead) {
								RaycastHit hitHead = headCollider.GetCollision (backgroundLayer);
								float force = (hitHead.normal * (headCollider.DistanceToECB - hitHead.distance)).y;
								if (hitHead.collider != null) {
										if (force < -1 * movement.skinSize && force < maxForce) {
												hasHitHead = true;
												maxForce = force;
										}
								}
						}

						if (hasHitHead) {
								myTransform.Translate (0.0f, maxForce, 0.0f, Space.World);		
								if (velocity.y > 0.0f)
										velocity.y = 0.0f;
						}
				}

		}

		public void UpdateECB() {
				PreviousTop = CurrentTop;
				PreviousLeft = CurrentLeft;
				PreviousRight = CurrentRight;
				PreviousBottom = CurrentBottom;
				CurrentTop = TOPTransform.position;
				CurrentLeft = ECBLeft.position;
				CurrentRight = ECBRight.position;
				CurrentBottom = BOTTransform.position;
		}

		protected void CheckDirection(){
				// You might need to switch 270 and 90 for other values depending on orientation of your model
//				if (x_direction == 1 ) {
//						transform.localRotation = Quaternion.Euler (270.0f, 0.0f, 0.0f);
//				} else if (x_direction == -1) {
//						transform.localRotation = Quaternion.Euler (90.0f, 0.0f, 0.0f);
//				}	
		}
	
	// Update is called once per frame

		public void ClearBuffer() {
				BfAction = BufferedAction.NONE;
				BufferTimer = 0;
		}


		#endregion

	void OnDrawGizmos(){
		if (ECBfeet != null) {
			foreach (RaycastDiamond c in ECBfeet) {
				if (c.BOTTransform == null) c.BOTTransform =  transform;
				c.DrawRayCast();	
			}
		}
		if (ECBhead != null) {
			foreach (RaycastDiamond c in ECBhead) {
				if (c.BOTTransform == null) c.BOTTransform = transform;
				c.DrawRayCast();	
			}
		}
		if (ECBsides != null) {
			foreach (RaycastDiamond c in ECBsides) {
				if (c.BOTTransform == null) c.BOTTransform = transform;
				c.DrawRayCast();	
			}
		}	
	}

		protected void UpdateAnimation() {
				CharacterAnimationEvent(state, previousState);
		}

	#region Exposed Classes

	[System.Serializable]
	public class RaycastDiamond {
		/// <summary>
		/// Attach to ECB Top.
		/// </summary>
		public Transform TOPTransform;
		/// <summary>
		/// Attach to ECB Bottom.
		/// </summary>
		public Transform BOTTransform;
		/// <summary>
		/// Attach to ECB Sides.
		/// </summary>
		public Transform ECBTransform;
		/// <summary>
		/// The distance of the raycast to the ECB.
		/// </summary> 
		public float DistanceToECB;	
		/// <summary>
		/// The direction of the collider.
		/// </summary>
		public RC_Direction direction;
		/// <summary>
		/// The position of the collider.
		/// </summary>
		public RC_Pos rcpos;
		/// <summary>
		/// The offset from the centre of the transform.
		/// </summary>
		public Vector3 offset;


		public bool IsColliding() {
						offset = GetOffset ();
						DistanceToECB = GetDistance ();
						return Physics.Raycast (BOTTransform.position + BOTTransform.localRotation * offset, BOTTransform.localRotation * GetVectorForDirection(), DistanceToECB);
				}

		public bool IsColliding(int layerMask) {
						offset = GetOffset ();
						DistanceToECB = GetDistance ();
						return Physics.Raycast (BOTTransform.position + BOTTransform.localRotation * offset, BOTTransform.localRotation * GetVectorForDirection(), DistanceToECB, layerMask);
				}

		public bool IsColliding(int layerMask, float extraDistance, float yOffset) {
						offset = GetOffset ();
						DistanceToECB = GetDistance ();
						return Physics.Raycast (BOTTransform.position + BOTTransform.localRotation * new Vector3(offset.x, offset.y + yOffset, offset.z), 
								BOTTransform.localRotation * GetVectorForDirection(), DistanceToECB + extraDistance, layerMask);
				}

		public bool IsColliding(int layerMask, float skinSize) {
						offset = GetOffset ();
						DistanceToECB = GetDistance ();
						return Physics.Raycast (BOTTransform.position + BOTTransform.localRotation * offset, BOTTransform.localRotation * GetVectorForDirection(), DistanceToECB + skinSize, layerMask);
				}

		public RaycastHit GetCollision() {
				offset = GetOffset ();
				DistanceToECB = GetDistance ();	
				RaycastHit hit;
						Physics.Raycast (BOTTransform.position + BOTTransform.localRotation * offset, BOTTransform.localRotation * GetVectorForDirection(), out hit, DistanceToECB);
				return hit;	
		}

		public RaycastHit GetCollision(int layerMask) {
				offset = GetOffset ();
				DistanceToECB = GetDistance ();
				RaycastHit hit;
						Physics.Raycast (BOTTransform.position + BOTTransform.localRotation * offset, BOTTransform.localRotation * GetVectorForDirection(), out hit, DistanceToECB, layerMask);
				return hit;	
		}

		public RaycastHit GetCollision(int layerMask, float extraDistance) {
				offset = GetOffset ();
				DistanceToECB = GetDistance ();
				RaycastHit hit;
						Physics.Raycast (BOTTransform.position + BOTTransform.localRotation * offset, BOTTransform.localRotation * GetVectorForDirection(), out hit, DistanceToECB + extraDistance, layerMask);
				return hit;	
		}

		public RaycastHit[] GetAllCollision(int layerMask) {
				offset = GetOffset ();
				DistanceToECB = GetDistance ();
						return Physics.RaycastAll (BOTTransform.position + BOTTransform.localRotation * offset, BOTTransform.localRotation * GetVectorForDirection(), DistanceToECB, layerMask);
		}

		public RaycastHit[] GetAllCollision(int layerMask, float extraDistance) {
				offset = GetOffset ();
				DistanceToECB = GetDistance ();
						return Physics.RaycastAll (BOTTransform.position + BOTTransform.localRotation * offset, BOTTransform.localRotation * GetVectorForDirection(), DistanceToECB + extraDistance, layerMask);
		}

		public void DrawRayCast ()
		{
			if (BOTTransform != null && ECBTransform != null) {
				switch (direction) {
				case RC_Direction.DOWN: Gizmos.color = Color.green; break;
				case RC_Direction.RIGHT: Gizmos.color = Color.red;  break;
				case RC_Direction.LEFT: Gizmos.color = Color.yellow;break;
				case RC_Direction.UP: Gizmos.color = Color.magenta; break;
				}
				Vector3 position = BOTTransform.position + BOTTransform.localRotation * GetOffset();
				
				Gizmos.DrawLine (position, position + ((BOTTransform.localRotation * GetVectorForDirection()) * GetDistance()));
			}
		}

		public Vector3 GetVectorForDirection(){
			switch (direction) {
			case RC_Direction.DOWN: return Vector3.up * -1;
			case RC_Direction.RIGHT: return Vector3.right;
			case RC_Direction.LEFT: return Vector3.right * -1;
			case RC_Direction.UP: return Vector3.up;
			}
			return Vector3.zero;
		}

		public Vector3 GetOffset(){
			switch (rcpos) {
			case RC_Pos.UPPER: return new Vector3(0, ((TOPTransform.position.y - ECBTransform.position.y)/2)+(ECBTransform.position.y - BOTTransform.position.y), 0);
			case RC_Pos.CENTER: return new Vector3(0,ECBTransform.position.y - BOTTransform.position.y, 0);
			case RC_Pos.LOWER: return new Vector3(0, (ECBTransform.position.y - BOTTransform.position.y)/2, 0);
			case RC_Pos.VERTHI: return new Vector3(0,ECBTransform.position.y - BOTTransform.position.y, 0);
			case RC_Pos.VERTLO: return new Vector3(offset.x,ECBTransform.position.y - BOTTransform.position.y, 0);
			}
			return Vector3.zero;
		}

		public float GetDistance(){
			switch (rcpos) {
			case RC_Pos.UPPER: return Mathf.Abs (ECBTransform.position.x - BOTTransform.position.x)/2;
			case RC_Pos.CENTER: return Mathf.Abs (ECBTransform.position.x - BOTTransform.position.x);
			case RC_Pos.LOWER: return Mathf.Abs (ECBTransform.position.x - BOTTransform.position.x)/2;
			case RC_Pos.VERTHI: return Mathf.Abs (TOPTransform.position.y - ECBTransform.position.y);
						case RC_Pos.VERTLO:
								if (Mathf.Abs (offset.x) > .25) {
										return 0;
								} else {
										return Mathf.Abs (ECBTransform.position.y - BOTTransform.position.y);
								}
			}
			return Mathf.Abs (ECBTransform.position.x - BOTTransform.position.x);
		}

	}

	#endregion

}
