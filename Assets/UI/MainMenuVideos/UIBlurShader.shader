Shader "Custom/UIBlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 0.01)) = 0.001
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0,0,0,0);
                float2 offsets[9] =
                {
                    float2(-_BlurSize,  _BlurSize),
                    float2(0,         _BlurSize),
                    float2(_BlurSize,  _BlurSize),
                    float2(-_BlurSize,  0),
                    float2(0,         0),
                    float2(_BlurSize,  0),
                    float2(-_BlurSize, -_BlurSize),
                    float2(0,        -_BlurSize),
                    float2(_BlurSize, -_BlurSize)
                };

                for(int j = 0; j < 9; j++)
                {
                    col += tex2D(_MainTex, i.uv + offsets[j]);
                }
                
                return (col / 9.0) * i.color;
            }
            ENDCG
        }
    }
}