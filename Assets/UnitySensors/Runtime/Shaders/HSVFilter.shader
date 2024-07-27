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
        _NumFilters ("Number of Filters", Range(1, 3)) = 1
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
            float4 _HueMin;
            float4 _HueMax;
            float4 _SatMin;
            float4 _SatMax;
            float4 _ValMin;
            float4 _ValMax;
            int _NumFilters;

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

            bool InRange(float3 hsv, float3 minHSV, float3 maxHSV, float tolerance)
            {
                return hsv.x >= minHSV.x - tolerance && hsv.x <= maxHSV.x + tolerance &&
                       hsv.y >= minHSV.y - tolerance && hsv.y <= maxHSV.y + tolerance &&
                       hsv.z >= minHSV.z - tolerance && hsv.z <= maxHSV.z + tolerance;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                float3 hsv = RGBToHSV(col.rgb);

                float tolerance = 0.01;
                bool filterOut = false;

                for (int j = 0; j < _NumFilters; ++j)
                {
                    if (InRange(hsv, float3(_HueMin[j], _SatMin[j], _ValMin[j]), 
                                    float3(_HueMax[j], _SatMax[j], _ValMax[j]), tolerance))
                    {
                        filterOut = true;
                        break;
                    }
                }

                return filterOut ? float4(0, 0, 0, 0) : col;
            }
            ENDCG
        }
    }
}
