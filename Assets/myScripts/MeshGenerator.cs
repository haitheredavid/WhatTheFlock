using System.Collections.Generic;
using System.Linq;
using myScripts;
using UnityEngine;

public static class Meshy {

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

    public static List<MeshRenderer> FindMeshRenderer( GameObject obj ) {
        List<MeshRenderer> temp = new List<MeshRenderer>( );

        foreach ( Transform c1 in obj.transform ) {
            MeshRenderer c1mesh = c1.transform.GetComponent<MeshRenderer>( );

            if ( c1mesh != null ) {
                temp.Add( c1mesh );
            }

            if ( c1.transform.childCount > 0 ) {
                List<MeshRenderer> childMeshes = FindMeshRenderer( c1.gameObject );

                foreach ( var m in childMeshes ) {
                    temp.Add( m );
                }
            }
        }
        return temp;
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
public class MeshGenerator : MonoBehaviour {

    public int xSize;
    public int ySize;
    public Material meshMaterial;
    public float pointScale = 0.1f;
    public RectTransform selectRect;
    private GameObject _meshObj;
    private Vector3[ ] _vertices;
    private List<MeshRenderer> _tempMeshes = new List<MeshRenderer>( );
    private GameObject[ ] _tempObjs;
    private Bounds _bounds;
    private MouseSelection _selection;
    private Vector3[ ] _selRect;
    private Vector3 _selSize;
    private Vector3 _selStart;

    private void Start( ) {
        _tempObjs = GameObject.FindGameObjectsWithTag( "BlueGoal" );
        _meshObj = Meshy.GenerateMeshObj( );
        _selection = new MouseSelection( selectRect, Camera.main, LayerMask.GetMask( "Ground" ) );
    }

    private void Update( ) {
        List<MeshRenderer> added = new List<MeshRenderer>( );

        foreach ( var o in _tempObjs ) {
            added = Meshy.FindMeshRenderer( o );

            foreach ( var m in added ) {
                _tempMeshes.Add( m );
            }
        }

        if ( _tempMeshes.Count > 0 ) {
            _bounds = _tempMeshes.EncapsulateBounds( );
        }
        _selection.CheckSelection( );
        _selRect = _selection.SelectionBounds;
        _selSize = _selection.SelectionSize;
        _selStart = _selection.StartPosition;

        if ( _selRect != null ) {
            Debug.Log( "draw mesh" );
            _meshObj.GetComponent<MeshFilter>( ).mesh = Meshy.GenerateMesh( xSize, ySize, _selSize.x, _selSize.z, _selStart );
            _meshObj.GetComponent<MeshRenderer>( ).material = meshMaterial;
        }
        Debug.DrawLine( _bounds.max, _bounds.min, Color.blue );
    }

    private void OnDrawGizmos( ) {
        Gizmos.color = Color.black;

        // draw points of the box and class for drawing the lines
        Gizmos.DrawWireCube( _bounds.center, _bounds.size );
        if ( _vertices == null ) return;

        for ( int i = 0; i < _vertices.Length; i++ ) {
            Gizmos.DrawSphere( _vertices[ i ], pointScale );
        }
    }

}