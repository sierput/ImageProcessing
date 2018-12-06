// DlLNativCpp.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <Windows.h>
#include <iostream>
#include "fstream"
extern "C"
{
	__declspec(dllexport)  unsigned char* GreyScaleNative(unsigned char *tab, int size)
	{
		for (size_t i = 0; i < size; i += 3)
		{
			// L = 0.0722·B +  0.7152·G + 0.2126·R
			double l = (0.07 * tab[i]) + (0.71 * tab[i + 1]) + (0.21 * tab[i + 2]);
			tab[i] = l;
			tab[i + 1] = l;
			tab[i + 2] = l;
		}
		return tab;
	}
}


