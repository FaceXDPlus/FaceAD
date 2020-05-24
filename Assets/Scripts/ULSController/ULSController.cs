#define DRAW_MARKERS

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ULSTrackerForUnity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FaceAD
{
	class ULSController : MonoBehaviour
	{

		public GameObject marker = null;
		public GameObject _marksParent;
		List<GameObject> _marks = new List<GameObject>();

		public float[] _trackPoints = null;

		bool initDone = false;
		public float AngleX = 0f;
		public float AngleY = 0f;
		public float AngleZ = 0f;

		public bool CaptureStatus = false;

		public RawImage RawImage;


		void createMesh()
		{
			_trackPoints = new float[Plugins.MAX_TRACKER_POINTS * 2];
			/*for (int i = 0; i < Plugins.MAX_TRACKER_POINTS; ++i)
			{
				var g = Instantiate(marker);
				g.transform.parent = transform.parent;
				g.SetActive(false);
				g.transform.SetParent(_marksParent.transform);
				_marks.Add(g);
			}*/
		}

		void Start()
		{
			InitializeTrackerAndCheckKey();
			createMesh();
			Application.targetFrameRate = 60;
		}

		void InitializeTrackerAndCheckKey()
		{
			Plugins.OnPreviewStart = initCameraTexture;

			int initTracker = Plugins.ULS_UnityTrackerInit();
			if (initTracker < 0)
			{
				Debug.Log("Failed to initialize tracker.");
			}
			else
			{
				Debug.Log("Tracker initialization succeeded");
			}
		}

		void initCameraTexture(Texture preview, int rotate)
		{
			RawImage.texture = preview;
#if UNITY_STANDALONE || UNITY_EDITOR
			transform.localScale = new Vector3(-1, -1, 1);
			_marksParent.transform.localScale = new Vector3(-1, -1, 1);
#elif UNITY_IOS || UNITY_ANDROID
		transform.eulerAngles = new Vector3 (0, 0, rotate); //orientation
			_marksParent.transform.eulerAngles = new Vector3 (0, 0, rotate); //orientation
#endif
			initDone = true;
		}

		void Update()
		{
			if (!initDone)
				return;

			if (0 < Plugins.ULS_UnityGetPoints(_trackPoints))
			{
				/*for (int j = 0; j < Plugins.MAX_TRACKER_POINTS; ++j)
				{
					int v = j * 2;
					Vector3 pt = new Vector3(_trackPoints[v], _trackPoints[v + 1], 0);
					_marks[j].name = "Point" + j;
					_marks[j].transform.localPosition = (pt);
					_marks[j].SetActive(true);
				}*/
				CaptureStatus = true;
				AngleX = Plugins.ULS_UnityGetYawRadians();
				AngleY = Plugins.ULS_UnityGetPitchRadians();
				AngleZ = Plugins.ULS_UnityGetRollRadians();
			}
			else
			{
				CaptureStatus = false;
			}
		}

	}
}