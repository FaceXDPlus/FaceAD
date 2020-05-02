using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using System.Collections.Generic;

namespace FaceAD
{
    /// <summary>
    /// </summary>
    public class FaceAD : MonoBehaviour
    {
        public Text exampleTitle;
        public Text versionInfo;
        public ScrollRect scrollRect;
        static float verticalNormalizedPosition = 1f;

        public enum DlibShapePredictorNamePreset : int
        {
            sp_human_face_68,
            sp_human_face_68_for_mobile
        }

        public Dropdown dlibShapePredictorNameDropdown;

        static DlibShapePredictorNamePreset dlibShapePredictorName = DlibShapePredictorNamePreset.sp_human_face_68;

        public static string dlibShapePredictorFileName {
            get {
                return dlibShapePredictorName.ToString () + ".dat";
            }
        }

        // Use this for initialization
        void Start ()
        {
            scrollRect.verticalNormalizedPosition = verticalNormalizedPosition;

            dlibShapePredictorNameDropdown.value = (int)dlibShapePredictorName;
        }

        // Update is called once per frame
        void Update ()
        {

        }

        public void OnScrollRectValueChanged ()
        {
            verticalNormalizedPosition = scrollRect.verticalNormalizedPosition;
        }

    }
}