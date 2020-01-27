using System.Collections.Generic;
using UnityEngine;

namespace myScripts {
    public class MeshContainer : MonoBehaviour {

        public Vector2 Verts {
            set
            {
                xVerts = (int) value.x;
                yVerts = (int) value.y;
            }
        }

        public Material MeshMaterial { private get; set; }
        private int xVerts = 1;
        private int yVerts = 1;
        private List<MeshRenderer> _storedMeshes = new List<MeshRenderer>( );
        private Vector3 _selSize;

        private void Awake( )
            {
                gameObject.AddComponent<MeshFilter>( );
                gameObject.AddComponent<MeshRenderer>( );
                gameObject.isStatic = true;
                gameObject.name = "meshContainer";
                Debug.Log( "mesh set" );
            }

    
        public void SetMeshContainer( string tagName )
            {
                GameObject[ ] tempObjs = GameObject.FindGameObjectsWithTag( tagName );

                _storedMeshes.Clear( );
                List<MeshRenderer> added = new List<MeshRenderer>( );

                foreach ( var o in tempObjs ) {
                    added = ObjHelper.FindMeshRenderer( o );

                    foreach ( var m in added ) {
                        _storedMeshes.Add( m );
                    }
                }
            }

        private void Update( )
            {
                if ( _storedMeshes.Count <= 0 ) return;

                Bounds tempBounds = _storedMeshes.EncapsulateBounds( );
                BasicBox tempBox = new BasicBox( tempBounds.center, tempBounds.extents, tempBounds.size );
                GetComponent<MeshFilter>( ).mesh = MeshHelper.GenerateMesh( xVerts, yVerts, tempBox.Size.x, tempBox.Size.z, tempBox.Bot_BL );
                GetComponent<MeshRenderer>( ).material = MeshMaterial;
            }

    }
}