using UnityEngine;

namespace Behaviors
{
	public class LookAt : MonoBehaviour
	{
		[SerializeField] protected Transform lookTarget;
		private void Update() => this.transform.rotation = Quaternion.LookRotation(this.transform.position - lookTarget.position); 
	}
}
