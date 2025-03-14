namespace MAGES.ExampleScene
{
    using System;
    using MAGES.SceneGraph;
    using UnityEngine;

    /// <summary>
    /// Class to handle the UI for the ChoiceActionData.
    /// </summary>
    public class ChoiceActionUI : MonoBehaviour, ChoiceActionData.IDecisionHandler
    {
        private Action<int> callbackAction;

        /// <summary>
        /// Method to set the callback handle for the decision.
        /// </summary>
        /// <param name="completeAction">The given Action.</param>
        public void SetCallbackHandle(Action<int> completeAction)
        {
            callbackAction = completeAction;
        }

        /// <summary>
        /// Slects the choice for the ChoiceActionData.
        /// </summary>
        /// <param name="choice">The id of the path.</param>
        public void SelectChoice(int choice)
        {
            callbackAction.Invoke(choice);
        }
    }
}