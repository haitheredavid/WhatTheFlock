using System.Net;
using UnityEngine;

namespace myScripts {
    public abstract class Geo {

        public Vector3 WorldPosition;

    }
    public class Grid : Geo {

        public int X, Y;
        public float CellRadius;
        public int[ , ] IntGrid;
        public Node[ , ] NodeGrid;
        private float _CellDiameter;

        public Grid( int x, int y, float cellRadius ) {
            X = x;
            Y = y;
            CellRadius = cellRadius;
            _CellDiameter = CellRadius * 2f;
        }

    

    }
    public class Node : Geo, IHeapItem<Node> {

        public bool Walkable;
        public int GridX;
        public int GridY;
        public Node Parent;
        private int _heapIndex;
        public int G_Cost;
        public int H_Cost;
        public int F_Cost => G_Cost + H_Cost;

        public Node( bool walkable, Vector3 worldPos, int gridX, int gridY ) {
            Walkable = walkable;
            WorldPosition = worldPos;
            GridX = gridX;
            GridY = gridY;
        }

        public int CompareTo( Node other ) {
            int compare = F_Cost.CompareTo( other.F_Cost );

            if ( compare == 0 ) {
                compare = H_Cost.CompareTo( other.H_Cost );
            }
            return -compare;
        }

        public int HeapIndex {
            get => _heapIndex;
            set => _heapIndex = value;
        }

    }
    public class BasicBox : Geo {

        public Vector3 Extent { get; }
        public Vector3 Center { get; }
        public Vector3 Size { get; }
        public Vector3 Top_TL => TopPoints[ 0 ];
        public Vector3 Top_BL => TopPoints[ 3 ];
        public Vector3 Bot_TL => BottomPoints[ 0 ];
        public Vector3 Bot_BL => BottomPoints[ 3 ];
        public Vector3[ ] TopPoints { get; private set; }
        public Vector3[ ] BottomPoints { get; private set; }

        public BasicBox( Vector3 center, Vector3 extent, Vector3 size ) {
            Center = center;
            Extent = extent;
            Size = size;
            BuildBox( );
        }

        private void BuildBox( ) {
            TopPoints = new[ ] {
                new Vector3( Center.x - Extent.x, Center.y + Extent.y, Center.z + Extent.z ), // top left
                new Vector3( Center.x + Extent.x, Center.y + Extent.y, Center.z + Extent.z ), // top right 
                new Vector3( Center.x + Extent.x, Center.y + Extent.y, Center.z - Extent.z ), // bot right 
                new Vector3( Center.x - Extent.x, Center.y + Extent.y, Center.z - Extent.z ) // bot left 
            };
            BottomPoints = new Vector3[ TopPoints.Length ];

            for ( var i = 0; i < BottomPoints.Length; i++ ) {
                BottomPoints[ i ] = new Vector3( TopPoints[ i ].x, TopPoints[ i ].y - Size.y, TopPoints[ i ].z );
            }
        }

    }
}