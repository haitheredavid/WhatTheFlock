using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace myScript {
    public class NavMeshSourceTag : MonoBehaviour {

        private static List<MeshFilter> _meshes = new List<MeshFilter>( );

        private void OnEnable( ) {
            var m = GetComponent<MeshFilter>( );

            if ( m != null ) {
                _meshes.Add( m );
            }
            
        }

        private void OnDisable( ) {
            var m = GetComponent<MeshFilter>( );

            if ( m != null ) {
                _meshes.Remove( m );
            }
        }
        public static void Collect( ref List<NavMeshBuildSource> sources ) {
            sources.Clear( );

            for ( var i = 0; i < _meshes.Count; ++i ) {
                var mf = _meshes[ i ];
                if ( mf == null ) continue;

                var m = mf.sharedMesh;
                if ( m == null ) continue;

                var s = new NavMeshBuildSource {
                    shape = NavMeshBuildSourceShape.Mesh,
                    sourceObject = m,
                    transform = mf.transform.localToWorldMatrix,
                    area = 0
                };
                sources.Add( s );
            }
        }

    }
}