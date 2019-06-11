using System;
using System.Diagnostics;
using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Threading.Tasks;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        bool pageIsActive;

        const int _res_x = 768, _res_y = 1280;

        SKBitmap image_result = new SKBitmap(_res_x, _res_y);

        SKCanvasView canvasView;
     
        private Hitable_list _hlist = new Hitable_list();

        private Random _rand = new Random(1337);

        private Camera _cam;

        private int _rebond = 50;

        public MainPage()
        {
            Title = "MyTests";

            InitializeComponent();

            _cam = new Camera(5,9,16);

            _hlist.Add(new Sphere(new Vector3(0, 0, 2), 1.0, new Lamberian(new Vector3(0.8,0.3,0.3))));
            _hlist.Add(new Sphere(new Vector3(0, -100, 20), 100.0, new Lamberian(new Vector3(0.8, 0.8, 0.0))));
            _hlist.Add(new Sphere(new Vector3(2, 0, 2), 1.0, new Metal(new Vector3(0.8, 0.6, 0.2), 0.0)));
            _hlist.Add(new Sphere(new Vector3(-2, 0, 2), 1.0, new Dielectric(1.5)));


            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            pageIsActive = true;
            await AnimationLoop();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
        }

        async Task AnimationLoop()
        {
            
            while (pageIsActive)
            {
                FillBitmapByteBuffer(ref image_result);
                canvasView.InvalidateSurface();
                await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
                break;
            }

            
        }

        private double Hit_sphere(ref Sphere sphere, ref Ray ray)
        {
            Vector3 dest = ray.Destination;
            Vector3 oc = ray.Origin - sphere.Position;
            double a = Vector3.Dot(ref dest, ref dest);
            double b = 2.0 * Vector3.Dot(ref oc, ref dest);
            double c = Vector3.Dot(ref oc, ref oc) - (sphere.Radius * sphere.Radius);
            double disc = b * b - 4 * a * c;
            if (disc < 0) return -1.0;
            return (-b - Math.Sqrt(disc)) / (2.0 * a);
        }

        private Vector3 Pixel_color(ref Ray ray, ref Hitable_list hlist, int depth)
        {
            Hit_record hrec = new Hit_record();
            Vector3 v;
            if (hlist.Hit(ref ray, 0.001, Double.MaxValue,ref hrec))
            {
                Ray scatered = new Ray();
                Vector3 attenuation = new Vector3(1.0,1.0,1.0);
                if(depth < this._rebond && hrec.mat_ref.Scatter(ref ray, ref hrec, ref attenuation, ref scatered))
                {
                    return attenuation * Pixel_color(ref scatered, ref hlist, depth + 1);
                }
                return new Vector3(0, 0, 0);
            }
            else
            {
                v = ray.Destination.Normalized();
                double t = 0.5 * (v[1] + 1.0);
                v = (1.0 - t) * new Vector3(1.0, 1.0, 1.0) + t * new Vector3(0.5, 0.7, 1.0);
                return v;
            }
        }

        private Vector3 Rand_in_unit_spthere()
        {
            Vector3 p = new Vector3(1.0,1.0,1.0), unit = new Vector3(1.0, 1.0, 1.0);
            do
            {
                p = 2.0 * new Vector3(_rand.NextDouble(), _rand.NextDouble(), _rand.NextDouble()) - unit;
            } while (p.Sqr_length() >= 1.0);
            return p;
        }

        private void FillBitmapByteBuffer(ref SKBitmap result)
        {

            byte[,,] buffer = new byte[_res_y, _res_x, 4];
            VectorRGBA pixel;
            Ray r;
            Vector3 vec;
            double u, v;
            for (int row = _res_y-1; row >= 0; row--)
                for (int col = 0; col < _res_x; col++)
                {
                    u = (double)col / (double)_res_x;
                    v = (double)row / (double)_res_y;
                    r = _cam.Get_ray(ref u, ref v, 10000000.0);
                    vec = Pixel_color(ref r, ref _hlist,0);
                    pixel = new VectorRGBA(Math.Sqrt(vec[0]), Math.Sqrt(vec[1]), Math.Sqrt(vec[2]));
                    buffer[row, col, 0] = (byte)pixel[0];   // red
                    buffer[row, col, 1] = (byte)pixel[1];   // green
                    buffer[row, col, 2] = (byte)pixel[2];   // blue
                    buffer[row, col, 3] = (byte)pixel[3];   // alpha
                }
            

            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    result.SetPixels((IntPtr)ptr);
                }
            }
        }



        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            int width = info.Width;
            int height = info.Height;

            canvas.Clear();
            canvas.DrawBitmap(image_result, new SKRect(0, 0, width, height));
        }
    }
}
