using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoV2 : MonoBehaviour //This is shadowcasting and is unfinished and does not work.
{
    public int viewRadius;

    private class OctantTransform
    {
        public int xx { get; private set; }
        public int xy { get; private set; }
        public int yx { get; private set; }
        public int yy { get; private set; }

        public OctantTransform(int xx, int xy, int yx, int yy)
        {
            this.xx = xx;
            this.xy = xy;
            this.yx = yx;
            this.yy = yy;
        }

        public override string ToString()
        {
            // consider formatting in constructor to reduce garbage
            return string.Format("[OctantTransform {0,2:D} {1,2:D} {2,2:D} {3,2:D}]",
                xx, xy, yx, yy);
        }
    }
    private static OctantTransform[] s_octantTransform = {
        new OctantTransform( 1,  0,  0,  1 ),   // 0 E-NE
        new OctantTransform( 0,  1,  1,  0 ),   // 1 NE-N
        new OctantTransform( 0, -1,  1,  0 ),   // 2 N-NW
        new OctantTransform(-1,  0,  0,  1 ),   // 3 NW-W
        new OctantTransform(-1,  0,  0, -1 ),   // 4 W-SW
        new OctantTransform( 0, -1, -1,  0 ),   // 5 SW-S
        new OctantTransform( 0,  1, -1,  0 ),   // 6 S-SE
        new OctantTransform( 1,  0,  0, -1 ),   // 7 SE-E
    };

    public static void ComputeVisibility(Vector2Int gridPosn, int viewRadius)
    {
        //Debug.Assert(gridPosn.x >= 0 && gridPosn.x < grid.xDim);
        //Debug.Assert(gridPosn.y >= 0 && gridPosn.y < grid.yDim);

        // Viewer's cell is always visible.
        MapManager.map[gridPosn.x, gridPosn.y].isVisible = true;

        // Cast light into cells for each of 8 octants.
        //
        // The left/right inverse slope values are initially 1 and 0, indicating a diagonal
        // and a horizontal line.  These aren't strictly correct, as the view area is supposed
        // to be based on corners, not center points.  We only really care about one side of the
        // wall at the edges of the octant though.
        //
        // NOTE: depending on the compiler, it's possible that passing the octant transform
        // values as four integers rather than an object reference would speed things up.
        // It's much tidier this way though.
        for (int txidx = 0; txidx < s_octantTransform.Length; txidx++)
        {
            CastLight(gridPosn, viewRadius, 1, 1.0f, 0.0f, s_octantTransform[txidx]);
        }
    }


    private static void CastLight(Vector2Int pos, int viewRad, int startColumn,
        float leftViewSlope, float rightViewSlope, OctantTransform txfrm)
    {

    }

}
