// Interface.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "Interface.h"


// This is an example of an exported variable
INTERFACE_API int nInterface=0;

// This is an example of an exported function.
INTERFACE_API int fnInterface(void)
{
    return 0;
}

// This is the constructor of a class that has been exported.
CInterface::CInterface()
{
    return;
}
