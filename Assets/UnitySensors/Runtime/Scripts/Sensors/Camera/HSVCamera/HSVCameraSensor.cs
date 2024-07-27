using UnityEngine;
using UnitySensors.Data.Texture;

namespace UnitySensors.Sensor.Camera {
    public class HSVCameraSensor : CameraSensor {

        [System.Serializable]
        public struct HSVRange
        {
            public float hueMin;
            public float hueMax;
            public float saturationMin;
            public float saturationMax;
            public float valueMin;
            public float valueMax;
        }

        [SerializeField] private HSVRange[] hsvRanges;

        private Material mat;

        protected override void Init()
        {
            base.Init();
            mat = new Material(Shader.Find("UnitySensors/HSVFilter"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture dest)
        {
            // Ensure that the number of filters does not exceed the array size defined in the shader
            int numFilters = Mathf.Clamp(hsvRanges.Length, 1, 3);
            mat.SetInt("_NumFilters", numFilters);

            for (int i = 0; i < numFilters; i++)
            {
                mat.SetFloat($"_HueMin[{i}]", hsvRanges[i].hueMin / 255.0f);
                mat.SetFloat($"_HueMax[{i}]", hsvRanges[i].hueMax / 255.0f);
                mat.SetFloat($"_SatMin[{i}]", hsvRanges[i].saturationMin / 255.0f);
                mat.SetFloat($"_SatMax[{i}]", hsvRanges[i].saturationMax / 255.0f);
                mat.SetFloat($"_ValMin[{i}]", hsvRanges[i].valueMin / 255.0f);
                mat.SetFloat($"_ValMax[{i}]", hsvRanges[i].valueMax / 255.0f);
            }

            Graphics.Blit(source, dest, mat);
        }
    }
}
