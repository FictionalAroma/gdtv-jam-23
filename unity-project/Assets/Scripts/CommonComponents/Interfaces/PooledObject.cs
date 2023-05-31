using UnityEngine;

namespace CommonComponents.Interfaces
{
	public abstract class PooledObject : MonoBehaviour
	{
		public IObjectCachePool Pool { get; private set; }
		public void SetPool(IObjectCachePool pool) => Pool = pool;

		public void ReturnToPool() => Pool?.ReturnToPool(this);

		public void SetStartPoint(Vector3 pos) => transform.position = pos;
	}
}