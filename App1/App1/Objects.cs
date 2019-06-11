using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    public struct Hit_record
    {
        public double t;
        public Vector3 p;
        public Vector3 normal;
        public Material mat_ref;
    }

    public abstract class Hitable
    {
        public abstract bool Hit(ref Ray ray, double f_min, double f_max, ref Hit_record rec);
    }
    public class Hitable_list : Hitable
    {
        private List<Hitable> _list = new List<Hitable> ();
        public Hitable_list() { }
        public override bool Hit(ref Ray ray, double t_min, double t_max, ref Hit_record rec)
        {
            Hit_record tmp = new Hit_record();
            bool did_hit = false;
            double closest_hit = t_max;
            foreach (var element in _list)
            {
                if (element.Hit(ref ray, t_min, closest_hit, ref tmp))
                {
                    did_hit = true;
                    closest_hit = tmp.t;
                    rec = tmp;
                }

            }
            return did_hit;
        }
        public void Add(Hitable obj)
        {
            _list.Add(obj);
        }
    }
    public class Sphere : Hitable
    {
        private Vector3 _position;
        private double _radius;
        private Material _material;
        public Vector3 Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }
        public double Radius
        {
            get
            {
                return this._radius;
            }
            set
            {
                this._radius = value;
            }
        }
        public Material Material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._material = value;
            }
        }
        public Sphere(Vector3 pos, double radius, Material mat)
        {
            this._position = pos;
            this._radius = radius;
            this._material = mat;
        }
        public override bool Hit(ref Ray ray, double t_min, double t_max, ref Hit_record rec)
        {
            Vector3 oc = ray.Origin - this.Position;
            Vector3 rdir = ray.Destination;
            double a = Vector3.Dot(ref rdir, ref rdir);
            double b = Vector3.Dot(ref oc, ref rdir);
            double c = Vector3.Dot(ref oc, ref oc) - this.Radius * this.Radius;
            double disc = b * b - a * c;
            if(disc > 0)
            {
                double tmp = (-b - Math.Sqrt(disc)) / a;
                if(tmp < t_max && tmp > t_min)
                {
                    rec.mat_ref = this._material;
                    rec.t = tmp;
                    rec.p = ray.Point_to_parameter(ref tmp);
                    rec.normal = (rec.p - this.Position) / this.Radius;
                    return true;
                }
                tmp = (-b + Math.Sqrt(disc)) / a;
                if(tmp < t_max && tmp > t_min)
                {
                    rec.mat_ref = this._material;
                    rec.t = tmp;
                    rec.p = ray.Point_to_parameter(ref tmp);
                    rec.normal = (rec.p - this.Position) / this.Radius;
                    return true;
                }
            }
            return false;
        }
    }
}
