using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.MagicLeap;

namespace MLTK.Image
{
    public class ImageTrackHandler : MonoBehaviour
    {
        [SerializeField] private ImageTrackBehaviour imageTrackBehaviour;
        public UnityEvent<MLImageTracker.Target, MLImageTracker.Target.Result> OnStartTrack;
        public UnityEvent<MLImageTracker.Target, MLImageTracker.Target.Result> OnEndTrack;
        public UnityEvent<MLImageTracker.Target, MLImageTracker.Target.Result> UpdateTrack;

        private void Start()
        {
            if (imageTrackBehaviour == null)
            {
                enabled = false;
                return;
            }

            OnStartTrack.AddListener((target, result) =>
            {
                Debug.Log($"{target.TargetSettings.Name} {result.Status}");
            });
            OnEndTrack.AddListener((target, result) =>
            {
                Debug.Log($"{target.TargetSettings.Name} {result.Status}");
            });
            UpdateTrack.AddListener((target, result) =>
            {
                Debug.Log($"{target.TargetSettings.Name} {result.Status}");
            });

            imageTrackBehaviour.OnTargetFound += OnStartTrack.Invoke;
            imageTrackBehaviour.OnTargetLost += OnEndTrack.Invoke;
            imageTrackBehaviour.OnTargetUpdated += UpdateTrack.Invoke;
        }
    }
}
