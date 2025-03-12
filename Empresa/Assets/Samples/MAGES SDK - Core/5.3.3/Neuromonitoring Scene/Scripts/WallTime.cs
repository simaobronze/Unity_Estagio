namespace MAGES.NeuromonitoringScene
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using TMPro;
    using UnityEngine;
    
    /// <summary>
    /// Displays the elapsed time since the scene started
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class WallTime : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private Stopwatch stopwatch;

        private void Start()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            text = GetComponent<TextMeshProUGUI>();
            StartCoroutine(UpdateTime());
        }

        private IEnumerator UpdateTime()
        {
            while (true)
            {
                TimeSpan elapsedTime = stopwatch.Elapsed;
                text.text = $"{(int)elapsedTime.TotalMinutes:D2}:{elapsedTime.Seconds:D2}";
                yield return new WaitForSeconds(1);
            }
        }
    }
}