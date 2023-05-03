// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PixelCamera"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_Resolution("Resolution", Range( 1 , 500)) = 50
		_NoiseSize("NoiseSize", Range( 0 , 1000)) = 65.53836
		_NoiseIntensity("NoiseIntensity", Range( -1 , 1)) = 0.1
		_LifePercent("LifePercent", Range( 0 , 100)) = 100

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float _NoiseSize;
			uniform float _NoiseIntensity;
			uniform float _Resolution;
			uniform float _LifePercent;
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 temp_cast_0 = (_NoiseSize).xx;
				float2 texCoord38 = i.uv.xy * temp_cast_0 + float2( 0,0 );
				float simplePerlin2D37 = snoise( ( texCoord38 + _Time.y ) );
				simplePerlin2D37 = simplePerlin2D37*0.5 + 0.5;
				float2 temp_cast_1 = (_Time.y).xx;
				float simplePerlin2D42 = snoise( ( texCoord38 - temp_cast_1 ) );
				simplePerlin2D42 = simplePerlin2D42*0.5 + 0.5;
				float NoiseEffect50 = ( (0.0 + (( simplePerlin2D37 + simplePerlin2D42 ) - 0.0) * (0.5 - 0.0) / (1.0 - 0.0)) * _NoiseIntensity );
				float2 texCoord4 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float pixelWidth3 =  1.0f / ( 1.5 * _Resolution );
				float pixelHeight3 = 1.0f / ( 1.0 * _Resolution );
				half2 pixelateduv3 = half2((int)(texCoord4.x / pixelWidth3) * pixelWidth3, (int)(texCoord4.y / pixelHeight3) * pixelHeight3);
				float4 PixelEffect49 = tex2D( _MainTex, pixelateduv3 );
				float LifePercentValue73 = (0.0 + (-( _LifePercent / 100.0 ) - -1.0) * (1.0 - 0.0) / (0.0 - -1.0));
				float3 desaturateInitialColor68 = ( NoiseEffect50 + PixelEffect49 ).rgb;
				float desaturateDot68 = dot( desaturateInitialColor68, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar68 = lerp( desaturateInitialColor68, desaturateDot68.xxx, LifePercentValue73 );
				

				finalColor = float4( desaturateVar68 , 0.0 );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18900
0;0;1366;707;4313.655;55.96112;1.00601;True;False
Node;AmplifyShaderEditor.RangedFloatNode;39;-3988.472,148.18;Inherit;False;Property;_NoiseSize;NoiseSize;3;0;Create;True;0;0;0;False;0;False;65.53836;300;0;1000;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;41;-3575.172,298.295;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;-3670.917,113.6398;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;43;-3372.46,231.4218;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-3369.631,102.9015;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;37;-3171.464,56.88349;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-3568,896;Inherit;False;Constant;_PixelsX;PixelsX;0;0;Create;True;0;0;0;False;0;False;1.5;0;150;150;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;42;-3168.508,289.4834;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-3568,1120;Inherit;False;Property;_Resolution;Resolution;2;0;Create;True;0;0;0;False;0;False;50;275;1;500;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-3565,988;Inherit;False;Constant;_PixelsY;PixelsY;1;0;Create;True;0;0;0;False;0;False;1;500;100;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-3363.769,709.644;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;69;-3604.418,1419.746;Inherit;False;Property;_LifePercent;LifePercent;5;0;Create;False;0;0;0;False;0;False;100;100;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-2848.354,155.5076;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-3214,1004;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-3224,874;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;45;-2561.852,38.65954;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-2539.422,293.8943;Inherit;False;Property;_NoiseIntensity;NoiseIntensity;4;0;Create;True;0;0;0;False;0;False;0.1;-0.2;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;70;-3291.381,1393.55;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-2944,720;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCPixelate;3;-2976,896;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;71;-3141.815,1386.754;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2720,752;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-2167.806,74.22683;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-2290,869;Inherit;False;PixelEffect;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;72;-2966.81,1406.129;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;50;-1953.42,113.2858;Inherit;False;NoiseEffect;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;-989.2962,22.41144;Inherit;False;49;PixelEffect;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;-982.3959,-104.5597;Inherit;False;50;NoiseEffect;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;73;-2738.476,1414.605;Float;False;LifePercentValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;74;-530.8373,267.5055;Inherit;False;73;LifePercentValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-659.6116,-45.07208;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-2581.076,-545.8412;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;68;-283.9755,-38.60804;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;9;-2337.282,-580.0262;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-2929.026,-354.4478;Inherit;False;Property;_FrequencyLines;FrequencyLines;0;0;Create;True;0;0;0;False;0;False;5;10;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;20;-3126.579,-733.8857;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;53;-2110.754,-621.4833;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.025;False;4;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-3363.199,-425.9434;Inherit;False;Property;_TimeScaleLines;TimeScaleLines;1;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-1788.997,-599.77;Inherit;False;ScaneLines;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;-986.885,126.0629;Inherit;False;56;ScaneLines;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;14;-3017.919,-456.7231;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-2793.346,-555.9008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;378.2302,-2.831953;Float;False;True;-1;2;ASEMaterialInspector;0;2;PixelCamera;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;38;0;39;0
WireConnection;43;0;38;0
WireConnection;43;1;41;0
WireConnection;40;0;38;0
WireConnection;40;1;41;0
WireConnection;37;0;40;0
WireConnection;42;0;43;0
WireConnection;44;0;37;0
WireConnection;44;1;42;0
WireConnection;36;0;6;0
WireConnection;36;1;34;0
WireConnection;35;0;5;0
WireConnection;35;1;34;0
WireConnection;45;0;44;0
WireConnection;70;0;69;0
WireConnection;3;0;4;0
WireConnection;3;1;35;0
WireConnection;3;2;36;0
WireConnection;71;0;70;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;47;0;45;0
WireConnection;47;1;48;0
WireConnection;49;0;2;0
WireConnection;72;0;71;0
WireConnection;50;0;47;0
WireConnection;73;0;72;0
WireConnection;46;0;51;0
WireConnection;46;1;52;0
WireConnection;55;0;13;0
WireConnection;55;1;11;0
WireConnection;68;0;46;0
WireConnection;68;1;74;0
WireConnection;9;0;55;0
WireConnection;53;0;9;0
WireConnection;56;0;53;0
WireConnection;14;0;15;0
WireConnection;13;0;20;2
WireConnection;13;1;14;0
WireConnection;0;0;68;0
ASEEND*/
//CHKSM=0ECB21C4C4E72FCF0CDBFFCF035CB646EFD8D258