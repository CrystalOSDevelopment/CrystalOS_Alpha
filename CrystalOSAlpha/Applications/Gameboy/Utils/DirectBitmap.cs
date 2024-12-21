﻿using System;
using System.Runtime.CompilerServices;
using Cosmos.System.Graphics;
using CrystalOSAlpha;

namespace ProjectDMG {
    public class DirectBitmap {
        public Bitmap Bitmap { get; private set; }
        public static int Height = ImprovedVBE.height;//144
        public static int Width = ImprovedVBE.width;//160

        public static int CachedY = -1;
        public static int CachedYbyWidth = -1;
        public static int MaxIndex = ImprovedVBE.cover.RawData.Length - 2;

        public DirectBitmap() {
            Bitmap = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(int x, int y, int colour) {
            switch(y != CachedY)
            {
                case true:
                    CachedY = PPU.y + y * 3;
                    CachedYbyWidth = CachedY * Width;
                    break;
            }
            int computedX = x * 3 + PPU.x;
            int index = computedX + CachedYbyWidth;

            // Draw 3 rows to stretch along the y-axis
            for (int i = 0; i < 3; i++)
            {
                switch(index >= 0 && index < MaxIndex)
                {
                    case true:
                        ImprovedVBE.cover.RawData[index] = colour;
                        ImprovedVBE.cover.RawData[index + 1] = colour;
                        ImprovedVBE.cover.RawData[index + 2] = colour;
                        break;
                }

                // Move to the next row
                index += Width;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetPixel(int x, int y) {
            int index = x + (y * Width);
            return Bitmap.RawData[index];
        }
    }
}