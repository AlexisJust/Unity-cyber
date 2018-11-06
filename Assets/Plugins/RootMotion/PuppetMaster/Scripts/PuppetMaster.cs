using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using RootMotion;

namespace RootMotion.Dynamics {

	/// <summary>
	/// The master of puppets. Enables character animation to be played physically in muscle space.
	/// </summary>
	[HelpURL("https://www.youtube.com/watch?v=LYusqeqHAUc")]
	[AddComponentMenu("Scripts/RootMotion.Dynamics/PuppetMaster/Puppet Master")]
	public partial class PuppetMaster: MonoBehaviour {

		// Open the User Manual URL
		[ContextMenu("User Manual (Setup)")]
		void OpenUserManualSetup() {
			Application.OpenURL("http://root-motion.com/puppetmasterdox/html/page4.html");
		}

		// Open the User Manual URL
		[ContextMenu("User Manual (Component)")]
		void OpenUserManualComponent() {
			Application.OpenURL("http://root-motion.com/puppetmasterdox/html/page5.html");
		}

		[ContextMenu("User Manual (Performance)")]
		void OpenUserManualPerformance() {
			Application.OpenURL("http://root-motion.com/puppetmasterdox/html/page8.html");
		}
		
		// Open the Script Reference URL
		[ContextMenu("Scrpt Reference")]
		void OpenScriptReference() {
			Application.OpenURL("http://root-motion.com/puppetmasterdox/html/class_root_motion_1_1_dynamics_1_1_puppet_master.html");
		}
		
		// Open a video tutorial about setting up the component
		[ContextMenu("TUTORIAL VIDEO (SETUP)")]
		void OpenSetupTutorial() {
			Application.OpenURL("https://www.youtube.com/watch?v=mIN9bxJgfOU&index=2&list=PLVxSIA1OaTOuE2SB9NUbckQ9r2hTg4mvL");
		}

		// Open a video tutorial about setting up the component
		[ContextMenu("TUTORIAL VIDEO (COMPONENT)")]
		void OpenComponentTutorial() {
			Application.OpenURL("https://www.youtube.com/watch?v=LYusqeqHAUc");
		}

		/// <summary>
		/// Active mode means all muscles are active and the character is physically simulated. Kinematic mode sets rigidbody.isKinematic to true for all the muscles and simply updates their position/rotation to match the target's. Disabled mode disables the ragdoll. Switching modes is done by simply changing this value, blending in/out will be handled automatically by the PuppetMaster.
		/// </summary>
		[System.Serializable]
		public enum Mode {
			Active,
			Kinematic,
			Disabled
		}

		[LargeHeader("Simulation")]

		[Tooltip("Sets/sets the state of the puppet (Alive, Dead or Frozen).")]
		/// <summary>
		/// Sets/sets the state of the puppet (Alive, Dead or Frozen).
		/// </summary>
		public State state;

		[ContextMenuItem("Reset To Default", "ResetStateSettings")]
		[Tooltip("Settings for killing and freezing the puppet.")]
		/// <summary>
		/// Settings for killing and freezing the puppet..
		/// </summary>
		public StateSettings stateSettings = StateSettings.Default;

		void ResetStateSettings() {
			stateSettings = StateSettings.Default;
		}

		[Tooltip("Active mode means all muscles are active and the character is physically simulated. Kinematic mode sets rigidbody.isKinematic to true for all the muscles and simply updates their position/rotation to match the target's. Disabled mode disables the ragdoll. Switching modes is done by simply changing this value, blending in/out will be handled automatically by the PuppetMaster.")] 
		/// <summary>
		/// Active mode means all muscles are active and the character is physically simulated. Kinematic mode sets rigidbody.isKinematic to true for all the muscles and simply updates their position/rotation to match the target's. Disabled mode disables the ragdoll. Switching modes is done by simply changing this value, blending in/out will be handled automatically by the PuppetMaster.
		/// </summary>
		public Mode mode;

		[Tooltip("The time of blending when switching from Active to Kinematic/Disabled or from Kinematic/Disabled to Active. Switching from Kinematic to Disabled or vice versa will be done instantly.")]
		/// <summary>
		/// The time of blending when switching from Active to Kinematic/Disabled or from Kinematic/Disabled to Active. Switching from Kinematic to Disabled or vice versa will be done instantly.
		/// </summary>
		public float blendTime = 0.1f;

		[Tooltip("If true, will fix the target character's Transforms to their default local positions and rotations in each update cycle to avoid drifting from additive reading-writing. Use this only if the target contains unanimated bones.")]
		/// <summary>
		/// If true, will fix the target character's Transforms to their default local positions and rotations in each update cycle to avoid drifting from additive reading-writing. Use this only if the target contains unanimated bones.
		/// </summary>
		public bool fixTargetTransforms = true;

		[Tooltip("Rigidbody.solverIterationCount for the muscles of this Puppet.")]
		/// <summary>
		/// Rigidbody.solverIterationCount for the muscles of this Puppet.
		/// </summary>
		public int solverIterationCount = 6;

		[Tooltip("If true, will draw the target's pose as green lines in the Scene view. This runs in the Editor only. If you wish to profile PuppetMaster, switch this off.")]
		/// <summary>
		/// If true, will draw the target's pose as green lines in the Scene view. This runs in the Editor only. If you wish to profile PuppetMaster, switch this off.
		/// </summary>
		public bool visualizeTargetPose = true;

		[LargeHeader("Master Weights")]

		[Tooltip("The weight of mapping the animated character to the ragdoll pose.")]
		/// <summary>
		/// The weight of mapping the animated character to the ragdoll pose.
		/// </summary>
		[Range(0f, 1f)] public float mappingWeight = 1f;

		[Tooltip("The weight of pinning the muscles to the position of their animated targets using simple AddForce.")]
		/// <summary>
		/// The weight of pinning the muscles to the position of their animated targets using simple AddForce.
		/// </summary>
		[Range(0f, 1f)] public float pinWeight = 1f;

		[Tooltip("The normalized strength of the muscles.")]
		/// <summary>
		/// The normalized strength of the muscles.
		/// </summary>
		[Range(0f, 1f)] public float muscleWeight = 1f;

		[LargeHeader("Joint and Muscle Settings")]

		[Tooltip("The positionSpring of the ConfigurableJoints' Slerp Drive.")]
		/// <summary>
		/// The positionSpring of the ConfigurableJoints' Slerp Drive.
		/// </summary>
		public float muscleSpring = 100f;

		[Tooltip("The positionDamper of the ConfigurableJoints' Slerp Drive.")]
		/// <summary>
		/// The positionDamper of the ConfigurableJoints' Slerp Drive.
		/// </summary>
		public float muscleDamper = 0f;

		[Tooltip("Adjusts the slope of the pinWeight curve. Has effect only while interpolating pinWeight from 0 to 1 and back.")]
		/// <summary>
		/// Adjusts the slope of the pinWeight curve. Has effect only while interpolating pinWeight from 0 to 1 and back.
		/// </summary>
		[Range(1f, 8f)] public float pinPow = 4f;

		[Tooltip("Reduces pinning force the farther away the target is. Bigger value loosens the pinning, resulting in sloppier behaviour.")]
		/// <summary>
		/// Reduces pinning force the farther away the target is. Bigger value loosens the pinning, resulting in sloppier behaviour.
		/// </summary>
		[Range(0f, 100f)] public float pinDistanceFalloff = 5;

		[Tooltip("When the target has animated bones between the muscle bones, the joint anchors need to be updated in every update cycle because the muscles' targets move relative to each other in position space. This gives much more accurate results, but is computationally expensive so consider leaving it off.")]
		/// <summary>
		/// When the target has animated bones between the muscle bones, the joint anchors need to be updated in every update cycle because the muscles' targets move relative to each other in position space. This gives much more accurate results, but is computationally expensive so consider leaving it off.
		/// </summary>
		public bool updateJointAnchors = true;

		[Tooltip("Enable this if any of the target's bones has translation animation.")]
		/// <summary>
		/// Enable this if any of the target's bones has translation animation.
		/// </summary>
		public bool supportTranslationAnimation;

		[Tooltip("Should the joints use angular limits? If the PuppetMaster fails to match the target's pose, it might be because the joint limits are too stiff and do not allow for such motion. Uncheck this to see if the limits are clamping the range of your puppet's animation. Since the joints are actuated, most PuppetMaster simulations will not actually require using joint limits at all.")]
		/// <summary>
		/// Should the joints use angular limits? If the PuppetMaster fails to match the target's pose, it might be because the joint limits are too stiff and do not allow for such motion. Uncheck this to see if the limits are clamping the range of your puppet's animation. Since the joints are actuated, most PuppetMaster simulations will not actually require using joint limits at all.
		/// </summary>
		public bool angularLimits;

		[Tooltip("Should the muscles collide with each other? Consider leaving this off while the puppet is pinned for performance and better accuracy.  Since the joints are actuated, most PuppetMaster simulations will not actually require internal collisions at all.")]
		/// <summary>
		/// Should the muscles collide with each other? Consider leaving this off while the puppet is pinned for performance and better accuracy.  Since the joints are actuated, most PuppetMaster simulations will not actually require internal collisions at all.
		/// </summary>
		public bool internalCollisions;

		/// <summary>
		/// If this flag is false, the MuscleCollisionBroadcasters will not broadcast collision events to the Puppet Behaviours. This value should be set by Puppet Behaviours only.
		/// </summary>
		[HideInInspector] public bool broadcastCollisions;

		[LargeHeader("Individual Muscle Settings")]

		[Tooltip("The Muscles managed by this PuppetMaster.")]
		/// <summary>
		/// The Muscles managed by this PuppetMaster.
		/// </summary>
		public Muscle[] muscles = new Muscle[0];

		public delegate void UpdateDelegate();

		/// <summary>
		/// Called after the puppet has initiated.
		/// </summary>
		public UpdateDelegate OnPostInitiate;

		/// <summary>
		/// Called before (and only if) reading.
		/// </summary>
		public UpdateDelegate OnRead;

		/// <summary>
		/// Called after (and only if) writing
		/// </summary>
		public UpdateDelegate OnWrite;

		/// <summary>
		/// Called after each LateUpdate.
		/// </summary>
		public UpdateDelegate OnPostLateUpdate;

		/// <summary>
		/// Called when it's the right time to fix target transforms.
		/// </summary>
		public UpdateDelegate OnFixTransforms;

		/// <summary>
		/// Called when the puppet hierarchy has changed by adding/removing muscles
		/// </summary>
		public UpdateDelegate OnHierarchyChanged;

		/// <summary>
		/// The root Transform of the animated target character.
		/// </summary>
		public Transform targetRoot { get; private set; }

		/// <summary>
		/// Gets the Animator on the target.
		/// </summary>
		public Animator targetAnimator { 
			get {
				// Protect from the Animator being replaced (UMA)
				if (_targetAnimator == null) _targetAnimator = targetRoot.GetComponentInChildren<Animator>();
				return _targetAnimator;
			}
			set {
				_targetAnimator = value;
			}
		}
		private Animator _targetAnimator;

		/// <summary>
		/// Gets the Animation component on the target.
		/// </summary>
		public Animation targetAnimation { get; private set; }

		/// <summary>
		/// Array of all Puppet Behaviours
		/// </summary>
		/// <value>The behaviours.</value>
		public BehaviourBase[] behaviours { get; private set; } // @todo add/remove behaviours in runtime (add OnDestroy to BehaviourBase)

		/// <summary>
		/// Returns true if the PuppetMaster is in active mode or blending in/out of it.
		/// </summary>
		public bool isActive { get { return isActiveAndEnabled && initiated && (activeMode == Mode.Active || isBlending); }}

		/// <summary>
		/// Has this PuppetMaster successfully initiated?
		/// </summary>
		public bool initiated { get; private set; }

		/// <summary>
		/// The list of solvers that will be updated by this PuppetMaster. When you add a Final-IK component in runtime after PuppetMaster has initiated, add it to this list using solver.Add(SolverManager solverManager).
		/// </summary>
		[HideInInspector] public List<SolverManager> solvers = new List<SolverManager>();

		/// <summary>
		/// Normal means Animator is in Normal or Unscaled Time or Animation has Animate Physics unchecked.
		/// AnimatePhysics is Legacy only, when the Animation component has Animate Physics checked.
		/// FixedUpdate means Animator is used and in Animate Physics mode. In this case PuppetMaster will take control of updating the Animator in FixedUpdate.
		/// </summary>
		[System.Serializable]
		public enum UpdateMode {
			Normal,
			AnimatePhysics,
			FixedUpdate
		}

		/// <summary>
		/// Gets the current update mode.
		/// </summary>
		/// <value>The update mode.</value>
		public UpdateMode updateMode {
			get {
				return targetUpdateMode == AnimatorUpdateMode.AnimatePhysics? (isLegacy? UpdateMode.AnimatePhysics: UpdateMode.FixedUpdate): UpdateMode.Normal;
			}
		}

		/// <summary>
		/// If the Animator's update mode is "Animate Phyics", PuppetMaster will take control of updating the Animator (in FixedUpdate). This does not happen with Legacy.
		/// </summary>
		public bool controlsAnimator {
			get {
				return isActiveAndEnabled && isActive && initiated && updateMode == UpdateMode.FixedUpdate;
			}
		}

		#region Update Sequence

		private bool internalCollisionsEnabled = true;
		private bool angularLimitsEnabled = true;
		private bool fixedFrame;
		private int lastSolverIterationCount;
		private bool isLegacy;
		private bool animatorDisabled;
		private bool awakeFailed;
		private bool interpolated;
		private bool freezeFlag;

		void OnDisable() {
			if (!gameObject.activeInHierarchy && initiated) foreach (Muscle m in muscles) m.Reset();
		}
		
		void Awake() {
#if UNITY_5_1
			Debug.LogError("PuppetMaster requires at least Unity 5.2.2.");
			awakeFailed = true;
			return;
#endif

			// Do not initiate when the component has been added in run-time. The muscles have not been set up yet.
			if (muscles.Length == 0) return;

			broadcastCollisions = false;

			Initiate();
			if (!initiated) awakeFailed = true;
		}

		void Start() {
#if UNITY_EDITOR
			if (UnityEngine.Profiling.Profiler.enabled && visualizeTargetPose) Debug.Log("Switch 'Visualize Target Pose' off when profiling PuppetMaster.", transform);
#endif
			if (!initiated && !awakeFailed) {
				Initiate();
			}

			if (!initiated) return;

			// Find the SolverManagers on the Target hierarchy
			var solversArray = (SolverManager[])targetRoot.GetComponentsInChildren<SolverManager>();
			solvers.AddRange(solversArray);
		}

		private Transform FindTargetRootRecursive(Transform t) {
			if (t.parent == null) return null;

			foreach (Transform child in t.parent) {
				if (child == transform) return t;
			}

			return FindTargetRootRecursive(t.parent);
		}

		private void Initiate() {
			initiated = false;

			// Find the target root
			if (muscles.Length > 0 && muscles[0].target != null) targetRoot = FindTargetRootRecursive(muscles[0].target);
			if (targetRoot != null) {
				targetAnimator = targetRoot.GetComponentInChildren<Animator>();
				targetAnimation = targetRoot.GetComponentInChildren<Animation>();
			}

			// Validation
			if (!IsValid(true)) return;

			isLegacy = targetAnimator == null && targetAnimation != null;

			behaviours = transform.parent.GetComponentsInChildren<BehaviourBase>();
			
			for (int i = 0; i < muscles.Length; i++) {
				// Initiating the muscles
				muscles[i].Initiate(muscles);
				
				// Collision event broadcasters
				if (behaviours.Length > 0) {
					muscles[i].broadcaster = muscles[i].joint.gameObject.AddComponent<MuscleCollisionBroadcaster>();
					muscles[i].broadcaster.puppetMaster = this;
					muscles[i].broadcaster.muscleIndex = i;
				}
			}
			
			UpdateHierarchies();
			
			initiated = true;

			// Initiate behaviours
			foreach (BehaviourBase behaviour in behaviours) {
				behaviour.Initiate(this);
			}

			// Switching states
			SwitchStates();

			// Switching modes
			SwitchModes();
			
			foreach (Muscle m in muscles) m.Read();
			
			// Mapping
			StoreTargetMappedState();

			if (PuppetMasterSettings.instance != null) {
				PuppetMasterSettings.instance.Register(this);
			}

			if (OnPostInitiate != null) OnPostInitiate();
		}

		void OnDestroy() {
			if (PuppetMasterSettings.instance != null) {
				PuppetMasterSettings.instance.Unregister(this);
			}
		}

		private bool IsInterpolated() {
			if (!initiated) return false;

			foreach (Muscle m in muscles) {
				if (m.rigidbody.interpolation != RigidbodyInterpolation.None) return true;
			}

			return false;
		}

		protected virtual void FixedUpdate() {
			if (!initiated) return;
			interpolated = IsInterpolated();

			fixedFrame = true;
			if (!isActive) return;

			pinWeight = Mathf.Clamp(pinWeight, 0f, 1f);
			muscleWeight = Mathf.Clamp(muscleWeight, 0f, 1f);
			muscleSpring = Mathf.Clamp(muscleSpring, 0f, muscleSpring);
			muscleDamper = Mathf.Clamp(muscleDamper, 0f, muscleDamper);
			pinPow = Mathf.Clamp(pinPow, 1f, 8f);
			pinDistanceFalloff = Mathf.Max(pinDistanceFalloff, 0f);

			// If updating the Animator manually here in FixedUpdate
			if (updateMode == UpdateMode.FixedUpdate) {
				FixTargetTransforms();

				if (targetAnimator.enabled || (!targetAnimator.enabled && animatorDisabled)) {
					targetAnimator.enabled = false;
					animatorDisabled = true;
					targetAnimator.Update(Time.fixedDeltaTime);
				} else {
					animatorDisabled = false;
					targetAnimator.enabled = false;
				}

				foreach (SolverManager solver in solvers) {
					if (solver != null) solver.UpdateSolverExternal();
				}

				Read();
			}

			if (!isFrozen) {
				// Update internal collision ignoring
				SetInternalCollisions(internalCollisions);
					
				// Update angular limit ignoring
				SetAngularLimits(angularLimits);


				// Update anchors
				if (isAlive && updateJointAnchors) {
					// @todo not last animated pose here, but last mapped pose, move this to LateUpdate maybe, remove muscle.targetLocalPosition
					// @todo might thi be that when Kinematic, joints are under stress for not having their anchors updated and will fly away when the puppet is activated?
					for (int i = 0; i < muscles.Length; i++) muscles[i].UpdateAnchor(supportTranslationAnimation);
				}

				// Set solver iteration count
				if (solverIterationCount != lastSolverIterationCount) {
					for (int i = 0; i < muscles.Length; i++) {
						muscles[i].rigidbody.solverIterations = solverIterationCount;
					}

					lastSolverIterationCount = solverIterationCount;
				}

				// Update Muscles
				for (int i = 0; i < muscles.Length; i++) {
					muscles[i].Update(pinWeight, muscleWeight, muscleSpring, muscleDamper, pinPow, pinDistanceFalloff, true);
				}
			}
			
			// Fix transforms to be sure of not having any drifting when the target bones are not animated
			if (updateMode == UpdateMode.AnimatePhysics) FixTargetTransforms();
		}

		protected virtual void Update() {
			if (!initiated) return;

			if (animatorDisabled) {
				targetAnimator.enabled = true;
				animatorDisabled = false;
			}

			if (updateMode != UpdateMode.Normal) return;

			// Fix transforms to be sure of not having any drifting when the target bones are not animated
			FixTargetTransforms();
		}

		protected virtual void LateUpdate() {
			OnLateUpdate();

			if (OnPostLateUpdate != null) OnPostLateUpdate();
		}

		protected virtual void OnLateUpdate() {
			if (!initiated) return;

			if (animatorDisabled) {
				targetAnimator.enabled = true;
				animatorDisabled = false;
			}

			// Switching states
			SwitchStates();

			// Switching modes
			SwitchModes();

			// Update modes
			switch(updateMode) {
			case UpdateMode.FixedUpdate:
				if (!fixedFrame && !interpolated) return;
				break;
			case UpdateMode.AnimatePhysics:
				if (!fixedFrame && !interpolated) return;
				if (isActive && !fixedFrame) Read();
				break;
			case UpdateMode.Normal:
				if (isActive) Read();
				break;
			}

			// Below is common code for all update modes! For AnimatePhysics modes the following code will run only in fixed frames
			fixedFrame = false;

			// Mapping
			if (!isFrozen) {
				mappingWeight = Mathf.Clamp(mappingWeight, 0f, 1f);
				float mW = mappingWeight * mappingBlend;

				if (mW > 0f) {
					if (isActive) {
						for (int i = 0; i < muscles.Length; i++) muscles[i].Map(mW);
					}
				} else {
					// Moving to Target when in Kinematic mode
					if (activeMode == Mode.Kinematic) MoveToTarget();
				}

				foreach (BehaviourBase behaviour in behaviours) behaviour.OnWrite();
				if (OnWrite != null) OnWrite();

				StoreTargetMappedState(); //@todo no need to do this all the time
			}

			// Freezing
			if (freezeFlag) OnFreezeFlag();
		}

		// Moves the muscles to where their targets are.
		private void MoveToTarget() {
			if (PuppetMasterSettings.instance == null || (PuppetMasterSettings.instance != null && PuppetMasterSettings.instance.UpdateMoveToTarget(this))) {

				foreach (Muscle m in muscles) {
					m.MoveToTarget();
				}
			}
		}

		// Read the current animated target pose
		private void Read() {
			if (OnRead != null) OnRead();
			foreach (BehaviourBase behaviour in behaviours) behaviour.OnRead();

			if (!isAlive) return;

			#if UNITY_EDITOR
			VisualizeTargetPose();
			#endif

			foreach (Muscle m in muscles) m.Read();
		}

		// Fix transforms to be sure of not having any drifting when the target bones are not animated
		private void FixTargetTransforms() {
			if (!isAlive) return;
			if (OnFixTransforms != null) OnFixTransforms();

			foreach (BehaviourBase behaviour in behaviours) behaviour.OnFixTransforms();
			
			if (!fixTargetTransforms && !hasProp) return;
			if (!isActive) return;
			
			mappingWeight = Mathf.Clamp(mappingWeight, 0f, 1f);
			
			float mW = mappingWeight * mappingBlend;
			if (mW <= 0f) return;

			for (int i = 0; i < muscles.Length; i++) {
				if (fixTargetTransforms || muscles[i].props.group == Muscle.Group.Prop) {
					muscles[i].FixTargetTransforms();
				}
			}
		}

		// Which update mode is the target's Animator/Animation using?
		private AnimatorUpdateMode targetUpdateMode {
			get {
				if (targetAnimator != null) return targetAnimator.updateMode;
				if (targetAnimation != null) return targetAnimation.animatePhysics? AnimatorUpdateMode.AnimatePhysics: AnimatorUpdateMode.Normal;
				return AnimatorUpdateMode.Normal;
			}
		}

		#endregion Update Sequence

		// Visualizes the target pose exactly as it is read by the PuppetMaster
		private void VisualizeTargetPose() {
			if (!visualizeTargetPose) return;
			if (!Application.isEditor) return;
			if (!isActive) return;

			foreach (Muscle m in muscles) {
				if (m.joint.connectedBody != null && m.connectedBodyTarget != null) {
					Debug.DrawLine(m.target.position, m.connectedBodyTarget.position, Color.cyan);
					
					bool isEndMuscle = true;
					foreach (Muscle m2 in muscles) {
						if (m != m2 && m2.joint.connectedBody == m.rigidbody) {
							isEndMuscle = false;
							break;
						}
					}
					
					if (isEndMuscle) VisualizeHierarchy(m.target, Color.cyan);
				}
			}
		}
		
		// Recursively visualizes a bone hierarchy
		private void VisualizeHierarchy(Transform t, Color color) {
			for (int i = 0; i < t.childCount; i++) {
				Debug.DrawLine(t.position, t.GetChild(i).position, color);
				VisualizeHierarchy(t.GetChild(i), color);
			}
		}

		// Update internal collision ignoring
		private void SetInternalCollisions(bool collide) {
			if (internalCollisionsEnabled == collide) return;
			
			for (int i = 0; i < muscles.Length; i++) {
				for (int i2 = i; i2 < muscles.Length; i2++) {
					if (i != i2) {
						muscles[i].IgnoreCollisions(muscles[i2], !collide);
					}
				}
			}
			
			internalCollisionsEnabled = collide;
		}

		// Update angular limit ignoring
		private void SetAngularLimits(bool limited) {
			if (angularLimitsEnabled == limited) return;
			
			for (int i = 0; i < muscles.Length; i++) {
				muscles[i].IgnoreAngularLimits(!limited);
			}
			
			angularLimitsEnabled = limited;
		}
	}
}
