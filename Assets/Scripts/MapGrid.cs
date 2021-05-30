using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    
    public int width = 10;
    public int height = 10;

    public Block cube;

    public float spacing = 1;

    bool[,] gridActive;
    Block[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        gridActive = new bool[width, height];
        grid = new Block[width, height];

        GenerateGrid();
        MoveUp(4, 6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        Vector3 bottomLeft = transform.position + new Vector3(spacing * -(width-1) / 2, 0, spacing * -(height-1) / 2);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Block copy = Instantiate(cube, bottomLeft + new Vector3(i*spacing, -spacing * 0.5f, j*spacing), transform.rotation, transform);
                copy.InitBlock(spacing);
                grid[i, j] = copy;
                gridActive[i, j] = false;
            }
        }
    }

    void MoveUp(int x, int z)
    {
        grid[x, z].MoveUp();
    }

    (int, int) GetGridFromPos(Vector3 pos)
    {
        Vector3 bottomLeft = transform.position - new Vector3(spacing * (width - 1) / 2, transform.position.y, spacing * (height - 1) / 2);
        Vector3 diff = pos - bottomLeft;
        int xPos = Mathf.RoundToInt(diff.x / spacing);
        int zPos = Mathf.RoundToInt(diff.z / spacing);

        return (xPos, zPos);
    }

    public void MoveSquare(Vector3 position, int size, bool up)
    {
        bool isEven = size % 2 == 0;

        int xPos;
        int zPos;

        (xPos, zPos) = GetGridFromPos(position);
        
        // don't go by the center. Go by the bottom left
        if (isEven)
        {
            // assume 2x2 square
            // Want the square you're standing on, but what other 3?
            Vector3 dirToCenter = position - grid[xPos, zPos].transform.position;
            // if dir.x is positive, want 2 blocks to the right
            // if dir.z is positive, want 2 blocks above

            // move the bottomLeft
            int offset = size / 2;
            if(dirToCenter.x < 0)
            {
                xPos -= offset;
            }
            else
            {
                xPos -= (offset - 1);
            }
            if(dirToCenter.z<0)
            {
                zPos -= offset;
            }
            else
            {
                zPos -= (offset - 1);
            }
        }
        else
        {
            int offset = size / 2;
            xPos -= offset;
            zPos -= offset;
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // can get negative if trying to spawn a square near the edge.
                // skip the negatives
                // also skip on the positive edges if out of bounds
                if ((xPos + i) >= 0 && (zPos + j) >= 0 && (xPos + 1) < width && (zPos + 1) < height)
                {
                    gridActive[xPos + i, zPos + j] = up;

                    if (up)
                    {
                        MoveUp(xPos + i, zPos + j);
                    }
                }
            }
        }
    }

    public void MoveCircle(Vector3 center, int radius, bool up)
    {
        // from the center, move every block radius distance from it (4-connected)
        // r=1 is a cross
        // r=2 is a diamond
        // r=3 is also a diamond. They're all diamonds
        
        // draw a circle. move any block 50% in the circle
    }
}
