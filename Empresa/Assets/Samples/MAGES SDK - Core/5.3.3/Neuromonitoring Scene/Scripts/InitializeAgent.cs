namespace MAGES.NeuromonitoringScene
{
    using MAGES;
#if MAGES_MICROSOFT_COGNITIVE_SERVICES
#if MAGES_CHARACTER_CONTROLLER
    using MAGES.CharacterController;
#endif
    using MAGES.Experimental.EmbodimentJARIA;
#endif
    using UnityEngine;

    /// <summary>
    /// Step to initialize the embodied agent in neuromonitoring sample scene.
    /// </summary>
    public class InitializeAgent : Step
    {
        private GameObject nonitorScreen;
#if MAGES_MICROSOFT_COGNITIVE_SERVICES
        private AgentConfiguration neuroAgent;
#endif

        /// <summary>
        /// Execution of step.
        /// </summary>
        /// <param name="action">The action state.</param>
        /// <param name="stepEvent">The event payload of the step.</param>
        public override void Execute(BaseActionData action, StepEvent stepEvent)
        {
#if MAGES_MICROSOFT_COGNITIVE_SERVICES
            neuroAgent = Hub.Instance.Get<EmbodimentJARIAModule>().InitializeAgent(0);
            nonitorScreen = GameObject.Find("UI");

            var target = Hub.Instance.Get<DeviceManagerModule>().CameraGameObject;
            Hub.Instance.Get<EmbodimentJARIAModule>().UserSubsComponent.GetComponent<LookAtObject>().TargetObject(target.transform);
#if MAGES_CHARACTER_CONTROLLER
            MAGESCharacterController.Instances["Embodiment Pharmacist"].MAGESHeadController.LookAt(target, 0);
#endif

            neuroAgent.OnStartTalking.AddListener(OnStartTalking);
            neuroAgent.OnEndTalking.AddListener(OnEndTalking);
            neuroAgent.OnInteract.AddListener(Interact);
            neuroAgent.OnCancel.AddListener(Cancel);
#endif
        }

#if MAGES_MICROSOFT_COGNITIVE_SERVICES
        private void OnStartTalking()
        {
#if MAGES_CHARACTER_CONTROLLER
            MAGESCharacterController.Instances["Embodiment Pharmacist"].MAGESAnimator.PlayAnimation("Talking");
#endif
        }

        private void OnEndTalking()
        {
#if MAGES_CHARACTER_CONTROLLER
            MAGESCharacterController.Instances["Embodiment Pharmacist"].MAGESAnimator.StopAllOnPlayAnimations();
#endif
        }

        private void Interact()
        {
            neuroAgent.AgentOutput.SetActive(true);
            foreach (Transform child in nonitorScreen.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        private void Cancel()
        {
            neuroAgent.AgentOutput.SetActive(false);
            foreach (Transform child in nonitorScreen.transform)
            {
                child.gameObject.SetActive(true);
            }
#if MAGES_CHARACTER_CONTROLLER
            MAGESCharacterController.Instances["Embodiment Pharmacist"].MAGESAnimator.StopAllOnPlayAnimations();
#endif
        }
#endif
        }
    }