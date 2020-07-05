Shader "Werplay/Curved/Alpha Shader" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0.5)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_QOffset ("Offset", Vector) = (-30,-30,0,0)
		_Dist ("Distance", Float) = 100.0
		_Alpha("Alpha", Range(0.0,1.0)) = 1.0
	}
	
	SubShader {
		//Tags { "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off
		 Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType" = "Transparent" }

		Pass
		{
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

            sampler2D _MainTex;
			float4 _QOffset;
			float _Dist;
			float _Alpha;
			float4 _Color;
			
			struct v2f {
			    float4 pos : SV_POSITION;
			    float4 uv : TEXCOORD0;
			    //float3 color : COLOR0;
			};

			v2f vert (appdata_base v)
			{
			    v2f o;
			    float4 vPos = mul (UNITY_MATRIX_MV, v.vertex);
			    float zOff = vPos.z/_Dist;
			    vPos += _QOffset*zOff*zOff;
			    o.pos = mul (UNITY_MATRIX_P, vPos);
			    o.uv = v.texcoord;
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
			    half4 col = tex2D(_MainTex,i.uv.xy);
//			    if (col.r == _Color.r && col.g == _Color.g && col.b == _Color.b)
//			    	col.a = col.a*_Alpha;
			  //  col.w  = .2;//tex2D(_MainTex, i.uv.xy).a;
			    return col * _Color; //half4(col.r,col.g,col.b , col.a );//col.a * _Alpha;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
