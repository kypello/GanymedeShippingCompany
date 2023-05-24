Shader "Custom/StarShader"
{
    Properties
    {
        _MainTex ("Stars", 2D) = "white" {}
        _PinkNebula ("PinkNebula", 2D) = "white" {}
        _PinkNebulaWarp ("PinkNebulaWarp", 2D) = "white" {}
        _PinkNebulaWarpAmount ("PinkNebulaWarpAmount", Float) = 0.0
        _BlueNebula("BlueNebula", 2D) = "white" {}
        _BlueNebulaWarp("BlueNebulaWarp", 2D) = "white" {}
        _BlueNebulaWarpAmount("BlueNebulaWarpAmount", Float) = 0.0
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv_pinkneb : TEXCOORD1;
                float2 uv_pinkneb_warp : TEXCOORD2;
                float2 uv_blueneb : TEXCOORD3;
                float2 uv_blueneb_warp : TEXCOORD4;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv_pinkneb : TEXCOORD1;
                float2 uv_pinkneb_warp : TEXCOORD2;
                float2 uv_blueneb : TEXCOORD3;
                float2 uv_blueneb_warp : TEXCOORD4;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _StarsOffset;
            float4 _StarsOffset_ST;

            sampler2D _PinkNebula;
            float4 _PinkNebula_ST;

            sampler2D _PinkNebulaWarp;
            float4 _PinkNebulaWarp_ST;
            float _PinkNebulaWarpAmount;

            sampler2D _BlueNebula;
            float4 _BlueNebula_ST;

            sampler2D _BlueNebulaWarp;
            float4 _BlueNebulaWarp_ST;
            float _BlueNebulaWarpAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv_pinkneb = TRANSFORM_TEX(v.uv, _PinkNebula);
                o.uv_pinkneb_warp = TRANSFORM_TEX(v.uv, _PinkNebulaWarp);
                o.uv_blueneb = TRANSFORM_TEX(v.uv, _BlueNebula);
                o.uv_blueneb_warp = TRANSFORM_TEX(v.uv, _BlueNebulaWarp);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 redNebulaColor = fixed4(0.75, 0.0, 0.5, 1.0);
                fixed4 blueNebulaColor = fixed4(0.0, 0.0, 0.75, 1);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);

                float nebulaWarpSample = tex2D(_PinkNebulaWarp, i.uv_pinkneb_warp);

                fixed4 nebulaSample = tex2D(_PinkNebula, i.uv_pinkneb + float2(nebulaWarpSample.r * _PinkNebulaWarpAmount, nebulaWarpSample.r * _PinkNebulaWarpAmount));
                float nebulaValue = nebulaSample.r - 0.25;
                

                float blueNebulaWarpSample = tex2D(_BlueNebulaWarp, i.uv_blueneb_warp);

                fixed4 blueNebulaSample = tex2D(_BlueNebula, i.uv_blueneb + float2(blueNebulaWarpSample.r * _BlueNebulaWarpAmount, blueNebulaWarpSample.r * _BlueNebulaWarpAmount));
                float blueNebulaValue = blueNebulaSample.r - 0.25;

                if (col.r > 0.85) {
                    //return col;
                    if (col.r < 0.87) {
                        return float4(0.5, 0.5, 0.5, 1);
                    }
                    return float4(1, 1, 1, 1);
                }

                

                return redNebulaColor * nebulaValue + blueNebulaColor * blueNebulaValue;
            }
            ENDCG
        }
    }
}
