using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 플레이어를 관리하는 클래스
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        public float maxForwardSpeed = 8f;        // How fast Ellen can run.
        public float gravity = 20f;               // How fast Ellen accelerates downwards when airborne.
        public float jumpSpeed = 10f;             // How fast Ellen takes off when jumping.
        public float minTurnSpeed = 400f;         // How fast Ellen turns when moving at maximum speed.
        public float maxTurnSpeed = 1200f;        // How fast Ellen turns when stationary.
        
        protected AnimatorStateInfo m_CurrentStateInfo;    // Information about the base layer of the animator cached.
        protected AnimatorStateInfo m_NextStateInfo;
        protected bool m_IsAnimatorTransitioning;
        protected AnimatorStateInfo m_PreviousCurrentStateInfo;    // Information about the base layer of the animator from last frame.
        protected AnimatorStateInfo m_PreviousNextStateInfo;
        protected bool m_PreviousIsAnimatorTransitioning;

        protected bool m_IsGrounded = true;            // Whether or not Ellen is currently standing on the ground.
        protected bool m_PreviouslyGrounded = true;    // Whether or not Ellen was standing on the ground last frame.
        protected bool m_ReadyToJump;                  // Whether or not the input state and Ellen are correct to allow jumping.
        protected float m_DesiredForwardSpeed;         // How fast Ellen aims be going along the ground based on input.
        protected float m_ForwardSpeed;                // How fast Ellen is currently going along the ground.
        protected float m_VerticalSpeed;               // How fast Ellen is currently moving up or down.

        protected PlayerInput m_Input;                 // Reference used to determine how Ellen should move.
        protected CharacterController m_CharCtrl;      // Reference used to actually move Ellen.
        protected Animator m_Animator;                 // Reference used to make decisions based on Ellen's current animation and to set parameters.

        protected Quaternion m_TargetRotation;         // What rotation Ellen is aiming to have based on input.
        protected float m_AngleDiff;                   // Angle in degrees between Ellen's current rotation and her target rotation.
        
        // These constants are used to ensure Ellen moves and behaves properly.
        // It is advised you don't change them without fully understanding what they do in code.
        const float k_AirborneTurnSpeedProportion = 5.4f;
        const float k_GroundedRayDistance = 1f;
        const float k_JumpAbortSpeed = 10f;
        const float k_MinEnemyDotCoeff = 0.2f;
        const float k_InverseOneEighty = 1f / 180f;
        const float k_StickingGravityProportion = 0.3f;
        const float k_GroundAcceleration = 20f;
        const float k_GroundDeceleration = 25f;

        // Parameters        

        // States

        // Tags        
        readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");
        #endregion

        #region Property
        //이동 입력값 체크
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Input.Movement.sqrMagnitude, 0f); }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_Input = GetComponent<PlayerInput>();
            m_Animator = GetComponent<Animator>();
            m_CharCtrl = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            CacheAnimatorState();
            UpdateInputBlocking();

            CalculateForwardMovement();
            CalculateVerticalMovement();

            //이동
            Vector3 movement = transform.forward * Time.deltaTime * m_ForwardSpeed;
            movement += Vector3.up * Time.deltaTime * m_VerticalSpeed;
            m_CharCtrl.Move(movement);
            m_IsGrounded = m_CharCtrl.isGrounded;

            SetTargetRotation();
            if (IsOrientationUpdated() && IsMoveInput)
                UpdateOrientation();

            TimeoutToIdle();
        }

        /*private void OnAnimatorMove()
        {
            Vector3 movement;

            // If Ellen is on the ground...
            if (m_IsGrounded)
            {
                // ... raycast into the ground...
                RaycastHit hit;
                Ray ray = new Ray(transform.position + Vector3.up * k_GroundedRayDistance * 0.5f, -Vector3.up);
                if (Physics.Raycast(ray, out hit, k_GroundedRayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    // ... and get the movement of the root motion rotated to lie along the plane of the ground.
                    movement = Vector3.ProjectOnPlane(m_Animator.deltaPosition, hit.normal);

                    // Also store the current walking surface so the correct audio is played.
                    //Renderer groundRenderer = hit.collider.GetComponentInChildren<Renderer>();
                    //m_CurrentWalkingSurface = groundRenderer ? groundRenderer.sharedMaterial : null;
                }
                else
                {
                    // If no ground is hit just get the movement as the root motion.
                    // Theoretically this should rarely happen as when grounded the ray should always hit.
                    movement = m_Animator.deltaPosition;
                    //m_CurrentWalkingSurface = null;
                }
            }
            else
            {
                // If not grounded the movement is just in the forward direction.
                movement = m_ForwardSpeed * transform.forward * Time.deltaTime;
            }

            // Rotate the transform of the character controller by the animation's root rotation.
            m_CharCtrl.transform.rotation *= m_Animator.deltaRotation;

            // Add to the movement with the calculated vertical speed.
            movement += m_VerticalSpeed * Vector3.up * Time.deltaTime;

            // Move the character controller.
            m_CharCtrl.Move(movement);

            // After the movement store whether or not the character controller is grounded.
            m_IsGrounded = m_CharCtrl.isGrounded;
        }*/
        #endregion

        #region Custom Method
        // Called at the start of FixedUpdate to record the current state of the base layer of the animator.
        void CacheAnimatorState()
        {
            m_PreviousCurrentStateInfo = m_CurrentStateInfo;
            m_PreviousNextStateInfo = m_NextStateInfo;
            m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

            m_CurrentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            m_NextStateInfo = m_Animator.GetNextAnimatorStateInfo(0);
            m_IsAnimatorTransitioning = m_Animator.IsInTransition(0);
        }

        // Called after the animator state has been cached to determine whether this script should block user input.
        void UpdateInputBlocking()
        {
            bool inputBlocked = m_CurrentStateInfo.tagHash == m_HashBlockInput && !m_IsAnimatorTransitioning;
            inputBlocked |= m_NextStateInfo.tagHash == m_HashBlockInput;
            m_Input.playerControllerInputBlocked = inputBlocked;
        }

        void CalculateForwardMovement()
        {
            // Cache the move input and cap it's magnitude at 1.
            Vector2 moveInput = m_Input.Movement;
            if (moveInput.sqrMagnitude > 1f)
                moveInput.Normalize();

            // Calculate the speed intended by input.
            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            // Determine change to speed based on whether there is currently any move input.
            float acceleration = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;

            // Adjust the forward speed towards the desired speed.
            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, acceleration * Time.deltaTime);

            // Set the animator parameter to control what animation is being played.
            
        }

        // Called each physics step.
        void CalculateVerticalMovement()
        {
            // If jump is not currently held and Ellen is on the ground then she is ready to jump.
            if (!m_Input.Jump && m_IsGrounded)
                m_ReadyToJump = true;

            if (m_IsGrounded)
            {
                // When grounded we apply a slight negative vertical speed to make Ellen "stick" to the ground.
                m_VerticalSpeed = -gravity * k_StickingGravityProportion;

                // If jump is held, Ellen is ready to jump and not currently in the middle of a melee combo...
                if (m_Input.Jump && m_ReadyToJump)// && !m_InCombo)
                {
                    // ... then override the previously set vertical speed and make sure she cannot jump again.
                    m_VerticalSpeed = jumpSpeed;
                    m_IsGrounded = false;
                    m_ReadyToJump = false;
                }
            }
            else
            {
                // If Ellen is airborne, the jump button is not held and Ellen is currently moving upwards...
                if (!m_Input.Jump && m_VerticalSpeed > 0.0f)
                {
                    // ... decrease Ellen's vertical speed.
                    // This is what causes holding jump to jump higher that tapping jump.
                    m_VerticalSpeed -= k_JumpAbortSpeed * Time.deltaTime;
                }

                // If a jump is approximately peaking, make it absolute.
                if (Mathf.Approximately(m_VerticalSpeed, 0f))
                {
                    m_VerticalSpeed = 0f;
                }

                // If Ellen is airborne, apply gravity.
                m_VerticalSpeed -= gravity * Time.deltaTime;
            }
        }

        // Called each physics step to set the rotation Ellen is aiming to have.
        void SetTargetRotation()
        {
            // Create three variables, move input local to the player, flattened forward direction of the camera and a local target rotation.
            Vector2 moveInput = m_Input.Movement;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            //Vector3 forward = Quaternion.Euler(0f, cameraSettings.Current.m_XAxis.Value, 0f) * Vector3.forward;
            Vector3 forward = Quaternion.Euler(localMovementDirection.x, localMovementDirection.y, localMovementDirection.z) * Vector3.forward;
            forward.y = 0f;
            forward.Normalize();

            Quaternion targetRotation;

            // If the local movement direction is the opposite of forward then the target rotation should be towards the camera.
            if (Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f))
            {
                targetRotation = Quaternion.LookRotation(-forward);
            }
            else
            {
                // Otherwise the rotation should be the offset of the input from the camera's forward.
                Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward, localMovementDirection);
                targetRotation = Quaternion.LookRotation(cameraToInputOffset * forward);
            }

            // The desired forward direction of Ellen.
            Vector3 resultingForward = targetRotation * Vector3.forward;

            // Find the difference between the current rotation of the player and the desired rotation of the player in radians.
            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

            m_AngleDiff = Mathf.DeltaAngle(angleCurrent, targetAngle);
            m_TargetRotation = targetRotation;
        }

        // Called each physics step to help determine whether Ellen can turn under player input.
        bool IsOrientationUpdated()
        {
            //bool updateOrientationForLocomotion = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLocomotion || m_NextStateInfo.shortNameHash == m_HashLocomotion;
            //bool updateOrientationForAirborne = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashAirborne || m_NextStateInfo.shortNameHash == m_HashAirborne;
            //bool updateOrientationForLanding = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLanding || m_NextStateInfo.shortNameHash == m_HashLanding;

            //return updateOrientationForLocomotion || updateOrientationForAirborne || updateOrientationForLanding;// || m_InCombo && !m_InAttack;
            return true;
        }

        // Called each physics step after SetTargetRotation if there is move input and Ellen is in the correct animator state according to IsOrientationUpdated.
        void UpdateOrientation()
        {
            //m_Animator
            

            Vector3 localInput = new Vector3(m_Input.Movement.x, 0f, m_Input.Movement.y);
            float groundedTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
            float actualTurnSpeed = m_IsGrounded ? groundedTurnSpeed : Vector3.Angle(transform.forward, localInput) * k_InverseOneEighty * k_AirborneTurnSpeedProportion * groundedTurnSpeed;
            m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, actualTurnSpeed * Time.deltaTime);

            transform.rotation = m_TargetRotation;
        }

        void TimeoutToIdle()
        {
            
        }
        #endregion
    }
}
