using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TechArtProject
{
    [RequireComponent(typeof(TMP_Text))]
    public class TypingTextEffect : MonoBehaviour
    {
        [Header("Timing")] [Tooltip("Total time for the whole text (or each line) to fully reveal.")] [SerializeField]
        private float duration = 1.2f;

        [Header("Gradient Band")]
        [Tooltip("How many characters are fading at the same time (width of the gradient).")]
        [SerializeField]
        private int bandChars = 6;

        [Tooltip("Remaps fade within the band. X=0..1 band progress, Y=alpha 0..1.")] [SerializeField]
        private AnimationCurve fadeCurve =
            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Behavior")] [Tooltip("If true, each line reveals from its start independently.")] [SerializeField]
        private bool perLine = false;

        private TMP_Text tmp;
        private float timer;
        private bool playing;

        void Awake()
        {
            tmp = GetComponent<TMP_Text>();
        }

        void OnEnable()
        {
            // Rebuild geometry and hide everything
            tmp.ForceMeshUpdate();
            HideAll();
            timer = 0f;
            playing = true;
        }

        void OnDisable()
        {
            playing = false;
        }

        void Update()
        {
            if (!playing) return;

            timer += Time.deltaTime;
            float p = Mathf.Clamp01(timer / Mathf.Max(0.0001f, duration));

            if (perLine)
                ApplyPerLine(p);
            else
                ApplyWhole(p);

            if (p >= 1f) playing = false;
        }

        // -------- Core algorithms --------

        // Single sweep across the whole visible string
        void ApplyWhole(float progress)
        {
            tmp.ForceMeshUpdate();

            int visCount = tmp.textInfo.characterCount;
            if (visCount == 0) return;

            // Head position moves from -band .. visCount (to give leading/ trailing room)
            float head = progress * (visCount + bandChars);

            for (int i = 0; i < visCount; i++)
            {
                var ch = tmp.textInfo.characterInfo[i];
                if (!ch.isVisible) continue;

                // Distance from the head (0 when head reaches this char)
                float t = (head - i) / Mathf.Max(1, bandChars);
                // Map: t<=0 => 0 alpha, t>=1 => 1 alpha, smooth within band (0..1)
                float a = Mathf.Clamp01(fadeCurve.Evaluate(Mathf.Clamp01(t)));

                SetCharAlpha(i, a);
            }

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        // Each line reveals independently left -> right
        void ApplyPerLine(float progress)
        {
            tmp.ForceMeshUpdate();

            int lineCount = tmp.textInfo.lineCount;
            if (lineCount == 0) return;

            for (int line = 0; line < lineCount; line++)
            {
                var lineInfo = tmp.textInfo.lineInfo[line];
                int start = lineInfo.firstCharacterIndex;
                int end = lineInfo.lastCharacterIndex;

                // Count visible chars in this line
                int visInLine = 0;
                for (int i = start; i <= end; i++)
                    if (tmp.textInfo.characterInfo[i].isVisible)
                        visInLine++;

                if (visInLine == 0) continue;

                float head = progress * (visInLine + bandChars);

                // Walk again applying fade using visible order
                int vIndex = 0;
                for (int i = start; i <= end; i++)
                {
                    var ch = tmp.textInfo.characterInfo[i];
                    if (!ch.isVisible) continue;

                    float t = (head - vIndex) / Mathf.Max(1, bandChars);
                    float a = Mathf.Clamp01(fadeCurve.Evaluate(Mathf.Clamp01(t)));
                    SetCharAlpha(i, a);

                    vIndex++;
                }
            }

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        // -------- Utilities --------

        void HideAll()
        {
            tmp.ForceMeshUpdate();
            int count = tmp.textInfo.characterCount;
            for (int i = 0; i < count; i++)
            {
                var ch = tmp.textInfo.characterInfo[i];
                if (!ch.isVisible) continue;
                SetCharAlpha(i, 0f);
            }

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        void SetCharAlpha(int index, float a01)
        {
            var ch = tmp.textInfo.characterInfo[index];
            int m = ch.materialReferenceIndex;
            int v = ch.vertexIndex;

            var colors = tmp.textInfo.meshInfo[m].colors32;
            byte a = (byte)Mathf.RoundToInt(a01 * 255f);

            colors[v + 0].a = a;
            colors[v + 1].a = a;
            colors[v + 2].a = a;
            colors[v + 3].a = a;
        }

        // Public API if you want to retrigger from code
        public void Restart()
        {
            OnEnable();
        }
    }
}