Shader "Unlit/StrengthIndicator"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white"
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

			Blend SrcAlpha OneMinusSrcAlpha

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
			sampler2D _MainTex;

			float Unity_RoundedRectangle_float(float2 UV, float Width, float Height, float Radius, float ratio)
			{
				ratio = ratio * _ScreenParams.x / _ScreenParams.y;

				Radius = max(min(min(abs(Radius * 2), abs(Width)), abs(Height)), 1e-5);
				float2 uv = abs(UV * 2 - 1) * float2(ratio,1) - float2(Width*ratio, Height) + Radius;
				float d = length(max(0, uv)) / Radius;
				return saturate((1 - d) / fwidth(d));
			}


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformToScreenMain(v.uv);
                o.uv = v.uv;
                return o;
            }

#define BORDER_WIDTH 0.15

            fixed4 frag (v2f i) : SV_Target
            {
				clip(_Strength - 0.01f);
				float4 col = lerp(float4(1,0,0,1), float4(0,1,0,1),_Strength);
				col.rgb = col.rgb * (i.uv.x < _Strength);

				float2 quadUV = i.uv - float2(0.5, 0.5);
				col.a *= Unity_RoundedRectangle_float(quadUV + float2(0.5,0.5),1,1,0.3,7.5);
				float innerQuad = Unity_RoundedRectangle_float(quadUV * float2(1 + BORDER_WIDTH *0.133 / 2, 1 + BORDER_WIDTH) + float2(0.5, 0.5), 1, 1, 0.3*(1- BORDER_WIDTH), 7.5);
				col.a *= lerp(0.7,0.5, innerQuad);
				col.rgb = lerp(0, col, innerQuad);

				float4 textureColor = tex2D(_MainTex, float2(2 * i.uv.x - 0.5, 1- i.uv.y));
				col.rgb = lerp(col.rgb, textureColor.rgb, textureColor.a);
                return col;
            }
            ENDCG
        }
    }
}
