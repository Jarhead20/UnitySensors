Shader "UnitySensors/TagBasedFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TagValue ("Tag Value", Range(0, 1)) = 0.0
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
            float _TagValue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                if (_TagValue > 0.5)
                {
                    return float4(1, 1, 1, 1); // White
                }
                else
                {
                    return float4(0, 0, 0, 1); // Black
                }
            }
            ENDCG
        }
    }
}
