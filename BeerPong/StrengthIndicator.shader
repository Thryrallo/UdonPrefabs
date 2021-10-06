Shader "Unlit/StrengthIndicator"
{
    Properties
    {
        _Strength("Strength", Float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Overlay" }
        LOD 100

        Pass
        {
			ZTest Always
			ZWrite Off
			CULL Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			float4 TransformToScreenMain(float2 uv) {
				#if defined(USING_STEREO_MATRICES)
					return float4(0, 0, 0, 0);
				#else
					float4 pos = float4(uv.xy * float2(1.5,0.2f) - float2(0.75f,-0.5), 0, 1); //moving to pixel position
					pos.y = ((_ProjectionParams.x < 0) * 2 - 1) * pos.y; //flip y if projection is flipped
					return pos;
				#endif
			}

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

			float _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformToScreenMain(v.uv);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				clip(_Strength - 0.01f);
				float4 col = lerp(float4(1,0,0,1), float4(0,1,0,1),_Strength);
				col.rgb = col.rgb * (i.uv.x < _Strength);
                return col;
            }
            ENDCG
        }
    }
}
