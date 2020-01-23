using System.Collections.Generic;
using UnityEngine;

namespace myScripts {
    public class MeshContainer : MonoBehaviour {

        public Vector2 Verts {
            set {
                xVerts = (int) value.x;
                yVerts = (int) value.y;
            }
        }
        public Material MeshMaterial { private get; set; }
        private int xVerts = 1;
        private int yVerts = 1;
        private List<MeshRenderer> _storedMeshes = new List<MeshRenderer>( );
        private Vector3 _selSize;

        private void Awake( ) {
            gameObject.AddComponent<MeshFilter>( );
            gameObject.AddComponent<MeshRenderer>( );
            gameObject.isStatic = true;
            gameObject.name = "meshContainer";
            Debug.Log( "mesh set" );
        }

     

        public void UpdateMeshContainer( string tagName ) {
            GameObject[] tempObjs = GameObject.FindGameObjectsWithTag( tagName );

            _storedMeshes.Clear( );
            List<MeshRenderer> added = new List<MeshRenderer>( );

            foreach ( var o in tempObjs ) {
                added = GeoHelper.FindMeshRenderer( o );

                foreach ( var m in added ) {
                    _storedMeshes.Add( m );
                }
            }
        }

        private void Update( ) {
            if ( _storedMeshes.Count <= 0 ) return;

            Bounds tempBounds = _storedMeshes.EncapsulateBounds( );
            Geo.BasicBox tempBox = new Geo.BasicBox( tempBounds.center, tempBounds.extents, tempBounds.size );
            GetComponent<MeshFilter>( ).mesh = GeoHelper.GenerateMesh( xVerts, yVerts, tempBox.Size.x, tempBox.Size.z, tempBox.Bot_BL );
            GetComponent<MeshRenderer>( ).material = MeshMaterial;
        }

    }
}

public static class CheckArea {

    public static bool IsWithinTriangle( Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3 ) {
        bool isWithin = false;
        float denominator = ( p2.z - p3.z ) * ( p1.x - p3.x ) + ( p3.x - p2.x ) * ( p1.z - p3.z );
        float a = ( ( p2.z - p3.z ) * ( p.x - p3.x ) + ( p3.x - p2.x ) * ( p.z - p3.z ) ) / denominator;
        float b = ( ( p3.z - p1.z ) * ( p.x - p3.x ) + ( p1.x - p3.x ) * ( p.z - p3.z ) ) / denominator;
        float c = 1 - a - b;

        // the point is within the tri if 
        // 0 <= a <= 1
        // 0 <= b <= 1 
        // 0 <= c <= 1
        if ( a >= 0f && a <= 1f && b >= 0f && b <= 1f && c > 0f && c <= 1f ) {
            isWithin = true;
        }
        return isWithin;
    }

}