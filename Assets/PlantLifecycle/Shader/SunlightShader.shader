Shader "Unlit/SunlightShader"
{
    Properties   //just need to make a shader that controls the x or y transparency of the image by a slider...
    {
        _MainTex ("Texture", 2D) = "white" {}
        _IsXAxis("Is X Axis", Float) = 0 
        _xPos ("x-Pos", Range(1, 0)) = 0.5
        _Alpha ("Alpha", Range(0, 1)) = 0.5
        _Edge ("Edge", Range(0, 1)) = 0.5
        _Color("Color",Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent" 
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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
            float4 _MainTex_ST;
            bool _IsXAxis;
            float _xPos;
            float _Alpha;
            float _Edge;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // return col;

                float distance=0;

                if(_IsXAxis == 1){
                    distance = length(i.uv.x);
                }
                else{
                    distance = length(i.uv.y);
                }

                // float alpha = (i.uv.x * _Transparency);

                float alpha = smoothstep(_xPos - _Edge, _xPos + _Edge, distance) * _Alpha;

                // col.a *= saturate(alpha);

                clip(alpha);

                return float4(_Color.xyz, alpha);

            }
            ENDCG
        }
    }
}
