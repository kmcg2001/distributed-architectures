// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the INTERFACE_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// INTERFACE_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef INTERFACE_EXPORTS
#define INTERFACE_API __declspec(dllexport)
#else
#define INTERFACE_API __declspec(dllimport)
#endif

// This class is exported from the dll
class INTERFACE_API CInterface {
public:
	CInterface(void);
	// TODO: add your methods here.
};

extern INTERFACE_API int nInterface;

INTERFACE_API int fnInterface(void);
