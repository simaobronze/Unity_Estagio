namespace MAGES.NeuromonitoringScene
{
    using UnityEngine;
    using MAGES.Interaction;
    using MAGES.Interaction.Interactables;

    /// <summary>
    /// Controller for the character in the neuromonitoring scene.
    /// </summary>
    public class NeuromonitoringCharacterController : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Play the animation for opening the skin and fat.
        /// </summary>
        public void PlayOpeningSkinFat()
        {
            animator.SetTrigger("OpeningSkinFat");
        }

        /// <summary>
        /// Play the animation for opening the peritonium.
        /// </summary>
        public void PlayOpeningPeritonium()
        {
            animator.SetTrigger("OpeningPeritonium");
        }

        /// <summary>
        /// Play the animation for pulling the vagal nerve.
        /// </summary>
        public void PlayPullVagalNerve()
        {
            animator.SetTrigger("PullVagalNerve");
            GameObject dissector = GameObject.Find("Dissector(Clone)");
            dissector.GetComponent<InteractableAnimationController>().Animator.SetTrigger("PullNerve");
        }
    }
}