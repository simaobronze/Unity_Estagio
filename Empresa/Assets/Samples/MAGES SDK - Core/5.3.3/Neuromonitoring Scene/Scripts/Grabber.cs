namespace MAGES.NeuromonitoringScene
{
    using MAGES;
    using MAGES.Interaction.Interactables;
    using UnityEngine;

    /// <summary>
    /// A component that can grab <see cref="GrabbablePoint"/>s.
    /// </summary>
    public class Grabber : MonoBehaviour
    {
        private MAGES.Interaction.Interactors.ToolInteractor interactor;

        private void Start()
        {
            Grabbable grabbable = GetComponentInParent<Grabbable>();
            grabbable.ActivateEntered.AddListener(OnActivateEntered);
            grabbable.ActivateExited.AddListener(OnActivateExited);
            interactor = GetComponentInParent<MAGES.Interaction.Interactors.ToolInteractor>();
        }

        private void OnActivateExited(ActivateExitInteractionEventArgs arg0)
        {
            interactor.ToggleSelection(false);
        }

        private void OnActivateEntered(ActivateEnterInteractionEventArgs arg0)
        {
            interactor.ToggleSelection(true);
        }
    }
}