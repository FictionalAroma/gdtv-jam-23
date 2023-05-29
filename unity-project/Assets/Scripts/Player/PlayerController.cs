using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Helpers;
using System.Collections;
using CommonComponents;
using CommonComponents.Interfaces;
using UI;
using Management;

namespace Player
{
	[RequireComponent(typeof(Damagable))]
	public class PlayerController : Damagable, PlayerInput.IPlayerActions
	{

		private CharacterController _characterController;
		AnimControlScript AnimControlScript;
		private InteractableActor _actor;


		private Vector2 _currentLookPosition;
		private Vector2 _currentMoveInputVector = Vector2.zero;
		private float _playerGrav;
		PauseMenuManager pauseMenuManager;
		public Jukebox jukeBox;
		#region Serialisation
		public BaseWeapon[] weapons;
		public BaseWeapon _currentWeapon;
		[SerializeField] private float moveSpeed;
		[SerializeField] private float gravityValue;
		Vector3 mouseToGroundPoint;
		private Camera _camera;
		private Vector3 _lookDir;
		private InputAction _primaryAction;
		[SerializeField] private float _dodgePower;
		[SerializeField] private float _dodgeCooldown;
		[SerializeField] private bool _canDodge;
		[SerializeField] private bool dodging;
		[SerializeField] private float dodgingDuration;
		[SerializeField] private float dodgeDuration;
		[SerializeField] GameObject playerAimTarget;
		[SerializeField] GameObject playerMoveTarget;
		#endregion

        // Start is called before the first frame update
		protected override void Awake()
		{
			AnimControlScript = GetComponent<AnimControlScript>();
			SingletonRepo.PlayerObject = this;
			_camera = Camera.main;
			_characterController = GetComponent<CharacterController>();
			jukeBox = FindObjectOfType<Jukebox>();

			_actor = GetComponent<InteractableActor>();
			
			CacheControls();
			
			base.Awake();
			var hpSlider = PlayerUIManager.Instance.PlayerHPSlider;
			hpSlider.MaxValue = MaxHP;
			hpSlider.SetToMax();
            HPChangedEvent += hpSlider.SetValues;
			HPEmpty += OnDeath;
			HPEmpty += AnimControlScript.OnDeath;
			_currentWeapon = weapons[0];
			pauseMenuManager = FindObjectOfType<PauseMenuManager>();
		}


		#region InputSetup
        private PlayerInput _controls;
		private InputAction _moveAction;
		private InputAction _lookAction;
		private InputAction _dodgeAction;
		private InputAction _swapWeaponsAction;
		private InputAction _secondaryAction;
		private InputAction _activateAction;

		private void CacheControls()
		{
			
			_controls = new PlayerInput();
			_moveAction = _controls.Player.Move;
			_lookAction = _controls.Player.Look;
			_primaryAction = _controls.Player.Primary;
			_dodgeAction = _controls.Player.Dodge;
			_secondaryAction = _controls.Player.Secondary;
			_swapWeaponsAction = _controls.Player.SwapWeapon;
			_activateAction = _controls.Player.Action;

			_moveAction.performed += AnimControlScript.OnMove;
			_lookAction.performed += AnimControlScript.OnLook;
			_primaryAction.started += AnimControlScript.OnPrimary;
			_primaryAction.canceled += AnimControlScript.OnPrimaryCancel;
			_secondaryAction.started += AnimControlScript.OnSecondary;
			_secondaryAction.canceled += AnimControlScript.OnSecondaryCancel;
			_dodgeAction.performed += AnimControlScript.OnDodge;
			_dodgeAction.performed += OnDodge;
			_moveAction.performed += OnMove;
			_lookAction.performed += OnLook;
			_primaryAction.started += OnPrimary;
			_primaryAction.canceled += OnPrimaryCancel;
			_secondaryAction.started += OnSecondary;
			_secondaryAction.canceled += OnSecondaryCancel;
			_swapWeaponsAction.started += OnSwapWeapon;
			_activateAction.performed += OnAction;
		}

		private void OnEnable()
		{
			EnableControls();
			_characterController.enabled = true;
		}

		private void OnDisable()
		{
			EnableControls();
			_characterController.enabled = false;
		}
		private void EnableControls()
		{
			_controls.Enable();
			_controls.Player.Enable();
			_canDodge = true;
		}

		private void DisableControls()
		{
			_controls.Disable();
			_controls.Player.Disable();
			_canDodge = false;
		}


		#endregion


		private void Update()
		{
			PlayerDodge();
			PlayerMove();
			UpdateLookDir();
			
			
		}

		IEnumerator DodgeCoolingDown()
		{
			_canDodge = false;
			yield return new WaitForSeconds(_dodgeCooldown);
			_canDodge = true;
		}

		private void PlayerDodge()
		{
			if (dodging)
			{
				var temp = _currentMoveInputVector.normalized.ToVector3TopDown() * (Time.deltaTime * _dodgePower);
				_playerGrav = _characterController.isGrounded ? 0f : gravityValue * Time.deltaTime;

				temp.y = _playerGrav;

				_characterController.Move(temp);
				dodgingDuration -= Time.deltaTime;
				dodging = dodgingDuration > 0f;
			}
			else
			{
				dodgingDuration = dodgeDuration;
			}
		}

		private void PlayerMove()
		{
			var temp = _currentMoveInputVector.normalized.ToVector3TopDown() * (Time.deltaTime * moveSpeed);
			
			_playerGrav = _characterController.isGrounded ? 0f : gravityValue * Time.deltaTime;

			temp.y = _playerGrav;
			//playerMoveTarget.transform.position = this.transform.position+_currentMoveInputVector.ToVector3TopDown()*(Time.deltaTime*moveSpeed);
			_characterController.Move(temp);
		}

		private void UpdateLookDir()
		{
			//transform.forward = transform.position - worldMousePos;
			//float lookAngle = Mathf.Atan2(_currentLookPosition.y, _currentLookPosition.x) * Mathf.Rad2Deg - 90f;
			//Debug.Log(lookAngle);

			//transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.up);
			var mouseRay = _camera.ScreenPointToRay(new Vector3(_currentLookPosition.x, _currentLookPosition.y, 50));
			if (Physics.Raycast(mouseRay, out var mouseRayHit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
			{
				mouseToGroundPoint = mouseRayHit.point;
				playerAimTarget.transform.position = new Vector3(mouseToGroundPoint.x,this.transform.position.y,mouseToGroundPoint.z);
				var dir = transform.position - mouseToGroundPoint;
				dir.y = 0;
				_lookDir = -dir;

				transform.forward = _lookDir;

				_currentWeapon.FireDirection = _lookDir;

			}
		}

		public void OnSwapWeapon(InputAction.CallbackContext context)
		{
			_currentWeapon = _currentWeapon == weapons[0] ? weapons[1] : weapons[0];
		}

		public void OnAction(InputAction.CallbackContext context)
		{
			_actor.ActionCurrent();
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			_currentMoveInputVector = !context.canceled ? context.ReadValue<Vector2>() : Vector2.zero;
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			_currentLookPosition = !context.canceled ? context.ReadValue<Vector2>() : Vector2.zero;

			//Debug.Log($"Look Vector From Object {_currentLookPosition}");
		}

		public void OnPrimary(InputAction.CallbackContext context) { _currentWeapon.BeginPrimaryAttack(_lookDir); }

		public void OnPrimaryCancel(InputAction.CallbackContext obj) { _currentWeapon.CancelPrimaryAttack(_lookDir); }
		

		public void OnSecondary(InputAction.CallbackContext context) {
			if (context.interaction is HoldInteraction)
            {
				bool holding = true;
				_currentWeapon.BeginSecondaryAttack(_lookDir, holding);
			}
				
         
		}

		public void OnSecondaryCancel(InputAction.CallbackContext obj) { _currentWeapon.CancelSecondaryAttack(_lookDir); }
		public void OnDodge(InputAction.CallbackContext context)
		{
			if (_canDodge)
			{
				dodging = true;
				StartCoroutine(DodgeCoolingDown());
			}
		}
		private void OnDeath(Damagable obj)
        {
			_controls.Disable();
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
			
			pauseMenuManager.TogglePause();
        }
    }
}