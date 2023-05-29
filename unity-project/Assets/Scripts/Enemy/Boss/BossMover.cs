using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Boss
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class BossMover : MonoBehaviour
	{
		protected NavMeshAgent _navMeshAgent;
		[SerializeField] protected Transform lookTarget;
		[SerializeField] protected float targetPositionUpdateFrequency = 2.0f;
		[SerializeField] protected float moveSpeed = 6f;
		[SerializeField] protected float jumpSpeed = 15.0f;

		public float GetDistanceRemaining => _navMeshAgent.remainingDistance;
		public Transform Target
		{
			get => target;
			set => target = value;
		}

		public Vector3 WalkPosition { get; set; }


		protected float _moveAdjustmentTimer = 2.0f;

		[SerializeField] private Transform target;
		public bool moveTo { get; set; } = true;
		public bool lookAt { get; set; } = true;



		protected virtual void Awake()
		{
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_moveAdjustmentTimer = targetPositionUpdateFrequency;
		}
		
		// Update is called once per frame
		protected virtual void Update()
		{
			_moveAdjustmentTimer -= Time.deltaTime;

			if (target != null && target.gameObject.activeInHierarchy)
			{
				if (lookAt)
				{
					transform.rotation = Quaternion.LookRotation(target.position - this.transform.position);
				}
				
				if (_moveAdjustmentTimer < 0f && moveTo )
				{
					SetNavDestination(target.position);
				}
			}

		}

		protected void SetNavDestination(Vector3 targetPosition)
		{
			_navMeshAgent.SetDestination(targetPosition);
			_moveAdjustmentTimer = targetPositionUpdateFrequency;
		}

		public void StopMoving()
        {
			_navMeshAgent.isStopped= true;
        }
		public void OnEnable()
		{
			_navMeshAgent.enabled = true;
		}

		public void OnDisable()
		{
			_navMeshAgent.enabled = false;
		}
		
		public void SetPosition(Vector3 position) => SetNavDestination(position);

		public void JumpToPosition(Vector3 position)
		{
			SetPosition(position);
			_navMeshAgent.speed = jumpSpeed;
			_navMeshAgent.acceleration = jumpSpeed / 2;
		}

		public float TargetStoppingRange
		{
			get => _navMeshAgent.stoppingDistance;
			set => _navMeshAgent.stoppingDistance = value;
		}
	}
}
