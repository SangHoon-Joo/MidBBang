using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ProjectWilson.Lookups;
using ProjectWilson.Actions;

namespace ProjectWilson
{
	public class UIGamePlayMainJoystick : MonoBehaviour
	{
		private NetworkPlayer _Player;
		public void Init(NetworkPlayer player)
		{
			if(player == null)
				return;
			
			_Player = player;
			Assert.IsNotNull<NetworkPlayer>(_Player);
		}

		public void OnClickAButton()
		{
			AttackLeft();
		}

		public void OnClickBButton()
		{
			AttackRight();
		}

		private void AttackLeft()
		{
			_Player.CharacterController.TryStartAction(HandlerTypes.Attack, new AttackContext(HandlerTypes.Attack, Side.Left));
		}

		private void AttackRight()
		{
			_Player.CharacterController.TryStartAction(HandlerTypes.Attack, new AttackContext(HandlerTypes.Attack, Side.Right));
		}
	}
}