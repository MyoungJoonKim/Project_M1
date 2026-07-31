using System.Collections.Generic;
using UnityEngine;

namespace BlackMassSoftware.FloatingTextEngine.Lite.Behaviors
{

    /// <summary>
    /// This class handles all the behaviors each spawned floating text object has. It will update all behaviors
    /// and remove them when finished until no more behaviors are left.  This class also handles the objects return to the object pool.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class FloatingBehaviorEngine : MonoBehaviour
    {
        /// <summary>
        /// The started behaviors
        /// </summary>
        private bool startedBehaviors = false;
        /// <summary>
        /// The behaviors
        /// </summary>
        private List<IFloatingBehavior> behaviors = new List<IFloatingBehavior>();

        // Start is called before the first frame update
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start()
        {

        }

        // Update is called once per frame
        /// <summary>
        /// Updates this instance.
        /// </summary>
        void Update()
        {
            for (int i = 0; i < behaviors.Count; ++i) {
                startedBehaviors = true;
                if (!behaviors[i].IsFinished) {

                    if (behaviors[i].DoWaitForDelay(Time.deltaTime)) {
                        // update the behavior
                        behaviors[i].UpdateBehavior(Time.deltaTime);
                    }
                    // check done
                    if (behaviors[i].WaitForFinish) {
                        break;
                    }
                }
                else {
                    behaviors.RemoveAt(i);
                }
            }

            if (startedBehaviors && behaviors.Count == 0) {
                gameObject.SetActive(false); // return to the object pool
                behaviors.Clear();
                FloatingTextEngine.ReleaseObjectToPool(gameObject);
            }
        }

        /// <summary>
        /// Adds a new behavior.
        /// </summary>
        /// <param name="newBehavior">The new behavior.</param>
        /// <returns></returns>
        public FloatingBehaviorEngine With(IFloatingBehavior newBehavior)
        {
            if (newBehavior != null) {
                newBehavior.InitializeBehavior(gameObject);
                behaviors.Add(newBehavior);
            }
            return this;
        }

        /// <summary>
        /// Adds a new preset from the FloatingPresets class.
        /// </summary>
        /// <param name="preset">The preset.</param>
        /// <returns></returns>
        public FloatingBehaviorEngine WithPreset(List<IFloatingBehavior> preset)
        {
            if (preset != null) {
                foreach (IFloatingBehavior behavior in preset) {
                    behavior.InitializeBehavior(gameObject);
                }
                behaviors.AddRange(preset);
            }
            return this;
        }

    }
}