using System;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MLTK.Image
{
    public class ImageTrackBehaviour : MonoBehaviour
    {
        [Serializable]
        public class ImageArgs
        {
            /// <summary>
            /// Image that needs to be tracked.
            /// Do not resize the image, the aspect ratio of the image provided here
            /// and the printed image should be the same. Set the "Non Power of 2"
            /// property of Texture2D to none.
            /// </summary>
            [Tooltip(
                "Texture2D  of image that needs to be tracked. Do not change the aspect ratio of the image, it should be the same as the printed image. Set the \"Non Power of 2\" property of Texture2D to \"none\".")]
            public Texture2D Image;

            /// <summary>
            /// Longer dimension of the printed image target in scene units.
            /// If width is greater than height, it is the width, height otherwise.
            /// </summary>
            [Tooltip("Longer dimension of the printed image target in scene units. If width is greater than height, it is the width, height otherwise.")]
            public float LongerDimensionInSceneUnits;

            /// <summary>
            /// Holds reference to the image target inside MLImageTracker.Target.
            /// </summary>
            public MLImageTracker.Target ImageTarget = null;
        }

        [SerializeField] private ImageArgs[] imageArgs;

        /// <summary>
        /// Cached tracking status.
        /// </summary>
        private MLImageTracker.Target.TrackingStatus status = MLImageTracker.Target.TrackingStatus.NotTracked;

        /// <summary>
        /// Set this to true if the position of this image target in the physical
        /// world is fixed and its surroundings are planar (ex: walls, floors, tables, etc).
        /// </summary>
        [Tooltip(
            "Set this to true if the position of this image target in the physical world is fixed and its surroundings are planar (ex: walls, floors, tables, etc).")]
        public bool IsStationary;

        /// <summary>
        /// Set this to true if the behavior should automatically move the attached game object.
        /// </summary>
        [Tooltip("Set this to true if the behavior should automatically move the attached game object.")]
        public bool AutoUpdate;


#if PLATFORM_LUMIN
        /// <summary>
        /// Delegate for status updates.
        /// </summary>
        public delegate void StatusUpdate(MLImageTracker.Target target, MLImageTracker.Target.Result result);

        /// <summary>
        /// Occurs when an existing image target is found.
        /// The this.status of the MLImageTracker.Target.Result will indicate if tracking is unreliable.
        /// </summary>
        public event StatusUpdate OnTargetFound = delegate { };

        /// <summary>
        /// Occurs when the image target is lost.
        /// </summary>
        public event StatusUpdate OnTargetLost = delegate { };

        /// <summary>
        /// Occurs when the result gets updated for the image target and happens once every frame.
        /// </summary>
        public event StatusUpdate OnTargetUpdated = delegate { };
#endif

        /// <summary>
        /// Starts the image tracker and adds the image target to the tracking system.
        /// </summary>
        void Start()
        {
            AddTarget();
        }

        /// <summary>
        /// Removes the image target from the tracking system and then stops the starter kit.
        /// </summary>
        void OnDestroy()
        {
#if PLATFORM_LUMIN
            MLImageTracker.RemoveTarget(gameObject.GetInstanceID().ToString());
#endif
        }

        /// <summary>
        /// Adds a new image target to be tracked.
        /// </summary>
        private void AddTarget()
        {
#if PLATFORM_LUMIN
            foreach (var imageArg in imageArgs)
            {
                imageArg.ImageTarget = MLImageTracker.AddTarget(imageArg.Image.GetHashCode().ToString(), imageArg.Image,
                    imageArg.LongerDimensionInSceneUnits, HandleAllTargetStatuses, IsStationary);

                if (imageArg.ImageTarget != null)
                {
                    continue;
                }
                Debug.LogErrorFormat("ImageTrackBehavior.AddTarget failed to add target {0} to the image tracker.",
                    imageArg.Image.name);
                return;
            }
#endif
        }


#if PLATFORM_LUMIN
        /// <summary>
        /// Handles all the image target's this.status updates. This is called every frame.
        /// </summary>
        private void HandleAllTargetStatuses(MLImageTracker.Target target, MLImageTracker.Target.Result result)
        {
            if (result.Status != this.status)
            {
                this.status = result.Status;

                if (this.status == MLImageTracker.Target.TrackingStatus.Tracked ||
                    this.status == MLImageTracker.Target.TrackingStatus.Unreliable)
                {
                    OnTargetFound(target, result);
                }

                else
                {
                    OnTargetLost(target, result);
                }
            }
            else
            {
                OnTargetUpdated(target, result);
            }

            if (AutoUpdate)
            {
                transform.position = result.Position;
                transform.rotation = result.Rotation;
            }
        }
#endif
    }
}
