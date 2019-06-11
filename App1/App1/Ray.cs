
using System;

namespace App1
{
    public class Ray
    {
        private Vector3 _A;
        private Vector3 _B;
        private double _length;
        public Vector3 Origin
        {
            get
            {
                return this._A;
            }
            set
            {
                this._A = value;
            }
        }
        public Vector3 Destination
        {
            get
            {
                return this._B;
            }
            set
            {
                this._B = value;
            }
        }

        public double Length
        {
            get
            {
                return this._length;
            }
            set
            {
                this._length = value;
            }
        }

        public Ray() { }
        public Ray(Vector3 a,Vector3 b, double length)
        {
            this._A = a;
            this._B = b;
            this._length = length;
        }


        public Vector3 Point_to_parameter(double t) { return this._A + (this._B * t) ; }
        public Vector3 Point_to_parameter(ref double t) { return this._A + (this._B * t); }

    }
    public class Camera
    {
        private Vector3 _p0, _p1, _p2;
        private Vector3 _camPos, _viewDir, _screenCenter;

        public Camera(double screend, double aw, double ah) {
            _camPos = new Vector3(0.0, 0.0, 0.0);
            _viewDir = new Vector3(0.0, 0.0, 1.0);
            _screenCenter = _camPos + _viewDir * screend;
            _p0 = _screenCenter + new Vector3(-aw, ah, 0);
            _p1 = _screenCenter + new Vector3(aw, ah, 0);
            _p2 = _screenCenter + new Vector3(-aw, -ah, 0);

        }
        public Ray Get_ray(ref double u, ref double v, double length)
        {
            Vector3 p = _p0 + (_p1 - _p0) * u + (_p2 - _p0) * v;
            p = p - _camPos;
            return new Ray(_camPos, p.Normalized(), length);
        }
    }
}
