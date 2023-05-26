using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Helpers;
using System.Collections;
using CommonComponents;
using CommonComponents.Interfaces;


namespace Player
{
	[RequireComponent(typeof(Damagable))]
	public class PlayerController : Damagable, PlayerInput.IPlayerActions
	{

		private CharacterController _characterController;

		private Vector2 _currentLookPosition;
		private Vector2 _currentMoveInputVector = Vector2.zero;
		private float _playerGrav;

		#region Serialisation

		[SerializeField] BaseWeapon _currentWeapon;
		[SerializeField] private float moveSpeed;
		[SerializeField] private float gravityValue;
		private Camera _camera;
		private Vector3 _lookDir;
		private InputAction _primaryAction;
		[SerializeField] private float _dodgePower;
		[SerializeField] private float _dodgeCooldown;
		[SerializeField] private bool _canDodge;
		[SerializeField] private bool dodging;
		[SerializeField] private float dodgingDuration;
		[SerializeField] private float dodgeDuration;

        #endregion

        // Start is called before the first frame update
		protected override void Awake()
		{
			var repo = SingletonRepo.Instance;
			repo.PlayerObject = this;
			_camera = Camera.main;
			GetComponent<Collider>();
			GetComponent<Rigidbody>();
			_characterController = GetComponent<CharacterController>();

			CacheControls();

			base.Awake();
			var hpSlider = PlayerUIManager.Instance.PlayerHPSlider;
			hpSlider.MaxValue = MaxHP;
			hpSlider.SetToMax();
			HPChanged += hpSlider.SetValues;
		}


		#region InputSetup
		private PlayerInput _controls;
		private InputAction _moveAction;
		private InputAction _lookAction;
		private InputAction _dodgeAction;
		private InputAction _secondaryAction;

		private void CacheControls()
		{
			_controls = new PlayerInput();
			_moveAction = _controls.Player.Move;
			_lookAction = _controls.Player.Look;
			_primaryAction = _controls.Player.Primary;
			_dodgeAction = _controls.Player.Dodge;
			_secondaryAction = _controls.Player.Secondary;
			
			_dodgeAction.performed += OnDodge;
			_moveAction.performed += OnMove;
			_lookAction.performed += OnLook;
			_primaryAction.started += OnPrimary;
			_primaryAction.canceled += OnPrimaryCancel;
			_secondaryAction.started += OnSecondary;
			_secondaryAction.canceled += OnSecondaryCancel;
			
		}

		private void OnEnable() { EnableControls(); }
		private void EnableControls()
		{
			_controls.Enable();
			_controls.Player.Enable();
			_canDodge = true;
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

			_characterController.Move(temp);
		}

		private void UpdateLookDir()
		{
			//transform.forward = transform.position - worldMousePos;
			//float lookAngle = Mathf.Atan2(_currentLookPosition.y, _currentLookPosition.x) * Mathf.Rad2Deg - 90f;
			//Debug.Log(lookAngle);

			//transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.up);
			var mouseRay = _camera.ScreenPointToRay(new Vector3(_currentLookPosition.x, _currentLookPosition.y, 50));
			if (Physics.Raycast(mouseRay, out var mouseRayHit, Mathf.Infinity))
			{
				var mouseToGroundPoint = mouseRayHit.point;
				var dir = transform.position - mouseToGroundPoint;
				dir.y = 0;
				_lookDir = -dir;

				transform.forward = _lookDir;

				_currentWeapon.FireDirection = _lookDir;

			}
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

		private void OnPrimaryCancel(InputAction.CallbackContext obj) { _currentWeapon.CancelPrimaryAttack(_lookDir); }
		

		public void OnSecondary(InputAction.CallbackContext context) {
			if (context.interaction is HoldInteraction)
            {
				bool holding = true;
				_currentWeapon.BeginSecondaryAttack(_lookDir, holding);
			}
				
         
		}

		private void OnSecondaryCancel(InputAction.CallbackContext obj) { _currentWeapon.CancelSecondaryAttack(_lookDir); }
		public void OnDodge(InputAction.CallbackContext context)
		{
			if (_canDodge)
			{
				dodging = true;
				StartCoroutine(DodgeCoolingDown());
			}
		}
	}
}