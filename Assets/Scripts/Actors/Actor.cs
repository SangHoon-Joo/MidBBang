using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using Krafton.SP2.X1.Lib;

namespace ProjectWilson
{
	public class Actor : MonoBehaviour
	{
		public Transform Transform
		{
			private set;
			get;
		}

		public Vector3 Position
		{
			set { Transform.position = value; }
			get { return Transform.position; }
		}
		public Quaternion Rotation
		{
			set { Transform.rotation = value; }
			get { return Transform.rotation; }
		}
		public Vector3 EulerAngles
		{
			set { Transform.eulerAngles = value; }
			get { return Transform.eulerAngles; }
		}
		public Vector3 LocalEulerAngles
		{
			set { Transform.localEulerAngles = value; }
			get { return Transform.localEulerAngles; }
		}
		public Vector3 LocalScale
		{
			set { Transform.localScale = value; }
			get { return Transform.localScale; }
		}
		public Vector3 PrevLocalPosition;
		public Vector3 PrevLocalEulerAngles;
		public Vector3 PrevLocalScale;

		public Vector3 GetForwardVectorTowardTarget(Actor targetActor)
		{
			return Vector3.Cross(Vector3.Cross(Transform.up, targetActor.Position - Position), Transform.up);
		}

		public Vector3 GetForwardVectorTowardTarget(Vector3 targetPosition)
		{
			return Vector3.Cross(Vector3.Cross(Transform.up, targetPosition - Position), Transform.up);
		}

		public Vector3 GetForwardVectorTowardTargetOpposite(Actor targetActor)
		{
			return Vector3.Cross(Vector3.Cross(Transform.up, Position - targetActor.Position), Transform.up);
		}

		public Vector3 GetForwardVectorTowardTargetOpposite(Vector3 targetPosition)
		{
			return Vector3.Cross(Vector3.Cross(Transform.up, Position - targetPosition), Transform.up);
		}

		public Quaternion GetQuaternionTowardTarget(Actor targetActor)
		{
			return Quaternion.LookRotation(GetForwardVectorTowardTarget(targetActor), Transform.up);
		}

		public Quaternion GetQuaternionTowardTarget(Vector3 targetPosition)
		{
			return Quaternion.LookRotation(GetForwardVectorTowardTarget(targetPosition), Transform.up);
		}

		public Quaternion GetQuaternionTowardTargetOpposite(Actor targetActor)
		{
			return Quaternion.LookRotation(GetForwardVectorTowardTargetOpposite(targetActor), Transform.up);
		}

		public Quaternion GetQuaternionTowardTargetOpposite(Vector3 targetPosition)
		{
			return Quaternion.LookRotation(GetForwardVectorTowardTargetOpposite(targetPosition), Transform.up);
		}

		[SerializeField]
		protected bool _IsInitialized = false;
		
		[SerializeField]
		private int _ID = 0;
		public int ID {	protected set { _ID = value; } get { return _ID; } }
		public string Name { protected set;	get; }
		public float Sight { protected set;	get; }
		public bool IsAlive	{ set; get; }
		public float Radius	{ protected set; get; }
		public float Height { protected set; get; }
		protected NavMeshAgent _NavMeshAgent;
		public NavMeshAgent NavMeshAgent
		{
			get { return _NavMeshAgent; }
		}

		protected NavMeshObstacle _NavMeshObstacle;
		public NavMeshObstacle NavMeshObstacle
		{
			get { return _NavMeshObstacle; }
		}
		
		protected void InitNavigation()
		{
			_NavMeshAgent = GetComponent<NavMeshAgent>();
			_NavMeshObstacle = GetComponent<NavMeshObstacle>();
		}

		public void Warp(Vector3 position)
		{
			if(_NavMeshAgent != null)
				_NavMeshAgent.Warp(position);
			else
				Position = position;
		}

		protected void InitWidthHeight()
		{
			if(_NavMeshAgent != null)
			{
				Radius = _NavMeshAgent.radius;
				Height = _NavMeshAgent.height;
			}

			if(_NavMeshObstacle != null)
			{
				Radius = _NavMeshObstacle.radius;
				Height = _NavMeshObstacle.height * 2.0f;
			}
		}

		protected virtual void Awake()
		{
			Transform = transform;
				
			InitNavigation();
			InitWidthHeight();

			IsAlive = false;
		}

		protected virtual void OnEnable()
		{

		}

		protected virtual void OnDisable()
		{

		}

		public virtual void Init(int id)
		{
			ID = id;

			_IsInitialized = true;

			Start();
		}

		protected virtual void Start()
		{
			if(!_IsInitialized)
				return;

			IsAlive = true;

			//if(!(this is World))
			//	SceneGamePlay.Instance.World.RegisterActor(this);
		}

		protected virtual void Update()
		{
			if(SceneGamePlay.Instance == null || SceneGamePlay.Instance.IsInitialized == false)
				return;

			if(!_IsInitialized)
				return;
		}

		protected virtual void OnDestroy()
		{
			//if(SceneGamePlay.Instance != null && SceneGamePlay.Instance.World != null && !(this is World))
			//	SceneGamePlay.Instance.World.UnregisterActor(this);

			IsAlive = false;
		}
	}
}