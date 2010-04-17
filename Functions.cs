using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace _3DCube
{
	public static class Funct {
	
		public static double ClippedMultiply(double input, double multiplier, double lowerCap, double upperCap) {
			// Performs multiplication of input by multiplier and makes sure the return
			// value is between lowerCap and upperCap.
			// Currently Depreciated after rewrite.
			input = input * multiplier;
			if (input > upperCap) {
				return upperCap;
			} else if (input < lowerCap) {
				return lowerCap;
			}
			return input;
		}
	}
}
