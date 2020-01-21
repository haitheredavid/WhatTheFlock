using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public static class PathHelper {

    public static Vector3 Quantize( Vector3 v, Vector3 q ) {
        float x = q.x * Mathf.Floor( v.x / q.x );
        float y = q.x * Mathf.Floor( v.y / q.y );
        float z = q.x * Mathf.Floor( v.z / q.z );
        return new Vector3( x, y, z );
    }

    public static float3 Quantize( float3 v, float3 q ) {
        float x = q.x * Mathf.Floor( v.x / q.x );
        float y = q.x * Mathf.Floor( v.y / q.y );
        float z = q.x * Mathf.Floor( v.z / q.z );
        return new float3( x, y, z );
    }

    public static Bounds QauntizeBounds( Vector3 center, Vector3 size, float factor ) {
        return new Bounds( Quantize( center, factor * size ), size );
    }

    public static IEnumerator GenerateVertices( int xAmount, int yAmount, float time ) {
        float3[ ] verts = new float3[ ( xAmount + 1 ) * ( yAmount + 1 ) ];

        for ( int i = 0, y = 0; y <= yAmount; y++ ) {
            for ( int x = 0; x <= xAmount; x++, i++ ) {
                verts[ i ] = new float3( x, y, 0 );
                yield return new WaitForSeconds( time );
            }
        }
        yield return verts;
    }

    public static Vector3[ ] GenerateVertices( int xAmount, int yAmount ) {
        Vector3[ ] verts = new Vector3[ ( xAmount + 1 ) * ( yAmount + 1 ) ];

        for ( int i = 0, y = 0; y <= yAmount; y++ ) {
            for ( int x = 0; x <= xAmount; x++, i++ ) {
                verts[ i ] = new Vector3( x, y, 0 );
            }
        }
        return verts;
    }

    public static Mesh GenerateMesh( int xAmount, int yAmount ) {
        Vector3[ ] verts = new Vector3[ ( xAmount + 1 ) * ( yAmount + 1 ) ];
        Vector2[ ] uvs = new Vector2[ verts.Length ];

        for ( int y = 0, i = 0; y <= yAmount; y++ ) {
            for ( int x = 0; x <= xAmount; x++, i++ ) {
                verts[ i ] = new Vector3( x, y, 0 );
                uvs[ i ] = new Vector2( (float) x / xAmount, (float) y / yAmount );
            }
        }
        int[ ] tri = new int[ xAmount * yAmount * 6 ];
        Mesh mesh = new Mesh {name = "Procedural Mesh", vertices = verts};

        for ( int ti = 0, vi = 0, y = 0; y < yAmount; y++, vi++ ) {
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

}
public static class SelectionHelper {

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
    public float delayTime = 0.1f;
    private Mesh _mesh;
    private GameObject _meshObj;
    private Vector3[ ] _vertices;

    void Start( ) {
        // build mesh object 
        _meshObj = new GameObject( "mesh" );
        _meshObj.AddComponent<MeshRenderer>( );
        _meshObj.AddComponent<MeshFilter>( );
        _vertices = PathHelper.GenerateVertices( xSize, ySize );
        _meshObj.GetComponent<MeshFilter>( ).mesh = PathHelper.GenerateMesh( xSize, ySize );
        _meshObj.GetComponent<MeshRenderer>( ).material = meshMaterial;
    }

    private void OnDrawGizmos( ) {
        Gizmos.color = Color.black;
        if ( _vertices == null ) return;

        for ( int i = 0; i < _vertices.Length; i++ ) {
            Gizmos.DrawSphere( _vertices[ i ], pointScale );
        }
    }

}