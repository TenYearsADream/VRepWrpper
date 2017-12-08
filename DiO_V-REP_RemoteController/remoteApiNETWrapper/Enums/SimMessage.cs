﻿
namespace VREPRemoteAPI.Enums
{
    public enum SimMessage
    {
        UIButtonStateChange = 0,
        reserved9 = 1,
        ObjectSelectionChanged = 2,
        ModelLoaded = 4,
        Reserved11 = 5,
        Keypress = 6,
        BannerClicked = 7,
        ForCApiOnlyStart = 256,
        Reserved1 = 257,
        Reserved2 = 258,
        Reserved3 = 259,
        EventcallbackScenesave = 260,
        EventcallbackModelsave = 261,
        EventcallbackModuleopen = 262,
        EventcallbackModulehandle = 263,
        EventcallbackModuleclose = 264,
        Reserved4 = 265,
        Reserved5 = 266,
        Reserved6 = 267,
        Reserved7 = 268,
        ЕventcallbackInstancepass = 269,
        ЕventcallbackBroadcast = 270,
        ЕventcallbackImagefilterEnumreset = 271,
        ЕventcallbackImagefilterEnumerate = 272,
        ЕventcallbackImagefilterAdjustparams = 273,
        ЕventcallbackImagefilterReserved = 274,
        ЕventcallbackImagefilterProcess = 275,
        ЕventcallbackReserved1 = 276,
        ЕventcallbackReserved2 = 277,
        ЕventcallbackReserved3 = 278,
        ЕventcallbackReserved4 = 279,
        ЕventcallbackAbouttoundo = 280,
        ЕventcallbackUndoperformed = 281,
        ЕventcallbackAbouttoredo = 282,
        ЕventcallbackRedoperformed = 283,
        ЕventcallbackScripticondblclick = 284,
        ЕventcallbackSimulationabouttostart = 285,
        ЕventcallbackSimulationended = 286,
        ЕventcallbackReserved5 = 287,
        ЕventcallbackKeypress = 288,
        ЕventcallbackModulehandleinsensingpart = 289,
        ЕventcallbackRenderingpass = 290,
        ЕventcallbackBannerclicked = 291,
        ЕventcallbackMenuitemselected = 292,
        ЕventcallbackRefreshdialogs = 293,
        ЕventcallbackSceneloaded = 294,
        ЕventcallbackModelloaded = 295,
        ЕventcallbackInstanceswitch = 296,
        ЕventcallbackGuipass = 297,
        ЕventcallbackMainscriptabouttobecalled = 298,
        SimulationStartResumeRequest = 4096,
        SimulationPauseRequest = 4097,
        SimulationStopRequest = 4098
    }
}
