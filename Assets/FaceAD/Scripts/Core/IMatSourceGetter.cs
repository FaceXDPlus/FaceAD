using OpenCVForUnity.CoreModule;

namespace FaceADVtuber
{
    public interface IMatSourceGetter
    {
        Mat GetMatSource ();

        Mat GetDownScaleMatSource ();

        float GetDownScaleRatio ();
    }
}