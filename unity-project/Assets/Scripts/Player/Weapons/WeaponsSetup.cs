using UnityEngine;

namespace Player.Weapons
{
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponSetup", order = 2)]
	public class WeaponsSetup : ScriptableObject
	{
		public WeaponMode primary;
        public WeaponMode secondary;

	}
}
