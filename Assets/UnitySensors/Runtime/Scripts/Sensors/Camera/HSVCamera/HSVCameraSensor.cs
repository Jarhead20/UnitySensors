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

        private Material mat;	

        protected override void Init()	
        {	
            base.Init();	
            mat = new Material(Shader.Find("UnitySensors/HSVFilter"));	
            // filteredImage = new Texture2D(resolution.x, resolution.y, TextureFormat.Alpha8, false);	
        }	

        private void OnRenderImage(RenderTexture source, RenderTexture dest)	
        {	

            mat.SetFloat("_HueMin", (float)(hueMin/255.0));	
            mat.SetFloat("_HueMax", (float)(hueMax/255.0));	
            mat.SetFloat("_SaturationMin", (float)(saturationMin/255.0));	
            mat.SetFloat("_SaturationMax", (float)(saturationMax/255.0));	
            mat.SetFloat("_ValueMin", (float)(valueMin/255.0));	
            mat.SetFloat("_ValueMax", (float)(valueMax/255.0));	
            Graphics.Blit(source, dest, mat);	
        }	
    }	
}