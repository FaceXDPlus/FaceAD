using NetworkSocket.WebSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FaceAD
{
    public class MyWebSocketClient : WebSocketClient
    {
        public MyWebSocketClient(Uri address)
            : base(address)
        {
        }
    }

    public class FaceADWSController : MonoBehaviour
    {
        private WebSocketClient P2Pclient;
        public Toggle P2PClientSwitch;
        public bool P2PClientStatus = false;

        public async void P2PClientStart(string uritext)
        {
            try
            {
                Globle.AddDataLog("[WSC]客户连接启动中");
                var uri = new Uri(uritext);
                P2Pclient = new MyWebSocketClient(uri);

                var addresses = System.Net.Dns.GetHostAddresses(uri.Host);
                if (addresses.Length == 0)
                {
                    throw new ArgumentException(
                        "解析IP失败",
                        ""
                    );
                }
                await P2Pclient.ConnectAsync(addresses[0], uri.Port);
                if (P2Pclient.IsConnected)
                {
                    this.P2PClientStatus = true;
                    Globle.AddDataLog("[WSC]客户连接已经启动");
                }
                else
                {
                    P2PClientSwitch.isOn = false;
                    Globle.AddDataLog("[WSC]客户连接启动失败");
                }
            }
            catch (Exception ex)
            {
                P2PClientSwitch.isOn = false;
                //Globle.AddDataLog("[WSC]客户连接发生错误 " + ex.Message + " : " + ex.StackTrace);
                Globle.AddDataLog("[WSC]客户连接发生错误 " + ex.Message);
            }
        }

        public void P2PClientSendBinary(byte[] binary)
        {
            try
            {
                if (P2Pclient.IsConnected)
                {
                    P2Pclient.SendBinary(binary);
                }
            }
            catch (Exception ex)
            {
                P2PClientSwitch.isOn = false;
                //Globle.AddDataLog("[WSC]发生错误 " + ex.Message + " : " + ex.StackTrace);
                Globle.AddDataLog("[WSC]发生错误 " + ex.Message);
            }
        }

        public void P2PClientStop()
        {
            if (P2Pclient != null)
            {
                this.P2PClientStatus = false;
                P2Pclient.Close();
                P2Pclient.Dispose();
            }
            Globle.AddDataLog("[WSC]客户连接已经关闭");
        }
    }

}