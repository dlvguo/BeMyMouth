using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存放Leap的手势
/// </summary>
public class LeapGestureEntity
{
    /// <summary>
    /// 手类型
    /// </summary>
    public HandType HandType { get; set; }

    /// <summary>
    /// 手掌方向
    /// </summary>
    public PalmDirection LeftPalmDirection { get; set; }
    /// <summary>
    /// 手掌方向
    /// </summary>
    public PalmDirection RightPalmDirection { get; set; }

    /// <summary>
    /// 各个手指关节计算 最短距离
    /// </summary>
    public List<float> LeftFingersDist { get; set; }
    /// <summary>
    /// 左手的手指是否展开/收缩
    /// </summary>
    public List<bool> LeftFingersExtenison { get; set; }

    public List<float> RightFingersDist { get; set; }

    public List<bool> RightFingersExtenison { get; set; }



}
