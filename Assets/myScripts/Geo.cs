using UnityEngine;

namespace myScripts {
    public abstract class Geo {



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
                new Vector3( Center.x - Extent.x, Center.y + Extent.y, Center.z - Extent.z )  // bot left 
            };
            BottomPoints = new Vector3[ TopPoints.Length ];

            for ( var i = 0; i < BottomPoints.Length; i++ ) {
                BottomPoints[ i ] = new Vector3( TopPoints[ i ].x, TopPoints[ i ].y - Size.y, TopPoints[ i ].z );
            }
        }

    }
}