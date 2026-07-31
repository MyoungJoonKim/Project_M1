using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using BlackMassSoftware.FloatingTextEngine.Lite.Behaviors;
using BlackMassSoftware.FloatingTextEngine.Lite.Utilities;

namespace BlackMassSoftware.FloatingTextEngine.Lite
{
    /// <summary>
    /// The main engine class for the Floating Text library. Call this to create floating text objects.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class FloatingTextEngine : MonoBehaviour
    {
        // the engine instance
        /// <summary>
        /// The engine instance
        /// </summary>
        private static FloatingTextEngine instance;
        // the object pool
        /// <summary>
        /// The floating text pool
        /// </summary>
        public ObjectPool<GameObject> floatingTextPool;

        /// <summary>
        /// The damage text object prefab
        /// </summary>
        [Header("Floating Text Information")]
        [SerializeField] private GameObject FloatingTextObjectPrefab;
        /// <summary>
        /// The default color when using the bool override of CreateFloatingTextAt
        /// </summary>
        [Space]
        [Header("Floating Damage Text Colors")]
        [SerializeField] private Color defaultTextColor = Color.white;
        /// <summary>
        /// The crit color when using the bool override of CreateFloatingTextAt
        /// </summary>
        [SerializeField] private Color critTextColor = Color.yellow;
        /// <summary>
        /// The floating text font size
        /// </summary>
        [SerializeField] private int floatingTextFontSize = 6;
        [Space]
        [Header("X axis variance values")]
        [SerializeField]private bool xSpatialVariance = true;
        /// <summary>
        /// This value determines the variance on the x axis that the floating text spawns at
        /// </summary>
        [SerializeField] private float xSpatialVarianceValue = 0.3f;

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            instance = this;
            // setup our methods for creation
            instance.floatingTextPool = new ObjectPool<GameObject>(OnCreateFloatingText, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
            instance.InitSpatialVariance(instance.xSpatialVariance);
        }

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

        }

        #region Floating Text Creation Methods

        /// <summary>
        /// Creates the floating text at.
        /// </summary>
        /// <param name="position">The position of the floating text.</param>
        /// <param name="damageValue">The damage value to show.</param>
        /// <param name="isCriticalDamage">if set to <c>true</c> [is critical damage].</param>
        /// <returns></returns>
        public static FloatingBehaviorEngine CreateFloatingTextAt(Vector3 position, int damageValue, bool isCriticalDamage = false)
        {
            GameObject newFloatingText = instance.floatingTextPool.Get();
            instance.CheckAndApplySpatialVariance(ref newFloatingText, position);
            instance.CheckAndApplyCriticalColor(ref newFloatingText, isCriticalDamage);
            
            newFloatingText.GetComponent<TextMeshPro>().text = damageValue.ToString();
            newFloatingText.GetComponent<TextMeshPro>().fontSize = instance.floatingTextFontSize;

            newFloatingText.TryGetComponent(out FloatingBehaviorEngine behaviorEngine);
            return behaviorEngine;
        }

        /// <summary>
        /// Creates the floating text at.
        /// </summary>
        /// <param name="position">The position of the floating text.</param>
        /// <param name="damageValue">The damage value to show.</param>
        /// <param name="isCriticalDamage">if set to <c>true</c> [is critical damage].</param>
        /// <returns></returns>
        public static FloatingBehaviorEngine CreateFloatingTextAt(Vector3 position, float damageValue, bool isCriticalDamage = false)
        {
            GameObject newFloatingText = instance.floatingTextPool.Get();
            instance.CheckAndApplySpatialVariance(ref newFloatingText, position);
            instance.CheckAndApplyCriticalColor(ref newFloatingText, isCriticalDamage);

            newFloatingText.GetComponent<TextMeshPro>().text = damageValue.ToString();
            newFloatingText.GetComponent<TextMeshPro>().fontSize = instance.floatingTextFontSize;

            newFloatingText.TryGetComponent(out FloatingBehaviorEngine behaviorEngine);
            return behaviorEngine;
        }

        /// <summary>
        /// Creates the floating text at.
        /// </summary>
        /// <param name="position">The position of the floating text.</param>
        /// <param name="damageValue">The damage value to show.</param>
        /// <param name="color">The color of the text.</param>
        /// <returns></returns>
        public static FloatingBehaviorEngine CreateFloatingTextAt(Vector3 position, int damageValue, Color color)
        {
            GameObject newFloatingText = instance.floatingTextPool.Get();
            instance.CheckAndApplySpatialVariance(ref newFloatingText, position);

            newFloatingText.GetComponent<TextMeshPro>().text = damageValue.ToString();
            newFloatingText.GetComponent<TextMeshPro>().color = color;
            newFloatingText.GetComponent<TextMeshPro>().fontSize = instance.floatingTextFontSize;
            
            newFloatingText.TryGetComponent(out FloatingBehaviorEngine behaviorEngine);
            return behaviorEngine;
        }

        /// <summary>
        /// Creates the floating text at.
        /// </summary>
        /// <param name="position">The position of the floating text.</param>
        /// <param name="damageValue">The damage value to show.</param>
        /// <param name="color">The color of the text.</param>
        /// <returns></returns>
        public static FloatingBehaviorEngine CreateFloatingTextAt(Vector3 position, float damageValue, Color color)
        {
            GameObject newFloatingText = instance.floatingTextPool.Get();
            instance.CheckAndApplySpatialVariance(ref newFloatingText, position);

            newFloatingText.GetComponent<TextMeshPro>().text = damageValue.ToString();
            newFloatingText.GetComponent<TextMeshPro>().color = color;
            newFloatingText.GetComponent<TextMeshPro>().fontSize = instance.floatingTextFontSize;

            newFloatingText.TryGetComponent(out FloatingBehaviorEngine behaviorEngine);
            return behaviorEngine;
        }

        // RGB versions
        /// <summary>
        /// Creates the floating text at.
        /// </summary>
        /// <param name="position">The position of the floating text.</param>
        /// <param name="damageValue">The damage value to show.</param>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        /// <returns></returns>
        public static FloatingBehaviorEngine CreateFloatingTextAt(Vector3 position, int damageValue, float r, float g, float b)
        {
            GameObject newFloatingText = instance.floatingTextPool.Get();
            instance.CheckAndApplySpatialVariance(ref newFloatingText, position);

            newFloatingText.GetComponent<TextMeshPro>().text = damageValue.ToString();
            newFloatingText.GetComponent<TextMeshPro>().color = FloatingTextUtils.ConvertRGBToColor(r, g, b);
            newFloatingText.GetComponent<TextMeshPro>().fontSize = instance.floatingTextFontSize;

            newFloatingText.TryGetComponent(out FloatingBehaviorEngine behaviorEngine);
            return behaviorEngine;
        }

        /// <summary>
        /// Creates the floating text at.
        /// </summary>
        /// <param name="position">The position of the floating text.</param>
        /// <param name="damageValue">The damage value to show.</param>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        /// <returns></returns>
        public static FloatingBehaviorEngine CreateFloatingTextAt(Vector3 position, float damageValue, float r, float g, float b)
        {
            GameObject newFloatingText = instance.floatingTextPool.Get();
            instance.CheckAndApplySpatialVariance(ref newFloatingText, position);

            newFloatingText.GetComponent<TextMeshPro>().text = damageValue.ToString();
            newFloatingText.GetComponent<TextMeshPro>().color = FloatingTextUtils.ConvertRGBToColor(r, g, b);
            newFloatingText.GetComponent<TextMeshPro>().fontSize = instance.floatingTextFontSize;

            newFloatingText.TryGetComponent(out FloatingBehaviorEngine behaviorEngine);
            return behaviorEngine;
        }

        // String floating text methods
        /// <summary>
        /// Creates the floating text at.
        /// </summary>
        /// <param name="position">The position of the floating text.</param>
        /// <param name="text">The text to show.</param>
        /// <param name="color">The color of the text.</param>
        /// <returns></returns>
        public static FloatingBehaviorEngine CreateFloatingTextAt(Vector3 position, string text, Color color)
        {
            GameObject newFloatingText = instance.floatingTextPool.Get();
            instance.CheckAndApplySpatialVariance(ref newFloatingText, position);

            newFloatingText.GetComponent<TextMeshPro>().text = text;
            newFloatingText.GetComponent<TextMeshPro>().color = color;
            newFloatingText.GetComponent<TextMeshPro>().fontSize = instance.floatingTextFontSize;

            newFloatingText.TryGetComponent(out FloatingBehaviorEngine behaviorEngine);
            return behaviorEngine;
        }

        /// <summary>
        /// Creates the floating text at.
        /// </summary>
        /// <param name="position">The position of the floating text.</param>
        /// <param name="text">The text to show.</param>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        /// <returns></returns>
        public static FloatingBehaviorEngine CreateFloatingTextAt(Vector3 position, string text, float r, float g, float b)
        {
            GameObject newFloatingText = instance.floatingTextPool.Get();
            instance.CheckAndApplySpatialVariance(ref newFloatingText, position);

            newFloatingText.GetComponent<TextMeshPro>().text = text;
            newFloatingText.GetComponent<TextMeshPro>().color = FloatingTextUtils.ConvertRGBToColor(r, g, b);
            newFloatingText.GetComponent<TextMeshPro>().fontSize = instance.floatingTextFontSize;

            newFloatingText.TryGetComponent(out FloatingBehaviorEngine behaviorEngine);
            return behaviorEngine;
        }

        #endregion


        #region Floating Text Pooling Methods

        /// <summary>
        /// Called when [create floating text].
        /// </summary>
        /// <returns></returns>
        private GameObject OnCreateFloatingText()
        {
            GameObject newTextObject = Instantiate(instance.FloatingTextObjectPrefab, gameObject.transform);
            return newTextObject;
        }

        // invoked when retrieving the next item from the object pool
        /// <summary>
        /// Called when [get from pool].
        /// </summary>
        /// <param name="pooledObject">The pooled object.</param>
        private void OnGetFromPool(GameObject pooledObject)
        {
            pooledObject.SetActive(true);

        }

        // invoked when returning an item to the object pool
        /// <summary>
        /// Called when [release to pool].
        /// </summary>
        /// <param name="pooledObject">The pooled object.</param>
        private void OnReleaseToPool(GameObject pooledObject)
        {
            pooledObject.transform.localScale = new Vector3(1f, 1f, 1f);
            pooledObject.transform.rotation = Quaternion.identity;
            pooledObject.SetActive(false);
        }

        // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
        /// <summary>
        /// Called when [destroy pooled object].
        /// </summary>
        /// <param name="pooledObject">The pooled object.</param>
        private void OnDestroyPooledObject(GameObject pooledObject)
        {
            Destroy(pooledObject);
        }

        public static void ReleaseObjectToPool(GameObject pooledObject)
        {
            instance.floatingTextPool.Release(pooledObject);
        }

        #endregion

        /// <summary>
        /// Internal call to enable/disable the xSpatialVariance
        /// </summary>
        /// <param name="enabled"></param>
        internal void InitSpatialVariance(bool enabled)
        {
            instance.xSpatialVariance = enabled;
        }

        /// <summary>
        /// Enables the spatial x variance.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public static void EnableSpatialXVariance(bool enabled)
        {
            instance.InitSpatialVariance(enabled);
        }

        /// <summary>
        /// Checks if spatial variance is active and alters the x axis of the position.
        /// </summary>
        /// <param name="newFloatingText">The new floating text.</param>
        /// <param name="spawnPosition">The spawn position.</param>
        private void CheckAndApplySpatialVariance(ref GameObject newFloatingText, Vector3 spawnPosition)
        {
            if (instance.xSpatialVariance) {
                float randomXInRange = (Random.insideUnitCircle * instance.xSpatialVarianceValue).x;
                newFloatingText.transform.position = new Vector3(spawnPosition.x + randomXInRange, spawnPosition.y, spawnPosition.z);
            }
            else {
                newFloatingText.transform.position = spawnPosition;
            }
        }

        /// <summary>
        /// Checks if the text is a critical strike and set the color.
        /// </summary>
        /// <param name="newFloatingText">The new floating text.</param>
        /// <param name="isCrit">if set to <c>true</c> [is crit].</param>
        private void CheckAndApplyCriticalColor(ref GameObject newFloatingText, bool isCrit)
        {
            if (isCrit) {
                newFloatingText.GetComponent<TextMeshPro>().color = instance.critTextColor;
            }
            else {
                newFloatingText.GetComponent<TextMeshPro>().color = instance.defaultTextColor;
            }
        }
    }
}