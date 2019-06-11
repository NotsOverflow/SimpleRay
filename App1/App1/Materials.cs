using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    public abstract class Material
    {
        public abstract bool Scatter(ref Ray ray_in, ref Hit_record rec, ref Vector3 attenuation, ref Ray scatered);
    }
    public class Dielectric : Material
    {
        private double _ref_idx = 0.0;
        private Random _rand = new Random(1337);
        public Dielectric(double ri) { this._ref_idx = ri; }
        public override bool Scatter(ref Ray ray_in, ref Hit_record rec, ref Vector3 attenuation, ref Ray scatered)
        {
            Vector3 direction = ray_in.Destination, out_norm;

            Vector3 reflected = reflect(ref direction, ref rec.normal);
            double ni_over_nt;
            attenuation = new Vector3(1.0, 1.0, 1.0);
            Vector3 refracted = new Vector3(1.0,1.0,1.0);
            double cosine, ref_prob;
            if(Vector3.Dot(ref direction, ref rec.normal) > 0)
            {
                out_norm = -rec.normal;
                ni_over_nt = this._ref_idx;
                cosine = Vector3.Dot(ref direction, ref rec.normal) / ray_in.Destination.Length();
                cosine = Math.Sqrt(1 - this._ref_idx * this._ref_idx * (1 - cosine * cosine));
            }
            else
            {
                out_norm = rec.normal;
                ni_over_nt = 1.0 / this._ref_idx;
                cosine = - Vector3.Dot(ref direction, ref rec.normal) / ray_in.Destination.Length();
            }
            if(refract(ref direction, ref out_norm, ref ni_over_nt, ref refracted))
            {
                ref_prob = schlick(cosine, this._ref_idx);
            }
            else
            {
                ref_prob = 1.0;
            }
            if (this._rand.NextDouble() < ref_prob)
            {
                scatered = new Ray(rec.p, reflected, 10000000.0);
            }
            else
            {
                scatered = new Ray(rec.p, refracted, 10000000.0);
            }
            return true;
        }
        private bool refract(ref Vector3 v,ref Vector3 n,ref double ni_over_nt,ref Vector3 refracted)
        {
            Vector3 uv = v.Normalized();
            double dt = Vector3.Dot(ref uv, ref n);
            double disc = 1.0 - ni_over_nt * ni_over_nt * (1 - dt * dt);
            if(disc > 0)
            {
                refracted = ni_over_nt * (uv - n * dt) - n * Math.Sqrt(disc);
                return true;
            }
            return false;
        }
        private Vector3 reflect(ref Vector3 v, ref Vector3 n)
        {
            return v - 2 * Vector3.Dot(ref v,ref n) * n;
        }
        private double schlick(double cosine, double ref_idx)
        {
            double r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow((1 - cosine), 5.0);
        }
    }

    public class Lamberian : Material
    {


        private Random _rand = new Random(1337);
        private Vector3 _albedo;

        public Lamberian(Vector3 a) { _albedo = a; }

        public override bool Scatter(ref Ray ray_in, ref Hit_record rec, ref Vector3 attenuation, ref Ray scatered)
        {
            Vector3 target = rec.normal + Rand_in_unit_spthere();
            scatered = new Ray( rec.p, target, 10000.0);
            attenuation = _albedo;
            return true;
        }


        private Vector3 Rand_in_unit_spthere()
        {
            Vector3 p = new Vector3(1.0, 1.0, 1.0), unit = new Vector3(1.0, 1.0, 1.0);
            do
            {
                p = 2.0 * new Vector3(_rand.NextDouble(), _rand.NextDouble(), _rand.NextDouble()) - unit;
            } while (p.Sqr_length() >= 1.0);
            return p;
        }
    }
    public class Metal : Material
    {


        private Random _rand = new Random(1337);
        private Vector3 _albedo;
        private double _fuzz;

        public Metal(Vector3 a, double f) { _albedo = a; _fuzz = f < 1 ?  f : 1;  }

        public override bool Scatter(ref Ray ray_in, ref Hit_record rec, ref Vector3 attenuation, ref Ray scatered)
        {
            Vector3 tmp = Reflect(ray_in.Destination.Normalized(), rec.normal );
            scatered = new Ray(rec.p, tmp + _fuzz*Rand_in_unit_spthere(), 10000.0);
            tmp = scatered.Destination;
            attenuation = _albedo;
            return (Vector3.Dot(ref tmp,ref  rec.normal) > 0);
        }

        private Vector3 Reflect( Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(ref v, ref n) * n;
        }
        private Vector3 Rand_in_unit_spthere()
        {
            Vector3 p = new Vector3(1.0, 1.0, 1.0), unit = new Vector3(1.0, 1.0, 1.0);
            do
            {
                p = 2.0 * new Vector3(_rand.NextDouble(), _rand.NextDouble(), _rand.NextDouble()) - unit;
            } while (p.Sqr_length() >= 1.0);
            return p;
        }
    }
}
