using UnityEngine;

namespace Behaviors
{
	public class LookAtCamera : MonoBehaviour
	{
		private Transform _camera;
		private void Awake() =>  _camera = Camera.main.transform;

		private void Update()
		{
			var q =  Quaternion.LookRotation(this.transform.position - _camera.position);
			q.z = 0;
			this.transform.rotation = q;
		}
	}
}
