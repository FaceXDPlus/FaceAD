using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceADVtuber
{
    [DisallowMultipleComponent]
    public class FaceADVTuberProcessOrderList : MonoBehaviour
    {
        [SerializeField]
        List<FaceADVTuberProcess> processOrderList = default(List<FaceADVTuberProcess>);

        public List<FaceADVTuberProcess> GetProcessOrderList ()
        {
            return processOrderList;
        }
    }
}
