using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存放Leap的手势
/// </summary>
public class LeapGestureEntity
{
    /// <summary>
    /// 手掌方向
    /// </summary>
    public PalmDirection PalmDirection { get; set; }
    /// <summary>
    /// 手类型
    /// </summary>
    public HandType HandType { get; set; }
    /// <summary>
    /// 手指方向
    /// </summary>
    public FingersDirection FingersDirection { get; set; }

    /// <summary>
    /// 手指坐标
    /// </summary>
    private List<Vector3> fingerCoordinates;

    //解析
    public LeapGestureEntity(string str)
    {

    }

}
