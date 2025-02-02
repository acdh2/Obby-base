Shader "Unlit/Spawn"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                //o.uv = v.uv;//mul(unity_ObjectToWorld, v.vertex + float3(0.5, 0.5, 0.5)).xz;
                // Extract object scale from unity_ObjectToWorld matrix
                float3 scaleX = float3(unity_ObjectToWorld._m00, unity_ObjectToWorld._m10, unity_ObjectToWorld._m20);
                float3 scaleZ = float3(unity_ObjectToWorld._m02, unity_ObjectToWorld._m12, unity_ObjectToWorld._m22);

                float scaleXLength = -length(scaleX);
                float scaleZLength = -length(scaleZ);

                // Scale the UVs accordingly
                o.uv = v.uv * float2(scaleXLength, scaleZLength);                

                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.normal = v.normal;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture            
                fixed4 col;
                if (i.normal.y > 0) {    
                    col = tex2D(_MainTex, i.uv);
                } else {
                    col = tex2D(_MainTex, fixed2(0, 0));
                }
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
