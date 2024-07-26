using UnityEngine;
using UnitySensors.Data.Texture;

namespace UnitySensors.Sensor.Camera {
    public class HSVCameraSensor : CameraSensor {

        [SerializeField] private float hueMin;
        [SerializeField] private float hueMax;
        [SerializeField] private float saturationMin;
        [SerializeField] private float saturationMax;
        [SerializeField] private float valueMin;
        [SerializeField] private float valueMax;

        private Texture2D filteredImage;

        public Texture2D mask { get => filteredImage; }

        private Material mat;

        // private Color b = new Color(0, 0, 0, 0);

        protected override void Init()
        {
            base.Init();
            mat = new Material(Shader.Find("UnitySensors/HSVFilter"));
            filteredImage = new Texture2D(resolution.x, resolution.y, TextureFormat.Alpha8, false);
        }

        protected override void UpdateSensor()
        {
            // Get the RGB image from the unity camera
            if (!LoadTexture()) return;
            if (onSensorUpdated != null)
                onSensorUpdated.Invoke();


            
        
            // Texture2D rgbImage = texture; 
            // Graphics.Blit (texture, destination, material);


            // // Filter the image based on HSV values
            // for (int y = 0; y < rgbImage.height; y++)
            // {
            //     for (int x = 0; x < rgbImage.width; x++)
            //     {
            //         Color pixelColor = rgbImage.GetPixel(x, y);
            //         float h;
            //         float s;
            //         float v;
            //         Color.RGBToHSV(pixelColor, out h, out s, out v);

            //         // Check if the pixel falls within the specified HSV range
            //         if (h >= hueMin && h <= hueMax &&
            //             s >= saturationMin && s <= saturationMax &&
            //             v >= valueMin && v <= valueMax)
            //         {
            //             filteredImage.SetPixel(x, y, Color.white);
            //         }
            //         else
            //         {
            //             filteredImage.SetPixel(x, y, Color.black);
            //         }
            //     }
            // }

            // // Apply the changes to the filtered image texture
            // filteredImage.Apply();
            

        }

        private void OnRenderImage(RenderTexture source, RenderTexture dest)
        {

            mat.SetFloat("_HueMin", hueMin);
            mat.SetFloat("_HueMax", hueMax);
            mat.SetFloat("_SaturationMin", saturationMin);
            mat.SetFloat("_SaturationMax", saturationMax);
            mat.SetFloat("_ValueMin", valueMin);
            mat.SetFloat("_ValueMax", valueMax);
            Graphics.Blit(source, dest, mat);
        }
    }
}