using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ScratchCard
{
    [RequireComponent(typeof(ScratchCardMask))]
    public class ScratchCardMaskUGUIDissolve : MonoBehaviour
    {
        private ScratchCardMask Mask 
        {
            get 
            {
                if (mask == null)
                {
                    mask = GetComponent<ScratchCardMask>();
                }

                return mask;
            }
        }
        
        private RawImage Image 
        {
            get
            {
                if (image == null)
                {
                    image = GetComponent<RawImage>();
                }

                return image;
            }
        }

        [SerializeField]
        private float duration = 0.5f;
        
        private RawImage image;
        private ScratchCardMask mask;
        private Coroutine dissolveRoutine;
        private bool wasRevealed;

        private void OnEnable()
        {
            Mask.RevealProgressChanged += RevealProgressChangedHandler;
            Image.canvasRenderer.SetAlpha(Mask.IsRevealed ? 0.0f : 1.0f);
        }

        private void OnDisable()
        {
            Mask.RevealProgressChanged -= RevealProgressChangedHandler;
            dissolveRoutine = null;
        }

        private void RevealProgressChangedHandler(float progress, bool isRevealed)
        {
            if (isRevealed != wasRevealed)
            {
                if (isRevealed)
                {
                    dissolveRoutine = StartCoroutine(DissolveRoutine());
                }
                else
                {
                    Image.canvasRenderer.SetAlpha(1.0f);

                    if (dissolveRoutine != null)
                    {
                        StopCoroutine(dissolveRoutine);
                        dissolveRoutine = null;
                    }
                }

                wasRevealed = isRevealed;
            }
        }

        private IEnumerator DissolveRoutine()
        {
            var elapsed = 0.0f;

            while (elapsed < 1.0f)
            {
                elapsed = Mathf.Clamp01(elapsed + Time.deltaTime / Mathf.Max(0.001f, duration));
                Image.canvasRenderer.SetAlpha(1.0f - elapsed);
                yield return null;
            }

            dissolveRoutine = null;
        }
    }
}