﻿//forward base setup
#ifndef MK_GLASS_FORWARD_BASE_SETUP
	#define MK_GLASS_FORWARD_BASE_SETUP

	#ifndef MK_GLASS_FWD_BASE_PASS
		#define MK_GLASS_FWD_BASE_PASS 1
	#endif

	#include "UnityCG.cginc"
	#include "AutoLight.cginc"

	#include "../Common/MKGlassDef.cginc"
	#include "../Common/MKGlassV.cginc"
	#include "../Common/MKGlassInc.cginc"
	#include "../Forward/MKGlassForwardIO.cginc"
	#include "../Surface/MKGlassSurfaceIO.cginc"
	#include "../Common/MKGlassLight.cginc"
	#include "../Surface/MKGlassSurface.cginc"
#endif