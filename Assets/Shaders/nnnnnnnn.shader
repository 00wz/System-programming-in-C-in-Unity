Shader "Custom/UnlitTextureMix"
{
    Properties
    {
        _Tex1 ("Texture1", 2D) = "white" {}
        _Tex2 ("Texture2", 2D) = "white" {}
        _MixValue("MixValue", Range(0,1)) = 0.5
        _Color("Main Color", COLOR) = (1,1,1,1)
        _Height("Height", Range(0,2)) = 0.5 // ���� ������

    }
    SubShader{
			Tags{"Queue" = "TransParent""IngnorProjector" = "True""RenderType" = "TransParent"}// ���������� ������� ����������
				Pass{
				Tags{"LightMode" = "ForwardBase"}
			
				ZWrite Off// ������� zwrite.
			Blend SrcAlpha OneMinusSrcAlpha// ������� ��������� �������
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members RdotV)
#pragma exclude_renderers d3d11
            #pragma vertex vert // ��������� ��� ��������� ������
            #pragma fragment frag // ��������� ��� ��������� ����������
            #include "UnityCG.cginc" // ���������� � ��������� ���������
            sampler2D _Tex1; // ��������1
            float4 _Tex1_ST;
            sampler2D _Tex2; // ��������2
            float4 _Tex2_ST;
            float _MixValue; // �������� ����������
            float4 _Color; // ����, ������� ����� ������������ �����������
            float _Height; // ���� ������

            // ���������, ������� �������� ������������� ������ ������� � ������ ���������
            struct v2f
            {
                float2 uv : TEXCOORD0; // UV-���������� �������
                float4 vertex : SV_POSITION; // ���������� �������
            };

            //����� ���������� ��������� ������
            v2f vert (appdata_full v)
            {
                v2f result;

                 // Normal in WorldSpace
                float3 worldNormal = UnityObjectToWorldNormal(v.normal.xyz);
                 // World position
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                // Camera direction
                float3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos.xyz));
                float RdotV = max(0., dot(worldNormal, viewDir));

                result.vertex = UnityObjectToClipPos(v.vertex);
                result.uv = TRANSFORM_TEX(v.texcoord, _Tex1);
                return result;
            }

            //����� ���������� ��������� ��������, ���� �������� ���������� �� ���� ���������
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color;
                color = tex2D(_Tex1, i.uv) * _MixValue;
                color += tex2D(_Tex2, i.uv) * (1 - _MixValue);
                color = color * _Color;
                color.w*=_Height;
                return color;
            }
            ENDCG
        }


    }
    FallBack "Diffuse"
}