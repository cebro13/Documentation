using UnityEngine;

namespace Dialog.ScriptableObjects
{
	[CreateAssetMenu(menuName = "Dialog: Create Dialog")]

	public class Dialogs : ScriptableObject
	{
		public bool HasCloseButton = true;
		public Lines[] Lines = new Lines[0];
		public bool HasHotKeys = true;
		public KeyCode PrevHotKey = KeyCode.Alpha1;
		public KeyCode Alt1HotKey = KeyCode.Alpha2;
		public KeyCode Alt2HotKey = KeyCode.Alpha3;
		public KeyCode NextHotKey = KeyCode.Alpha4;
		public KeyCode CloseHotKey = KeyCode.Escape;
	}
}
