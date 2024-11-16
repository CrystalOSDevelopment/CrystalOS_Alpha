namespace CrystalOSAlpha.Graphics.Engine
{
    class WebsafeColors
    {
        public static int ReturnWithColor(byte INPUT)
        {
            // Calculate R, G, B values based on the web-safe color scheme
            int r = (INPUT / 36) * 51;             // Red channel
            int g = ((INPUT / 6) % 6) * 51;        // Green channel
            int b = (INPUT % 6) * 51;              // Blue channel

            // Convert RGB to a single integer color value
            return ImprovedVBE.colourToNumber(r, g, b);
            //switch (INPUT)
            //{
            //    case 0:
            //        return ImprovedVBE.colourToNumber(0, 0, 0);
            //    case 1:
            //        return ImprovedVBE.colourToNumber(0, 0, 51);
            //    case 2:
            //        return ImprovedVBE.colourToNumber(0, 0, 102);
            //    case 3:
            //        return ImprovedVBE.colourToNumber(0, 0, 153);
            //    case 4:
            //        return ImprovedVBE.colourToNumber(0, 0, 204);
            //    case 5:
            //        return ImprovedVBE.colourToNumber(0, 0, 255);
            //    case 6:
            //        return ImprovedVBE.colourToNumber(0, 51, 0);
            //    case 7:
            //        return ImprovedVBE.colourToNumber(0, 51, 51);
            //    case 8:
            //        return ImprovedVBE.colourToNumber(0, 51, 102);
            //    case 9:
            //        return ImprovedVBE.colourToNumber(0, 51, 153);
            //    case 10:
            //        return ImprovedVBE.colourToNumber(0, 51, 204);
            //    case 11:
            //        return ImprovedVBE.colourToNumber(0, 51, 255);
            //    case 12:
            //        return ImprovedVBE.colourToNumber(0, 102, 0);
            //    case 13:
            //        return ImprovedVBE.colourToNumber(0, 102, 51);
            //    case 14:
            //        return ImprovedVBE.colourToNumber(0, 102, 102);
            //    case 15:
            //        return ImprovedVBE.colourToNumber(0, 102, 153);
            //    case 16:
            //        return ImprovedVBE.colourToNumber(0, 102, 204);
            //    case 17:
            //        return ImprovedVBE.colourToNumber(0, 102, 255);
            //    case 18:
            //        return ImprovedVBE.colourToNumber(0, 153, 0);
            //    case 19:
            //        return ImprovedVBE.colourToNumber(0, 153, 51);
            //    case 20:
            //        return ImprovedVBE.colourToNumber(0, 153, 102);
            //    case 21:
            //        return ImprovedVBE.colourToNumber(0, 153, 153);
            //    case 22:
            //        return ImprovedVBE.colourToNumber(0, 153, 204);
            //    case 23:
            //        return ImprovedVBE.colourToNumber(0, 153, 255);
            //    case 24:
            //        return ImprovedVBE.colourToNumber(0, 204, 0);
            //    case 25:
            //        return ImprovedVBE.colourToNumber(0, 204, 51);
            //    case 26:
            //        return ImprovedVBE.colourToNumber(0, 204, 102);
            //    case 27:
            //        return ImprovedVBE.colourToNumber(0, 204, 153);
            //    case 28:
            //        return ImprovedVBE.colourToNumber(0, 204, 204);
            //    case 29:
            //        return ImprovedVBE.colourToNumber(0, 204, 255);
            //    case 30:
            //        return ImprovedVBE.colourToNumber(0, 255, 0);
            //    case 31:
            //        return ImprovedVBE.colourToNumber(0, 255, 51);
            //    case 32:
            //        return ImprovedVBE.colourToNumber(0, 255, 102);
            //    case 33:
            //        return ImprovedVBE.colourToNumber(0, 255, 153);
            //    case 34:
            //        return ImprovedVBE.colourToNumber(0, 255, 204);
            //    case 35:
            //        return ImprovedVBE.colourToNumber(0, 255, 255);
            //    case 36:
            //        return ImprovedVBE.colourToNumber(51, 0, 0);
            //    case 37:
            //        return ImprovedVBE.colourToNumber(51, 0, 51);
            //    case 38:
            //        return ImprovedVBE.colourToNumber(51, 0, 102);
            //    case 39:
            //        return ImprovedVBE.colourToNumber(51, 0, 153);
            //    case 40:
            //        return ImprovedVBE.colourToNumber(51, 0, 204);
            //    case 41:
            //        return ImprovedVBE.colourToNumber(51, 0, 255);
            //    case 42:
            //        return ImprovedVBE.colourToNumber(51, 51, 0);
            //    case 43:
            //        return ImprovedVBE.colourToNumber(51, 51, 51);
            //    case 44:
            //        return ImprovedVBE.colourToNumber(51, 51, 102);
            //    case 45:
            //        return ImprovedVBE.colourToNumber(51, 51, 153);
            //    case 46:
            //        return ImprovedVBE.colourToNumber(51, 51, 204);
            //    case 47:
            //        return ImprovedVBE.colourToNumber(51, 51, 255);
            //    case 48:
            //        return ImprovedVBE.colourToNumber(51, 102, 0);
            //    case 49:
            //        return ImprovedVBE.colourToNumber(51, 102, 51);
            //    case 50:
            //        return ImprovedVBE.colourToNumber(51, 102, 102);
            //    case 51:
            //        return ImprovedVBE.colourToNumber(51, 102, 153);
            //    case 52:
            //        return ImprovedVBE.colourToNumber(51, 102, 204);
            //    case 53:
            //        return ImprovedVBE.colourToNumber(51, 102, 255);
            //    case 54:
            //        return ImprovedVBE.colourToNumber(51, 153, 0);
            //    case 55:
            //        return ImprovedVBE.colourToNumber(51, 153, 51);
            //    case 56:
            //        return ImprovedVBE.colourToNumber(51, 153, 102);
            //    case 57:
            //        return ImprovedVBE.colourToNumber(51, 153, 153);
            //    case 58:
            //        return ImprovedVBE.colourToNumber(51, 153, 204);
            //    case 59:
            //        return ImprovedVBE.colourToNumber(51, 153, 255);
            //    case 60:
            //        return ImprovedVBE.colourToNumber(51, 204, 0);
            //    case 61:
            //        return ImprovedVBE.colourToNumber(51, 204, 51);
            //    case 62:
            //        return ImprovedVBE.colourToNumber(51, 204, 102);
            //    case 63:
            //        return ImprovedVBE.colourToNumber(51, 204, 153);
            //    case 64:
            //        return ImprovedVBE.colourToNumber(51, 204, 204);
            //    case 65:
            //        return ImprovedVBE.colourToNumber(51, 204, 255);
            //    case 66:
            //        return ImprovedVBE.colourToNumber(51, 255, 0);
            //    case 67:
            //        return ImprovedVBE.colourToNumber(51, 255, 51);
            //    case 68:
            //        return ImprovedVBE.colourToNumber(51, 255, 102);
            //    case 69:
            //        return ImprovedVBE.colourToNumber(51, 255, 153);
            //    case 70:
            //        return ImprovedVBE.colourToNumber(51, 255, 204);
            //    case 71:
            //        return ImprovedVBE.colourToNumber(51, 255, 255);
            //    case 72:
            //        return ImprovedVBE.colourToNumber(102, 0, 0);
            //    case 73:
            //        return ImprovedVBE.colourToNumber(102, 0, 51);
            //    case 74:
            //        return ImprovedVBE.colourToNumber(102, 0, 102);
            //    case 75:
            //        return ImprovedVBE.colourToNumber(102, 0, 153);
            //    case 76:
            //        return ImprovedVBE.colourToNumber(102, 0, 204);
            //    case 77:
            //        return ImprovedVBE.colourToNumber(102, 0, 255);
            //    case 78:
            //        return ImprovedVBE.colourToNumber(102, 51, 0);
            //    case 79:
            //        return ImprovedVBE.colourToNumber(102, 51, 51);
            //    case 80:
            //        return ImprovedVBE.colourToNumber(102, 51, 102);
            //    case 81:
            //        return ImprovedVBE.colourToNumber(102, 51, 153);
            //    case 82:
            //        return ImprovedVBE.colourToNumber(102, 51, 204);
            //    case 83:
            //        return ImprovedVBE.colourToNumber(102, 51, 255);
            //    case 84:
            //        return ImprovedVBE.colourToNumber(102, 102, 0);
            //    case 85:
            //        return ImprovedVBE.colourToNumber(102, 102, 51);
            //    case 86:
            //        return ImprovedVBE.colourToNumber(102, 102, 102);
            //    case 87:
            //        return ImprovedVBE.colourToNumber(102, 102, 153);
            //    case 88:
            //        return ImprovedVBE.colourToNumber(102, 102, 204);
            //    case 89:
            //        return ImprovedVBE.colourToNumber(102, 102, 255);
            //    case 90:
            //        return ImprovedVBE.colourToNumber(102, 153, 0);
            //    case 91:
            //        return ImprovedVBE.colourToNumber(102, 153, 51);
            //    case 92:
            //        return ImprovedVBE.colourToNumber(102, 153, 102);
            //    case 93:
            //        return ImprovedVBE.colourToNumber(102, 153, 153);
            //    case 94:
            //        return ImprovedVBE.colourToNumber(102, 153, 204);
            //    case 95:
            //        return ImprovedVBE.colourToNumber(102, 153, 255);
            //    case 96:
            //        return ImprovedVBE.colourToNumber(102, 204, 0);
            //    case 97:
            //        return ImprovedVBE.colourToNumber(102, 204, 51);
            //    case 98:
            //        return ImprovedVBE.colourToNumber(102, 204, 102);
            //    case 99:
            //        return ImprovedVBE.colourToNumber(102, 204, 153);
            //    case 100:
            //        return ImprovedVBE.colourToNumber(102, 204, 204);
            //    case 101:
            //        return ImprovedVBE.colourToNumber(102, 204, 255);
            //    case 102:
            //        return ImprovedVBE.colourToNumber(102, 255, 0);
            //    case 103:
            //        return ImprovedVBE.colourToNumber(102, 255, 51);
            //    case 104:
            //        return ImprovedVBE.colourToNumber(102, 255, 102);
            //    case 105:
            //        return ImprovedVBE.colourToNumber(102, 255, 153);
            //    case 106:
            //        return ImprovedVBE.colourToNumber(102, 255, 204);
            //    case 107:
            //        return ImprovedVBE.colourToNumber(102, 255, 255);
            //    case 108:
            //        return ImprovedVBE.colourToNumber(153, 0, 0);
            //    case 109:
            //        return ImprovedVBE.colourToNumber(153, 0, 51);
            //    case 110:
            //        return ImprovedVBE.colourToNumber(153, 0, 102);
            //    case 111:
            //        return ImprovedVBE.colourToNumber(153, 0, 153);
            //    case 112:
            //        return ImprovedVBE.colourToNumber(153, 0, 204);
            //    case 113:
            //        return ImprovedVBE.colourToNumber(153, 0, 255);
            //    case 114:
            //        return ImprovedVBE.colourToNumber(153, 51, 0);
            //    case 115:
            //        return ImprovedVBE.colourToNumber(153, 51, 51);
            //    case 116:
            //        return ImprovedVBE.colourToNumber(153, 51, 102);
            //    case 117:
            //        return ImprovedVBE.colourToNumber(153, 51, 153);
            //    case 118:
            //        return ImprovedVBE.colourToNumber(153, 51, 204);
            //    case 119:
            //        return ImprovedVBE.colourToNumber(153, 51, 255);
            //    case 120:
            //        return ImprovedVBE.colourToNumber(153, 102, 0);
            //    case 121:
            //        return ImprovedVBE.colourToNumber(153, 102, 51);
            //    case 122:
            //        return ImprovedVBE.colourToNumber(153, 102, 102);
            //    case 123:
            //        return ImprovedVBE.colourToNumber(153, 102, 153);
            //    case 124:
            //        return ImprovedVBE.colourToNumber(153, 102, 204);
            //    case 125:
            //        return ImprovedVBE.colourToNumber(153, 102, 255);
            //    case 126:
            //        return ImprovedVBE.colourToNumber(153, 153, 0);
            //    case 127:
            //        return ImprovedVBE.colourToNumber(153, 153, 51);
            //    case 128:
            //        return ImprovedVBE.colourToNumber(153, 153, 102);
            //    case 129:
            //        return ImprovedVBE.colourToNumber(153, 153, 153);
            //    case 130:
            //        return ImprovedVBE.colourToNumber(153, 153, 204);
            //    case 131:
            //        return ImprovedVBE.colourToNumber(153, 153, 255);
            //    case 132:
            //        return ImprovedVBE.colourToNumber(153, 204, 0);
            //    case 133:
            //        return ImprovedVBE.colourToNumber(153, 204, 51);
            //    case 134:
            //        return ImprovedVBE.colourToNumber(153, 204, 102);
            //    case 135:
            //        return ImprovedVBE.colourToNumber(153, 204, 153);
            //    case 136:
            //        return ImprovedVBE.colourToNumber(153, 204, 204);
            //    case 137:
            //        return ImprovedVBE.colourToNumber(153, 204, 255);
            //    case 138:
            //        return ImprovedVBE.colourToNumber(153, 255, 0);
            //    case 139:
            //        return ImprovedVBE.colourToNumber(153, 255, 51);
            //    case 140:
            //        return ImprovedVBE.colourToNumber(153, 255, 102);
            //    case 141:
            //        return ImprovedVBE.colourToNumber(153, 255, 153);
            //    case 142:
            //        return ImprovedVBE.colourToNumber(153, 255, 204);
            //    case 143:
            //        return ImprovedVBE.colourToNumber(153, 255, 255);
            //    case 144:
            //        return ImprovedVBE.colourToNumber(204, 0, 0);
            //    case 145:
            //        return ImprovedVBE.colourToNumber(204, 0, 51);
            //    case 146:
            //        return ImprovedVBE.colourToNumber(204, 0, 102);
            //    case 147:
            //        return ImprovedVBE.colourToNumber(204, 0, 153);
            //    case 148:
            //        return ImprovedVBE.colourToNumber(204, 0, 204);
            //    case 149:
            //        return ImprovedVBE.colourToNumber(204, 0, 255);
            //    case 150:
            //        return ImprovedVBE.colourToNumber(204, 51, 0);
            //    case 151:
            //        return ImprovedVBE.colourToNumber(204, 51, 51);
            //    case 152:
            //        return ImprovedVBE.colourToNumber(204, 51, 102);
            //    case 153:
            //        return ImprovedVBE.colourToNumber(204, 51, 153);
            //    case 154:
            //        return ImprovedVBE.colourToNumber(204, 51, 204);
            //    case 155:
            //        return ImprovedVBE.colourToNumber(204, 51, 255);
            //    case 156:
            //        return ImprovedVBE.colourToNumber(204, 102, 0);
            //    case 157:
            //        return ImprovedVBE.colourToNumber(204, 102, 51);
            //    case 158:
            //        return ImprovedVBE.colourToNumber(204, 102, 102);
            //    case 159:
            //        return ImprovedVBE.colourToNumber(204, 102, 153);
            //    case 160:
            //        return ImprovedVBE.colourToNumber(204, 102, 204);
            //    case 161:
            //        return ImprovedVBE.colourToNumber(204, 102, 255);
            //    case 162:
            //        return ImprovedVBE.colourToNumber(204, 153, 0);
            //    case 163:
            //        return ImprovedVBE.colourToNumber(204, 153, 51);
            //    case 164:
            //        return ImprovedVBE.colourToNumber(204, 153, 102);
            //    case 165:
            //        return ImprovedVBE.colourToNumber(204, 153, 153);
            //    case 166:
            //        return ImprovedVBE.colourToNumber(204, 153, 204);
            //    case 167:
            //        return ImprovedVBE.colourToNumber(204, 153, 255);
            //    case 168:
            //        return ImprovedVBE.colourToNumber(204, 204, 0);
            //    case 169:
            //        return ImprovedVBE.colourToNumber(204, 204, 51);
            //    case 170:
            //        return ImprovedVBE.colourToNumber(204, 204, 102);
            //    case 171:
            //        return ImprovedVBE.colourToNumber(204, 204, 153);
            //    case 172:
            //        return ImprovedVBE.colourToNumber(204, 204, 204);
            //    case 173:
            //        return ImprovedVBE.colourToNumber(204, 204, 255);
            //    case 174:
            //        return ImprovedVBE.colourToNumber(204, 255, 0);
            //    case 175:
            //        return ImprovedVBE.colourToNumber(204, 255, 51);
            //    case 176:
            //        return ImprovedVBE.colourToNumber(204, 255, 102);
            //    case 177:
            //        return ImprovedVBE.colourToNumber(204, 255, 153);
            //    case 178:
            //        return ImprovedVBE.colourToNumber(204, 255, 204);
            //    case 179:
            //        return ImprovedVBE.colourToNumber(204, 255, 255);
            //    case 180:
            //        return ImprovedVBE.colourToNumber(255, 0, 0);
            //    case 181:
            //        return ImprovedVBE.colourToNumber(255, 0, 51);
            //    case 182:
            //        return ImprovedVBE.colourToNumber(255, 0, 102);
            //    case 183:
            //        return ImprovedVBE.colourToNumber(255, 0, 153);
            //    case 184:
            //        return ImprovedVBE.colourToNumber(255, 0, 204);
            //    case 185:
            //        return ImprovedVBE.colourToNumber(255, 0, 255);
            //    case 186:
            //        return ImprovedVBE.colourToNumber(255, 51, 0);
            //    case 187:
            //        return ImprovedVBE.colourToNumber(255, 51, 51);
            //    case 188:
            //        return ImprovedVBE.colourToNumber(255, 51, 102);
            //    case 189:
            //        return ImprovedVBE.colourToNumber(255, 51, 153);
            //    case 190:
            //        return ImprovedVBE.colourToNumber(255, 51, 204);
            //    case 191:
            //        return ImprovedVBE.colourToNumber(255, 51, 255);
            //    case 192:
            //        return ImprovedVBE.colourToNumber(255, 102, 0);
            //    case 193:
            //        return ImprovedVBE.colourToNumber(255, 102, 51);
            //    case 194:
            //        return ImprovedVBE.colourToNumber(255, 102, 102);
            //    case 195:
            //        return ImprovedVBE.colourToNumber(255, 102, 153);
            //    case 196:
            //        return ImprovedVBE.colourToNumber(255, 102, 204);
            //    case 197:
            //        return ImprovedVBE.colourToNumber(255, 102, 255);
            //    case 198:
            //        return ImprovedVBE.colourToNumber(255, 153, 0);
            //    case 199:
            //        return ImprovedVBE.colourToNumber(255, 153, 51);
            //    case 200:
            //        return ImprovedVBE.colourToNumber(255, 153, 102);
            //    case 201:
            //        return ImprovedVBE.colourToNumber(255, 153, 153);
            //    case 202:
            //        return ImprovedVBE.colourToNumber(255, 153, 204);
            //    case 203:
            //        return ImprovedVBE.colourToNumber(255, 153, 255);
            //    case 204:
            //        return ImprovedVBE.colourToNumber(255, 204, 0);
            //    case 205:
            //        return ImprovedVBE.colourToNumber(255, 204, 51);
            //    case 206:
            //        return ImprovedVBE.colourToNumber(255, 204, 102);
            //    case 207:
            //        return ImprovedVBE.colourToNumber(255, 204, 153);
            //    case 208:
            //        return ImprovedVBE.colourToNumber(255, 204, 204);
            //    case 209:
            //        return ImprovedVBE.colourToNumber(255, 204, 255);
            //    case 210:
            //        return ImprovedVBE.colourToNumber(255, 255, 0);
            //    case 211:
            //        return ImprovedVBE.colourToNumber(255, 255, 51);
            //    case 212:
            //        return ImprovedVBE.colourToNumber(255, 255, 102);
            //    case 213:
            //        return ImprovedVBE.colourToNumber(255, 255, 153);
            //    case 214:
            //        return ImprovedVBE.colourToNumber(255, 255, 204);
            //    case 215:
            //        return ImprovedVBE.colourToNumber(255, 255, 255);
            //    default: return ImprovedVBE.colourToNumber(0, 0, 0);
            //}
        }
    }
}
