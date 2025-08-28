using UnityEngine;
using UnityEngine.UI;

public class PopupBackgroundBlur : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RawImage blurImage;        // Your BlurBackground RawImage
    [SerializeField] private Material blurMaterial;     // Material with the Blur shader
    [SerializeField] private Camera captureCamera;      // Main Camera (used by Canvas in Screen Space - Camera mode)

    private RenderTexture rt;

    [ContextMenu("ApplyBlur")]
    public void ApplyBlur()
    {
        if (rt != null) rt.Release();

        // Use the camera's true pixel size (stable in Editor & builds)
        int w = captureCamera.pixelWidth;
        int h = captureCamera.pixelHeight;

        rt = new RenderTexture(w, h, 16, RenderTextureFormat.ARGB32);
        rt.Create();

        captureCamera.targetTexture = rt;
        captureCamera.Render();
        captureCamera.targetTexture = null;

        blurImage.texture = rt;
        blurImage.material = blurMaterial;

        // RawImage stretches [0..1] to the rect; since aspect matches camera, no UV fix needed:
        blurImage.uvRect = new Rect(0, 0, 1, 1);
    }

    private void CorrectAspect(RawImage img, int texWidth, int texHeight)
    {
        float texAspect = (float)texWidth / texHeight;
        float screenAspect = (float)Screen.width / Screen.height;

        if (texAspect > screenAspect)
        {
            // Texture is wider than screen: fit width, crop top/bottom
            float scale = screenAspect / texAspect;
            img.uvRect = new Rect(0f, (1f - scale) / 2f, 1f, scale);
        }
        else
        {
            // Texture is taller than screen: fit height, crop left/right
            float scale = texAspect / screenAspect;
            img.uvRect = new Rect((1f - scale) / 2f, 0f, scale, 1f);
        }
    }

    private void OnDisable()
    {
        if (rt != null) rt.Release();
    }
}