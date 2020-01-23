using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace myScripts {
    public static class GeoHelper {

        public static Vector3 Quantize( Vector3 v, Vector3 q ) {
            float x = q.x * Mathf.Floor( v.x / q.x );
            float y = q.x * Mathf.Floor( v.y / q.y );
            float z = q.x * Mathf.Floor( v.z / q.z );
            return new Vector3( x, y, z );
        }

        public static Bounds QauntizeBounds( Vector3 center, Vector3 size, float factor ) {
            return new Bounds( Quantize( center, factor * size ), size );
        }

        public static Vector3[ ] GenerateVertices( int xAmount, int zAmount, Vector3 loc ) {
            Vector3[ ] verts = new Vector3[ ( xAmount + 1 ) * ( zAmount + 1 ) ];

            for ( int i = 0, y = 0; y <= zAmount; y++ ) {
                for ( int x = 0; x <= xAmount; x++, i++ ) {
                    verts[ i ] = new Vector3( x, 0, y ) + loc;
                }
            }
            return verts;
        }

        public static Vector3[ ] GenerateVertices( int xAmount, int zAmount, float xSize, float zSize, Vector3 loc ) {
            Vector3[ ] verts = new Vector3[ ( xAmount + 1 ) * ( zAmount + 1 ) ];

            for ( int i = 0, z = 0; z <= zAmount; z++ ) {
                for ( int x = 0; x <= xAmount; x++, i++ ) {
                    verts[ i ] = new Vector3( (float) x / xAmount * xSize, 0, (float) z / zAmount * zSize ) + loc;
                }
            }
            return verts;
        }

        public static Mesh GenerateMesh( int xAmount, int zAmount, float xSize, float zSize, Vector3 loc ) {
            Vector3[ ] verts = new Vector3[ ( xAmount + 1 ) * ( zAmount + 1 ) ];
            Vector2[ ] uvs = new Vector2[ verts.Length ];

            for ( int z = 0, i = 0; z <= zAmount; z++ ) {
                for ( int x = 0; x <= xAmount; x++, i++ ) {
                    verts[ i ] = new Vector3( (float) x / xAmount * xSize, 0, (float) z / zAmount * zSize ) + loc;
                    uvs[ i ] = new Vector2( (float) x / xAmount, (float) z / zAmount );
                }
            }
            int[ ] tri = new int[ xAmount * zAmount * 6 ];
            Mesh mesh = new Mesh {name = "Procedural Mesh", vertices = verts};

            for ( int ti = 0, vi = 0, y = 0; y < zAmount; y++, vi++ ) {
                for ( int x = 0; x < xAmount; x++, ti += 6, vi++ ) {
                    tri[ ti ] = vi;
                    tri[ ti + 3 ] = tri[ ti + 2 ] = vi + 1;
                    tri[ ti + 4 ] = tri[ ti + 1 ] = vi + xAmount + 1;
                    tri[ ti + 5 ] = vi + xAmount + 2;
                }
            }
            mesh.triangles = tri;
            mesh.uv = uvs;
            mesh.RecalculateNormals( );
            return mesh;
        }

        public static Mesh GenerateMesh( int xAmount, int zAmount, Vector3 loc ) {
            Vector3[ ] verts = new Vector3[ ( xAmount + 1 ) * ( zAmount + 1 ) ];
            Vector2[ ] uvs = new Vector2[ verts.Length ];

            for ( int y = 0, i = 0; y <= zAmount; y++ ) {
                for ( int x = 0; x <= xAmount; x++, i++ ) {
                    verts[ i ] = new Vector3( x, 0, y ) + loc;
                    uvs[ i ] = new Vector2( (float) x / xAmount, (float) y / zAmount );
                }
            }
            int[ ] tri = new int[ xAmount * zAmount * 6 ];
            Mesh mesh = new Mesh {name = "Procedural Mesh", vertices = verts};

            for ( int ti = 0, vi = 0, y = 0; y < zAmount; y++, vi++ ) {
                for ( int x = 0; x < xAmount; x++, ti += 6, vi++ ) {
                    tri[ ti ] = vi;
                    tri[ ti + 3 ] = tri[ ti + 2 ] = vi + 1;
                    tri[ ti + 4 ] = tri[ ti + 1 ] = vi + xAmount + 1;
                    tri[ ti + 5 ] = vi + xAmount + 2;
                }
            }
            mesh.triangles = tri;
            mesh.uv = uvs;
            mesh.RecalculateNormals( );
            return mesh;
        }

        public static GameObject GenerateMeshObj( ) {
            GameObject meshObj = new GameObject( "mesh" );
            meshObj.AddComponent<MeshRenderer>( );
            meshObj.AddComponent<MeshFilter>( );
            return meshObj;
        }

        public static Bounds EncapsulateBounds( this IEnumerable<Renderer> renderers ) {
            return renderers.Select( mesh => mesh.bounds ).Encapsulation( );
        }

        public static Bounds EncapsulateBounds( this IEnumerable<Mesh> meshes ) {
            return meshes.Select( mesh => mesh.bounds ).Encapsulation( );
        }

        private static Bounds Encapsulation( this IEnumerable<Bounds> bounds ) {
            return bounds.Aggregate( ( encapsulation, next ) => {
                encapsulation.Encapsulate( next );
                return encapsulation;
            } );
        }

        public static List<MeshRenderer> FindMeshRenderer( GameObject c0 ) {
            List<MeshRenderer> temp = new List<MeshRenderer>( );
            MeshRenderer c0mesh = c0.transform.GetComponent<MeshRenderer>( );

            if ( c0mesh != null ) {
                temp.Add( c0mesh );
            }
            if ( c0.transform.childCount <= 0 ) return temp;

            foreach ( Transform c1 in c0.transform ) {
                List<MeshRenderer> childMeshes = FindMeshRenderer( c1.gameObject );

                foreach ( var m in childMeshes ) {
                    temp.Add( m );
                }
            }
            return temp;
        }

    }
}