using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityMesh : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    Mesh mesh;

    public int width = 20;
    public int height = 20;
    public float scale = 3;
    public Color colour = Color.gray;

    Vector3[] verts;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        MakeVerts();
        MakeMesh();

        CarveCube(transform.position, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeVerts()
    {
        verts = new Vector3[width * height];
        int vertIndex = 0;

        // center the mesh on the gameObject
        Vector3 bottomLeftPos = transform.position - new Vector3(scale * (width - 1) / 2, 0, scale * (height - 1) / 2);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                verts[vertIndex] = bottomLeftPos + new Vector3(i * scale, 0, j * scale);
                vertIndex++;
            }
        }
    }

    void MakeMesh()
    {
        mesh = new Mesh();
        triangles = new int[(width - 1) * (height - 1) * 6];
        int triIndex = 0;
        int vertIndex = 0;
        Vector2[] uvs = new Vector2[width * height];

        Texture2D texture = new Texture2D(width, height);
        Color[] colourMap = new Color[width * height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                uvs[vertIndex] = new Vector2(j / (float)height, i / (float)width);
                colourMap[vertIndex] = colour;

                if(i<(width-1) && j<(height-1))
                {
                    triangles[triIndex] = vertIndex;
                    triangles[triIndex + 1] = vertIndex + width + 1;
                    triangles[triIndex + 2] = vertIndex + width;
                    triIndex += 3;

                    triangles[triIndex] = vertIndex + width+1;
                    triangles[triIndex + 1] = vertIndex;
                    triangles[triIndex + 2] = vertIndex + 1;
                    triIndex += 3;
                }
                vertIndex++;
            }
        }
        mesh.MarkDynamic();

        mesh.vertices = verts;
        mesh.uv = uvs;

        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();

        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshRenderer.sharedMaterial.mainTexture = texture;
    }

    int GetIndexFromPos(Vector3 position)
    {
        Vector3 bottomLeftPos = transform.position - new Vector3(scale * (width - 1) / 2, 0, scale * (height - 1) / 2);
        Vector3 diff = position - bottomLeftPos;
        int xPos = Mathf.RoundToInt(diff.x / scale);
        int zPos = Mathf.RoundToInt(diff.z / scale);
        return xPos * width + zPos;
    }

    int GetIndexFromGrid(int x, int z)
    {
        return x * width + z;
    }

    void CarveCube(Vector3 center, int size)
    {
        int vertIndex = GetIndexFromPos(center);
        int left = size / 2;
        int bottom = size / 2;

        // what do I need to do?
        // dupe edge verts.
        // create new verts directly under them
        // attach edge verts to new vert
        // attach new verts to center verts
        // move center verts down

        // how many extra tris?
        // how many extra verts?
        // 4x4 square (3 cubes)
        // v += 4 bottom + 4 top + 2 + 2 sides = 12 3*4 = 12
        // 6x6
        // v += 6+6+4+4 = 20 = 4*5
        // v += (size-1)*4
        // triangles
        // 4x4
        // 6 each side
        // (size-1) * 2 * 4

        // triangles[] = vertIndex
        // need to update the trianlges[] for every vert past this hole.
        // or do I?
        // can I tack these new tris at the end?
        // new verts too probably
        //Vector3[] newVerts = new Vector3[verts.Length + (size - 1) * 4];
        //int[] newTris = new int[triangles.Length + (size - 1) * 8];

        int newVertIndex = verts.Length;
        int triIndex = triangles.Length;
        int oldTriIndex = vertIndex * 2;

        Array.Resize(ref verts, verts.Length + (size - 1) * 4);
        Array.Resize(ref triangles, triangles.Length + (size - 1) * 8*3);

        Debug.Log($"oldLength: {newVertIndex} newLength: {verts.Length}");
        Debug.Log($"oldLength: {triIndex} newLength: {triangles.Length}");


        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                bool edge = i == 0 || j == 0 || i == (size - 1) || j == (size - 1);
                if (edge)
                {
                    // make a new vert. Add it to the last 
                    Debug.Log($"{i},{j} new:{newVertIndex} vert:{vertIndex} total:{vertIndex + i * width + j}");
                    verts[newVertIndex] = verts[vertIndex + i * width + j] + new Vector3(0, -5, 0);

                    int oldVertsIndex = vertIndex + i * width + j;
                    // fix old tris
                    // triangles skip the top vert so it's not as simple as *6
                    int x = oldVertsIndex / height;
                    int z = oldVertsIndex % height;
                    oldTriIndex = ((height - 1) * x + z) * 6;

                    if ((i == 0 || i == (size - 1)) && (j < (size - 1)))
                    {
                        Debug.Log($"LR x,y {i},{j}");
                        // left and right edges without top edge
                        // what are the points? 
                        // new points are on the bottom
                        // old, old +z, new, new +z

                        // need to find a diff triIndex
                        // need to edit old triangles
                        // and copy the old ones to use the new verts
                        triangles[triIndex + 0] = oldVertsIndex;
                        triangles[triIndex + 1] = oldVertsIndex + 1;
                        triangles[triIndex + 2] = newVertIndex + 1;
                        triIndex += 3;

                        triangles[triIndex + 0] = oldVertsIndex;
                        triangles[triIndex + 1] = newVertIndex + 1;
                        triangles[triIndex + 2] = newVertIndex;
                        triIndex += 3;

                        triangles[oldTriIndex] = newVertIndex;
                        triangles[oldTriIndex+4] = newVertIndex;
                        triangles[oldTriIndex+5] = newVertIndex+1;

                    }

                    if ((j == 0 || j == (size - 1)) && (i < (size - 1)))
                    {
                        Debug.Log($"TB x,y {i},{j}");

                        // top and bottom edges without right edge
                        // what are the points?
                        // new points are on the bottom
                        // old, old +x, new, new +x

                        // at 0,0, need new, new + size
                        // at 1,0, need new, new +2
                        // at 2,0, need new, new +2
                        // at size, 0, need new, new +2
                        // at size, size, need new, new +size
                        // not really size, size. size-1, size-2

                        int spacing = 2;
                        if ((j == 0 && i == 0) || (j == (size - 1) && i == (size - 2)))
                        {
                            spacing = size;
                        }

                        triangles[triIndex + 0] = oldVertsIndex;
                        triangles[triIndex + 1] = newVertIndex;
                        triangles[triIndex + 2] = newVertIndex + spacing;
                        triIndex += 3;

                        triangles[triIndex + 0] = oldVertsIndex;
                        triangles[triIndex + 1] = newVertIndex + spacing;
                        triangles[triIndex + 2] = oldVertsIndex + width;
                        triIndex += 3;

                        triangles[oldTriIndex + 2] = newVertIndex + spacing;
                    }
                    newVertIndex++;
                }
                else
                {
                    // modify the existing vert
                    verts[vertIndex + i * width + j] = verts[vertIndex + i * width + j] + new Vector3(0, -5, 0);
                }
            }
            
        }

        // Add trianlges
        // how?
        // what order?
        // corners are different than sides
        // Need to each edge. Always 4 edges of a square
        // Say I do and ij loop like before
        // i==0 first z loop, Make trianlges up.
        // i==1 z==0 && z==size make triangles left
        // i==size make trianlges up AND triangles left at z0,zSize

        mesh.MarkDynamic();
        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
