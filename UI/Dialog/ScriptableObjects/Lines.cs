using UnityEngine;
using UnityEngine.Events;
using System;

namespace Dialog.ScriptableObjects {
	[System.Serializable]
	public class LineTrigger {
		public string Name;
	}

	[CreateAssetMenu(menuName = "Dialog: Create Line")]
	public class Lines : ScriptableObject {

		public string Line;
		public bool IsAutoClosing = false;
		public float AutoCloseTimeout = 0f;

		[Header("Previous Action")]
		public bool HasPrev = true;
		public string PrevText = "Prev";

		[Header("Next Action")]
		public bool HasNext = true;
		public string NextText = "Next";

		[Header("Alternative Button 1")]
		public bool HasAlternative1 = false;
		public string AlternativeText1 = "";
		public UnityEvent Alternative1TriggerEvents;

		[Header("Alternative Button 2")]
		public bool HasAlternative2 = false;
		public string AlternativeText2 = "";
		public UnityEvent Alternative2TriggerEvents;

		[Header("Override NPC")]
		public bool OverrideNPC = false;
		public string NPCName;
		public Texture2D NPCThumbnail;

		[Header("On Open / Close Trigger Events")]
		public UnityEvent OnOpenTriggerEvents;
		public UnityEvent OnCloseTriggerEvents;

		public event Action OnLineClosed;

		public void CloseLine()
    	{
        	OnLineClosed?.Invoke();
    	}

	}
}