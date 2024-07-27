Shader "UnitySensors/HSVFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HueMin ("Hue Min", Range(0, 1)) = 0.0
        _HueMax ("Hue Max", Range(0, 1)) = 1.0
        _SatMin ("Saturation Min", Range(0, 1)) = 0.0
        _SatMax ("Saturation Max", Range(0, 1)) = 1.0
        _ValMin ("Value Min", Range(0, 1)) = 0.0
        _ValMax ("Value Max", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _HueMin;
            float _HueMax;
            float _SatMin;
            float _SatMax;
            float _ValMin;
            float _ValMax;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float3 RGBToHSV(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                float3 hsv = RGBToHSV(col.rgb);

                if (hsv.x >= _HueMin && hsv.x <= _HueMax &&
                    hsv.y >= _SatMin && hsv.y <= _SatMax &&
                    hsv.z >= _ValMin && hsv.z <= _ValMax)
                {
                    return float4(1, 1, 1, 1);;
                }
                else
                {
                    return float4(0, 0, 0, 0); // Filter out
                }
            }
            ENDCG
        }
    }
}