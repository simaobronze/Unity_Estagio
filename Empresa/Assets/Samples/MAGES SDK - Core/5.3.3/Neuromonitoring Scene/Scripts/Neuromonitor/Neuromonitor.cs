namespace MAGES.NeuromonitoringScene
{
    using MAGES;
    using UnityEngine;

    /// <summary>
    /// The Neuromonitor class is a singleton that controls the audio source and screen of the neuromonitor.
    /// </summary>
    public class Neuromonitor : MonoBehaviour
    {
        private static Neuromonitor instance;
        private AudioSource source;
        private GameObject screen;

        /// <summary>
        /// Gets the instance of the neuromonitor.
        /// </summary>
        public static Neuromonitor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<Neuromonitor>();
                }

                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
            source = GetComponentInChildren<AudioSource>();
            screen = transform.parent.Find("Screen").gameObject;
            screen.SetActive(false);
        }

        /// <summary>
        /// The SetNeuromonitorPlayingStep class is a step that controls the playing of the neuromonitor.
        /// </summary>
        [System.Serializable]
        public class SetNeuromonitorPlayingStep : Step
        {
            [SerializeField]
            private bool connected = false;

            /// <summary>
            /// Executes the step.
            /// </summary>
            /// <param name="action">The action script.</param>
            /// <param name="stepEvent">The event to execute.</param>
            public override void Execute(BaseActionData action, StepEvent stepEvent)
            {
                if (connected)
                {
                    Neuromonitor.Instance.screen.SetActive(true);
                    Neuromonitor.Instance.source.Play();
                }
                else
                {
                    Neuromonitor.Instance.source.Stop();
                }
            }
        }
    }
}