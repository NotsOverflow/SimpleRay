using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    public class Vector3
    {
        private double[] xyz = new double[3];

        public Vector3(double x, double y, double z){ xyz[0] = x; xyz[1] = y; xyz[2] = z; }
        
        public double X() { return xyz[0]; }
        public double Y() { return xyz[1]; }
        public double Z() { return xyz[2]; }

        public static Vector3 operator +(Vector3 v) { return v; }
        public static Vector3 operator -(Vector3 v) { return new Vector3(-v.xyz[0], -v.xyz[1], -v.xyz[2]); }
        public double this[int i]
        {
            get { return this.xyz[i]; }
            set { this.xyz[i] = value; }
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2) {
            return new Vector3(v1[0] + v2[0], v1[1] + v2[1], v1[2] + v2[2]);
        }
        public static Vector3 operator +(Vector3 v1, double t)
        {
            return new Vector3(v1[0] + t, v1[1] + t, v1[2] + t);
        }
        public static Vector3 operator +(double t, Vector3 v1)
        {
            return new Vector3(v1[0] + t, v1[1] + t, v1[2] + t);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1[0] - v2[0], v1[1] - v2[1], v1[2] - v2[2]);
        }
        public static Vector3 operator -(double t, Vector3 v1)
        {
            return new Vector3(v1[0] - t, v1[1] - t, v1[2] - t);
        }
        public static Vector3 operator -(Vector3 v1, double t)
        {
            return new Vector3(v1[0] - t, v1[1] - t, v1[2] - t);
        }
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1[0] * v2[0], v1[1] * v2[1], v1[2] * v2[2]);
        }
        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1[0] / v2[0], v1[1] / v2[1], v1[2] / v2[2]);
        }
        public static Vector3 operator *(Vector3 v1, double t)
        {
            return new Vector3(v1[0] * t, v1[1] * t, v1[2] * t);
        }
        public static Vector3 operator /(Vector3 v1, double t)
        {
            return new Vector3(v1[0] / t, v1[1] / t, v1[2] / t);
        }
        public static Vector3 operator *( double t, Vector3 v1)
        {
            return new Vector3(v1[0] * t, v1[1] * t, v1[2] * t);
        }
        public static Vector3 operator /(double t, Vector3 v1)
        {
            return new Vector3(v1[0] / t, v1[1] / t, v1[2] / t);
        }


        public double Length() { return (double)Math.Sqrt( this[0]*this[0] + this[1]*this[1] + this[2]*this[2] ); }
        public double Sqr_length() { return this[0] * this[0] + this[1] * this[1] + this[2] * this[2] ; }
        public static Vector3 Normalized(Vector3 v)
        {
            return v / v.Length();
        }
        public Vector3 Normalized()
        {
            return this / this.Length();
        }
        public static double Dot(ref Vector3 v1,ref Vector3 v2)
        {
            return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2] ;
        }
        public static Vector3 Cross(ref Vector3 v1,ref Vector3 v2)
        {
            return new Vector3(
                  v1[1] * v2[2] - v1[2] * v2[1],
                -(v1[0] * v2[2] - v1[2] * v2[0]),
                  v1[0] * v2[1] - v1[1] * v2[0]
            );
        }

    }
    public class VectorRGBA
    {
        private int[] rgba = new int[4];

        public VectorRGBA(int r, int g, int b, int a){ rgba[0] = this.normalize(r); rgba[1] = this.normalize(g); rgba[2] = this.normalize(b); rgba[3] = this.normalize(a); }
        public VectorRGBA(int r, int g, int b) { rgba[0] = this.normalize(r); rgba[1] = this.normalize(g); rgba[2] = this.normalize(b); rgba[3] = 255; }
        public VectorRGBA(double r, double g, double b, double a) { rgba[0] = this.normalize(r); rgba[1] = this.normalize(g); rgba[2] = this.normalize(b); rgba[3] = this.normalize(a); }
        public VectorRGBA(double r, double g, double b) { rgba[0] = this.normalize(r); rgba[1] = this.normalize(g); rgba[2] = this.normalize(b); rgba[3] = 255; }
        public VectorRGBA(ref Vector3 v) { rgba[0] = this.normalize(v[0]); rgba[1] = this.normalize(v[1]); rgba[2] = this.normalize(v[2]); rgba[3] = 255; }
        public VectorRGBA() { rgba[0] = 255; rgba[1] = 255; rgba[2] = 255; rgba[3] = 255; }

        public int normalize(int value)
        {
            if (value > 255) return 255;
            else if (value < 0) return 0;
            else return value;
        }
        public int normalize(double value)
        {
            return this.normalize((int)(255.99*value));
        }

        public int r() { return rgba[0]; }
        public int g() { return rgba[1]; }
        public int b() { return rgba[2]; }
        public int a() { return rgba[2]; }

        public int this[int i]
        {
            get { return this.rgba[i]; }
            set { this.rgba[i] = this.normalize(value); }
        }
    }
}
