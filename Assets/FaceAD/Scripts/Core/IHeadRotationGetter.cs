using UnityEngine;

namespace FaceADVtuber
{
    public interface IHeadRotationGetter
    {
        Quaternion GetHeadRotation ();

        Vector3 GetHeadEulerAngles ();
    }
}