using Dialog.ScriptableObjects;
using UnityEngine;

namespace Dialog {

	public abstract class BaseNPCBehaviour : MonoBehaviour {

		public Texture2D Thumbnail;
		public string Name;
		public float MinDistance = 5;
		public Dialogs[] dialogs;

		protected Dialogs currentDialog;
		protected Transform invokerTransform;
		protected bool DialogIsOpen = false;

		public virtual void Awake() {
			SetDialog(0);
		}

		public virtual void Update() {
			if (DialogIsOpen && invokerTransform != null) {
				if ((invokerTransform.position - transform.position).magnitude > MinDistance) {
					CloseDialog();
				}
			}
		}

		public virtual void SetDialog(int index) {
			if (dialogs.Length - 1 < index) {
				Debug.LogError("Invalid Dialog Index: " + index);
				return;
			}
			currentDialog = dialogs[index];
		}

		public virtual void ShowLine(int index) {
			if (currentDialog.Lines.Length - 1 < index) {
				Debug.LogError("Invalid Line Index: " + index);
				return;
			}
			OpenDialog(invokerTransform, index);
		}

		public void OpenDialog(Transform invokerTransform) {
			if (dialogs.Length > 0) {
				DialogIsOpen = true;
				OpenDialog(invokerTransform, 0);
			} else {
				Debug.LogError("No dialogs attached to: " + this.name);
			}
		}

		public void OpenDialog(Transform invokerTransform, int lineIndex) {
			if (currentDialog.Lines.Length > lineIndex) {
				OpenDialog(invokerTransform, lineIndex, currentDialog);
			} else {
				Debug.LogError("Invalid line index: " + lineIndex + " for dialog " + currentDialog.name);
			}
		}

		public virtual void OpenDialog(Transform invokerTransform, int lineIndex, Dialogs dialog) {
			this.invokerTransform = invokerTransform;
			BaseDialogManager.Instance.OpenDialog(this, dialog, lineIndex);
		}

		public virtual void CloseDialog() {
			DialogIsOpen = false;
			BaseDialogManager.Instance.HideDialog();
		}
	}
}