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

        private Color b = new Color(0, 0, 0, 0);

        protected override void Init()
        {
            base.Init();

            filteredImage = new Texture2D(resolution.x, resolution.y, TextureFormat.Alpha8, false);
        }

        private void UpdateSensor()
        {
            // Get the RGB image from the unity camera

            // Texture2D rgbImage = texture;

            // Filter the image based on HSV values
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Color pixelColor = texture.GetPixel(x, y);
                    float h;
                    float s;
                    float v;
                    Color.RGBToHSV(pixelColor, out h, out s, out v);

                    // Check if the pixel falls within the specified HSV range
                    if (h >= hueMin && h <= hueMax &&
                        s >= saturationMin && s <= saturationMax &&
                        v >= valueMin && v <= valueMax)
                    {
                        texture.SetPixel(x, y, Color.clear);
                    }
                    else
                    {
                        texture.SetPixel(x, y, b);
                    }
                }
            }

            // Apply the changes to the filtered image texture
            texture.Apply();

        }
    }
}