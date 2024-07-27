using UnityEngine;
using UnitySensors.Data.Texture;

namespace UnitySensors.Sensor.Camera {
    public class TagBasedCameraSensor : CameraSensor {

        [SerializeField] private string tagToRender = "Filter";
        private Material mat;

        protected override void Init()
        {
            base.Init();
            mat = new Material(Shader.Find("UnitySensors/TagBasedFilter"));
        }

        private void OnRenderImage(RenderTexture source, RenderTexture dest)
        {
            // Create a temporary render texture
            RenderTexture tempRT = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
            Graphics.Blit(source, tempRT);

            // Find all objects with the specified tag
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tagToRender);
            foreach (GameObject obj in taggedObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    RenderTexture currentRT = RenderTexture.active;
                    RenderTexture.active = tempRT;

                    GL.PushMatrix();
                    GL.LoadOrtho();
                    mat.SetFloat("_TagValue", 1.0f); // Set Tag Value to 1 for white
                    mat.SetPass(0);
                    GL.Begin(GL.QUADS);
                    GL.TexCoord2(0, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.TexCoord2(1, 0);
                    GL.Vertex3(1, 0, 0);
                    GL.TexCoord2(1, 1);
                    GL.Vertex3(1, 1, 0);
                    GL.TexCoord2(0, 1);
                    GL.Vertex3(0, 1, 0);
                    GL.End();
                    GL.PopMatrix();

                    RenderTexture.active = currentRT;
                }
            }

            Graphics.Blit(tempRT, dest);
            RenderTexture.ReleaseTemporary(tempRT);
        }
    }
}
