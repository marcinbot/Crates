using System;
using FarseerGames.FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    public class Camera2D
    {
        Vector2 _position;
        Vector2 _origPosition;
        float _rotation;
        float _origRotation;
        float _zoom;
        float _origZoom;
        float _zoomRate;
        float _maxZoom;
        float _minZoom;
        float _rotationRate;
        Vector2 _size;
        Vector2 _minPosition;
        Vector2 _maxPosition;
        Body _trackingBody;
        public bool rotate = false;

        public Camera2D(Vector2 size,
                        Vector2? position,
                        float? rotation,
                        float? rotationRate,
                        float? zoom, float?
                        zoomRate, float? minZoom,
                        float? maxZoom,
                        Vector2? minPosition,
                        Vector2? maxPosition)
        {
            _position = (position.HasValue) ? position.Value : Vector2.Zero;
            _origPosition = (position.HasValue) ? position.Value : Vector2.Zero;
            _rotation = (rotation.HasValue) ? rotation.Value : 0;
            _origRotation = (rotation.HasValue) ? rotation.Value : 0;
            _rotationRate = (rotationRate.HasValue) ? rotationRate.Value : 0.01f;
            _zoom = (zoom.HasValue) ? zoom.Value : 1;
            _origZoom = (zoom.HasValue) ? zoom.Value : 1;
            _zoomRate = (zoomRate.HasValue) ? zoomRate.Value : 0.01f;
            _minZoom = (minZoom.HasValue) ? minZoom.Value : 0.25f;
            _maxZoom = (maxZoom.HasValue) ? maxZoom.Value : 4f;
            _size = size;
            _minPosition = (minPosition.HasValue) ? minPosition.Value : Vector2.Zero;
            _maxPosition = (maxPosition.HasValue) ? maxPosition.Value : Vector2.Zero;
        }

        public float ZoomRate
        {
            get { return _zoomRate; }
            set { _zoomRate = value; }
        }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }
        public float MaxZoom
        {
            get { return _maxZoom; }
            set { _maxZoom = value; }
        }
        public float MinZoom
        {
            get { return _minZoom; }
            set { _minZoom = value; }
        }
        public float RotationRate
        {
            get { return _rotationRate; }
            set { _rotationRate = value; }
        }
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }
        public Vector2 CurSize
        {
            get { return Vector2.Multiply(_size, 1 / _zoom); }
        }
        public Matrix CameraMatrix
        {
            get
            {
                return Matrix.Identity *
                      Matrix.CreateTranslation(new Vector3(-_position, 0)) *
                      Matrix.CreateScale(_zoom) *
                      Matrix.CreateRotationZ(_rotation) *
                      Matrix.CreateTranslation(new Vector3(_size / 2, 0));
            }
        }
        public Vector2 MinPosition
        {
            get { return _minPosition; }
            set { _minPosition = value; }
        }
        public Vector2 MaxPosition
        {
            get { return _maxPosition; }
            set { _maxPosition = value; }
        }
        public Body TrackingBody
        {
            get { return _trackingBody; }
            set { _trackingBody = value; }
        }

        public void Update(KeyboardState kbCurrent, KeyboardState kbPrevious)//GamePadState state)
        {
            /*if (_trackingBody == null)
            {
                if (_minPosition.X != _maxPosition.X || _minPosition.Y != _maxPosition.Y)
                    _position = Vector2.Clamp(_position + new Vector2(state.ThumbSticks.Right.X * _zoom, -state.ThumbSticks.Right.Y * _zoom), _minPosition, _maxPosition);
                else
                    _position += new Vector2(state.ThumbSticks.Right.X * _zoom, -state.ThumbSticks.Right.Y * _zoom);
            }
            else
            {*/
                if (_minPosition.X != _maxPosition.X || _minPosition.Y != _maxPosition.Y)
                    _position = Vector2.Clamp(_trackingBody.Position, _minPosition, _maxPosition);
                else
                    _position = _trackingBody.Position;
            if (kbCurrent.IsKeyDown(Keys.R) & kbPrevious.IsKeyUp(Keys.R))
            {
                if (rotate)
                    Rotation = 0;
                rotate = !rotate;
            }
            if (rotate)
                _rotation = -_trackingBody.Rotation;
            if (kbCurrent.IsKeyDown(Keys.Add))
            {
                _zoom = Math.Min(_maxZoom, _zoom + _zoomRate);
            }
            if (kbCurrent.IsKeyDown(Keys.Subtract))
            {
                _zoom = Math.Max(_minZoom, _zoom - _zoomRate);
            }
            //}
            /*if (state.DPad.Up == ButtonState.Pressed)
                _zoom = Math.Min(_maxZoom, _zoom + _zoomRate);
            if (state.DPad.Down == ButtonState.Pressed)
                _zoom = Math.Max(_minZoom, _zoom - _zoomRate);*/
            //uncomment this to enable rotation
            /*
            if (state.DPad.Left == ButtonState.Pressed)
                _rotation = (_rotation + _rotationRate) % (float)(Math.PI * 2);
            if (state.DPad.Right == ButtonState.Pressed)
                _rotation = (_rotation - _rotationRate) % (float)(Math.PI * 2);
             */
            /*if (state.Buttons.RightStick == ButtonState.Pressed)
                ResetCamera();*/
        }

        private void ResetCamera()
        {
            _position = _origPosition;
            _rotation = _origRotation;
            _zoom = _origZoom;
            _trackingBody = null;
        }
    }
}