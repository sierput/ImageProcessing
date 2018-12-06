#pragma once
#ifdef MATHLIBRARY_EXPORTS  
#define MATHLIBRARY_API __declspec(dllexport)   
#else  
#define HELLO_API __declspec(dllimport)   
#endif  

namespace MathLibrary
{
	// This class is exported from the MathLibrary.dll  
	class Functions
	{
	public:
		// Returns a + b  
		static HELLO_API double Add(double a, double b);

		// Returns a * b  
		static HELLO_API double Multiply(double a, double b);

		// Returns a + (a * b)  
		static HELLO_API double AddMultiply(double a, double b);
	};
}
