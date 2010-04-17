using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3DCube
{
	public static class Funct {
	
		public static double ClippedMultiply(double input, double multiplier, double lowerCap, double upperCap) {
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
