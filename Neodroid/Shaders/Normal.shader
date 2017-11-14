Shader "Neodroid/Normals" {
  SubShader {
    //Tags {
    //  "RenderType"="Opaque" // What RenderType shaders to replace there is a copy of this exact shame shader below with the "Transparent" RenderType
    //}

    Lighting Off // Turn off lighting
    Cull Off // No culling

    Pass {
      CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        // include file that contains UnityObjectToWorldNormal helper function
        #include "UnityCG.cginc"

        struct v2f {
            // we'll output world space normal as one of regular ("texcoord") interpolators
            //half3 worldNormal : TEXCOORD0;
            half3 cameraNormal : TEXCOORD0;
            float4 pos : SV_POSITION;
        };

        // vertex shader: takes object space normal as input too
        v2f vert (float4 vertex : POSITION, float3 normal : NORMAL) {
            v2f o;
            o.pos = UnityObjectToClipPos(vertex);
            o.cameraNormal = normal;
            // UnityCG.cginc file contains function to transform
            // normal from object to world space, use that
            //o.worldNormal = UnityObjectToWorldNormal(normal);
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target {
            fixed4 c = 0;
            // normal is a 3D vector with xyz components; in -1..1
            // range. To display it as color, bring the range into 0..1
            // and put into red, green, blue components
            c.rgb = i.cameraNormal*0.5+0.5;
            //return c;
            return fixed4(c.r, c.g, c.b, 1);
        }
      ENDCG
    }
  }
}