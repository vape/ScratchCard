using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScratchCard.Sample
{
    public class SampleSceneControls : MonoBehaviour
    {
        public void OnRestoreClick()
        {
            foreach (var mask in FindObjectsOfType<ScratchCardMaskUGUI>())
            {
                mask.Restore();
            }
        }
    }
}
