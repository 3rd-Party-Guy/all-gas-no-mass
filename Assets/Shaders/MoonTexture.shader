// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Unlit/Moon" {
    Properties {
        _Tint ("Tint", Color) = (0, 0, 0, 1)
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Tint;

            struct appdata {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = ComputeScreenPos(o.vertex);

                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET {
                float2 texCoord = i.uv.xy / i.uv.w;
                float aspect = _ScreenParams.x / _ScreenParams.y;
                texCoord.x *= aspect;
                texCoord = TRANSFORM_TEX(texCoord, _MainTex);
                fixed4 col = tex2D(_MainTex, texCoord);

                col *= _Tint;

                return col;
            }


            ENDCG
        }
    }

    Fallback "VertexLit"
}