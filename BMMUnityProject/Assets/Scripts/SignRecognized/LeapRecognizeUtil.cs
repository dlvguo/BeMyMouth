using System.Collections;
using UnityEngine;

public class LeapRecognizeUtil
{
    //计算手指双向
    public static PalmDirection FigurePalmDirection(Leap.Vector vector)
    {
        double x = Mathf.Abs(vector.x), y = Mathf.Abs(vector.y), z = Mathf.Abs(vector.z);
        // x最大说明是Left和Right
        if (x > y && x > z)
        {
            if (vector.x > 0)
                return PalmDirection.Left;
            return PalmDirection.Right;
        }
        // y最大说明是Front和Behind
        else if (y > z && y > x)
        {
            if (vector.y > 0)
                return PalmDirection.Front;
            return PalmDirection.Behind;

        }
        //z最大说明是Up和Down
        else
        {
            if (vector.z > 0)
                return PalmDirection.Down;
            return PalmDirection.Up;
        }
    }

}
