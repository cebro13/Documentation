using UnityEngine;
using UnityEngine.UI;

namespace Dialog {

    public class AnimatedDialogManager : BaseDialogManager {
        private const string SHOW = "Show";
        private const string HIDE = "Hide";

        private Animator m_animator;
        private CanvasGroup m_canvasGroup;

        public override void Awake() {
            base.Awake();
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_canvasGroup.interactable = false;
            m_canvasGroup.alpha = 0f;
            m_animator.ResetTrigger(SHOW);
            m_animator.ResetTrigger(HIDE);
        }


        protected override void ActivateDialogGameObject()
        {
            base.ActivateDialogGameObject();
            if(ControlInputUI.Instance.GetIsShow())
            {
                ControlInputUI.Instance.SetCanShow(false);
                ControlInputUI.Instance.TriggerTextHide();
            }
            m_animator.SetTrigger(SHOW);
            ThisGameManager.Instance.ToggleGameDialog();
            //SelectButtonDefault();
            m_canvasGroup.interactable = true;
        }

        protected override void DeactivateDialogGameObject()
        {
            base.DeactivateDialogGameObject();
            ControlInputUI.Instance.SetCanShow(true);
            m_animator.SetTrigger(HIDE);
            ThisGameManager.Instance.ToggleGameDialog();
            m_canvasGroup.interactable = false;
        }
    }

}