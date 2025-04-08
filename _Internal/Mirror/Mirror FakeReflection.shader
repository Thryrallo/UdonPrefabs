// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Thry/MirrorReflection Metallic"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Roughness("Roughness", Range(0.0, 1.0)) = 0.0
        [HideInInspector] _ReflectionTex0("", 2D) = "white" {}
        [HideInInspector] _ReflectionTex1("", 2D) = "white" {}
    }
    SubShader
    {
        Tags{ "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityInstancing.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _ReflectionTex0;
            sampler2D _ReflectionTex1;

            float _Roughness;

            struct appdata 
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 refl : TEXCOORD1;
                float4 pos : SV_POSITION;
                
                float3 worldPos : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.refl = ComputeNonStereoScreenPos(o.pos);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                return o;
            }

            float3 BoxProjection(float3 direction, float3 position, float4 cubemapPosition, float3 boxMin, float3 boxMax)
            {
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    //UNITY_BRANCH
                    if (cubemapPosition.w > 0)
                    {
                        float3 factors = ((direction > 0 ? boxMax : boxMin) - position) / direction;
                        float scalar = min(min(factors.x, factors.y), factors.z);
                        direction = direction * scalar + (position - cubemapPosition.xyz);
                    }
                #endif
                return direction;
            }

            half4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                // half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                // half3 reflection = reflect(-worldViewDir, i.worldNormal);
                // float3 reflDir = reflection; 

                // reflDir = BoxProjection(reflDir, i.worldPos, unity_SpecCube0_ProbePosition, unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax);
                // float4 envSample0 = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflDir, _Roughness);
                // float3 skyColor = DecodeHDR(envSample0, unity_SpecCube0_HDR);

                half4 refl = unity_StereoEyeIndex == 0 ? tex2Dproj(_ReflectionTex0, UNITY_PROJ_COORD(i.refl)) : tex2Dproj(_ReflectionTex1, UNITY_PROJ_COORD(i.refl));
                return refl;
                // return float4(refl.rgb * refl.a + skyColor * (1- refl.a), 1);
            }
            ENDCG
        }
    }
}
