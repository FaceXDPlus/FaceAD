using System.Collections.Generic;
using UnityEngine;

namespace FaceADVtuber
{
    public interface IFaceLandmarkGetter
    {
        List<Vector2> GetFaceLanmarkPoints ();
    }
}