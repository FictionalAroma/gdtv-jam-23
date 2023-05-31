using System;
using CommonComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hacking.Player
{
	public class HackingPlayerController :  Damagable, PlayerInput.IPlayerActions
	{
		public Vector2 _currentLookPosition;
		private Vector2 _currentMoveInputVector = Vector2.zero;
		private float _playerGrav;

		[SerializeField] private TMPro.TMP_Text hpText;
		[SerializeField] private HackingManager manager;

		#region Serialisation
		public HackingBullet hackingBullet;
		public HackingGrenade hackingGrenade;
		[SerializeField] private float moveSpeed;
		[SerializeField] private float gravityValue;
		private Camera _camera;
	
		private InputAction _primaryAction;
		[SerializeField] private float _dodgePower;
		[SerializeField] private float _dodgeCooldown;
		[SerializeField] private bool _canDodge;
		[SerializeField] private bool dodging;
		[SerializeField] private float dodgingDuration;
		[SerializeField] private float dodgeDuration;
		[SerializeField] GameObject playerAimTarget;
		[SerializeField] GameObject playerMoveTarget;
		Vector2 lookDir;
		#endregion

		// Start is called before the first frame update
		public new void Awake()
		{
		
		
			_camera = Camera.main;

			base.Awake();
			CacheControls();
			EnableControls();

		
			//var hpSlider = PlayerUIManager.Instance.PlayerHPSlider;
			//hpSlider.MaxValue = MaxHP;
			//hpSlider.SetToMax();
			HPChangedEvent += UpdateHP;
			HPEmpty += manager.PlayerLose;

		}

		private void UpdateHP(float changeby, float newhp)
		{
			hpText.text = newhp.ToString("#00%");
		}


		#region InputSetup
		private PlayerInput _controls;
		private InputAction _moveAction;
		private InputAction _lookAction;
		private InputAction _secondaryAction;
		[SerializeField] private Camera hackingCamera;

		#endregion
		private void CacheControls()
		{

			_controls = new PlayerInput();
			_moveAction = _controls.Player.Move;
			_lookAction = _controls.Player.Look;
			_primaryAction = _controls.Player.Primary;
			_secondaryAction = _controls.Player.Secondary;

		
			_moveAction.performed += OnMove;
			_lookAction.performed += OnLook;
			_primaryAction.started += OnPrimary;
			_primaryAction.canceled += OnPrimaryCancel;
			_secondaryAction.started += OnSecondary;
			_secondaryAction.canceled += OnSecondaryCancel;
		}

		private void OnPrimaryCancel(InputAction.CallbackContext obj)
		{
			Debug.Log("primaryCancel");
		}

		private void OnSecondaryCancel(InputAction.CallbackContext obj)
		{
			Debug.Log("secondary");
		}

		private void OnEnable()
		{
			EnableControls();
		
		}

		private void OnDisable()
		{
			DisableControls();
		
		}
		private void EnableControls()
		{
			_controls.Enable();
			_controls.Player.Enable();
			_canDodge = true;
		}

		public void DisableControls()
		{
			_controls.Disable();
			_controls.Player.Disable();
			_canDodge = false;
		}
	

		// Update is called once per frame
		void Update()
		{

			//PlayerDodge();
			PlayerMove();
			UpdateLookDir();
		}
		public void PlayerMove()
		{
			var temp = _currentMoveInputVector.normalized * (Time.deltaTime * moveSpeed);

        
			transform.Translate(temp);
		}
		private void UpdateLookDir()
		{
			var screenToWorldPoint = hackingCamera.ScreenToWorldPoint(_currentLookPosition);
			lookDir = screenToWorldPoint - transform.position;
			//Debug.Log(lookAngle);
			playerAimTarget.transform.position = screenToWorldPoint;
			float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

			transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
		}
		
		public void OnMove(InputAction.CallbackContext context)
		{
			_currentMoveInputVector = !context.canceled ? context.ReadValue<Vector2>() : Vector2.zero;
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			_currentLookPosition = !context.canceled ? context.ReadValue<Vector2>() : Vector2.zero;
		}

		public void OnPrimary(InputAction.CallbackContext context)
		{
			var newBullet = Instantiate(hackingBullet,transform.position,Quaternion.identity);
			newBullet.moveVector = lookDir.normalized;
			Debug.Log(newBullet.moveVector);

		}

		public void OnSecondary(InputAction.CallbackContext context)
		{
			var newGrenade = Instantiate(hackingGrenade, transform.position, Quaternion.identity);
		
		
		}

		public void OnDodge(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

		public void OnSwapWeapon(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

		public void OnAction(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

		public void OnDoPause(InputAction.CallbackContext context) { throw new NotImplementedException(); }

		public void OnEscape(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}
