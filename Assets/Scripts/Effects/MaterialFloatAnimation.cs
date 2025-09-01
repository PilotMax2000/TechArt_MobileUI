using UnityEngine;
using UnityEngine.UI;

namespace TechArtProject
{
    public class MaterialFloatAnimation : MonoBehaviour
    {
        [Header("Target & Shader Property")] 
        [SerializeField] private Graphic target; // auto-filled if null

        [SerializeField] private string floatName = "_Progress"; // shader float to drive

        [Header("Animate this field in Animator")]
        public float floatValue = 0f; // keyframe this

        [Header("Optional")]
        [Tooltip("If set, this material will be cloned at runtime. If null, the Graphic's current material is used as the base.")]
        [SerializeField]
        private Material baseMaterialOverride;

        private Material baseMat; // original material to restore on destroy
        private Material runtimeMat; // persistent instance
        private int floatId;

        void Awake()
        {
            if (!target) target = GetComponent<Graphic>();
            floatId = Shader.PropertyToID(floatName);

            // Capture the base material once (either override or the current)
            baseMat = baseMaterialOverride ? baseMaterialOverride : target.material;
        }

        void OnEnable()
        {
            EnsureInstance();
            Apply();
        }

        // IMPORTANT: don't destroy the instance on disable — keep it so re-enable is seamless.
        void OnDisable()
        {
            // No-op on purpose.
        }

        void OnDestroy()
        {
            // Restore original material in editor usage so there are no stray instances.
            if (target && baseMat) target.material = baseMat;
            if (runtimeMat) Destroy(runtimeMat);
            runtimeMat = null;
        }

        void Update()
        {
            Apply();
        }

        private void EnsureInstance()
        {
            if (runtimeMat == null)
            {
                var src = baseMat ? baseMat : target.material; // fallback safety
                runtimeMat = new Material(src);
                runtimeMat.name = (src ? src.name : "UI Material") + " (Instance)";
            }

            // Re-assign each enable (some components reset the material on enable)
            if (target.material != runtimeMat)
            {
                target.material = runtimeMat;
                target.SetMaterialDirty();
            }
        }

        private void Apply()
        {
            if (!runtimeMat) return;

            runtimeMat.SetFloat(floatId, floatValue);
            // Tell CanvasRenderer to update this frame
            target.SetMaterialDirty();
        }

        // If you change the base material at runtime and want to rebind:
        public void RebindBaseFromCurrent()
        {
            baseMat = target.material;
            if (runtimeMat)
            {
                Destroy(runtimeMat);
                runtimeMat = null;
            }

            EnsureInstance();
            Apply();
        }
    }
}