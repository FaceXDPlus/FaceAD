using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using ULSTrackerForUnity;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

namespace FaceAD
{
    public class FaceADMain : MonoBehaviour
    {
        [SerializeField] private Toggle SubmitSwitch;
        [SerializeField] private GameObject CaptureObject;
        [SerializeField] private Text DebugLogText;
        [SerializeField] private Scrollbar DebugLogScrollbar;
        [SerializeField] private InputField IPAddress;
        [SerializeField] private Text IPAddressDisplay;
        [SerializeField] private InputField Port;
        [SerializeField] private Text PortDisplay;

        private FaceADWSController WSController = null;
        public static Storage storage;
        GameObject dialog = null;
        void Start()
        {
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
                dialog = new GameObject();
            }
#endif
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Load();
            IPAddress.text = storage.ip;
            Port.text = storage.port;
            IPAddressDisplay.text = storage.ip;
            PortDisplay.text = storage.port;
            WSController = new FaceADWSController();
            WSController.P2PClientSwitch = SubmitSwitch;
        }

        private void Update()
        {
            if (Globle.DataLog != "" && Globle.DataLog != null)
            {
                this.Log(Globle.DataLog);
                Globle.DataLog = null;
            }
        }

        void FixedUpdate()
        {
            var faceMask = CaptureObject.GetComponent<ULSController>();
            if (faceMask.CaptureStatus)
            {
                CalcLive2DParams(faceMask._trackPoints, faceMask.AngleX, faceMask.AngleY, faceMask.AngleZ);
            }
        }

        protected void CalcLive2DParams(float[] _trackPoints, float x, float y, float z)
        {
            if (WSController.P2PClientStatus) {
                var distanceOfNoseHeight = new Vector2(_trackPoints[24] - (_trackPoints[32] + _trackPoints[36]) / 2, _trackPoints[25] - (_trackPoints[33] + _trackPoints[37]) / 2).sqrMagnitude;
                var distanceBetweenEyes = new Vector2(_trackPoints[32] - _trackPoints[36], _trackPoints[33] - _trackPoints[37]).sqrMagnitude;

                //print(new Vector2((_trackPoints[22 * 2] - _trackPoints[25 * 2]), (_trackPoints[22 * 2 + 1] - 
                //	_trackPoints[25 * 2 + 1])).sqrMagnitude / distanceBetweenEyes);
                var LeftEyeOpen_Value = Mathf.InverseLerp(0.003f, 0.015f, new Vector2((_trackPoints[34] - _trackPoints[30]), (_trackPoints[35] - _trackPoints[31])).sqrMagnitude / distanceOfNoseHeight);
                //text.text = LeftEyeOpen_Value.ToString();
                var RightEyeOpen_Value = Mathf.InverseLerp(0.003f, 0.015f, new Vector2((_trackPoints[42] - _trackPoints[38]), (_trackPoints[43] - _trackPoints[39])).sqrMagnitude / distanceOfNoseHeight);
                var MouthOpen_Value = Mathf.InverseLerp(0.006f, 0.6f, new Vector2((_trackPoints[58] - _trackPoints[56]), (_trackPoints[59] - _trackPoints[57])).sqrMagnitude / distanceOfNoseHeight);
                var MouthFrom_Value = Mathf.InverseLerp(0.4f, 5f, new Vector2((_trackPoints[44] - _trackPoints[50]), (_trackPoints[22 * 2 + 1] - _trackPoints[25 * 2 + 1])).sqrMagnitude / distanceBetweenEyes);
                var LeftBrow_Value = Mathf.InverseLerp(0.2f, 0.8f, new Vector2(_trackPoints[10] - (_trackPoints[28] + (_trackPoints[32])) / 2, _trackPoints[11] - (_trackPoints[29] + (_trackPoints[33])) / 2).sqrMagnitude / distanceOfNoseHeight);
                var RightBrow_Value = Mathf.InverseLerp(0.2f, 0.8f, new Vector2(_trackPoints[16] - (_trackPoints[36] + (_trackPoints[40])) / 2, _trackPoints[17] - (_trackPoints[37] + (_trackPoints[41])) / 2).sqrMagnitude / distanceOfNoseHeight);




                var returnArray = new Dictionary<string, float>();
                returnArray.Add("mouthOpenY", MouthOpen_Value * 5f);
                returnArray.Add("eyeX", 0);
                returnArray.Add("eyeY", 0);
#if UNITY_STANDALONE || UNITY_EDITOR
                returnArray.Add("headYaw", x * 30f);
                returnArray.Add("headPitch", -y * 30f);
#elif UNITY_IOS || UNITY_ANDROID
                returnArray.Add("headYaw", -y * 30f);
                returnArray.Add("headPitch", -x * 30f);
#endif
                returnArray.Add("headRoll", -z * 30f);
                returnArray.Add("eyeBrowLForm", 0);
                returnArray.Add("eyeBrowRForm", 0);
                returnArray.Add("eyeBrowAngleL", 0);
                returnArray.Add("eyeBrowAngleR", 0);
                returnArray.Add("mouthForm", (MouthFrom_Value) * 11 - 2.7f);
                returnArray.Add("eyeBrowYL", (LeftBrow_Value - 0.5f) * 2f);
                returnArray.Add("eyeBrowYR", (RightBrow_Value - 0.5f) * 2f);
                returnArray.Add("eyeLOpen", LeftEyeOpen_Value);
                returnArray.Add("eyeROpen", RightEyeOpen_Value);


                var indentFile = JsonConvert.SerializeObject(returnArray);
                WSController.P2PClientSendBinary(Encoding.ASCII.GetBytes(indentFile.ToCharArray()));
            }
        }

        public void OnSubmitSwitchSwitched()
        {
            if (SubmitSwitch.isOn == true)
            {
                storage.ip = IPAddress.text;
                storage.port = Port.text;
                Save();
                Globle.AddDataLog("[Main]配置已保存");
                WSController.P2PClientStart("ws://" + IPAddress.text + ":" + Port.text);
            }
            else
            {
                WSController.P2PClientStop();
            }
        }

        public void Log(string text)
        {
            DebugLogText.text += text;
            DebugLogScrollbar.value = -0.0f;
        }

        public struct Storage
        {
            public string ip;//ip
            public string port;//端口
        };

        public void Save()
        {
            var serializer = new XmlSerializer(typeof(Storage));
            var stream = new FileStream(Application.persistentDataPath + "/Storage", FileMode.Create);



            using (stream)
            {
                serializer.Serialize(stream, storage);
            }
        }

        public void Load()
        {
            var serializer = new XmlSerializer(typeof(Storage));
            if (File.Exists(Application.persistentDataPath + "/Storage"))
            {
                var stream = new FileStream(Application.persistentDataPath + "/Storage", FileMode.Open);


                using (stream)
                {
                    storage = (Storage)serializer.Deserialize(stream);
                }
            }
        }
    }
}
