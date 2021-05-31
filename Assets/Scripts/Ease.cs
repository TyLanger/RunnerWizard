using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ease
{
    /// <summary>
    /// Grow over time following a sine curve
    /// </summary>
    /// <param name="t">Between 0 and 1</param>
    /// <returns></returns>
    public static float Grow(float t)
    {
        return Mathf.Sin(Mathf.PI * t * 0.5f);
    }

    /// <summary>
    /// Decay over time following a cos curve
    /// </summary>
    /// <param name="t">Between 0 and 1</param>
    /// <returns></returns>
    public static float Decay(float t)
    {
        return Mathf.Cos(Mathf.PI * t * 0.5f);
    }

    /// <summary>
    /// Grow and then shrink following a sine wave.
    /// 0 - 1 - 0
    /// </summary>
    /// <param name="t">Between 0 and 1</param>
    /// <returns></returns>
    public static float Hill(float t)
    {
        return Mathf.Sin(Mathf.PI * t);
    }

    /// <summary>
    /// Decay and then grow following a sin wave.
    /// 1 - 0 - 1
    /// </summary>
    /// <param name="t">Between 0 and 1</param>
    /// <returns></returns>
    public static float Valley(float t)
    {
        return 1 - Hill(t);
    }

    /// <summary>
    /// Cubes the input.
    /// 0 - 0.125 - 1
    /// </summary>
    /// <param name="t">Between 0 and 1</param>
    /// <returns></returns>
    public static float Cube(float t)
    {
        return Mathf.Pow(t, 3);
    }

    /// <summary>
    /// Linear with smooth start and end. S-shaped
    /// </summary>
    /// <param name="t">Between 0 and 1</param>
    /// <returns></returns>
    public static float SmoothStep(float t)
    {
        return t * t * (3 - 2 * t);
    }

    // easings.net has some more good ones
}
