#ifndef ReplacePalette_INCLUDED
#define ReplacePalette_INCLUDED

void GenerateGradientPallete_float(float4 Color1, float4 Color2, float4 Color3, float4 Color4, out Gradient GradientPallete)
        {
            GradientPallete.type = 1;
            GradientPallete.colorsLength = 4;
            GradientPallete.alphasLength = 2;
            GradientPallete.colors[0] = Color1;
            GradientPallete.colors[1] = Color2;
            GradientPallete.colors[2] = Color3;
            GradientPallete.colors[3] = Color4;
            GradientPallete.colors[4] = float4(0, 0, 0, 0);
            GradientPallete.colors[5] = float4(0, 0, 0, 0);
            GradientPallete.colors[6] = float4(0, 0, 0, 0);
            GradientPallete.colors[7] = float4(0, 0, 0, 0);
            GradientPallete.alphas[0] = float2(1, 1);
            GradientPallete.alphas[1] = float2(1, 1);
            GradientPallete.alphas[2] = float2(1, 1);
            GradientPallete.alphas[3] = float2(1, 1);
            GradientPallete.alphas[4] = float2(0, 0);
            GradientPallete.alphas[5] = float2(0, 0);
            GradientPallete.alphas[6] = float2(0, 0);
            GradientPallete.alphas[7] = float2(0, 0); 
        }
bool isfinite(Gradient g)
{
   return true;
}


void ReplacePalette_float(float4 Color1, float4 Color2, float4 Color3, float4 Color4,float Color1Mask, float Color2Mask, float Color3Mask, float Color4Mask, out float4 ReplacedPixels,out float Alpha)
        {
            float4 C1Pixels = Color1 * Color1Mask;
            float4 C2Pixels = Color2 * Color2Mask;
            float4 C3Pixels = Color3 * Color3Mask;
            float4 C4Pixels = Color4 * Color4Mask;
            float4 CombinedPixels = C1Pixels + C2Pixels + C3Pixels + C4Pixels;

            Alpha = Color1.a * Color1Mask + Color2.a * Color2Mask + Color3.a * Color3Mask + Color4.a * Color4Mask;
            

            ReplacedPixels = CombinedPixels;
            
        }

#endif