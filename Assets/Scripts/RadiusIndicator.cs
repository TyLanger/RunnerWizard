using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusIndicator : MonoBehaviour
{
    // stole this from
    // https://gamedev.stackexchange.com/questions/126427/draw-circle-around-gameobject-to-indicate-radius


    [Range(0, 50)]
    public int segments = 50;
    [Range(0, 60)]
    public float xradius = 5;
    [Range(0, 60)]
    public float zradius = 5;
    LineRenderer line;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();
    }

    public void CreatePoints()
    {
        float x;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * zradius;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }
}
