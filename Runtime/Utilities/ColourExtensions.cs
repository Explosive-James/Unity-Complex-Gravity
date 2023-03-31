using UnityEngine;
using System.Runtime.CompilerServices;

namespace ComplexGravity.Utilities
{
    public static class ColourExtensions
    {
        #region Public Functions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color GetRandomColour()
        {
            System.Random rng = new System.Random();
            return Color.HSVToRGB((float)rng.NextDouble(), .7f, 1);
        }

        /// <summary>
        /// Returns the colour after setting the red channel.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color SetRed(this Color color, float red) => new Color(red, color.g, color.b, color.a);
        /// <summary>
        /// Returns the colour after setting the green channel.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color SetGreen(this Color color, float green) => new Color(color.r, green, color.b, color.a);
        /// <summary>
        /// Returns the colour after setting the blue channel.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color SetBlue(this Color color, float blue) => new Color(color.r, color.g, blue, color.a);
        /// <summary>
        /// Returns the colour after setting the alpha channel.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color SetAlpha(this Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);
        #endregion
    }
}
