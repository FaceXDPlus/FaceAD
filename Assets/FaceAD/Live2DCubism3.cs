using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using FaceADVtuber;

namespace FaceAD
{
    public class Live2DCubism3 : MonoBehaviour
    {
        /// <summary>
        /// The webcam texture mat source getter.
        /// </summary>
        public WebCamTextureMatSourceGetter webCamTextureMatSourceGetter;

        /// <summary>
        /// The dlib face landmark getter.
        /// </summary>
        public DlibFaceLandmarkGetter dlibFaceLandmarkGetter;

        // Use this for initialization
        void Start ()
        {
            // Load global settings.
            dlibFaceLandmarkGetter.dlibShapePredictorFileName = FaceAD.dlibShapePredictorFileName;
            dlibFaceLandmarkGetter.dlibShapePredictorMobileFileName = FaceAD.dlibShapePredictorFileName;
        }

        /// <summary>
        /// Raises the back button click event.
        /// </summary>
        public void OnQuitButtonClick ()
        {
            Application.Quit();
        }

        /// <summary>
        /// Raises the change camera button click event.
        /// </summary>
        public void OnChangeCameraButtonClick ()
        {
            webCamTextureMatSourceGetter.ChangeCamera ();
        }
    }
}