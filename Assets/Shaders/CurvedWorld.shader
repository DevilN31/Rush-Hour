// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/CurvedWorld" {
	Properties{
		// Diffuse texture
		_MainTex("Base (RGB)", 2D) = "white" {}
	// Degree of curvature
	_Curvature("Curvature", Float) = 0.001
	}
		SubShader{

		Tags{ "RenderType" = "Opaque" }
		/*Pass
		{*/

		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		// Surface shader function is called surf, and vertex preprocessor function is called vert
		// addshadow used to add shadow collector and caster passes following vertex modification
#pragma surface surf Lambert vertex:vert addshadow

		// Access the shaderlab properties
	/*	#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"*/

		uniform sampler2D _MainTex;
		uniform float _Curvature;

		// Basic input structure to the shader function
		// requires only a single set of UV texture mapping coordinates
			struct v2f {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				//float3 color : COLOR0;
			};
			struct Input {
				float2 uv_MainTex;
			};
			// This is where the curvature is applied
			void vert(inout appdata_full v)
			{
				// Transform the vertex coordinates from model space into world space
				float4 vv = mul(unity_ObjectToWorld, v.vertex);

				// Now adjust the coordinates to be relative to the camera position
				vv.xyz -= _WorldSpaceCameraPos.xyz;

				// Reduce the y coordinate (i.e. lower the "height") of each vertex based
				// on the square of the distance from the camera in the z axis, multiplied
				// by the chosen curvature factor
				vv = float4(0.0f, (vv.z * vv.z) * -_Curvature, 0.0f, 0.0f);

				// Now apply the offset back to the vertices in model space
				v.vertex += mul(unity_WorldToObject, vv);
			}
			//half4 frag(v2f i) : COLOR
			//{
			//	half4 col = tex2D(_MainTex,i.uv.xy);
			//	//			    if (col.r == _Color.r && col.g == _Color.g && col.b == _Color.b)
			//	//			    	col.a = col.a*_Alpha;
			//	//  col.w  = .2;//tex2D(_MainTex, i.uv.xy).a;
			//	return col; //half4(col.r,col.g,col.b , col.a );//col.a * _Alpha;
			//}
				// This is just a default surface shader
				void surf(Input IN, inout SurfaceOutput o) {
					half4 c = tex2D(_MainTex, IN.uv_MainTex);
					o.Albedo = c.rgb;
					o.Alpha = c.a;
				}
				ENDCG
		//}
	}
		

		// FallBack "Diffuse"
}