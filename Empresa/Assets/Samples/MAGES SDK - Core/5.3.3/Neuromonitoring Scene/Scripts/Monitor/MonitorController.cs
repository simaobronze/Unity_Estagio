namespace MAGES.NeuromonitoringScene
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controlls the monitor UI.
    /// </summary>
    public class MonitorController : MonoBehaviour
    {
        private static MonitorController instance = null;
        private TextMeshProUGUI currentActionNameText;
        private TextMeshProUGUI actionsCompletedText;
        private TextMeshProUGUI toolsText;
        private Slider actionsSlider;
        private int totalActions;
        private int completedActions;
        private bool isUndone;

        /// <summary>
        /// Gets the instance of the monitor controller.
        /// </summary>
        public static MonitorController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MonitorController>();
                }

                return instance;
            }
        }

        /// <summary>
        /// Sets the current action name.
        /// </summary>
        /// <param name="actionName">The name.</param>
        public void NextAction(string actionName)
        {
            currentActionNameText.text = actionName;
        }

        /// <summary>
        /// Sets the tools text.
        /// </summary>
        /// <param name="tools">The text.</param>
        public void SetTools(string tools)
        {
            toolsText.text = tools;
        }

        /// <summary>
        /// Sets the completion counter.
        /// </summary>
        /// <param name="completed">The completed actions.</param>
        /// <param name="total">The number of total actions.</param>
        public void SetCompletionCounter(int completed, int total)
        {
            actionsCompletedText.text = $"{completed}/{total}";
        }

        private void Awake()
        {
            currentActionNameText = transform.Find("Current Action Name").GetComponent<TextMeshProUGUI>();
            toolsText = transform.Find("Tools").GetComponent<TextMeshProUGUI>();
            actionsSlider = transform.Find("Action Slider/Slider").GetComponent<Slider>();
            actionsCompletedText = transform.Find("Action Slider/Actions").GetComponent<TextMeshProUGUI>();
            instance = this;
        }

        private void Start()
        {
            totalActions = 0;
            Hub.Instance.Get<SceneGraphModule>().ForEachAction((x) => totalActions++);
            totalActions--;
            isUndone = false;
            Hub.Instance.Get<SceneGraphModule>().ActionPerformed(OnActionPerformed);
            Hub.Instance.Get<SceneGraphModule>().ActionUndone(OnActionUndone);
        }

        private void OnActionPerformed(BaseActionData data, bool skipped)
        {
            completedActions += 1;
            UpdateActionSlider();
        }

        private void OnActionUndone(BaseActionData data)
        {
            isUndone = !isUndone;
            if (isUndone)
            {
                completedActions -= 1;
                UpdateActionSlider();
            }
        }

        private void UpdateActionSlider()
        {
            actionsSlider.value = (float)completedActions / (float)totalActions;
        }

        /// <summary>
        /// Step to set the monitor controller next action.
        /// </summary>
        [StepDetails("Set Monitor Controller Next Action")]
        [System.Serializable]
        public class SetMonitorControllerNextActionStep : Step
        {
            [SerializeField]
            private string actionName;

            /// <summary>
            /// Executes the step.
            /// </summary>
            /// <param name="action">The action.</param>
            /// <param name="stepEvent">The event step.</param>
            public override void Execute(BaseActionData action, StepEvent stepEvent)
            {
                MonitorController.Instance.NextAction(actionName);
            }
        }

        /// <summary>
        /// Step to set the monitor controller tools.
        /// </summary>
        [StepDetails("Set Monitor Controller Tools")]
        [System.Serializable]
        public class SetMonitorControllerToolsStep : Step
        {
            [SerializeField]
            private string toolsText;

            /// <summary>
            /// Executes the step.
            /// </summary>
            /// <param name="action">The action.</param>
            /// <param name="stepEvent">The event step.</param>
            public override void Execute(BaseActionData action, StepEvent stepEvent)
            {
                MonitorController.Instance.SetTools(toolsText);
            }
        }

        /// <summary>
        /// Step to set the monitor controller completion count.
        /// </summary>
        [StepDetails("Set Monitor Controller completion count")]
        [System.Serializable]
        public class SetMonitorCompletionCountStep : Step
        {
            [SerializeField]
            private int completed;

            [SerializeField]
            private int total;

            /// <summary>
            /// Executes the step.
            /// </summary>
            /// <param name="action">The action.</param>
            /// <param name="stepEvent">The event step.</param>
            public override void Execute(BaseActionData action, StepEvent stepEvent)
            {
                MonitorController.Instance.SetCompletionCounter(completed, total);
            }
        }
    }
}