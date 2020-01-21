using System;
using UnityEngine;

namespace myScripts {
    public class MouseSelection : MonoBehaviour {

        public bool drawDebug = true;
        public RectTransform selectionRect;
        private Vector3 _rectStartPoint, _rectEndPoint;

        private Vector3 _topLeft, _topRight, _botRight, _botLeft;
        private Vector3[ ] _createdRectPoints = new Vector3[ 4 ];
        
        private float _delay = 0.3f;
        private float _clickedTime = 0.0f;
        private bool _createdSquare;
        private Camera _camera;
        private LayerMask _mask;

        private void Awake( ) {
            selectionRect = Instantiate( selectionRect );
            selectionRect.name = "Selection Rect";
            selectionRect.gameObject.SetActive( false );
            _mask = LayerMask.GetMask( "Ground" );
            _camera = Camera.main;
        }

        private void SelectObjects( ) {
            //https://www.habrador.com/tutorials/select-units-within-rectangle/
            bool isHoldingDown = false;

            // when input has been triggered
            if ( Input.GetMouseButtonDown( 0 ) ) {
                // store time when clicked 
                _clickedTime = Time.time;
                // cast ray   
                Ray ray = _camera.ScreenPointToRay( Input.mousePosition );

                // only check if hit was within mask 
                if ( Physics.Raycast( ray, out var hit, _mask ) ) {
                    // register first hit 
                    _rectStartPoint = hit.point;
                }
            }

            // when input is released
            if ( Input.GetMouseButtonUp( 0 ) ) {
                // select objects within square
                if ( _createdSquare ) {
                    _createdSquare = false;
                    ObjectsInSelectedArea( );
                }
            }

            // if input is being held
            if ( Input.GetMouseButton( 0 ) ) {
                if ( Time.time - _clickedTime > _delay ) {
                    isHoldingDown = true;
                }
            }

            // if we are currently drawing the rect
            if ( isHoldingDown ) {
                if ( !selectionRect.gameObject.activeInHierarchy ) {
                    selectionRect.gameObject.SetActive( true );
                }
                _rectEndPoint = Input.mousePosition;
                DrawRect( );
            }
        }

        private void ObjectsInSelectedArea( ) {
            _createdRectPoints = new[ ] {_topLeft, _topRight, _botRight, _botLeft};
        }

        private void DrawRect( ) {
            Vector3 rectStartScreen = _camera.WorldToScreenPoint( _rectStartPoint );
            rectStartScreen.z = 0f;
            Vector3 middle = ( rectStartScreen + _rectEndPoint ) / 2f;
            float xDim = Mathf.Abs( rectStartScreen.x - _rectEndPoint.x );
            float yDim = Mathf.Abs( rectStartScreen.y - _rectEndPoint.y );
            selectionRect.position = middle;
            selectionRect.sizeDelta = new Vector2( xDim, yDim );

            // build the corners of the square
            _topLeft = new Vector3( middle.x - xDim / 2f, middle.y + yDim / 2f, 0f );
            _topRight = new Vector3( middle.x + xDim / 2f, middle.y + yDim / 2f, 0f );
            _botRight = new Vector3( middle.x + xDim / 2f, middle.y - yDim / 2f, 0f );
            _botLeft = new Vector3( middle.x - xDim / 2f, middle.y - yDim / 2f, 0f );
            int i = 0;

            if ( Physics.Raycast( _camera.ScreenPointToRay( _topLeft ), out var hit, _mask ) ) {
                _topLeft = hit.point;
                i++;
            }

            if ( Physics.Raycast( _camera.ScreenPointToRay( _topRight ), out hit, _mask ) ) {
                _topRight = hit.point;
                i++;
            }

            if ( Physics.Raycast( _camera.ScreenPointToRay( _botRight ), out hit, _mask ) ) {
                _botRight = hit.point;
                i++;
            }

            if ( Physics.Raycast( _camera.ScreenPointToRay( _botLeft ), out hit, _mask ) ) {
                _botLeft = hit.point;
                i++;
            }
            _createdSquare = false;
            if ( i != 4 ) return;

            if ( drawDebug ) {
                Debug.DrawLine( _topLeft, _topRight, Color.blue );
                Debug.DrawLine( _topRight, _botRight, Color.yellow );
                Debug.DrawLine( _botRight, _botLeft, Color.red );
                Debug.DrawLine( _botLeft, _topLeft, Color.green );
            }
            _createdSquare = true;
        }

    }
}