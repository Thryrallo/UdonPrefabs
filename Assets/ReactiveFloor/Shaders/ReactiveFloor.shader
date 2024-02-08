Shader "Thry/Poiyomi Reactive Floor"
{
	Properties
	{
		[HideInInspector] shader_master_label ("<color=#E75898ff>Poiyomi 9.0.α29</color>", Float) = 0
		[HideInInspector] shader_is_using_thry_editor ("", Float) = 0
		[HideInInspector] shader_locale ("0db0b86376c3dca4b9a6828ef8615fe0", Float) = 0
		[HideInInspector] footer_youtube ("{texture:{name:icon-youtube,height:16},action:{type:URL,data:https://www.youtube.com/poiyomi},hover:YOUTUBE}", Float) = 0
		[HideInInspector] footer_twitter ("{texture:{name:icon-twitter,height:16},action:{type:URL,data:https://twitter.com/poiyomi},hover:TWITTER}", Float) = 0
		[HideInInspector] footer_patreon ("{texture:{name:icon-patreon,height:16},action:{type:URL,data:https://www.patreon.com/poiyomi},hover:PATREON}", Float) = 0
		[HideInInspector] footer_discord ("{texture:{name:icon-discord,height:16},action:{type:URL,data:https://discord.gg/Ays52PY},hover:DISCORD}", Float) = 0
		[HideInInspector] footer_github ("{texture:{name:icon-github,height:16},action:{type:URL,data:https://github.com/poiyomi/PoiyomiToonShader},hover:GITHUB}", Float) = 0
		
		// Warning that only shows up when ThryEditor hasn't loaded
		[Header(POIYOMI SHADER UI FAILED TO LOAD)]
		[Header(.    This is caused by scripts failing to compile. It can be fixed.)]
		[Header(.          The inspector will look broken and will not work properly until fixed.)]
		[Header(.    Please check your console for script errors.)]
		[Header(.          You can filter by errors in the console window.)]
		[Header(.          Often the topmost error points to the erroring script.)]
		[Space(30)][Header(Common Error Causes)]
		[Header(.    Installing multiple Poiyomi Shader packages)]
		[Header(.          Make sure to delete the Poiyomi shader folder before you update Poiyomi.)]
		[Header(.          If a package came with Poiyomi this is bad practice and can cause issues.)]
		[Header(.          Delete the package and import it without any Poiyomi components.)]
		[Header(.    Bad VRCSDK installation (e.g. Both VCC and Standalone))]
		[Header(.          Delete the VRCSDK Folder in Assets if you are using the VCC.)]
		[Header(.          Avoid using third party SDKs. They can cause incompatibility.)]
		[Header(.    Script Errors in other scripts)]
		[Header(.          Outdated tools or prefabs can cause this.)]
		[Header(.          Update things that are throwing errors or move them outside the project.)]
		[Space(30)][Header(Visit Our Discord to Ask For Help)]
		[Space(5)]_ShaderUIWarning0 (" → discord.gg/poiyomi ←    We can help you get it fixed!                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         --{condition_showS:(0==1)}", Int) = -0
		[Space(1400)][Header(POIYOMI SHADER UI FAILED TO LOAD)]
		_ShaderUIWarning1 ("Please scroll up for more information!                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     --{condition_showS:(0==1)}", Int) = -0
		
		// Keyword to remind users in the VRChat SDK that this material hasn't been locked.  Inelegant but it works.
		[HideInInspector] _ForgotToLockMaterial (";;YOU_FORGOT_TO_LOCK_THIS_MATERIAL;", Int) = 1
		[ThryShaderOptimizerLockButton] _ShaderOptimizerEnabled ("", Int) = 0
		[HideInInspector] GeometryShader_Enabled("GEOMETRY SHADER ENABLED", Float) = 1
		[HideInInspector] Tessellation_Enabled("TESSELLATION ENABLED", Float) = 1
		//[ThryCustomGUI(Poi.Tools.ModularShaderSystem.ModularShadersForThryEditor,Poi.Tools,GUICustomPoiMSS)] _CustomShaderButton("CustomShaderButton", Float) = 0
		[ThryWideEnum(Opaque, 0, Cutout, 1, TransClipping, 9, Fade, 2, Transparent, 3, Additive, 4, Soft Additive, 5, Multiplicative, 6, 2x Multiplicative, 7)]_Mode("Rendering Preset--{on_value_actions:[
		{value:0,actions:[{type:SET_PROPERTY,data:render_queue=2000},{type:SET_PROPERTY,data:_AlphaForceOpaque=1}, {type:SET_PROPERTY,data:render_type=Opaque},            {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=1}, {type:SET_PROPERTY,data:_DstBlend=0},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=1}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=1}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=1}, {type:SET_PROPERTY,data:_OutlineDstBlend=0},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=0}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:1,actions:[{type:SET_PROPERTY,data:render_queue=2450},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=TransparentCutout}, {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=.5}, {type:SET_PROPERTY,data:_SrcBlend=1}, {type:SET_PROPERTY,data:_DstBlend=0},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=1}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=1}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=1}, {type:SET_PROPERTY,data:_OutlineDstBlend=0},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:9,actions:[{type:SET_PROPERTY,data:render_queue=2460},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=TransparentCutout}, {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0.01},  {type:SET_PROPERTY,data:_SrcBlend=5}, {type:SET_PROPERTY,data:_DstBlend=10}, {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=5}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=1}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=5}, {type:SET_PROPERTY,data:_OutlineDstBlend=10}, {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:2,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0.002},  {type:SET_PROPERTY,data:_SrcBlend=5}, {type:SET_PROPERTY,data:_DstBlend=10}, {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=5}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=5}, {type:SET_PROPERTY,data:_OutlineDstBlend=10}, {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:3,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=1}, {type:SET_PROPERTY,data:_DstBlend=10}, {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=1}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=1}, {type:SET_PROPERTY,data:_OutlineSrcBlend=1}, {type:SET_PROPERTY,data:_OutlineDstBlend=10}, {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:4,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=1}, {type:SET_PROPERTY,data:_DstBlend=1},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=1}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=1}, {type:SET_PROPERTY,data:_OutlineDstBlend=1},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:5,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=4}, {type:SET_PROPERTY,data:_DstBlend=1},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=4}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=4}, {type:SET_PROPERTY,data:_OutlineDstBlend=1},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:6,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=2}, {type:SET_PROPERTY,data:_DstBlend=0},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=2}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=2}, {type:SET_PROPERTY,data:_OutlineDstBlend=0},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]},
		{value:7,actions:[{type:SET_PROPERTY,data:render_queue=3000},{type:SET_PROPERTY,data:_AlphaForceOpaque=0}, {type:SET_PROPERTY,data:render_type=Transparent},       {type:SET_PROPERTY,data:_BlendOp=0}, {type:SET_PROPERTY,data:_BlendOpAlpha=4}, {type:SET_PROPERTY,data:_Cutoff=0},  {type:SET_PROPERTY,data:_SrcBlend=2}, {type:SET_PROPERTY,data:_DstBlend=3},  {type:SET_PROPERTY,data:_SrcBlendAlpha=1}, {type:SET_PROPERTY,data:_DstBlendAlpha=1},  {type:SET_PROPERTY,data:_AddSrcBlend=2}, {type:SET_PROPERTY,data:_AddDstBlend=1}, {type:SET_PROPERTY,data:_AddSrcBlendAlpha=0}, {type:SET_PROPERTY,data:_AddDstBlendAlpha=1}, {type:SET_PROPERTY,data:_AlphaToCoverage=0},  {type:SET_PROPERTY,data:_ZWrite=0}, {type:SET_PROPERTY,data:_ZTest=4},   {type:SET_PROPERTY,data:_AlphaPremultiply=0}, {type:SET_PROPERTY,data:_OutlineSrcBlend=2}, {type:SET_PROPERTY,data:_OutlineDstBlend=3},  {type:SET_PROPERTY,data:_OutlineSrcBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineDstBlendAlpha=1}, {type:SET_PROPERTY,data:_OutlineBlendOp=0}, {type:SET_PROPERTY,data:_OutlineBlendOpAlpha=4}]}
		}]}]}", Int) = 0
		
		[HideInInspector] m_mainCategory ("Color & Normals--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/color-and-normals/main},hover:Documentation}}", Float) = 0
		//Main-main
		_Color ("Color & Alpha--{reference_property:_ColorThemeIndex}", Color) = (1, 1, 1, 1)
		[HideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _ColorThemeIndex ("", Int) = 0
		[sRGBWarning(true)]_MainTex ("Texture--{reference_properties:[_MainTexPan, _MainTexUV, _MainPixelMode, _MainTexStochastic]}", 2D) = "white" { }
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _MainTexUV ("UV", Int) = 0
		[HideInInspector][Vector2]_MainTexPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ToggleUI]_MainPixelMode ("Pixel Mode", Float) = 0
		[HideInInspector][ToggleUI]_MainTexStochastic ("Stochastic Sampling", Float) = 0
		[Normal]_BumpMap ("Normal Map--{reference_properties:[_BumpMapPan, _BumpMapUV, _BumpScale, _BumpMapStochastic]}", 2D) = "bump" { }
		[HideInInspector][Vector2]_BumpMapPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _BumpMapUV ("UV", Int) = 0
		[HideInInspector]_BumpScale ("Intensity", Range(0, 10)) = 1
		[HideInInspector][ToggleUI]_BumpMapStochastic ("Stochastic Sampling", Float) = 0
		[sRGBWarning]_AlphaMask ("Alpha Map--{reference_properties:[_AlphaMaskPan, _AlphaMaskUV, _AlphaMaskInvert, _AlphaMaskMode, _AlphaMaskScale, _AlphaMaskValue], alts:[_AlphaMap]}", 2D) = "white" { }
		[HideInInspector][Vector2]_AlphaMaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _AlphaMaskUV ("UV", Int) = 0
		[HideInInspector][ThryWideEnum(Off, 0, Replace, 1, Multiply, 2, Add, 3, Subtract, 4)]_AlphaMaskMode ("Blend Mode", Int) = 2
		[HideInInspector]_AlphaMaskScale ("Blend Strength", Float) = 1
		[HideInInspector]_AlphaMaskValue ("Blend Offset", Float) = 0
		[HideInInspector][ToggleUI]_AlphaMaskInvert ("Invert", Float) = 0
		_Cutoff ("Alpha Cutoff", Range(0, 1.001)) = 0.5
		
		//ifex _MainColorAdjustToggle==0
		[HideInInspector] m_start_ColorAdjust ("Color Adjust--{reference_property:_MainColorAdjustToggle,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/color-and-normals/color-adjust},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(COLOR_GRADING_HDR)] _MainColorAdjustToggle ("Adjust Colors", Float) = 0
		[sRGBWarning][ThryRGBAPacker(R Hue Mask, G Brightness Mask, B Saturation Mask, , linear, false)]_MainColorAdjustTexture ("Mask (Expand)--{reference_properties:[_MainColorAdjustTexturePan, _MainColorAdjustTextureUV]}", 2D) = "white" { }
		[HideInInspector][Vector2]_MainColorAdjustTexturePan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _MainColorAdjustTextureUV ("UV", Int) = 0
		_Saturation ("Saturation", Range(-1, 10)) = 0
		_MainBrightness ("Brightness", Range(-1, 1)) = 0
		
		[HideInInspector] s_start_MainHueShift ("Hue Shift--{reference_property:_MainHueShiftToggle,persistent_expand:true,default_expand:true}", Float) = 1
		[HideInInspector][ThryToggleUI(true)] _MainHueShiftToggle ("<size=13><b>  Hue Shift</b></size>", Float) = 0
		[ToggleUI]_MainHueShiftReplace ("Hue Replace?", Float) = 1
		_MainHueShift ("Hue Shift", Range(0, 1)) = 0
		_MainHueShiftSpeed ("Hue Shift Speed", Float) = 0
		
		[HideInInspector] s_start_MainHueShiftAL ("Hue Shift Audio Link--{reference_property:_MainHueALCTEnabled,persistent_expand:true,default_expand:false, condition_showS:(_EnableAudioLink==1)}", Float) = 0
		[HideInInspector][ThryToggleUI(true)]_MainHueALCTEnabled ("Hue Shift Audio Link", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)]_MainALHueShiftBand ("Band", Int) = 0
		[ThryWideEnum(Motion increases as intensity of band increases, 0, Above but Smooth, 1, Motion moves back and forth as a function of intensity, 2, Above but Smoooth, 3, Fixed speed increase when the band is dark Stationary when light, 4, Above but Smooooth, 5, Fixed speed increase when the band is dark Fixed speed decrease when light, 6, Above but Smoooooth, 7)]_MainALHueShiftCTIndex ("Motion Type", Int) = 0
		_MainHueALMotionSpeed ("Motion Speed", Float) = 1
		[HideInInspector] s_end_MainHueShiftAL ("Audio Link", Float) = 0
		[HideInInspector] s_end_MainHueShift ("Name Motion", Float) = 0
		
		[HideInInspector] s_start_ColorAdjustColorGrading ("Color Grading--{reference_property:_ColorGradingToggle, persistent_expand:true}", Float) = 0
		[HideInInspector][ToggleUI] _ColorGradingToggle ("Color Grading", Float) = 0
		[NoScaleOffset] _MainGradationTex ("Gradation Map", 2D) = "white" { }
		_MainGradationStrength ("Gradation Strength", Range(0, 1)) = 0
		[HideInInspector] s_end_ColorAdjustColorGrading ("Color Grading", Float) = 0
		
		[HideInInspector] s_start_MainHueShiftGlobalMask ("Global Mask--{persistent_expand:true}", Float) = 0
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _MainHueGlobalMask ("Hue--{reference_property:_MainHueGlobalMaskBlendType}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _MainHueGlobalMaskBlendType ("Blending", Int) = 2
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _MainSaturationGlobalMask ("Saturation--{reference_property:_MainSaturationGlobalMaskBlendType}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _MainSaturationGlobalMaskBlendType ("Blending", Int) = 2
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _MainBrightnessGlobalMask ("Brightness--{reference_property:_MainBrightnessGlobalMaskBlendType}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _MainBrightnessGlobalMaskBlendType ("Blending", Int) = 2
		[HideInInspector] s_end_MainHueShiftGlobalMask ("Global Mask", Float) = 0
		[HideInInspector] m_end_ColorAdjust ("Color Adjust", Float) = 0
		//endex
		
		[HideInInspector] m_start_Alpha ("Alpha Options--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/color-and-normals/alpha-options},hover:Documentation}}", Float) = 0
		[ToggleUI]_AlphaForceOpaque ("Force Opaque", Float) = 1
		_AlphaMod ("Alpha Mod", Range(-1, 1)) = 0.0
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _AlphaGlobalMask ("Global Mask--{reference_property:_AlphaGlobalMaskBlendType}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _AlphaGlobalMaskBlendType ("Blending", Int) = 2
		
		//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
		[HideInInspector] s_start_AlphaToCoverage ("Alpha To Coverage--{reference_property:_AlphaToCoverage,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI] _AlphaToCoverage ("A2CToggle", Float) = 0
		[ToggleUI]_AlphaSharpenedA2C ("Sharpened  A2C", Float) = 0
		_AlphaMipScale ("Mip Level Alpha Scale", Range(0, 1)) = 0.25
		[HideInInspector] s_end_AlphaToCoverage ("Alpha To Coverage", Float) = 0
		//endex
		
		//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
		[HideInInspector] s_start_AlphaDithering ("Dithering--{reference_property:_AlphaDithering,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI] _AlphaDithering ("Dithering", Float) = 0
		_AlphaDitherGradient ("Dither Gradient", Range(0, 1)) = .1
		_AlphaDitherBias ("Dither Bias", Range(0, 1)) = 0
		[HideInInspector] s_end_AlphaDithering ("Alpha To Coverage", Float) = 0
		//endex
		
		//ifex _AlphaDistanceFade==0 && isNotAnimated(_AlphaDistanceFade)
		[HideInInspector] s_start_AlphaDistanceFade ("Distance Alpha / Distance Fade--{reference_property:_AlphaDistanceFade,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI] _AlphaDistanceFade ("Distance Alpha", Float) = 0
		[Enum(Object Position, 0, Pixel Position, 1)] _AlphaDistanceFadeType ("Pos To Use", Int) = 1
		_AlphaDistanceFadeMinAlpha ("Min Distance Alpha", Range(0, 1)) = 0
		_AlphaDistanceFadeMaxAlpha ("Max Distance Alpha", Range(0, 1)) = 1
		_AlphaDistanceFadeMin ("Min Distance", Float) = 0
		_AlphaDistanceFadeMax ("Max Distance", Float) = 0
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _AlphaDistanceFadeGlobalMask ("Global Mask", Int) = 0
		[HideInInspector] s_end_AlphaDistanceFade ("Distance Alpha / Distance Fade", Float) = 0
		//endex
		
		//ifex _AlphaFresnel==0 && isNotAnimated(_AlphaFresnel)
		[HideInInspector] s_start_AlphaFresnel ("Fresnel Alpha--{reference_property:_AlphaFresnel,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI] _AlphaFresnel ("Fresnel Alpha", Float) = 0
		_AlphaFresnelAlpha ("Intensity", Range(0, 1)) = 0
		_AlphaFresnelSharpness ("Sharpness", Range(0, 1)) = .5
		_AlphaFresnelWidth ("Width", Range(0, 1)) = .5
		[ToggleUI]_AlphaFresnelInvert ("Invert", Float) = 0
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _AlphaFresnelGlobalMask ("Global Mask", Int) = 0
		[HideInInspector] s_end_AlphaFresnel ("Fresnel Alpha", Float) = 0
		//endex
		
		//ifex _AlphaAngular==0 && isNotAnimated(_AlphaAngular)
		[HideInInspector] s_start_AlphaAngular ("Angular Alpha--{reference_property:_AlphaAngular,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI] _AlphaAngular ("Angular Alpha", Float) = 0
		[Enum(Camera Face Model, 0, Model Face Camera, 1, Face Each Other, 2)] _AngleType ("Angle Type", Int) = 0
		[Enum(Model, 0, Vertex, 1)] _AngleCompareTo ("Model or Vert Positon", Int) = 0
		[Vector3]_AngleForwardDirection ("Forward Direction", Vector) = (0, 0, 1)
		_CameraAngleMin ("Camera Angle Min", Range(0, 180)) = 45
		_CameraAngleMax ("Camera Angle Max", Range(0, 180)) = 90
		_ModelAngleMin ("Model Angle Min", Range(0, 180)) = 45
		_ModelAngleMax ("Model Angle Max", Range(0, 180)) = 90
		_AngleMinAlpha ("Min Alpha", Range(0, 1)) = 0
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _AlphaAngularGlobalMask ("Global Mask", Int) = 0
		[HideInInspector] s_end_AlphaAngular ("Name", Float) = 0
		//endex
		
		//ifex _AlphaAudioLinkEnabled==0 && isNotAnimated(_AlphaAudioLinkEnabled)
		[HideInInspector] s_start_ALAlpha ("Alpha Audio Link--{reference_property:_AlphaAudioLinkEnabled,persistent_expand:true,default_expand:false, condition_showS:(_EnableAudioLink==1)}", Float) = 0
		[HideInInspector][ToggleUI]_AlphaAudioLinkEnabled ("Alpha Audio Link", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _AlphaAudioLinkAddBand ("Add Band", Int) = 0
		[VectorLabel(Min, Max)]_AlphaAudioLinkAddRange ("Add Range", Vector) = (0, 0, 0)
		[HideInInspector] s_end_ALAlpha ("Alpha Audio Link", Float) = 0
		
		[HideInInspector] s_start_AlphaAdvanced ("Advanced--{persistent_expand:true,default_expand:false}", Float) = 0
		[ToggleUI]_AlphaPremultiply ("Alpha Premultiply", Float) = 0
		_AlphaBoostFA ("Boost Transparency in ForwardAdd--{condition_showS:(_AddBlendOp==4)}", Range(1, 100)) = 10
		[HideInInspector] s_end_AlphaAdvanced ("Advanced", Float) = 0
		//endex
		[HideInInspector] m_end_Alpha ("Alpha Options", Float) = 0
		
		[HideInInspector] m_lightingCategory ("Shading", Float) = 0
		
		[HideInInspector] m_start_PoiLightData ("Light Data--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/shading/light-data},hover:Documentation}}", Float) = 0
		// Lighting Data
		[sRGBWarning]_LightingAOMaps ("AO Maps (expand)--{reference_properties:[_LightingAOMapsPan, _LightingAOMapsUV,_LightDataAOStrengthR,_LightDataAOStrengthG,_LightDataAOStrengthB,_LightDataAOStrengthA, _LightDataAOGlobalMaskR]}", 2D) = "white" { }
		[HideInInspector][Vector2]_LightingAOMapsPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _LightingAOMapsUV ("UV", Int) = 0
		[HideInInspector]_LightDataAOStrengthR ("R Strength", Range(0, 1)) = 1
		[HideInInspector]_LightDataAOStrengthG ("G Strength", Range(0, 1)) = 0
		[HideInInspector]_LightDataAOStrengthB ("B Strength", Range(0, 1)) = 0
		[HideInInspector]_LightDataAOStrengthA ("A Strength", Range(0, 1)) = 0
		[HideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _LightDataAOGlobalMaskR ("Global Mask--{reference_property:_LightDataAOGlobalMaskBlendTypeR}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _LightDataAOGlobalMaskBlendTypeR ("Blending", Range(0, 1)) = 2
		[sRGBWarning]_LightingDetailShadowMaps ("Shadow Map (expand)--{reference_properties:[_LightingDetailShadowMapsPan, _LightingDetailShadowMapsUV,_LightingDetailShadowStrengthR,_LightingDetailShadowStrengthG,_LightingDetailShadowStrengthB,_LightingDetailShadowStrengthA,_LightingAddDetailShadowStrengthR,_LightingAddDetailShadowStrengthG,_LightingAddDetailShadowStrengthB,_LightingAddDetailShadowStrengthA, _LightDataDetailShadowGlobalMaskR]}", 2D) = "white" { }
		[HideInInspector][Vector2]_LightingDetailShadowMapsPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _LightingDetailShadowMapsUV ("UV", Int) = 0
		[HideInInspector]_LightingDetailShadowStrengthR ("R Strength", Range(0, 1)) = 1
		[HideInInspector]_LightingDetailShadowStrengthG ("G Strength", Range(0, 1)) = 0
		[HideInInspector]_LightingDetailShadowStrengthB ("B Strength", Range(0, 1)) = 0
		[HideInInspector]_LightingDetailShadowStrengthA ("A Strength", Range(0, 1)) = 0
		[HideInInspector]_LightingAddDetailShadowStrengthR ("Additive R Strength", Range(0, 1)) = 1
		[HideInInspector]_LightingAddDetailShadowStrengthG ("Additive G Strength", Range(0, 1)) = 0
		[HideInInspector]_LightingAddDetailShadowStrengthB ("Additive B Strength", Range(0, 1)) = 0
		[HideInInspector]_LightingAddDetailShadowStrengthA ("Additive A Strength", Range(0, 1)) = 0
		[HideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _LightDataDetailShadowGlobalMaskR ("Global Mask--{reference_property:_LightDataDetailShadowGlobalMaskBlendTypeR}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _LightDataDetailShadowGlobalMaskBlendTypeR ("Blending", Range(0, 1)) = 2
		[sRGBWarning]_LightingShadowMasks ("Shadow Masks (expand)--{reference_properties:[_LightingShadowMasksPan, _LightingShadowMasksUV,_LightingShadowMaskStrengthR,_LightingShadowMaskStrengthG,_LightingShadowMaskStrengthB,_LightingShadowMaskStrengthA, _LightDataShadowMaskGlobalMaskR]}", 2D) = "white" { }
		[HideInInspector][Vector2]_LightingShadowMasksPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _LightingShadowMasksUV ("UV", Int) = 0
		[HideInInspector]_LightingShadowMaskStrengthR ("R Strength", Range(0, 1)) = 1
		[HideInInspector]_LightingShadowMaskStrengthG ("G Strength", Range(0, 1)) = 0
		[HideInInspector]_LightingShadowMaskStrengthB ("B Strength", Range(0, 1)) = 0
		[HideInInspector]_LightingShadowMaskStrengthA ("A Strength", Range(0, 1)) = 0
		[HideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _LightDataShadowMaskGlobalMaskR ("Global Mask--{reference_property:_LightDataShadowMaskGlobalMaskBlendTypeR}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)] _LightDataShadowMaskGlobalMaskBlendTypeR ("Blending", Range(0, 1)) = 2
		// Base Pass
		[HideInInspector] s_start_LightDataBasePass ("Base Pass (Directional & Baked Lights)--{persistent_expand:true,default_expand:true}", Float) = 1
		[Enum(Poi Custom, 0, Standard, 1, UTS2, 2, OpenLit(lil toon), 3)] _LightingColorMode ("Light Color Mode", Int) = 0
		[Enum(Poi Custom, 0, Normalized NDotL, 1, Saturated NDotL, 2, Casted Shadows Only, 3)] _LightingMapMode ("Light Map Mode", Int) = 0
		[Enum(Poi Custom, 0, Forced Local Direction, 1, Forced World Direction, 2, UTS2, 3, OpenLit(lil toon), 4, View Direction, 5)] _LightingDirectionMode ("Light Direction Mode", Int) = 0
		[Vector3]_LightngForcedDirection ("Forced Direction--{condition_showS:(_LightingDirectionMode==1 || _LightingDirectionMode==2)}", Vector) = (0, 0, 0)
		_LightingViewDirOffsetPitch ("View Dir Offset Pitch--{condition_showS:_LightingDirectionMode==5}", Range(-90, 90)) = 0
		_LightingViewDirOffsetYaw ("View Dir Offset Yaw--{condition_showS:_LightingDirectionMode==5}", Range(-90, 90)) = 0
		[ToggleUI]_LightingForceColorEnabled ("Force Light Color", Float) = 0
		_LightingForcedColor ("Forced Color--{condition_showS:(_LightingForceColorEnabled==1), reference_property:_LightingForcedColorThemeIndex}", Color) = (1, 1, 1)
		[HideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _LightingForcedColorThemeIndex ("", Int) = 0
		_Unlit_Intensity ("Unlit_Intensity--{condition_showS:(_LightingColorMode==2)}", Range(0.001, 4)) = 1
		[ToggleUI]_LightingCapEnabled ("Limit Brightness", Float) = 1
		_LightingCap ("Max Brightness--{condition_showS:(_LightingCapEnabled==1)}", Range(0, 10)) = 1
		_LightingMinLightBrightness ("Min Brightness", Range(0, 1)) = 0
		_LightingIndirectUsesNormals ("Indirect Uses Normals--{condition_showS:(_LightingColorMode==0)}", Range(0, 1)) = 0
		_LightingCastedShadows ("Receive Casted Shadows", Range(0, 1)) = 0
		_LightingMonochromatic ("Grayscale Lighting", Range(0, 1)) = 0
		[ToggleUI]_LightingVertexLightingEnabled ("Vertex lights (Non-Important)", Float) = 1
		[ToggleUI]_LightingMirrorVertexLightingEnabled ("Mirror Vertex lights (Non-Important)", Float) = 0
		[HideInInspector] s_end_LightDataBasePass ("Base Pass", Float) = 1
		// Lighting Additive
		[HideInInspector] s_start_LightDataAddPass ("Add Pass (Point & Spot lights)--{persistent_expand:true,default_expand:true}", Float) = 1
		[ToggleUI]_LightingAdditiveEnable ("Pixel lights (Important)", Float) = 1
		[ToggleUI]_DisableDirectionalInAdd ("Ignore Directional--{condition_showS:(_LightingAdditiveEnable==1)}", Float) = 1
		[ToggleUI]_LightingAdditiveLimited ("Limit Brightness", Float) = 1
		_LightingAdditiveLimit ("Max Brightness--{condition_showS:(_LightingAdditiveLimited==1)}", Range(0, 10)) = 1
		_LightingAdditiveCastedShadows ("Receive Casted Shadows", Range(0, 1)) = 1
		_LightingAdditiveMonochromatic ("Grayscale Lighting", Range(0, 1)) = 0
		_LightingAdditivePassthrough ("Point Light Passthrough--{condition_showS:(_LightingAdditiveEnable==1)}", Range(0, 1)) = .5
		[HideInInspector] s_end_LightDataAddPass ("Add Pass", Float) = 1
		// Lighting Data Debug
		[HideInInspector] s_start_LightDataDebug ("Debug / Data Visualizations--{reference_property:_LightDataDebugEnabled,persistent_expand:true}", Float) = 0
		[HideInInspector][NoAnimate][ThryToggleUI(false)]_LightDataDebugEnabled ("Debug", Float) = 0
		[ThryWideEnum(Direct Color, 0, Indirect Color, 1, Light Map, 2, Attenuation, 3, N Dot L, 4, Half Dir, 5, Direction, 6, Add Color, 7, Add Attenuation, 8, Add Shadow, 9, Add N Dot L, 10)] _LightingDebugVisualize ("Visualize", Int) = 0
		[HideInInspector] s_end_LightDataDebug ("Debug", Float) = 0
		[HideInInspector] m_end_PoiLightData ("Light Data", Float) = 0
		
		[HideInInspector] m_OutlineCategory (" Outlines--{reference_property:_EnableOutlines,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/outlines/main},hover:Documentation}}", Float) = 0
		
		[HideInInspector] m_specialFXCategory ("Special FX", Float) = 0
		//ifex _EnableEmission==0
		//Emission 1
		[HideInInspector] m_start_emissionOptions ("Emission 0--{reference_property:_EnableEmission,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/special-fx/emission},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(_EMISSION)]_EnableEmission ("Enable Emission--{alts:[_UseEmission]}", Float) = 0
		[sRGBWarning]_EmissionMask ("Emission Mask--{reference_properties:[_EmissionMaskPan, _EmissionMaskUV, _EmissionMaskChannel, _EmissionMaskInvert, _EmissionMask0GlobalMask]}", 2D) = "white" { }
		[HideInInspector][Vector2]_EmissionMaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _EmissionMaskUV ("UV", Int) = 0
		[HideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_EmissionMaskChannel ("Channel", Float) = 0
		[HideInInspector][ToggleUI]_EmissionMaskInvert ("Invert", Float) = 0
		[HideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _EmissionMask0GlobalMask ("Global Mask--{reference_property:_EmissionMask0GlobalMaskBlendType}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)]_EmissionMask0GlobalMaskBlendType ("Blending", Range(0, 1)) = 2
		
		[HDR]_EmissionColor ("Emission Color--{reference_property:_EmissionColorThemeIndex}", Color) = (1, 1, 1, 1)
		[HideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _EmissionColorThemeIndex ("", Int) = 0
		[sRGBWarning(true)][Gradient]_EmissionMap ("Emission Map--{reference_properties:[_EmissionMapPan, _EmissionMapUV]}", 2D) = "white" { }
		[HideInInspector][Vector2]_EmissionMapPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _EmissionMapUV ("UV", Int) = 0
		_EmissionStrength ("Emission Strength", Range(0, 20)) = 0
		[ToggleUI]_EmissionBaseColorAsMap ("Use Base Colors", Float) = 0
		[ToggleUI]_EmissionReplace0 ("Override Base Color", Float) = 0
		
		[HideInInspector] s_start_EmissionHueShift0 ("Hue Shift--{reference_property:_EmissionHueShiftEnabled,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionHueShiftEnabled ("Hue Shift", Float) = 0
		_EmissionHueShift ("Hue Shift", Range(0, 1)) = 0
		_EmissionHueShiftSpeed ("Hue Shift Speed", Float) = 0
		[HideInInspector] s_end_EmissionHueShift0 ("", Float) = 0
		
		// Center out emission
		[HideInInspector] s_start_EmissionCenterOut0 ("Center Out--{reference_property:_EmissionCenterOutEnabled,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionCenterOutEnabled ("Center Out", Float) = 0
		_EmissionCenterOutSpeed ("Flow Speed", Float) = 5
		[HideInInspector] s_end_EmissionCenterOut0 ("", Float) = 0
		
		// Glow in the dark Emission
		[HideInInspector] s_start_EmissionLightBased0 ("Light Based--{reference_property:_EnableGITDEmission,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EnableGITDEmission ("Light Based", Float) = 0
		[Enum(World, 0, Mesh, 1)] _GITDEWorldOrMesh ("Lighting Type", Int) = 0
		_GITDEMinEmissionMultiplier ("Min Emission Multiplier", Range(0, 1)) = 1
		_GITDEMaxEmissionMultiplier ("Max Emission Multiplier", Range(0, 1)) = 0
		_GITDEMinLight ("Min Lighting", Range(0, 1)) = 0
		_GITDEMaxLight ("Max Lighting", Range(0, 1)) = 1
		[HideInInspector] s_end_EmissionLightBased0 ("", Float) = 0
		
		// Blinking Emission
		[HideInInspector] s_start_EmissionBlinking0 ("Blinking--{reference_property:_EmissionBlinkingEnabled,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionBlinkingEnabled ("Blinking", Float) = 0
		_EmissiveBlink_Min ("Emissive Blink Min", Float) = 0
		_EmissiveBlink_Max ("Emissive Blink Max", Float) = 1
		_EmissiveBlink_Velocity ("Emissive Blink Velocity", Float) = 4
		_EmissionBlinkingOffset ("Offset", Float) = 0
		[HideInInspector] s_end_EmissionBlinking0 ("", Float) = 0
		
		// Scrolling Emission
		[HideInInspector] s_start_ScrollingEmission0 ("Scrolling--{reference_property:_ScrollingEmission,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI] _ScrollingEmission ("Scrolling", Float) = 0
		[ToggleUI]_EmissionScrollingUseCurve ("Use Curve", float) = 0
		[Curve]_EmissionScrollingCurve ("Curve--{condition_showS:(_EmissionScrollingUseCurve==1)}", 2D) = "white" { }
		[ToggleUI]_EmissionScrollingVertexColor ("VColor as position", float) = 0
		_EmissiveScroll_Direction ("Direction", Vector) = (0, -10, 0, 0)
		_EmissiveScroll_Width ("Width", Float) = 10
		_EmissiveScroll_Velocity ("Velocity", Float) = 10
		_EmissiveScroll_Interval ("Interval", Float) = 20
		_EmissionScrollingOffset ("Offset", Float) = 0
		[HideInInspector] s_end_ScrollingEmission0 ("", Float) = 0
		
		[Space(4)]
		[ThryToggleUI(true)] _EmissionAL0Enabled ("<size=13><b>  Audio Link</b></size>--{ condition_showS:_EnableAudioLink==1}", Float) = 0
		[HideInInspector] s_start_EmissionAL0Multiply ("Strength Multiply--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL0Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL0MultipliersBand ("Band", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL0Multipliers ("Multiplier", Vector) = (1, 1, 0, 0)
		[HideInInspector] s_end_EmissionAL0Multiply ("Strength Multiply", Float) = 0
		
		[HideInInspector] s_start_EmissionAL0Add ("Strength Add--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL0Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL0StrengthBand ("Band", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL0StrengthMod ("Strength", Vector) = (0, 0, 0, 0)
		[HideInInspector] s_end_EmissionAL0Add ("Strength Add", Float) = 0
		
		[HideInInspector] s_start_EmissionAL0COut ("Center Out--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL0Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _AudioLinkEmission0CenterOutBand ("Band", Int) = 0
		[VectorLabel(Min, Max)] _AudioLinkEmission0CenterOut ("Strength", Vector) = (0, 0, 0, 0)
		_AudioLinkEmission0CenterOutSize ("Intensity Threshold", Range(0, 1)) = 0
		_AudioLinkEmission0CenterOutDuration ("Duration", Range(-1, 1)) = 1
		[HideInInspector] s_end_EmissionAL0COut ("Center Out", Float) = 0
		[HideInInspector] m_end_emissionOptions ("", Float) = 0
		//endex
		//ifex _EnableEmission1==0
		// Second Emission
		[HideInInspector] m_start_emission1Options ("Emission 1--{reference_property:_EnableEmission1,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/special-fx/emission},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(POI_EMISSION_1)]_EnableEmission1 ("Enable Emission 2", Float) = 0
		[sRGBWarning]_EmissionMask1 ("Emission Mask--{reference_properties:[_EmissionMask1Pan, _EmissionMask1UV, _EmissionMask1Channel, _EmissionMaskInvert1, _EmissionMask1GlobalMask]}", 2D) = "white" { }
		[HideInInspector][Vector2]_EmissionMask1Pan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _EmissionMask1UV ("UV", Int) = 0
		[HideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_EmissionMask1Channel ("Channel", Float) = 0
		[HideInInspector][ToggleUI]_EmissionMaskInvert1 ("Invert", Float) = 0
		[HideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _EmissionMask1GlobalMask ("Global Mask--{reference_property:_EmissionMask1GlobalMaskBlendType}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)]_EmissionMask1GlobalMaskBlendType ("Blending", Range(0, 1)) = 2
		[HDR]_EmissionColor1 ("Emission Color--{reference_property:_EmissionColor1ThemeIndex}", Color) = (1, 1, 1, 1)
		[HideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _EmissionColor1ThemeIndex ("", Int) = 0
		[sRGBWarning(true)][Gradient]_EmissionMap1 ("Emission Map--{reference_properties:[_EmissionMap1Pan, _EmissionMap1UV]}", 2D) = "white" { }
		[HideInInspector][Vector2]_EmissionMap1Pan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _EmissionMap1UV ("UV", Int) = 0
		_EmissionStrength1 ("Emission Strength", Range(0, 20)) = 0
		[ToggleUI]_EmissionBaseColorAsMap1 ("Use Base Colors", Float) = 0
		[ToggleUI]_EmissionReplace1 ("Override Base Color", Float) = 0
		
		[HideInInspector] s_start_EmissionHueShift1 ("Hue Shift--{reference_property:_EmissionHueShiftEnabled1,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionHueShiftEnabled1 ("Hue Shift", Float) = 0
		_EmissionHueShift1 ("Hue Shift", Range(0, 1)) = 0
		_EmissionHueShiftSpeed1 ("Hue Shift Speed", Float) = 0
		[HideInInspector] s_end_EmissionHueShift1 ("", Float) = 0
		
		// Second Center Out Enission
		[HideInInspector] s_start_EmissionCenterOutEnabled1 ("Center Out--{reference_property:_EmissionCenterOutEnabled1,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionCenterOutEnabled1 ("Center Out", Float) = 0
		_EmissionCenterOutSpeed1 ("Flow Speed", Float) = 5
		[HideInInspector] s_end_EmissionCenterOutEnabled1 ("", Float) = 0
		
		// Second Glow In The Dark Emission
		[HideInInspector] s_start_EmissionLightBased1 ("Light Based--{reference_property:_EnableGITDEmission1,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EnableGITDEmission1 ("Light Based", Float) = 0
		[Enum(World, 0, Mesh, 1)] _GITDEWorldOrMesh1 ("Lighting Type", Int) = 0
		_GITDEMinEmissionMultiplier1 ("Min Emission Multiplier", Range(0, 1)) = 1
		_GITDEMaxEmissionMultiplier1 ("Max Emission Multiplier", Range(0, 1)) = 0
		_GITDEMinLight1 ("Min Lighting", Range(0, 1)) = 0
		_GITDEMaxLight1 ("Max Lighting", Range(0, 1)) = 1
		[HideInInspector] s_end_EmissionLightBased1 ("", Float) = 0
		
		// Second Blinking Emission
		[HideInInspector] s_start_EmissionBlinking1 ("Blinking--{reference_property:_EmissionBlinkingEnabled1,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionBlinkingEnabled1 ("Blinking", Float) = 0
		_EmissiveBlink_Min1 ("Emissive Blink Min", Float) = 0
		_EmissiveBlink_Max1 ("Emissive Blink Max", Float) = 1
		_EmissiveBlink_Velocity1 ("Emissive Blink Velocity", Float) = 4
		_EmissionBlinkingOffset1 ("Offset", Float) = 0
		[HideInInspector] s_end_EmissionBlinking1 ("", Float) = 0
		
		// Second Scrolling Emission
		[HideInInspector] s_start_EmissionScrolling1 ("Scrolling--{reference_property:_ScrollingEmission1,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI] _ScrollingEmission1 ("Scrolling", Float) = 0
		[ToggleUI]_EmissionScrollingUseCurve1 ("Use Curve", float) = 0
		[Curve]_EmissionScrollingCurve1 ("Curve--{condition_showS:(_EmissionScrollingUseCurve1==1)}", 2D) = "white" { }
		[ToggleUI]_EmissionScrollingVertexColor1 ("VColor as position", float) = 0
		_EmissiveScroll_Direction1 ("Direction", Vector) = (0, -10, 0, 0)
		_EmissiveScroll_Width1 ("Width", Float) = 10
		_EmissiveScroll_Velocity1 ("Velocity", Float) = 10
		_EmissiveScroll_Interval1 ("Interval", Float) = 20
		_EmissionScrollingOffset1 ("Offset", Float) = 0
		[HideInInspector] s_end_EmissionScrolling1 ("", Float) = 0
		
		[Space(4)]
		[ThryToggleUI(true)] _EmissionAL1Enabled ("<size=13><b>  Audio Link</b></size>--{ condition_showS:_EnableAudioLink==1}", Float) = 0
		[HideInInspector] s_start_EmissionAL1Multiply ("Strength Multiply--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL1Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL1MultipliersBand ("Band", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL1Multipliers ("Multiplier", Vector) = (1, 1, 0, 0)
		[HideInInspector] s_end_EmissionAL1Multiply ("Strength Multiply", Float) = 0
		
		[HideInInspector] s_start_EmissionAL1Add ("Strength Add--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL1Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL1StrengthBand ("Band", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL1StrengthMod ("Strength Add", Vector) = (0, 0, 0, 0)
		[HideInInspector] s_end_EmissionAL1Add ("Strength Add", Float) = 0
		
		[HideInInspector] s_start_EmissionAL1COut ("Center Out--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL1Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _AudioLinkEmission1CenterOutBand ("Band", Int) = 0
		[VectorLabel(Min, Max)] _AudioLinkEmission1CenterOut ("Strength", Vector) = (0, 0, 0, 0)
		_AudioLinkEmission1CenterOutSize ("Intensity Threshold", Range(0, 1)) = 0
		_AudioLinkEmission1CenterOutDuration ("Duration--{tooltip:''How much AL history is used. Negative values reverse direction'', condition_showS:(_EmissionAL1Enabled==1 && _EnableAudioLink==1)}", Range(-1, 1)) = 1
		[HideInInspector] s_end_EmissionAL1COut ("Center Out", Float) = 0
		
		[HideInInspector] m_end_emission1Options ("", Float) = 0
		//endex
		//ifex _EnableEmission2==0
		// Third Emission
		[HideInInspector] m_start_emission2Options ("Emission 2--{reference_property:_EnableEmission2,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/special-fx/emission},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(POI_EMISSION_2)]_EnableEmission2 ("Enable Emission 2", Float) = 0
		[sRGBWarning]_EmissionMask2 ("Emission Mask--{reference_properties:[_EmissionMask2Pan, _EmissionMask2UV, _EmissionMask2Channel, _EmissionMaskInvert2, _EmissionMask2GlobalMask]}", 2D) = "white" { }
		[HideInInspector][Vector2]_EmissionMask2Pan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _EmissionMask2UV ("UV", Int) = 0
		[HideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_EmissionMask2Channel ("Channel", Float) = 0
		[HideInInspector][ToggleUI]_EmissionMaskInvert2 ("Invert", Float) = 0
		[HideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _EmissionMask2GlobalMask ("Global Mask--{reference_property:_EmissionMask2GlobalMaskBlendType}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)]_EmissionMask2GlobalMaskBlendType ("Blending", Range(0, 1)) = 2
		[HDR]_EmissionColor2 ("Emission Color--{reference_property:_EmissionColor2ThemeIndex}", Color) = (1, 1, 1, 1)
		[HideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _EmissionColor2ThemeIndex ("", Int) = 0
		[Gradient]_EmissionMap2 ("Emission Map--{reference_properties:[_EmissionMap2Pan, _EmissionMap2UV]}", 2D) = "white" { }
		[HideInInspector][Vector2]_EmissionMap2Pan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _EmissionMap2UV ("UV", Int) = 0
		_EmissionStrength2 ("Emission Strength", Range(0, 20)) = 0
		[ToggleUI]_EmissionBaseColorAsMap2 ("Use Base Colors", Float) = 0
		[ToggleUI]_EmissionReplace2 ("Override Base Color", Float) = 0
		
		[HideInInspector] s_start_EmissionHueShift2 ("Hue Shift--{reference_property:_EmissionHueShiftEnabled2,persistent_expand:true,default_expand:false}", Float) = 0
		[ToggleUI]_EmissionHueShiftEnabled2 ("Hue Shift", Float) = 0
		_EmissionHueShift2 ("Hue Shift", Range(0, 1)) = 0
		_EmissionHueShiftSpeed2 ("Hue Shift Speed", Float) = 0
		[HideInInspector] s_end_EmissionHueShift2 ("", Float) = 0
		
		// Third Center Out Enission
		[HideInInspector] s_start_EmissionCenterOut2 ("Center Out--{reference_property:_EmissionCenterOutEnabled2,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionCenterOutEnabled2 ("Center Out", Float) = 0
		_EmissionCenterOutSpeed2 ("Flow Speed", Float) = 5
		[HideInInspector] s_end_EmissionCenterOutEnabled2 ("", Float) = 0
		
		// Third Glow In The Dark Emission
		[HideInInspector] s_start_EmissionLightBased2 ("Light Based--{reference_property:_EnableGITDEmission2,persistent_expand:true,default_expand:false}", Float) = 0
		[ToggleUI]_EnableGITDEmission2 ("Light Based", Float) = 0
		[Enum(World, 0, Mesh, 1)] _GITDEWorldOrMesh2 ("Lighting Type", Int) = 0
		_GITDEMinEmissionMultiplier2 ("Min Emission Multiplier", Range(0, 1)) = 1
		_GITDEMaxEmissionMultiplier2 ("Max Emission Multiplier", Range(0, 1)) = 0
		_GITDEMinLight2 ("Min Lighting", Range(0, 1)) = 0
		_GITDEMaxLight2 ("Max Lighting", Range(0, 1)) = 1
		[HideInInspector] s_end_EmissionLightBased2 ("", Float) = 0
		
		// Third Blinking Emission
		[HideInInspector] s_start_EmissionBlinking2 ("Blinking--{reference_property:_EmissionBlinkingEnabled2,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionBlinkingEnabled2 ("Blinking", Float) = 0
		_EmissiveBlink_Min2 ("Emissive Blink Min", Float) = 0
		_EmissiveBlink_Max2 ("Emissive Blink Max", Float) = 1
		_EmissiveBlink_Velocity2 ("Emissive Blink Velocity", Float) = 4
		_EmissionBlinkingOffset2 ("Offset", Float) = 0
		[HideInInspector] s_end_EmissionBlinking2 ("", Float) = 0
		
		// Third Scrolling Emission
		[HideInInspector] s_start_EmissionScrolling2 ("Scrolling--{reference_property:_ScrollingEmission2,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI] _ScrollingEmission2 ("Scrolling", Float) = 0
		[ToggleUI]_EmissionScrollingUseCurve2 ("Use Curve", float) = 0
		[Curve]_EmissionScrollingCurve2 ("Curve--{condition_showS:(_EmissionScrollingUseCurve2==1)}", 2D) = "white" { }
		[ToggleUI]_EmissionScrollingVertexColor2 ("VColor as position", float) = 0
		_EmissiveScroll_Direction2 ("Direction", Vector) = (0, -10, 0, 0)
		_EmissiveScroll_Width2 ("Width", Float) = 10
		_EmissiveScroll_Velocity2 ("Velocity", Float) = 10
		_EmissiveScroll_Interval2 ("Interval", Float) = 20
		_EmissionScrollingOffset2 ("Offset", Float) = 0
		[HideInInspector] s_end_EmissionScrolling2 ("", Float) = 0
		
		[Space(4)]
		[ThryToggleUI(true)] _EmissionAL2Enabled ("<size=13><b>  Audio Link</b></size>--{ condition_showS:_EnableAudioLink==1}", Float) = 0
		[HideInInspector]s_start_EmissionAL2Multiply ("Strength Multiply--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL2Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL2MultipliersBand ("Emission Multiplier Band", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL2Multipliers ("Emission Multiplier", Vector) = (1, 1, 0, 0)
		[HideInInspector]s_end_EmissionAL2Multiply ("Strength Multiply", Float) = 0
		
		[HideInInspector]s_start_EmissionAL2Add ("Strength Add--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL2Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL2StrengthBand ("Band", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL2StrengthMod ("Strength Add", Vector) = (0, 0, 0, 0)
		[HideInInspector]s_end_EmissionAL2Add ("Strength Add", Float) = 0
		
		[HideInInspector]s_start_EmissionAL2COut ("Center Out--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL2Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _AudioLinkEmission2CenterOutBand ("Band", Int) = 0[VectorLabel(Min, Max)]
		_AudioLinkEmission2CenterOut ("Strength", Vector) = (0, 0, 0, 0)
		_AudioLinkEmission2CenterOutSize ("Intensity Threshold", Range(0, 1)) = 0
		_AudioLinkEmission2CenterOutDuration ("Duration--{tooltip:''How much AL history is used. Negative values reverse direction''}", Range(-1, 1)) = 1
		[HideInInspector]s_end_EmissionAL2COut ("Center Out", Float) = 0
		[HideInInspector] m_end_emission2Options ("", Float) = 0
		//endex
		
		//ifex _EnableEmission3==0
		// Fourth Emission
		[HideInInspector] m_start_emission3Options ("Emission 3--{reference_property:_EnableEmission3,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/special-fx/emission},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(POI_EMISSION_3)]_EnableEmission3 ("Enable Emission 3", Float) = 0
		[sRGBWarning]_EmissionMask3 ("Emission Mask--{reference_properties:[_EmissionMask3Pan, _EmissionMask3UV, _EmissionMask3Channel, _EmissionMaskInvert3, _EmissionMask3GlobalMask]}", 2D) = "white" { }
		[HideInInspector][Vector2]_EmissionMask3Pan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _EmissionMask3UV ("UV", Int) = 0
		[HideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_EmissionMask3Channel ("Channel", Float) = 0
		[HideInInspector][ToggleUI]_EmissionMaskInvert3 ("Invert", Float) = 0
		[HideInInspector][ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _EmissionMask3GlobalMask ("Global Mask--{reference_property:_EmissionMask3GlobalMaskBlendType}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)]_EmissionMask3GlobalMaskBlendType ("Blending", Range(0, 1)) = 2
		[HDR]_EmissionColor3 ("Emission Color--{reference_property:_EmissionColor3ThemeIndex}", Color) = (1, 1, 1, 1)
		[HideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _EmissionColor3ThemeIndex ("", Int) = 0
		[sRGBWarning(true)][Gradient]_EmissionMap3 ("Emission Map--{reference_properties:[_EmissionMap3Pan, _EmissionMap3UV]}", 2D) = "white" { }
		[HideInInspector][Vector2]_EmissionMap3Pan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _EmissionMap3UV ("UV", Int) = 0
		_EmissionStrength3 ("Emission Strength", Range(0, 20)) = 0
		[ToggleUI]_EmissionBaseColorAsMap3 ("Use Base Colors", Float) = 0
		[ToggleUI]_EmissionReplace3 ("Override Base Color", Float) = 0
		
		[HideInInspector] s_start_EmissionHueShift3 ("Hue Shift--{reference_property:_EmissionHueShiftEnabled3,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionHueShiftEnabled3 ("Hue Shift", Float) = 0
		_EmissionHueShift3 ("Hue Shift", Range(0, 1)) = 0
		_EmissionHueShiftSpeed3 ("Hue Shift Speed", Float) = 0
		[HideInInspector] s_end_EmissionHueShift3 ("", Float) = 0
		
		// Fourth Center Out Enission
		[HideInInspector] s_start_EmissionCenterOutEnabled3 ("Center Out--{reference_property:_EmissionCenterOutEnabled3,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionCenterOutEnabled3 ("Center Out", Float) = 0
		_EmissionCenterOutSpeed3 ("Flow Speed", Float) = 5
		[HideInInspector] s_end_EmissionCenterOutEnabled3 ("", Float) = 0
		
		// Fourth Glow In The Dark Emission
		[HideInInspector] s_start_EmissionLightBased3 ("Light Based--{reference_property:_EnableGITDEmission2,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EnableGITDEmission3 ("Light Based", Float) = 0
		[Enum(World, 0, Mesh, 1)] _GITDEWorldOrMesh3 ("Lighting Type", Int) = 0
		_GITDEMinEmissionMultiplier3 ("Min Emission Multiplier", Range(0, 1)) = 1
		_GITDEMaxEmissionMultiplier3 ("Max Emission Multiplier", Range(0, 1)) = 0
		_GITDEMinLight3 ("Min Lighting", Range(0, 1)) = 0
		_GITDEMaxLight3 ("Max Lighting", Range(0, 1)) = 1
		[HideInInspector] s_end_EmissionLightBased3 ("", Float) = 0
		
		// Fourth Blinking Emission
		[HideInInspector] s_start_EmissionBlinking3 ("Blinking--{reference_property:_EmissionBlinkingEnabled3,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_EmissionBlinkingEnabled3 ("Blinking", Float) = 0
		_EmissiveBlink_Min3 ("Emissive Blink Min", Float) = 0
		_EmissiveBlink_Max3 ("Emissive Blink Max", Float) = 1
		_EmissiveBlink_Velocity3 ("Emissive Blink Velocity", Float) = 4
		_EmissionBlinkingOffset3 ("Offset", Float) = 0
		[HideInInspector] s_end_EmissionBlinking3 ("", Float) = 0
		
		// Fourth Scrolling Emission
		[HideInInspector] s_start_EmissionScrolling3 ("Scrolling--{reference_property:_ScrollingEmission3,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI] _ScrollingEmission3 ("Scrolling", Float) = 0
		[ToggleUI]_EmissionScrollingUseCurve3 ("Use Curve", float) = 0
		[Curve]_EmissionScrollingCurve3 ("Curve--{condition_showS:(_EmissionScrollingUseCurve3==1)}", 2D) = "white" { }
		[ToggleUI]_EmissionScrollingVertexColor3 ("VColor as position", float) = 0
		_EmissiveScroll_Direction3 ("Direction", Vector) = (0, -10, 0, 0)
		_EmissiveScroll_Width3 ("Width", Float) = 10
		_EmissiveScroll_Velocity3 ("Velocity", Float) = 10
		_EmissiveScroll_Interval3 ("Interval", Float) = 20
		_EmissionScrollingOffset3 ("Offset", Float) = 0
		[HideInInspector] s_end_EmissionScrolling3 ("", Float) = 0
		
		[Space(4)]
		[ThryToggleUI(true)] _EmissionAL3Enabled ("<size=13><b>  Audio Link</b></size>--{ condition_showS:_EnableAudioLink==1}", Float) = 0
		
		[HideInInspector] s_start_EmissionAL3Multiply ("Strength Multiply--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL3Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL3MultipliersBand ("Bands", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL3Multipliers ("Multipliers", Vector) = (1, 1, 0, 0)
		[HideInInspector] s_end_EmissionAL3Multiply ("Strength Multiply", Float) = 0
		
		[HideInInspector] s_start_EmissionAL3Add ("Strength Add--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL3Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _EmissionAL3StrengthBand ("Strength Add Bands", Int) = 0
		[VectorLabel(Min, Max)]_EmissionAL3StrengthMod ("Strength Adds", Vector) = (0, 0, 0, 0)
		[HideInInspector] s_end_EmissionAL3Add ("Strength Add", Float) = 0
		
		[HideInInspector] s_start_EmissionAL3COut ("Center Out--{persistent_expand:true,default_expand:false, condition_showS:(_EmissionAL3Enabled==1 && _EnableAudioLink==1)}", Float) = 0
		[Enum(Bass, 0, Low Mid, 1, High Mid, 2, Treble, 3, Volume, 4)] _AudioLinkEmission3CenterOutBand ("Bands", Int) = 0
		[VectorLabel(Min, Max)] _AudioLinkEmission3CenterOut ("Strengths", Vector) = (0, 0, 0, 0)
		_AudioLinkEmission3CenterOutSize ("Intensity Thresholds", Range(0, 1)) = 0
		_AudioLinkEmission3CenterOutDuration ("Duration", Range(-1, 1)) = 1
		[HideInInspector] s_end_EmissionAL3COut ("Center Out", Float) = 0
		
		[HideInInspector] m_end_emission3Options ("", Float) = 0
		//endex
		
		// Glitter
		//ifex _GlitterEnable==0
		[HideInInspector] m_start_glitter ("Glitter / Sparkle--{reference_property:_GlitterEnable,button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/special-fx/glitter},hover:Documentation}}", Float) = 0
		[HideInInspector][ThryToggle(_SUNDISK_SIMPLE)]_GlitterEnable ("Enable Glitter", Float) = 0
		[ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _GlitterUV ("UV", Int) = 0
		[Enum(Angle, 0, Linear Emission, 1, Light Reflections, 2)]_GlitterMode ("Mode", Int) = 0
		[Enum(Circle, 0, Square, 1)]_GlitterShape ("Shape", Int) = 0
		[Enum(Add, 0, Replace, 1)] _GlitterBlendType ("Blend Mode", Int) = 0
		_GlitterUseNormals ("Use Normals", Range(0, 1)) = 0
		[IntRange]_GlitterLayers ("Layers", Range(1, 4)) = 2
		
		[HideInInspector] s_start_GlitterColorAndShape ("Shape & Color--{persistent_expand:true,default_expand:true}", Float) = 1
		_GlitterTexture ("Shape Texture", 2D) = "white" { }
		[sRGBWarning(true)]_GlitterColorMap ("Color Map--{reference_properties:[_GlitterColorMapPan, _GlitterColorMapUV]}", 2D) = "white" { }
		[HideInInspector][Vector2]_GlitterColorMapPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _GlitterColorMapUV ("UV", Int) = 0
		[HDR]_GlitterColor ("Color--{reference_property:_GlitterColorThemeIndex}", Color) = (1, 1, 1)
		[HideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _GlitterColorThemeIndex ("", Int) = 0
		_GlitterUseSurfaceColor ("Use Base Color", Range(0, 1)) = 0
		[ToggleUI]_GlitterRandomColors ("Random Colors", Float) = 0
		[MultiSlider]_GlitterMinMaxSaturation ("Saturation Range--{condition_showS:(_GlitterRandomColors==1)}", Vector) = (0.8, 1, 0, 1)
		[MultiSlider]_GlitterMinMaxBrightness ("Brightness Range--{condition_showS:(_GlitterRandomColors==1)}", Vector) = (0.8, 1, 0, 1)
		[HideInInspector] s_end_GlitterColorAndShape ("Color", Float) = 0
		
		[HideInInspector] s_start_GlitterPositionSize ("Position & Size--{persistent_expand:true,default_expand:true}", Float) = 01
		_GlitterFrequency ("Glitter Density", Float) = 300.0
		_GlitterSize ("Glitter Size--{condition_show:(_GlitterRandomSize==0)}", Range(0, 1)) = .3
		[Vector2]_GlitterUVPanning ("Panning", Vector) = (0, 0, 0, 0)
		[ToggleUI]_GlitterRandomLocation ("Random Position", Float) = 1.0
		[ToggleUI]_GlitterRandomSize ("Random Size", Float) = 0
		[MultiSlider]_GlitterMinMaxSize ("Size Range--{condition_show:(_GlitterRandomSize==1)}", Vector) = (0.1, 0.5, 0, 1)
		[HideInInspector] s_end_GlitterPositionSize ("Position & Size", Float) = 0
		
		[HideInInspector] s_start_GlitterSparkleControl ("Sparkle Control--{persistent_expand:true,default_expand:true}", Float) = 1
		_GlitterMinBrightness ("Glitter Min Brightness", Range(0, 1)) = 0
		_GlitterBrightness ("Glitter Max Brightness", Range(0, 40)) = 3
		_GlitterSpeed ("Speed", Float) = 10.0
		_GlitterAngleRange ("Glitter Angle Range--{condition_showS:(_GlitterMode==0||_GlitterMode==2)}", Range(0, 90)) = 90
		_GlitterBias ("Glitter Bias--{condition_show:(_GlitterMode==0)}", Range(0, 1)) = .8
		_GlitterCenterSize ("dim light--{condition_show:{type:AND,condition1:{type:PROPERTY_BOOL,data:_GlitterMode==1},condition2:{type:PROPERTY_BOOL,data:_GlitterShape==1}}}", Range(0, 1)) = .08
		_GlitterContrast ("Post Contrast--{condition_showS:(_GlitterMode==0||_GlitterMode==2)}", Range(1, 1000)) = 300
		_GlitterJaggyFix ("Distant Jaggy Fix--{condition_show:{type:PROPERTY_BOOL,data:_GlitterShape==1}}", Range(0, .1)) = .0
		[HideInInspector] s_end_GlitterSparkleControl ("Sparkle Control", Float) = 0
		
		[HideInInspector] s_start_GlitterRotationSection ("Rotations--{persistent_expand:true,default_expand:false}", Float) = 0
		[ToggleUI]_GlitterRandomRotation ("Random Offset", Float) = 0
		_GlitterTextureRotation ("Constant Speed", Float) = 0
		[VectorLabel(Min, Max)]_GlitterRandomRotationSpeed ("Random Speed Range", Vector) = (0, 0, 0, 0)
		[HideInInspector] s_end_GlitterRotationSection ("Rotations", Float) = 0
		
		[HideInInspector] s_start_GlitterMask ("Masking & Light Masking--{persistent_expand:true,default_expand:false}", Float) = 0
		[sRGBWarning]_GlitterMask ("Glitter Mask--{reference_properties:[_GlitterMaskPan, _GlitterMaskUV, _GlitterMaskChannel, _GlitterMaskInvert]}", 2D) = "white" { }
		[HideInInspector][Vector2]_GlitterMaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)] _GlitterMaskUV ("UV", Int) = 0
		[HideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_GlitterMaskChannel ("Channel", Float) = 0
		[HideInInspector][ToggleUI]_GlitterMaskInvert ("Invert", Float) = 0
		_GlitterHideInShadow ("Hide in shadow", Range(0, 1)) = 0
		_GlitterScaleWithLighting ("Scale With Lighting", Range(0, 1)) = 0
		[ThryWideEnum(Off, 0, 1R, 1, 1G, 2, 1B, 3, 1A, 4, 2R, 5, 2G, 6, 2B, 7, 2A, 8, 3R, 9, 3G, 10, 3B, 11, 3A, 12, 4R, 13, 4G, 14, 4B, 15, 4A, 16)] _GlitterMaskGlobalMask ("Global Mask--{reference_property:_GlitterMaskGlobalMaskBlendType}", Int) = 0
		[HideInInspector][ThryWideEnum(Add, 7, Subtract, 1, Multiply, 2, Divide, 3, Min, 4, Max, 5, Average, 6, Replace, 0)]_GlitterMaskGlobalMaskBlendType ("Blending", Range(0, 1)) = 2
		[HideInInspector] s_end_GlitterMask ("Masking", Float) = 0
		
		[HideInInspector] s_start_GlitterHueShiftSection ("Hue Shift--{reference_property:_GlitterHueShiftEnabled, persistent_expand:true,default_expand:false)}", Float) = 0
		[HideInInspector][ToggleUI]_GlitterHueShiftEnabled ("Enable Hue Shift", Float) = 0
		_GlitterHueShiftSpeed ("Shift Speed", Float) = 0
		_GlitterHueShift ("Hue Shift", Range(0, 1)) = 0
		[HideInInspector] s_end_GlitterHueShiftSection ("Hue Shift--{reference_property:_ShadowBorderMapToggle, persistent_expand:true,default_expand:false)}", Float) = 0
		[HideInInspector] m_end_glitter ("Glitter / Sparkle", Float) = 0
		//endex
		
		//ifex _PoiInternalParallax==0
		[HideInInspector] m_start_internalparallax (" Internal Parallax--{reference_property:_PoiInternalParallax}", Float) = 0
		[HideInInspector][ThryToggle(POI_INTERNALPARALLAX)]_PoiInternalParallax ("Enable", Float) = 0
		
		[Enum(Basic, 0, HeightMap, 1)] _ParallaxInternalHeightmapMode ("Parallax Mode", Int) = 0
		
		[ThryRGBAPacker(RGB Color, A Height, sRGB, false)][sRGBWarning(true)]_ParallaxInternalMap ("Internal Map--{reference_properties:[_ParallaxInternalMapPan, _ParallaxInternalPanDepthSpeed, _ParallaxInternalHeightFromAlpha]}", 2D) = "black" { }
		[HideInInspector][Vector2]_ParallaxInternalMapPan ("Panning", Vector) = (0, 0, 1, 1)
		[HideInInspector][Vector2]_ParallaxInternalPanDepthSpeed ("Per Level Pan Multiplier", Vector) = (0, 0, 1, 1)
		[HideInInspector][ToggleUI]_ParallaxInternalHeightFromAlpha ("Height From Alpha", Float) = 0
		
		[sRGBWarning][ThryTexture]_ParallaxInternalMapMask ("Mask--{reference_properties:[_ParallaxInternalMapMaskPan, _ParallaxInternalMapMaskUV, _ParallaxInternalMapMaskChannel]}", 2D) = "white" { }
		[HideInInspector][Vector2]_ParallaxInternalMapMaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][Enum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, distorteduv0, 4)] _ParallaxInternalMapMaskUV ("UV", Int) = 0
		[HideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_ParallaxInternalMapMaskChannel ("Channel", Float) = 0
		
		[HideInInspector] s_start_ParallaxInternalLayerControls ("Layer Controls--{persistent_expand:true,default_expand:true}", Float) = 1
		[IntRange]_ParallaxInternalIterations ("Parallax Internal Iterations", Range(1, 50)) = 4
		_ParallaxInternalMinDepth ("Min Depth", Float) = 0
		_ParallaxInternalMaxDepth ("Max Depth", Float) = 0.1
		[HideInInspector] s_end_ParallaxInternalLayerControls ("", Float) = 0
		
		[HideInInspector] s_start_ParallaxInternalLayerColoring ("Layer Colors--{persistent_expand:true,default_expand:true}", Float) = 1
		_ParallaxInternalMinFade ("Min Depth Brightness", Range(0, 5)) = 1.0
		_ParallaxInternalMaxFade ("Max Depth Brightness", Range(0, 5)) = 0.1
		_ParallaxInternalMinColor ("Min Depth Color--{reference_property:_ParallaxInternalMinColorThemeIndex}", Color) = (1, 1, 1, 1)
		[HideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _ParallaxInternalMinColorThemeIndex ("", Int) = 0
		_ParallaxInternalMaxColor ("Max Depth Color--{reference_property:_ParallaxInternalMaxColorThemeIndex}", Color) = (1, 1, 1, 1)
		[HideInInspector][ThryWideEnum(Off, 0, Theme Color 0, 1, Theme Color 1, 2, Theme Color 2, 3, Theme Color 3, 4, ColorChord 0, 5, ColorChord 1, 6, ColorChord 2, 7, ColorChord 3, 8, AL Theme 0, 9, AL Theme 1, 10, AL Theme 2, 11, AL Theme 3, 12)] _ParallaxInternalMaxColorThemeIndex ("", Int) = 0
		[Enum(Add, 0, Max, 1)] _ParallaxInternalBlendMode ("Internal Blend Mode", Int) = 0
		[ThryWideEnum(Replace, 0, Multiply, 2, Screen, 6, Subtract, 7, Add, 8, Overlay, 9, Mixed, 20)] _ParallaxInternalSurfaceBlendMode ("Surface Blend Mode", Int) = 8
		[HideInInspector] s_end_ParallaxInternalLayerColoring ("", Float) = 0
		// [Vector2]_ParallaxInternalPanSpeed ("Pan Speed", Vector) = (0, 0, 0, 0)
		
		[HideInInspector] s_start_ParallaxInternalHueShift ("Hue Shift--{reference_property:_ParallaxInternalHueShiftEnabled,persistent_expand:true,default_expand:false}", Float) = 0
		[HideInInspector][ToggleUI]_ParallaxInternalHueShiftEnabled ("Hue Shift", Float) = 0
		_ParallaxInternalHueShift ("Hue Shift", Range(0, 1)) = 0
		_ParallaxInternalHueShiftSpeed ("Hue Shift Speed", Float) = 0
		_ParallaxInternalHueShiftPerLevel ("Hue Shift Per Level", Float) = 0
		[HideInInspector] s_end_ParallaxInternalHueShift ("", Float) = 0
		// _ParallaxInternalHueShiftPerLevelSpeed ("Hue Shift Per Level Speed", Float) = 0
		
		[HideInInspector] m_end_internalparallax ("Internal Parallax", Float) = 0
		//endex
		
		[HideInInspector] m_RF("Reactive Floor--{reference_property:_EnableEmission}", Float) = 0
		_RF_Min_Distance("RF Min Distance", Float) = 0
		_RF_Max_Distance("RF Max Distance", Float) = 5
		[Gradient]_RF_Map("Map--{texture:{width:512,height:4,filterMode:Bilinear,wrapMode:Clamp},force_texture_options:true}", 2D) = "white" { }
		_RF_Ramp_Pan("Ramp Pan Speed", Float) = 0
		
		// _RF_Color0("RF Color 1", Color) = (1,0,0,1)
		// _RF_Color1("RF Color 2", Color) = (0,1,0,1)
		// _RF_Color2("RF Color 3", Color) = (0,0,1,1)
		// _RF_Color3("RF Color 4", Color) = (0,0,1,1)
		// _RF_Color4("RF Color 5", Color) = (0,0,1,1)
		// _RF_Color5("RF Color 6", Color) = (0,0,1,1)
		// _RF_Color6("RF Color 7", Color) = (0,0,1,1)
		// _RF_Color7("RF Color 8", Color) = (0,0,1,1)
		
		[HideInInspector][NonModifiableTextureData] _RF_ArrayLength ("RF Array Length", Integer) = 0
		
		[HideInInspector] m_modifierCategory ("Global Modifiers & Data", Float) = 0
		[HideInInspector] m_start_PoiGlobalCategory ("Global Data and Masks", Float) = 0
		
		[HideInInspector] m_end_PoiGlobalCategory ("Global Data and Masks ", Float) = 0
		[HideInInspector] m_start_PoiUVCategory ("UVs", Float) = 0
		[HideInInspector] m_start_Stochastic ("Stochastic Sampling", Float) = 0
		[KeywordEnum(Deliot Heitz, Hextile, None)] _StochasticMode ("Sampling Mode", Float) = 0
		[HideInInspector] s_start_deliot ("Deliot Heitz--{persistent_expand:true,default_expand:false,condition_show:{type:PROPERTY_BOOL,data:_StochasticMode==0}}", Float) = 0
		_StochasticDeliotHeitzDensity ("Detiling Density", Range(0.1, 10)) = 1
		[HideInInspector] s_end_deliot ("Deliot Heitz", Float) = 0
		[HideInInspector] s_start_hextile ("Hextile--{persistent_expand:true,default_expand:false,condition_show:{type:PROPERTY_BOOL,data:_StochasticMode==1}}", Float) = 0
		_StochasticHexGridDensity ("Hex Grid Density", Range(0.1, 10)) = 1
		_StochasticHexRotationStrength ("Rotation Strength", Range(0, 2)) = 0
		_StochasticHexFallOffContrast("Falloff Contrast", Range(0.01, 0.99)) = 0.6
		_StochasticHexFallOffPower("Falloff Power", Range(0, 20)) = 7
		[HideInInspector] s_end_hextile ("Hextile", Float) = 0
		[HideInInspector] m_end_Stochastic ("Stochastic Sampling", Float) = 0
		
		//ifex _PoiParallax==0
		[HideInInspector] m_start_parallax (" Parallax Heightmapping--{reference_property:_PoiParallax}", Float) = 0
		[HideInInspector][ThryToggle(POI_PARALLAX)]_PoiParallax ("Enable", Float) = 0
		[ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)]_ParallaxUV ("Applies To: ", Int) = 0
		
		[sRGBWarning][ThryTexture]_HeightMap ("Heightmap--{reference_properties:[_HeightMapPan, _HeightMapUV]}", 2D) = "white" { }
		[HideInInspector][Vector2]_HeightMapPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)]_HeightMapUV ("UV", Int) = 0
		
		[sRGBWarning][ThryTexture]_Heightmask ("Mask--{reference_properties:[_HeightmaskPan, _HeightmaskUV, _HeightmaskChannel, _HeightmaskInvert]}", 2D) = "white" { }
		[HideInInspector][Vector2]_HeightmaskPan ("Panning", Vector) = (0, 0, 0, 0)
		[HideInInspector][Enum(R, 0, G, 1, B, 2, A, 3)]_HeightmaskChannel ("Channel", Float) = 0
		[HideInInspector][ToggleUI]_HeightmaskInvert ("Invert", Float) = 0
		[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos, 5, Local Pos, 8, Polar UV, 6, Distorted UV, 7)]_HeightmaskUV ("UV", Int) = 0
		_HeightStrength ("Strength", Range(0, 1)) = 0.4247461
		//_HeightOffset ("Offset", Range(-1, 1)) = 0
		_CurvatureU ("Curvature U", Range(0, 100)) = 0
		_CurvatureV ("Curvature V", Range(0, 30)) = 0
		[IntRange]_HeightStepsMin ("Steps Min", Range(0, 128)) = 10
		[IntRange]_HeightStepsMax ("Steps Max", Range(0, 128)) = 128
		_CurvFix ("Curvature Bias", Range(0, 1)) = 1
		// [ThryToggle]_ParallaxUV0 ("UV0", Float) = 0
		// [ThryToggle]_ParallaxUV1 ("UV1", Float) = 0
		// [ThryToggle]_ParallaxUV2 ("UV2", Float) = 0
		// [ThryToggle]_ParallaxUV3 ("UV3", Float) = 0
		// [ThryToggle]_ParallaxPano ("Panosphere", Float) = 0
		// [ThryToggle]_ParallaxWorldPos ("World Pos", Float) = 0
		// [ThryToggle]_ParallaxPolar ("Polar", Float) = 0
		// [ThryToggle]_ParallaxDist ("Distorted UV", Float) = 0
		
		[HideInInspector] m_end_parallax ("Parallax Heightmapping", Float) = 0
		//endex
		
		[HideInInspector] m_end_PoiUVCategory ("UVs ", Float) = 0
		[HideInInspector] m_start_PoiPostProcessingCategory ("Post Processing", Float) = 0
		
		[HideInInspector] m_end_PoiPostProcessingCategory ("Post Processing ", Float) = 0
		
		[HideInInspector] m_thirdpartyCategory ("Third Party", Float) = 0
		
		[HideInInspector] m_renderingCategory ("Rendering--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/rendering/main},hover:Documentation}}", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[Enum(Off, 0, On, 1)] _ZWrite ("ZWrite", Int) = 1
		[Enum(Thry.ColorMask)] _ColorMask ("Color Mask", Int) = 15
		_OffsetFactor ("Offset Factor", Float) = 0.0
		_OffsetUnits ("Offset Units", Float) = 0.0
		[ToggleUI]_RenderingReduceClipDistance ("Reduce Clip Distance", Float) = 0
		[ToggleUI]_IgnoreFog ("Ignore Fog", Float) = 0
		[ToggleUI]_FlipBackfaceNormals ("Flip Backface Normals", Int) = 1
		[HideInInspector] Instancing ("Instancing", Float) = 0 //add this property for instancing variants settings to be shown
		[ToggleUI] _RenderingEarlyZEnabled ("Early Z", Float) = 0
		
		// Blending Options
		[HideInInspector] m_start_blending ("Blending--{button_help:{text:Tutorial,action:{type:URL,data:https://www.poiyomi.com/rendering/blending},hover:Documentation}}", Float) = 0
		[Enum(Thry.BlendOp)]_BlendOp ("RGB Blend Op", Int) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("RGB Source Blend", Int) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("RGB Destination Blend", Int) = 0
		[Space][ThryHeaderLabel(Additive Blending, 13)]
		[Enum(Thry.BlendOp)]_AddBlendOp ("RGB Blend Op", Int) = 4
		[Enum(UnityEngine.Rendering.BlendMode)] _AddSrcBlend ("RGB Source Blend", Int) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _AddDstBlend ("RGB Destination Blend", Int) = 1
		
		[HideInInspector] m_start_alphaBlending ("Advanced Alpha Blending", Float) = 0
		[Enum(Thry.BlendOp)]_BlendOpAlpha ("Alpha Blend Op", Int) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendAlpha ("Alpha Source Blend", Int) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendAlpha ("Alpha Destination Blend", Int) = 10
		[Space][ThryHeaderLabel(Additive Blending, 13)]
		[Enum(Thry.BlendOp)]_AddBlendOpAlpha ("Alpha Blend Op", Int) = 4
		[Enum(UnityEngine.Rendering.BlendMode)] _AddSrcBlendAlpha ("Alpha Source Blend", Int) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _AddDstBlendAlpha ("Alpha Destination Blend", Int) = 1
		[HideInInspector] m_end_alphaBlending ("Advanced Alpha Blending", Float) = 0
		
		[HideInInspector] m_end_blending ("Blending", Float) = 0
		
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "VRCFallback" = "Standard" }
		
		//ifex _RenderingEarlyZEnabled==0
		
		Pass
		{
			Name "EarlyZ"
			Tags { "LightMode" = "ForwardBase" }
			
			ZWrite On
			Cull [_Cull]
			ColorMask 0
			
			CGPROGRAM
			/*
			// Disable warnings we aren't interested in
			#if defined(UNITY_COMPILER_HLSL)
			#pragma warning(disable : 3205) // conversion of larger type to smaller
			#pragma warning(disable : 3568) // unknown pragma ignored
			#pragma warning(disable : 3571) // "pow(f,e) will not work for negative f"; however in majority of our calls to pow we know f is not negative
			#pragma warning(disable : 3206) // implicit truncation of vector type
			#endif
			*/
			#pragma target 5.0
			//ifex 0==0
			#pragma skip_optimizations d3d11
			//endex
			
			#pragma shader_feature_local _STOCHASTICMODE_DELIOT_HEITZ _STOCHASTICMODE_HEXTILE _STOCHASTICMODE_NONE
			
			//ifex _MainColorAdjustToggle==0
			#pragma shader_feature COLOR_GRADING_HDR
			//endex
			
			//#pragma shader_feature KEYWORD
			
			#pragma skip_variants LIGHTMAP_ON DYNAMICLIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SHADOWMASK DIRLIGHTMAP_COMBINED _MIXED_LIGHTING_SUBTRACTIVE
			#pragma skip_variants DECALS_OFF DECALS_3RT DECALS_4RT DECAL_SURFACE_GRADIENT _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma skip_variants _ADDITIONAL_LIGHT_SHADOWS
			#pragma skip_variants PROBE_VOLUMES_OFF PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
			#pragma skip_variants _SCREEN_SPACE_OCCLUSION
			
			//ifex _PoiParallax==0
			#pragma shader_feature_local POI_PARALLAX
			//endex
			
			//ifex _EnableEmission==0
			#pragma shader_feature _EMISSION
			//endex
			//ifex _EnableEmission1==0
			#pragma shader_feature_local POI_EMISSION_1
			//endex
			//ifex _EnableEmission2==0
			#pragma shader_feature_local POI_EMISSION_2
			//endex
			//ifex _EnableEmission3==0
			#pragma shader_feature_local POI_EMISSION_3
			//endex
			
			//ifex _GlitterEnable==0
			#pragma shader_feature _SUNDISK_SIMPLE
			//endex
			
			//ifex _PoiInternalParallax==0
			#pragma shader_feature_local POI_INTERNALPARALLAX
			//endex
			
			#pragma multi_compile_instancing
			#pragma multi_compile_fog
			#define POI_PASS_EARLYZ
			
			// UNITY Includes
			#include "UnityCG.cginc"
			#include "UnityStandardUtils.cginc"
			#include "AutoLight.cginc"
			#include "UnityLightingCommon.cginc"
			#include "UnityPBSLighting.cginc"
			#ifdef POI_PASS_META
			#include "UnityMetaPass.cginc"
			#endif
			#pragma vertex vert
			
			#pragma fragment frag
			
			#define DielectricSpec float4(0.04, 0.04, 0.04, 1.0 - 0.04)
			#define PI float(3.14159265359)
			
			#define POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, samplertex, coord, dx, dy) tex.SampleGrad(sampler##samplertex, coord, dx, dy)
			#define POI2D_SAMPLE_TEX2D_SAMPLERGRADD(tex, samp, uv, pan, dx, dy) tex.SampleGrad(samp, POI_PAN_UV(uv, pan), dx, dy)
			
			#define POI_PAN_UV(uv, pan) (uv + _Time.x * pan)
			#define POI2D_SAMPLER_PAN(tex, texSampler, uv, pan) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, POI_PAN_UV(uv, pan)))
			#define POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, POI_PAN_UV(uv, pan), dx, dy))
			#define POI2D_SAMPLER(tex, texSampler, uv) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, uv))
			#define POI_SAMPLE_1D_X(tex, samp, uv) tex.Sample(samp, float2(uv, 0.5))
			#define POI2D_SAMPLER_GRAD(tex, texSampler, uv, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, uv, dx, dy))
			#define POI2D_SAMPLER_GRADD(tex, texSampler, uv, dx, dy) tex.SampleGrad(texSampler, uv, dx, dy)
			#define POI2D_PAN(tex, uv, pan) (tex2D(tex, POI_PAN_UV(uv, pan)))
			#define POI2D(tex, uv) (tex2D(tex, uv))
			#define POI_SAMPLE_TEX2D(tex, uv) (UNITY_SAMPLE_TEX2D(tex, uv))
			#define POI_SAMPLE_TEX2D_PAN(tex, uv, pan) (UNITY_SAMPLE_TEX2D(tex, POI_PAN_UV(uv, pan)))
			#define POI_SAMPLE_CUBE_LOD(tex, samp, uv, lod) texCUBElod(tex, float4(uv, 0, lod))
			
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define TEXTURE2D_SCREEN(tex)                   Texture2DArray tex
			#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, float3(uv, unity_StereoEyeIndex))
			#else
			#define TEXTURE2D_SCREEN(tex)                   Texture2D tex
			#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, uv)
			#endif
			
			// When using, properties won't properly lock at optimize time; needs macro evaluation implemented
			// #define POI2D_MAINTEX_SAMPLER_PAN_INLINED(tex, poiMesh) (POI2D_SAMPLER_PAN(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan))
			
			#define POI_SAFE_RGB0 float4(mainTexture.rgb * .0001, 0)
			#define POI_SAFE_RGB1 float4(mainTexture.rgb * .0001, 1)
			#define POI_SAFE_RGBA mainTexture
			
			#if defined(UNITY_COMPILER_HLSL)
			#define PoiInitStruct(type, name) name = (type)0;
			#else
			#define PoiInitStruct(type, name)
			#endif
			
			#define POI_ERROR(poiMesh, gridSize) lerp(float3(1, 0, 1), float3(0, 0, 0), fmod(floor((poiMesh.worldPos.x) * gridSize) + floor((poiMesh.worldPos.y) * gridSize) + floor((poiMesh.worldPos.z) * gridSize), 2) == 0)
			#define POI_NAN (asfloat(-1))
			
			#define POI_MODE_OPAQUE 0
			#define POI_MODE_CUTOUT 1
			#define POI_MODE_FADE 2
			#define POI_MODE_TRANSPARENT 3
			#define POI_MODE_ADDITIVE 4
			#define POI_MODE_SOFTADDITIVE 5
			#define POI_MODE_MULTIPLICATIVE 6
			#define POI_MODE_2XMULTIPLICATIVE 7
			#define POI_MODE_TRANSCLIPPING 9
			
			/*
			Texture2D ;
			float4 _ST;
			float2 Pan;
			float UV;
			float Stochastic;
			
			[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos XZ, 5, Polar UV, 6, Distorted UV, 7 )]
			*/
			
			float _RenderingEarlyZEnabled;
			
			float _IgnoreFog;
			float _RenderingReduceClipDistance;
			int _FlipBackfaceNormals;
			float _AddBlendOp;
			float _Cull;
			
			float4 _Color;
			float _ColorThemeIndex;
			UNITY_DECLARE_TEX2D(_MainTex);
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
			float _MainPixelMode;
			float4 _MainTex_ST;
			float2 _MainTexPan;
			float _MainTexUV;
			float4 _MainTex_TexelSize;
			float _MainTexStochastic;
			#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _BumpMap;
			#endif
			float4 _BumpMap_ST;
			float2 _BumpMapPan;
			float _BumpMapUV;
			float _BumpScale;
			float _BumpMapStochastic;
			#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _AlphaMask;
			float4 _AlphaMask_ST;
			float2 _AlphaMaskPan;
			float _AlphaMaskUV;
			float _AlphaMaskInvert;
			float _AlphaMaskMode;
			float _AlphaMaskScale;
			float _AlphaMaskValue;
			#endif
			float _Cutoff;
			//ifex _MainColorAdjustToggle==0
			float _MainColorAdjustToggle;
			#if defined(PROP_MAINCOLORADJUSTTEXTURE) || !defined(OPTIMIZER_ENABLED)
			Texture2D _MainColorAdjustTexture;
			#endif
			float4 _MainColorAdjustTexture_ST;
			float2 _MainColorAdjustTexturePan;
			float _MainColorAdjustTextureUV;
			float _MainHueShiftToggle;
			float _MainHueShiftReplace;
			float _MainHueShift;
			float _MainHueShiftSpeed;
			float _Saturation;
			float _MainBrightness;
			
			float _MainHueALCTEnabled;
			float _MainALHueShiftBand;
			float _MainALHueShiftCTIndex;
			float _MainHueALMotionSpeed;
			
			float _MainHueGlobalMask;
			float _MainHueGlobalMaskBlendType;
			float _MainSaturationGlobalMask;
			float _MainSaturationGlobalMaskBlendType;
			float _MainBrightnessGlobalMask;
			float _MainBrightnessGlobalMaskBlendType;
			
			#if defined(PROP_MAINGRADATIONTEX) || !defined(OPTIMIZER_ENABLED)
			Texture2D _MainGradationTex;
			#endif
			float _ColorGradingToggle;
			float _MainGradationStrength;
			//endex
			
			SamplerState sampler_linear_clamp;
			SamplerState sampler_linear_repeat;
			SamplerState sampler_trilinear_repeat;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				uint vertexId : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct VertexOut
			{
				float4 pos : SV_POSITION;
				float4 uv[2] : TEXCOORD0;
				float3 normal : TEXCOORD2;
				float4 tangent : TEXCOORD3;
				float4 worldPos : TEXCOORD4;
				float4 localPos : TEXCOORD5;
				float4 vertexColor : TEXCOORD6;
				float4 lightmapUV : TEXCOORD7;
				float2 fogCoord: TEXCOORD10;
				UNITY_SHADOW_COORDS(11)
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			struct PoiMesh
			{
				
				// 0 Vertex normal
				// 1 Fragment normal
				float3 normals[2];
				float3 objNormal;
				float3 tangentSpaceNormal;
				float3 binormal[2];
				float3 tangent[2];
				float3 worldPos;
				float3 localPos;
				float3 objectPosition;
				float isFrontFace;
				float4 vertexColor;
				float4 lightmapUV;
				// 0-3 UV0-UV3
				// 4 Panosphere UV
				// 5 world pos xz
				// 6 Polar UV
				// 7 Distorted UV
				float2 uv[9];
				float2 parallaxUV;
				float2 dx;
				float2 dy;
			};
			
			struct PoiCam
			{
				float3 viewDir;
				float3 forwardDir;
				float3 worldPos;
				float distanceToVert;
				float4 clipPos;
				float4 screenSpacePosition;
				float3 reflectionDir;
				float3 vertexReflectionDir;
				float3 tangentViewDir;
				float4 posScreenSpace;
				float2 posScreenPixels;
				float2 screenUV;
				float vDotN;
				float4 worldDirection;
				
			};
			
			struct PoiMods
			{
				float4 Mask;
				float audioLink[5];
				float audioLinkAvailable;
				float audioLinkVersion;
				float4 audioLinkTexture;
				float audioLinkViaLuma;
				float2 detailMask;
				float2 backFaceDetailIntensity;
				float globalEmission;
				float4 globalColorTheme[12];
				float globalMask[16];
				float ALTime[8];
			};
			
			struct PoiLight
			{
				
				float3 direction;
				float attenuation;
				float attenuationStrength;
				float3 directColor;
				float3 indirectColor;
				float occlusion;
				float shadowMask;
				float detailShadow;
				float3 halfDir;
				float lightMap;
				float lightMapNoAttenuation;
				float3 rampedLightMap;
				float vertexNDotL;
				float nDotL;
				float nDotV;
				float vertexNDotV;
				float nDotH;
				float vertexNDotH;
				float lDotv;
				float lDotH;
				float nDotLSaturated;
				float nDotLNormalized;
				#ifdef POI_PASS_ADD
				float additiveShadow;
				#endif
				float3 finalLighting;
				float3 finalLightAdd;
				float3 LTCGISpecular;
				float3 LTCGIDiffuse;
				float directLuminance;
				float indirectLuminance;
				float finalLuminance;
				
				#if defined(VERTEXLIGHT_ON)
				// Non Important Lights
				float4 vDotNL;
				float4 vertexVDotNL;
				float3 vColor[4];
				float4 vCorrectedDotNL;
				float4 vAttenuation;
				float4 vAttenuationDotNL;
				float3 vPosition[4];
				float3 vDirection[4];
				float3 vFinalLighting;
				float3 vHalfDir[4];
				half4 vDotNH;
				half4 vertexVDotNH;
				half4 vDotLH;
				#endif
				
			};
			
			struct PoiVertexLights
			{
				
				float3 direction;
				float3 color;
				float attenuation;
			};
			
			struct PoiFragData
			{
				float smoothness;
				float smoothness2;
				float metallic;
				float specularMask;
				float reflectionMask;
				
				float3 baseColor;
				float3 finalColor;
				float alpha;
				float3 emission;
				float toggleVertexLights;
			};
			
			float4 poiTransformClipSpacetoScreenSpaceFrag(float4 clipPos)
			{
				float4 positionSS = float4(clipPos.xyz * clipPos.w, clipPos.w);
				positionSS.xy = positionSS.xy / _ScreenParams.xy;
				return positionSS;
			}
			
			// glsl_mod behaves better on negative numbers, and
			// in some situations actually outperforms HLSL's fmod()
			#ifndef glsl_mod
			#define glsl_mod(x, y) (((x) - (y) * floor((x) / (y))))
			#endif
			
			uniform float random_uniform_float_only_used_to_stop_compiler_warnings = 0.0f;
			
			float2 poiUV(float2 uv, float4 tex_st)
			{
				return uv * tex_st.xy + tex_st.zw;
			}
			
			float2 vertexUV(in VertexOut o, int index)
			{
				switch(index)
				{
					case 0:
					return o.uv[0].xy;
					case 1:
					return o.uv[0].zw;
					case 2:
					return o.uv[1].xy;
					case 3:
					return o.uv[1].zw;
					default:
					return o.uv[0].xy;
				}
			}
			
			float2 vertexUV(in appdata v, int index)
			{
				switch(index)
				{
					case 0:
					return v.uv0.xy;
					case 1:
					return v.uv1.xy;
					case 2:
					return v.uv2.xy;
					case 3:
					return v.uv3.xy;
					default:
					return v.uv0.xy;
				}
			}
			
			//Lighting Helpers
			float calculateluminance(float3 color)
			{
				return color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
			}
			
			// Set by VRChat (as of open beta 1245)
			// _VRChatCameraMode: 0 => Normal, 1 => VR HandCam, 2 => Desktop Handcam, 3 => Screenshot/Photo
			// _VRChatMirrorMode: 0 => Normal, 1 => Mirror (VR), 2 => Mirror (Deskie)
			float _VRChatCameraMode;
			float _VRChatMirrorMode;
			
			float VRCCameraMode()
			{
				return _VRChatCameraMode;
			}
			
			float VRCMirrorMode()
			{
				return _VRChatMirrorMode;
			}
			
			bool IsInMirror()
			{
				return unity_CameraProjection[2][0] != 0.f || unity_CameraProjection[2][1] != 0.f;
			}
			
			bool IsOrthographicCamera()
			{
				return unity_OrthoParams.w == 1 || UNITY_MATRIX_P[3][3] == 1;
			}
			
			float shEvaluateDiffuseL1Geomerics_local(float L0, float3 L1, float3 n)
			{
				// average energy
				float R0 = max(0, L0);
				
				// avg direction of incoming light
				float3 R1 = 0.5f * L1;
				
				// directional brightness
				float lenR1 = length(R1);
				
				// linear angle between normal and direction 0-1
				//float q = 0.5f * (1.0f + dot(R1 / lenR1, n));
				//float q = dot(R1 / lenR1, n) * 0.5 + 0.5;
				float q = dot(normalize(R1), n) * 0.5 + 0.5;
				q = saturate(q); // Thanks to ScruffyRuffles for the bug identity.
				
				// power for q
				// lerps from 1 (linear) to 3 (cubic) based on directionality
				float p = 1.0f + 2.0f * lenR1 / R0;
				
				// dynamic range constant
				// should vary between 4 (highly directional) and 0 (ambient)
				float a = (1.0f - lenR1 / R0) / (1.0f + lenR1 / R0);
				
				return R0 * (a + (1.0f - a) * (p + 1.0f) * pow(q, p));
			}
			
			half3 BetterSH9(half4 normal)
			{
				float3 indirect;
				float3 L0 = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w) + float3(unity_SHBr.z, unity_SHBg.z, unity_SHBb.z) / 3.0;
				indirect.r = shEvaluateDiffuseL1Geomerics_local(L0.r, unity_SHAr.xyz, normal.xyz);
				indirect.g = shEvaluateDiffuseL1Geomerics_local(L0.g, unity_SHAg.xyz, normal.xyz);
				indirect.b = shEvaluateDiffuseL1Geomerics_local(L0.b, unity_SHAb.xyz, normal.xyz);
				indirect = max(0, indirect);
				indirect += SHEvalLinearL2(normal);
				return indirect;
			}
			
			// Silent's code ends here
			
			float3 getCameraForward()
			{
				#if UNITY_SINGLE_PASS_STEREO
				float3 p1 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 1, 1));
				float3 p2 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 0, 1));
				#else
				float3 p1 = mul(unity_CameraToWorld, float4(0, 0, 1, 1)).xyz;
				float3 p2 = mul(unity_CameraToWorld, float4(0, 0, 0, 1)).xyz;
				#endif
				return normalize(p2 - p1);
			}
			
			half3 GetSHLength()
			{
				half3 x, x1;
				x.r = length(unity_SHAr);
				x.g = length(unity_SHAg);
				x.b = length(unity_SHAb);
				x1.r = length(unity_SHBr);
				x1.g = length(unity_SHBg);
				x1.b = length(unity_SHBb);
				return x + x1;
			}
			
			float3 BoxProjection(float3 direction, float3 position, float4 cubemapPosition, float3 boxMin, float3 boxMax)
			{
				#if UNITY_SPECCUBE_BOX_PROJECTION
				//UNITY_BRANCH
				if (cubemapPosition.w > 0)
				{
					float3 factors = ((direction > 0 ? boxMax : boxMin) - position) / direction;
					float scalar = min(min(factors.x, factors.y), factors.z);
					direction = direction * scalar + (position - cubemapPosition.xyz);
				}
				#endif
				return direction;
			}
			
			float poiMax(float2 i)
			{
				return max(i.x, i.y);
			}
			
			float poiMax(float3 i)
			{
				return max(max(i.x, i.y), i.z);
			}
			
			float poiMax(float4 i)
			{
				return max(max(max(i.x, i.y), i.z), i.w);
			}
			
			float3 calculateNormal(in float3 baseNormal, in PoiMesh poiMesh, in Texture2D normalTexture, in float4 normal_ST, in float2 normalPan, in float normalUV, in float normalIntensity)
			{
				float3 normal = UnpackScaleNormal(POI2D_SAMPLER_PAN(normalTexture, _MainTex, poiUV(poiMesh.uv[normalUV], normal_ST), normalPan), normalIntensity);
				return normalize(
				normal.x * poiMesh.tangent[0] +
				normal.y * poiMesh.binormal[0] +
				normal.z * baseNormal
				);
			}
			
			float remap(float x, float minOld, float maxOld, float minNew = 0, float maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float2 remap(float2 x, float2 minOld, float2 maxOld, float2 minNew = 0, float2 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float3 remap(float3 x, float3 minOld, float3 maxOld, float3 minNew = 0, float3 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float4 remap(float4 x, float4 minOld, float4 maxOld, float4 minNew = 0, float4 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float remapClamped(float minOld, float maxOld, float x, float minNew = 0, float maxNew = 1)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float2 remapClamped(float2 minOld, float2 maxOld, float2 x, float2 minNew, float2 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float3 remapClamped(float3 minOld, float3 maxOld, float3 x, float3 minNew, float3 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float4 remapClamped(float4 minOld, float4 maxOld, float4 x, float4 minNew, float4 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			float2 calcParallax(in float height, in PoiCam poiCam)
			{
				return ((height * - 1) + 1) * (poiCam.tangentViewDir.xy / poiCam.tangentViewDir.z);
			}
			
			/*
			0: Zero	                float4(0.0, 0.0, 0.0, 0.0),
			1: One	                float4(1.0, 1.0, 1.0, 1.0),
			2: DstColor	            destinationColor,
			3: SrcColor	            sourceColor,
			4: OneMinusDstColor	    float4(1.0, 1.0, 1.0, 1.0) - destinationColor,
			5: SrcAlpha	            sourceColor.aaaa,
			6: OneMinusSrcColor	    float4(1.0, 1.0, 1.0, 1.0) - sourceColor,
			7: DstAlpha	            destinationColor.aaaa,
			8: OneMinusDstAlpha	    float4(1.0, 1.0, 1.0, 1.0) - destinationColor.,
			9: SrcAlphaSaturate     saturate(sourceColor.aaaa),
			10: OneMinusSrcAlpha	float4(1.0, 1.0, 1.0, 1.0) - sourceColor.aaaa,
			*/
			
			float4 poiBlend(const float sourceFactor, const  float4 sourceColor, const  float destinationFactor, const  float4 destinationColor, const float4 blendFactor)
			{
				float4 sA = 1 - blendFactor;
				const float4 blendData[11] = {
					float4(0.0, 0.0, 0.0, 0.0),
					float4(1.0, 1.0, 1.0, 1.0),
					destinationColor,
					sourceColor,
					float4(1.0, 1.0, 1.0, 1.0) - destinationColor,
					sA,
					float4(1.0, 1.0, 1.0, 1.0) - sourceColor,
					sA,
					float4(1.0, 1.0, 1.0, 1.0) - sA,
					saturate(sourceColor.aaaa),
					1 - sA,
				};
				
				return lerp(blendData[sourceFactor] * sourceColor + blendData[destinationFactor] * destinationColor, sourceColor, sA);
			}
			
			// Average
			float blendAverage(float base, float blend)
			{
				return (base + blend) / 2.0;
			}
			float3 blendAverage(float3 base, float3 blend)
			{
				return (base + blend) / 2.0;
			}
			
			// Color burn
			float blendColorBurn(float base, float blend)
			{
				return (blend == 0.0) ? blend : max((1.0 - ((1.0 - base) * rcp(random_uniform_float_only_used_to_stop_compiler_warnings + blend))), 0.0);
			}
			
			float3 blendColorBurn(float3 base, float3 blend)
			{
				return float3(blendColorBurn(base.r, blend.r), blendColorBurn(base.g, blend.g), blendColorBurn(base.b, blend.b));
			}
			
			// Color Dodge
			float blendColorDodge(float base, float blend)
			{
				return (blend == 1.0) ? blend : min(base / (1.0 - blend), 1.0);
			}
			
			float3 blendColorDodge(float3 base, float3 blend)
			{
				return float3(blendColorDodge(base.r, blend.r), blendColorDodge(base.g, blend.g), blendColorDodge(base.b, blend.b));
			}
			
			// Darken
			float blendDarken(float base, float blend)
			{
				return min(blend, base);
			}
			
			float3 blendDarken(float3 base, float3 blend)
			{
				return float3(blendDarken(base.r, blend.r), blendDarken(base.g, blend.g), blendDarken(base.b, blend.b));
			}
			
			// Exclusion
			float blendExclusion(float base, float blend)
			{
				return base + blend - 2.0 * base * blend;
			}
			float3 blendExclusion(float3 base, float3 blend)
			{
				return base + blend - 2.0 * base * blend;
			}
			
			// Reflect
			float blendReflect(float base, float blend)
			{
				return (blend == 1.0) ? blend : min(base * base / (1.0 - blend), 1.0);
			}
			
			float3 blendReflect(float3 base, float3 blend)
			{
				return float3(blendReflect(base.r, blend.r), blendReflect(base.g, blend.g), blendReflect(base.b, blend.b));
			}
			
			// Glow
			float blendGlow(float base, float blend)
			{
				return blendReflect(blend, base);
			}
			float3 blendGlow(float3 base, float3 blend)
			{
				return blendReflect(blend, base);
			}
			
			// Overlay
			float blendOverlay(float base, float blend)
			{
				return base < 0.5 ? (2.0 * base * blend) : (1.0 - 2.0 * (1.0 - base) * (1.0 - blend));
			}
			
			float3 blendOverlay(float3 base, float3 blend)
			{
				return float3(blendOverlay(base.r, blend.r), blendOverlay(base.g, blend.g), blendOverlay(base.b, blend.b));
			}
			
			// Hard Light
			float blendHardLight(float base, float blend)
			{
				return blendOverlay(blend, base);
			}
			float3 blendHardLight(float3 base, float3 blend)
			{
				return blendOverlay(blend, base);
			}
			
			// Vivid light
			float blendVividLight(float base, float blend)
			{
				return (blend < 0.5) ? blendColorBurn(base, (2.0 * blend)) : blendColorDodge(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendVividLight(float3 base, float3 blend)
			{
				return float3(blendVividLight(base.r, blend.r), blendVividLight(base.g, blend.g), blendVividLight(base.b, blend.b));
			}
			
			// Hard mix
			float blendHardMix(float base, float blend)
			{
				return (blendVividLight(base, blend) < 0.5) ? 0.0 : 1.0;
			}
			
			float3 blendHardMix(float3 base, float3 blend)
			{
				return float3(blendHardMix(base.r, blend.r), blendHardMix(base.g, blend.g), blendHardMix(base.b, blend.b));
			}
			
			// Lighten
			float blendLighten(float base, float blend)
			{
				return max(blend, base);
			}
			
			float3 blendLighten(float3 base, float3 blend)
			{
				return float3(blendLighten(base.r, blend.r), blendLighten(base.g, blend.g), blendLighten(base.b, blend.b));
			}
			
			// Linear Burn
			float blendLinearBurn(float base, float blend)
			{
				// Note : Same implementation as BlendSubtractf
				return max(base + blend - 1.0, 0.0);
			}
			
			float3 blendLinearBurn(float3 base, float3 blend)
			{
				// Note : Same implementation as BlendSubtract
				return max(base + blend - float3(1.0, 1.0, 1.0), float3(0.0, 0.0, 0.0));
			}
			
			// Linear Dodge
			float blendLinearDodge(float base, float blend)
			{
				// Note : Same implementation as BlendAddf
				return min(base + blend, 1.0);
			}
			
			float3 blendLinearDodge(float3 base, float3 blend)
			{
				// Note : Same implementation as BlendAdd
				return base + blend;
			}
			
			// Linear light
			float blendLinearLight(float base, float blend)
			{
				return blend < 0.5 ? blendLinearBurn(base, (2.0 * blend)) : blendLinearDodge(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendLinearLight(float3 base, float3 blend)
			{
				return float3(blendLinearLight(base.r, blend.r), blendLinearLight(base.g, blend.g), blendLinearLight(base.b, blend.b));
			}
			
			// Multiply
			float blendMultiply(float base, float blend)
			{
				return base * blend;
			}
			float3 blendMultiply(float3 base, float3 blend)
			{
				return base * blend;
			}
			
			// Negation
			float blendNegation(float base, float blend)
			{
				return 1.0 - abs(1.0 - base - blend);
			}
			float3 blendNegation(float3 base, float3 blend)
			{
				return float3(1.0, 1.0, 1.0) - abs(float3(1.0, 1.0, 1.0) - base - blend);
			}
			
			// Normal
			float blendNormal(float base, float blend)
			{
				return blend;
			}
			float3 blendNormal(float3 base, float3 blend)
			{
				return blend;
			}
			
			// Phoenix
			float blendPhoenix(float base, float blend)
			{
				return min(base, blend) - max(base, blend) + 1.0;
			}
			float3 blendPhoenix(float3 base, float3 blend)
			{
				return min(base, blend) - max(base, blend) + float3(1.0, 1.0, 1.0);
			}
			
			// Pin light
			float blendPinLight(float base, float blend)
			{
				return (blend < 0.5) ? blendDarken(base, (2.0 * blend)) : blendLighten(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendPinLight(float3 base, float3 blend)
			{
				return float3(blendPinLight(base.r, blend.r), blendPinLight(base.g, blend.g), blendPinLight(base.b, blend.b));
			}
			
			// Screen
			float blendScreen(float base, float blend)
			{
				return 1.0 - ((1.0 - base) * (1.0 - blend));
			}
			
			float3 blendScreen(float3 base, float3 blend)
			{
				return float3(blendScreen(base.r, blend.r), blendScreen(base.g, blend.g), blendScreen(base.b, blend.b));
			}
			
			// Soft Light
			float blendSoftLight(float base, float blend)
			{
				return (blend < 0.5) ? (2.0 * base * blend + base * base * (1.0 - 2.0 * blend)) : (sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend));
			}
			
			float3 blendSoftLight(float3 base, float3 blend)
			{
				return float3(blendSoftLight(base.r, blend.r), blendSoftLight(base.g, blend.g), blendSoftLight(base.b, blend.b));
			}
			
			// Subtract
			float blendSubtract(float base, float blend)
			{
				return max(base - blend, 0.0);
			}
			
			float3 blendSubtract(float3 base, float3 blend)
			{
				return max(base - blend, 0.0);
			}
			
			// Difference
			float blendDifference(float base, float blend)
			{
				return abs(base - blend);
			}
			
			float3 blendDifference(float3 base, float3 blend)
			{
				return abs(base - blend);
			}
			
			// Divide
			float blendDivide(float base, float blend)
			{
				return base / max(blend, 0.0001);
			}
			
			float3 blendDivide(float3 base, float3 blend)
			{
				return base / max(blend, 0.0001);
			}
			
			float blendMixed(float base, float blend)
			{
				return base + base * blend;
			}
			
			float3 blendMixed(float3 base, float3 blend)
			{
				return base + base * blend;
			}
			
			float3 customBlend(float3 base, float3 blend, float blendType, float alpha = 1)
			{
				float3 output = base;
				switch(blendType)
				{
					case 0: output = lerp(base, blend, alpha); break;
					case 2: output = base * lerp(1, blend, alpha); break;
					case 6: output = lerp(base, blendScreen(base, blend), alpha); break;
					case 7: output = blendSubtract(base, blend * alpha); break;
					case 8: output = lerp(base, blendLinearDodge(base, blend), alpha); break;
					case 9: output = lerp(base, blendOverlay(base, blend), alpha); break;
					case 20: output = lerp(base, blendMixed(base, blend), alpha); break;
					default: output = 0; break;
				}
				return output;
			}
			
			float3 customBlend(float base, float blend, float blendType, float alpha = 1)
			{
				float3 output = base;
				switch(blendType)
				{
					case 0: output = lerp(base, blend, alpha); break;
					case 2: output = base * lerp(1, blend, alpha); break;
					case 6: output = lerp(base, blendScreen(base, blend), alpha); break;
					case 7: output = blendSubtract(base, blend * alpha); break;
					case 8: output = lerp(base, blendLinearDodge(base, blend), alpha); break;
					case 9: output = lerp(base, blendOverlay(base, blend), alpha); break;
					case 20: output = lerp(base, blendMixed(base, blend), alpha); break;
					default: output = 0; break;
				}
				return output;
			}
			
			#define REPLACE 0
			#define SUBSTRACT 1
			#define MULTIPLY 2
			#define DIVIDE 3
			#define MIN 4
			#define MAX 5
			#define AVERAGE 6
			#define ADD 7
			
			float maskBlend(float baseMask, float blendMask, float blendType)
			{
				float output = 0;
				switch(blendType)
				{
					case REPLACE: output = blendMask; break;
					case SUBSTRACT: output = baseMask - blendMask; break;
					case MULTIPLY: output = baseMask * blendMask; break;
					case DIVIDE: output = baseMask / blendMask; break;
					case MIN: output = min(baseMask, blendMask); break;
					case MAX: output = max(baseMask, blendMask); break;
					case AVERAGE: output = (baseMask + blendMask) * 0.5; break;
					case ADD: output = baseMask + blendMask; break;
				}
				return saturate(output);
			}
			
			float globalMaskBlend(float baseMask, float globalMaskIndex, float blendType, PoiMods poiMods)
			{
				if (globalMaskIndex == 0)
				{
					return baseMask;
				}
				else
				{
					return maskBlend(baseMask, poiMods.globalMask[globalMaskIndex - 1], blendType);
				}
			}
			
			float random(float2 p)
			{
				return frac(sin(dot(p, float2(12.9898, 78.2383))) * 43758.5453123);
			}
			
			float2 random2(float2 p)
			{
				return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
			}
			
			float3 random3(float2 p)
			{
				return frac(sin(float3(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)), dot(p, float2(248.3, 315.9)))) * 43758.5453);
			}
			
			float3 random3(float3 p)
			{
				return frac(sin(float3(dot(p, float3(127.1, 311.7, 248.6)), dot(p, float3(269.5, 183.3, 423.3)), dot(p, float3(248.3, 315.9, 184.2)))) * 43758.5453);
			}
			
			float3 randomFloat3(float2 Seed, float maximum)
			{
				return (.5 + float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed), float2(12.9898, 78.233))) * 43758.5453)
				) * .5) * (maximum);
			}
			
			float3 randomFloat3Range(float2 Seed, float Range)
			{
				return (float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
				) * 2 - 1) * Range;
			}
			
			float3 randomFloat3WiggleRange(float2 Seed, float Range, float wiggleSpeed)
			{
				float3 rando = (float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
				) * 2 - 1);
				float speed = 1 + wiggleSpeed;
				return float3(sin((_Time.x + rando.x * PI) * speed), sin((_Time.x + rando.y * PI) * speed), sin((_Time.x + rando.z * PI) * speed)) * Range;
			}
			
			void poiDither(float4 In, float4 ScreenPosition, out float4 Out)
			{
				float2 uv = ScreenPosition.xy * _ScreenParams.xy;
				float DITHER_THRESHOLDS[16] = {
					1.0 / 17.0, 9.0 / 17.0, 3.0 / 17.0, 11.0 / 17.0,
					13.0 / 17.0, 5.0 / 17.0, 15.0 / 17.0, 7.0 / 17.0,
					4.0 / 17.0, 12.0 / 17.0, 2.0 / 17.0, 10.0 / 17.0,
					16.0 / 17.0, 8.0 / 17.0, 14.0 / 17.0, 6.0 / 17.0
				};
				uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
				Out = In - DITHER_THRESHOLDS[index];
			}
			
			static const float Epsilon = 1e-10;
			// The weights of RGB contributions to luminance.
			// Should sum to unity.
			static const float3 HCYwts = float3(0.299, 0.587, 0.114);
			static const float HCLgamma = 3;
			static const float HCLy0 = 100;
			static const float HCLmaxL = 0.530454533953517; // == exp(HCLgamma / HCLy0) - 0.5
			static const float3 wref = float3(1.0, 1.0, 1.0);
			#define TAU 6.28318531
			
			float3 HUEtoRGB(in float H)
			{
				float R = abs(H * 6 - 3) - 1;
				float G = 2 - abs(H * 6 - 2);
				float B = 2 - abs(H * 6 - 4);
				return saturate(float3(R, G, B));
			}
			
			float3 RGBtoHCV(in float3 RGB)
			{
				// Based on work by Sam Hocevar and Emil Persson
				float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
				float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
				float C = Q.x - min(Q.w, Q.y);
				float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
				return float3(H, C, Q.x);
			}
			
			float3 HSVtoRGB(in float3 HSV)
			{
				float3 RGB = HUEtoRGB(HSV.x);
				return ((RGB - 1) * HSV.y + 1) * HSV.z;
			}
			
			float3 RGBtoHSV(in float3 RGB)
			{
				float3 HCV = RGBtoHCV(RGB);
				float S = HCV.y / (HCV.z + Epsilon);
				return float3(HCV.x, S, HCV.z);
			}
			
			float3 HSLtoRGB(in float3 HSL)
			{
				float3 RGB = HUEtoRGB(HSL.x);
				float C = (1 - abs(2 * HSL.z - 1)) * HSL.y;
				return (RGB - 0.5) * C + HSL.z;
			}
			
			float3 RGBtoHSL(in float3 RGB)
			{
				float3 HCV = RGBtoHCV(RGB);
				float L = HCV.z - HCV.y * 0.5;
				float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
				return float3(HCV.x, S, L);
			}
			
			void DecomposeHDRColor(in float3 linearColorHDR, out float3 baseLinearColor, out float exposure)
			{
				// Optimization/adaptation of https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/GUI/ColorMutator.cs#L23 but skips weird photoshop stuff
				float maxColorComponent = max(linearColorHDR.r, max(linearColorHDR.g, linearColorHDR.b));
				bool isSDR = maxColorComponent <= 1.0;
				
				float scaleFactor = isSDR ? 1.0 : (1.0 / maxColorComponent);
				exposure = isSDR ? 0.0 : log(maxColorComponent) * 1.44269504089; // ln(2)
				
				baseLinearColor = scaleFactor * linearColorHDR;
			}
			
			float3 ApplyHDRExposure(float3 linearColor, float exposure)
			{
				return linearColor * pow(2, exposure);
			}
			
			// Transforms an RGB color using a matrix. Note that S and V are absolute values here
			float3 ModifyViaHSV(float3 color, float h, float s, float v)
			{
				float3 colorHSV = RGBtoHSV(color);
				colorHSV.x = frac(colorHSV.x + h);
				colorHSV.y = saturate(colorHSV.y + s);
				colorHSV.z = saturate(colorHSV.z + v);
				return HSVtoRGB(colorHSV);
			}
			
			float3 ModifyViaHSV(float3 color, float3 HSVMod)
			{
				return ModifyViaHSV(color, HSVMod.x, HSVMod.y, HSVMod.z);
			}
			
			float4x4 brightnessMatrix(float brightness)
			{
				return float4x4(
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				brightness, brightness, brightness, 1
				);
			}
			
			float4x4 contrastMatrix(float contrast)
			{
				float t = (1.0 - contrast) / 2.0;
				
				return float4x4(
				contrast, 0, 0, 0,
				0, contrast, 0, 0,
				0, 0, contrast, 0,
				t, t, t, 1
				);
			}
			
			float4x4 saturationMatrix(float saturation)
			{
				float3 luminance = float3(0.3086, 0.6094, 0.0820);
				
				float oneMinusSat = 1.0 - saturation;
				
				float3 red = luminance.x * oneMinusSat;
				red += float3(saturation, 0, 0);
				
				float3 green = luminance.y * oneMinusSat;
				green += float3(0, saturation, 0);
				
				float3 blue = luminance.z * oneMinusSat;
				blue += float3(0, 0, saturation);
				
				return float4x4(
				red, 0,
				green, 0,
				blue, 0,
				0, 0, 0, 1
				);
			}
			
			float4 PoiColorBCS(float4 color, float brightness, float contrast, float saturation)
			{
				return mul(color, mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation))));
			}
			float3 PoiColorBCS(float3 color, float brightness, float contrast, float saturation)
			{
				return mul(float4(color, 1), mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation)))).rgb;
			}
			
			float3 linear_srgb_to_oklab(float3 c)
			{
				float l = 0.4122214708 * c.x + 0.5363325363 * c.y + 0.0514459929 * c.z;
				float m = 0.2119034982 * c.x + 0.6806995451 * c.y + 0.1073969566 * c.z;
				float s = 0.0883024619 * c.x + 0.2817188376 * c.y + 0.6299787005 * c.z;
				
				float l_ = pow(l, 1.0 / 3.0);
				float m_ = pow(m, 1.0 / 3.0);
				float s_ = pow(s, 1.0 / 3.0);
				
				return float3(
				0.2104542553 * l_ + 0.7936177850 * m_ - 0.0040720468 * s_,
				1.9779984951 * l_ - 2.4285922050 * m_ + 0.4505937099 * s_,
				0.0259040371 * l_ + 0.7827717662 * m_ - 0.8086757660 * s_
				);
			}
			
			float3 oklab_to_linear_srgb(float3 c)
			{
				float l_ = c.x + 0.3963377774 * c.y + 0.2158037573 * c.z;
				float m_ = c.x - 0.1055613458 * c.y - 0.0638541728 * c.z;
				float s_ = c.x - 0.0894841775 * c.y - 1.2914855480 * c.z;
				
				float l = l_ * l_ * l_;
				float m = m_ * m_ * m_;
				float s = s_ * s_ * s_;
				
				return float3(
				+ 4.0767416621 * l - 3.3077115913 * m + 0.2309699292 * s,
				- 1.2684380046 * l + 2.6097574011 * m - 0.3413193965 * s,
				- 0.0041960863 * l - 0.7034186147 * m + 1.7076147010 * s
				);
			}
			
			float3 hueShift(float3 color, float shift)
			{
				float3 oklab = linear_srgb_to_oklab(max(color, 0.0000000001));
				float hue = atan2(oklab.z, oklab.y);
				hue += shift * PI * 2;  // Add the hue shift
				
				float chroma = length(oklab.yz);
				oklab.y = cos(hue) * chroma;
				oklab.z = sin(hue) * chroma;
				
				return oklab_to_linear_srgb(oklab);
			}
			
			float3 hueShift(float4 color, float shift)
			{
				return hueShift(color.rgb, shift);
			}
			
			/*
			float3 hueShift(float3 color, float hueOffset)
			{
				color = RGBtoHSV(color);
				color.x = frac(hueOffset +color.x);
				return HSVtoRGB(color);
			}
			*/
			
			// LCH
			float xyzF(float t)
			{
				return lerp(pow(t, 1. / 3.), 7.787037 * t + 0.139731, step(t, 0.00885645));
			}
			float xyzR(float t)
			{
				return lerp(t * t * t, 0.1284185 * (t - 0.139731), step(t, 0.20689655));
			}
			
			float4x4 poiRotationMatrixFromAngles(float x, float y, float z)
			{
				float angleX = radians(x);
				float c = cos(angleX);
				float s = sin(angleX);
				float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
				0, c, -s, 0,
				0, s, c, 0,
				0, 0, 0, 1);
				
				float angleY = radians(y);
				c = cos(angleY);
				s = sin(angleY);
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
				0, 1, 0, 0,
				- s, 0, c, 0,
				0, 0, 0, 1);
				
				float angleZ = radians(z);
				c = cos(angleZ);
				s = sin(angleZ);
				float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
				s, c, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
				
				return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
			}
			
			float4x4 poiRotationMatrixFromAngles(float3 angles)
			{
				float angleX = radians(angles.x);
				float c = cos(angleX);
				float s = sin(angleX);
				float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
				0, c, -s, 0,
				0, s, c, 0,
				0, 0, 0, 1);
				
				float angleY = radians(angles.y);
				c = cos(angleY);
				s = sin(angleY);
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
				0, 1, 0, 0,
				- s, 0, c, 0,
				0, 0, 0, 1);
				
				float angleZ = radians(angles.z);
				c = cos(angleZ);
				s = sin(angleZ);
				float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
				s, c, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
				
				return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
			}
			
			float3 getCameraPosition()
			{
				#ifdef USING_STEREO_MATRICES
				return lerp(unity_StereoWorldSpaceCameraPos[0], unity_StereoWorldSpaceCameraPos[1], 0.5);
				#endif
				return _WorldSpaceCameraPos;
			}
			
			float2 calcPixelScreenUVs(half4 grabPos)
			{
				half2 uv = grabPos.xy / (grabPos.w + 0.0000000001);
				#if UNITY_SINGLE_PASS_STEREO
				uv.xy *= half2(_ScreenParams.x * 2, _ScreenParams.y);
				#else
				uv.xy *= _ScreenParams.xy;
				#endif
				
				return uv;
			}
			
			float CalcMipLevel(float2 texture_coord)
			{
				float2 dx = ddx(texture_coord);
				float2 dy = ddy(texture_coord);
				float delta_max_sqr = max(dot(dx, dx), dot(dy, dy));
				
				return 0.5 * log2(delta_max_sqr);
			}
			
			float inverseLerp(float A, float B, float T)
			{
				return (T - A) / (B - A);
			}
			
			float inverseLerp2(float2 a, float2 b, float2 value)
			{
				float2 AB = b - a;
				float2 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			float inverseLerp3(float3 a, float3 b, float3 value)
			{
				float3 AB = b - a;
				float3 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			float inverseLerp4(float4 a, float4 b, float4 value)
			{
				float4 AB = b - a;
				float4 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			/*
			MIT License
			
			Copyright (c) 2019 wraikny
			
			Permission is hereby granted, free of charge, to any person obtaining a copy
			of this software and associated documentation files (the "Software"), to deal
			in the Software without restriction, including without limitation the rights
			to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
			copies of the Software, and to permit persons to whom the Software is
			furnished to do so, subject to the following conditions:
			
			The above copyright notice and this permission notice shall be included in all
			copies or substantial portions of the Software.
			
			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
			IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
			FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
			AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
			LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
			OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
			SOFTWARE.
			
			VertexTransformShader is dependent on:
			*/
			
			float4 quaternion_conjugate(float4 v)
			{
				return float4(
				v.x, -v.yzw
				);
			}
			
			float4 quaternion_mul(float4 v1, float4 v2)
			{
				float4 result1 = (v1.x * v2 + v1 * v2.x);
				
				float4 result2 = float4(
				- dot(v1.yzw, v2.yzw),
				cross(v1.yzw, v2.yzw)
				);
				
				return float4(result1 + result2);
			}
			
			// angle : radians
			float4 get_quaternion_from_angle(float3 axis, float angle)
			{
				float sn = sin(angle * 0.5);
				float cs = cos(angle * 0.5);
				return float4(axis * sn, cs);
			}
			
			float4 quaternion_from_vector(float3 inVec)
			{
				return float4(0.0, inVec);
			}
			
			float degree_to_radius(float degree)
			{
				return (
				degree / 180.0 * PI
				);
			}
			
			float3 rotate_with_quaternion(float3 inVec, float3 rotation)
			{
				float4 qx = get_quaternion_from_angle(float3(1, 0, 0), radians(rotation.x));
				float4 qy = get_quaternion_from_angle(float3(0, 1, 0), radians(rotation.y));
				float4 qz = get_quaternion_from_angle(float3(0, 0, 1), radians(rotation.z));
				
				#define MUL3(A, B, C) quaternion_mul(quaternion_mul((A), (B)), (C))
				float4 quaternion = normalize(MUL3(qx, qy, qz));
				float4 conjugate = quaternion_conjugate(quaternion);
				
				float4 inVecQ = quaternion_from_vector(inVec);
				
				float3 rotated = (
				MUL3(quaternion, inVecQ, conjugate)
				).yzw;
				
				return rotated;
			}
			
			float4 transform(float4 input, float4 pos, float4 rotation, float4 scale)
			{
				input.rgb *= (scale.xyz * scale.w);
				input = float4(rotate_with_quaternion(input.xyz, rotation.xyz * rotation.w) + (pos.xyz * pos.w), input.w);
				return input;
			}
			
			float2 RotateUV(float2 _uv, float _radian, float2 _piv, float _time)
			{
				float RotateUV_ang = _radian;
				float RotateUV_cos = cos(_time * RotateUV_ang);
				float RotateUV_sin = sin(_time * RotateUV_ang);
				return (mul(_uv - _piv, float2x2(RotateUV_cos, -RotateUV_sin, RotateUV_sin, RotateUV_cos)) + _piv);
			}
			
			/*
			MIT END
			*/
			
			float3 RotateAroundAxis(float3 original, float3 axis, float radian)
			{
				float s = sin(radian);
				float c = cos(radian);
				float one_minus_c = 1.0 - c;
				
				axis = normalize(axis);
				float3x3 rot_mat = {
					one_minus_c * axis.x * axis.x + c, one_minus_c * axis.x * axis.y - axis.z * s, one_minus_c * axis.z * axis.x + axis.y * s,
					one_minus_c * axis.x * axis.y + axis.z * s, one_minus_c * axis.y * axis.y + c, one_minus_c * axis.y * axis.z - axis.x * s,
					one_minus_c * axis.z * axis.x - axis.y * s, one_minus_c * axis.y * axis.z + axis.x * s, one_minus_c * axis.z * axis.z + c
				};
				return mul(rot_mat, original);
			}
			
			float3 poiThemeColor(in PoiMods poiMods, in float3 srcColor, in float themeIndex)
			{
				if (themeIndex == 0) return srcColor;
				themeIndex -= 1;
				
				if (themeIndex <= 3)
				{
					return poiMods.globalColorTheme[themeIndex];
				}
				
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable)
				{
					return poiMods.globalColorTheme[themeIndex];
				}
				#endif
				
				return srcColor;
			}
			
			float3 lilToneCorrection(float3 c, float4 hsvg)
			{
				// gamma
				c = pow(abs(c), hsvg.w);
				// rgb -> hsv
				float4 p = (c.b > c.g) ? float4(c.bg, -1.0, 2.0 / 3.0) : float4(c.gb, 0.0, -1.0 / 3.0);
				float4 q = (p.x > c.r) ? float4(p.xyw, c.r) : float4(c.r, p.yzx);
				float d = q.x - min(q.w, q.y);
				float e = 1.0e-10;
				float3 hsv = float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
				// shift
				hsv = float3(hsv.x + hsvg.x, saturate(hsv.y * hsvg.y), saturate(hsv.z * hsvg.z));
				// hsv -> rgb
				return hsv.z - hsv.z * hsv.y + hsv.z * hsv.y * saturate(abs(frac(hsv.x + float3(1.0, 2.0 / 3.0, 1.0 / 3.0)) * 6.0 - 3.0) - 1.0);
			}
			
			float3 lilBlendColor(float3 dstCol, float3 srcCol, float3 srcA, int blendMode)
			{
				float3 ad = dstCol + srcCol;
				float3 mu = dstCol * srcCol;
				float3 outCol;
				if (blendMode == 0) outCol = srcCol;               // Normal
				if (blendMode == 1) outCol = ad;                   // Add
				if (blendMode == 2) outCol = max(ad - mu, dstCol); // Screen
				if (blendMode == 3) outCol = mu;                   // Multiply
				return lerp(dstCol, outCol, srcA);
			}
			
			float lilIsIn0to1(float f)
			{
				float value = 0.5 - abs(f - 0.5);
				return saturate(value / clamp(fwidth(value), 0.0001, 1.0));
			}
			
			float lilIsIn0to1(float f, float nv)
			{
				float value = 0.5 - abs(f - 0.5);
				return saturate(value / clamp(fwidth(value), 0.0001, nv));
			}
			
			float poiEdgeLinearNoSaturate(float value, float border)
			{
				return (value - border) / clamp(fwidth(value), 0.0001, 1.0);
			}
			
			float3 poiEdgeLinearNoSaturate(float value, float3 border)
			{
				return float3(
				(value - border.x) / clamp(fwidth(value), 0.0001, 1.0),
				(value - border.y) / clamp(fwidth(value), 0.0001, 1.0),
				(value - border.z) / clamp(fwidth(value), 0.0001, 1.0)
				);
			}
			
			float poiEdgeLinearNoSaturate(float value, float border, float blur)
			{
				float borderMin = saturate(border - blur * 0.5);
				float borderMax = saturate(border + blur * 0.5);
				return (value - borderMin) / saturate(borderMax - borderMin + fwidth(value));
			}
			
			float poiEdgeLinearNoSaturate(float value, float border, float blur, float borderRange)
			{
				float borderMin = saturate(border - blur * 0.5 - borderRange);
				float borderMax = saturate(border + blur * 0.5);
				return (value - borderMin) / saturate(borderMax - borderMin + fwidth(value));
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border)
			{
				//return (value - border) / clamp(fwidth(value), 0.0001, 1.0);
				
				float fwidthValue = fwidth(value);
				return smoothstep(border - fwidthValue, border + fwidthValue, value);
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border, float blur)
			{
				float fwidthValue = fwidth(value);
				float borderMin = saturate(border - blur * 0.5);
				float borderMax = saturate(border + blur * 0.5);
				return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border, float blur, float borderRange)
			{
				float fwidthValue = fwidth(value);
				float borderMin = saturate(border - blur * 0.5 - borderRange);
				float borderMax = saturate(border + blur * 0.5);
				return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
			}
			
			float poiEdgeNonLinear(float value, float border)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border));
			}
			
			float poiEdgeNonLinear(float value, float border, float blur)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border, blur));
			}
			
			float poiEdgeNonLinear(float value, float border, float blur, float borderRange)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border, blur, borderRange));
			}
			
			float poiEdgeLinear(float value, float border)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border));
			}
			
			float poiEdgeLinear(float value, float border, float blur)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border, blur));
			}
			
			float poiEdgeLinear(float value, float border, float blur, float borderRange)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border, blur, borderRange));
			}
			// From https://github.com/lilxyzw/OpenLit/blob/main/Assets/OpenLit/core.hlsl
			float3 OpenLitLinearToSRGB(float3 col)
			{
				return LinearToGammaSpace(col);
			}
			
			float3 OpenLitSRGBToLinear(float3 col)
			{
				return GammaToLinearSpace(col);
			}
			
			float OpenLitLuminance(float3 rgb)
			{
				#if defined(UNITY_COLORSPACE_GAMMA)
				return dot(rgb, float3(0.22, 0.707, 0.071));
				#else
				return dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
				#endif
			}
			
			float3 AdjustLitLuminance(float3 rgb, float targetLuminance)
			{
				float currentLuminance;
				#if defined(UNITY_COLORSPACE_GAMMA)
				currentLuminance = dot(rgb, float3(0.22, 0.707, 0.071));
				#else
				currentLuminance = dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
				#endif
				
				float luminanceRatio = targetLuminance / currentLuminance;
				return rgb * luminanceRatio;
			}
			
			float3 ClampLuminance(float3 rgb, float minLuminance, float maxLuminance)
			{
				float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
				float minRatio = (currentLuminance != 0) ? minLuminance / currentLuminance : 1.0;
				float maxRatio = (currentLuminance != 0) ? maxLuminance / currentLuminance : 1.0;
				float luminanceRatio = clamp(min(maxRatio, max(minRatio, 1.0)), 0.0, 1.0);
				return lerp(rgb, rgb * luminanceRatio, luminanceRatio < 1.0);
			}
			
			float3 MaxLuminance(float3 rgb, float maxLuminance)
			{
				float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
				float luminanceRatio = (currentLuminance != 0) ? maxLuminance / currentLuminance : 1.0;
				return lerp(rgb, rgb * luminanceRatio, currentLuminance > maxLuminance);
			}
			
			float OpenLitGray(float3 rgb)
			{
				return dot(rgb, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0));
			}
			
			void OpenLitShadeSH9ToonDouble(float3 lightDirection, out float3 shMax, out float3 shMin)
			{
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 N = lightDirection * 0.666666;
				float4 vB = N.xyzz * N.yzzx;
				// L0 L2
				float3 res = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
				res.r += dot(unity_SHBr, vB);
				res.g += dot(unity_SHBg, vB);
				res.b += dot(unity_SHBb, vB);
				res += unity_SHC.rgb * (N.x * N.x - N.y * N.y);
				// L1
				float3 l1;
				l1.r = dot(unity_SHAr.rgb, N);
				l1.g = dot(unity_SHAg.rgb, N);
				l1.b = dot(unity_SHAb.rgb, N);
				shMax = res + l1;
				shMin = res - l1;
				#if defined(UNITY_COLORSPACE_GAMMA)
				shMax = OpenLitLinearToSRGB(shMax);
				shMin = OpenLitLinearToSRGB(shMin);
				#endif
				#else
				shMax = 0.0;
				shMin = 0.0;
				#endif
			}
			
			float3 OpenLitComputeCustomLightDirection(float4 lightDirectionOverride)
			{
				float3 customDir = length(lightDirectionOverride.xyz) * normalize(mul((float3x3)unity_ObjectToWorld, lightDirectionOverride.xyz));
				return lightDirectionOverride.w ? customDir : lightDirectionOverride.xyz; // .w isn't doc'd anywhere and is always 0 unless end user changes it
				
			}
			
			float3 OpenLitLightingDirectionForSH9()
			{
				float3 mainDir = _WorldSpaceLightPos0.xyz * OpenLitLuminance(_LightColor0.rgb);
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 sh9Dir = unity_SHAr.xyz * 0.333333 + unity_SHAg.xyz * 0.333333 + unity_SHAb.xyz * 0.333333;
				float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
				#else
				float3 sh9Dir = 0;
				float3 sh9DirAbs = 0;
				#endif
				
				float3 lightDirectionForSH9 = sh9Dir + mainDir;
				lightDirectionForSH9 = dot(lightDirectionForSH9, lightDirectionForSH9) < 0.000001 ? 0 : normalize(lightDirectionForSH9);
				return lightDirectionForSH9;
			}
			
			float3 OpenLitLightingDirection(float4 lightDirectionOverride)
			{
				float3 mainDir = _WorldSpaceLightPos0.xyz * OpenLitLuminance(_LightColor0.rgb);
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 sh9Dir = unity_SHAr.xyz * 0.333333 + unity_SHAg.xyz * 0.333333 + unity_SHAb.xyz * 0.333333;
				float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
				#else
				float3 sh9Dir = 0;
				float3 sh9DirAbs = 0;
				#endif
				float3 customDir = OpenLitComputeCustomLightDirection(lightDirectionOverride);
				
				return normalize(sh9DirAbs + mainDir + customDir);
			}
			
			float3 OpenLitLightingDirection()
			{
				float4 customDir = float4(0.001, 0.002, 0.001, 0.0);
				return OpenLitLightingDirection(customDir);
			}
			
			inline float4 CalculateFrustumCorrection()
			{
				float x1 = -UNITY_MATRIX_P._31 / (UNITY_MATRIX_P._11 * UNITY_MATRIX_P._34);
				float x2 = -UNITY_MATRIX_P._32 / (UNITY_MATRIX_P._22 * UNITY_MATRIX_P._34);
				return float4(x1, x2, 0, UNITY_MATRIX_P._33 / UNITY_MATRIX_P._34 + x1 * UNITY_MATRIX_P._13 + x2 * UNITY_MATRIX_P._23);
			}
			
			inline float CorrectedLinearEyeDepth(float z, float B)
			{
				return 1.0 / (z / UNITY_MATRIX_P._34 + B);
			}
			
			//Silent's code
			float2 sharpSample(float4 texelSize, float2 p)
			{
				p = p * texelSize.zw;
				float2 c = max(0.0, fwidth(p));
				p = floor(p) + saturate(frac(p) / c);
				p = (p - 0.5) * texelSize.xy;
				return p;
			}
			
			void applyToGlobalMask(inout PoiMods poiMods, int index, int blendType, float val)
			{
				float valBlended = saturate(maskBlend(poiMods.globalMask[index], val, blendType));
				switch(index)
				{
					case 0: poiMods.globalMask[0] = valBlended; break;
					case 1: poiMods.globalMask[1] = valBlended; break;
					case 2: poiMods.globalMask[2] = valBlended; break;
					case 3: poiMods.globalMask[3] = valBlended; break;
					case 4: poiMods.globalMask[4] = valBlended; break;
					case 5: poiMods.globalMask[5] = valBlended; break;
					case 6: poiMods.globalMask[6] = valBlended; break;
					case 7: poiMods.globalMask[7] = valBlended; break;
					case 8: poiMods.globalMask[8] = valBlended; break;
					case 9: poiMods.globalMask[9] = valBlended; break;
					case 10: poiMods.globalMask[10] = valBlended; break;
					case 11: poiMods.globalMask[11] = valBlended; break;
					case 12: poiMods.globalMask[12] = valBlended; break;
					case 13: poiMods.globalMask[13] = valBlended; break;
					case 14: poiMods.globalMask[14] = valBlended; break;
					case 15: poiMods.globalMask[15] = valBlended; break;
				}
			}
			
			void assignValueToVectorFromIndex(inout float4 vec, int index, float value)
			{
				switch(index)
				{
					case 0: vec[0] = value; break;
					case 1: vec[1] = value; break;
					case 2: vec[2] = value; break;
					case 3: vec[3] = value; break;
				}
			}
			
			// SNose
			float3 mod289(float3 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			float2 mod289(float2 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			float3 permute(float3 x)
			{
				return mod289(((x * 34.0) + 1.0) * x);
			}
			
			float snoise(float2 v)
			{
				const float4 C = float4(0.211324865405187, // (3.0-sqrt(3.0))/6.0
				0.366025403784439, // 0.5*(sqrt(3.0)-1.0)
				- 0.577350269189626, // -1.0 + 2.0 * C.x
				0.024390243902439); // 1.0 / 41.0
				float2 i = floor(v + dot(v, C.yy));
				float2 x0 = v - i + dot(i, C.xx);
				float2 i1;
				i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod289(i); // Avoid truncation effects in permutation
				float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0))
				+ i.x + float3(0.0, i1.x, 1.0));
				
				float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
				m = m * m ;
				m = m * m ;
				float3 x = 2.0 * frac(p * C.www) - 1.0;
				float3 h = abs(x) - 0.5;
				float3 ox = floor(x + 0.5);
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot(m, g);
			}
			
			float nsqDistance(float2 a, float2 b)
			{
				return dot(a - b, a - b);
			}
			
			float poiInvertToggle(in float value, in float toggle)
			{
				return (toggle == 0 ? value : 1 - value);
			}
			
			// OKLAB
			
			float3 PoiBlendNormal(float3 dstNormal, float3 srcNormal)
			{
				return float3(dstNormal.xy + srcNormal.xy, dstNormal.z * srcNormal.z);
			}
			
			float3 lilTransformDirOStoWS(float3 directionOS, bool doNormalize)
			{
				if (doNormalize) return normalize(mul((float3x3)unity_ObjectToWorld, directionOS));
				else            return mul((float3x3)unity_ObjectToWorld, directionOS);
			}
			
			float2 poiGetWidthAndHeight(Texture2D tex)
			{
				uint width, height;
				tex.GetDimensions(width, height);
				return float2(width, height);
			}
			
			float2 poiGetWidthAndHeight(Texture2DArray tex)
			{
				uint width, height, element;
				tex.GetDimensions(width, height, element);
				return float2(width, height);
			}
			
			VertexOut vert(
			#ifndef POI_TESSELLATED
			appdata v
			#else
			tessAppData v
			#endif
			)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				VertexOut o;
				PoiInitStruct(VertexOut, o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				#ifdef POI_TESSELLATED
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(v);
				#endif
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.tangent.xyz = UnityObjectToWorldDir(v.tangent);
				o.tangent.w = v.tangent.w;
				o.vertexColor = v.color;
				
				o.uv[0] = float4(v.uv0.xy, v.uv1.xy);
				o.uv[1] = float4(v.uv2.xy, v.uv3.xy);
				
				#if defined(LIGHTMAP_ON)
				o.lightmapUV.xy = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				#ifdef DYNAMICLIGHTMAP_ON
				o.lightmapUV.zw = v.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif
				
				o.localPos = v.vertex;
				o.worldPos = mul(unity_ObjectToWorld, o.localPos);
				
				float3 localOffset = float3(0, 0, 0);
				float3 worldOffset = float3(0, 0, 0);
				
				o.localPos.rgb += localOffset;
				o.worldPos.rgb += worldOffset;
				
				o.pos = UnityObjectToClipPos(o.localPos);
				
				#ifdef POI_PASS_OUTLINE
				#if defined(UNITY_REVERSED_Z)
				//DX
				o.pos.z += _Offset_Z * - 0.01;
				#else
				//OpenGL
				o.pos.z += _Offset_Z * 0.01;
				#endif
				#endif
				//o.grabPos = ComputeGrabScreenPos(o.pos);
				
				#ifndef FORWARD_META_PASS
				#if !defined(UNITY_PASS_SHADOWCASTER)
				UNITY_TRANSFER_SHADOW(o, o.uv[0].xy);
				#else
				v.vertex.xyz = o.localPos.xyz;
				TRANSFER_SHADOW_CASTER_NOPOS(o, o.pos);
				#endif
				#endif
				
				UNITY_TRANSFER_FOG(o, o.pos);
				
				if (_RenderingReduceClipDistance)
				{
					if (o.pos.w < _ProjectionParams.y * 1.01 && o.pos.w > 0)
					{
						#if defined(UNITY_REVERSED_Z) // DirectX
						o.pos.z = o.pos.z * 0.0001 + o.pos.w * 0.999;
						#else // OpenGL
						o.pos.z = o.pos.z * 0.0001 - o.pos.w * 0.999;
						#endif
					}
				}
				
				#ifdef POI_PASS_META
				o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
				#endif
				
				return o;
			}
			
			float4 frag(VertexOut i, uint facing : SV_IsFrontFace) : SV_Target
			/*
			#ifdef
			, out float depth : SV_DEPTH
			#endif
			*/
			{
				clip(_RenderingEarlyZEnabled - 1.0);
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				return float4(1, 1, 1, 1);
			}
			
			ENDCG
		}
		//endex
		
		Pass
		{
			Name "Base"
			Tags { "LightMode" = "ForwardBase" }
			
			ZWrite [_ZWrite]
			Cull [_Cull]
			
			AlphaToMask [_AlphaToCoverage]
			ZTest [_ZTest]
			ColorMask [_ColorMask]
			Offset [_OffsetFactor], [_OffsetUnits]
			
			BlendOp [_BlendOp], [_BlendOpAlpha]
			Blend [_SrcBlend] [_DstBlend], [_SrcBlendAlpha] [_DstBlendAlpha]
			
			CGPROGRAM
			/*
			// Disable warnings we aren't interested in
			#if defined(UNITY_COMPILER_HLSL)
			#pragma warning(disable : 3205) // conversion of larger type to smaller
			#pragma warning(disable : 3568) // unknown pragma ignored
			#pragma warning(disable : 3571) // "pow(f,e) will not work for negative f"; however in majority of our calls to pow we know f is not negative
			#pragma warning(disable : 3206) // implicit truncation of vector type
			#endif
			*/
			#pragma target 5.0
			//ifex 0==0
			#pragma skip_optimizations d3d11
			//endex
			
			#pragma shader_feature_local _STOCHASTICMODE_DELIOT_HEITZ _STOCHASTICMODE_HEXTILE _STOCHASTICMODE_NONE
			
			//ifex _MainColorAdjustToggle==0
			#pragma shader_feature COLOR_GRADING_HDR
			//endex
			
			//#pragma shader_feature KEYWORD
			
			#pragma skip_variants LIGHTMAP_ON DYNAMICLIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SHADOWMASK DIRLIGHTMAP_COMBINED _MIXED_LIGHTING_SUBTRACTIVE
			#pragma skip_variants DECALS_OFF DECALS_3RT DECALS_4RT DECAL_SURFACE_GRADIENT _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma skip_variants _ADDITIONAL_LIGHT_SHADOWS
			#pragma skip_variants PROBE_VOLUMES_OFF PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
			#pragma skip_variants _SCREEN_SPACE_OCCLUSION
			
			//ifex _PoiParallax==0
			#pragma shader_feature_local POI_PARALLAX
			//endex
			
			//ifex _EnableEmission==0
			#pragma shader_feature _EMISSION
			//endex
			//ifex _EnableEmission1==0
			#pragma shader_feature_local POI_EMISSION_1
			//endex
			//ifex _EnableEmission2==0
			#pragma shader_feature_local POI_EMISSION_2
			//endex
			//ifex _EnableEmission3==0
			#pragma shader_feature_local POI_EMISSION_3
			//endex
			
			//ifex _GlitterEnable==0
			#pragma shader_feature _SUNDISK_SIMPLE
			//endex
			
			//ifex _PoiInternalParallax==0
			#pragma shader_feature_local POI_INTERNALPARALLAX
			//endex
			
			#pragma multi_compile_fwdbase
			#pragma multi_compile_instancing
			#pragma multi_compile_fog
			#pragma multi_compile _ VERTEXLIGHT_ON
			#define POI_PASS_BASE
			
			// UNITY Includes
			#include "UnityCG.cginc"
			#include "UnityStandardUtils.cginc"
			#include "AutoLight.cginc"
			#include "UnityLightingCommon.cginc"
			#include "UnityPBSLighting.cginc"
			#ifdef POI_PASS_META
			#include "UnityMetaPass.cginc"
			#endif
			#pragma vertex vert
			
			#pragma fragment frag
			
			#define DielectricSpec float4(0.04, 0.04, 0.04, 1.0 - 0.04)
			#define PI float(3.14159265359)
			
			#define POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, samplertex, coord, dx, dy) tex.SampleGrad(sampler##samplertex, coord, dx, dy)
			#define POI2D_SAMPLE_TEX2D_SAMPLERGRADD(tex, samp, uv, pan, dx, dy) tex.SampleGrad(samp, POI_PAN_UV(uv, pan), dx, dy)
			
			#define POI_PAN_UV(uv, pan) (uv + _Time.x * pan)
			#define POI2D_SAMPLER_PAN(tex, texSampler, uv, pan) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, POI_PAN_UV(uv, pan)))
			#define POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, POI_PAN_UV(uv, pan), dx, dy))
			#define POI2D_SAMPLER(tex, texSampler, uv) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, uv))
			#define POI_SAMPLE_1D_X(tex, samp, uv) tex.Sample(samp, float2(uv, 0.5))
			#define POI2D_SAMPLER_GRAD(tex, texSampler, uv, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, uv, dx, dy))
			#define POI2D_SAMPLER_GRADD(tex, texSampler, uv, dx, dy) tex.SampleGrad(texSampler, uv, dx, dy)
			#define POI2D_PAN(tex, uv, pan) (tex2D(tex, POI_PAN_UV(uv, pan)))
			#define POI2D(tex, uv) (tex2D(tex, uv))
			#define POI_SAMPLE_TEX2D(tex, uv) (UNITY_SAMPLE_TEX2D(tex, uv))
			#define POI_SAMPLE_TEX2D_PAN(tex, uv, pan) (UNITY_SAMPLE_TEX2D(tex, POI_PAN_UV(uv, pan)))
			#define POI_SAMPLE_CUBE_LOD(tex, samp, uv, lod) texCUBElod(tex, float4(uv, 0, lod))
			
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define TEXTURE2D_SCREEN(tex)                   Texture2DArray tex
			#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, float3(uv, unity_StereoEyeIndex))
			#else
			#define TEXTURE2D_SCREEN(tex)                   Texture2D tex
			#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, uv)
			#endif
			
			// When using, properties won't properly lock at optimize time; needs macro evaluation implemented
			// #define POI2D_MAINTEX_SAMPLER_PAN_INLINED(tex, poiMesh) (POI2D_SAMPLER_PAN(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan))
			
			#define POI_SAFE_RGB0 float4(mainTexture.rgb * .0001, 0)
			#define POI_SAFE_RGB1 float4(mainTexture.rgb * .0001, 1)
			#define POI_SAFE_RGBA mainTexture
			
			#if defined(UNITY_COMPILER_HLSL)
			#define PoiInitStruct(type, name) name = (type)0;
			#else
			#define PoiInitStruct(type, name)
			#endif
			
			#define POI_ERROR(poiMesh, gridSize) lerp(float3(1, 0, 1), float3(0, 0, 0), fmod(floor((poiMesh.worldPos.x) * gridSize) + floor((poiMesh.worldPos.y) * gridSize) + floor((poiMesh.worldPos.z) * gridSize), 2) == 0)
			#define POI_NAN (asfloat(-1))
			
			#define POI_MODE_OPAQUE 0
			#define POI_MODE_CUTOUT 1
			#define POI_MODE_FADE 2
			#define POI_MODE_TRANSPARENT 3
			#define POI_MODE_ADDITIVE 4
			#define POI_MODE_SOFTADDITIVE 5
			#define POI_MODE_MULTIPLICATIVE 6
			#define POI_MODE_2XMULTIPLICATIVE 7
			#define POI_MODE_TRANSCLIPPING 9
			
			/*
			Texture2D ;
			float4 _ST;
			float2 Pan;
			float UV;
			float Stochastic;
			
			[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos XZ, 5, Polar UV, 6, Distorted UV, 7 )]
			*/
			
			float _GrabMode;
			float _Mode;
			
			float _StochasticDeliotHeitzDensity;
			float _StochasticHexGridDensity;
			float _StochasticHexRotationStrength;
			float _StochasticHexFallOffContrast;
			float _StochasticHexFallOffPower;
			
			#if defined(PROP_LIGHTINGAOMAPS) || !defined(OPTIMIZER_ENABLED)
			Texture2D _LightingAOMaps;
			#endif
			float4 _LightingAOMaps_ST;
			float2 _LightingAOMapsPan;
			float _LightingAOMapsUV;
			float _LightDataAOStrengthR;
			float _LightDataAOStrengthG;
			float _LightDataAOStrengthB;
			float _LightDataAOStrengthA;
			float _LightDataAOGlobalMaskR;
			float _LightDataAOGlobalMaskBlendTypeR;
			
			#if defined(PROP_LIGHTINGDETAILSHADOWMAPS) || !defined(OPTIMIZER_ENABLED)
			Texture2D _LightingDetailShadowMaps;
			#endif
			float4 _LightingDetailShadowMaps_ST;
			float2 _LightingDetailShadowMapsPan;
			float _LightingDetailShadowMapsUV;
			float _LightingDetailShadowStrengthR;
			float _LightingDetailShadowStrengthG;
			float _LightingDetailShadowStrengthB;
			float _LightingDetailShadowStrengthA;
			float _LightingAddDetailShadowStrengthR;
			float _LightingAddDetailShadowStrengthG;
			float _LightingAddDetailShadowStrengthB;
			float _LightingAddDetailShadowStrengthA;
			float _LightDataDetailShadowGlobalMaskR;
			float _LightDataDetailShadowGlobalMaskBlendTypeR;
			
			#if defined(PROP_LIGHTINGSHADOWMASKS) || !defined(OPTIMIZER_ENABLED)
			Texture2D _LightingShadowMasks;
			#endif
			float4 _LightingShadowMasks_ST;
			float2 _LightingShadowMasksPan;
			float _LightingShadowMasksUV;
			float _LightingShadowMaskStrengthR;
			float _LightingShadowMaskStrengthG;
			float _LightingShadowMaskStrengthB;
			float _LightingShadowMaskStrengthA;
			float _LightDataShadowMaskGlobalMaskR;
			float _LightDataShadowMaskGlobalMaskBlendTypeR;
			
			// Lighting Data
			float _Unlit_Intensity;
			float _LightingColorMode;
			float _LightingMapMode;
			float _LightingDirectionMode;
			float3 _LightngForcedDirection;
			float _LightingViewDirOffsetPitch;
			float _LightingViewDirOffsetYaw;
			float _LightingIndirectUsesNormals;
			float _LightingCapEnabled;
			float _LightingCap;
			float _LightingForceColorEnabled;
			float3 _LightingForcedColor;
			float _LightingForcedColorThemeIndex;
			float _LightingCastedShadows;
			float _LightingMonochromatic;
			float _LightingMinLightBrightness;
			// Additive Lighting Data
			float _LightingAdditiveEnable;
			float _LightingAdditiveLimited;
			float _LightingAdditiveLimit;
			float _LightingAdditiveCastedShadows;
			float _LightingAdditiveMonochromatic;
			float _LightingAdditivePassthrough;
			float _DisableDirectionalInAdd;
			float _LightingVertexLightingEnabled;
			float _LightingMirrorVertexLightingEnabled;
			// Lighting Data Debug
			float _LightDataDebugEnabled;
			float _LightingDebugVisualize;
			
			float _IgnoreFog;
			float _RenderingReduceClipDistance;
			int _FlipBackfaceNormals;
			float _AddBlendOp;
			float _Cull;
			
			float4 _Color;
			float _ColorThemeIndex;
			UNITY_DECLARE_TEX2D(_MainTex);
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
			float _MainPixelMode;
			float4 _MainTex_ST;
			float2 _MainTexPan;
			float _MainTexUV;
			float4 _MainTex_TexelSize;
			float _MainTexStochastic;
			#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _BumpMap;
			#endif
			float4 _BumpMap_ST;
			float2 _BumpMapPan;
			float _BumpMapUV;
			float _BumpScale;
			float _BumpMapStochastic;
			#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _AlphaMask;
			float4 _AlphaMask_ST;
			float2 _AlphaMaskPan;
			float _AlphaMaskUV;
			float _AlphaMaskInvert;
			float _AlphaMaskMode;
			float _AlphaMaskScale;
			float _AlphaMaskValue;
			#endif
			float _Cutoff;
			//ifex _MainColorAdjustToggle==0
			float _MainColorAdjustToggle;
			#if defined(PROP_MAINCOLORADJUSTTEXTURE) || !defined(OPTIMIZER_ENABLED)
			Texture2D _MainColorAdjustTexture;
			#endif
			float4 _MainColorAdjustTexture_ST;
			float2 _MainColorAdjustTexturePan;
			float _MainColorAdjustTextureUV;
			float _MainHueShiftToggle;
			float _MainHueShiftReplace;
			float _MainHueShift;
			float _MainHueShiftSpeed;
			float _Saturation;
			float _MainBrightness;
			
			float _MainHueALCTEnabled;
			float _MainALHueShiftBand;
			float _MainALHueShiftCTIndex;
			float _MainHueALMotionSpeed;
			
			float _MainHueGlobalMask;
			float _MainHueGlobalMaskBlendType;
			float _MainSaturationGlobalMask;
			float _MainSaturationGlobalMaskBlendType;
			float _MainBrightnessGlobalMask;
			float _MainBrightnessGlobalMaskBlendType;
			
			#if defined(PROP_MAINGRADATIONTEX) || !defined(OPTIMIZER_ENABLED)
			Texture2D _MainGradationTex;
			#endif
			float _ColorGradingToggle;
			float _MainGradationStrength;
			//endex
			
			SamplerState sampler_linear_clamp;
			SamplerState sampler_linear_repeat;
			SamplerState sampler_trilinear_repeat;
			
			float _AlphaForceOpaque;
			float _AlphaMod;
			float _AlphaPremultiply;
			float _AlphaBoostFA;
			//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
			float _AlphaToCoverage;
			float _AlphaSharpenedA2C;
			float _AlphaMipScale;
			//endex
			
			//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
			float _AlphaDithering;
			float _AlphaDitherGradient;
			float _AlphaDitherBias;
			//endex
			
			//ifex _AlphaDistanceFade==0 && isNotAnimated(_AlphaDistanceFade)
			float _AlphaDistanceFade;
			float _AlphaDistanceFadeType;
			float _AlphaDistanceFadeMinAlpha;
			float _AlphaDistanceFadeMaxAlpha;
			float _AlphaDistanceFadeMin;
			float _AlphaDistanceFadeMax;
			float _AlphaDistanceFadeGlobalMask;
			float _AlphaDistanceFadeGlobalMaskBlendType;
			//endex
			
			//ifex _AlphaFresnel==0 && isNotAnimated(_AlphaFresnel)
			float _AlphaFresnel;
			float _AlphaFresnelAlpha;
			float _AlphaFresnelSharpness;
			float _AlphaFresnelWidth;
			float _AlphaFresnelInvert;
			float _AlphaFresnelGlobalMask;
			float _AlphaFresnelGlobalMaskBlendType;
			//endex
			
			//ifex _AlphaAngular==0 && isNotAnimated(_AlphaAngular)
			float _AlphaAngular;
			float _AngleType;
			float _AngleCompareTo;
			float3 _AngleForwardDirection;
			float _CameraAngleMin;
			float _CameraAngleMax;
			float _ModelAngleMin;
			float _ModelAngleMax;
			float _AngleMinAlpha;
			float _AlphaAngularGlobalMask;
			float _AlphaAngularGlobalMaskBlendType;
			//endex
			
			//ifex _AlphaAudioLinkEnabled==0 && isNotAnimated(_AlphaAudioLinkEnabled)
			float _AlphaAudioLinkEnabled;
			float2 _AlphaAudioLinkAddRange;
			float _AlphaAudioLinkAddBand;
			//endex
			
			float _AlphaGlobalMask;
			float _AlphaGlobalMaskBlendType;
			
			//ifex _PoiParallax==0
			#ifdef POI_PARALLAX
			
			sampler2D _HeightMap;
			float4 _HeightMap_ST;
			float2 _HeightMapPan;
			float _HeightMapUV;
			
			#if defined(PROP_HEIGHTMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _Heightmask;
			float4 _Heightmask_ST;
			float2 _HeightmaskPan;
			float _HeightmaskUV;
			float _HeightmaskChannel;
			float _HeightmaskInvert;
			SamplerState _linear_repeat;
			#endif
			
			float _ParallaxUV;
			float _HeightStrength;
			float _HeightOffset;
			float _HeightStepsMin;
			float _HeightStepsMax;
			
			float _CurvatureU;
			float _CurvatureV;
			float _CurvFix;
			#endif
			//endex
			
			//ifex _EnableEmission==0
			#ifdef _EMISSION
			#if defined(PROP_EMISSIONMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMap;
			#endif
			float4 _EmissionMap_ST;
			float2 _EmissionMapPan;
			float _EmissionMapUV;
			#if defined(PROP_EMISSIONMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMask;
			#endif
			float4 _EmissionMask_ST;
			float2 _EmissionMaskPan;
			float _EmissionMaskUV;
			float _EmissionMaskInvert;
			float _EmissionMaskChannel;
			float _EmissionMask0GlobalMask;
			float _EmissionMask0GlobalMaskBlendType;
			#if defined(PROP_EMISSIONSCROLLINGCURVE) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionScrollingCurve;
			#endif
			float4 _EmissionScrollingCurve_ST;
			
			float4 _EmissionColor;
			float _EmissionBaseColorAsMap;
			float _EmissionStrength;
			float _EmissionHueShiftEnabled;
			float _EmissionHueShift;
			float _EmissionHueShiftSpeed;
			float _EmissionCenterOutEnabled;
			float _EmissionCenterOutSpeed;
			float _EnableGITDEmission;
			float _GITDEWorldOrMesh;
			float _GITDEMinEmissionMultiplier;
			float _GITDEMaxEmissionMultiplier;
			float _GITDEMinLight;
			float _GITDEMaxLight;
			float _EmissionBlinkingEnabled;
			float _EmissiveBlink_Min;
			float _EmissiveBlink_Max;
			float _EmissiveBlink_Velocity;
			float _EmissionBlinkingOffset;
			float _ScrollingEmission;
			float4 _EmissiveScroll_Direction;
			float _EmissiveScroll_Width;
			float _EmissiveScroll_Velocity;
			float _EmissiveScroll_Interval;
			float _EmissionScrollingOffset;
			
			float _EmissionReplace0;
			float _EmissionScrollingVertexColor;
			float _EmissionScrollingUseCurve;
			float _EmissionColorThemeIndex;
			
			// Audio Link
			float _EmissionAL0Enabled;
			float2 _EmissionAL0StrengthMod;
			float _EmissionAL0StrengthBand;
			float _EmissionAL0StrengthBandSmoothing;
			float2 _AudioLinkEmission0CenterOut;
			float _AudioLinkEmission0CenterOutSize;
			float _AudioLinkEmission0CenterOutBand;
			float _AudioLinkEmission0CenterOutDuration;
			float2 _EmissionAL0Multipliers;
			float _EmissionAL0MultipliersBand;
			#endif
			//endex
			//ifex _EnableEmission1==0
			#ifdef POI_EMISSION_1
			
			#if defined(PROP_EMISSIONMAP1) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMap1;
			#endif
			float4 _EmissionMap1_ST;
			float2 _EmissionMap1Pan;
			float _EmissionMap1UV;
			#if defined(PROP_EMISSIONMASK1) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMask1;
			#endif
			float4 _EmissionMask1_ST;
			float2 _EmissionMask1Pan;
			float _EmissionMask1UV;
			float _EmissionMask1Channel;
			float _EmissionMaskInvert1;
			float _EmissionMask1GlobalMask;
			float _EmissionMask1GlobalMaskBlendType;
			#if defined(PROP_EMISSIONSCROLLINGCURVE1) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionScrollingCurve1;
			#endif
			float4 _EmissionScrollingCurve1_ST;
			
			float4 _EmissionColor1;
			float _EmissionBaseColorAsMap1;
			float _EmissionStrength1;
			float _EnableEmission1;
			float _EmissionHueShift1;
			float _EmissionHueShiftSpeed1;
			float4 _EmissiveScroll_Direction1;
			float _EmissiveScroll_Width1;
			float _EmissiveScroll_Velocity1;
			float _EmissiveScroll_Interval1;
			float _EmissionBlinkingEnabled1;
			float _EmissiveBlink_Min1;
			float _EmissiveBlink_Max1;
			float _EmissiveBlink_Velocity1;
			float _ScrollingEmission1;
			float _EnableGITDEmission1;
			float _GITDEMinEmissionMultiplier1;
			float _GITDEMaxEmissionMultiplier1;
			float _GITDEMinLight1;
			float _GITDEMaxLight1;
			float _GITDEWorldOrMesh1;
			float _EmissionCenterOutEnabled1;
			float _EmissionCenterOutSpeed1;
			float _EmissionHueShiftEnabled1;
			float _EmissionBlinkingOffset1;
			float _EmissionScrollingOffset1;
			float _EmissionScrollingVertexColor1;
			float _EmissionScrollingUseCurve1;
			float _EmissionReplace1;
			float _EmissionColor1ThemeIndex;
			
			// Audio Link
			float _EmissionAL1Enabled;
			float2 _EmissionAL1StrengthMod;
			float _EmissionAL1StrengthBand;
			float2 _AudioLinkEmission1CenterOut;
			float _AudioLinkEmission1CenterOutSize;
			float _AudioLinkEmission1CenterOutBand;
			float _AudioLinkEmission1CenterOutDuration;
			float2 _EmissionAL1Multipliers;
			float _EmissionAL1MultipliersBand;
			#endif
			//endex
			//ifex _EnableEmission2==0
			#ifdef POI_EMISSION_2
			
			#if defined(PROP_EMISSIONMAP2) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMap2;
			#endif
			float4 _EmissionMap2_ST;
			float2 _EmissionMap2Pan;
			float _EmissionMap2UV;
			#if defined(PROP_EMISSIONMASK2) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMask2;
			#endif
			float4 _EmissionMask2_ST;
			float2 _EmissionMask2Pan;
			float _EmissionMask2UV;
			float _EmissionMask2Channel;
			float _EmissionMaskInvert2;
			float _EmissionMask2GlobalMask;
			float _EmissionMask2GlobalMaskBlendType;
			#if defined(PROP_EMISSIONSCROLLINGCURVE2) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionScrollingCurve2;
			#endif
			float4 _EmissionScrollingCurve2_ST;
			
			float4 _EmissionColor2;
			float _EmissionBaseColorAsMap2;
			float _EmissionStrength2;
			float _EnableEmission2;
			float _EmissionHueShift2;
			float _EmissionHueShiftSpeed2;
			float4 _EmissiveScroll_Direction2;
			float _EmissiveScroll_Width2;
			float _EmissiveScroll_Velocity2;
			float _EmissiveScroll_Interval2;
			float _EmissionBlinkingEnabled2;
			float _EmissiveBlink_Min2;
			float _EmissiveBlink_Max2;
			float _EmissiveBlink_Velocity2;
			float _ScrollingEmission2;
			float _EnableGITDEmission2;
			float _GITDEMinEmissionMultiplier2;
			float _GITDEMaxEmissionMultiplier2;
			float _GITDEMinLight2;
			float _GITDEMaxLight2;
			float _GITDEWorldOrMesh2;
			float _EmissionCenterOutEnabled2;
			float _EmissionCenterOutSpeed2;
			float _EmissionHueShiftEnabled2;
			float _EmissionBlinkingOffset2;
			float _EmissionScrollingOffset2;
			float _EmissionScrollingVertexColor2;
			float _EmissionScrollingUseCurve2;
			float _EmissionReplace2;
			float _EmissionColor2ThemeIndex;
			
			// Audio Link
			float _EmissionAL2Enabled;
			float2 _EmissionAL2StrengthMod;
			float _EmissionAL2StrengthBand;
			float2 _AudioLinkEmission2CenterOut;
			float _AudioLinkEmission2CenterOutSize;
			float _AudioLinkEmission2CenterOutBand;
			float _AudioLinkEmission2CenterOutDuration;
			float2 _EmissionAL2Multipliers;
			float _EmissionAL2MultipliersBand;
			#endif
			//endex
			//ifex _EnableEmission3==0
			#ifdef POI_EMISSION_3
			
			#if defined(PROP_EMISSIONMAP3) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMap3;
			#endif
			float4 _EmissionMap3_ST;
			float2 _EmissionMap3Pan;
			float _EmissionMap3UV;
			#if defined(PROP_EMISSIONMASK3) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionMask3;
			#endif
			float4 _EmissionMask3_ST;
			float2 _EmissionMask3Pan;
			float _EmissionMask3UV;
			float _EmissionMask3Channel;
			float _EmissionMaskInvert3;
			float _EmissionMask3GlobalMask;
			float _EmissionMask3GlobalMaskBlendType;
			#if defined(PROP_EMISSIONSCROLLINGCURVE3) || !defined(OPTIMIZER_ENABLED)
			Texture2D _EmissionScrollingCurve3;
			#endif
			float4 _EmissionScrollingCurve3_ST;
			
			float4 _EmissionColor3;
			float _EmissionBaseColorAsMap3;
			float _EmissionStrength3;
			float _EnableEmission3;
			float _EmissionHueShift3;
			float _EmissionHueShiftSpeed3;
			float4 _EmissiveScroll_Direction3;
			float _EmissiveScroll_Width3;
			float _EmissiveScroll_Velocity3;
			float _EmissiveScroll_Interval3;
			float _EmissionBlinkingEnabled3;
			float _EmissiveBlink_Min3;
			float _EmissiveBlink_Max3;
			float _EmissiveBlink_Velocity3;
			float _ScrollingEmission3;
			float _EnableGITDEmission3;
			float _GITDEMinEmissionMultiplier3;
			float _GITDEMaxEmissionMultiplier3;
			float _GITDEMinLight3;
			float _GITDEMaxLight3;
			float _GITDEWorldOrMesh3;
			float _EmissionCenterOutEnabled3;
			float _EmissionCenterOutSpeed3;
			float _EmissionHueShiftEnabled3;
			float _EmissionBlinkingOffset3;
			float _EmissionScrollingOffset3;
			float _EmissionScrollingVertexColor3;
			float _EmissionScrollingUseCurve3;
			float _EmissionReplace3;
			float _EmissionColor3ThemeIndex;
			
			// Audio Link
			float _EmissionAL3Enabled;
			float2 _EmissionAL3StrengthMod;
			float _EmissionAL3StrengthBand;
			float2 _AudioLinkEmission3CenterOut;
			float _AudioLinkEmission3CenterOutSize;
			float _AudioLinkEmission3CenterOutBand;
			float _AudioLinkEmission3CenterOutDuration;
			float2 _EmissionAL3Multipliers;
			float _EmissionAL3MultipliersBand;
			#endif
			//endex
			
			//ifex _GlitterEnable==0
			#ifdef _SUNDISK_SIMPLE
			float4 _GlitterRandomRotationSpeed;
			float _GlitterLayers;
			float _GlitterUseNormals;
			float _GlitterUV;
			half3 _GlitterColor;
			float _GlitterColorThemeIndex;
			float2 _GlitterPan;
			half _GlitterSpeed;
			half _GlitterBrightness;
			float _GlitterFrequency;
			float _GlitterRandomLocation;
			half _GlitterSize;
			half _GlitterContrast;
			half _GlitterAngleRange;
			half _GlitterMinBrightness;
			half _GlitterBias;
			fixed _GlitterUseSurfaceColor;
			float _GlitterBlendType;
			float _GlitterMode;
			float _GlitterShape;
			float _GlitterCenterSize;
			float _GlitterJaggyFix;
			float _GlitterTextureRotation;
			float2 _GlitterUVPanning;
			
			float _GlitterHueShiftEnabled;
			float _GlitterHueShiftSpeed;
			float _GlitterHueShift;
			float _GlitterHideInShadow;
			float _GlitterScaleWithLighting;
			
			float _GlitterRandomColors;
			float2 _GlitterMinMaxSaturation;
			float2 _GlitterMinMaxBrightness;
			float _GlitterRandomSize;
			float4 _GlitterMinMaxSize;
			float _GlitterRandomRotation;
			
			#if defined(PROP_GLITTERMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterMask;
			#endif
			float4 _GlitterMask_ST;
			float2 _GlitterMaskPan;
			float _GlitterMaskUV;
			float _GlitterMaskChannel;
			float _GlitterMaskInvert;
			float _GlitterMaskGlobalMask;
			float _GlitterMaskGlobalMaskBlendType;
			#if defined(PROP_GLITTERCOLORMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterColorMap;
			#endif
			float4 _GlitterColorMap_ST;
			float2 _GlitterColorMapPan;
			float _GlitterColorMapUV;
			#if defined(PROP_GLITTERTEXTURE) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterTexture;
			#endif
			float4 _GlitterTexture_ST;
			float2 _GlitterTexturePan;
			float _GlitterTextureUV;
			#endif
			//endex
			
			//ifex _PoiInternalParallax==0
			#ifdef POI_INTERNALPARALLAX
			#if defined(PROP_PARALLAXINTERNALMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _ParallaxInternalMap;
			#endif
			float4 _ParallaxInternalMap_ST;
			float2 _ParallaxInternalMapPan;
			
			#if defined(PROP_PARALLAXINTERNALMAPMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _ParallaxInternalMapMask;
			#endif
			float4 _ParallaxInternalMapMask_ST;
			float2 _ParallaxInternalMapMaskPan;
			float _ParallaxInternalMapMaskUV;
			float _ParallaxInternalMapMaskChannel;
			
			float _ParallaxInternalIterations;
			float _ParallaxInternalMinDepth;
			float _ParallaxInternalMaxDepth;
			float _ParallaxInternalMinFade;
			float _ParallaxInternalMaxFade;
			float4 _ParallaxInternalMinColor;
			float4 _ParallaxInternalMaxColor;
			float _ParallaxInternalMinColorThemeIndex;
			float _ParallaxInternalMaxColorThemeIndex;
			// float4 _ParallaxInternalPanSpeed;
			float4 _ParallaxInternalPanDepthSpeed;
			float _ParallaxInternalHeightmapMode;
			float _ParallaxInternalHeightFromAlpha;
			
			float _ParallaxInternalHueShiftEnabled;
			float _ParallaxInternalHueShift;
			float _ParallaxInternalHueShiftSpeed;
			float _ParallaxInternalHueShiftPerLevel;
			float _ParallaxInternalSurfaceBlendMode;
			float _ParallaxInternalBlendMode;
			// float _ParallaxInternalHueShiftPerLevelSpeed;
			#endif
			//endex
			
			Texture2D _RF_Map;
			float4 _RF_Map_ST;
			float4 _ReactivePositions[100];
			float4 _ReactiveColors1[100];
			float4 _ReactiveColors2[100];
			float4 _ReactiveSpecial[100];
			float _RF_Min_Distance;
			float _RF_Max_Distance;
			int _RF_ArrayLength;
			float _RF_Ramp_Pan;
			
			float4 _RF_Color0;
			float4 _RF_Color1;
			float4 _RF_Color2;
			float4 _RF_Color3;
			float4 _RF_Color4;
			float4 _RF_Color5;
			float4 _RF_Color6;
			float4 _RF_Color7;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				uint vertexId : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct VertexOut
			{
				float4 pos : SV_POSITION;
				float4 uv[2] : TEXCOORD0;
				float3 normal : TEXCOORD2;
				float4 tangent : TEXCOORD3;
				float4 worldPos : TEXCOORD4;
				float4 localPos : TEXCOORD5;
				float4 vertexColor : TEXCOORD6;
				float4 lightmapUV : TEXCOORD7;
				float2 fogCoord: TEXCOORD10;
				UNITY_SHADOW_COORDS(11)
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			struct PoiMesh
			{
				
				// 0 Vertex normal
				// 1 Fragment normal
				float3 normals[2];
				float3 objNormal;
				float3 tangentSpaceNormal;
				float3 binormal[2];
				float3 tangent[2];
				float3 worldPos;
				float3 localPos;
				float3 objectPosition;
				float isFrontFace;
				float4 vertexColor;
				float4 lightmapUV;
				// 0-3 UV0-UV3
				// 4 Panosphere UV
				// 5 world pos xz
				// 6 Polar UV
				// 7 Distorted UV
				float2 uv[9];
				float2 parallaxUV;
				float2 dx;
				float2 dy;
			};
			
			struct PoiCam
			{
				float3 viewDir;
				float3 forwardDir;
				float3 worldPos;
				float distanceToVert;
				float4 clipPos;
				float4 screenSpacePosition;
				float3 reflectionDir;
				float3 vertexReflectionDir;
				float3 tangentViewDir;
				float4 posScreenSpace;
				float2 posScreenPixels;
				float2 screenUV;
				float vDotN;
				float4 worldDirection;
				
			};
			
			struct PoiMods
			{
				float4 Mask;
				float audioLink[5];
				float audioLinkAvailable;
				float audioLinkVersion;
				float4 audioLinkTexture;
				float audioLinkViaLuma;
				float2 detailMask;
				float2 backFaceDetailIntensity;
				float globalEmission;
				float4 globalColorTheme[12];
				float globalMask[16];
				float ALTime[8];
			};
			
			struct PoiLight
			{
				
				float3 direction;
				float attenuation;
				float attenuationStrength;
				float3 directColor;
				float3 indirectColor;
				float occlusion;
				float shadowMask;
				float detailShadow;
				float3 halfDir;
				float lightMap;
				float lightMapNoAttenuation;
				float3 rampedLightMap;
				float vertexNDotL;
				float nDotL;
				float nDotV;
				float vertexNDotV;
				float nDotH;
				float vertexNDotH;
				float lDotv;
				float lDotH;
				float nDotLSaturated;
				float nDotLNormalized;
				#ifdef POI_PASS_ADD
				float additiveShadow;
				#endif
				float3 finalLighting;
				float3 finalLightAdd;
				float3 LTCGISpecular;
				float3 LTCGIDiffuse;
				float directLuminance;
				float indirectLuminance;
				float finalLuminance;
				
				#if defined(VERTEXLIGHT_ON)
				// Non Important Lights
				float4 vDotNL;
				float4 vertexVDotNL;
				float3 vColor[4];
				float4 vCorrectedDotNL;
				float4 vAttenuation;
				float4 vAttenuationDotNL;
				float3 vPosition[4];
				float3 vDirection[4];
				float3 vFinalLighting;
				float3 vHalfDir[4];
				half4 vDotNH;
				half4 vertexVDotNH;
				half4 vDotLH;
				#endif
				
			};
			
			struct PoiVertexLights
			{
				
				float3 direction;
				float3 color;
				float attenuation;
			};
			
			struct PoiFragData
			{
				float smoothness;
				float smoothness2;
				float metallic;
				float specularMask;
				float reflectionMask;
				
				float3 baseColor;
				float3 finalColor;
				float alpha;
				float3 emission;
				float toggleVertexLights;
			};
			
			float4 poiTransformClipSpacetoScreenSpaceFrag(float4 clipPos)
			{
				float4 positionSS = float4(clipPos.xyz * clipPos.w, clipPos.w);
				positionSS.xy = positionSS.xy / _ScreenParams.xy;
				return positionSS;
			}
			
			// glsl_mod behaves better on negative numbers, and
			// in some situations actually outperforms HLSL's fmod()
			#ifndef glsl_mod
			#define glsl_mod(x, y) (((x) - (y) * floor((x) / (y))))
			#endif
			
			uniform float random_uniform_float_only_used_to_stop_compiler_warnings = 0.0f;
			
			float2 poiUV(float2 uv, float4 tex_st)
			{
				return uv * tex_st.xy + tex_st.zw;
			}
			
			float2 vertexUV(in VertexOut o, int index)
			{
				switch(index)
				{
					case 0:
					return o.uv[0].xy;
					case 1:
					return o.uv[0].zw;
					case 2:
					return o.uv[1].xy;
					case 3:
					return o.uv[1].zw;
					default:
					return o.uv[0].xy;
				}
			}
			
			float2 vertexUV(in appdata v, int index)
			{
				switch(index)
				{
					case 0:
					return v.uv0.xy;
					case 1:
					return v.uv1.xy;
					case 2:
					return v.uv2.xy;
					case 3:
					return v.uv3.xy;
					default:
					return v.uv0.xy;
				}
			}
			
			//Lighting Helpers
			float calculateluminance(float3 color)
			{
				return color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
			}
			
			// Set by VRChat (as of open beta 1245)
			// _VRChatCameraMode: 0 => Normal, 1 => VR HandCam, 2 => Desktop Handcam, 3 => Screenshot/Photo
			// _VRChatMirrorMode: 0 => Normal, 1 => Mirror (VR), 2 => Mirror (Deskie)
			float _VRChatCameraMode;
			float _VRChatMirrorMode;
			
			float VRCCameraMode()
			{
				return _VRChatCameraMode;
			}
			
			float VRCMirrorMode()
			{
				return _VRChatMirrorMode;
			}
			
			bool IsInMirror()
			{
				return unity_CameraProjection[2][0] != 0.f || unity_CameraProjection[2][1] != 0.f;
			}
			
			bool IsOrthographicCamera()
			{
				return unity_OrthoParams.w == 1 || UNITY_MATRIX_P[3][3] == 1;
			}
			
			float shEvaluateDiffuseL1Geomerics_local(float L0, float3 L1, float3 n)
			{
				// average energy
				float R0 = max(0, L0);
				
				// avg direction of incoming light
				float3 R1 = 0.5f * L1;
				
				// directional brightness
				float lenR1 = length(R1);
				
				// linear angle between normal and direction 0-1
				//float q = 0.5f * (1.0f + dot(R1 / lenR1, n));
				//float q = dot(R1 / lenR1, n) * 0.5 + 0.5;
				float q = dot(normalize(R1), n) * 0.5 + 0.5;
				q = saturate(q); // Thanks to ScruffyRuffles for the bug identity.
				
				// power for q
				// lerps from 1 (linear) to 3 (cubic) based on directionality
				float p = 1.0f + 2.0f * lenR1 / R0;
				
				// dynamic range constant
				// should vary between 4 (highly directional) and 0 (ambient)
				float a = (1.0f - lenR1 / R0) / (1.0f + lenR1 / R0);
				
				return R0 * (a + (1.0f - a) * (p + 1.0f) * pow(q, p));
			}
			
			half3 BetterSH9(half4 normal)
			{
				float3 indirect;
				float3 L0 = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w) + float3(unity_SHBr.z, unity_SHBg.z, unity_SHBb.z) / 3.0;
				indirect.r = shEvaluateDiffuseL1Geomerics_local(L0.r, unity_SHAr.xyz, normal.xyz);
				indirect.g = shEvaluateDiffuseL1Geomerics_local(L0.g, unity_SHAg.xyz, normal.xyz);
				indirect.b = shEvaluateDiffuseL1Geomerics_local(L0.b, unity_SHAb.xyz, normal.xyz);
				indirect = max(0, indirect);
				indirect += SHEvalLinearL2(normal);
				return indirect;
			}
			
			// Silent's code ends here
			
			float3 getCameraForward()
			{
				#if UNITY_SINGLE_PASS_STEREO
				float3 p1 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 1, 1));
				float3 p2 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 0, 1));
				#else
				float3 p1 = mul(unity_CameraToWorld, float4(0, 0, 1, 1)).xyz;
				float3 p2 = mul(unity_CameraToWorld, float4(0, 0, 0, 1)).xyz;
				#endif
				return normalize(p2 - p1);
			}
			
			half3 GetSHLength()
			{
				half3 x, x1;
				x.r = length(unity_SHAr);
				x.g = length(unity_SHAg);
				x.b = length(unity_SHAb);
				x1.r = length(unity_SHBr);
				x1.g = length(unity_SHBg);
				x1.b = length(unity_SHBb);
				return x + x1;
			}
			
			float3 BoxProjection(float3 direction, float3 position, float4 cubemapPosition, float3 boxMin, float3 boxMax)
			{
				#if UNITY_SPECCUBE_BOX_PROJECTION
				//UNITY_BRANCH
				if (cubemapPosition.w > 0)
				{
					float3 factors = ((direction > 0 ? boxMax : boxMin) - position) / direction;
					float scalar = min(min(factors.x, factors.y), factors.z);
					direction = direction * scalar + (position - cubemapPosition.xyz);
				}
				#endif
				return direction;
			}
			
			float poiMax(float2 i)
			{
				return max(i.x, i.y);
			}
			
			float poiMax(float3 i)
			{
				return max(max(i.x, i.y), i.z);
			}
			
			float poiMax(float4 i)
			{
				return max(max(max(i.x, i.y), i.z), i.w);
			}
			
			float3 calculateNormal(in float3 baseNormal, in PoiMesh poiMesh, in Texture2D normalTexture, in float4 normal_ST, in float2 normalPan, in float normalUV, in float normalIntensity)
			{
				float3 normal = UnpackScaleNormal(POI2D_SAMPLER_PAN(normalTexture, _MainTex, poiUV(poiMesh.uv[normalUV], normal_ST), normalPan), normalIntensity);
				return normalize(
				normal.x * poiMesh.tangent[0] +
				normal.y * poiMesh.binormal[0] +
				normal.z * baseNormal
				);
			}
			
			float remap(float x, float minOld, float maxOld, float minNew = 0, float maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float2 remap(float2 x, float2 minOld, float2 maxOld, float2 minNew = 0, float2 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float3 remap(float3 x, float3 minOld, float3 maxOld, float3 minNew = 0, float3 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float4 remap(float4 x, float4 minOld, float4 maxOld, float4 minNew = 0, float4 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float remapClamped(float minOld, float maxOld, float x, float minNew = 0, float maxNew = 1)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float2 remapClamped(float2 minOld, float2 maxOld, float2 x, float2 minNew, float2 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float3 remapClamped(float3 minOld, float3 maxOld, float3 x, float3 minNew, float3 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float4 remapClamped(float4 minOld, float4 maxOld, float4 x, float4 minNew, float4 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			float2 calcParallax(in float height, in PoiCam poiCam)
			{
				return ((height * - 1) + 1) * (poiCam.tangentViewDir.xy / poiCam.tangentViewDir.z);
			}
			
			/*
			0: Zero	                float4(0.0, 0.0, 0.0, 0.0),
			1: One	                float4(1.0, 1.0, 1.0, 1.0),
			2: DstColor	            destinationColor,
			3: SrcColor	            sourceColor,
			4: OneMinusDstColor	    float4(1.0, 1.0, 1.0, 1.0) - destinationColor,
			5: SrcAlpha	            sourceColor.aaaa,
			6: OneMinusSrcColor	    float4(1.0, 1.0, 1.0, 1.0) - sourceColor,
			7: DstAlpha	            destinationColor.aaaa,
			8: OneMinusDstAlpha	    float4(1.0, 1.0, 1.0, 1.0) - destinationColor.,
			9: SrcAlphaSaturate     saturate(sourceColor.aaaa),
			10: OneMinusSrcAlpha	float4(1.0, 1.0, 1.0, 1.0) - sourceColor.aaaa,
			*/
			
			float4 poiBlend(const float sourceFactor, const  float4 sourceColor, const  float destinationFactor, const  float4 destinationColor, const float4 blendFactor)
			{
				float4 sA = 1 - blendFactor;
				const float4 blendData[11] = {
					float4(0.0, 0.0, 0.0, 0.0),
					float4(1.0, 1.0, 1.0, 1.0),
					destinationColor,
					sourceColor,
					float4(1.0, 1.0, 1.0, 1.0) - destinationColor,
					sA,
					float4(1.0, 1.0, 1.0, 1.0) - sourceColor,
					sA,
					float4(1.0, 1.0, 1.0, 1.0) - sA,
					saturate(sourceColor.aaaa),
					1 - sA,
				};
				
				return lerp(blendData[sourceFactor] * sourceColor + blendData[destinationFactor] * destinationColor, sourceColor, sA);
			}
			
			// Average
			float blendAverage(float base, float blend)
			{
				return (base + blend) / 2.0;
			}
			float3 blendAverage(float3 base, float3 blend)
			{
				return (base + blend) / 2.0;
			}
			
			// Color burn
			float blendColorBurn(float base, float blend)
			{
				return (blend == 0.0) ? blend : max((1.0 - ((1.0 - base) * rcp(random_uniform_float_only_used_to_stop_compiler_warnings + blend))), 0.0);
			}
			
			float3 blendColorBurn(float3 base, float3 blend)
			{
				return float3(blendColorBurn(base.r, blend.r), blendColorBurn(base.g, blend.g), blendColorBurn(base.b, blend.b));
			}
			
			// Color Dodge
			float blendColorDodge(float base, float blend)
			{
				return (blend == 1.0) ? blend : min(base / (1.0 - blend), 1.0);
			}
			
			float3 blendColorDodge(float3 base, float3 blend)
			{
				return float3(blendColorDodge(base.r, blend.r), blendColorDodge(base.g, blend.g), blendColorDodge(base.b, blend.b));
			}
			
			// Darken
			float blendDarken(float base, float blend)
			{
				return min(blend, base);
			}
			
			float3 blendDarken(float3 base, float3 blend)
			{
				return float3(blendDarken(base.r, blend.r), blendDarken(base.g, blend.g), blendDarken(base.b, blend.b));
			}
			
			// Exclusion
			float blendExclusion(float base, float blend)
			{
				return base + blend - 2.0 * base * blend;
			}
			float3 blendExclusion(float3 base, float3 blend)
			{
				return base + blend - 2.0 * base * blend;
			}
			
			// Reflect
			float blendReflect(float base, float blend)
			{
				return (blend == 1.0) ? blend : min(base * base / (1.0 - blend), 1.0);
			}
			
			float3 blendReflect(float3 base, float3 blend)
			{
				return float3(blendReflect(base.r, blend.r), blendReflect(base.g, blend.g), blendReflect(base.b, blend.b));
			}
			
			// Glow
			float blendGlow(float base, float blend)
			{
				return blendReflect(blend, base);
			}
			float3 blendGlow(float3 base, float3 blend)
			{
				return blendReflect(blend, base);
			}
			
			// Overlay
			float blendOverlay(float base, float blend)
			{
				return base < 0.5 ? (2.0 * base * blend) : (1.0 - 2.0 * (1.0 - base) * (1.0 - blend));
			}
			
			float3 blendOverlay(float3 base, float3 blend)
			{
				return float3(blendOverlay(base.r, blend.r), blendOverlay(base.g, blend.g), blendOverlay(base.b, blend.b));
			}
			
			// Hard Light
			float blendHardLight(float base, float blend)
			{
				return blendOverlay(blend, base);
			}
			float3 blendHardLight(float3 base, float3 blend)
			{
				return blendOverlay(blend, base);
			}
			
			// Vivid light
			float blendVividLight(float base, float blend)
			{
				return (blend < 0.5) ? blendColorBurn(base, (2.0 * blend)) : blendColorDodge(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendVividLight(float3 base, float3 blend)
			{
				return float3(blendVividLight(base.r, blend.r), blendVividLight(base.g, blend.g), blendVividLight(base.b, blend.b));
			}
			
			// Hard mix
			float blendHardMix(float base, float blend)
			{
				return (blendVividLight(base, blend) < 0.5) ? 0.0 : 1.0;
			}
			
			float3 blendHardMix(float3 base, float3 blend)
			{
				return float3(blendHardMix(base.r, blend.r), blendHardMix(base.g, blend.g), blendHardMix(base.b, blend.b));
			}
			
			// Lighten
			float blendLighten(float base, float blend)
			{
				return max(blend, base);
			}
			
			float3 blendLighten(float3 base, float3 blend)
			{
				return float3(blendLighten(base.r, blend.r), blendLighten(base.g, blend.g), blendLighten(base.b, blend.b));
			}
			
			// Linear Burn
			float blendLinearBurn(float base, float blend)
			{
				// Note : Same implementation as BlendSubtractf
				return max(base + blend - 1.0, 0.0);
			}
			
			float3 blendLinearBurn(float3 base, float3 blend)
			{
				// Note : Same implementation as BlendSubtract
				return max(base + blend - float3(1.0, 1.0, 1.0), float3(0.0, 0.0, 0.0));
			}
			
			// Linear Dodge
			float blendLinearDodge(float base, float blend)
			{
				// Note : Same implementation as BlendAddf
				return min(base + blend, 1.0);
			}
			
			float3 blendLinearDodge(float3 base, float3 blend)
			{
				// Note : Same implementation as BlendAdd
				return base + blend;
			}
			
			// Linear light
			float blendLinearLight(float base, float blend)
			{
				return blend < 0.5 ? blendLinearBurn(base, (2.0 * blend)) : blendLinearDodge(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendLinearLight(float3 base, float3 blend)
			{
				return float3(blendLinearLight(base.r, blend.r), blendLinearLight(base.g, blend.g), blendLinearLight(base.b, blend.b));
			}
			
			// Multiply
			float blendMultiply(float base, float blend)
			{
				return base * blend;
			}
			float3 blendMultiply(float3 base, float3 blend)
			{
				return base * blend;
			}
			
			// Negation
			float blendNegation(float base, float blend)
			{
				return 1.0 - abs(1.0 - base - blend);
			}
			float3 blendNegation(float3 base, float3 blend)
			{
				return float3(1.0, 1.0, 1.0) - abs(float3(1.0, 1.0, 1.0) - base - blend);
			}
			
			// Normal
			float blendNormal(float base, float blend)
			{
				return blend;
			}
			float3 blendNormal(float3 base, float3 blend)
			{
				return blend;
			}
			
			// Phoenix
			float blendPhoenix(float base, float blend)
			{
				return min(base, blend) - max(base, blend) + 1.0;
			}
			float3 blendPhoenix(float3 base, float3 blend)
			{
				return min(base, blend) - max(base, blend) + float3(1.0, 1.0, 1.0);
			}
			
			// Pin light
			float blendPinLight(float base, float blend)
			{
				return (blend < 0.5) ? blendDarken(base, (2.0 * blend)) : blendLighten(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendPinLight(float3 base, float3 blend)
			{
				return float3(blendPinLight(base.r, blend.r), blendPinLight(base.g, blend.g), blendPinLight(base.b, blend.b));
			}
			
			// Screen
			float blendScreen(float base, float blend)
			{
				return 1.0 - ((1.0 - base) * (1.0 - blend));
			}
			
			float3 blendScreen(float3 base, float3 blend)
			{
				return float3(blendScreen(base.r, blend.r), blendScreen(base.g, blend.g), blendScreen(base.b, blend.b));
			}
			
			// Soft Light
			float blendSoftLight(float base, float blend)
			{
				return (blend < 0.5) ? (2.0 * base * blend + base * base * (1.0 - 2.0 * blend)) : (sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend));
			}
			
			float3 blendSoftLight(float3 base, float3 blend)
			{
				return float3(blendSoftLight(base.r, blend.r), blendSoftLight(base.g, blend.g), blendSoftLight(base.b, blend.b));
			}
			
			// Subtract
			float blendSubtract(float base, float blend)
			{
				return max(base - blend, 0.0);
			}
			
			float3 blendSubtract(float3 base, float3 blend)
			{
				return max(base - blend, 0.0);
			}
			
			// Difference
			float blendDifference(float base, float blend)
			{
				return abs(base - blend);
			}
			
			float3 blendDifference(float3 base, float3 blend)
			{
				return abs(base - blend);
			}
			
			// Divide
			float blendDivide(float base, float blend)
			{
				return base / max(blend, 0.0001);
			}
			
			float3 blendDivide(float3 base, float3 blend)
			{
				return base / max(blend, 0.0001);
			}
			
			float blendMixed(float base, float blend)
			{
				return base + base * blend;
			}
			
			float3 blendMixed(float3 base, float3 blend)
			{
				return base + base * blend;
			}
			
			float3 customBlend(float3 base, float3 blend, float blendType, float alpha = 1)
			{
				float3 output = base;
				switch(blendType)
				{
					case 0: output = lerp(base, blend, alpha); break;
					case 2: output = base * lerp(1, blend, alpha); break;
					case 6: output = lerp(base, blendScreen(base, blend), alpha); break;
					case 7: output = blendSubtract(base, blend * alpha); break;
					case 8: output = lerp(base, blendLinearDodge(base, blend), alpha); break;
					case 9: output = lerp(base, blendOverlay(base, blend), alpha); break;
					case 20: output = lerp(base, blendMixed(base, blend), alpha); break;
					default: output = 0; break;
				}
				return output;
			}
			
			float3 customBlend(float base, float blend, float blendType, float alpha = 1)
			{
				float3 output = base;
				switch(blendType)
				{
					case 0: output = lerp(base, blend, alpha); break;
					case 2: output = base * lerp(1, blend, alpha); break;
					case 6: output = lerp(base, blendScreen(base, blend), alpha); break;
					case 7: output = blendSubtract(base, blend * alpha); break;
					case 8: output = lerp(base, blendLinearDodge(base, blend), alpha); break;
					case 9: output = lerp(base, blendOverlay(base, blend), alpha); break;
					case 20: output = lerp(base, blendMixed(base, blend), alpha); break;
					default: output = 0; break;
				}
				return output;
			}
			
			#define REPLACE 0
			#define SUBSTRACT 1
			#define MULTIPLY 2
			#define DIVIDE 3
			#define MIN 4
			#define MAX 5
			#define AVERAGE 6
			#define ADD 7
			
			float maskBlend(float baseMask, float blendMask, float blendType)
			{
				float output = 0;
				switch(blendType)
				{
					case REPLACE: output = blendMask; break;
					case SUBSTRACT: output = baseMask - blendMask; break;
					case MULTIPLY: output = baseMask * blendMask; break;
					case DIVIDE: output = baseMask / blendMask; break;
					case MIN: output = min(baseMask, blendMask); break;
					case MAX: output = max(baseMask, blendMask); break;
					case AVERAGE: output = (baseMask + blendMask) * 0.5; break;
					case ADD: output = baseMask + blendMask; break;
				}
				return saturate(output);
			}
			
			float globalMaskBlend(float baseMask, float globalMaskIndex, float blendType, PoiMods poiMods)
			{
				if (globalMaskIndex == 0)
				{
					return baseMask;
				}
				else
				{
					return maskBlend(baseMask, poiMods.globalMask[globalMaskIndex - 1], blendType);
				}
			}
			
			float random(float2 p)
			{
				return frac(sin(dot(p, float2(12.9898, 78.2383))) * 43758.5453123);
			}
			
			float2 random2(float2 p)
			{
				return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
			}
			
			float3 random3(float2 p)
			{
				return frac(sin(float3(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)), dot(p, float2(248.3, 315.9)))) * 43758.5453);
			}
			
			float3 random3(float3 p)
			{
				return frac(sin(float3(dot(p, float3(127.1, 311.7, 248.6)), dot(p, float3(269.5, 183.3, 423.3)), dot(p, float3(248.3, 315.9, 184.2)))) * 43758.5453);
			}
			
			float3 randomFloat3(float2 Seed, float maximum)
			{
				return (.5 + float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed), float2(12.9898, 78.233))) * 43758.5453)
				) * .5) * (maximum);
			}
			
			float3 randomFloat3Range(float2 Seed, float Range)
			{
				return (float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
				) * 2 - 1) * Range;
			}
			
			float3 randomFloat3WiggleRange(float2 Seed, float Range, float wiggleSpeed)
			{
				float3 rando = (float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
				) * 2 - 1);
				float speed = 1 + wiggleSpeed;
				return float3(sin((_Time.x + rando.x * PI) * speed), sin((_Time.x + rando.y * PI) * speed), sin((_Time.x + rando.z * PI) * speed)) * Range;
			}
			
			void poiDither(float4 In, float4 ScreenPosition, out float4 Out)
			{
				float2 uv = ScreenPosition.xy * _ScreenParams.xy;
				float DITHER_THRESHOLDS[16] = {
					1.0 / 17.0, 9.0 / 17.0, 3.0 / 17.0, 11.0 / 17.0,
					13.0 / 17.0, 5.0 / 17.0, 15.0 / 17.0, 7.0 / 17.0,
					4.0 / 17.0, 12.0 / 17.0, 2.0 / 17.0, 10.0 / 17.0,
					16.0 / 17.0, 8.0 / 17.0, 14.0 / 17.0, 6.0 / 17.0
				};
				uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
				Out = In - DITHER_THRESHOLDS[index];
			}
			
			static const float Epsilon = 1e-10;
			// The weights of RGB contributions to luminance.
			// Should sum to unity.
			static const float3 HCYwts = float3(0.299, 0.587, 0.114);
			static const float HCLgamma = 3;
			static const float HCLy0 = 100;
			static const float HCLmaxL = 0.530454533953517; // == exp(HCLgamma / HCLy0) - 0.5
			static const float3 wref = float3(1.0, 1.0, 1.0);
			#define TAU 6.28318531
			
			float3 HUEtoRGB(in float H)
			{
				float R = abs(H * 6 - 3) - 1;
				float G = 2 - abs(H * 6 - 2);
				float B = 2 - abs(H * 6 - 4);
				return saturate(float3(R, G, B));
			}
			
			float3 RGBtoHCV(in float3 RGB)
			{
				// Based on work by Sam Hocevar and Emil Persson
				float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
				float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
				float C = Q.x - min(Q.w, Q.y);
				float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
				return float3(H, C, Q.x);
			}
			
			float3 HSVtoRGB(in float3 HSV)
			{
				float3 RGB = HUEtoRGB(HSV.x);
				return ((RGB - 1) * HSV.y + 1) * HSV.z;
			}
			
			float3 RGBtoHSV(in float3 RGB)
			{
				float3 HCV = RGBtoHCV(RGB);
				float S = HCV.y / (HCV.z + Epsilon);
				return float3(HCV.x, S, HCV.z);
			}
			
			float3 HSLtoRGB(in float3 HSL)
			{
				float3 RGB = HUEtoRGB(HSL.x);
				float C = (1 - abs(2 * HSL.z - 1)) * HSL.y;
				return (RGB - 0.5) * C + HSL.z;
			}
			
			float3 RGBtoHSL(in float3 RGB)
			{
				float3 HCV = RGBtoHCV(RGB);
				float L = HCV.z - HCV.y * 0.5;
				float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
				return float3(HCV.x, S, L);
			}
			
			void DecomposeHDRColor(in float3 linearColorHDR, out float3 baseLinearColor, out float exposure)
			{
				// Optimization/adaptation of https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/GUI/ColorMutator.cs#L23 but skips weird photoshop stuff
				float maxColorComponent = max(linearColorHDR.r, max(linearColorHDR.g, linearColorHDR.b));
				bool isSDR = maxColorComponent <= 1.0;
				
				float scaleFactor = isSDR ? 1.0 : (1.0 / maxColorComponent);
				exposure = isSDR ? 0.0 : log(maxColorComponent) * 1.44269504089; // ln(2)
				
				baseLinearColor = scaleFactor * linearColorHDR;
			}
			
			float3 ApplyHDRExposure(float3 linearColor, float exposure)
			{
				return linearColor * pow(2, exposure);
			}
			
			// Transforms an RGB color using a matrix. Note that S and V are absolute values here
			float3 ModifyViaHSV(float3 color, float h, float s, float v)
			{
				float3 colorHSV = RGBtoHSV(color);
				colorHSV.x = frac(colorHSV.x + h);
				colorHSV.y = saturate(colorHSV.y + s);
				colorHSV.z = saturate(colorHSV.z + v);
				return HSVtoRGB(colorHSV);
			}
			
			float3 ModifyViaHSV(float3 color, float3 HSVMod)
			{
				return ModifyViaHSV(color, HSVMod.x, HSVMod.y, HSVMod.z);
			}
			
			float4x4 brightnessMatrix(float brightness)
			{
				return float4x4(
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				brightness, brightness, brightness, 1
				);
			}
			
			float4x4 contrastMatrix(float contrast)
			{
				float t = (1.0 - contrast) / 2.0;
				
				return float4x4(
				contrast, 0, 0, 0,
				0, contrast, 0, 0,
				0, 0, contrast, 0,
				t, t, t, 1
				);
			}
			
			float4x4 saturationMatrix(float saturation)
			{
				float3 luminance = float3(0.3086, 0.6094, 0.0820);
				
				float oneMinusSat = 1.0 - saturation;
				
				float3 red = luminance.x * oneMinusSat;
				red += float3(saturation, 0, 0);
				
				float3 green = luminance.y * oneMinusSat;
				green += float3(0, saturation, 0);
				
				float3 blue = luminance.z * oneMinusSat;
				blue += float3(0, 0, saturation);
				
				return float4x4(
				red, 0,
				green, 0,
				blue, 0,
				0, 0, 0, 1
				);
			}
			
			float4 PoiColorBCS(float4 color, float brightness, float contrast, float saturation)
			{
				return mul(color, mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation))));
			}
			float3 PoiColorBCS(float3 color, float brightness, float contrast, float saturation)
			{
				return mul(float4(color, 1), mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation)))).rgb;
			}
			
			float3 linear_srgb_to_oklab(float3 c)
			{
				float l = 0.4122214708 * c.x + 0.5363325363 * c.y + 0.0514459929 * c.z;
				float m = 0.2119034982 * c.x + 0.6806995451 * c.y + 0.1073969566 * c.z;
				float s = 0.0883024619 * c.x + 0.2817188376 * c.y + 0.6299787005 * c.z;
				
				float l_ = pow(l, 1.0 / 3.0);
				float m_ = pow(m, 1.0 / 3.0);
				float s_ = pow(s, 1.0 / 3.0);
				
				return float3(
				0.2104542553 * l_ + 0.7936177850 * m_ - 0.0040720468 * s_,
				1.9779984951 * l_ - 2.4285922050 * m_ + 0.4505937099 * s_,
				0.0259040371 * l_ + 0.7827717662 * m_ - 0.8086757660 * s_
				);
			}
			
			float3 oklab_to_linear_srgb(float3 c)
			{
				float l_ = c.x + 0.3963377774 * c.y + 0.2158037573 * c.z;
				float m_ = c.x - 0.1055613458 * c.y - 0.0638541728 * c.z;
				float s_ = c.x - 0.0894841775 * c.y - 1.2914855480 * c.z;
				
				float l = l_ * l_ * l_;
				float m = m_ * m_ * m_;
				float s = s_ * s_ * s_;
				
				return float3(
				+ 4.0767416621 * l - 3.3077115913 * m + 0.2309699292 * s,
				- 1.2684380046 * l + 2.6097574011 * m - 0.3413193965 * s,
				- 0.0041960863 * l - 0.7034186147 * m + 1.7076147010 * s
				);
			}
			
			float3 hueShift(float3 color, float shift)
			{
				float3 oklab = linear_srgb_to_oklab(max(color, 0.0000000001));
				float hue = atan2(oklab.z, oklab.y);
				hue += shift * PI * 2;  // Add the hue shift
				
				float chroma = length(oklab.yz);
				oklab.y = cos(hue) * chroma;
				oklab.z = sin(hue) * chroma;
				
				return oklab_to_linear_srgb(oklab);
			}
			
			float3 hueShift(float4 color, float shift)
			{
				return hueShift(color.rgb, shift);
			}
			
			/*
			float3 hueShift(float3 color, float hueOffset)
			{
				color = RGBtoHSV(color);
				color.x = frac(hueOffset +color.x);
				return HSVtoRGB(color);
			}
			*/
			
			// LCH
			float xyzF(float t)
			{
				return lerp(pow(t, 1. / 3.), 7.787037 * t + 0.139731, step(t, 0.00885645));
			}
			float xyzR(float t)
			{
				return lerp(t * t * t, 0.1284185 * (t - 0.139731), step(t, 0.20689655));
			}
			
			float4x4 poiRotationMatrixFromAngles(float x, float y, float z)
			{
				float angleX = radians(x);
				float c = cos(angleX);
				float s = sin(angleX);
				float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
				0, c, -s, 0,
				0, s, c, 0,
				0, 0, 0, 1);
				
				float angleY = radians(y);
				c = cos(angleY);
				s = sin(angleY);
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
				0, 1, 0, 0,
				- s, 0, c, 0,
				0, 0, 0, 1);
				
				float angleZ = radians(z);
				c = cos(angleZ);
				s = sin(angleZ);
				float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
				s, c, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
				
				return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
			}
			
			float4x4 poiRotationMatrixFromAngles(float3 angles)
			{
				float angleX = radians(angles.x);
				float c = cos(angleX);
				float s = sin(angleX);
				float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
				0, c, -s, 0,
				0, s, c, 0,
				0, 0, 0, 1);
				
				float angleY = radians(angles.y);
				c = cos(angleY);
				s = sin(angleY);
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
				0, 1, 0, 0,
				- s, 0, c, 0,
				0, 0, 0, 1);
				
				float angleZ = radians(angles.z);
				c = cos(angleZ);
				s = sin(angleZ);
				float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
				s, c, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
				
				return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
			}
			
			float3 getCameraPosition()
			{
				#ifdef USING_STEREO_MATRICES
				return lerp(unity_StereoWorldSpaceCameraPos[0], unity_StereoWorldSpaceCameraPos[1], 0.5);
				#endif
				return _WorldSpaceCameraPos;
			}
			
			float2 calcPixelScreenUVs(half4 grabPos)
			{
				half2 uv = grabPos.xy / (grabPos.w + 0.0000000001);
				#if UNITY_SINGLE_PASS_STEREO
				uv.xy *= half2(_ScreenParams.x * 2, _ScreenParams.y);
				#else
				uv.xy *= _ScreenParams.xy;
				#endif
				
				return uv;
			}
			
			float CalcMipLevel(float2 texture_coord)
			{
				float2 dx = ddx(texture_coord);
				float2 dy = ddy(texture_coord);
				float delta_max_sqr = max(dot(dx, dx), dot(dy, dy));
				
				return 0.5 * log2(delta_max_sqr);
			}
			
			float inverseLerp(float A, float B, float T)
			{
				return (T - A) / (B - A);
			}
			
			float inverseLerp2(float2 a, float2 b, float2 value)
			{
				float2 AB = b - a;
				float2 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			float inverseLerp3(float3 a, float3 b, float3 value)
			{
				float3 AB = b - a;
				float3 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			float inverseLerp4(float4 a, float4 b, float4 value)
			{
				float4 AB = b - a;
				float4 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			/*
			MIT License
			
			Copyright (c) 2019 wraikny
			
			Permission is hereby granted, free of charge, to any person obtaining a copy
			of this software and associated documentation files (the "Software"), to deal
			in the Software without restriction, including without limitation the rights
			to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
			copies of the Software, and to permit persons to whom the Software is
			furnished to do so, subject to the following conditions:
			
			The above copyright notice and this permission notice shall be included in all
			copies or substantial portions of the Software.
			
			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
			IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
			FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
			AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
			LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
			OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
			SOFTWARE.
			
			VertexTransformShader is dependent on:
			*/
			
			float4 quaternion_conjugate(float4 v)
			{
				return float4(
				v.x, -v.yzw
				);
			}
			
			float4 quaternion_mul(float4 v1, float4 v2)
			{
				float4 result1 = (v1.x * v2 + v1 * v2.x);
				
				float4 result2 = float4(
				- dot(v1.yzw, v2.yzw),
				cross(v1.yzw, v2.yzw)
				);
				
				return float4(result1 + result2);
			}
			
			// angle : radians
			float4 get_quaternion_from_angle(float3 axis, float angle)
			{
				float sn = sin(angle * 0.5);
				float cs = cos(angle * 0.5);
				return float4(axis * sn, cs);
			}
			
			float4 quaternion_from_vector(float3 inVec)
			{
				return float4(0.0, inVec);
			}
			
			float degree_to_radius(float degree)
			{
				return (
				degree / 180.0 * PI
				);
			}
			
			float3 rotate_with_quaternion(float3 inVec, float3 rotation)
			{
				float4 qx = get_quaternion_from_angle(float3(1, 0, 0), radians(rotation.x));
				float4 qy = get_quaternion_from_angle(float3(0, 1, 0), radians(rotation.y));
				float4 qz = get_quaternion_from_angle(float3(0, 0, 1), radians(rotation.z));
				
				#define MUL3(A, B, C) quaternion_mul(quaternion_mul((A), (B)), (C))
				float4 quaternion = normalize(MUL3(qx, qy, qz));
				float4 conjugate = quaternion_conjugate(quaternion);
				
				float4 inVecQ = quaternion_from_vector(inVec);
				
				float3 rotated = (
				MUL3(quaternion, inVecQ, conjugate)
				).yzw;
				
				return rotated;
			}
			
			float4 transform(float4 input, float4 pos, float4 rotation, float4 scale)
			{
				input.rgb *= (scale.xyz * scale.w);
				input = float4(rotate_with_quaternion(input.xyz, rotation.xyz * rotation.w) + (pos.xyz * pos.w), input.w);
				return input;
			}
			
			float2 RotateUV(float2 _uv, float _radian, float2 _piv, float _time)
			{
				float RotateUV_ang = _radian;
				float RotateUV_cos = cos(_time * RotateUV_ang);
				float RotateUV_sin = sin(_time * RotateUV_ang);
				return (mul(_uv - _piv, float2x2(RotateUV_cos, -RotateUV_sin, RotateUV_sin, RotateUV_cos)) + _piv);
			}
			
			/*
			MIT END
			*/
			
			float3 RotateAroundAxis(float3 original, float3 axis, float radian)
			{
				float s = sin(radian);
				float c = cos(radian);
				float one_minus_c = 1.0 - c;
				
				axis = normalize(axis);
				float3x3 rot_mat = {
					one_minus_c * axis.x * axis.x + c, one_minus_c * axis.x * axis.y - axis.z * s, one_minus_c * axis.z * axis.x + axis.y * s,
					one_minus_c * axis.x * axis.y + axis.z * s, one_minus_c * axis.y * axis.y + c, one_minus_c * axis.y * axis.z - axis.x * s,
					one_minus_c * axis.z * axis.x - axis.y * s, one_minus_c * axis.y * axis.z + axis.x * s, one_minus_c * axis.z * axis.z + c
				};
				return mul(rot_mat, original);
			}
			
			float3 poiThemeColor(in PoiMods poiMods, in float3 srcColor, in float themeIndex)
			{
				if (themeIndex == 0) return srcColor;
				themeIndex -= 1;
				
				if (themeIndex <= 3)
				{
					return poiMods.globalColorTheme[themeIndex];
				}
				
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable)
				{
					return poiMods.globalColorTheme[themeIndex];
				}
				#endif
				
				return srcColor;
			}
			
			float3 lilToneCorrection(float3 c, float4 hsvg)
			{
				// gamma
				c = pow(abs(c), hsvg.w);
				// rgb -> hsv
				float4 p = (c.b > c.g) ? float4(c.bg, -1.0, 2.0 / 3.0) : float4(c.gb, 0.0, -1.0 / 3.0);
				float4 q = (p.x > c.r) ? float4(p.xyw, c.r) : float4(c.r, p.yzx);
				float d = q.x - min(q.w, q.y);
				float e = 1.0e-10;
				float3 hsv = float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
				// shift
				hsv = float3(hsv.x + hsvg.x, saturate(hsv.y * hsvg.y), saturate(hsv.z * hsvg.z));
				// hsv -> rgb
				return hsv.z - hsv.z * hsv.y + hsv.z * hsv.y * saturate(abs(frac(hsv.x + float3(1.0, 2.0 / 3.0, 1.0 / 3.0)) * 6.0 - 3.0) - 1.0);
			}
			
			float3 lilBlendColor(float3 dstCol, float3 srcCol, float3 srcA, int blendMode)
			{
				float3 ad = dstCol + srcCol;
				float3 mu = dstCol * srcCol;
				float3 outCol;
				if (blendMode == 0) outCol = srcCol;               // Normal
				if (blendMode == 1) outCol = ad;                   // Add
				if (blendMode == 2) outCol = max(ad - mu, dstCol); // Screen
				if (blendMode == 3) outCol = mu;                   // Multiply
				return lerp(dstCol, outCol, srcA);
			}
			
			float lilIsIn0to1(float f)
			{
				float value = 0.5 - abs(f - 0.5);
				return saturate(value / clamp(fwidth(value), 0.0001, 1.0));
			}
			
			float lilIsIn0to1(float f, float nv)
			{
				float value = 0.5 - abs(f - 0.5);
				return saturate(value / clamp(fwidth(value), 0.0001, nv));
			}
			
			float poiEdgeLinearNoSaturate(float value, float border)
			{
				return (value - border) / clamp(fwidth(value), 0.0001, 1.0);
			}
			
			float3 poiEdgeLinearNoSaturate(float value, float3 border)
			{
				return float3(
				(value - border.x) / clamp(fwidth(value), 0.0001, 1.0),
				(value - border.y) / clamp(fwidth(value), 0.0001, 1.0),
				(value - border.z) / clamp(fwidth(value), 0.0001, 1.0)
				);
			}
			
			float poiEdgeLinearNoSaturate(float value, float border, float blur)
			{
				float borderMin = saturate(border - blur * 0.5);
				float borderMax = saturate(border + blur * 0.5);
				return (value - borderMin) / saturate(borderMax - borderMin + fwidth(value));
			}
			
			float poiEdgeLinearNoSaturate(float value, float border, float blur, float borderRange)
			{
				float borderMin = saturate(border - blur * 0.5 - borderRange);
				float borderMax = saturate(border + blur * 0.5);
				return (value - borderMin) / saturate(borderMax - borderMin + fwidth(value));
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border)
			{
				//return (value - border) / clamp(fwidth(value), 0.0001, 1.0);
				
				float fwidthValue = fwidth(value);
				return smoothstep(border - fwidthValue, border + fwidthValue, value);
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border, float blur)
			{
				float fwidthValue = fwidth(value);
				float borderMin = saturate(border - blur * 0.5);
				float borderMax = saturate(border + blur * 0.5);
				return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border, float blur, float borderRange)
			{
				float fwidthValue = fwidth(value);
				float borderMin = saturate(border - blur * 0.5 - borderRange);
				float borderMax = saturate(border + blur * 0.5);
				return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
			}
			
			float poiEdgeNonLinear(float value, float border)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border));
			}
			
			float poiEdgeNonLinear(float value, float border, float blur)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border, blur));
			}
			
			float poiEdgeNonLinear(float value, float border, float blur, float borderRange)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border, blur, borderRange));
			}
			
			float poiEdgeLinear(float value, float border)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border));
			}
			
			float poiEdgeLinear(float value, float border, float blur)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border, blur));
			}
			
			float poiEdgeLinear(float value, float border, float blur, float borderRange)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border, blur, borderRange));
			}
			// From https://github.com/lilxyzw/OpenLit/blob/main/Assets/OpenLit/core.hlsl
			float3 OpenLitLinearToSRGB(float3 col)
			{
				return LinearToGammaSpace(col);
			}
			
			float3 OpenLitSRGBToLinear(float3 col)
			{
				return GammaToLinearSpace(col);
			}
			
			float OpenLitLuminance(float3 rgb)
			{
				#if defined(UNITY_COLORSPACE_GAMMA)
				return dot(rgb, float3(0.22, 0.707, 0.071));
				#else
				return dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
				#endif
			}
			
			float3 AdjustLitLuminance(float3 rgb, float targetLuminance)
			{
				float currentLuminance;
				#if defined(UNITY_COLORSPACE_GAMMA)
				currentLuminance = dot(rgb, float3(0.22, 0.707, 0.071));
				#else
				currentLuminance = dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
				#endif
				
				float luminanceRatio = targetLuminance / currentLuminance;
				return rgb * luminanceRatio;
			}
			
			float3 ClampLuminance(float3 rgb, float minLuminance, float maxLuminance)
			{
				float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
				float minRatio = (currentLuminance != 0) ? minLuminance / currentLuminance : 1.0;
				float maxRatio = (currentLuminance != 0) ? maxLuminance / currentLuminance : 1.0;
				float luminanceRatio = clamp(min(maxRatio, max(minRatio, 1.0)), 0.0, 1.0);
				return lerp(rgb, rgb * luminanceRatio, luminanceRatio < 1.0);
			}
			
			float3 MaxLuminance(float3 rgb, float maxLuminance)
			{
				float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
				float luminanceRatio = (currentLuminance != 0) ? maxLuminance / currentLuminance : 1.0;
				return lerp(rgb, rgb * luminanceRatio, currentLuminance > maxLuminance);
			}
			
			float OpenLitGray(float3 rgb)
			{
				return dot(rgb, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0));
			}
			
			void OpenLitShadeSH9ToonDouble(float3 lightDirection, out float3 shMax, out float3 shMin)
			{
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 N = lightDirection * 0.666666;
				float4 vB = N.xyzz * N.yzzx;
				// L0 L2
				float3 res = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
				res.r += dot(unity_SHBr, vB);
				res.g += dot(unity_SHBg, vB);
				res.b += dot(unity_SHBb, vB);
				res += unity_SHC.rgb * (N.x * N.x - N.y * N.y);
				// L1
				float3 l1;
				l1.r = dot(unity_SHAr.rgb, N);
				l1.g = dot(unity_SHAg.rgb, N);
				l1.b = dot(unity_SHAb.rgb, N);
				shMax = res + l1;
				shMin = res - l1;
				#if defined(UNITY_COLORSPACE_GAMMA)
				shMax = OpenLitLinearToSRGB(shMax);
				shMin = OpenLitLinearToSRGB(shMin);
				#endif
				#else
				shMax = 0.0;
				shMin = 0.0;
				#endif
			}
			
			float3 OpenLitComputeCustomLightDirection(float4 lightDirectionOverride)
			{
				float3 customDir = length(lightDirectionOverride.xyz) * normalize(mul((float3x3)unity_ObjectToWorld, lightDirectionOverride.xyz));
				return lightDirectionOverride.w ? customDir : lightDirectionOverride.xyz; // .w isn't doc'd anywhere and is always 0 unless end user changes it
				
			}
			
			float3 OpenLitLightingDirectionForSH9()
			{
				float3 mainDir = _WorldSpaceLightPos0.xyz * OpenLitLuminance(_LightColor0.rgb);
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 sh9Dir = unity_SHAr.xyz * 0.333333 + unity_SHAg.xyz * 0.333333 + unity_SHAb.xyz * 0.333333;
				float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
				#else
				float3 sh9Dir = 0;
				float3 sh9DirAbs = 0;
				#endif
				
				float3 lightDirectionForSH9 = sh9Dir + mainDir;
				lightDirectionForSH9 = dot(lightDirectionForSH9, lightDirectionForSH9) < 0.000001 ? 0 : normalize(lightDirectionForSH9);
				return lightDirectionForSH9;
			}
			
			float3 OpenLitLightingDirection(float4 lightDirectionOverride)
			{
				float3 mainDir = _WorldSpaceLightPos0.xyz * OpenLitLuminance(_LightColor0.rgb);
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 sh9Dir = unity_SHAr.xyz * 0.333333 + unity_SHAg.xyz * 0.333333 + unity_SHAb.xyz * 0.333333;
				float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
				#else
				float3 sh9Dir = 0;
				float3 sh9DirAbs = 0;
				#endif
				float3 customDir = OpenLitComputeCustomLightDirection(lightDirectionOverride);
				
				return normalize(sh9DirAbs + mainDir + customDir);
			}
			
			float3 OpenLitLightingDirection()
			{
				float4 customDir = float4(0.001, 0.002, 0.001, 0.0);
				return OpenLitLightingDirection(customDir);
			}
			
			inline float4 CalculateFrustumCorrection()
			{
				float x1 = -UNITY_MATRIX_P._31 / (UNITY_MATRIX_P._11 * UNITY_MATRIX_P._34);
				float x2 = -UNITY_MATRIX_P._32 / (UNITY_MATRIX_P._22 * UNITY_MATRIX_P._34);
				return float4(x1, x2, 0, UNITY_MATRIX_P._33 / UNITY_MATRIX_P._34 + x1 * UNITY_MATRIX_P._13 + x2 * UNITY_MATRIX_P._23);
			}
			
			inline float CorrectedLinearEyeDepth(float z, float B)
			{
				return 1.0 / (z / UNITY_MATRIX_P._34 + B);
			}
			
			//Silent's code
			float2 sharpSample(float4 texelSize, float2 p)
			{
				p = p * texelSize.zw;
				float2 c = max(0.0, fwidth(p));
				p = floor(p) + saturate(frac(p) / c);
				p = (p - 0.5) * texelSize.xy;
				return p;
			}
			
			void applyToGlobalMask(inout PoiMods poiMods, int index, int blendType, float val)
			{
				float valBlended = saturate(maskBlend(poiMods.globalMask[index], val, blendType));
				switch(index)
				{
					case 0: poiMods.globalMask[0] = valBlended; break;
					case 1: poiMods.globalMask[1] = valBlended; break;
					case 2: poiMods.globalMask[2] = valBlended; break;
					case 3: poiMods.globalMask[3] = valBlended; break;
					case 4: poiMods.globalMask[4] = valBlended; break;
					case 5: poiMods.globalMask[5] = valBlended; break;
					case 6: poiMods.globalMask[6] = valBlended; break;
					case 7: poiMods.globalMask[7] = valBlended; break;
					case 8: poiMods.globalMask[8] = valBlended; break;
					case 9: poiMods.globalMask[9] = valBlended; break;
					case 10: poiMods.globalMask[10] = valBlended; break;
					case 11: poiMods.globalMask[11] = valBlended; break;
					case 12: poiMods.globalMask[12] = valBlended; break;
					case 13: poiMods.globalMask[13] = valBlended; break;
					case 14: poiMods.globalMask[14] = valBlended; break;
					case 15: poiMods.globalMask[15] = valBlended; break;
				}
			}
			
			void assignValueToVectorFromIndex(inout float4 vec, int index, float value)
			{
				switch(index)
				{
					case 0: vec[0] = value; break;
					case 1: vec[1] = value; break;
					case 2: vec[2] = value; break;
					case 3: vec[3] = value; break;
				}
			}
			
			// SNose
			float3 mod289(float3 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			float2 mod289(float2 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			float3 permute(float3 x)
			{
				return mod289(((x * 34.0) + 1.0) * x);
			}
			
			float snoise(float2 v)
			{
				const float4 C = float4(0.211324865405187, // (3.0-sqrt(3.0))/6.0
				0.366025403784439, // 0.5*(sqrt(3.0)-1.0)
				- 0.577350269189626, // -1.0 + 2.0 * C.x
				0.024390243902439); // 1.0 / 41.0
				float2 i = floor(v + dot(v, C.yy));
				float2 x0 = v - i + dot(i, C.xx);
				float2 i1;
				i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod289(i); // Avoid truncation effects in permutation
				float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0))
				+ i.x + float3(0.0, i1.x, 1.0));
				
				float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
				m = m * m ;
				m = m * m ;
				float3 x = 2.0 * frac(p * C.www) - 1.0;
				float3 h = abs(x) - 0.5;
				float3 ox = floor(x + 0.5);
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot(m, g);
			}
			
			float nsqDistance(float2 a, float2 b)
			{
				return dot(a - b, a - b);
			}
			
			float poiInvertToggle(in float value, in float toggle)
			{
				return (toggle == 0 ? value : 1 - value);
			}
			
			// OKLAB
			
			float3 PoiBlendNormal(float3 dstNormal, float3 srcNormal)
			{
				return float3(dstNormal.xy + srcNormal.xy, dstNormal.z * srcNormal.z);
			}
			
			float3 lilTransformDirOStoWS(float3 directionOS, bool doNormalize)
			{
				if (doNormalize) return normalize(mul((float3x3)unity_ObjectToWorld, directionOS));
				else            return mul((float3x3)unity_ObjectToWorld, directionOS);
			}
			
			float2 poiGetWidthAndHeight(Texture2D tex)
			{
				uint width, height;
				tex.GetDimensions(width, height);
				return float2(width, height);
			}
			
			float2 poiGetWidthAndHeight(Texture2DArray tex)
			{
				uint width, height, element;
				tex.GetDimensions(width, height, element);
				return float2(width, height);
			}
			
			VertexOut vert(
			#ifndef POI_TESSELLATED
			appdata v
			#else
			tessAppData v
			#endif
			)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				VertexOut o;
				PoiInitStruct(VertexOut, o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				#ifdef POI_TESSELLATED
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(v);
				#endif
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.tangent.xyz = UnityObjectToWorldDir(v.tangent);
				o.tangent.w = v.tangent.w;
				o.vertexColor = v.color;
				
				o.uv[0] = float4(v.uv0.xy, v.uv1.xy);
				o.uv[1] = float4(v.uv2.xy, v.uv3.xy);
				
				#if defined(LIGHTMAP_ON)
				o.lightmapUV.xy = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				#ifdef DYNAMICLIGHTMAP_ON
				o.lightmapUV.zw = v.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif
				
				o.localPos = v.vertex;
				o.worldPos = mul(unity_ObjectToWorld, o.localPos);
				
				float3 localOffset = float3(0, 0, 0);
				float3 worldOffset = float3(0, 0, 0);
				
				o.localPos.rgb += localOffset;
				o.worldPos.rgb += worldOffset;
				
				o.pos = UnityObjectToClipPos(o.localPos);
				
				#ifdef POI_PASS_OUTLINE
				#if defined(UNITY_REVERSED_Z)
				//DX
				o.pos.z += _Offset_Z * - 0.01;
				#else
				//OpenGL
				o.pos.z += _Offset_Z * 0.01;
				#endif
				#endif
				//o.grabPos = ComputeGrabScreenPos(o.pos);
				
				#ifndef FORWARD_META_PASS
				#if !defined(UNITY_PASS_SHADOWCASTER)
				UNITY_TRANSFER_SHADOW(o, o.uv[0].xy);
				#else
				v.vertex.xyz = o.localPos.xyz;
				TRANSFER_SHADOW_CASTER_NOPOS(o, o.pos);
				#endif
				#endif
				
				UNITY_TRANSFER_FOG(o, o.pos);
				
				if (_RenderingReduceClipDistance)
				{
					if (o.pos.w < _ProjectionParams.y * 1.01 && o.pos.w > 0)
					{
						#if defined(UNITY_REVERSED_Z) // DirectX
						o.pos.z = o.pos.z * 0.0001 + o.pos.w * 0.999;
						#else // OpenGL
						o.pos.z = o.pos.z * 0.0001 - o.pos.w * 0.999;
						#endif
					}
				}
				
				#ifdef POI_PASS_META
				o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
				#endif
				
				return o;
			}
			
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, uv) : POI2D_SAMPLER(tex, texSampler, uv))
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan)) : POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), dx, dy) : POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			#if defined(_STOCHASTICMODE_HEXTILE)
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (useStochastic ? HextileSampleTexture(tex, sampler##texSampler, uv, false) : POI2D_SAMPLER(tex, texSampler, uv))
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (useStochastic ? HextileSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), false) : POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (useStochastic ? HextileSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), false, dx, dy) : POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			
			#ifndef POI2D_SAMPLER_STOCHASTIC
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (POI2D_SAMPLER(tex, texSampler, uv))
			#endif
			#ifndef POI2D_SAMPLER_PAN_STOCHASTIC
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#endif
			#ifndef POI2D_SAMPLER_PANGRAD_STOCHASTIC
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			
			// When using, properties won't properly lock at optimize time; needs macro evaluation implemented
			// #define POI2D_SAMPLER_STOCHASTIC_INLINED(tex, texSampler) (POI2D_SAMPLER_STOCHASTIC(tex, texSampler, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Stochastic))
			// #define POI2D_SAMPLER_PAN_STOCHASTIC_INLINED(tex, texSampler) (POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan, tex##Stochastic))
			
			// #define POI2D_MAINTEX_SAMPLER_STOCHASTIC_INLINED(tex) (POI2D_SAMPLER_STOCHASTIC(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Stochastic))
			// #define POI2D_MAINTEX_SAMPLER_PAN_STOCHASTIC_INLINED(tex) (POI2D_SAMPLER_PAN_STOCHASTIC(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan, tex##Stochastic))
			
			// Deliot, Heitz 2019 - Fast, but non-histogram-preserving (ends up looking a bit blurry and lower contrast)
			// https://eheitzresearch.wordpress.com/738-2/
			
			// Classic Magic Numbers fracsin
			#if !defined(_STOCHASTICMODE_NONE)
			float2 StochasticHash2D2D (float2 s)
			{
				return frac(sin(glsl_mod(float2(dot(s, float2(127.1,311.7)), dot(s, float2(269.5,183.3))), 3.14159)) * 43758.5453);
			}
			#endif
			
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			// UV Offsets and blend weights
			// UVBW[0...2].xy = UV Offsets
			// UVBW[0...2].z = Blend Weights
			float3x3 DeliotHeitzStochasticUVBW(float2 uv)
			{
				// UV transformed into triangular grid space with UV scaled by approximation of 2*sqrt(3)
				const float2x2 stochasticSkewedGrid = float2x2(1.0, -0.57735027, 0.0, 1.15470054);
				float2 skewUV = mul(stochasticSkewedGrid, uv * 3.4641 * _StochasticDeliotHeitzDensity);
				
				// Vertex IDs and barycentric coords
				float2 vxID = floor(skewUV);
				float3 bary = float3(frac(skewUV), 0);
				bary.z = 1.0 - bary.x - bary.y;
				
				float3x3 pos = float3x3(
				float3(vxID, 				bary.z),
				float3(vxID + float2(0, 1), bary.y),
				float3(vxID + float2(1, 0), bary.x)
				);
				
				float3x3 neg = float3x3(
				float3(vxID + float2(1, 1), 	 -bary.z),
				float3(vxID + float2(1, 0), 1.0 - bary.y),
				float3(vxID + float2(0, 1), 1.0 - bary.x)
				);
				
				return (bary.z > 0) ? pos : neg;
			}
			
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, float2 dx, float2 dy)
			{
				// UVBW[0...2].xy = UV Offsets
				// UVBW[0...2].z = Blend Weights
				float3x3 UVBW = DeliotHeitzStochasticUVBW(uv);
				
				//blend samples with calculated weights
				return 	mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[0].xy), dx, dy), UVBW[0].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[1].xy), dx, dy), UVBW[1].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[2].xy), dx, dy), UVBW[2].z) ;
			}
			
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv)
			{
				float2 dx = ddx(uv), dy = ddy(uv);
				return DeliotHeitzSampleTexture(tex, texSampler, uv, dx, dy);
			}
			#endif // defined(_STOCHASTICMODE_DELIOT_HEITZ)
			
			#if defined(_STOCHASTICMODE_HEXTILE)
			// HexTiling: Slower, but histogram-preserving
			// SPDX-License-Idenfitier: MIT
			// Copyright (c) 2022 mmikk
			// https://github.com/mmikk/hextile-demo
			float2 HextileMakeCenUV(float2 vertex)
			{
				// 0.288675 ~= 1/(2*sqrt(3))
				const float2x2 stochasticInverseSkewedGrid = float2x2(1.0, 0.5, 0.0, 1.0/1.15470054);
				return mul(stochasticInverseSkewedGrid, vertex) * 0.288675;
			}
			
			float2x2 HextileLoadRot2x2(float2 idx, float rotStrength)
			{
				float angle = abs(idx.x * idx.y) + abs(idx.x + idx.y) + PI;
				
				// remap to +/-pi
				angle = glsl_mod(angle, 2 * PI);
				if(angle < 0)  angle += 2 * PI;
				if(angle > PI) angle -= 2 * PI;
				
				angle *= rotStrength;
				
				float cs = cos(angle), si = sin(angle);
				return float2x2(cs, -si, si, cs);
			}
			
			// UV Offsets and base blend weights
			// UVBWR[0...2].xy = UV Offsets
			// UVBWR[0...2].zw = rotation costh/sinth -> reconstruct rotation matrix with float2x2(UVBWR[n].z, -UVBWR[n].w, UVBWR[n].w, UVBWR[n].z)
			// UVBWR[3].xyz = Blend Weights (w unused) - needs luminance weighting
			float4x4 HextileUVBWR(float2 uv)
			{
				// Create Triangle Grid
				// Skew input space into simplex triangle grid (3.4641 ~= 2*sqrt(3))
				const float2x2 stochasticSkewedGrid = float2x2(1.0, -0.57735027, 0.0, 1.15470054);
				float2 skewedCoord = mul(stochasticSkewedGrid, uv * 3.4641 * _StochasticHexGridDensity);
				
				float2 baseId = float2(floor(skewedCoord));
				float3 temp = float3(frac(skewedCoord), 0);
				temp.z = 1 - temp.x - temp.y;
				
				float s = step(0.0, -temp.z);
				float s2 = 2 * s - 1;
				
				float3 weights = float3(-temp.z * s2, s - temp.y * s2, s - temp.x * s2);
				
				float2 vertex0 = baseId + float2(s, s);
				float2 vertex1 = baseId + float2(s, 1 - s);
				float2 vertex2 = baseId + float2(1 - s, s);
				
				float2 cen0 = HextileMakeCenUV(vertex0), cen1 = HextileMakeCenUV(vertex1), cen2 = HextileMakeCenUV(vertex2);
				float2x2 rot0 = float2x2(1, 0, 0, 1), rot1 = float2x2(1, 0, 0, 1), rot2 = float2x2(1, 0, 0, 1);
				
				if(_StochasticHexRotationStrength > 0)
				{
					rot0 = HextileLoadRot2x2(vertex0, _StochasticHexRotationStrength);
					rot1 = HextileLoadRot2x2(vertex1, _StochasticHexRotationStrength);
					rot2 = HextileLoadRot2x2(vertex2, _StochasticHexRotationStrength);
				}
				
				return float4x4(
				float4(mul(uv - cen0, rot0) + cen0 + StochasticHash2D2D(vertex0), rot0[0].x, -rot0[0].y),
				float4(mul(uv - cen1, rot1) + cen1 + StochasticHash2D2D(vertex1), rot1[0].x, -rot1[0].y),
				float4(mul(uv - cen2, rot2) + cen2 + StochasticHash2D2D(vertex2), rot2[0].x, -rot2[0].y),
				float4(weights, 0)
				);
			}
			
			float4 HextileSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, bool isNormalMap, float2 dUVdx, float2 dUVdy)
			{
				// For some reason doing this instead of just calculating it directly prevents it from \
				// breaking after a certain number of textures use it. I don't understand why yet
				float4x4 UVBWR = HextileUVBWR(uv);
				
				// 2D Rotation Matrices for dUVdx/dy
				// Not sure if this constant folds during compiling when rot is locked at 0, so force it
				float2x2 rot0 = float2x2(1, 0, 0, 1), rot1 = float2x2(1, 0, 0, 1), rot2 = float2x2(1, 0, 0, 1);
				
				if(_StochasticHexRotationStrength > 0)
				{
					rot0 = float2x2(UVBWR[0].z, -UVBWR[0].w, UVBWR[0].w, UVBWR[0].z);
					rot1 = float2x2(UVBWR[1].z, -UVBWR[1].w, UVBWR[1].w, UVBWR[1].z);
					rot2 = float2x2(UVBWR[2].z, -UVBWR[2].w, UVBWR[2].w, UVBWR[2].z);
				}
				
				// Weights
				float3 W = UVBWR[3].xyz;
				
				// Sample texture
				// float3x4 c = float3x4(
				// 	tex.SampleGrad(texSampler, UVBWR[0].xy, mul(dUVdx, rot0), mul(dUVdy, rot0)),
				// 	tex.SampleGrad(texSampler, UVBWR[1].xy, mul(dUVdx, rot1), mul(dUVdy, rot1)),
				// 	tex.SampleGrad(texSampler, UVBWR[2].xy, mul(dUVdx, rot2), mul(dUVdy, rot2))
				// );
				
				float4 c0 = tex.SampleGrad(texSampler, UVBWR[0].xy, mul(dUVdx, rot0), mul(dUVdy, rot0));
				float4 c1 = tex.SampleGrad(texSampler, UVBWR[1].xy, mul(dUVdx, rot1), mul(dUVdy, rot1));
				float4 c2 = tex.SampleGrad(texSampler, UVBWR[2].xy, mul(dUVdx, rot2), mul(dUVdy, rot2));
				
				// Blend samples using luminance
				// This is technically incorrect for normal maps, but produces very similar
				// results to blending using normal map gradients (steepness)
				const float3 Lw = float3(0.299, 0.587, 0.114);
				float3 Dw = float3(dot(c0.xyz, Lw), dot(c1.xyz, Lw), dot(c2.xyz, Lw));
				
				Dw = lerp(1.0, Dw, _StochasticHexFallOffContrast);
				W = Dw * pow(W, _StochasticHexFallOffPower);
				// In the original hextiling there's a Gain3 step here, but it seems to slow things down \
				// and cause the UVs to break, so I've omitted it. Looks fine without
				
				W /= (W.x + W.y + W.z);
				return W.x * c0 + W.y * c1 + W.z * c2;
			}
			
			float4 HextileSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, bool isNormalMap)
			{
				return HextileSampleTexture(tex, texSampler, uv, isNormalMap, ddx(uv), ddy(uv));
			}
			#endif // defined(_STOCHASTICMODE_HEXTILE)
			
			void applyAlphaOptions(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiMods poiMods)
			{
				poiFragData.alpha = saturate(poiFragData.alpha + _AlphaMod);
				
				if (_AlphaGlobalMask > 0)
				{
					poiFragData.alpha = maskBlend(poiFragData.alpha, poiMods.globalMask[_AlphaGlobalMask - 1], _AlphaGlobalMaskBlendType);
				}
				
				//ifex _AlphaDistanceFade==0 && isNotAnimated(_AlphaDistanceFade)
				if (_AlphaDistanceFade)
				{
					float3 position = _AlphaDistanceFadeType ? poiMesh.worldPos : poiMesh.objectPosition;
					float distanceFadeMultiplier = lerp(_AlphaDistanceFadeMinAlpha, _AlphaDistanceFadeMaxAlpha, smoothstep(_AlphaDistanceFadeMin, _AlphaDistanceFadeMax, distance(position, poiCam.worldPos)));
					if (_AlphaDistanceFadeGlobalMask > 0)
					{
						distanceFadeMultiplier = lerp(1, distanceFadeMultiplier, poiMods.globalMask[_AlphaDistanceFadeGlobalMask - 1]);
					}
					poiFragData.alpha *= distanceFadeMultiplier;
				}
				//endex
				
				//ifex _AlphaFresnel==0 && isNotAnimated(_AlphaFresnel)
				if (_AlphaFresnel)
				{
					float holoRim = saturate(1 - smoothstep(min(_AlphaFresnelSharpness, _AlphaFresnelWidth), _AlphaFresnelWidth, (poiCam.vDotN)));
					holoRim = abs(lerp(1, holoRim, _AlphaFresnelAlpha));
					holoRim = _AlphaFresnelInvert ? 1 - holoRim : holoRim;
					if (_AlphaFresnelGlobalMask > 0)
					{
						holoRim = lerp(1, holoRim, poiMods.globalMask[_AlphaFresnelGlobalMask - 1]);
					}
					poiFragData.alpha *= holoRim;
				}
				//endex
				
				//ifex _AlphaAngular==0 && isNotAnimated(_AlphaAngular)
				if (_AlphaAngular)
				{
					half cameraAngleMin = _CameraAngleMin / 180;
					half cameraAngleMax = _CameraAngleMax / 180;
					half modelAngleMin = _ModelAngleMin / 180;
					half modelAngleMax = _ModelAngleMax / 180;
					float3 pos = _AngleCompareTo == 0 ? poiMesh.objectPosition : poiMesh.worldPos;
					half3 cameraToModelDirection = normalize(pos - getCameraPosition());
					half3 modelForwardDirection = normalize(mul(unity_ObjectToWorld, normalize(_AngleForwardDirection.rgb)));
					half cameraLookAtModel = remapClamped(cameraAngleMax, cameraAngleMin, .5 * dot(cameraToModelDirection, getCameraForward()) + .5);
					half modelLookAtCamera = remapClamped(modelAngleMax, modelAngleMin, .5 * dot(-cameraToModelDirection, modelForwardDirection) + .5);
					float angularAlphaMod = 1;
					if (_AngleType == 0)
					{
						angularAlphaMod = max(cameraLookAtModel, _AngleMinAlpha);
					}
					else if (_AngleType == 1)
					{
						angularAlphaMod = max(modelLookAtCamera, _AngleMinAlpha);
					}
					else if (_AngleType == 2)
					{
						angularAlphaMod = max(cameraLookAtModel * modelLookAtCamera, _AngleMinAlpha);
					}
					if (_AlphaAngularGlobalMask > 0)
					{
						angularAlphaMod = lerp(1, angularAlphaMod, poiMods.globalMask[_AlphaAngularGlobalMask - 1]);
					}
					poiFragData.alpha *= angularAlphaMod;
				}
				//endex
				
				//ifex _AlphaAudioLinkEnabled==0 && isNotAnimated(_AlphaAudioLinkEnabled)
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable && _AlphaAudioLinkEnabled)
				{
					poiFragData.alpha = saturate(poiFragData.alpha + lerp(_AlphaAudioLinkAddRange.x, _AlphaAudioLinkAddRange.y, poiMods.audioLink[_AlphaAudioLinkAddBand]));
				}
				#endif
				//endex
				
			}
			
			//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
			inline half Dither8x8Bayer(int x, int y)
			{
				// Premultiplied by 1/64
				const half dither[ 64 ] = {
					0.015625, 0.765625, 0.203125, 0.953125, 0.06250, 0.81250, 0.25000, 1.00000,
					0.515625, 0.265625, 0.703125, 0.453125, 0.56250, 0.31250, 0.75000, 0.50000,
					0.140625, 0.890625, 0.078125, 0.828125, 0.18750, 0.93750, 0.12500, 0.87500,
					0.640625, 0.390625, 0.578125, 0.328125, 0.68750, 0.43750, 0.62500, 0.37500,
					0.046875, 0.796875, 0.234375, 0.984375, 0.03125, 0.78125, 0.21875, 0.96875,
					0.546875, 0.296875, 0.734375, 0.484375, 0.53125, 0.28125, 0.71875, 0.46875,
					0.171875, 0.921875, 0.109375, 0.859375, 0.15625, 0.90625, 0.09375, 0.84375,
					0.671875, 0.421875, 0.609375, 0.359375, 0.65625, 0.40625, 0.59375, 0.34375
				};
				int r = y * 8 + x;
				return dither[r];
			}
			
			half calcDither(half2 grabPos)
			{
				return Dither8x8Bayer(glsl_mod(grabPos.x, 8), glsl_mod(grabPos.y, 8));
			}
			
			void applyDithering(inout PoiFragData poiFragData, in PoiCam poiCam)
			{
				if (_AlphaDithering)
				{
					float dither = calcDither(poiCam.posScreenPixels) - _AlphaDitherBias;
					poiFragData.alpha = saturate(poiFragData.alpha - (dither * (1 - poiFragData.alpha) * _AlphaDitherGradient));
				}
			}
			//endex
			
			//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
			void ApplyAlphaToCoverage(inout PoiFragData poiFragData, in PoiMesh poiMesh)
			{
				// Force Model Opacity to 1 if desired
				UNITY_BRANCH
				if (_Mode == 1)
				{
					UNITY_BRANCH
					if (_AlphaSharpenedA2C && _AlphaToCoverage)
					{
						// rescale alpha by mip level
						poiFragData.alpha *= 1 + max(0, CalcMipLevel(poiMesh.uv[0] * _MainTex_TexelSize.zw)) * _AlphaMipScale;
						// rescale alpha by partial derivative
						poiFragData.alpha = (poiFragData.alpha - _Cutoff) / max(fwidth(poiFragData.alpha), 0.0001) + _Cutoff;
						poiFragData.alpha = saturate(poiFragData.alpha);
					}
				}
			}
			//endex
			
			//ifex _PoiParallax==0
			#ifdef POI_PARALLAX
			inline float2 POM(in PoiLight poiLight, sampler2D heightMap, in PoiMesh poiMesh, float3 worldViewDir, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv)
			{
				#if defined(PROP_HEIGHTMASK) || !defined(OPTIMIZER_ENABLED)
				float heightMask = POI2D_SAMPLER_PAN(_Heightmask, _linear_repeat, poiUV(poiMesh.uv[_HeightmaskUV], _Heightmask_ST), _HeightmaskPan)[_HeightmaskChannel];
				if (_HeightmaskInvert)
				{
					heightMask = 1 - heightMask;
				}
				#else
				float heightMask = 1;
				#endif
				
				float2 uvs = poiUV(poiMesh.uv[_HeightMapUV], _HeightMap_ST);
				float2 dx = ddx(uvs);
				float2 dy = ddy(uvs);
				float3 result = 0;
				int stepIndex = 0;
				int numSteps = (int)lerp(maxSamples, minSamples, saturate(dot(poiMesh.normals[0], worldViewDir)));
				float layerHeight = 1.0 / numSteps;
				float2 plane = parallax * heightMask * (viewDirTan.xy / viewDirTan.z);
				uvs += refPlane * plane;
				float2 deltaTex = -plane * layerHeight;
				float2 prevTexOffset = 0;
				float prevRayZ = 1.0f;
				float prevHeight = 0.0f;
				float2 currTexOffset = deltaTex;
				float currRayZ = 1.0f - layerHeight;
				float currHeight = 0.0f;
				float intersection = 0;
				float2 finalTexOffset = 0;
				while (stepIndex < numSteps + 1)
				{
					result.z = dot(curv, currTexOffset * currTexOffset);
					currHeight = tex2Dgrad(heightMap, uvs + currTexOffset, dx, dy).r * (1 - result.z);
					if (currHeight > currRayZ)
					{
						stepIndex = numSteps + 1;
					}
					else
					{
						stepIndex++;
						prevTexOffset = currTexOffset;
						prevRayZ = currRayZ;
						prevHeight = currHeight;
						currTexOffset += deltaTex;
						currRayZ -= layerHeight * (1 - result.z) * (1 + _CurvFix);
					}
				}
				int sectionSteps = 10;
				int sectionIndex = 0;
				float newZ = 0;
				float newHeight = 0;
				while (sectionIndex < sectionSteps)
				{
					intersection = (prevHeight - prevRayZ) / (prevHeight - currHeight + currRayZ - prevRayZ);
					finalTexOffset = prevTexOffset +intersection * deltaTex;
					newZ = prevRayZ - intersection * layerHeight;
					newHeight = tex2Dgrad(heightMap, uvs + finalTexOffset, dx, dy).r;
					if (newHeight > newZ)
					{
						currTexOffset = finalTexOffset;
						currHeight = newHeight;
						currRayZ = newZ;
						deltaTex = intersection * deltaTex;
						layerHeight = intersection * layerHeight;
					}
					else
					{
						prevTexOffset = finalTexOffset;
						prevHeight = newHeight;
						prevRayZ = newZ;
						deltaTex = (1 - intersection) * deltaTex;
						layerHeight = (1 - intersection) * layerHeight;
					}
					sectionIndex++;
				}
				#ifdef UNITY_PASS_SHADOWCASTER
				if (unity_LightShadowBias.z == 0.0)
				{
					#endif
					if (result.z > 1)
					clip(-1);
					#ifdef UNITY_PASS_SHADOWCASTER
				}
				#endif
				
				return uvs + finalTexOffset;
			}
			/*
			float2 ParallaxOffsetMultiStep(float surfaceHeight, float strength, float2 uv, float3 tangentViewDir)
			{
				float2 uvOffset = 0;
				float2 prevUVOffset = 0;
				float stepSize = 1.0 / _HeightSteps;
				float stepHeight = 1;
				float2 uvDelta = tangentViewDir.xy * (stepSize * strength);
				float prevStepHeight = stepHeight;
				float prevSurfaceHeight = surfaceHeight;
				
				[unroll(20)]
				for (int j = 1; j <= _HeightSteps && stepHeight > surfaceHeight; j++)
				{
					prevUVOffset = uvOffset;
					prevStepHeight = stepHeight;
					prevSurfaceHeight = surfaceHeight;
					uvOffset -= uvDelta;
					stepHeight -= stepSize;
					surfaceHeight = POI2D_SAMPLER_PAN(_Heightmap, _MainTex, poiUV(uv + uvOffset, _Heightmap_ST), _HeightmapPan) + _HeightOffset;
				}
				
				[unroll(3)]
				for (int k = 0; k < 3; k++)
				{
					uvDelta *= 0.5;
					stepSize *= 0.5;
					
					if (stepHeight < surfaceHeight)
					{
						uvOffset += uvDelta;
						stepHeight += stepSize;
					}
					else
					{
						uvOffset -= uvDelta;
						stepHeight -= stepSize;
					}
					surfaceHeight = POI2D_SAMPLER_PAN(_Heightmap, _MainTex, poiUV(uv + uvOffset, _Heightmap_ST), _HeightmapPan) + _HeightOffset;
				}
				return uvOffset;
			}
			*/
			void applyParallax(inout PoiMesh poiMesh, in PoiLight poiLight, in PoiCam poiCam)
			{
				/*
				half h = POI2D_SAMPLER_PAN(_Heightmap, _linear_repeat, poiUV(poiMesh.uv[_HeightmaskUV], _Heightmap_ST), _HeightmapPan).r + _HeightOffset;
				#if defined(PROP_HEIGHTMASK) || !defined(OPTIMIZER_ENABLED)
				half m = POI2D_SAMPLER_PAN(_Heightmask, _linear_repeat, poiUV(poiMesh.uv[_HeightmaskUV], _Heightmask_ST), _HeightmaskPan).r + _HeightOffset;
				#else
				half m = 1 + _HeightOffset;
				#endif
				h = clamp(h, 0, 0.999);
				m = lerp(m, 1 - m, _HeightmaskInvert);
				#if defined(OPTIMIZER_ENABLED)das
				poiMesh.uv[_ParallaxUV] += ParallaxOffsetMultiStep(h, _HeightStrength * m, poiMesh.uv[_HeightmapUV], tangentViewDir / tangentViewDir.z);
				#else
				float2 offset = ParallaxOffsetMultiStep(h, _HeightStrength * m, poiMesh.uv[_HeightmapUV], tangentViewDir / tangentViewDir.z);
				if (_ParallaxUV == 0)       poiMesh.uv[0] += offset;
				if (_ParallaxUV == 1)       poiMesh.uv[1] += offset;
				if (_ParallaxUV == 2)       poiMesh.uv[2] += offset;
				if (_ParallaxUV == 3)       poiMesh.uv[3] += offset;
				if (_ParallaxUV == 4)       poiMesh.uv[4] += offset;
				if (_ParallaxUV == 5)       poiMesh.uv[5] += offset;
				if (_ParallaxUV == 6)       poiMesh.uv[6] += offset;
				if (_ParallaxUV == 7)       poiMesh.uv[7] += offset;
				#endif
				*/
				
				#if defined(OPTIMIZER_ENABLED)
				poiMesh.uv[_ParallaxUV] = POM(poiLight, _HeightMap, poiMesh, poiCam.viewDir, poiCam.tangentViewDir, _HeightStepsMin, _HeightStepsMax, _HeightStrength, 0, _HeightMap_ST.xy, float2(_CurvatureU, _CurvatureV));
				#else
				float2 offset = POM(poiLight, _HeightMap, poiMesh, poiCam.viewDir, poiCam.tangentViewDir, _HeightStepsMin, _HeightStepsMax, _HeightStrength, 0, _HeightMap_ST.xy, float2(_CurvatureU, _CurvatureV));
				if (_ParallaxUV == 0)       poiMesh.uv[0] = offset;
				if (_ParallaxUV == 1)       poiMesh.uv[1] = offset;
				if (_ParallaxUV == 2)       poiMesh.uv[2] = offset;
				if (_ParallaxUV == 3)       poiMesh.uv[3] = offset;
				if (_ParallaxUV == 4)       poiMesh.uv[4] = offset;
				if (_ParallaxUV == 5)       poiMesh.uv[5] = offset;
				if (_ParallaxUV == 6)       poiMesh.uv[6] = offset;
				if (_ParallaxUV == 7)       poiMesh.uv[7] = offset;
				#endif
			}
			#endif
			//endex
			
			//ifex _EnableEmission==0 && _EnableEmission1==0 && _EnableEmission2==0 && _EnableEmission3==0
			float calculateGlowInTheDark(in float minLight, in float maxLight, in float minEmissionMultiplier, in float maxEmissionMultiplier, in float enabled, in float worldOrMesh, in PoiLight poiLight)
			{
				float glowInTheDarkMultiplier = 1;
				//UNITY_BRANCH
				if (enabled)
				{
					float3 lightValue = worldOrMesh ? calculateluminance(poiLight.finalLighting.rgb) : calculateluminance(poiLight.directColor.rgb);
					float gitdeAlpha = saturate(inverseLerp(minLight, maxLight, lightValue));
					glowInTheDarkMultiplier = lerp(minEmissionMultiplier, maxEmissionMultiplier, gitdeAlpha);
				}
				return glowInTheDarkMultiplier;
			}
			
			float calculateScrollingEmission(in float3 direction, in float velocity, in float interval, in float scrollWidth, float offset, float3 position)
			{
				scrollWidth = max(scrollWidth, 0);
				float phase = 0;
				phase = dot(position, direction);
				phase -= (_Time.y + offset) * velocity;
				phase /= interval;
				phase -= floor(phase);
				phase = saturate(phase);
				return (pow(phase, scrollWidth) + pow(1 - phase, scrollWidth * 4)) * 0.5;
			}
			
			float calculateBlinkingEmission(in float blinkMin, in float blinkMax, in float blinkVelocity, float offset)
			{
				float amplitude = (blinkMax - blinkMin) * 0.5f;
				float base = blinkMin + amplitude;
				return sin((_Time.y + offset) * blinkVelocity) * amplitude + base;
			}
			
			void applyALEmmissionStrength(in PoiMods poiMods, inout float emissionStrength, in float2 emissionStrengthMod, in float emissionStrengthBand, in float2 _EmissionALMultipliers, in float _EmissionALMultipliersBand, in float enabled)
			{
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable && enabled)
				{
					emissionStrength += lerp(emissionStrengthMod.x, emissionStrengthMod.y, poiMods.audioLink[emissionStrengthBand]);
					emissionStrength *= lerp(_EmissionALMultipliers.x, _EmissionALMultipliers.y, poiMods.audioLink[_EmissionALMultipliersBand]);
				}
				#endif
			}
			
			void applyALCenterOutEmission(in PoiMods poiMods, in float nDotV, inout float emissionStrength, in float size, in float band, in float2 emissionToAdd, in float enabled, in float duration)
			{
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable && enabled)
				{
					float intensity;
					[flatten]
					if (duration >= 0)
					{
						intensity = getBandAtTime(band, saturate(remap(nDotV, 1, 0, 0, duration)), size);
					}
					else
					{
						duration *= -1;
						intensity = getBandAtTime(band, saturate(remap(pow(nDotV, 2), 0, 1 + duration, 0, duration)), size);
					}
					emissionStrength += lerp(emissionToAdd[0], emissionToAdd[1], intensity);
				}
				#endif
			}
			void applyLumaGradient(in PoiMods poiMods, inout float3 emissionColor, in float themeIndex, in float nDotV)
			{
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable && poiMods.audioLinkViaLuma && themeIndex >= 5 && themeIndex <= 7)
				{
					emissionColor = getLumaGradient(themeIndex - 5, saturate(1 - nDotV));
				}
				#endif
			}
			//endex
			
			//ifex _EnableEmission==0
			#ifdef _EMISSION
			float3 applyEmission(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiLight poiLight, in PoiCam poiCam, in PoiMods poiMods)
			{
				
				// First Emission
				float3 emission0 = 0;
				float emissionStrength0 = _EmissionStrength;
				float3 emissionColor0 = 0;
				applyALEmmissionStrength(poiMods, emissionStrength0, _EmissionAL0StrengthMod, _EmissionAL0StrengthBand, _EmissionAL0Multipliers, _EmissionAL0MultipliersBand, _EmissionAL0Enabled);
				applyALCenterOutEmission(poiMods, poiLight.nDotV, emissionStrength0, _AudioLinkEmission0CenterOutSize, _AudioLinkEmission0CenterOutBand, _AudioLinkEmission0CenterOut, _EmissionAL0Enabled, _AudioLinkEmission0CenterOutDuration);
				
				float glowInTheDarkMultiplier0 = calculateGlowInTheDark(_GITDEMinLight, _GITDEMaxLight, _GITDEMinEmissionMultiplier, _GITDEMaxEmissionMultiplier, _EnableGITDEmission, _GITDEWorldOrMesh, poiLight);
				
				#if defined(PROP_EMISSIONMAP) || !defined(OPTIMIZER_ENABLED)
				//UNITY_BRANCH
				if (!_EmissionCenterOutEnabled)
				{
					emissionColor0 = POI2D_SAMPLER_PAN(_EmissionMap, _MainTex, poiUV(poiMesh.uv[_EmissionMapUV], _EmissionMap_ST), _EmissionMapPan).rgb * lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap).rgb * poiThemeColor(poiMods, _EmissionColor.rgb, _EmissionColorThemeIndex);
				}
				else
				{
					emissionColor0 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMap, _MainTex, ((.5 + poiLight.nDotV * .5) * _EmissionMap_ST.xy) + _Time.x * _EmissionCenterOutSpeed).rgb * lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap).rgb * poiThemeColor(poiMods, _EmissionColor.rgb, _EmissionColorThemeIndex);
				}
				#else
				emissionColor0 = lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap).rgb * poiThemeColor(poiMods, _EmissionColor.rgb, _EmissionColorThemeIndex);
				#endif
				
				//UNITY_BRANCH
				if (_ScrollingEmission)
				{
					float3 pos = poiMesh.localPos;
					//UNITY_BRANCH
					if (_EmissionScrollingVertexColor)
					{
						pos = poiMesh.vertexColor.rgb;
					}
					
					//UNITY_BRANCH
					if (_EmissionScrollingUseCurve)
					{
						#if defined(PROP_EMISSIONSCROLLINGCURVE) || !defined(OPTIMIZER_ENABLED)
						emissionStrength0 *= UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionScrollingCurve, _MainTex, poiUV(poiMesh.uv[_EmissionMapUV], _EmissionScrollingCurve_ST) + (dot(pos, _EmissiveScroll_Direction.xyz) * _EmissiveScroll_Interval) + _Time.x * _EmissiveScroll_Velocity).r;
						#endif
					}
					else
					{
						emissionStrength0 *= calculateScrollingEmission(_EmissiveScroll_Direction.xyz, _EmissiveScroll_Velocity, _EmissiveScroll_Interval, _EmissiveScroll_Width, _EmissionScrollingOffset, pos);
					}
				}
				
				//UNITY_BRANCH
				if (_EmissionBlinkingEnabled)
				{
					emissionStrength0 *= calculateBlinkingEmission(_EmissiveBlink_Min, _EmissiveBlink_Max, _EmissiveBlink_Velocity, _EmissionBlinkingOffset);
				}
				
				applyLumaGradient(poiMods, emissionColor0, _EmissionColorThemeIndex, poiLight.nDotV);
				emissionColor0 = hueShift(emissionColor0, frac(_EmissionHueShift + _EmissionHueShiftSpeed * _Time.x) * _EmissionHueShiftEnabled);
				
				#if defined(PROP_EMISSIONMASK) || !defined(OPTIMIZER_ENABLED)
				float emissionMask0 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMask, _MainTex, poiUV(poiMesh.uv[_EmissionMaskUV], _EmissionMask_ST) + _Time.x * _EmissionMaskPan)[_EmissionMaskChannel];
				#else
				float emissionMask0 = 1;
				#endif
				
				if (_EmissionMaskInvert)
				{
					emissionMask0 = 1 - emissionMask0;
				}
				
				if (_EmissionMask0GlobalMask > 0)
				{
					emissionMask0 = maskBlend(emissionMask0, poiMods.globalMask[_EmissionMask0GlobalMask - 1], _EmissionMask0GlobalMaskBlendType);
				}
				
				emissionStrength0 *= glowInTheDarkMultiplier0 * emissionMask0;
				emission0 = max(emissionStrength0 * emissionColor0, 0);
				
				#ifdef POI_DISSOLVE
				//UNITY_BRANCH
				if (_DissolveEmissionSide != 2)
				{
					emission0 *= lerp(1 - dissolveAlpha, dissolveAlpha, _DissolveEmissionSide);
				}
				#endif
				
				// poiFragData.finalColor.rgb = lerp(poiFragData.finalColor.rgb, saturate(emission0 + emission1), _EmissionReplace * poiMax(emission0 + emission1));
				
				poiFragData.emission += emission0;
				return emission0 * _EmissionReplace0;
			}
			#endif
			//endex
			//ifex _EnableEmission1==0
			#ifdef POI_EMISSION_1
			float3 applyEmission1(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiLight poiLight, in PoiCam poiCam, in PoiMods poiMods)
			{
				
				// Second Emission
				float3 emission1 = 0;
				float emissionStrength1 = 0;
				float3 emissionColor1 = 0;
				
				emissionStrength1 = _EmissionStrength1;
				applyALEmmissionStrength(poiMods, emissionStrength1, _EmissionAL1StrengthMod, _EmissionAL1StrengthBand, _EmissionAL1Multipliers, _EmissionAL1MultipliersBand, _EmissionAL1Enabled);
				applyALCenterOutEmission(poiMods, poiLight.nDotV, emissionStrength1, _AudioLinkEmission1CenterOutSize, _AudioLinkEmission1CenterOutBand, _AudioLinkEmission1CenterOut, _EmissionAL1Enabled, _AudioLinkEmission1CenterOutDuration);
				
				float glowInTheDarkMultiplier1 = calculateGlowInTheDark(_GITDEMinLight1, _GITDEMaxLight1, _GITDEMinEmissionMultiplier1, _GITDEMaxEmissionMultiplier1, _EnableGITDEmission1, _GITDEWorldOrMesh1, poiLight);
				#if defined(PROP_EMISSIONMAP1) || !defined(OPTIMIZER_ENABLED)
				
				//UNITY_BRANCH
				if (!_EmissionCenterOutEnabled1)
				{
					emissionColor1 = POI2D_SAMPLER_PAN(_EmissionMap1, _MainTex, poiUV(poiMesh.uv[_EmissionMap1UV], _EmissionMap1_ST), _EmissionMap1Pan) * lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap1).rgb * poiThemeColor(poiMods, _EmissionColor1.rgb, _EmissionColor1ThemeIndex);
				}
				else
				{
					emissionColor1 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMap1, _MainTex, ((.5 + poiLight.nDotV * .5) * _EmissionMap1_ST.xy) + _Time.x * _EmissionCenterOutSpeed1).rgb * lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap1).rgb * poiThemeColor(poiMods, _EmissionColor1.rgb, _EmissionColor1ThemeIndex);
				}
				#else
				emissionColor1 = lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap1).rgb * poiThemeColor(poiMods, _EmissionColor1.rgb, _EmissionColor1ThemeIndex);
				#endif
				//UNITY_BRANCH
				if (_ScrollingEmission1)
				{
					float3 pos1 = poiMesh.localPos;
					//UNITY_BRANCH
					if (_EmissionScrollingVertexColor1)
					{
						pos1 = poiMesh.vertexColor.rgb;
					}
					
					//UNITY_BRANCH
					if (_EmissionScrollingUseCurve1)
					{
						#if defined(PROP_EMISSIONSCROLLINGCURVE1) || !defined(OPTIMIZER_ENABLED)
						emissionStrength1 *= UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionScrollingCurve1, _MainTex, poiUV(poiMesh.uv[_EmissionMap1UV], _EmissionScrollingCurve1_ST) + (dot(pos1, _EmissiveScroll_Direction1) * _EmissiveScroll_Interval1) + _Time.x * _EmissiveScroll_Velocity1);
						#endif
					}
					else
					{
						emissionStrength1 *= calculateScrollingEmission(_EmissiveScroll_Direction1, _EmissiveScroll_Velocity1, _EmissiveScroll_Interval1, _EmissiveScroll_Width1, _EmissionScrollingOffset1, pos1);
					}
				}
				//UNITY_BRANCH
				if (_EmissionBlinkingEnabled1)
				{
					emissionStrength1 *= calculateBlinkingEmission(_EmissiveBlink_Min1, _EmissiveBlink_Max1, _EmissiveBlink_Velocity1, _EmissionBlinkingOffset1);
				}
				
				applyLumaGradient(poiMods, emissionColor1, _EmissionColor1ThemeIndex, poiLight.nDotV);
				emissionColor1 = hueShift(emissionColor1, frac(_EmissionHueShift1 + _EmissionHueShiftSpeed1 * _Time.x) * _EmissionHueShiftEnabled1);
				#if defined(PROP_EMISSIONMASK1) || !defined(OPTIMIZER_ENABLED)
				float emissionMask1 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMask1, _MainTex, poiUV(poiMesh.uv[_EmissionMask1UV], _EmissionMask1_ST) + _Time.x * _EmissionMask1Pan)[_EmissionMask1Channel];
				#else
				float emissionMask1 = 1;
				#endif
				
				if (_EmissionMaskInvert1)
				{
					emissionMask1 = 1 - emissionMask1;
				}
				
				if (_EmissionMask1GlobalMask > 0)
				{
					emissionMask1 = maskBlend(emissionMask1, poiMods.globalMask[_EmissionMask1GlobalMask - 1], _EmissionMask1GlobalMaskBlendType);
				}
				
				emissionStrength1 *= glowInTheDarkMultiplier1 * emissionMask1;
				emission1 = max(emissionStrength1 * emissionColor1, 0);
				
				poiFragData.emission += emission1;
				return emission1 * _EmissionReplace1;
			}
			#endif
			//endex
			//ifex _EnableEmission2==0
			#ifdef POI_EMISSION_2
			float3 applyEmission2(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiLight poiLight, in PoiCam poiCam, in PoiMods poiMods)
			{
				
				// Second Emission
				float3 emission2 = 0;
				float emissionStrength2 = 0;
				float3 emissionColor2 = 0;
				
				emissionStrength2 = _EmissionStrength2;
				applyALEmmissionStrength(poiMods, emissionStrength2, _EmissionAL2StrengthMod, _EmissionAL2StrengthBand, _EmissionAL2Multipliers, _EmissionAL2MultipliersBand, _EmissionAL2Enabled);
				applyALCenterOutEmission(poiMods, poiLight.nDotV, emissionStrength2, _AudioLinkEmission2CenterOutSize, _AudioLinkEmission2CenterOutBand, _AudioLinkEmission2CenterOut, _EmissionAL2Enabled, _AudioLinkEmission2CenterOutDuration);
				
				float glowInTheDarkMultiplier2 = calculateGlowInTheDark(_GITDEMinLight2, _GITDEMaxLight2, _GITDEMinEmissionMultiplier2, _GITDEMaxEmissionMultiplier2, _EnableGITDEmission2, _GITDEWorldOrMesh2, poiLight);
				#if defined(PROP_EMISSIONMAP2) || !defined(OPTIMIZER_ENABLED)
				
				//UNITY_BRANCH
				if (!_EmissionCenterOutEnabled2)
				{
					emissionColor2 = POI2D_SAMPLER_PAN(_EmissionMap2, _MainTex, poiUV(poiMesh.uv[_EmissionMap2UV], _EmissionMap2_ST), _EmissionMap2Pan) * lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap2).rgb * poiThemeColor(poiMods, _EmissionColor2.rgb, _EmissionColor2ThemeIndex);
				}
				else
				{
					emissionColor2 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMap2, _MainTex, ((.5 + poiLight.nDotV * .5) * _EmissionMap2_ST.xy) + _Time.x * _EmissionCenterOutSpeed2).rgb * lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap2).rgb * poiThemeColor(poiMods, _EmissionColor2.rgb, _EmissionColor2ThemeIndex);
				}
				#else
				emissionColor2 = lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap2).rgb * poiThemeColor(poiMods, _EmissionColor2.rgb, _EmissionColor2ThemeIndex);
				#endif
				//UNITY_BRANCH
				if (_ScrollingEmission2)
				{
					float3 pos2 = poiMesh.localPos;
					//UNITY_BRANCH
					if (_EmissionScrollingVertexColor2)
					{
						pos2 = poiMesh.vertexColor.rgb;
					}
					
					//UNITY_BRANCH
					if (_EmissionScrollingUseCurve2)
					{
						#if defined(PROP_EMISSIONSCROLLINGCURVE2) || !defined(OPTIMIZER_ENABLED)
						emissionStrength2 *= UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionScrollingCurve2, _MainTex, poiUV(poiMesh.uv[_EmissionMap2UV], _EmissionScrollingCurve2_ST) + (dot(pos2, _EmissiveScroll_Direction2) * _EmissiveScroll_Interval2) + _Time.x * _EmissiveScroll_Velocity2);
						#endif
					}
					else
					{
						emissionStrength2 *= calculateScrollingEmission(_EmissiveScroll_Direction2, _EmissiveScroll_Velocity2, _EmissiveScroll_Interval2, _EmissiveScroll_Width2, _EmissionScrollingOffset2, pos2);
					}
				}
				//UNITY_BRANCH
				if (_EmissionBlinkingEnabled2)
				{
					emissionStrength2 *= calculateBlinkingEmission(_EmissiveBlink_Min2, _EmissiveBlink_Max2, _EmissiveBlink_Velocity2, _EmissionBlinkingOffset2);
				}
				
				applyLumaGradient(poiMods, emissionColor2, _EmissionColor2ThemeIndex, poiLight.nDotV);
				emissionColor2 = hueShift(emissionColor2, frac(_EmissionHueShift2 + _EmissionHueShiftSpeed2 * _Time.x) * _EmissionHueShiftEnabled2);
				#if defined(PROP_EMISSIONMASK2) || !defined(OPTIMIZER_ENABLED)
				float emissionMask2 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMask2, _MainTex, poiUV(poiMesh.uv[_EmissionMask2UV], _EmissionMask2_ST) + _Time.x * _EmissionMask2Pan)[_EmissionMask2Channel];
				#else
				float emissionMask2 = 1;
				#endif
				if (_EmissionMaskInvert2)
				{
					emissionMask2 = 1 - emissionMask2;
				}
				
				if (_EmissionMask2GlobalMask > 0)
				{
					emissionMask2 = maskBlend(emissionMask2, poiMods.globalMask[_EmissionMask2GlobalMask - 1], _EmissionMask2GlobalMaskBlendType);
				}
				emissionStrength2 *= glowInTheDarkMultiplier2 * emissionMask2;
				emission2 = max(emissionStrength2 * emissionColor2, 0);
				
				poiFragData.emission += emission2;
				return emission2 * _EmissionReplace2;
			}
			#endif
			//endex
			//ifex _EnableEmission3==0
			#ifdef POI_EMISSION_3
			float3 applyEmission3(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiLight poiLight, in PoiCam poiCam, in PoiMods poiMods)
			{
				
				// Second Emission
				float3 emission3 = 0;
				float emissionStrength3 = 0;
				float3 emissionColor3 = 0;
				
				emissionStrength3 = _EmissionStrength3;
				applyALEmmissionStrength(poiMods, emissionStrength3, _EmissionAL3StrengthMod, _EmissionAL3StrengthBand, _EmissionAL3Multipliers, _EmissionAL3MultipliersBand, _EmissionAL3Enabled);
				applyALCenterOutEmission(poiMods, poiLight.nDotV, emissionStrength3, _AudioLinkEmission3CenterOutSize, _AudioLinkEmission3CenterOutBand, _AudioLinkEmission3CenterOut, _EmissionAL3Enabled, _AudioLinkEmission3CenterOutDuration);
				
				float glowInTheDarkMultiplier3 = calculateGlowInTheDark(_GITDEMinLight3, _GITDEMaxLight3, _GITDEMinEmissionMultiplier3, _GITDEMaxEmissionMultiplier3, _EnableGITDEmission3, _GITDEWorldOrMesh3, poiLight);
				#if defined(PROP_EMISSIONMAP3) || !defined(OPTIMIZER_ENABLED)
				
				//UNITY_BRANCH
				if (!_EmissionCenterOutEnabled3)
				{
					emissionColor3 = POI2D_SAMPLER_PAN(_EmissionMap3, _MainTex, poiUV(poiMesh.uv[_EmissionMap3UV], _EmissionMap3_ST), _EmissionMap3Pan) * lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap3).rgb * poiThemeColor(poiMods, _EmissionColor3.rgb, _EmissionColor3ThemeIndex);
				}
				else
				{
					emissionColor3 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMap3, _MainTex, ((.5 + poiLight.nDotV * .5) * _EmissionMap3_ST.xy) + _Time.x * _EmissionCenterOutSpeed3).rgb * lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap3).rgb * poiThemeColor(poiMods, _EmissionColor3.rgb, _EmissionColor3ThemeIndex);
				}
				#else
				emissionColor3 = lerp(1, poiFragData.baseColor, _EmissionBaseColorAsMap3).rgb * poiThemeColor(poiMods, _EmissionColor3.rgb, _EmissionColor3ThemeIndex);
				#endif
				//UNITY_BRANCH
				if (_ScrollingEmission3)
				{
					float3 pos3 = poiMesh.localPos;
					//UNITY_BRANCH
					if (_EmissionScrollingVertexColor3)
					{
						pos3 = poiMesh.vertexColor.rgb;
					}
					
					//UNITY_BRANCH
					if (_EmissionScrollingUseCurve3)
					{
						#if defined(PROP_EMISSIONSCROLLINGCURVE3) || !defined(OPTIMIZER_ENABLED)
						emissionStrength3 *= UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionScrollingCurve3, _MainTex, poiUV(poiMesh.uv[_EmissionMap3UV], _EmissionScrollingCurve3_ST) + (dot(pos3, _EmissiveScroll_Direction3) * _EmissiveScroll_Interval3) + _Time.x * _EmissiveScroll_Velocity3);
						#endif
					}
					else
					{
						emissionStrength3 *= calculateScrollingEmission(_EmissiveScroll_Direction3, _EmissiveScroll_Velocity3, _EmissiveScroll_Interval3, _EmissiveScroll_Width3, _EmissionScrollingOffset3, pos3);
					}
				}
				//UNITY_BRANCH
				if (_EmissionBlinkingEnabled3)
				{
					emissionStrength3 *= calculateBlinkingEmission(_EmissiveBlink_Min3, _EmissiveBlink_Max3, _EmissiveBlink_Velocity3, _EmissionBlinkingOffset3);
				}
				
				applyLumaGradient(poiMods, emissionColor3, _EmissionColor3ThemeIndex, poiLight.nDotV);
				emissionColor3 = hueShift(emissionColor3, frac(_EmissionHueShift3 + _EmissionHueShiftSpeed3 * _Time.x) * _EmissionHueShiftEnabled3);
				#if defined(PROP_EMISSIONMASK3) || !defined(OPTIMIZER_ENABLED)
				float emissionMask3 = UNITY_SAMPLE_TEX2D_SAMPLER(_EmissionMask3, _MainTex, poiUV(poiMesh.uv[_EmissionMask3UV], _EmissionMask3_ST) + _Time.x * _EmissionMask3Pan)[_EmissionMask3Channel];
				#else
				float emissionMask3 = 1;
				#endif
				
				if (_EmissionMaskInvert3)
				{
					emissionMask3 = 1 - emissionMask3;
				}
				
				if (_EmissionMask3GlobalMask > 0)
				{
					emissionMask3 = maskBlend(emissionMask3, poiMods.globalMask[_EmissionMask3GlobalMask - 1], _EmissionMask3GlobalMaskBlendType);
				}
				emissionStrength3 *= glowInTheDarkMultiplier3 * emissionMask3;
				emission3 = max(emissionStrength3 * emissionColor3, 0);
				
				poiFragData.emission += emission3;
				return emission3 * _EmissionReplace3;
			}
			#endif
			//endex
			
			//ifex _GlitterEnable==0
			#ifdef _SUNDISK_SIMPLE
			
			float3 RandomColorFromPoint(float2 rando)
			{
				fixed hue = random2(rando.x + rando.y).x;
				fixed saturation = lerp(_GlitterMinMaxSaturation.x, _GlitterMinMaxSaturation.y, rando.x);
				fixed value = lerp(_GlitterMinMaxBrightness.x, _GlitterMinMaxBrightness.y, rando.y);
				float3 hsv = float3(hue, saturation, value);
				return HSVtoRGB(hsv);
			}
			
			void applyGlitter(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiLight poiLight, in PoiMods poiMods)
			{
				for (int glitterLayer = 0; glitterLayer < _GlitterLayers; glitterLayer++)
				{
					
					// Scale
					
					float2 st = (poiMesh.uv[_GlitterUV] + _GlitterUVPanning.xy * _Time.x) * _GlitterFrequency;
					
					// Tile the space
					float2 i_st = floor(st);
					float2 f_st = frac(st);
					
					float m_dist = 10.;  // minimun distance
					float2 m_point = 0;        // minimum point
					float2 randoPoint = 0;
					float2 dank = 0;
					for (int j = -1; j <= 1; j++)
					{
						for (int i = -1; i <= 1; i++)
						{
							float2 neighbor = float2(i, j);
							float2 pos = random2(i_st + neighbor + glitterLayer * 0.5141);
							float2 rando = pos;
							pos = pos * _GlitterRandomLocation;
							float2 diff = neighbor + pos - f_st;
							float dist = length(diff);
							
							if (dist < m_dist)
							{
								dank = diff;
								m_dist = dist;
								m_point = pos;
								randoPoint = rando;
							}
						}
					}
					
					float randomFromPoint = random(randoPoint);
					
					float size = _GlitterSize;
					UNITY_BRANCH
					if (_GlitterRandomSize)
					{
						size = lerp(_GlitterMinMaxSize.x, _GlitterMinMaxSize.y, randomFromPoint);
					}
					
					// Assign a color using the closest point position
					//color += dot(m_point, float2(.3, .6));
					
					// Add distance field to closest point center
					// color.g = m_dist;
					
					// Show isolines
					//color -= abs(sin(40.0 * m_dist)) * 0.07;
					
					// Draw cell center
					half glitterAlpha = 1;
					switch(_GlitterShape)
					{
						case 0: //circle
						glitterAlpha = saturate((size - m_dist) / clamp(fwidth(m_dist), 0.0001, 1.0));
						break;
						case 1: //sqaure
						float jaggyFix = pow(poiCam.distanceToVert, 2) * _GlitterJaggyFix;
						UNITY_BRANCH
						if (_GlitterRandomRotation == 1 || _GlitterTextureRotation != 0 || _GlitterRandomRotationSpeed.x != 0 || _GlitterRandomRotationSpeed.y != 0)
						{
							float2 center = float2(0, 0);
							float randomBoy = 0;
							UNITY_BRANCH
							if (_GlitterRandomRotation || _GlitterRandomRotationSpeed.x != 0 || _GlitterRandomRotationSpeed.y != 0)
							{
								randomBoy = random(m_point * 200);
							}
							float theta = radians((randomBoy + _Time.x * (_GlitterTextureRotation + lerp(_GlitterRandomRotationSpeed.x, _GlitterRandomRotationSpeed.y, randomBoy))) * 360);
							float cs = cos(theta);
							float sn = sin(theta);
							dank = float2((dank.x - center.x) * cs - (dank.y - center.y) * sn + center.x, (dank.x - center.x) * sn + (dank.y - center.y) * cs + center.y);
							glitterAlpha = (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.x))) * (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.y)));
						}
						else
						{
							glitterAlpha = (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.x))) * (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.y)));
						}
						break;
					}
					
					float3 finalGlitter = 0;
					
					half3 glitterColor = poiThemeColor(poiMods, _GlitterColor, _GlitterColorThemeIndex);
					
					float3 norm = lerp(poiMesh.normals[0], poiMesh.normals[1], _GlitterUseNormals);
					float3 randomRotation = 0;
					switch(_GlitterMode)
					{
						case 0:
						UNITY_BRANCH
						if (_GlitterSpeed > 0)
						{
							randomRotation = randomFloat3WiggleRange(randoPoint, _GlitterAngleRange, _GlitterSpeed);
						}
						else
						{
							randomRotation = randomFloat3Range(randoPoint, _GlitterAngleRange);
						}
						
						float3 glitterReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
						finalGlitter = lerp(0, _GlitterMinBrightness * glitterAlpha, glitterAlpha) + max(pow(saturate(dot(lerp(glitterReflectionDirection, poiCam.viewDir, _GlitterBias), poiCam.viewDir)), _GlitterContrast), 0);
						finalGlitter *= glitterAlpha;
						break;
						case 1:
						float randomOffset = random(randoPoint);
						float brightness = (sin((_Time.x * 10 + randomOffset) * _GlitterSpeed) * .5 + .5);
						finalGlitter = max(_GlitterMinBrightness * glitterAlpha, brightness * glitterAlpha * smoothstep(0, 1, 1 - m_dist * _GlitterCenterSize * 10));
						break;
						case 2:
						if (_GlitterSpeed > 0)
						{
							randomRotation = randomFloat3WiggleRange(randoPoint, _GlitterAngleRange, _GlitterSpeed);
						}
						else
						{
							randomRotation = randomFloat3Range(randoPoint, _GlitterAngleRange);
						}
						
						float3 glitterLightReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
						
						#ifdef POI_PASS_ADD
						glitterAlpha *= poiLight.nDotLSaturated * poiLight.attenuation;
						#endif
						#ifdef UNITY_PASS_FORWARDBASE
						glitterAlpha *= poiLight.nDotLSaturated;
						#endif
						
						float3 halfDir = normalize(poiLight.direction + poiCam.viewDir);
						float specAngle = max(dot(halfDir, glitterLightReflectionDirection), 0.0);
						
						finalGlitter = lerp(0, _GlitterMinBrightness * glitterAlpha, glitterAlpha) + max(pow(specAngle, _GlitterContrast), 0);
						
						glitterColor *= poiLight.directColor;
						finalGlitter *= glitterAlpha;
						
						break;
					}
					
					glitterColor *= lerp(1, poiFragData.baseColor, _GlitterUseSurfaceColor);
					#if defined(PROP_GLITTERCOLORMAP) || !defined(OPTIMIZER_ENABLED)
					glitterColor *= POI2D_SAMPLER_PAN(_GlitterColorMap, _MainTex, poiUV(poiMesh.uv[_GlitterColorMapUV], _GlitterColorMap_ST), _GlitterColorMapPan).rgb;
					#endif
					float2 uv = remapClamped(-size, size, dank, 0, 1);
					UNITY_BRANCH
					
					if (_GlitterRandomRotation == 1 || _GlitterTextureRotation != 0 || _GlitterRandomRotationSpeed.x != 0 || _GlitterRandomRotationSpeed.y && !_GlitterShape)
					{
						float2 fakeUVCenter = float2(.5, .5);
						float randomBoy = 0;
						UNITY_BRANCH
						if (_GlitterRandomRotation || _GlitterRandomRotationSpeed.x != 0 || _GlitterRandomRotationSpeed.y != 0)
						{
							randomBoy = random(randoPoint * 20);
						}
						float theta = radians((randomBoy + _Time.x * (_GlitterTextureRotation + lerp(_GlitterRandomRotationSpeed.x, _GlitterRandomRotationSpeed.y, randomBoy))) * 360);
						float cs = cos(theta);
						float sn = sin(theta);
						uv = float2((uv.x - fakeUVCenter.x) * cs - (uv.y - fakeUVCenter.y) * sn + fakeUVCenter.x, (uv.x - fakeUVCenter.x) * sn + (uv.y - fakeUVCenter.y) * cs + fakeUVCenter.y);
					}
					
					#if defined(PROP_GLITTERTEXTURE) || !defined(OPTIMIZER_ENABLED)
					float4 glitterTexture = POI2D_SAMPLER_PANGRAD(_GlitterTexture, _linear_clamp, poiUV(uv, _GlitterTexture_ST), _GlitterTexturePan, poiMesh.dx, poiMesh.dy);
					#else
					float4 glitterTexture = 1;
					#endif
					//float4 glitterTexture = _GlitterTexture.SampleGrad(sampler_MainTex, frac(uv), ddx(uv), ddy(uv));
					glitterColor *= glitterTexture.rgb;
					#if defined(PROP_GLITTERMASK) || !defined(OPTIMIZER_ENABLED)
					float glitterMask = POI2D_SAMPLER_PAN(_GlitterMask, _MainTex, poiUV(poiMesh.uv[_GlitterMaskUV], _GlitterMask_ST), _GlitterMaskPan)[_GlitterMaskChannel];
					#else
					float glitterMask = 1;
					#endif
					
					if (_GlitterMaskInvert)
					{
						glitterMask = 1 - glitterMask;
					}
					
					glitterMask *= lerp(1, poiLight.rampedLightMap, _GlitterHideInShadow);
					glitterMask *= lerp(1, poiLight.directLuminance, _GlitterScaleWithLighting);
					
					if (_GlitterMaskGlobalMask > 0)
					{
						glitterMask = maskBlend(glitterMask, poiMods.globalMask[_GlitterMaskGlobalMask - 1], _GlitterMaskGlobalMaskBlendType);
					}
					
					if (_GlitterRandomColors)
					{
						glitterColor *= RandomColorFromPoint(random2(randoPoint.x + randoPoint.y));
					}
					
					UNITY_BRANCH
					if (_GlitterHueShiftEnabled)
					{
						glitterColor.rgb = hueShift(glitterColor.rgb, _GlitterHueShift + _Time.x * _GlitterHueShiftSpeed);
					}
					
					UNITY_BRANCH
					if (_GlitterBlendType == 1)
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, finalGlitter * glitterColor * _GlitterBrightness, finalGlitter * glitterTexture.a * glitterMask);
						poiFragData.emission += finalGlitter * glitterColor * max(0, (_GlitterBrightness - 1) * glitterTexture.a) * glitterMask;
					}
					else
					{
						poiFragData.emission += finalGlitter * glitterColor * _GlitterBrightness * glitterTexture.a * glitterMask;
					}
				}
			}
			#endif
			//endex
			
			//ifex _PoiInternalParallax==0
			#ifdef POI_INTERNALPARALLAX
			void applyInternalParallax(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiMods poiMods)
			{
				float3 parallax = 0;
				
				for (int j = _ParallaxInternalIterations - 1; j >= 0; j--)
				{
					float ratio = saturate((float)j / max(_ParallaxInternalIterations - 1, 1));
					float2 parallaxOffset = _Time.y * (_ParallaxInternalMapPan + ratio * _ParallaxInternalPanDepthSpeed);
					float fade = lerp(_ParallaxInternalMinFade, _ParallaxInternalMaxFade, ratio);
					fade = pow(fade, 2.2);
					#if defined(PROP_PARALLAXINTERNALMAP) || !defined(OPTIMIZER_ENABLED)
					float4 parallaxColor = UNITY_SAMPLE_TEX2D_SAMPLER(_ParallaxInternalMap, _MainTex, TRANSFORM_TEX(poiMesh.uv[0], _ParallaxInternalMap) + (lerp(_ParallaxInternalMinDepth, _ParallaxInternalMaxDepth, ratio)) * - (poiCam.tangentViewDir.xy / (poiCam.tangentViewDir.z)) + parallaxOffset);
					#else
					float4 parallaxColor = 0;
					#endif
					float3 minColor = poiThemeColor(poiMods, _ParallaxInternalMinColor.rgb, _ParallaxInternalMinColorThemeIndex);
					float3 maxColor = poiThemeColor(poiMods, _ParallaxInternalMaxColor.rgb, _ParallaxInternalMaxColorThemeIndex);
					float3 parallaxTint = lerp(minColor, maxColor, ratio);
					float parallaxHeight;
					if (_ParallaxInternalHeightFromAlpha)
					{
						parallaxTint *= parallaxColor.rgb;
						parallaxHeight = parallaxColor.a;
					}
					else
					{
						parallaxHeight = parallaxColor.r;
					}
					// parallaxTint = hueShift(parallaxTint, frac((ratio * _ParallaxInternalHueShiftPerLevel) + (ratio * _ParallaxInternalHueShiftPerLevelSpeed * _Time.x)) * _ParallaxInternalHueShiftEnabled);
					parallaxTint = hueShift(parallaxTint, frac(ratio * _ParallaxInternalHueShiftPerLevel) * _ParallaxInternalHueShiftEnabled);
					//float parallaxColor *= lerp(_ParallaxInternalMinColor, _ParallaxInternalMaxColor, 1 - ratio);
					UNITY_BRANCH
					if (_ParallaxInternalHeightmapMode == 1)
					{
						parallax = lerp(parallax, parallaxTint * fade, parallaxHeight >= 1 - ratio);
					}
					else
					{
						if (_ParallaxInternalBlendMode == 0) parallax += parallaxTint * parallaxHeight * fade;
						if (_ParallaxInternalBlendMode == 1) parallax = max(parallax, parallaxTint * parallaxHeight * fade);
					}
				}
				parallax = hueShift(parallax, frac(_ParallaxInternalHueShift + _ParallaxInternalHueShiftSpeed * _Time.x) * _ParallaxInternalHueShiftEnabled);
				//parallax /= _ParallaxInternalIterations;
				#if defined(PROP_PARALLAXINTERNALMAPMASK) || !defined(OPTIMIZER_ENABLED)
				poiFragData.baseColor.rgb = customBlend(poiFragData.baseColor.rgb, parallax.rgb, _ParallaxInternalSurfaceBlendMode, POI2D_SAMPLER_PAN(_ParallaxInternalMapMask, _MainTex, poiUV(poiMesh.uv[_ParallaxInternalMapMaskUV], _ParallaxInternalMapMask_ST), _ParallaxInternalMapMaskPan)[_ParallaxInternalMapMaskChannel]);
				#else
				poiFragData.baseColor.rgb = customBlend(poiFragData.baseColor.rgb, parallax.rgb, _ParallaxInternalSurfaceBlendMode);
				#endif
			}
			#endif
			//endex
			
			void CalcThryRF(inout PoiFragData poiFragData, inout PoiMods poiMods, in PoiMesh poiMesh)
			{
				// _ReactivePositions[0] = float4(5.26, 0, -6.11, 1);
				// _ReactivePositions[1] = float4(4.46, 0, -6.11, 1);
				// _RF_ArrayLength = 2;
				
				float totalHue = 0;
				float totalStrength = 0.000001;
				float maxLerpStrength = 0;
				
				float totalHueBackground = 0;
				float totalStrengthBackground = 0.000001;
				float maxLerpStrengthBackground = 0;
				for (int i = 0; i < _RF_ArrayLength; i++) {
					if (length(_ReactivePositions[i].xyz) > 0) {
						float normalizedDistance = (distance(poiMesh.worldPos, _ReactivePositions[i].xyz) - _RF_Min_Distance) / (_RF_Max_Distance - _RF_Min_Distance);
						
						// float4 circlecolor;
						// int colorI = (i % 6);
						// if (colorI == 0) circlecolor = _RF_Color0;
						// else if (colorI == 1) circlecolor = _RF_Color1;
						// else if (colorI == 2) circlecolor = _RF_Color2;
						// else if (colorI == 3) circlecolor = _RF_Color3;
						// else if (colorI == 4) circlecolor = _RF_Color4;
						// else if (colorI == 5) circlecolor = _RF_Color5;
						// else if (colorI == 6) circlecolor = _RF_Color6;
						// else if (colorI == 7) circlecolor = _RF_Color7;
						// float3 hsv = RGBtoHSV(circlecolor.rgb);
						float3 hsvFG = RGBtoHSV(_ReactiveColors1[i].rgb);
						float3 hsvBG = RGBtoHSV(_ReactiveColors2[i].rgb);
						
						if(_ReactiveSpecial[i].x > 0.5)
						{
							float delta = _Time.x * (_ReactiveSpecial[i].y - 0.5) * 10;
							hsvFG.x = fmod(hsvFG.x + delta, 1);
							hsvBG.x = fmod(hsvBG.x + delta, 1);
						}
						
						float v = 1 - saturate(normalizedDistance);
						if (v > 0.0001) {
							float mask = UNITY_SAMPLE_TEX2D_SAMPLER(_RF_Map, _MainTex, float2(v + 0.01, 0.5)).a;
							float ramp = UNITY_SAMPLE_TEX2D_SAMPLER(_RF_Map, _MainTex, _RF_Map_ST.x * float2(v + _RF_Ramp_Pan * _Time.x, 0.5)).r;
							
							// float lerpValue = ramp * mask * circlecolor.a;
							float lerpValue = ramp * mask;
							
							totalHue += hsvFG.x * mask;
							totalStrength += mask;
							maxLerpStrength = max(maxLerpStrength, lerpValue);
						}
						
						float backgroundLerp = 1 - remapClamped(1.1, 1.1, normalizedDistance);
						totalHueBackground += hsvBG.x * backgroundLerp;
						totalStrengthBackground += backgroundLerp;
						maxLerpStrengthBackground = max(maxLerpStrengthBackground, backgroundLerp);
					}
				}
				
				// colorshift the background to the same hue
				totalHueBackground = totalHueBackground / totalStrengthBackground;
				float3 bgHSV = RGBtoHSV(_ParallaxInternalMaxColor.rgb);
				bgHSV.x = lerp(bgHSV.x, totalHueBackground, maxLerpStrengthBackground);
				poiMods.globalColorTheme[1] = float4(HSVtoRGB(bgHSV), 1);
				
				totalHue = totalHue / totalStrength;
				float3 totalColor = HSVtoRGB(float3(totalHue, 1, 1));
				poiMods.globalColorTheme[0] = float4(totalColor, maxLerpStrength);
			}
			
			void ApplyThryRF(inout PoiFragData poiFragData, inout PoiMods poiMods)
			{
				poiFragData.finalColor.rgb = lerp(poiFragData.finalColor.rgb, poiMods.globalColorTheme[0].rgb, poiMods.globalColorTheme[0].a);
			}
			
			float4 frag(VertexOut i, uint facing : SV_IsFrontFace) : SV_Target
			/*
			#ifdef
			, out float depth : SV_DEPTH
			#endif
			*/
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				
				PoiMesh poiMesh;
				PoiInitStruct(PoiMesh, poiMesh);
				
				PoiLight poiLight;
				PoiInitStruct(PoiLight, poiLight);
				
				PoiVertexLights poiVertexLights;
				PoiInitStruct(PoiVertexLights, poiVertexLights);
				
				PoiCam poiCam;
				PoiInitStruct(PoiCam, poiCam);
				
				PoiMods poiMods;
				PoiInitStruct(PoiMods, poiMods);
				poiMods.globalEmission = 1;
				
				PoiFragData poiFragData;
				poiFragData.smoothness = 1;
				poiFragData.smoothness2 = 1;
				poiFragData.metallic = 1;
				poiFragData.specularMask = 1;
				poiFragData.reflectionMask = 1;
				poiFragData.emission = 0;
				poiFragData.baseColor = float3(0, 0, 0);
				poiFragData.finalColor = float3(0, 0, 0);
				poiFragData.alpha = 1;
				poiFragData.toggleVertexLights = 0;
				
				// Mesh Data
				poiMesh.objectPosition = mul(unity_ObjectToWorld, float3(0, 0, 0)).xyz;
				poiMesh.objNormal = mul(unity_WorldToObject, i.normal);
				poiMesh.normals[0] = i.normal;
				poiMesh.tangent[0] = i.tangent.xyz;
				poiMesh.binormal[0] = cross(i.normal, i.tangent.xyz) * (i.tangent.w * unity_WorldTransformParams.w);
				poiMesh.worldPos = i.worldPos.xyz;
				poiMesh.localPos = i.localPos.xyz;
				poiMesh.vertexColor = i.vertexColor;
				poiMesh.isFrontFace = facing;
				poiMesh.dx = ddx(poiMesh.uv[0]);
				poiMesh.dy = ddy(poiMesh.uv[0]);
				
				#ifndef POI_PASS_OUTLINE
				if (!poiMesh.isFrontFace && _FlipBackfaceNormals)
				{
					poiMesh.normals[0] *= -1;
					poiMesh.tangent[0] *= -1;
					poiMesh.binormal[0] *= -1;
				}
				#endif
				
				poiCam.viewDir = !IsOrthographicCamera() ? normalize(_WorldSpaceCameraPos - i.worldPos.xyz) : normalize(UNITY_MATRIX_I_V._m02_m12_m22);
				float3 tanToWorld0 = float3(poiMesh.tangent[0].x, poiMesh.binormal[0].x, poiMesh.normals[0].x);
				float3 tanToWorld1 = float3(poiMesh.tangent[0].y, poiMesh.binormal[0].y, poiMesh.normals[0].y);
				float3 tanToWorld2 = float3(poiMesh.tangent[0].z, poiMesh.binormal[0].z, poiMesh.normals[0].z);
				float3 ase_tanViewDir = tanToWorld0 * poiCam.viewDir.x + tanToWorld1 * poiCam.viewDir.y + tanToWorld2 * poiCam.viewDir.z;
				poiCam.tangentViewDir = normalize(ase_tanViewDir);
				
				// 0-3 UV0-UV3
				// 4 Panosphere UV
				// 5 world pos xz
				// 6 Polar UV
				// 6 Distorted UV
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
				poiMesh.lightmapUV = i.lightmapUV;
				#endif
				poiMesh.parallaxUV = poiCam.tangentViewDir.xy / max(poiCam.tangentViewDir.z, 0.0001);
				poiMesh.uv[0] = i.uv[0].xy;
				poiMesh.uv[1] = i.uv[0].zw;
				poiMesh.uv[2] = i.uv[1].xy;
				poiMesh.uv[3] = i.uv[1].zw;
				poiMesh.uv[4] = poiMesh.uv[0];
				poiMesh.uv[5] = poiMesh.uv[0];
				poiMesh.uv[6] = poiMesh.uv[0];
				poiMesh.uv[7] = poiMesh.uv[0];
				poiMesh.uv[8] = poiMesh.uv[0];
				
				//ifex _PoiParallax==0
				#ifdef POI_PARALLAX
				#ifndef POI_PASS_OUTLINE
				//return frac(i.tangentViewDir.x);
				//return float4(i.binormal.xyz,1);
				applyParallax(poiMesh, poiLight, poiCam);
				#endif
				#endif
				//endex
				
				float2 mainUV = poiUV(poiMesh.uv[_MainTexUV].xy, _MainTex_ST);
				
				if (_MainPixelMode)
				{
					mainUV = sharpSample(_MainTex_TexelSize, mainUV);
				}
				
				float4 mainTexture = POI2D_SAMPLER_PAN_STOCHASTIC(_MainTex, _MainTex, mainUV, _MainTexPan, _MainTexStochastic);
				
				#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
				poiMesh.tangentSpaceNormal = UnpackScaleNormal(POI2D_SAMPLER_PAN_STOCHASTIC(_BumpMap, _MainTex, poiUV(poiMesh.uv[_BumpMapUV].xy, _BumpMap_ST), _BumpMapPan, _BumpMapStochastic), _BumpScale);
				#else
				poiMesh.tangentSpaceNormal = UnpackNormal(float4(0.5, 0.5, 1, 1));
				#endif
				
				poiMesh.normals[1] = normalize(
				poiMesh.tangentSpaceNormal.x * poiMesh.tangent[0] +
				poiMesh.tangentSpaceNormal.y * poiMesh.binormal[0] +
				poiMesh.tangentSpaceNormal.z * poiMesh.normals[0]
				);
				
				poiMesh.tangent[1] = cross(poiMesh.binormal[0], -poiMesh.normals[1]);
				poiMesh.binormal[1] = cross(-poiMesh.normals[1], poiMesh.tangent[0]);
				
				// Camera data
				poiCam.forwardDir = getCameraForward();
				poiCam.worldPos = _WorldSpaceCameraPos;
				poiCam.reflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[1]);
				poiCam.vertexReflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[0]);
				//poiCam.distanceToModel = distance(poiMesh.modelPos, poiCam.worldPos);
				poiCam.clipPos = i.pos;
				poiCam.distanceToVert = distance(poiMesh.worldPos, poiCam.worldPos);
				poiCam.posScreenSpace = poiTransformClipSpacetoScreenSpaceFrag(poiCam.clipPos);
				#if defined(POI_GRABPASS) && defined(POI_PASS_BASE)
				poiCam.screenUV = poiCam.clipPos.xy / poiGetWidthAndHeight(_PoiGrab2);
				#endif
				poiCam.posScreenPixels = calcPixelScreenUVs(poiCam.posScreenSpace);
				poiCam.vDotN = abs(dot(poiCam.viewDir, poiMesh.normals[1]));
				
				poiCam.worldDirection.xyz = poiMesh.worldPos.xyz - poiCam.worldPos;
				poiCam.worldDirection.w = dot(poiCam.clipPos, CalculateFrustumCorrection());
				
				poiLight.finalLightAdd = 0;
				
				// Ambient Occlusion
				#if defined(PROP_LIGHTINGAOMAPS) || !defined(OPTIMIZER_ENABLED)
				float4 AOMaps = POI2D_SAMPLER_PAN(_LightingAOMaps, _MainTex, poiUV(poiMesh.uv[_LightingAOMapsUV], _LightingAOMaps_ST), _LightingAOMapsPan);
				poiLight.occlusion = min(min(min(lerp(1, AOMaps.r, _LightDataAOStrengthR), lerp(1, AOMaps.g, _LightDataAOStrengthG)), lerp(1, AOMaps.b, _LightDataAOStrengthB)), lerp(1, AOMaps.a, _LightDataAOStrengthA));
				#else
				poiLight.occlusion = 1;
				#endif
				
				if (_LightDataAOGlobalMaskR > 0)
				{
					poiLight.occlusion = maskBlend(poiLight.occlusion, poiMods.globalMask[_LightDataAOGlobalMaskR - 1], _LightDataAOGlobalMaskBlendTypeR);
				}
				
				// Detail Shadows
				#if defined(PROP_LIGHTINGDETAILSHADOWMAPS) || !defined(OPTIMIZER_ENABLED)
				float4 DetailShadows = POI2D_SAMPLER_PAN(_LightingDetailShadowMaps, _MainTex, poiUV(poiMesh.uv[_LightingDetailShadowMapsUV], _LightingDetailShadowMaps_ST), _LightingDetailShadowMapsPan);
				#ifndef POI_PASS_ADD
				poiLight.detailShadow = lerp(1, DetailShadows.r, _LightingDetailShadowStrengthR) * lerp(1, DetailShadows.g, _LightingDetailShadowStrengthG) * lerp(1, DetailShadows.b, _LightingDetailShadowStrengthB) * lerp(1, DetailShadows.a, _LightingDetailShadowStrengthA);
				#else
				poiLight.detailShadow = lerp(1, DetailShadows.r, _LightingAddDetailShadowStrengthR) * lerp(1, DetailShadows.g, _LightingAddDetailShadowStrengthG) * lerp(1, DetailShadows.b, _LightingAddDetailShadowStrengthB) * lerp(1, DetailShadows.a, _LightingAddDetailShadowStrengthA);
				#endif
				#else
				poiLight.detailShadow = 1;
				#endif
				
				if (_LightDataDetailShadowGlobalMaskR > 0)
				{
					poiLight.detailShadow = maskBlend(poiLight.detailShadow, poiMods.globalMask[_LightDataDetailShadowGlobalMaskR - 1], _LightDataDetailShadowGlobalMaskBlendTypeR);
				}
				
				// Shadow Masks
				#if defined(PROP_LIGHTINGSHADOWMASKS) || !defined(OPTIMIZER_ENABLED)
				float4 ShadowMasks = POI2D_SAMPLER_PAN(_LightingShadowMasks, _MainTex, poiUV(poiMesh.uv[_LightingShadowMasksUV], _LightingShadowMasks_ST), _LightingShadowMasksPan);
				poiLight.shadowMask = lerp(1, ShadowMasks.r, _LightingShadowMaskStrengthR) * lerp(1, ShadowMasks.g, _LightingShadowMaskStrengthG) * lerp(1, ShadowMasks.b, _LightingShadowMaskStrengthB) * lerp(1, ShadowMasks.a, _LightingShadowMaskStrengthA);
				#else
				poiLight.shadowMask = 1;
				#endif
				if (_LightDataShadowMaskGlobalMaskR > 0)
				{
					poiLight.shadowMask = maskBlend(poiLight.shadowMask, poiMods.globalMask[_LightDataShadowMaskGlobalMaskR - 1], _LightDataShadowMaskGlobalMaskBlendTypeR);
				}
				
				#ifdef UNITY_PASS_FORWARDBASE
				
				bool lightExists = false;
				if (any(_LightColor0.rgb >= 0.002))
				{
					lightExists = true;
				}
				
				if (_LightingVertexLightingEnabled)
				{
					poiFragData.toggleVertexLights = 1;
				}
				if (IsInMirror() && _LightingMirrorVertexLightingEnabled == 0)
				{
					poiFragData.toggleVertexLights = 0;
				}
				
				if (_LightingVertexLightingEnabled)
				{
					#if defined(VERTEXLIGHT_ON)
					float4 toLightX = unity_4LightPosX0 - i.worldPos.x;
					float4 toLightY = unity_4LightPosY0 - i.worldPos.y;
					float4 toLightZ = unity_4LightPosZ0 - i.worldPos.z;
					float4 lengthSq = 0;
					lengthSq += toLightX * toLightX;
					lengthSq += toLightY * toLightY;
					lengthSq += toLightZ * toLightZ;
					
					float4 lightAttenSq = unity_4LightAtten0;
					float4 atten = 1.0 / (1.0 + lengthSq * lightAttenSq);
					float4 vLightWeight = saturate(1 - (lengthSq * lightAttenSq / 25));
					poiLight.vAttenuation = min(atten, vLightWeight * vLightWeight);
					
					poiLight.vDotNL = 0;
					poiLight.vDotNL += toLightX * poiMesh.normals[1].x;
					poiLight.vDotNL += toLightY * poiMesh.normals[1].y;
					poiLight.vDotNL += toLightZ * poiMesh.normals[1].z;
					
					float4 corr = rsqrt(lengthSq);
					poiLight.vertexVDotNL = max(0, poiLight.vDotNL * corr);
					
					poiLight.vertexVDotNL = 0;
					poiLight.vertexVDotNL += toLightX * poiMesh.normals[0].x;
					poiLight.vertexVDotNL += toLightY * poiMesh.normals[0].y;
					poiLight.vertexVDotNL += toLightZ * poiMesh.normals[0].z;
					
					poiLight.vertexVDotNL = max(0, poiLight.vDotNL * corr);
					
					poiLight.vAttenuationDotNL = saturate(poiLight.vAttenuation * saturate(poiLight.vDotNL));
					
					[unroll]
					for (int index = 0; index < 4; index++)
					{
						poiLight.vPosition[index] = float3(unity_4LightPosX0[index], unity_4LightPosY0[index], unity_4LightPosZ0[index]);
						
						float3 vertexToLightSource = poiLight.vPosition[index] - poiMesh.worldPos;
						poiLight.vDirection[index] = normalize(vertexToLightSource);
						//poiLight.vAttenuationDotNL[index] = 1.0 / (1.0 + unity_4LightAtten0[index] * poiLight.vDotNL[index]);
						poiLight.vColor[index] = _LightingAdditiveLimited ? min(_LightingAdditiveLimit, unity_LightColor[index].rgb) : unity_LightColor[index].rgb;
						poiLight.vColor[index] = lerp(poiLight.vColor[index], dot(poiLight.vColor[index], float3(0.299, 0.587, 0.114)), _LightingAdditiveMonochromatic);
						poiLight.vHalfDir[index] = Unity_SafeNormalize(poiLight.vDirection[index] + poiCam.viewDir);
						poiLight.vDotNL[index] = dot(poiMesh.normals[1], poiLight.vDirection[index]);
						poiLight.vCorrectedDotNL[index] = .5 * (poiLight.vDotNL[index] + 1);
						poiLight.vDotLH[index] = saturate(dot(poiLight.vDirection[index], poiLight.vHalfDir[index]));
						
						poiLight.vDotNH[index] = dot(poiMesh.normals[1], poiLight.vHalfDir[index]);
						poiLight.vertexVDotNH[index] = saturate(dot(poiMesh.normals[0], poiLight.vHalfDir[index]));
					}
					#endif
				}
				
				//UNITY_BRANCH
				if (_LightingColorMode == 0) // Poi Custom Light Color
				
				{
					float3 magic = max(BetterSH9(normalize(unity_SHAr + unity_SHAg + unity_SHAb)), 0);
					float3 normalLight = _LightColor0.rgb + BetterSH9(float4(0, 0, 0, 1));
					
					float magiLumi = calculateluminance(magic);
					float normaLumi = calculateluminance(normalLight);
					float maginormalumi = magiLumi + normaLumi;
					
					float magiratio = magiLumi / maginormalumi;
					float normaRatio = normaLumi / maginormalumi;
					
					float target = calculateluminance(magic * magiratio + normalLight * normaRatio);
					float3 properLightColor = magic + normalLight;
					float properLuminance = calculateluminance(magic + normalLight);
					poiLight.directColor = properLightColor * max(0.0001, (target / properLuminance));
					
					poiLight.indirectColor = BetterSH9(float4(lerp(0, poiMesh.normals[1], _LightingIndirectUsesNormals), 1));
				}
				
				//UNITY_BRANCH
				if (_LightingColorMode == 1) // More standard approach to light color
				
				{
					float3 indirectColor = BetterSH9(float4(poiMesh.normals[1], 1));
					if (lightExists)
					{
						poiLight.directColor = _LightColor0.rgb;
						poiLight.indirectColor = indirectColor;
					}
					else
					{
						poiLight.directColor = indirectColor * 0.6;
						poiLight.indirectColor = indirectColor * 0.5;
					}
				}
				
				if (_LightingColorMode == 2) // UTS style
				
				{
					poiLight.indirectColor = saturate(max(half3(0.05, 0.05, 0.05) * _Unlit_Intensity, max(ShadeSH9(half4(0.0, 0.0, 0.0, 1.0)), ShadeSH9(half4(0.0, -1.0, 0.0, 1.0)).rgb) * _Unlit_Intensity));
					poiLight.directColor = max(poiLight.indirectColor, _LightColor0.rgb);
				}
				
				if (_LightingColorMode == 3) // OpenLit
				
				{
					float3 lightDirectionForSH9 = OpenLitLightingDirectionForSH9();
					OpenLitShadeSH9ToonDouble(lightDirectionForSH9, poiLight.directColor, poiLight.indirectColor);
					poiLight.directColor += _LightColor0.rgb;
					// OpenLit does a few other things by default like clamp direct colour
					// see https://github.com/lilxyzw/OpenLit/blob/main/Assets/OpenLit/core.hlsl#L174
					// Should we add (if) statements to override Poi versions of these things?
					// Or should we add new properties with same names with the same functions?
					
				}
				
				float lightMapMode = _LightingMapMode;
				//UNITY_BRANCH
				if (_LightingDirectionMode == 0)
				{
					poiLight.direction = _WorldSpaceLightPos0.xyz + unity_SHAr.xyz + unity_SHAg.xyz + unity_SHAb.xyz;
				}
				if (_LightingDirectionMode == 1 || _LightingDirectionMode == 2)
				{
					//UNITY_BRANCH
					if (_LightingDirectionMode == 1)
					{
						poiLight.direction = mul(unity_ObjectToWorld, _LightngForcedDirection).xyz;;
					}
					//UNITY_BRANCH
					if (_LightingDirectionMode == 2)
					{
						poiLight.direction = _LightngForcedDirection;
					}
					if (lightMapMode == 0)
					{
						lightMapMode == 1;
					}
				}
				
				if (_LightingDirectionMode == 3) // UTS
				
				{
					float3 defaultLightDirection = normalize(UNITY_MATRIX_V[2].xyz + UNITY_MATRIX_V[1].xyz);
					float3 lightDirection = normalize(lerp(defaultLightDirection, _WorldSpaceLightPos0.xyz, any(_WorldSpaceLightPos0.xyz)));
					poiLight.direction = lightDirection;
				}
				if (_LightingDirectionMode == 4) // OpenLit
				
				{
					poiLight.direction = OpenLitLightingDirection(); // float4 customDir = 0; // Do we want to give users to alter this (OpenLit always does!)?
					
				}
				
				if (_LightingDirectionMode == 5) // View Direction
				
				{
					float3 upViewDir = normalize(UNITY_MATRIX_V[1].xyz);
					float3 rightViewDir = normalize(UNITY_MATRIX_V[0].xyz);
					float yawOffset_Rads = radians(!IsInMirror() ? - _LightingViewDirOffsetYaw : _LightingViewDirOffsetYaw);
					float3 rotatedViewYaw = normalize(RotateAroundAxis(rightViewDir, upViewDir, yawOffset_Rads));
					float3 rotatedViewCameraMeshOffset = RotateAroundAxis((getCameraPosition() - (poiMesh.worldPos)), upViewDir, yawOffset_Rads);
					float pitchOffset_Rads = radians(!IsInMirror() ? _LightingViewDirOffsetPitch : - _LightingViewDirOffsetPitch);
					float3 rotatedViewPitch = RotateAroundAxis(rotatedViewCameraMeshOffset, rotatedViewYaw, pitchOffset_Rads);
					poiLight.direction = normalize(rotatedViewPitch);
				}
				
				if (!any(poiLight.direction))
				{
					poiLight.direction = float3(.4, 1, .4);
				}
				
				poiLight.direction = normalize(poiLight.direction);
				poiLight.attenuationStrength = _LightingCastedShadows;
				poiLight.attenuation = 1;
				if (!all(_LightColor0.rgb == 0.0))
				{
					UNITY_LIGHT_ATTENUATION(attenuation, i, poiMesh.worldPos)
					poiLight.attenuation *= attenuation;
				}
				
				if (!any(poiLight.directColor) && !any(poiLight.indirectColor) && lightMapMode == 0)
				{
					lightMapMode = 1;
					if (_LightingDirectionMode == 0)
					{
						poiLight.direction = normalize(float3(.4, 1, .4));
					}
				}
				
				poiLight.halfDir = normalize(poiLight.direction + poiCam.viewDir);
				poiLight.vertexNDotL = dot(poiMesh.normals[0], poiLight.direction);
				poiLight.nDotL = dot(poiMesh.normals[1], poiLight.direction);
				poiLight.nDotLSaturated = saturate(poiLight.nDotL);
				poiLight.nDotLNormalized = (poiLight.nDotL + 1) * 0.5;
				poiLight.nDotV = abs(dot(poiMesh.normals[1], poiCam.viewDir));
				poiLight.vertexNDotV = abs(dot(poiMesh.normals[0], poiCam.viewDir));
				poiLight.nDotH = dot(poiMesh.normals[1], poiLight.halfDir);
				poiLight.vertexNDotH = max(0.00001, dot(poiMesh.normals[0], poiLight.halfDir));
				poiLight.lDotv = dot(poiLight.direction, poiCam.viewDir);
				poiLight.lDotH = max(0.00001, dot(poiLight.direction, poiLight.halfDir));
				
				// Poi special light map
				if (lightMapMode == 0)
				{
					float3 ShadeSH9Plus = GetSHLength();
					float3 ShadeSH9Minus = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w) + float3(unity_SHBr.z, unity_SHBg.z, unity_SHBb.z) / 3.0;
					
					float3 greyScaleVector = float3(.33333, .33333, .33333);
					float bw_lightColor = dot(poiLight.directColor, greyScaleVector);
					float bw_directLighting = (((poiLight.nDotL * 0.5 + 0.5) * bw_lightColor * lerp(1, poiLight.attenuation, poiLight.attenuationStrength)) + dot(ShadeSH9(float4(poiMesh.normals[1], 1)), greyScaleVector));
					float bw_directLightingNoAtten = (((poiLight.nDotL * 0.5 + 0.5) * bw_lightColor) + dot(ShadeSH9(float4(poiMesh.normals[1], 1)), greyScaleVector));
					float bw_bottomIndirectLighting = dot(ShadeSH9Minus, greyScaleVector);
					float bw_topIndirectLighting = dot(ShadeSH9Plus, greyScaleVector);
					float lightDifference = ((bw_topIndirectLighting + bw_lightColor) - bw_bottomIndirectLighting);
					
					poiLight.lightMap = smoothstep(0, lightDifference, bw_directLighting - bw_bottomIndirectLighting);
					poiLight.lightMapNoAttenuation = smoothstep(0, lightDifference, bw_directLightingNoAtten - bw_bottomIndirectLighting);
				}
				// Normalized nDotL
				if (lightMapMode == 1)
				{
					poiLight.lightMapNoAttenuation = poiLight.nDotLNormalized;
					poiLight.lightMap = poiLight.nDotLNormalized * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				// Saturated nDotL
				if (lightMapMode == 2)
				{
					poiLight.lightMapNoAttenuation = poiLight.nDotLSaturated;
					poiLight.lightMap = poiLight.nDotLSaturated * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				if (lightMapMode == 3)
				{
					poiLight.lightMapNoAttenuation = 1;
					poiLight.lightMap = lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				poiLight.lightMapNoAttenuation *= poiLight.detailShadow;
				poiLight.lightMap *= poiLight.detailShadow;
				
				poiLight.directColor = max(poiLight.directColor, 0.0001);
				poiLight.indirectColor = max(poiLight.indirectColor, 0.0001);
				if (_LightingColorMode == 3)
				{
					// OpenLit
					poiLight.directColor = max(poiLight.directColor, _LightingMinLightBrightness);
				}
				else
				{
					poiLight.directColor = max(poiLight.directColor, poiLight.directColor * min(10000, (_LightingMinLightBrightness * rcp(calculateluminance(poiLight.directColor)))));
					poiLight.indirectColor = max(poiLight.indirectColor, poiLight.indirectColor * min(10000, (_LightingMinLightBrightness * rcp(calculateluminance(poiLight.indirectColor)))));
				}
				
				poiLight.directColor = lerp(poiLight.directColor, dot(poiLight.directColor, float3(0.299, 0.587, 0.114)), _LightingMonochromatic);
				poiLight.indirectColor = lerp(poiLight.indirectColor, dot(poiLight.indirectColor, float3(0.299, 0.587, 0.114)), _LightingMonochromatic);
				
				if (_LightingCapEnabled)
				{
					poiLight.directColor = min(poiLight.directColor, _LightingCap);
					poiLight.indirectColor = min(poiLight.indirectColor, _LightingCap);
				}
				
				if (_LightingForceColorEnabled)
				{
					poiLight.directColor = poiThemeColor(poiMods, _LightingForcedColor, _LightingForcedColorThemeIndex);
				}
				
				#endif
				
				#ifdef POI_PASS_ADD
				if (!_LightingAdditiveEnable)
				{
					return float4(mainTexture.rgb * .0001, 1);
				}
				
				#if defined(DIRECTIONAL)
				if (_DisableDirectionalInAdd)
				{
					return float4(mainTexture.rgb * .0001, 1);
				}
				#endif
				
				poiLight.direction = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz * _WorldSpaceLightPos0.w);
				#if defined(POINT) || defined(SPOT)
				#ifdef POINT
				unityShadowCoord3 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(poiMesh.worldPos, 1)).xyz;
				poiLight.attenuation = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).r;
				#endif
				
				#ifdef SPOT
				unityShadowCoord4 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(poiMesh.worldPos, 1));
				poiLight.attenuation = (lightCoord.z > 0) * UnitySpotCookie(lightCoord) * UnitySpotAttenuate(lightCoord.xyz);
				#endif
				#else
				UNITY_LIGHT_ATTENUATION(attenuation, i, poiMesh.worldPos)
				poiLight.attenuation = attenuation;
				#endif
				poiLight.additiveShadow = UNITY_SHADOW_ATTENUATION(i, poiMesh.worldPos);
				poiLight.attenuationStrength = _LightingAdditiveCastedShadows;
				poiLight.directColor = _LightingAdditiveLimited ? MaxLuminance(_LightColor0.rgb, _LightingAdditiveLimit) : _LightColor0.rgb;
				
				#if defined(POINT_COOKIE) || defined(DIRECTIONAL_COOKIE)
				poiLight.indirectColor = 0;
				#else
				poiLight.indirectColor = lerp(0, poiLight.directColor, _LightingAdditivePassthrough);
				poiLight.indirectColor = _LightingAdditiveLimited ? MaxLuminance(poiLight.indirectColor, _LightingAdditiveLimit) : poiLight.indirectColor;
				#endif
				
				poiLight.directColor = lerp(poiLight.directColor, dot(poiLight.directColor, float3(0.299, 0.587, 0.114)), _LightingAdditiveMonochromatic);
				poiLight.indirectColor = lerp(poiLight.indirectColor, dot(poiLight.indirectColor, float3(0.299, 0.587, 0.114)), _LightingAdditiveMonochromatic);
				
				poiLight.halfDir = normalize(poiLight.direction + poiCam.viewDir);
				poiLight.nDotL = dot(poiMesh.normals[1], poiLight.direction);
				poiLight.nDotLSaturated = saturate(poiLight.nDotL);
				poiLight.nDotLNormalized = (poiLight.nDotL + 1) * 0.5;
				poiLight.nDotV = abs(dot(poiMesh.normals[1], poiCam.viewDir));
				poiLight.nDotH = dot(poiMesh.normals[1], poiLight.halfDir);
				poiLight.lDotv = dot(poiLight.direction, poiCam.viewDir);
				poiLight.lDotH = dot(poiLight.direction, poiLight.halfDir);
				poiLight.vertexNDotL = dot(poiMesh.normals[0], poiLight.direction);
				poiLight.vertexNDotV = abs(dot(poiMesh.normals[0], poiCam.viewDir));
				poiLight.vertexNDotH = max(0.00001, dot(poiMesh.normals[0], poiLight.halfDir));
				
				// Normalized nDotL
				if (_LightingMapMode == 0 || _LightingMapMode == 1 || _LightingMapMode == 2)
				{
					poiLight.lightMap = poiLight.nDotLNormalized;
				}
				if (_LightingMapMode == 3)
				{
					poiLight.lightMap = 1;
				}
				poiLight.lightMap *= poiLight.detailShadow;
				poiLight.lightMapNoAttenuation = poiLight.lightMap;
				poiLight.lightMap *= lerp(1, poiLight.additiveShadow, poiLight.attenuationStrength);
				#endif
				
				//ifex _LightDataDebugEnabled==0
				if (_LightDataDebugEnabled)
				{
					#ifdef UNITY_PASS_FORWARDBASE
					//UNITY_BRANCH
					if (_LightingDebugVisualize <= 6)
					{
						switch(_LightingDebugVisualize)
						{
							case 0: // Direct Light Color
							return float4(poiLight.directColor + mainTexture.rgb * .0001, 1);
							break;
							case 1: // Indirect Light Color
							return float4(poiLight.indirectColor + mainTexture.rgb * .0001, 1);
							break;
							case 2: // Light Map
							return float4(poiLight.lightMap + mainTexture.rgb * .0001, 1);
							break;
							case 3: // Attenuation
							return float4(poiLight.attenuation + mainTexture.rgb * .0001, 1);
							break;
							case 4: // N Dot L
							return float4(poiLight.nDotLNormalized, poiLight.nDotLNormalized, poiLight.nDotLNormalized, 1) + mainTexture * .0001;
							break;
							case 5:
							return float4(poiLight.halfDir, 1) + mainTexture * .0001;
							break;
							case 6:
							return float4(poiLight.direction, 1) + mainTexture * .0001;
							break;
						}
					}
					else
					{
						return POI_SAFE_RGB1;
					}
					#endif
					#ifdef POI_PASS_ADD
					//UNITY_BRANCH
					if (_LightingDebugVisualize < 6)
					{
						return POI_SAFE_RGB1;
					}
					else
					{
						switch(_LightingDebugVisualize)
						{
							case 7:
							return float4(poiLight.directColor * poiLight.attenuation + mainTexture.rgb * .0001, 1);
							break;
							case 8:
							return float4(poiLight.attenuation + mainTexture.rgb * .0001, 1);
							break;
							case 9:
							return float4(poiLight.additiveShadow + mainTexture.rgb * .0001, 1);
							break;
							case 10:
							return float4(poiLight.nDotLNormalized + mainTexture.rgb * .0001, 1);
							break;
							case 11:
							return float4(poiLight.halfDir, 1) + mainTexture * .0001;
							break;
						}
					}
					#endif
				}
				//endex
				
				CalcThryRF(poiFragData, poiMods, poiMesh);
				
				CalcThryRF(poiFragData, poiMods, poiMesh);
				
				poiFragData.baseColor = mainTexture.rgb * poiThemeColor(poiMods, _Color.rgb, _ColorThemeIndex);
				poiFragData.alpha = mainTexture.a * _Color.a;
				
				//ifex _MainColorAdjustToggle==0
				#ifdef COLOR_GRADING_HDR
				#if defined(PROP_MAINCOLORADJUSTTEXTURE) || !defined(OPTIMIZER_ENABLED)
				float4 hueShiftAlpha = POI2D_SAMPLER_PAN(_MainColorAdjustTexture, _MainTex, poiUV(poiMesh.uv[_MainColorAdjustTextureUV], _MainColorAdjustTexture_ST), _MainColorAdjustTexturePan);
				#else
				float4 hueShiftAlpha = 1;
				#endif
				
				if (_MainHueGlobalMask > 0)
				{
					hueShiftAlpha.r = maskBlend(hueShiftAlpha.r, poiMods.globalMask[_MainHueGlobalMask - 1], _MainHueGlobalMaskBlendType);
				}
				if (_MainSaturationGlobalMask > 0)
				{
					hueShiftAlpha.b = maskBlend(hueShiftAlpha.b, poiMods.globalMask[_MainSaturationGlobalMask - 1], _MainSaturationGlobalMaskBlendType);
				}
				if (_MainBrightnessGlobalMask > 0)
				{
					hueShiftAlpha.g = maskBlend(hueShiftAlpha.g, poiMods.globalMask[_MainBrightnessGlobalMask - 1], _MainBrightnessGlobalMaskBlendType);
				}
				
				if (_MainHueShiftToggle)
				{
					float shift = _MainHueShift;
					#ifdef POI_AUDIOLINK
					//UNITY_BRANCH
					if (poiMods.audioLinkAvailable && _MainHueALCTEnabled)
					{
						shift += AudioLinkGetChronoTime(_MainALHueShiftCTIndex, _MainALHueShiftBand) * _MainHueALMotionSpeed;
					}
					#endif
					if (_MainHueShiftReplace)
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, hueShift(poiFragData.baseColor, shift + _MainHueShiftSpeed * _Time.x), hueShiftAlpha.r);
					}
					else
					{
						poiFragData.baseColor = hueShift(poiFragData.baseColor, frac((shift - (1 - hueShiftAlpha.r) + _MainHueShiftSpeed * _Time.x)));
					}
				}
				
				#if defined(PROP_MAINGRADATIONTEX) || !defined(OPTIMIZER_ENABLED)
				
				if (_MainGradationStrength && _ColorGradingToggle)
				{
					#if !defined(UNITY_COLORSPACE_GAMMA)
					float3 tempColor = OpenLitLinearToSRGB(poiFragData.baseColor);
					#endif
					tempColor.r = POI_SAMPLE_1D_X(_MainGradationTex, sampler_linear_clamp, tempColor.r).r;
					tempColor.g = POI_SAMPLE_1D_X(_MainGradationTex, sampler_linear_clamp, tempColor.g).g;
					tempColor.b = POI_SAMPLE_1D_X(_MainGradationTex, sampler_linear_clamp, tempColor.b).b;
					#if !defined(UNITY_COLORSPACE_GAMMA)
					tempColor = OpenLitSRGBToLinear(tempColor);
					#endif
					poiFragData.baseColor = lerp(poiFragData.baseColor, tempColor, _MainGradationStrength);
				}
				#endif
				
				poiFragData.baseColor = lerp(poiFragData.baseColor, dot(poiFragData.baseColor, float3(0.3, 0.59, 0.11)), - (_Saturation) * hueShiftAlpha.b);
				poiFragData.baseColor = saturate(lerp(poiFragData.baseColor, poiFragData.baseColor * (_MainBrightness + 1), hueShiftAlpha.g));
				#endif
				//endex
				
				#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
				
				if (_AlphaMaskMode)
				{
					float alphaMask = POI2D_SAMPLER_PAN(_AlphaMask, _MainTex, poiUV(poiMesh.uv[_AlphaMaskUV], _AlphaMask_ST), _AlphaMaskPan.xy).r;
					alphaMask = saturate(alphaMask * _AlphaMaskScale + _AlphaMaskValue);
					if (_AlphaMaskInvert) alphaMask = 1 - alphaMask;
					if (_AlphaMaskMode == 1) poiFragData.alpha = alphaMask;
					if (_AlphaMaskMode == 2) poiFragData.alpha = poiFragData.alpha * alphaMask;
					if (_AlphaMaskMode == 3) poiFragData.alpha = saturate(poiFragData.alpha + alphaMask);
					if (_AlphaMaskMode == 4) poiFragData.alpha = saturate(poiFragData.alpha - alphaMask);
				}
				#endif
				
				applyAlphaOptions(poiFragData, poiMesh, poiCam, poiMods);
				
				//ifex _GlitterEnable==0
				#ifdef _SUNDISK_SIMPLE
				applyGlitter(poiFragData, poiMesh, poiCam, poiLight, poiMods);
				#endif
				//endex
				
				//ifex _PoiInternalParallax==0
				#ifdef POI_INTERNALPARALLAX
				applyInternalParallax(poiFragData, poiMesh, poiCam, poiMods);
				#endif
				//endex
				
				UNITY_BRANCH
				if (_AlphaPremultiply)
				{
					poiFragData.baseColor *= saturate(poiFragData.alpha);
				}
				poiFragData.finalColor = poiFragData.baseColor;
				
				//ifex _EnableEmission==0 && _EnableEmission1==0 && _EnableEmission2==0 && _EnableEmission3==0
				#if defined(_EMISSION) || defined(POI_EMISSION_1) || defined(POI_EMISSION_2) || defined(POI_EMISSION_3)
				float3 emissionBaseReplace = 0;
				#endif
				//endex
				
				//ifex _EnableEmission==0
				#ifdef _EMISSION
				emissionBaseReplace += applyEmission(poiFragData, poiMesh, poiLight, poiCam, poiMods);
				#endif
				//endex
				//ifex _EnableEmission1==0
				#ifdef POI_EMISSION_1
				emissionBaseReplace += applyEmission1(poiFragData, poiMesh, poiLight, poiCam, poiMods);
				#endif
				//endex
				//ifex _EnableEmission2==0
				#ifdef POI_EMISSION_2
				emissionBaseReplace += applyEmission2(poiFragData, poiMesh, poiLight, poiCam, poiMods);
				#endif
				//endex
				//ifex _EnableEmission3==0
				#ifdef POI_EMISSION_3
				emissionBaseReplace += applyEmission3(poiFragData, poiMesh, poiLight, poiCam, poiMods);
				#endif
				//endex
				
				//ifex _EnableEmission==0 && _EnableEmission1==0 && _EnableEmission2==0 && _EnableEmission3==0
				#if defined(_EMISSION) || defined(POI_EMISSION_1) || defined(POI_EMISSION_2) || defined(POI_EMISSION_3)
				poiFragData.finalColor.rgb = lerp(poiFragData.finalColor.rgb, saturate(emissionBaseReplace), poiMax(emissionBaseReplace));
				#endif
				//endex
				
				ApplyThryRF(poiFragData, poiMods);
				
				//UNITY_BRANCH
				if (_IgnoreFog == 0)
				{
					UNITY_APPLY_FOG(i.fogCoord, poiFragData.finalColor);
				}
				
				poiFragData.alpha = _AlphaForceOpaque ? 1 : poiFragData.alpha;
				
				//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
				ApplyAlphaToCoverage(poiFragData, poiMesh);
				//endex
				
				//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
				applyDithering(poiFragData, poiCam);
				//endex
				
				if (_Mode == POI_MODE_OPAQUE)
				{
					//poiFragData.alpha = 1;
				}
				
				clip(poiFragData.alpha - _Cutoff);
				
				if (_Mode == POI_MODE_CUTOUT && !_AlphaToCoverage)
				{
					poiFragData.alpha = 1;
				}
				
				return float4(poiFragData.finalColor + poiFragData.emission * poiMods.globalEmission, poiFragData.alpha) + POI_SAFE_RGB0;
			}
			
			ENDCG
		}
		
		Pass
		{
			Name "Add"
			Tags { "LightMode" = "ForwardAdd" }
			
			ZWrite Off
			Cull [_Cull]
			
			AlphaToMask [_AlphaToCoverage]
			ZTest [_ZTest]
			ColorMask [_ColorMask]
			Offset [_OffsetFactor], [_OffsetUnits]
			
			BlendOp [_AddBlendOp], [_AddBlendOpAlpha]
			Blend [_AddSrcBlend] [_AddDstBlend], [_AddSrcBlendAlpha] [_AddDstBlendAlpha]
			
			CGPROGRAM
			/*
			// Disable warnings we aren't interested in
			#if defined(UNITY_COMPILER_HLSL)
			#pragma warning(disable : 3205) // conversion of larger type to smaller
			#pragma warning(disable : 3568) // unknown pragma ignored
			#pragma warning(disable : 3571) // "pow(f,e) will not work for negative f"; however in majority of our calls to pow we know f is not negative
			#pragma warning(disable : 3206) // implicit truncation of vector type
			#endif
			*/
			#pragma target 5.0
			//ifex 0==0
			#pragma skip_optimizations d3d11
			//endex
			
			#pragma shader_feature_local _STOCHASTICMODE_DELIOT_HEITZ _STOCHASTICMODE_HEXTILE _STOCHASTICMODE_NONE
			
			//ifex _MainColorAdjustToggle==0
			#pragma shader_feature COLOR_GRADING_HDR
			//endex
			
			//#pragma shader_feature KEYWORD
			
			#pragma skip_variants LIGHTMAP_ON DYNAMICLIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SHADOWMASK DIRLIGHTMAP_COMBINED _MIXED_LIGHTING_SUBTRACTIVE
			#pragma skip_variants DECALS_OFF DECALS_3RT DECALS_4RT DECAL_SURFACE_GRADIENT _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma skip_variants _ADDITIONAL_LIGHT_SHADOWS
			#pragma skip_variants PROBE_VOLUMES_OFF PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
			#pragma skip_variants _SCREEN_SPACE_OCCLUSION
			
			//ifex _PoiParallax==0
			#pragma shader_feature_local POI_PARALLAX
			//endex
			
			//ifex _EnableEmission==0
			#pragma shader_feature _EMISSION
			//endex
			//ifex _EnableEmission1==0
			#pragma shader_feature_local POI_EMISSION_1
			//endex
			//ifex _EnableEmission2==0
			#pragma shader_feature_local POI_EMISSION_2
			//endex
			//ifex _EnableEmission3==0
			#pragma shader_feature_local POI_EMISSION_3
			//endex
			
			//ifex _GlitterEnable==0
			#pragma shader_feature _SUNDISK_SIMPLE
			//endex
			
			//ifex _PoiInternalParallax==0
			#pragma shader_feature_local POI_INTERNALPARALLAX
			//endex
			
			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_instancing
			#pragma multi_compile_fog
			#define POI_PASS_ADD
			
			// UNITY Includes
			#include "UnityCG.cginc"
			#include "UnityStandardUtils.cginc"
			#include "AutoLight.cginc"
			#include "UnityLightingCommon.cginc"
			#include "UnityPBSLighting.cginc"
			#ifdef POI_PASS_META
			#include "UnityMetaPass.cginc"
			#endif
			#pragma vertex vert
			
			#pragma fragment frag
			
			#define DielectricSpec float4(0.04, 0.04, 0.04, 1.0 - 0.04)
			#define PI float(3.14159265359)
			
			#define POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, samplertex, coord, dx, dy) tex.SampleGrad(sampler##samplertex, coord, dx, dy)
			#define POI2D_SAMPLE_TEX2D_SAMPLERGRADD(tex, samp, uv, pan, dx, dy) tex.SampleGrad(samp, POI_PAN_UV(uv, pan), dx, dy)
			
			#define POI_PAN_UV(uv, pan) (uv + _Time.x * pan)
			#define POI2D_SAMPLER_PAN(tex, texSampler, uv, pan) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, POI_PAN_UV(uv, pan)))
			#define POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, POI_PAN_UV(uv, pan), dx, dy))
			#define POI2D_SAMPLER(tex, texSampler, uv) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, uv))
			#define POI_SAMPLE_1D_X(tex, samp, uv) tex.Sample(samp, float2(uv, 0.5))
			#define POI2D_SAMPLER_GRAD(tex, texSampler, uv, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, uv, dx, dy))
			#define POI2D_SAMPLER_GRADD(tex, texSampler, uv, dx, dy) tex.SampleGrad(texSampler, uv, dx, dy)
			#define POI2D_PAN(tex, uv, pan) (tex2D(tex, POI_PAN_UV(uv, pan)))
			#define POI2D(tex, uv) (tex2D(tex, uv))
			#define POI_SAMPLE_TEX2D(tex, uv) (UNITY_SAMPLE_TEX2D(tex, uv))
			#define POI_SAMPLE_TEX2D_PAN(tex, uv, pan) (UNITY_SAMPLE_TEX2D(tex, POI_PAN_UV(uv, pan)))
			#define POI_SAMPLE_CUBE_LOD(tex, samp, uv, lod) texCUBElod(tex, float4(uv, 0, lod))
			
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define TEXTURE2D_SCREEN(tex)                   Texture2DArray tex
			#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, float3(uv, unity_StereoEyeIndex))
			#else
			#define TEXTURE2D_SCREEN(tex)                   Texture2D tex
			#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, uv)
			#endif
			
			// When using, properties won't properly lock at optimize time; needs macro evaluation implemented
			// #define POI2D_MAINTEX_SAMPLER_PAN_INLINED(tex, poiMesh) (POI2D_SAMPLER_PAN(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan))
			
			#define POI_SAFE_RGB0 float4(mainTexture.rgb * .0001, 0)
			#define POI_SAFE_RGB1 float4(mainTexture.rgb * .0001, 1)
			#define POI_SAFE_RGBA mainTexture
			
			#if defined(UNITY_COMPILER_HLSL)
			#define PoiInitStruct(type, name) name = (type)0;
			#else
			#define PoiInitStruct(type, name)
			#endif
			
			#define POI_ERROR(poiMesh, gridSize) lerp(float3(1, 0, 1), float3(0, 0, 0), fmod(floor((poiMesh.worldPos.x) * gridSize) + floor((poiMesh.worldPos.y) * gridSize) + floor((poiMesh.worldPos.z) * gridSize), 2) == 0)
			#define POI_NAN (asfloat(-1))
			
			#define POI_MODE_OPAQUE 0
			#define POI_MODE_CUTOUT 1
			#define POI_MODE_FADE 2
			#define POI_MODE_TRANSPARENT 3
			#define POI_MODE_ADDITIVE 4
			#define POI_MODE_SOFTADDITIVE 5
			#define POI_MODE_MULTIPLICATIVE 6
			#define POI_MODE_2XMULTIPLICATIVE 7
			#define POI_MODE_TRANSCLIPPING 9
			
			/*
			Texture2D ;
			float4 _ST;
			float2 Pan;
			float UV;
			float Stochastic;
			
			[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos XZ, 5, Polar UV, 6, Distorted UV, 7 )]
			*/
			
			float _GrabMode;
			float _Mode;
			
			float _StochasticDeliotHeitzDensity;
			float _StochasticHexGridDensity;
			float _StochasticHexRotationStrength;
			float _StochasticHexFallOffContrast;
			float _StochasticHexFallOffPower;
			
			#if defined(PROP_LIGHTINGAOMAPS) || !defined(OPTIMIZER_ENABLED)
			Texture2D _LightingAOMaps;
			#endif
			float4 _LightingAOMaps_ST;
			float2 _LightingAOMapsPan;
			float _LightingAOMapsUV;
			float _LightDataAOStrengthR;
			float _LightDataAOStrengthG;
			float _LightDataAOStrengthB;
			float _LightDataAOStrengthA;
			float _LightDataAOGlobalMaskR;
			float _LightDataAOGlobalMaskBlendTypeR;
			
			#if defined(PROP_LIGHTINGDETAILSHADOWMAPS) || !defined(OPTIMIZER_ENABLED)
			Texture2D _LightingDetailShadowMaps;
			#endif
			float4 _LightingDetailShadowMaps_ST;
			float2 _LightingDetailShadowMapsPan;
			float _LightingDetailShadowMapsUV;
			float _LightingDetailShadowStrengthR;
			float _LightingDetailShadowStrengthG;
			float _LightingDetailShadowStrengthB;
			float _LightingDetailShadowStrengthA;
			float _LightingAddDetailShadowStrengthR;
			float _LightingAddDetailShadowStrengthG;
			float _LightingAddDetailShadowStrengthB;
			float _LightingAddDetailShadowStrengthA;
			float _LightDataDetailShadowGlobalMaskR;
			float _LightDataDetailShadowGlobalMaskBlendTypeR;
			
			#if defined(PROP_LIGHTINGSHADOWMASKS) || !defined(OPTIMIZER_ENABLED)
			Texture2D _LightingShadowMasks;
			#endif
			float4 _LightingShadowMasks_ST;
			float2 _LightingShadowMasksPan;
			float _LightingShadowMasksUV;
			float _LightingShadowMaskStrengthR;
			float _LightingShadowMaskStrengthG;
			float _LightingShadowMaskStrengthB;
			float _LightingShadowMaskStrengthA;
			float _LightDataShadowMaskGlobalMaskR;
			float _LightDataShadowMaskGlobalMaskBlendTypeR;
			
			// Lighting Data
			float _Unlit_Intensity;
			float _LightingColorMode;
			float _LightingMapMode;
			float _LightingDirectionMode;
			float3 _LightngForcedDirection;
			float _LightingViewDirOffsetPitch;
			float _LightingViewDirOffsetYaw;
			float _LightingIndirectUsesNormals;
			float _LightingCapEnabled;
			float _LightingCap;
			float _LightingForceColorEnabled;
			float3 _LightingForcedColor;
			float _LightingForcedColorThemeIndex;
			float _LightingCastedShadows;
			float _LightingMonochromatic;
			float _LightingMinLightBrightness;
			// Additive Lighting Data
			float _LightingAdditiveEnable;
			float _LightingAdditiveLimited;
			float _LightingAdditiveLimit;
			float _LightingAdditiveCastedShadows;
			float _LightingAdditiveMonochromatic;
			float _LightingAdditivePassthrough;
			float _DisableDirectionalInAdd;
			float _LightingVertexLightingEnabled;
			float _LightingMirrorVertexLightingEnabled;
			// Lighting Data Debug
			float _LightDataDebugEnabled;
			float _LightingDebugVisualize;
			
			float _IgnoreFog;
			float _RenderingReduceClipDistance;
			int _FlipBackfaceNormals;
			float _AddBlendOp;
			float _Cull;
			
			float4 _Color;
			float _ColorThemeIndex;
			UNITY_DECLARE_TEX2D(_MainTex);
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
			float _MainPixelMode;
			float4 _MainTex_ST;
			float2 _MainTexPan;
			float _MainTexUV;
			float4 _MainTex_TexelSize;
			float _MainTexStochastic;
			#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _BumpMap;
			#endif
			float4 _BumpMap_ST;
			float2 _BumpMapPan;
			float _BumpMapUV;
			float _BumpScale;
			float _BumpMapStochastic;
			#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _AlphaMask;
			float4 _AlphaMask_ST;
			float2 _AlphaMaskPan;
			float _AlphaMaskUV;
			float _AlphaMaskInvert;
			float _AlphaMaskMode;
			float _AlphaMaskScale;
			float _AlphaMaskValue;
			#endif
			float _Cutoff;
			//ifex _MainColorAdjustToggle==0
			float _MainColorAdjustToggle;
			#if defined(PROP_MAINCOLORADJUSTTEXTURE) || !defined(OPTIMIZER_ENABLED)
			Texture2D _MainColorAdjustTexture;
			#endif
			float4 _MainColorAdjustTexture_ST;
			float2 _MainColorAdjustTexturePan;
			float _MainColorAdjustTextureUV;
			float _MainHueShiftToggle;
			float _MainHueShiftReplace;
			float _MainHueShift;
			float _MainHueShiftSpeed;
			float _Saturation;
			float _MainBrightness;
			
			float _MainHueALCTEnabled;
			float _MainALHueShiftBand;
			float _MainALHueShiftCTIndex;
			float _MainHueALMotionSpeed;
			
			float _MainHueGlobalMask;
			float _MainHueGlobalMaskBlendType;
			float _MainSaturationGlobalMask;
			float _MainSaturationGlobalMaskBlendType;
			float _MainBrightnessGlobalMask;
			float _MainBrightnessGlobalMaskBlendType;
			
			#if defined(PROP_MAINGRADATIONTEX) || !defined(OPTIMIZER_ENABLED)
			Texture2D _MainGradationTex;
			#endif
			float _ColorGradingToggle;
			float _MainGradationStrength;
			//endex
			
			SamplerState sampler_linear_clamp;
			SamplerState sampler_linear_repeat;
			SamplerState sampler_trilinear_repeat;
			
			float _AlphaForceOpaque;
			float _AlphaMod;
			float _AlphaPremultiply;
			float _AlphaBoostFA;
			//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
			float _AlphaToCoverage;
			float _AlphaSharpenedA2C;
			float _AlphaMipScale;
			//endex
			
			//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
			float _AlphaDithering;
			float _AlphaDitherGradient;
			float _AlphaDitherBias;
			//endex
			
			//ifex _AlphaDistanceFade==0 && isNotAnimated(_AlphaDistanceFade)
			float _AlphaDistanceFade;
			float _AlphaDistanceFadeType;
			float _AlphaDistanceFadeMinAlpha;
			float _AlphaDistanceFadeMaxAlpha;
			float _AlphaDistanceFadeMin;
			float _AlphaDistanceFadeMax;
			float _AlphaDistanceFadeGlobalMask;
			float _AlphaDistanceFadeGlobalMaskBlendType;
			//endex
			
			//ifex _AlphaFresnel==0 && isNotAnimated(_AlphaFresnel)
			float _AlphaFresnel;
			float _AlphaFresnelAlpha;
			float _AlphaFresnelSharpness;
			float _AlphaFresnelWidth;
			float _AlphaFresnelInvert;
			float _AlphaFresnelGlobalMask;
			float _AlphaFresnelGlobalMaskBlendType;
			//endex
			
			//ifex _AlphaAngular==0 && isNotAnimated(_AlphaAngular)
			float _AlphaAngular;
			float _AngleType;
			float _AngleCompareTo;
			float3 _AngleForwardDirection;
			float _CameraAngleMin;
			float _CameraAngleMax;
			float _ModelAngleMin;
			float _ModelAngleMax;
			float _AngleMinAlpha;
			float _AlphaAngularGlobalMask;
			float _AlphaAngularGlobalMaskBlendType;
			//endex
			
			//ifex _AlphaAudioLinkEnabled==0 && isNotAnimated(_AlphaAudioLinkEnabled)
			float _AlphaAudioLinkEnabled;
			float2 _AlphaAudioLinkAddRange;
			float _AlphaAudioLinkAddBand;
			//endex
			
			float _AlphaGlobalMask;
			float _AlphaGlobalMaskBlendType;
			
			//ifex _PoiParallax==0
			#ifdef POI_PARALLAX
			
			sampler2D _HeightMap;
			float4 _HeightMap_ST;
			float2 _HeightMapPan;
			float _HeightMapUV;
			
			#if defined(PROP_HEIGHTMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _Heightmask;
			float4 _Heightmask_ST;
			float2 _HeightmaskPan;
			float _HeightmaskUV;
			float _HeightmaskChannel;
			float _HeightmaskInvert;
			SamplerState _linear_repeat;
			#endif
			
			float _ParallaxUV;
			float _HeightStrength;
			float _HeightOffset;
			float _HeightStepsMin;
			float _HeightStepsMax;
			
			float _CurvatureU;
			float _CurvatureV;
			float _CurvFix;
			#endif
			//endex
			
			//ifex _GlitterEnable==0
			#ifdef _SUNDISK_SIMPLE
			float4 _GlitterRandomRotationSpeed;
			float _GlitterLayers;
			float _GlitterUseNormals;
			float _GlitterUV;
			half3 _GlitterColor;
			float _GlitterColorThemeIndex;
			float2 _GlitterPan;
			half _GlitterSpeed;
			half _GlitterBrightness;
			float _GlitterFrequency;
			float _GlitterRandomLocation;
			half _GlitterSize;
			half _GlitterContrast;
			half _GlitterAngleRange;
			half _GlitterMinBrightness;
			half _GlitterBias;
			fixed _GlitterUseSurfaceColor;
			float _GlitterBlendType;
			float _GlitterMode;
			float _GlitterShape;
			float _GlitterCenterSize;
			float _GlitterJaggyFix;
			float _GlitterTextureRotation;
			float2 _GlitterUVPanning;
			
			float _GlitterHueShiftEnabled;
			float _GlitterHueShiftSpeed;
			float _GlitterHueShift;
			float _GlitterHideInShadow;
			float _GlitterScaleWithLighting;
			
			float _GlitterRandomColors;
			float2 _GlitterMinMaxSaturation;
			float2 _GlitterMinMaxBrightness;
			float _GlitterRandomSize;
			float4 _GlitterMinMaxSize;
			float _GlitterRandomRotation;
			
			#if defined(PROP_GLITTERMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterMask;
			#endif
			float4 _GlitterMask_ST;
			float2 _GlitterMaskPan;
			float _GlitterMaskUV;
			float _GlitterMaskChannel;
			float _GlitterMaskInvert;
			float _GlitterMaskGlobalMask;
			float _GlitterMaskGlobalMaskBlendType;
			#if defined(PROP_GLITTERCOLORMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterColorMap;
			#endif
			float4 _GlitterColorMap_ST;
			float2 _GlitterColorMapPan;
			float _GlitterColorMapUV;
			#if defined(PROP_GLITTERTEXTURE) || !defined(OPTIMIZER_ENABLED)
			Texture2D _GlitterTexture;
			#endif
			float4 _GlitterTexture_ST;
			float2 _GlitterTexturePan;
			float _GlitterTextureUV;
			#endif
			//endex
			
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				uint vertexId : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct VertexOut
			{
				float4 pos : SV_POSITION;
				float4 uv[2] : TEXCOORD0;
				float3 normal : TEXCOORD2;
				float4 tangent : TEXCOORD3;
				float4 worldPos : TEXCOORD4;
				float4 localPos : TEXCOORD5;
				float4 vertexColor : TEXCOORD6;
				float4 lightmapUV : TEXCOORD7;
				float2 fogCoord: TEXCOORD10;
				UNITY_SHADOW_COORDS(11)
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			struct PoiMesh
			{
				
				// 0 Vertex normal
				// 1 Fragment normal
				float3 normals[2];
				float3 objNormal;
				float3 tangentSpaceNormal;
				float3 binormal[2];
				float3 tangent[2];
				float3 worldPos;
				float3 localPos;
				float3 objectPosition;
				float isFrontFace;
				float4 vertexColor;
				float4 lightmapUV;
				// 0-3 UV0-UV3
				// 4 Panosphere UV
				// 5 world pos xz
				// 6 Polar UV
				// 7 Distorted UV
				float2 uv[9];
				float2 parallaxUV;
				float2 dx;
				float2 dy;
			};
			
			struct PoiCam
			{
				float3 viewDir;
				float3 forwardDir;
				float3 worldPos;
				float distanceToVert;
				float4 clipPos;
				float4 screenSpacePosition;
				float3 reflectionDir;
				float3 vertexReflectionDir;
				float3 tangentViewDir;
				float4 posScreenSpace;
				float2 posScreenPixels;
				float2 screenUV;
				float vDotN;
				float4 worldDirection;
				
			};
			
			struct PoiMods
			{
				float4 Mask;
				float audioLink[5];
				float audioLinkAvailable;
				float audioLinkVersion;
				float4 audioLinkTexture;
				float audioLinkViaLuma;
				float2 detailMask;
				float2 backFaceDetailIntensity;
				float globalEmission;
				float4 globalColorTheme[12];
				float globalMask[16];
				float ALTime[8];
			};
			
			struct PoiLight
			{
				
				float3 direction;
				float attenuation;
				float attenuationStrength;
				float3 directColor;
				float3 indirectColor;
				float occlusion;
				float shadowMask;
				float detailShadow;
				float3 halfDir;
				float lightMap;
				float lightMapNoAttenuation;
				float3 rampedLightMap;
				float vertexNDotL;
				float nDotL;
				float nDotV;
				float vertexNDotV;
				float nDotH;
				float vertexNDotH;
				float lDotv;
				float lDotH;
				float nDotLSaturated;
				float nDotLNormalized;
				#ifdef POI_PASS_ADD
				float additiveShadow;
				#endif
				float3 finalLighting;
				float3 finalLightAdd;
				float3 LTCGISpecular;
				float3 LTCGIDiffuse;
				float directLuminance;
				float indirectLuminance;
				float finalLuminance;
				
				#if defined(VERTEXLIGHT_ON)
				// Non Important Lights
				float4 vDotNL;
				float4 vertexVDotNL;
				float3 vColor[4];
				float4 vCorrectedDotNL;
				float4 vAttenuation;
				float4 vAttenuationDotNL;
				float3 vPosition[4];
				float3 vDirection[4];
				float3 vFinalLighting;
				float3 vHalfDir[4];
				half4 vDotNH;
				half4 vertexVDotNH;
				half4 vDotLH;
				#endif
				
			};
			
			struct PoiVertexLights
			{
				
				float3 direction;
				float3 color;
				float attenuation;
			};
			
			struct PoiFragData
			{
				float smoothness;
				float smoothness2;
				float metallic;
				float specularMask;
				float reflectionMask;
				
				float3 baseColor;
				float3 finalColor;
				float alpha;
				float3 emission;
				float toggleVertexLights;
			};
			
			float4 poiTransformClipSpacetoScreenSpaceFrag(float4 clipPos)
			{
				float4 positionSS = float4(clipPos.xyz * clipPos.w, clipPos.w);
				positionSS.xy = positionSS.xy / _ScreenParams.xy;
				return positionSS;
			}
			
			// glsl_mod behaves better on negative numbers, and
			// in some situations actually outperforms HLSL's fmod()
			#ifndef glsl_mod
			#define glsl_mod(x, y) (((x) - (y) * floor((x) / (y))))
			#endif
			
			uniform float random_uniform_float_only_used_to_stop_compiler_warnings = 0.0f;
			
			float2 poiUV(float2 uv, float4 tex_st)
			{
				return uv * tex_st.xy + tex_st.zw;
			}
			
			float2 vertexUV(in VertexOut o, int index)
			{
				switch(index)
				{
					case 0:
					return o.uv[0].xy;
					case 1:
					return o.uv[0].zw;
					case 2:
					return o.uv[1].xy;
					case 3:
					return o.uv[1].zw;
					default:
					return o.uv[0].xy;
				}
			}
			
			float2 vertexUV(in appdata v, int index)
			{
				switch(index)
				{
					case 0:
					return v.uv0.xy;
					case 1:
					return v.uv1.xy;
					case 2:
					return v.uv2.xy;
					case 3:
					return v.uv3.xy;
					default:
					return v.uv0.xy;
				}
			}
			
			//Lighting Helpers
			float calculateluminance(float3 color)
			{
				return color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
			}
			
			// Set by VRChat (as of open beta 1245)
			// _VRChatCameraMode: 0 => Normal, 1 => VR HandCam, 2 => Desktop Handcam, 3 => Screenshot/Photo
			// _VRChatMirrorMode: 0 => Normal, 1 => Mirror (VR), 2 => Mirror (Deskie)
			float _VRChatCameraMode;
			float _VRChatMirrorMode;
			
			float VRCCameraMode()
			{
				return _VRChatCameraMode;
			}
			
			float VRCMirrorMode()
			{
				return _VRChatMirrorMode;
			}
			
			bool IsInMirror()
			{
				return unity_CameraProjection[2][0] != 0.f || unity_CameraProjection[2][1] != 0.f;
			}
			
			bool IsOrthographicCamera()
			{
				return unity_OrthoParams.w == 1 || UNITY_MATRIX_P[3][3] == 1;
			}
			
			float shEvaluateDiffuseL1Geomerics_local(float L0, float3 L1, float3 n)
			{
				// average energy
				float R0 = max(0, L0);
				
				// avg direction of incoming light
				float3 R1 = 0.5f * L1;
				
				// directional brightness
				float lenR1 = length(R1);
				
				// linear angle between normal and direction 0-1
				//float q = 0.5f * (1.0f + dot(R1 / lenR1, n));
				//float q = dot(R1 / lenR1, n) * 0.5 + 0.5;
				float q = dot(normalize(R1), n) * 0.5 + 0.5;
				q = saturate(q); // Thanks to ScruffyRuffles for the bug identity.
				
				// power for q
				// lerps from 1 (linear) to 3 (cubic) based on directionality
				float p = 1.0f + 2.0f * lenR1 / R0;
				
				// dynamic range constant
				// should vary between 4 (highly directional) and 0 (ambient)
				float a = (1.0f - lenR1 / R0) / (1.0f + lenR1 / R0);
				
				return R0 * (a + (1.0f - a) * (p + 1.0f) * pow(q, p));
			}
			
			half3 BetterSH9(half4 normal)
			{
				float3 indirect;
				float3 L0 = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w) + float3(unity_SHBr.z, unity_SHBg.z, unity_SHBb.z) / 3.0;
				indirect.r = shEvaluateDiffuseL1Geomerics_local(L0.r, unity_SHAr.xyz, normal.xyz);
				indirect.g = shEvaluateDiffuseL1Geomerics_local(L0.g, unity_SHAg.xyz, normal.xyz);
				indirect.b = shEvaluateDiffuseL1Geomerics_local(L0.b, unity_SHAb.xyz, normal.xyz);
				indirect = max(0, indirect);
				indirect += SHEvalLinearL2(normal);
				return indirect;
			}
			
			// Silent's code ends here
			
			float3 getCameraForward()
			{
				#if UNITY_SINGLE_PASS_STEREO
				float3 p1 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 1, 1));
				float3 p2 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 0, 1));
				#else
				float3 p1 = mul(unity_CameraToWorld, float4(0, 0, 1, 1)).xyz;
				float3 p2 = mul(unity_CameraToWorld, float4(0, 0, 0, 1)).xyz;
				#endif
				return normalize(p2 - p1);
			}
			
			half3 GetSHLength()
			{
				half3 x, x1;
				x.r = length(unity_SHAr);
				x.g = length(unity_SHAg);
				x.b = length(unity_SHAb);
				x1.r = length(unity_SHBr);
				x1.g = length(unity_SHBg);
				x1.b = length(unity_SHBb);
				return x + x1;
			}
			
			float3 BoxProjection(float3 direction, float3 position, float4 cubemapPosition, float3 boxMin, float3 boxMax)
			{
				#if UNITY_SPECCUBE_BOX_PROJECTION
				//UNITY_BRANCH
				if (cubemapPosition.w > 0)
				{
					float3 factors = ((direction > 0 ? boxMax : boxMin) - position) / direction;
					float scalar = min(min(factors.x, factors.y), factors.z);
					direction = direction * scalar + (position - cubemapPosition.xyz);
				}
				#endif
				return direction;
			}
			
			float poiMax(float2 i)
			{
				return max(i.x, i.y);
			}
			
			float poiMax(float3 i)
			{
				return max(max(i.x, i.y), i.z);
			}
			
			float poiMax(float4 i)
			{
				return max(max(max(i.x, i.y), i.z), i.w);
			}
			
			float3 calculateNormal(in float3 baseNormal, in PoiMesh poiMesh, in Texture2D normalTexture, in float4 normal_ST, in float2 normalPan, in float normalUV, in float normalIntensity)
			{
				float3 normal = UnpackScaleNormal(POI2D_SAMPLER_PAN(normalTexture, _MainTex, poiUV(poiMesh.uv[normalUV], normal_ST), normalPan), normalIntensity);
				return normalize(
				normal.x * poiMesh.tangent[0] +
				normal.y * poiMesh.binormal[0] +
				normal.z * baseNormal
				);
			}
			
			float remap(float x, float minOld, float maxOld, float minNew = 0, float maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float2 remap(float2 x, float2 minOld, float2 maxOld, float2 minNew = 0, float2 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float3 remap(float3 x, float3 minOld, float3 maxOld, float3 minNew = 0, float3 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float4 remap(float4 x, float4 minOld, float4 maxOld, float4 minNew = 0, float4 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float remapClamped(float minOld, float maxOld, float x, float minNew = 0, float maxNew = 1)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float2 remapClamped(float2 minOld, float2 maxOld, float2 x, float2 minNew, float2 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float3 remapClamped(float3 minOld, float3 maxOld, float3 x, float3 minNew, float3 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float4 remapClamped(float4 minOld, float4 maxOld, float4 x, float4 minNew, float4 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			float2 calcParallax(in float height, in PoiCam poiCam)
			{
				return ((height * - 1) + 1) * (poiCam.tangentViewDir.xy / poiCam.tangentViewDir.z);
			}
			
			/*
			0: Zero	                float4(0.0, 0.0, 0.0, 0.0),
			1: One	                float4(1.0, 1.0, 1.0, 1.0),
			2: DstColor	            destinationColor,
			3: SrcColor	            sourceColor,
			4: OneMinusDstColor	    float4(1.0, 1.0, 1.0, 1.0) - destinationColor,
			5: SrcAlpha	            sourceColor.aaaa,
			6: OneMinusSrcColor	    float4(1.0, 1.0, 1.0, 1.0) - sourceColor,
			7: DstAlpha	            destinationColor.aaaa,
			8: OneMinusDstAlpha	    float4(1.0, 1.0, 1.0, 1.0) - destinationColor.,
			9: SrcAlphaSaturate     saturate(sourceColor.aaaa),
			10: OneMinusSrcAlpha	float4(1.0, 1.0, 1.0, 1.0) - sourceColor.aaaa,
			*/
			
			float4 poiBlend(const float sourceFactor, const  float4 sourceColor, const  float destinationFactor, const  float4 destinationColor, const float4 blendFactor)
			{
				float4 sA = 1 - blendFactor;
				const float4 blendData[11] = {
					float4(0.0, 0.0, 0.0, 0.0),
					float4(1.0, 1.0, 1.0, 1.0),
					destinationColor,
					sourceColor,
					float4(1.0, 1.0, 1.0, 1.0) - destinationColor,
					sA,
					float4(1.0, 1.0, 1.0, 1.0) - sourceColor,
					sA,
					float4(1.0, 1.0, 1.0, 1.0) - sA,
					saturate(sourceColor.aaaa),
					1 - sA,
				};
				
				return lerp(blendData[sourceFactor] * sourceColor + blendData[destinationFactor] * destinationColor, sourceColor, sA);
			}
			
			// Average
			float blendAverage(float base, float blend)
			{
				return (base + blend) / 2.0;
			}
			float3 blendAverage(float3 base, float3 blend)
			{
				return (base + blend) / 2.0;
			}
			
			// Color burn
			float blendColorBurn(float base, float blend)
			{
				return (blend == 0.0) ? blend : max((1.0 - ((1.0 - base) * rcp(random_uniform_float_only_used_to_stop_compiler_warnings + blend))), 0.0);
			}
			
			float3 blendColorBurn(float3 base, float3 blend)
			{
				return float3(blendColorBurn(base.r, blend.r), blendColorBurn(base.g, blend.g), blendColorBurn(base.b, blend.b));
			}
			
			// Color Dodge
			float blendColorDodge(float base, float blend)
			{
				return (blend == 1.0) ? blend : min(base / (1.0 - blend), 1.0);
			}
			
			float3 blendColorDodge(float3 base, float3 blend)
			{
				return float3(blendColorDodge(base.r, blend.r), blendColorDodge(base.g, blend.g), blendColorDodge(base.b, blend.b));
			}
			
			// Darken
			float blendDarken(float base, float blend)
			{
				return min(blend, base);
			}
			
			float3 blendDarken(float3 base, float3 blend)
			{
				return float3(blendDarken(base.r, blend.r), blendDarken(base.g, blend.g), blendDarken(base.b, blend.b));
			}
			
			// Exclusion
			float blendExclusion(float base, float blend)
			{
				return base + blend - 2.0 * base * blend;
			}
			float3 blendExclusion(float3 base, float3 blend)
			{
				return base + blend - 2.0 * base * blend;
			}
			
			// Reflect
			float blendReflect(float base, float blend)
			{
				return (blend == 1.0) ? blend : min(base * base / (1.0 - blend), 1.0);
			}
			
			float3 blendReflect(float3 base, float3 blend)
			{
				return float3(blendReflect(base.r, blend.r), blendReflect(base.g, blend.g), blendReflect(base.b, blend.b));
			}
			
			// Glow
			float blendGlow(float base, float blend)
			{
				return blendReflect(blend, base);
			}
			float3 blendGlow(float3 base, float3 blend)
			{
				return blendReflect(blend, base);
			}
			
			// Overlay
			float blendOverlay(float base, float blend)
			{
				return base < 0.5 ? (2.0 * base * blend) : (1.0 - 2.0 * (1.0 - base) * (1.0 - blend));
			}
			
			float3 blendOverlay(float3 base, float3 blend)
			{
				return float3(blendOverlay(base.r, blend.r), blendOverlay(base.g, blend.g), blendOverlay(base.b, blend.b));
			}
			
			// Hard Light
			float blendHardLight(float base, float blend)
			{
				return blendOverlay(blend, base);
			}
			float3 blendHardLight(float3 base, float3 blend)
			{
				return blendOverlay(blend, base);
			}
			
			// Vivid light
			float blendVividLight(float base, float blend)
			{
				return (blend < 0.5) ? blendColorBurn(base, (2.0 * blend)) : blendColorDodge(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendVividLight(float3 base, float3 blend)
			{
				return float3(blendVividLight(base.r, blend.r), blendVividLight(base.g, blend.g), blendVividLight(base.b, blend.b));
			}
			
			// Hard mix
			float blendHardMix(float base, float blend)
			{
				return (blendVividLight(base, blend) < 0.5) ? 0.0 : 1.0;
			}
			
			float3 blendHardMix(float3 base, float3 blend)
			{
				return float3(blendHardMix(base.r, blend.r), blendHardMix(base.g, blend.g), blendHardMix(base.b, blend.b));
			}
			
			// Lighten
			float blendLighten(float base, float blend)
			{
				return max(blend, base);
			}
			
			float3 blendLighten(float3 base, float3 blend)
			{
				return float3(blendLighten(base.r, blend.r), blendLighten(base.g, blend.g), blendLighten(base.b, blend.b));
			}
			
			// Linear Burn
			float blendLinearBurn(float base, float blend)
			{
				// Note : Same implementation as BlendSubtractf
				return max(base + blend - 1.0, 0.0);
			}
			
			float3 blendLinearBurn(float3 base, float3 blend)
			{
				// Note : Same implementation as BlendSubtract
				return max(base + blend - float3(1.0, 1.0, 1.0), float3(0.0, 0.0, 0.0));
			}
			
			// Linear Dodge
			float blendLinearDodge(float base, float blend)
			{
				// Note : Same implementation as BlendAddf
				return min(base + blend, 1.0);
			}
			
			float3 blendLinearDodge(float3 base, float3 blend)
			{
				// Note : Same implementation as BlendAdd
				return base + blend;
			}
			
			// Linear light
			float blendLinearLight(float base, float blend)
			{
				return blend < 0.5 ? blendLinearBurn(base, (2.0 * blend)) : blendLinearDodge(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendLinearLight(float3 base, float3 blend)
			{
				return float3(blendLinearLight(base.r, blend.r), blendLinearLight(base.g, blend.g), blendLinearLight(base.b, blend.b));
			}
			
			// Multiply
			float blendMultiply(float base, float blend)
			{
				return base * blend;
			}
			float3 blendMultiply(float3 base, float3 blend)
			{
				return base * blend;
			}
			
			// Negation
			float blendNegation(float base, float blend)
			{
				return 1.0 - abs(1.0 - base - blend);
			}
			float3 blendNegation(float3 base, float3 blend)
			{
				return float3(1.0, 1.0, 1.0) - abs(float3(1.0, 1.0, 1.0) - base - blend);
			}
			
			// Normal
			float blendNormal(float base, float blend)
			{
				return blend;
			}
			float3 blendNormal(float3 base, float3 blend)
			{
				return blend;
			}
			
			// Phoenix
			float blendPhoenix(float base, float blend)
			{
				return min(base, blend) - max(base, blend) + 1.0;
			}
			float3 blendPhoenix(float3 base, float3 blend)
			{
				return min(base, blend) - max(base, blend) + float3(1.0, 1.0, 1.0);
			}
			
			// Pin light
			float blendPinLight(float base, float blend)
			{
				return (blend < 0.5) ? blendDarken(base, (2.0 * blend)) : blendLighten(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendPinLight(float3 base, float3 blend)
			{
				return float3(blendPinLight(base.r, blend.r), blendPinLight(base.g, blend.g), blendPinLight(base.b, blend.b));
			}
			
			// Screen
			float blendScreen(float base, float blend)
			{
				return 1.0 - ((1.0 - base) * (1.0 - blend));
			}
			
			float3 blendScreen(float3 base, float3 blend)
			{
				return float3(blendScreen(base.r, blend.r), blendScreen(base.g, blend.g), blendScreen(base.b, blend.b));
			}
			
			// Soft Light
			float blendSoftLight(float base, float blend)
			{
				return (blend < 0.5) ? (2.0 * base * blend + base * base * (1.0 - 2.0 * blend)) : (sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend));
			}
			
			float3 blendSoftLight(float3 base, float3 blend)
			{
				return float3(blendSoftLight(base.r, blend.r), blendSoftLight(base.g, blend.g), blendSoftLight(base.b, blend.b));
			}
			
			// Subtract
			float blendSubtract(float base, float blend)
			{
				return max(base - blend, 0.0);
			}
			
			float3 blendSubtract(float3 base, float3 blend)
			{
				return max(base - blend, 0.0);
			}
			
			// Difference
			float blendDifference(float base, float blend)
			{
				return abs(base - blend);
			}
			
			float3 blendDifference(float3 base, float3 blend)
			{
				return abs(base - blend);
			}
			
			// Divide
			float blendDivide(float base, float blend)
			{
				return base / max(blend, 0.0001);
			}
			
			float3 blendDivide(float3 base, float3 blend)
			{
				return base / max(blend, 0.0001);
			}
			
			float blendMixed(float base, float blend)
			{
				return base + base * blend;
			}
			
			float3 blendMixed(float3 base, float3 blend)
			{
				return base + base * blend;
			}
			
			float3 customBlend(float3 base, float3 blend, float blendType, float alpha = 1)
			{
				float3 output = base;
				switch(blendType)
				{
					case 0: output = lerp(base, blend, alpha); break;
					case 2: output = base * lerp(1, blend, alpha); break;
					case 6: output = lerp(base, blendScreen(base, blend), alpha); break;
					case 7: output = blendSubtract(base, blend * alpha); break;
					case 8: output = lerp(base, blendLinearDodge(base, blend), alpha); break;
					case 9: output = lerp(base, blendOverlay(base, blend), alpha); break;
					case 20: output = lerp(base, blendMixed(base, blend), alpha); break;
					default: output = 0; break;
				}
				return output;
			}
			
			float3 customBlend(float base, float blend, float blendType, float alpha = 1)
			{
				float3 output = base;
				switch(blendType)
				{
					case 0: output = lerp(base, blend, alpha); break;
					case 2: output = base * lerp(1, blend, alpha); break;
					case 6: output = lerp(base, blendScreen(base, blend), alpha); break;
					case 7: output = blendSubtract(base, blend * alpha); break;
					case 8: output = lerp(base, blendLinearDodge(base, blend), alpha); break;
					case 9: output = lerp(base, blendOverlay(base, blend), alpha); break;
					case 20: output = lerp(base, blendMixed(base, blend), alpha); break;
					default: output = 0; break;
				}
				return output;
			}
			
			#define REPLACE 0
			#define SUBSTRACT 1
			#define MULTIPLY 2
			#define DIVIDE 3
			#define MIN 4
			#define MAX 5
			#define AVERAGE 6
			#define ADD 7
			
			float maskBlend(float baseMask, float blendMask, float blendType)
			{
				float output = 0;
				switch(blendType)
				{
					case REPLACE: output = blendMask; break;
					case SUBSTRACT: output = baseMask - blendMask; break;
					case MULTIPLY: output = baseMask * blendMask; break;
					case DIVIDE: output = baseMask / blendMask; break;
					case MIN: output = min(baseMask, blendMask); break;
					case MAX: output = max(baseMask, blendMask); break;
					case AVERAGE: output = (baseMask + blendMask) * 0.5; break;
					case ADD: output = baseMask + blendMask; break;
				}
				return saturate(output);
			}
			
			float globalMaskBlend(float baseMask, float globalMaskIndex, float blendType, PoiMods poiMods)
			{
				if (globalMaskIndex == 0)
				{
					return baseMask;
				}
				else
				{
					return maskBlend(baseMask, poiMods.globalMask[globalMaskIndex - 1], blendType);
				}
			}
			
			float random(float2 p)
			{
				return frac(sin(dot(p, float2(12.9898, 78.2383))) * 43758.5453123);
			}
			
			float2 random2(float2 p)
			{
				return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
			}
			
			float3 random3(float2 p)
			{
				return frac(sin(float3(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)), dot(p, float2(248.3, 315.9)))) * 43758.5453);
			}
			
			float3 random3(float3 p)
			{
				return frac(sin(float3(dot(p, float3(127.1, 311.7, 248.6)), dot(p, float3(269.5, 183.3, 423.3)), dot(p, float3(248.3, 315.9, 184.2)))) * 43758.5453);
			}
			
			float3 randomFloat3(float2 Seed, float maximum)
			{
				return (.5 + float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed), float2(12.9898, 78.233))) * 43758.5453)
				) * .5) * (maximum);
			}
			
			float3 randomFloat3Range(float2 Seed, float Range)
			{
				return (float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
				) * 2 - 1) * Range;
			}
			
			float3 randomFloat3WiggleRange(float2 Seed, float Range, float wiggleSpeed)
			{
				float3 rando = (float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
				) * 2 - 1);
				float speed = 1 + wiggleSpeed;
				return float3(sin((_Time.x + rando.x * PI) * speed), sin((_Time.x + rando.y * PI) * speed), sin((_Time.x + rando.z * PI) * speed)) * Range;
			}
			
			void poiDither(float4 In, float4 ScreenPosition, out float4 Out)
			{
				float2 uv = ScreenPosition.xy * _ScreenParams.xy;
				float DITHER_THRESHOLDS[16] = {
					1.0 / 17.0, 9.0 / 17.0, 3.0 / 17.0, 11.0 / 17.0,
					13.0 / 17.0, 5.0 / 17.0, 15.0 / 17.0, 7.0 / 17.0,
					4.0 / 17.0, 12.0 / 17.0, 2.0 / 17.0, 10.0 / 17.0,
					16.0 / 17.0, 8.0 / 17.0, 14.0 / 17.0, 6.0 / 17.0
				};
				uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
				Out = In - DITHER_THRESHOLDS[index];
			}
			
			static const float Epsilon = 1e-10;
			// The weights of RGB contributions to luminance.
			// Should sum to unity.
			static const float3 HCYwts = float3(0.299, 0.587, 0.114);
			static const float HCLgamma = 3;
			static const float HCLy0 = 100;
			static const float HCLmaxL = 0.530454533953517; // == exp(HCLgamma / HCLy0) - 0.5
			static const float3 wref = float3(1.0, 1.0, 1.0);
			#define TAU 6.28318531
			
			float3 HUEtoRGB(in float H)
			{
				float R = abs(H * 6 - 3) - 1;
				float G = 2 - abs(H * 6 - 2);
				float B = 2 - abs(H * 6 - 4);
				return saturate(float3(R, G, B));
			}
			
			float3 RGBtoHCV(in float3 RGB)
			{
				// Based on work by Sam Hocevar and Emil Persson
				float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
				float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
				float C = Q.x - min(Q.w, Q.y);
				float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
				return float3(H, C, Q.x);
			}
			
			float3 HSVtoRGB(in float3 HSV)
			{
				float3 RGB = HUEtoRGB(HSV.x);
				return ((RGB - 1) * HSV.y + 1) * HSV.z;
			}
			
			float3 RGBtoHSV(in float3 RGB)
			{
				float3 HCV = RGBtoHCV(RGB);
				float S = HCV.y / (HCV.z + Epsilon);
				return float3(HCV.x, S, HCV.z);
			}
			
			float3 HSLtoRGB(in float3 HSL)
			{
				float3 RGB = HUEtoRGB(HSL.x);
				float C = (1 - abs(2 * HSL.z - 1)) * HSL.y;
				return (RGB - 0.5) * C + HSL.z;
			}
			
			float3 RGBtoHSL(in float3 RGB)
			{
				float3 HCV = RGBtoHCV(RGB);
				float L = HCV.z - HCV.y * 0.5;
				float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
				return float3(HCV.x, S, L);
			}
			
			void DecomposeHDRColor(in float3 linearColorHDR, out float3 baseLinearColor, out float exposure)
			{
				// Optimization/adaptation of https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/GUI/ColorMutator.cs#L23 but skips weird photoshop stuff
				float maxColorComponent = max(linearColorHDR.r, max(linearColorHDR.g, linearColorHDR.b));
				bool isSDR = maxColorComponent <= 1.0;
				
				float scaleFactor = isSDR ? 1.0 : (1.0 / maxColorComponent);
				exposure = isSDR ? 0.0 : log(maxColorComponent) * 1.44269504089; // ln(2)
				
				baseLinearColor = scaleFactor * linearColorHDR;
			}
			
			float3 ApplyHDRExposure(float3 linearColor, float exposure)
			{
				return linearColor * pow(2, exposure);
			}
			
			// Transforms an RGB color using a matrix. Note that S and V are absolute values here
			float3 ModifyViaHSV(float3 color, float h, float s, float v)
			{
				float3 colorHSV = RGBtoHSV(color);
				colorHSV.x = frac(colorHSV.x + h);
				colorHSV.y = saturate(colorHSV.y + s);
				colorHSV.z = saturate(colorHSV.z + v);
				return HSVtoRGB(colorHSV);
			}
			
			float3 ModifyViaHSV(float3 color, float3 HSVMod)
			{
				return ModifyViaHSV(color, HSVMod.x, HSVMod.y, HSVMod.z);
			}
			
			float4x4 brightnessMatrix(float brightness)
			{
				return float4x4(
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				brightness, brightness, brightness, 1
				);
			}
			
			float4x4 contrastMatrix(float contrast)
			{
				float t = (1.0 - contrast) / 2.0;
				
				return float4x4(
				contrast, 0, 0, 0,
				0, contrast, 0, 0,
				0, 0, contrast, 0,
				t, t, t, 1
				);
			}
			
			float4x4 saturationMatrix(float saturation)
			{
				float3 luminance = float3(0.3086, 0.6094, 0.0820);
				
				float oneMinusSat = 1.0 - saturation;
				
				float3 red = luminance.x * oneMinusSat;
				red += float3(saturation, 0, 0);
				
				float3 green = luminance.y * oneMinusSat;
				green += float3(0, saturation, 0);
				
				float3 blue = luminance.z * oneMinusSat;
				blue += float3(0, 0, saturation);
				
				return float4x4(
				red, 0,
				green, 0,
				blue, 0,
				0, 0, 0, 1
				);
			}
			
			float4 PoiColorBCS(float4 color, float brightness, float contrast, float saturation)
			{
				return mul(color, mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation))));
			}
			float3 PoiColorBCS(float3 color, float brightness, float contrast, float saturation)
			{
				return mul(float4(color, 1), mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation)))).rgb;
			}
			
			float3 linear_srgb_to_oklab(float3 c)
			{
				float l = 0.4122214708 * c.x + 0.5363325363 * c.y + 0.0514459929 * c.z;
				float m = 0.2119034982 * c.x + 0.6806995451 * c.y + 0.1073969566 * c.z;
				float s = 0.0883024619 * c.x + 0.2817188376 * c.y + 0.6299787005 * c.z;
				
				float l_ = pow(l, 1.0 / 3.0);
				float m_ = pow(m, 1.0 / 3.0);
				float s_ = pow(s, 1.0 / 3.0);
				
				return float3(
				0.2104542553 * l_ + 0.7936177850 * m_ - 0.0040720468 * s_,
				1.9779984951 * l_ - 2.4285922050 * m_ + 0.4505937099 * s_,
				0.0259040371 * l_ + 0.7827717662 * m_ - 0.8086757660 * s_
				);
			}
			
			float3 oklab_to_linear_srgb(float3 c)
			{
				float l_ = c.x + 0.3963377774 * c.y + 0.2158037573 * c.z;
				float m_ = c.x - 0.1055613458 * c.y - 0.0638541728 * c.z;
				float s_ = c.x - 0.0894841775 * c.y - 1.2914855480 * c.z;
				
				float l = l_ * l_ * l_;
				float m = m_ * m_ * m_;
				float s = s_ * s_ * s_;
				
				return float3(
				+ 4.0767416621 * l - 3.3077115913 * m + 0.2309699292 * s,
				- 1.2684380046 * l + 2.6097574011 * m - 0.3413193965 * s,
				- 0.0041960863 * l - 0.7034186147 * m + 1.7076147010 * s
				);
			}
			
			float3 hueShift(float3 color, float shift)
			{
				float3 oklab = linear_srgb_to_oklab(max(color, 0.0000000001));
				float hue = atan2(oklab.z, oklab.y);
				hue += shift * PI * 2;  // Add the hue shift
				
				float chroma = length(oklab.yz);
				oklab.y = cos(hue) * chroma;
				oklab.z = sin(hue) * chroma;
				
				return oklab_to_linear_srgb(oklab);
			}
			
			float3 hueShift(float4 color, float shift)
			{
				return hueShift(color.rgb, shift);
			}
			
			/*
			float3 hueShift(float3 color, float hueOffset)
			{
				color = RGBtoHSV(color);
				color.x = frac(hueOffset +color.x);
				return HSVtoRGB(color);
			}
			*/
			
			// LCH
			float xyzF(float t)
			{
				return lerp(pow(t, 1. / 3.), 7.787037 * t + 0.139731, step(t, 0.00885645));
			}
			float xyzR(float t)
			{
				return lerp(t * t * t, 0.1284185 * (t - 0.139731), step(t, 0.20689655));
			}
			
			float4x4 poiRotationMatrixFromAngles(float x, float y, float z)
			{
				float angleX = radians(x);
				float c = cos(angleX);
				float s = sin(angleX);
				float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
				0, c, -s, 0,
				0, s, c, 0,
				0, 0, 0, 1);
				
				float angleY = radians(y);
				c = cos(angleY);
				s = sin(angleY);
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
				0, 1, 0, 0,
				- s, 0, c, 0,
				0, 0, 0, 1);
				
				float angleZ = radians(z);
				c = cos(angleZ);
				s = sin(angleZ);
				float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
				s, c, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
				
				return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
			}
			
			float4x4 poiRotationMatrixFromAngles(float3 angles)
			{
				float angleX = radians(angles.x);
				float c = cos(angleX);
				float s = sin(angleX);
				float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
				0, c, -s, 0,
				0, s, c, 0,
				0, 0, 0, 1);
				
				float angleY = radians(angles.y);
				c = cos(angleY);
				s = sin(angleY);
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
				0, 1, 0, 0,
				- s, 0, c, 0,
				0, 0, 0, 1);
				
				float angleZ = radians(angles.z);
				c = cos(angleZ);
				s = sin(angleZ);
				float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
				s, c, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
				
				return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
			}
			
			float3 getCameraPosition()
			{
				#ifdef USING_STEREO_MATRICES
				return lerp(unity_StereoWorldSpaceCameraPos[0], unity_StereoWorldSpaceCameraPos[1], 0.5);
				#endif
				return _WorldSpaceCameraPos;
			}
			
			float2 calcPixelScreenUVs(half4 grabPos)
			{
				half2 uv = grabPos.xy / (grabPos.w + 0.0000000001);
				#if UNITY_SINGLE_PASS_STEREO
				uv.xy *= half2(_ScreenParams.x * 2, _ScreenParams.y);
				#else
				uv.xy *= _ScreenParams.xy;
				#endif
				
				return uv;
			}
			
			float CalcMipLevel(float2 texture_coord)
			{
				float2 dx = ddx(texture_coord);
				float2 dy = ddy(texture_coord);
				float delta_max_sqr = max(dot(dx, dx), dot(dy, dy));
				
				return 0.5 * log2(delta_max_sqr);
			}
			
			float inverseLerp(float A, float B, float T)
			{
				return (T - A) / (B - A);
			}
			
			float inverseLerp2(float2 a, float2 b, float2 value)
			{
				float2 AB = b - a;
				float2 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			float inverseLerp3(float3 a, float3 b, float3 value)
			{
				float3 AB = b - a;
				float3 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			float inverseLerp4(float4 a, float4 b, float4 value)
			{
				float4 AB = b - a;
				float4 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			/*
			MIT License
			
			Copyright (c) 2019 wraikny
			
			Permission is hereby granted, free of charge, to any person obtaining a copy
			of this software and associated documentation files (the "Software"), to deal
			in the Software without restriction, including without limitation the rights
			to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
			copies of the Software, and to permit persons to whom the Software is
			furnished to do so, subject to the following conditions:
			
			The above copyright notice and this permission notice shall be included in all
			copies or substantial portions of the Software.
			
			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
			IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
			FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
			AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
			LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
			OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
			SOFTWARE.
			
			VertexTransformShader is dependent on:
			*/
			
			float4 quaternion_conjugate(float4 v)
			{
				return float4(
				v.x, -v.yzw
				);
			}
			
			float4 quaternion_mul(float4 v1, float4 v2)
			{
				float4 result1 = (v1.x * v2 + v1 * v2.x);
				
				float4 result2 = float4(
				- dot(v1.yzw, v2.yzw),
				cross(v1.yzw, v2.yzw)
				);
				
				return float4(result1 + result2);
			}
			
			// angle : radians
			float4 get_quaternion_from_angle(float3 axis, float angle)
			{
				float sn = sin(angle * 0.5);
				float cs = cos(angle * 0.5);
				return float4(axis * sn, cs);
			}
			
			float4 quaternion_from_vector(float3 inVec)
			{
				return float4(0.0, inVec);
			}
			
			float degree_to_radius(float degree)
			{
				return (
				degree / 180.0 * PI
				);
			}
			
			float3 rotate_with_quaternion(float3 inVec, float3 rotation)
			{
				float4 qx = get_quaternion_from_angle(float3(1, 0, 0), radians(rotation.x));
				float4 qy = get_quaternion_from_angle(float3(0, 1, 0), radians(rotation.y));
				float4 qz = get_quaternion_from_angle(float3(0, 0, 1), radians(rotation.z));
				
				#define MUL3(A, B, C) quaternion_mul(quaternion_mul((A), (B)), (C))
				float4 quaternion = normalize(MUL3(qx, qy, qz));
				float4 conjugate = quaternion_conjugate(quaternion);
				
				float4 inVecQ = quaternion_from_vector(inVec);
				
				float3 rotated = (
				MUL3(quaternion, inVecQ, conjugate)
				).yzw;
				
				return rotated;
			}
			
			float4 transform(float4 input, float4 pos, float4 rotation, float4 scale)
			{
				input.rgb *= (scale.xyz * scale.w);
				input = float4(rotate_with_quaternion(input.xyz, rotation.xyz * rotation.w) + (pos.xyz * pos.w), input.w);
				return input;
			}
			
			float2 RotateUV(float2 _uv, float _radian, float2 _piv, float _time)
			{
				float RotateUV_ang = _radian;
				float RotateUV_cos = cos(_time * RotateUV_ang);
				float RotateUV_sin = sin(_time * RotateUV_ang);
				return (mul(_uv - _piv, float2x2(RotateUV_cos, -RotateUV_sin, RotateUV_sin, RotateUV_cos)) + _piv);
			}
			
			/*
			MIT END
			*/
			
			float3 RotateAroundAxis(float3 original, float3 axis, float radian)
			{
				float s = sin(radian);
				float c = cos(radian);
				float one_minus_c = 1.0 - c;
				
				axis = normalize(axis);
				float3x3 rot_mat = {
					one_minus_c * axis.x * axis.x + c, one_minus_c * axis.x * axis.y - axis.z * s, one_minus_c * axis.z * axis.x + axis.y * s,
					one_minus_c * axis.x * axis.y + axis.z * s, one_minus_c * axis.y * axis.y + c, one_minus_c * axis.y * axis.z - axis.x * s,
					one_minus_c * axis.z * axis.x - axis.y * s, one_minus_c * axis.y * axis.z + axis.x * s, one_minus_c * axis.z * axis.z + c
				};
				return mul(rot_mat, original);
			}
			
			float3 poiThemeColor(in PoiMods poiMods, in float3 srcColor, in float themeIndex)
			{
				if (themeIndex == 0) return srcColor;
				themeIndex -= 1;
				
				if (themeIndex <= 3)
				{
					return poiMods.globalColorTheme[themeIndex];
				}
				
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable)
				{
					return poiMods.globalColorTheme[themeIndex];
				}
				#endif
				
				return srcColor;
			}
			
			float3 lilToneCorrection(float3 c, float4 hsvg)
			{
				// gamma
				c = pow(abs(c), hsvg.w);
				// rgb -> hsv
				float4 p = (c.b > c.g) ? float4(c.bg, -1.0, 2.0 / 3.0) : float4(c.gb, 0.0, -1.0 / 3.0);
				float4 q = (p.x > c.r) ? float4(p.xyw, c.r) : float4(c.r, p.yzx);
				float d = q.x - min(q.w, q.y);
				float e = 1.0e-10;
				float3 hsv = float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
				// shift
				hsv = float3(hsv.x + hsvg.x, saturate(hsv.y * hsvg.y), saturate(hsv.z * hsvg.z));
				// hsv -> rgb
				return hsv.z - hsv.z * hsv.y + hsv.z * hsv.y * saturate(abs(frac(hsv.x + float3(1.0, 2.0 / 3.0, 1.0 / 3.0)) * 6.0 - 3.0) - 1.0);
			}
			
			float3 lilBlendColor(float3 dstCol, float3 srcCol, float3 srcA, int blendMode)
			{
				float3 ad = dstCol + srcCol;
				float3 mu = dstCol * srcCol;
				float3 outCol;
				if (blendMode == 0) outCol = srcCol;               // Normal
				if (blendMode == 1) outCol = ad;                   // Add
				if (blendMode == 2) outCol = max(ad - mu, dstCol); // Screen
				if (blendMode == 3) outCol = mu;                   // Multiply
				return lerp(dstCol, outCol, srcA);
			}
			
			float lilIsIn0to1(float f)
			{
				float value = 0.5 - abs(f - 0.5);
				return saturate(value / clamp(fwidth(value), 0.0001, 1.0));
			}
			
			float lilIsIn0to1(float f, float nv)
			{
				float value = 0.5 - abs(f - 0.5);
				return saturate(value / clamp(fwidth(value), 0.0001, nv));
			}
			
			float poiEdgeLinearNoSaturate(float value, float border)
			{
				return (value - border) / clamp(fwidth(value), 0.0001, 1.0);
			}
			
			float3 poiEdgeLinearNoSaturate(float value, float3 border)
			{
				return float3(
				(value - border.x) / clamp(fwidth(value), 0.0001, 1.0),
				(value - border.y) / clamp(fwidth(value), 0.0001, 1.0),
				(value - border.z) / clamp(fwidth(value), 0.0001, 1.0)
				);
			}
			
			float poiEdgeLinearNoSaturate(float value, float border, float blur)
			{
				float borderMin = saturate(border - blur * 0.5);
				float borderMax = saturate(border + blur * 0.5);
				return (value - borderMin) / saturate(borderMax - borderMin + fwidth(value));
			}
			
			float poiEdgeLinearNoSaturate(float value, float border, float blur, float borderRange)
			{
				float borderMin = saturate(border - blur * 0.5 - borderRange);
				float borderMax = saturate(border + blur * 0.5);
				return (value - borderMin) / saturate(borderMax - borderMin + fwidth(value));
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border)
			{
				//return (value - border) / clamp(fwidth(value), 0.0001, 1.0);
				
				float fwidthValue = fwidth(value);
				return smoothstep(border - fwidthValue, border + fwidthValue, value);
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border, float blur)
			{
				float fwidthValue = fwidth(value);
				float borderMin = saturate(border - blur * 0.5);
				float borderMax = saturate(border + blur * 0.5);
				return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border, float blur, float borderRange)
			{
				float fwidthValue = fwidth(value);
				float borderMin = saturate(border - blur * 0.5 - borderRange);
				float borderMax = saturate(border + blur * 0.5);
				return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
			}
			
			float poiEdgeNonLinear(float value, float border)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border));
			}
			
			float poiEdgeNonLinear(float value, float border, float blur)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border, blur));
			}
			
			float poiEdgeNonLinear(float value, float border, float blur, float borderRange)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border, blur, borderRange));
			}
			
			float poiEdgeLinear(float value, float border)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border));
			}
			
			float poiEdgeLinear(float value, float border, float blur)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border, blur));
			}
			
			float poiEdgeLinear(float value, float border, float blur, float borderRange)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border, blur, borderRange));
			}
			// From https://github.com/lilxyzw/OpenLit/blob/main/Assets/OpenLit/core.hlsl
			float3 OpenLitLinearToSRGB(float3 col)
			{
				return LinearToGammaSpace(col);
			}
			
			float3 OpenLitSRGBToLinear(float3 col)
			{
				return GammaToLinearSpace(col);
			}
			
			float OpenLitLuminance(float3 rgb)
			{
				#if defined(UNITY_COLORSPACE_GAMMA)
				return dot(rgb, float3(0.22, 0.707, 0.071));
				#else
				return dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
				#endif
			}
			
			float3 AdjustLitLuminance(float3 rgb, float targetLuminance)
			{
				float currentLuminance;
				#if defined(UNITY_COLORSPACE_GAMMA)
				currentLuminance = dot(rgb, float3(0.22, 0.707, 0.071));
				#else
				currentLuminance = dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
				#endif
				
				float luminanceRatio = targetLuminance / currentLuminance;
				return rgb * luminanceRatio;
			}
			
			float3 ClampLuminance(float3 rgb, float minLuminance, float maxLuminance)
			{
				float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
				float minRatio = (currentLuminance != 0) ? minLuminance / currentLuminance : 1.0;
				float maxRatio = (currentLuminance != 0) ? maxLuminance / currentLuminance : 1.0;
				float luminanceRatio = clamp(min(maxRatio, max(minRatio, 1.0)), 0.0, 1.0);
				return lerp(rgb, rgb * luminanceRatio, luminanceRatio < 1.0);
			}
			
			float3 MaxLuminance(float3 rgb, float maxLuminance)
			{
				float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
				float luminanceRatio = (currentLuminance != 0) ? maxLuminance / currentLuminance : 1.0;
				return lerp(rgb, rgb * luminanceRatio, currentLuminance > maxLuminance);
			}
			
			float OpenLitGray(float3 rgb)
			{
				return dot(rgb, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0));
			}
			
			void OpenLitShadeSH9ToonDouble(float3 lightDirection, out float3 shMax, out float3 shMin)
			{
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 N = lightDirection * 0.666666;
				float4 vB = N.xyzz * N.yzzx;
				// L0 L2
				float3 res = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
				res.r += dot(unity_SHBr, vB);
				res.g += dot(unity_SHBg, vB);
				res.b += dot(unity_SHBb, vB);
				res += unity_SHC.rgb * (N.x * N.x - N.y * N.y);
				// L1
				float3 l1;
				l1.r = dot(unity_SHAr.rgb, N);
				l1.g = dot(unity_SHAg.rgb, N);
				l1.b = dot(unity_SHAb.rgb, N);
				shMax = res + l1;
				shMin = res - l1;
				#if defined(UNITY_COLORSPACE_GAMMA)
				shMax = OpenLitLinearToSRGB(shMax);
				shMin = OpenLitLinearToSRGB(shMin);
				#endif
				#else
				shMax = 0.0;
				shMin = 0.0;
				#endif
			}
			
			float3 OpenLitComputeCustomLightDirection(float4 lightDirectionOverride)
			{
				float3 customDir = length(lightDirectionOverride.xyz) * normalize(mul((float3x3)unity_ObjectToWorld, lightDirectionOverride.xyz));
				return lightDirectionOverride.w ? customDir : lightDirectionOverride.xyz; // .w isn't doc'd anywhere and is always 0 unless end user changes it
				
			}
			
			float3 OpenLitLightingDirectionForSH9()
			{
				float3 mainDir = _WorldSpaceLightPos0.xyz * OpenLitLuminance(_LightColor0.rgb);
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 sh9Dir = unity_SHAr.xyz * 0.333333 + unity_SHAg.xyz * 0.333333 + unity_SHAb.xyz * 0.333333;
				float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
				#else
				float3 sh9Dir = 0;
				float3 sh9DirAbs = 0;
				#endif
				
				float3 lightDirectionForSH9 = sh9Dir + mainDir;
				lightDirectionForSH9 = dot(lightDirectionForSH9, lightDirectionForSH9) < 0.000001 ? 0 : normalize(lightDirectionForSH9);
				return lightDirectionForSH9;
			}
			
			float3 OpenLitLightingDirection(float4 lightDirectionOverride)
			{
				float3 mainDir = _WorldSpaceLightPos0.xyz * OpenLitLuminance(_LightColor0.rgb);
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 sh9Dir = unity_SHAr.xyz * 0.333333 + unity_SHAg.xyz * 0.333333 + unity_SHAb.xyz * 0.333333;
				float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
				#else
				float3 sh9Dir = 0;
				float3 sh9DirAbs = 0;
				#endif
				float3 customDir = OpenLitComputeCustomLightDirection(lightDirectionOverride);
				
				return normalize(sh9DirAbs + mainDir + customDir);
			}
			
			float3 OpenLitLightingDirection()
			{
				float4 customDir = float4(0.001, 0.002, 0.001, 0.0);
				return OpenLitLightingDirection(customDir);
			}
			
			inline float4 CalculateFrustumCorrection()
			{
				float x1 = -UNITY_MATRIX_P._31 / (UNITY_MATRIX_P._11 * UNITY_MATRIX_P._34);
				float x2 = -UNITY_MATRIX_P._32 / (UNITY_MATRIX_P._22 * UNITY_MATRIX_P._34);
				return float4(x1, x2, 0, UNITY_MATRIX_P._33 / UNITY_MATRIX_P._34 + x1 * UNITY_MATRIX_P._13 + x2 * UNITY_MATRIX_P._23);
			}
			
			inline float CorrectedLinearEyeDepth(float z, float B)
			{
				return 1.0 / (z / UNITY_MATRIX_P._34 + B);
			}
			
			//Silent's code
			float2 sharpSample(float4 texelSize, float2 p)
			{
				p = p * texelSize.zw;
				float2 c = max(0.0, fwidth(p));
				p = floor(p) + saturate(frac(p) / c);
				p = (p - 0.5) * texelSize.xy;
				return p;
			}
			
			void applyToGlobalMask(inout PoiMods poiMods, int index, int blendType, float val)
			{
				float valBlended = saturate(maskBlend(poiMods.globalMask[index], val, blendType));
				switch(index)
				{
					case 0: poiMods.globalMask[0] = valBlended; break;
					case 1: poiMods.globalMask[1] = valBlended; break;
					case 2: poiMods.globalMask[2] = valBlended; break;
					case 3: poiMods.globalMask[3] = valBlended; break;
					case 4: poiMods.globalMask[4] = valBlended; break;
					case 5: poiMods.globalMask[5] = valBlended; break;
					case 6: poiMods.globalMask[6] = valBlended; break;
					case 7: poiMods.globalMask[7] = valBlended; break;
					case 8: poiMods.globalMask[8] = valBlended; break;
					case 9: poiMods.globalMask[9] = valBlended; break;
					case 10: poiMods.globalMask[10] = valBlended; break;
					case 11: poiMods.globalMask[11] = valBlended; break;
					case 12: poiMods.globalMask[12] = valBlended; break;
					case 13: poiMods.globalMask[13] = valBlended; break;
					case 14: poiMods.globalMask[14] = valBlended; break;
					case 15: poiMods.globalMask[15] = valBlended; break;
				}
			}
			
			void assignValueToVectorFromIndex(inout float4 vec, int index, float value)
			{
				switch(index)
				{
					case 0: vec[0] = value; break;
					case 1: vec[1] = value; break;
					case 2: vec[2] = value; break;
					case 3: vec[3] = value; break;
				}
			}
			
			// SNose
			float3 mod289(float3 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			float2 mod289(float2 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			float3 permute(float3 x)
			{
				return mod289(((x * 34.0) + 1.0) * x);
			}
			
			float snoise(float2 v)
			{
				const float4 C = float4(0.211324865405187, // (3.0-sqrt(3.0))/6.0
				0.366025403784439, // 0.5*(sqrt(3.0)-1.0)
				- 0.577350269189626, // -1.0 + 2.0 * C.x
				0.024390243902439); // 1.0 / 41.0
				float2 i = floor(v + dot(v, C.yy));
				float2 x0 = v - i + dot(i, C.xx);
				float2 i1;
				i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod289(i); // Avoid truncation effects in permutation
				float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0))
				+ i.x + float3(0.0, i1.x, 1.0));
				
				float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
				m = m * m ;
				m = m * m ;
				float3 x = 2.0 * frac(p * C.www) - 1.0;
				float3 h = abs(x) - 0.5;
				float3 ox = floor(x + 0.5);
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot(m, g);
			}
			
			float nsqDistance(float2 a, float2 b)
			{
				return dot(a - b, a - b);
			}
			
			float poiInvertToggle(in float value, in float toggle)
			{
				return (toggle == 0 ? value : 1 - value);
			}
			
			// OKLAB
			
			float3 PoiBlendNormal(float3 dstNormal, float3 srcNormal)
			{
				return float3(dstNormal.xy + srcNormal.xy, dstNormal.z * srcNormal.z);
			}
			
			float3 lilTransformDirOStoWS(float3 directionOS, bool doNormalize)
			{
				if (doNormalize) return normalize(mul((float3x3)unity_ObjectToWorld, directionOS));
				else            return mul((float3x3)unity_ObjectToWorld, directionOS);
			}
			
			float2 poiGetWidthAndHeight(Texture2D tex)
			{
				uint width, height;
				tex.GetDimensions(width, height);
				return float2(width, height);
			}
			
			float2 poiGetWidthAndHeight(Texture2DArray tex)
			{
				uint width, height, element;
				tex.GetDimensions(width, height, element);
				return float2(width, height);
			}
			
			VertexOut vert(
			#ifndef POI_TESSELLATED
			appdata v
			#else
			tessAppData v
			#endif
			)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				VertexOut o;
				PoiInitStruct(VertexOut, o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				#ifdef POI_TESSELLATED
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(v);
				#endif
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.tangent.xyz = UnityObjectToWorldDir(v.tangent);
				o.tangent.w = v.tangent.w;
				o.vertexColor = v.color;
				
				o.uv[0] = float4(v.uv0.xy, v.uv1.xy);
				o.uv[1] = float4(v.uv2.xy, v.uv3.xy);
				
				#if defined(LIGHTMAP_ON)
				o.lightmapUV.xy = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				#ifdef DYNAMICLIGHTMAP_ON
				o.lightmapUV.zw = v.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif
				
				o.localPos = v.vertex;
				o.worldPos = mul(unity_ObjectToWorld, o.localPos);
				
				float3 localOffset = float3(0, 0, 0);
				float3 worldOffset = float3(0, 0, 0);
				
				o.localPos.rgb += localOffset;
				o.worldPos.rgb += worldOffset;
				
				o.pos = UnityObjectToClipPos(o.localPos);
				
				#ifdef POI_PASS_OUTLINE
				#if defined(UNITY_REVERSED_Z)
				//DX
				o.pos.z += _Offset_Z * - 0.01;
				#else
				//OpenGL
				o.pos.z += _Offset_Z * 0.01;
				#endif
				#endif
				//o.grabPos = ComputeGrabScreenPos(o.pos);
				
				#ifndef FORWARD_META_PASS
				#if !defined(UNITY_PASS_SHADOWCASTER)
				UNITY_TRANSFER_SHADOW(o, o.uv[0].xy);
				#else
				v.vertex.xyz = o.localPos.xyz;
				TRANSFER_SHADOW_CASTER_NOPOS(o, o.pos);
				#endif
				#endif
				
				UNITY_TRANSFER_FOG(o, o.pos);
				
				if (_RenderingReduceClipDistance)
				{
					if (o.pos.w < _ProjectionParams.y * 1.01 && o.pos.w > 0)
					{
						#if defined(UNITY_REVERSED_Z) // DirectX
						o.pos.z = o.pos.z * 0.0001 + o.pos.w * 0.999;
						#else // OpenGL
						o.pos.z = o.pos.z * 0.0001 - o.pos.w * 0.999;
						#endif
					}
				}
				
				#ifdef POI_PASS_META
				o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
				#endif
				
				return o;
			}
			
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, uv) : POI2D_SAMPLER(tex, texSampler, uv))
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan)) : POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), dx, dy) : POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			#if defined(_STOCHASTICMODE_HEXTILE)
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (useStochastic ? HextileSampleTexture(tex, sampler##texSampler, uv, false) : POI2D_SAMPLER(tex, texSampler, uv))
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (useStochastic ? HextileSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), false) : POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (useStochastic ? HextileSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), false, dx, dy) : POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			
			#ifndef POI2D_SAMPLER_STOCHASTIC
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (POI2D_SAMPLER(tex, texSampler, uv))
			#endif
			#ifndef POI2D_SAMPLER_PAN_STOCHASTIC
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#endif
			#ifndef POI2D_SAMPLER_PANGRAD_STOCHASTIC
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			
			// When using, properties won't properly lock at optimize time; needs macro evaluation implemented
			// #define POI2D_SAMPLER_STOCHASTIC_INLINED(tex, texSampler) (POI2D_SAMPLER_STOCHASTIC(tex, texSampler, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Stochastic))
			// #define POI2D_SAMPLER_PAN_STOCHASTIC_INLINED(tex, texSampler) (POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan, tex##Stochastic))
			
			// #define POI2D_MAINTEX_SAMPLER_STOCHASTIC_INLINED(tex) (POI2D_SAMPLER_STOCHASTIC(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Stochastic))
			// #define POI2D_MAINTEX_SAMPLER_PAN_STOCHASTIC_INLINED(tex) (POI2D_SAMPLER_PAN_STOCHASTIC(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan, tex##Stochastic))
			
			// Deliot, Heitz 2019 - Fast, but non-histogram-preserving (ends up looking a bit blurry and lower contrast)
			// https://eheitzresearch.wordpress.com/738-2/
			
			// Classic Magic Numbers fracsin
			#if !defined(_STOCHASTICMODE_NONE)
			float2 StochasticHash2D2D (float2 s)
			{
				return frac(sin(glsl_mod(float2(dot(s, float2(127.1,311.7)), dot(s, float2(269.5,183.3))), 3.14159)) * 43758.5453);
			}
			#endif
			
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			// UV Offsets and blend weights
			// UVBW[0...2].xy = UV Offsets
			// UVBW[0...2].z = Blend Weights
			float3x3 DeliotHeitzStochasticUVBW(float2 uv)
			{
				// UV transformed into triangular grid space with UV scaled by approximation of 2*sqrt(3)
				const float2x2 stochasticSkewedGrid = float2x2(1.0, -0.57735027, 0.0, 1.15470054);
				float2 skewUV = mul(stochasticSkewedGrid, uv * 3.4641 * _StochasticDeliotHeitzDensity);
				
				// Vertex IDs and barycentric coords
				float2 vxID = floor(skewUV);
				float3 bary = float3(frac(skewUV), 0);
				bary.z = 1.0 - bary.x - bary.y;
				
				float3x3 pos = float3x3(
				float3(vxID, 				bary.z),
				float3(vxID + float2(0, 1), bary.y),
				float3(vxID + float2(1, 0), bary.x)
				);
				
				float3x3 neg = float3x3(
				float3(vxID + float2(1, 1), 	 -bary.z),
				float3(vxID + float2(1, 0), 1.0 - bary.y),
				float3(vxID + float2(0, 1), 1.0 - bary.x)
				);
				
				return (bary.z > 0) ? pos : neg;
			}
			
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, float2 dx, float2 dy)
			{
				// UVBW[0...2].xy = UV Offsets
				// UVBW[0...2].z = Blend Weights
				float3x3 UVBW = DeliotHeitzStochasticUVBW(uv);
				
				//blend samples with calculated weights
				return 	mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[0].xy), dx, dy), UVBW[0].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[1].xy), dx, dy), UVBW[1].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[2].xy), dx, dy), UVBW[2].z) ;
			}
			
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv)
			{
				float2 dx = ddx(uv), dy = ddy(uv);
				return DeliotHeitzSampleTexture(tex, texSampler, uv, dx, dy);
			}
			#endif // defined(_STOCHASTICMODE_DELIOT_HEITZ)
			
			#if defined(_STOCHASTICMODE_HEXTILE)
			// HexTiling: Slower, but histogram-preserving
			// SPDX-License-Idenfitier: MIT
			// Copyright (c) 2022 mmikk
			// https://github.com/mmikk/hextile-demo
			float2 HextileMakeCenUV(float2 vertex)
			{
				// 0.288675 ~= 1/(2*sqrt(3))
				const float2x2 stochasticInverseSkewedGrid = float2x2(1.0, 0.5, 0.0, 1.0/1.15470054);
				return mul(stochasticInverseSkewedGrid, vertex) * 0.288675;
			}
			
			float2x2 HextileLoadRot2x2(float2 idx, float rotStrength)
			{
				float angle = abs(idx.x * idx.y) + abs(idx.x + idx.y) + PI;
				
				// remap to +/-pi
				angle = glsl_mod(angle, 2 * PI);
				if(angle < 0)  angle += 2 * PI;
				if(angle > PI) angle -= 2 * PI;
				
				angle *= rotStrength;
				
				float cs = cos(angle), si = sin(angle);
				return float2x2(cs, -si, si, cs);
			}
			
			// UV Offsets and base blend weights
			// UVBWR[0...2].xy = UV Offsets
			// UVBWR[0...2].zw = rotation costh/sinth -> reconstruct rotation matrix with float2x2(UVBWR[n].z, -UVBWR[n].w, UVBWR[n].w, UVBWR[n].z)
			// UVBWR[3].xyz = Blend Weights (w unused) - needs luminance weighting
			float4x4 HextileUVBWR(float2 uv)
			{
				// Create Triangle Grid
				// Skew input space into simplex triangle grid (3.4641 ~= 2*sqrt(3))
				const float2x2 stochasticSkewedGrid = float2x2(1.0, -0.57735027, 0.0, 1.15470054);
				float2 skewedCoord = mul(stochasticSkewedGrid, uv * 3.4641 * _StochasticHexGridDensity);
				
				float2 baseId = float2(floor(skewedCoord));
				float3 temp = float3(frac(skewedCoord), 0);
				temp.z = 1 - temp.x - temp.y;
				
				float s = step(0.0, -temp.z);
				float s2 = 2 * s - 1;
				
				float3 weights = float3(-temp.z * s2, s - temp.y * s2, s - temp.x * s2);
				
				float2 vertex0 = baseId + float2(s, s);
				float2 vertex1 = baseId + float2(s, 1 - s);
				float2 vertex2 = baseId + float2(1 - s, s);
				
				float2 cen0 = HextileMakeCenUV(vertex0), cen1 = HextileMakeCenUV(vertex1), cen2 = HextileMakeCenUV(vertex2);
				float2x2 rot0 = float2x2(1, 0, 0, 1), rot1 = float2x2(1, 0, 0, 1), rot2 = float2x2(1, 0, 0, 1);
				
				if(_StochasticHexRotationStrength > 0)
				{
					rot0 = HextileLoadRot2x2(vertex0, _StochasticHexRotationStrength);
					rot1 = HextileLoadRot2x2(vertex1, _StochasticHexRotationStrength);
					rot2 = HextileLoadRot2x2(vertex2, _StochasticHexRotationStrength);
				}
				
				return float4x4(
				float4(mul(uv - cen0, rot0) + cen0 + StochasticHash2D2D(vertex0), rot0[0].x, -rot0[0].y),
				float4(mul(uv - cen1, rot1) + cen1 + StochasticHash2D2D(vertex1), rot1[0].x, -rot1[0].y),
				float4(mul(uv - cen2, rot2) + cen2 + StochasticHash2D2D(vertex2), rot2[0].x, -rot2[0].y),
				float4(weights, 0)
				);
			}
			
			float4 HextileSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, bool isNormalMap, float2 dUVdx, float2 dUVdy)
			{
				// For some reason doing this instead of just calculating it directly prevents it from \
				// breaking after a certain number of textures use it. I don't understand why yet
				float4x4 UVBWR = HextileUVBWR(uv);
				
				// 2D Rotation Matrices for dUVdx/dy
				// Not sure if this constant folds during compiling when rot is locked at 0, so force it
				float2x2 rot0 = float2x2(1, 0, 0, 1), rot1 = float2x2(1, 0, 0, 1), rot2 = float2x2(1, 0, 0, 1);
				
				if(_StochasticHexRotationStrength > 0)
				{
					rot0 = float2x2(UVBWR[0].z, -UVBWR[0].w, UVBWR[0].w, UVBWR[0].z);
					rot1 = float2x2(UVBWR[1].z, -UVBWR[1].w, UVBWR[1].w, UVBWR[1].z);
					rot2 = float2x2(UVBWR[2].z, -UVBWR[2].w, UVBWR[2].w, UVBWR[2].z);
				}
				
				// Weights
				float3 W = UVBWR[3].xyz;
				
				// Sample texture
				// float3x4 c = float3x4(
				// 	tex.SampleGrad(texSampler, UVBWR[0].xy, mul(dUVdx, rot0), mul(dUVdy, rot0)),
				// 	tex.SampleGrad(texSampler, UVBWR[1].xy, mul(dUVdx, rot1), mul(dUVdy, rot1)),
				// 	tex.SampleGrad(texSampler, UVBWR[2].xy, mul(dUVdx, rot2), mul(dUVdy, rot2))
				// );
				
				float4 c0 = tex.SampleGrad(texSampler, UVBWR[0].xy, mul(dUVdx, rot0), mul(dUVdy, rot0));
				float4 c1 = tex.SampleGrad(texSampler, UVBWR[1].xy, mul(dUVdx, rot1), mul(dUVdy, rot1));
				float4 c2 = tex.SampleGrad(texSampler, UVBWR[2].xy, mul(dUVdx, rot2), mul(dUVdy, rot2));
				
				// Blend samples using luminance
				// This is technically incorrect for normal maps, but produces very similar
				// results to blending using normal map gradients (steepness)
				const float3 Lw = float3(0.299, 0.587, 0.114);
				float3 Dw = float3(dot(c0.xyz, Lw), dot(c1.xyz, Lw), dot(c2.xyz, Lw));
				
				Dw = lerp(1.0, Dw, _StochasticHexFallOffContrast);
				W = Dw * pow(W, _StochasticHexFallOffPower);
				// In the original hextiling there's a Gain3 step here, but it seems to slow things down \
				// and cause the UVs to break, so I've omitted it. Looks fine without
				
				W /= (W.x + W.y + W.z);
				return W.x * c0 + W.y * c1 + W.z * c2;
			}
			
			float4 HextileSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, bool isNormalMap)
			{
				return HextileSampleTexture(tex, texSampler, uv, isNormalMap, ddx(uv), ddy(uv));
			}
			#endif // defined(_STOCHASTICMODE_HEXTILE)
			
			void applyAlphaOptions(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiMods poiMods)
			{
				poiFragData.alpha = saturate(poiFragData.alpha + _AlphaMod);
				
				if (_AlphaGlobalMask > 0)
				{
					poiFragData.alpha = maskBlend(poiFragData.alpha, poiMods.globalMask[_AlphaGlobalMask - 1], _AlphaGlobalMaskBlendType);
				}
				
				//ifex _AlphaDistanceFade==0 && isNotAnimated(_AlphaDistanceFade)
				if (_AlphaDistanceFade)
				{
					float3 position = _AlphaDistanceFadeType ? poiMesh.worldPos : poiMesh.objectPosition;
					float distanceFadeMultiplier = lerp(_AlphaDistanceFadeMinAlpha, _AlphaDistanceFadeMaxAlpha, smoothstep(_AlphaDistanceFadeMin, _AlphaDistanceFadeMax, distance(position, poiCam.worldPos)));
					if (_AlphaDistanceFadeGlobalMask > 0)
					{
						distanceFadeMultiplier = lerp(1, distanceFadeMultiplier, poiMods.globalMask[_AlphaDistanceFadeGlobalMask - 1]);
					}
					poiFragData.alpha *= distanceFadeMultiplier;
				}
				//endex
				
				//ifex _AlphaFresnel==0 && isNotAnimated(_AlphaFresnel)
				if (_AlphaFresnel)
				{
					float holoRim = saturate(1 - smoothstep(min(_AlphaFresnelSharpness, _AlphaFresnelWidth), _AlphaFresnelWidth, (poiCam.vDotN)));
					holoRim = abs(lerp(1, holoRim, _AlphaFresnelAlpha));
					holoRim = _AlphaFresnelInvert ? 1 - holoRim : holoRim;
					if (_AlphaFresnelGlobalMask > 0)
					{
						holoRim = lerp(1, holoRim, poiMods.globalMask[_AlphaFresnelGlobalMask - 1]);
					}
					poiFragData.alpha *= holoRim;
				}
				//endex
				
				//ifex _AlphaAngular==0 && isNotAnimated(_AlphaAngular)
				if (_AlphaAngular)
				{
					half cameraAngleMin = _CameraAngleMin / 180;
					half cameraAngleMax = _CameraAngleMax / 180;
					half modelAngleMin = _ModelAngleMin / 180;
					half modelAngleMax = _ModelAngleMax / 180;
					float3 pos = _AngleCompareTo == 0 ? poiMesh.objectPosition : poiMesh.worldPos;
					half3 cameraToModelDirection = normalize(pos - getCameraPosition());
					half3 modelForwardDirection = normalize(mul(unity_ObjectToWorld, normalize(_AngleForwardDirection.rgb)));
					half cameraLookAtModel = remapClamped(cameraAngleMax, cameraAngleMin, .5 * dot(cameraToModelDirection, getCameraForward()) + .5);
					half modelLookAtCamera = remapClamped(modelAngleMax, modelAngleMin, .5 * dot(-cameraToModelDirection, modelForwardDirection) + .5);
					float angularAlphaMod = 1;
					if (_AngleType == 0)
					{
						angularAlphaMod = max(cameraLookAtModel, _AngleMinAlpha);
					}
					else if (_AngleType == 1)
					{
						angularAlphaMod = max(modelLookAtCamera, _AngleMinAlpha);
					}
					else if (_AngleType == 2)
					{
						angularAlphaMod = max(cameraLookAtModel * modelLookAtCamera, _AngleMinAlpha);
					}
					if (_AlphaAngularGlobalMask > 0)
					{
						angularAlphaMod = lerp(1, angularAlphaMod, poiMods.globalMask[_AlphaAngularGlobalMask - 1]);
					}
					poiFragData.alpha *= angularAlphaMod;
				}
				//endex
				
				//ifex _AlphaAudioLinkEnabled==0 && isNotAnimated(_AlphaAudioLinkEnabled)
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable && _AlphaAudioLinkEnabled)
				{
					poiFragData.alpha = saturate(poiFragData.alpha + lerp(_AlphaAudioLinkAddRange.x, _AlphaAudioLinkAddRange.y, poiMods.audioLink[_AlphaAudioLinkAddBand]));
				}
				#endif
				//endex
				
			}
			
			//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
			inline half Dither8x8Bayer(int x, int y)
			{
				// Premultiplied by 1/64
				const half dither[ 64 ] = {
					0.015625, 0.765625, 0.203125, 0.953125, 0.06250, 0.81250, 0.25000, 1.00000,
					0.515625, 0.265625, 0.703125, 0.453125, 0.56250, 0.31250, 0.75000, 0.50000,
					0.140625, 0.890625, 0.078125, 0.828125, 0.18750, 0.93750, 0.12500, 0.87500,
					0.640625, 0.390625, 0.578125, 0.328125, 0.68750, 0.43750, 0.62500, 0.37500,
					0.046875, 0.796875, 0.234375, 0.984375, 0.03125, 0.78125, 0.21875, 0.96875,
					0.546875, 0.296875, 0.734375, 0.484375, 0.53125, 0.28125, 0.71875, 0.46875,
					0.171875, 0.921875, 0.109375, 0.859375, 0.15625, 0.90625, 0.09375, 0.84375,
					0.671875, 0.421875, 0.609375, 0.359375, 0.65625, 0.40625, 0.59375, 0.34375
				};
				int r = y * 8 + x;
				return dither[r];
			}
			
			half calcDither(half2 grabPos)
			{
				return Dither8x8Bayer(glsl_mod(grabPos.x, 8), glsl_mod(grabPos.y, 8));
			}
			
			void applyDithering(inout PoiFragData poiFragData, in PoiCam poiCam)
			{
				if (_AlphaDithering)
				{
					float dither = calcDither(poiCam.posScreenPixels) - _AlphaDitherBias;
					poiFragData.alpha = saturate(poiFragData.alpha - (dither * (1 - poiFragData.alpha) * _AlphaDitherGradient));
				}
			}
			//endex
			
			//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
			void ApplyAlphaToCoverage(inout PoiFragData poiFragData, in PoiMesh poiMesh)
			{
				// Force Model Opacity to 1 if desired
				UNITY_BRANCH
				if (_Mode == 1)
				{
					UNITY_BRANCH
					if (_AlphaSharpenedA2C && _AlphaToCoverage)
					{
						// rescale alpha by mip level
						poiFragData.alpha *= 1 + max(0, CalcMipLevel(poiMesh.uv[0] * _MainTex_TexelSize.zw)) * _AlphaMipScale;
						// rescale alpha by partial derivative
						poiFragData.alpha = (poiFragData.alpha - _Cutoff) / max(fwidth(poiFragData.alpha), 0.0001) + _Cutoff;
						poiFragData.alpha = saturate(poiFragData.alpha);
					}
				}
			}
			//endex
			
			//ifex _PoiParallax==0
			#ifdef POI_PARALLAX
			inline float2 POM(in PoiLight poiLight, sampler2D heightMap, in PoiMesh poiMesh, float3 worldViewDir, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv)
			{
				#if defined(PROP_HEIGHTMASK) || !defined(OPTIMIZER_ENABLED)
				float heightMask = POI2D_SAMPLER_PAN(_Heightmask, _linear_repeat, poiUV(poiMesh.uv[_HeightmaskUV], _Heightmask_ST), _HeightmaskPan)[_HeightmaskChannel];
				if (_HeightmaskInvert)
				{
					heightMask = 1 - heightMask;
				}
				#else
				float heightMask = 1;
				#endif
				
				float2 uvs = poiUV(poiMesh.uv[_HeightMapUV], _HeightMap_ST);
				float2 dx = ddx(uvs);
				float2 dy = ddy(uvs);
				float3 result = 0;
				int stepIndex = 0;
				int numSteps = (int)lerp(maxSamples, minSamples, saturate(dot(poiMesh.normals[0], worldViewDir)));
				float layerHeight = 1.0 / numSteps;
				float2 plane = parallax * heightMask * (viewDirTan.xy / viewDirTan.z);
				uvs += refPlane * plane;
				float2 deltaTex = -plane * layerHeight;
				float2 prevTexOffset = 0;
				float prevRayZ = 1.0f;
				float prevHeight = 0.0f;
				float2 currTexOffset = deltaTex;
				float currRayZ = 1.0f - layerHeight;
				float currHeight = 0.0f;
				float intersection = 0;
				float2 finalTexOffset = 0;
				while (stepIndex < numSteps + 1)
				{
					result.z = dot(curv, currTexOffset * currTexOffset);
					currHeight = tex2Dgrad(heightMap, uvs + currTexOffset, dx, dy).r * (1 - result.z);
					if (currHeight > currRayZ)
					{
						stepIndex = numSteps + 1;
					}
					else
					{
						stepIndex++;
						prevTexOffset = currTexOffset;
						prevRayZ = currRayZ;
						prevHeight = currHeight;
						currTexOffset += deltaTex;
						currRayZ -= layerHeight * (1 - result.z) * (1 + _CurvFix);
					}
				}
				int sectionSteps = 10;
				int sectionIndex = 0;
				float newZ = 0;
				float newHeight = 0;
				while (sectionIndex < sectionSteps)
				{
					intersection = (prevHeight - prevRayZ) / (prevHeight - currHeight + currRayZ - prevRayZ);
					finalTexOffset = prevTexOffset +intersection * deltaTex;
					newZ = prevRayZ - intersection * layerHeight;
					newHeight = tex2Dgrad(heightMap, uvs + finalTexOffset, dx, dy).r;
					if (newHeight > newZ)
					{
						currTexOffset = finalTexOffset;
						currHeight = newHeight;
						currRayZ = newZ;
						deltaTex = intersection * deltaTex;
						layerHeight = intersection * layerHeight;
					}
					else
					{
						prevTexOffset = finalTexOffset;
						prevHeight = newHeight;
						prevRayZ = newZ;
						deltaTex = (1 - intersection) * deltaTex;
						layerHeight = (1 - intersection) * layerHeight;
					}
					sectionIndex++;
				}
				#ifdef UNITY_PASS_SHADOWCASTER
				if (unity_LightShadowBias.z == 0.0)
				{
					#endif
					if (result.z > 1)
					clip(-1);
					#ifdef UNITY_PASS_SHADOWCASTER
				}
				#endif
				
				return uvs + finalTexOffset;
			}
			/*
			float2 ParallaxOffsetMultiStep(float surfaceHeight, float strength, float2 uv, float3 tangentViewDir)
			{
				float2 uvOffset = 0;
				float2 prevUVOffset = 0;
				float stepSize = 1.0 / _HeightSteps;
				float stepHeight = 1;
				float2 uvDelta = tangentViewDir.xy * (stepSize * strength);
				float prevStepHeight = stepHeight;
				float prevSurfaceHeight = surfaceHeight;
				
				[unroll(20)]
				for (int j = 1; j <= _HeightSteps && stepHeight > surfaceHeight; j++)
				{
					prevUVOffset = uvOffset;
					prevStepHeight = stepHeight;
					prevSurfaceHeight = surfaceHeight;
					uvOffset -= uvDelta;
					stepHeight -= stepSize;
					surfaceHeight = POI2D_SAMPLER_PAN(_Heightmap, _MainTex, poiUV(uv + uvOffset, _Heightmap_ST), _HeightmapPan) + _HeightOffset;
				}
				
				[unroll(3)]
				for (int k = 0; k < 3; k++)
				{
					uvDelta *= 0.5;
					stepSize *= 0.5;
					
					if (stepHeight < surfaceHeight)
					{
						uvOffset += uvDelta;
						stepHeight += stepSize;
					}
					else
					{
						uvOffset -= uvDelta;
						stepHeight -= stepSize;
					}
					surfaceHeight = POI2D_SAMPLER_PAN(_Heightmap, _MainTex, poiUV(uv + uvOffset, _Heightmap_ST), _HeightmapPan) + _HeightOffset;
				}
				return uvOffset;
			}
			*/
			void applyParallax(inout PoiMesh poiMesh, in PoiLight poiLight, in PoiCam poiCam)
			{
				/*
				half h = POI2D_SAMPLER_PAN(_Heightmap, _linear_repeat, poiUV(poiMesh.uv[_HeightmaskUV], _Heightmap_ST), _HeightmapPan).r + _HeightOffset;
				#if defined(PROP_HEIGHTMASK) || !defined(OPTIMIZER_ENABLED)
				half m = POI2D_SAMPLER_PAN(_Heightmask, _linear_repeat, poiUV(poiMesh.uv[_HeightmaskUV], _Heightmask_ST), _HeightmaskPan).r + _HeightOffset;
				#else
				half m = 1 + _HeightOffset;
				#endif
				h = clamp(h, 0, 0.999);
				m = lerp(m, 1 - m, _HeightmaskInvert);
				#if defined(OPTIMIZER_ENABLED)das
				poiMesh.uv[_ParallaxUV] += ParallaxOffsetMultiStep(h, _HeightStrength * m, poiMesh.uv[_HeightmapUV], tangentViewDir / tangentViewDir.z);
				#else
				float2 offset = ParallaxOffsetMultiStep(h, _HeightStrength * m, poiMesh.uv[_HeightmapUV], tangentViewDir / tangentViewDir.z);
				if (_ParallaxUV == 0)       poiMesh.uv[0] += offset;
				if (_ParallaxUV == 1)       poiMesh.uv[1] += offset;
				if (_ParallaxUV == 2)       poiMesh.uv[2] += offset;
				if (_ParallaxUV == 3)       poiMesh.uv[3] += offset;
				if (_ParallaxUV == 4)       poiMesh.uv[4] += offset;
				if (_ParallaxUV == 5)       poiMesh.uv[5] += offset;
				if (_ParallaxUV == 6)       poiMesh.uv[6] += offset;
				if (_ParallaxUV == 7)       poiMesh.uv[7] += offset;
				#endif
				*/
				
				#if defined(OPTIMIZER_ENABLED)
				poiMesh.uv[_ParallaxUV] = POM(poiLight, _HeightMap, poiMesh, poiCam.viewDir, poiCam.tangentViewDir, _HeightStepsMin, _HeightStepsMax, _HeightStrength, 0, _HeightMap_ST.xy, float2(_CurvatureU, _CurvatureV));
				#else
				float2 offset = POM(poiLight, _HeightMap, poiMesh, poiCam.viewDir, poiCam.tangentViewDir, _HeightStepsMin, _HeightStepsMax, _HeightStrength, 0, _HeightMap_ST.xy, float2(_CurvatureU, _CurvatureV));
				if (_ParallaxUV == 0)       poiMesh.uv[0] = offset;
				if (_ParallaxUV == 1)       poiMesh.uv[1] = offset;
				if (_ParallaxUV == 2)       poiMesh.uv[2] = offset;
				if (_ParallaxUV == 3)       poiMesh.uv[3] = offset;
				if (_ParallaxUV == 4)       poiMesh.uv[4] = offset;
				if (_ParallaxUV == 5)       poiMesh.uv[5] = offset;
				if (_ParallaxUV == 6)       poiMesh.uv[6] = offset;
				if (_ParallaxUV == 7)       poiMesh.uv[7] = offset;
				#endif
			}
			#endif
			//endex
			
			//ifex _GlitterEnable==0
			#ifdef _SUNDISK_SIMPLE
			
			float3 RandomColorFromPoint(float2 rando)
			{
				fixed hue = random2(rando.x + rando.y).x;
				fixed saturation = lerp(_GlitterMinMaxSaturation.x, _GlitterMinMaxSaturation.y, rando.x);
				fixed value = lerp(_GlitterMinMaxBrightness.x, _GlitterMinMaxBrightness.y, rando.y);
				float3 hsv = float3(hue, saturation, value);
				return HSVtoRGB(hsv);
			}
			
			void applyGlitter(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiLight poiLight, in PoiMods poiMods)
			{
				for (int glitterLayer = 0; glitterLayer < _GlitterLayers; glitterLayer++)
				{
					
					// Scale
					
					float2 st = (poiMesh.uv[_GlitterUV] + _GlitterUVPanning.xy * _Time.x) * _GlitterFrequency;
					
					// Tile the space
					float2 i_st = floor(st);
					float2 f_st = frac(st);
					
					float m_dist = 10.;  // minimun distance
					float2 m_point = 0;        // minimum point
					float2 randoPoint = 0;
					float2 dank = 0;
					for (int j = -1; j <= 1; j++)
					{
						for (int i = -1; i <= 1; i++)
						{
							float2 neighbor = float2(i, j);
							float2 pos = random2(i_st + neighbor + glitterLayer * 0.5141);
							float2 rando = pos;
							pos = pos * _GlitterRandomLocation;
							float2 diff = neighbor + pos - f_st;
							float dist = length(diff);
							
							if (dist < m_dist)
							{
								dank = diff;
								m_dist = dist;
								m_point = pos;
								randoPoint = rando;
							}
						}
					}
					
					float randomFromPoint = random(randoPoint);
					
					float size = _GlitterSize;
					UNITY_BRANCH
					if (_GlitterRandomSize)
					{
						size = lerp(_GlitterMinMaxSize.x, _GlitterMinMaxSize.y, randomFromPoint);
					}
					
					// Assign a color using the closest point position
					//color += dot(m_point, float2(.3, .6));
					
					// Add distance field to closest point center
					// color.g = m_dist;
					
					// Show isolines
					//color -= abs(sin(40.0 * m_dist)) * 0.07;
					
					// Draw cell center
					half glitterAlpha = 1;
					switch(_GlitterShape)
					{
						case 0: //circle
						glitterAlpha = saturate((size - m_dist) / clamp(fwidth(m_dist), 0.0001, 1.0));
						break;
						case 1: //sqaure
						float jaggyFix = pow(poiCam.distanceToVert, 2) * _GlitterJaggyFix;
						UNITY_BRANCH
						if (_GlitterRandomRotation == 1 || _GlitterTextureRotation != 0 || _GlitterRandomRotationSpeed.x != 0 || _GlitterRandomRotationSpeed.y != 0)
						{
							float2 center = float2(0, 0);
							float randomBoy = 0;
							UNITY_BRANCH
							if (_GlitterRandomRotation || _GlitterRandomRotationSpeed.x != 0 || _GlitterRandomRotationSpeed.y != 0)
							{
								randomBoy = random(m_point * 200);
							}
							float theta = radians((randomBoy + _Time.x * (_GlitterTextureRotation + lerp(_GlitterRandomRotationSpeed.x, _GlitterRandomRotationSpeed.y, randomBoy))) * 360);
							float cs = cos(theta);
							float sn = sin(theta);
							dank = float2((dank.x - center.x) * cs - (dank.y - center.y) * sn + center.x, (dank.x - center.x) * sn + (dank.y - center.y) * cs + center.y);
							glitterAlpha = (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.x))) * (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.y)));
						}
						else
						{
							glitterAlpha = (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.x))) * (1. - smoothstep(size - .1 * jaggyFix, size, abs(dank.y)));
						}
						break;
					}
					
					float3 finalGlitter = 0;
					
					half3 glitterColor = poiThemeColor(poiMods, _GlitterColor, _GlitterColorThemeIndex);
					
					float3 norm = lerp(poiMesh.normals[0], poiMesh.normals[1], _GlitterUseNormals);
					float3 randomRotation = 0;
					switch(_GlitterMode)
					{
						case 0:
						UNITY_BRANCH
						if (_GlitterSpeed > 0)
						{
							randomRotation = randomFloat3WiggleRange(randoPoint, _GlitterAngleRange, _GlitterSpeed);
						}
						else
						{
							randomRotation = randomFloat3Range(randoPoint, _GlitterAngleRange);
						}
						
						float3 glitterReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
						finalGlitter = lerp(0, _GlitterMinBrightness * glitterAlpha, glitterAlpha) + max(pow(saturate(dot(lerp(glitterReflectionDirection, poiCam.viewDir, _GlitterBias), poiCam.viewDir)), _GlitterContrast), 0);
						finalGlitter *= glitterAlpha;
						break;
						case 1:
						float randomOffset = random(randoPoint);
						float brightness = (sin((_Time.x * 10 + randomOffset) * _GlitterSpeed) * .5 + .5);
						finalGlitter = max(_GlitterMinBrightness * glitterAlpha, brightness * glitterAlpha * smoothstep(0, 1, 1 - m_dist * _GlitterCenterSize * 10));
						break;
						case 2:
						if (_GlitterSpeed > 0)
						{
							randomRotation = randomFloat3WiggleRange(randoPoint, _GlitterAngleRange, _GlitterSpeed);
						}
						else
						{
							randomRotation = randomFloat3Range(randoPoint, _GlitterAngleRange);
						}
						
						float3 glitterLightReflectionDirection = normalize(mul(poiRotationMatrixFromAngles(randomRotation), norm));
						
						#ifdef POI_PASS_ADD
						glitterAlpha *= poiLight.nDotLSaturated * poiLight.attenuation;
						#endif
						#ifdef UNITY_PASS_FORWARDBASE
						glitterAlpha *= poiLight.nDotLSaturated;
						#endif
						
						float3 halfDir = normalize(poiLight.direction + poiCam.viewDir);
						float specAngle = max(dot(halfDir, glitterLightReflectionDirection), 0.0);
						
						finalGlitter = lerp(0, _GlitterMinBrightness * glitterAlpha, glitterAlpha) + max(pow(specAngle, _GlitterContrast), 0);
						
						glitterColor *= poiLight.directColor;
						finalGlitter *= glitterAlpha;
						
						break;
					}
					
					glitterColor *= lerp(1, poiFragData.baseColor, _GlitterUseSurfaceColor);
					#if defined(PROP_GLITTERCOLORMAP) || !defined(OPTIMIZER_ENABLED)
					glitterColor *= POI2D_SAMPLER_PAN(_GlitterColorMap, _MainTex, poiUV(poiMesh.uv[_GlitterColorMapUV], _GlitterColorMap_ST), _GlitterColorMapPan).rgb;
					#endif
					float2 uv = remapClamped(-size, size, dank, 0, 1);
					UNITY_BRANCH
					
					if (_GlitterRandomRotation == 1 || _GlitterTextureRotation != 0 || _GlitterRandomRotationSpeed.x != 0 || _GlitterRandomRotationSpeed.y && !_GlitterShape)
					{
						float2 fakeUVCenter = float2(.5, .5);
						float randomBoy = 0;
						UNITY_BRANCH
						if (_GlitterRandomRotation || _GlitterRandomRotationSpeed.x != 0 || _GlitterRandomRotationSpeed.y != 0)
						{
							randomBoy = random(randoPoint * 20);
						}
						float theta = radians((randomBoy + _Time.x * (_GlitterTextureRotation + lerp(_GlitterRandomRotationSpeed.x, _GlitterRandomRotationSpeed.y, randomBoy))) * 360);
						float cs = cos(theta);
						float sn = sin(theta);
						uv = float2((uv.x - fakeUVCenter.x) * cs - (uv.y - fakeUVCenter.y) * sn + fakeUVCenter.x, (uv.x - fakeUVCenter.x) * sn + (uv.y - fakeUVCenter.y) * cs + fakeUVCenter.y);
					}
					
					#if defined(PROP_GLITTERTEXTURE) || !defined(OPTIMIZER_ENABLED)
					float4 glitterTexture = POI2D_SAMPLER_PANGRAD(_GlitterTexture, _linear_clamp, poiUV(uv, _GlitterTexture_ST), _GlitterTexturePan, poiMesh.dx, poiMesh.dy);
					#else
					float4 glitterTexture = 1;
					#endif
					//float4 glitterTexture = _GlitterTexture.SampleGrad(sampler_MainTex, frac(uv), ddx(uv), ddy(uv));
					glitterColor *= glitterTexture.rgb;
					#if defined(PROP_GLITTERMASK) || !defined(OPTIMIZER_ENABLED)
					float glitterMask = POI2D_SAMPLER_PAN(_GlitterMask, _MainTex, poiUV(poiMesh.uv[_GlitterMaskUV], _GlitterMask_ST), _GlitterMaskPan)[_GlitterMaskChannel];
					#else
					float glitterMask = 1;
					#endif
					
					if (_GlitterMaskInvert)
					{
						glitterMask = 1 - glitterMask;
					}
					
					glitterMask *= lerp(1, poiLight.rampedLightMap, _GlitterHideInShadow);
					glitterMask *= lerp(1, poiLight.directLuminance, _GlitterScaleWithLighting);
					
					if (_GlitterMaskGlobalMask > 0)
					{
						glitterMask = maskBlend(glitterMask, poiMods.globalMask[_GlitterMaskGlobalMask - 1], _GlitterMaskGlobalMaskBlendType);
					}
					
					if (_GlitterRandomColors)
					{
						glitterColor *= RandomColorFromPoint(random2(randoPoint.x + randoPoint.y));
					}
					
					UNITY_BRANCH
					if (_GlitterHueShiftEnabled)
					{
						glitterColor.rgb = hueShift(glitterColor.rgb, _GlitterHueShift + _Time.x * _GlitterHueShiftSpeed);
					}
					
					UNITY_BRANCH
					if (_GlitterBlendType == 1)
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, finalGlitter * glitterColor * _GlitterBrightness, finalGlitter * glitterTexture.a * glitterMask);
						poiFragData.emission += finalGlitter * glitterColor * max(0, (_GlitterBrightness - 1) * glitterTexture.a) * glitterMask;
					}
					else
					{
						poiFragData.emission += finalGlitter * glitterColor * _GlitterBrightness * glitterTexture.a * glitterMask;
					}
				}
			}
			#endif
			//endex
			
			float4 frag(VertexOut i, uint facing : SV_IsFrontFace) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				
				PoiMesh poiMesh;
				PoiInitStruct(PoiMesh, poiMesh);
				
				PoiLight poiLight;
				PoiInitStruct(PoiLight, poiLight);
				
				PoiVertexLights poiVertexLights;
				PoiInitStruct(PoiVertexLights, poiVertexLights);
				
				PoiCam poiCam;
				PoiInitStruct(PoiCam, poiCam);
				
				PoiMods poiMods;
				PoiInitStruct(PoiMods, poiMods);
				poiMods.globalEmission = 1;
				
				PoiFragData poiFragData;
				poiFragData.smoothness = 1;
				poiFragData.smoothness2 = 1;
				poiFragData.metallic = 1;
				poiFragData.specularMask = 1;
				poiFragData.reflectionMask = 1;
				poiFragData.emission = 0;
				poiFragData.baseColor = float3(0, 0, 0);
				poiFragData.finalColor = float3(0, 0, 0);
				poiFragData.alpha = 1;
				poiFragData.toggleVertexLights = 0;
				
				// Mesh Data
				poiMesh.objectPosition = mul(unity_ObjectToWorld, float3(0, 0, 0)).xyz;
				poiMesh.objNormal = mul(unity_WorldToObject, i.normal);
				poiMesh.normals[0] = i.normal;
				poiMesh.tangent[0] = i.tangent.xyz;
				poiMesh.binormal[0] = cross(i.normal, i.tangent.xyz) * (i.tangent.w * unity_WorldTransformParams.w);
				poiMesh.worldPos = i.worldPos.xyz;
				poiMesh.localPos = i.localPos.xyz;
				poiMesh.vertexColor = i.vertexColor;
				poiMesh.isFrontFace = facing;
				poiMesh.dx = ddx(poiMesh.uv[0]);
				poiMesh.dy = ddy(poiMesh.uv[0]);
				
				#ifndef POI_PASS_OUTLINE
				if (!poiMesh.isFrontFace && _FlipBackfaceNormals)
				{
					poiMesh.normals[0] *= -1;
					poiMesh.tangent[0] *= -1;
					poiMesh.binormal[0] *= -1;
				}
				#endif
				
				poiCam.viewDir = !IsOrthographicCamera() ? normalize(_WorldSpaceCameraPos - i.worldPos.xyz) : normalize(UNITY_MATRIX_I_V._m02_m12_m22);
				float3 tanToWorld0 = float3(poiMesh.tangent[0].x, poiMesh.binormal[0].x, poiMesh.normals[0].x);
				float3 tanToWorld1 = float3(poiMesh.tangent[0].y, poiMesh.binormal[0].y, poiMesh.normals[0].y);
				float3 tanToWorld2 = float3(poiMesh.tangent[0].z, poiMesh.binormal[0].z, poiMesh.normals[0].z);
				float3 ase_tanViewDir = tanToWorld0 * poiCam.viewDir.x + tanToWorld1 * poiCam.viewDir.y + tanToWorld2 * poiCam.viewDir.z;
				poiCam.tangentViewDir = normalize(ase_tanViewDir);
				
				// 0-3 UV0-UV3
				// 4 Panosphere UV
				// 5 world pos xz
				// 6 Polar UV
				// 6 Distorted UV
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
				poiMesh.lightmapUV = i.lightmapUV;
				#endif
				poiMesh.parallaxUV = poiCam.tangentViewDir.xy / max(poiCam.tangentViewDir.z, 0.0001);
				poiMesh.uv[0] = i.uv[0].xy;
				poiMesh.uv[1] = i.uv[0].zw;
				poiMesh.uv[2] = i.uv[1].xy;
				poiMesh.uv[3] = i.uv[1].zw;
				poiMesh.uv[4] = poiMesh.uv[0];
				poiMesh.uv[5] = poiMesh.uv[0];
				poiMesh.uv[6] = poiMesh.uv[0];
				poiMesh.uv[7] = poiMesh.uv[0];
				poiMesh.uv[8] = poiMesh.uv[0];
				
				//ifex _PoiParallax==0
				#ifdef POI_PARALLAX
				#ifndef POI_PASS_OUTLINE
				//return frac(i.tangentViewDir.x);
				//return float4(i.binormal.xyz,1);
				applyParallax(poiMesh, poiLight, poiCam);
				#endif
				#endif
				//endex
				
				float2 mainUV = poiUV(poiMesh.uv[_MainTexUV].xy, _MainTex_ST);
				
				if (_MainPixelMode)
				{
					mainUV = sharpSample(_MainTex_TexelSize, mainUV);
				}
				
				float4 mainTexture = POI2D_SAMPLER_PAN_STOCHASTIC(_MainTex, _MainTex, mainUV, _MainTexPan, _MainTexStochastic);
				
				#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
				poiMesh.tangentSpaceNormal = UnpackScaleNormal(POI2D_SAMPLER_PAN_STOCHASTIC(_BumpMap, _MainTex, poiUV(poiMesh.uv[_BumpMapUV].xy, _BumpMap_ST), _BumpMapPan, _BumpMapStochastic), _BumpScale);
				#else
				poiMesh.tangentSpaceNormal = UnpackNormal(float4(0.5, 0.5, 1, 1));
				#endif
				
				poiMesh.normals[1] = normalize(
				poiMesh.tangentSpaceNormal.x * poiMesh.tangent[0] +
				poiMesh.tangentSpaceNormal.y * poiMesh.binormal[0] +
				poiMesh.tangentSpaceNormal.z * poiMesh.normals[0]
				);
				
				poiMesh.tangent[1] = cross(poiMesh.binormal[0], -poiMesh.normals[1]);
				poiMesh.binormal[1] = cross(-poiMesh.normals[1], poiMesh.tangent[0]);
				
				// Camera data
				poiCam.forwardDir = getCameraForward();
				poiCam.worldPos = _WorldSpaceCameraPos;
				poiCam.reflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[1]);
				poiCam.vertexReflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[0]);
				//poiCam.distanceToModel = distance(poiMesh.modelPos, poiCam.worldPos);
				poiCam.clipPos = i.pos;
				poiCam.distanceToVert = distance(poiMesh.worldPos, poiCam.worldPos);
				poiCam.posScreenSpace = poiTransformClipSpacetoScreenSpaceFrag(poiCam.clipPos);
				#if defined(POI_GRABPASS) && defined(POI_PASS_BASE)
				poiCam.screenUV = poiCam.clipPos.xy / poiGetWidthAndHeight(_PoiGrab2);
				#endif
				poiCam.posScreenPixels = calcPixelScreenUVs(poiCam.posScreenSpace);
				poiCam.vDotN = abs(dot(poiCam.viewDir, poiMesh.normals[1]));
				
				poiCam.worldDirection.xyz = poiMesh.worldPos.xyz - poiCam.worldPos;
				poiCam.worldDirection.w = dot(poiCam.clipPos, CalculateFrustumCorrection());
				
				poiLight.finalLightAdd = 0;
				
				// Ambient Occlusion
				#if defined(PROP_LIGHTINGAOMAPS) || !defined(OPTIMIZER_ENABLED)
				float4 AOMaps = POI2D_SAMPLER_PAN(_LightingAOMaps, _MainTex, poiUV(poiMesh.uv[_LightingAOMapsUV], _LightingAOMaps_ST), _LightingAOMapsPan);
				poiLight.occlusion = min(min(min(lerp(1, AOMaps.r, _LightDataAOStrengthR), lerp(1, AOMaps.g, _LightDataAOStrengthG)), lerp(1, AOMaps.b, _LightDataAOStrengthB)), lerp(1, AOMaps.a, _LightDataAOStrengthA));
				#else
				poiLight.occlusion = 1;
				#endif
				
				if (_LightDataAOGlobalMaskR > 0)
				{
					poiLight.occlusion = maskBlend(poiLight.occlusion, poiMods.globalMask[_LightDataAOGlobalMaskR - 1], _LightDataAOGlobalMaskBlendTypeR);
				}
				
				// Detail Shadows
				#if defined(PROP_LIGHTINGDETAILSHADOWMAPS) || !defined(OPTIMIZER_ENABLED)
				float4 DetailShadows = POI2D_SAMPLER_PAN(_LightingDetailShadowMaps, _MainTex, poiUV(poiMesh.uv[_LightingDetailShadowMapsUV], _LightingDetailShadowMaps_ST), _LightingDetailShadowMapsPan);
				#ifndef POI_PASS_ADD
				poiLight.detailShadow = lerp(1, DetailShadows.r, _LightingDetailShadowStrengthR) * lerp(1, DetailShadows.g, _LightingDetailShadowStrengthG) * lerp(1, DetailShadows.b, _LightingDetailShadowStrengthB) * lerp(1, DetailShadows.a, _LightingDetailShadowStrengthA);
				#else
				poiLight.detailShadow = lerp(1, DetailShadows.r, _LightingAddDetailShadowStrengthR) * lerp(1, DetailShadows.g, _LightingAddDetailShadowStrengthG) * lerp(1, DetailShadows.b, _LightingAddDetailShadowStrengthB) * lerp(1, DetailShadows.a, _LightingAddDetailShadowStrengthA);
				#endif
				#else
				poiLight.detailShadow = 1;
				#endif
				
				if (_LightDataDetailShadowGlobalMaskR > 0)
				{
					poiLight.detailShadow = maskBlend(poiLight.detailShadow, poiMods.globalMask[_LightDataDetailShadowGlobalMaskR - 1], _LightDataDetailShadowGlobalMaskBlendTypeR);
				}
				
				// Shadow Masks
				#if defined(PROP_LIGHTINGSHADOWMASKS) || !defined(OPTIMIZER_ENABLED)
				float4 ShadowMasks = POI2D_SAMPLER_PAN(_LightingShadowMasks, _MainTex, poiUV(poiMesh.uv[_LightingShadowMasksUV], _LightingShadowMasks_ST), _LightingShadowMasksPan);
				poiLight.shadowMask = lerp(1, ShadowMasks.r, _LightingShadowMaskStrengthR) * lerp(1, ShadowMasks.g, _LightingShadowMaskStrengthG) * lerp(1, ShadowMasks.b, _LightingShadowMaskStrengthB) * lerp(1, ShadowMasks.a, _LightingShadowMaskStrengthA);
				#else
				poiLight.shadowMask = 1;
				#endif
				if (_LightDataShadowMaskGlobalMaskR > 0)
				{
					poiLight.shadowMask = maskBlend(poiLight.shadowMask, poiMods.globalMask[_LightDataShadowMaskGlobalMaskR - 1], _LightDataShadowMaskGlobalMaskBlendTypeR);
				}
				
				#ifdef UNITY_PASS_FORWARDBASE
				
				bool lightExists = false;
				if (any(_LightColor0.rgb >= 0.002))
				{
					lightExists = true;
				}
				
				if (_LightingVertexLightingEnabled)
				{
					poiFragData.toggleVertexLights = 1;
				}
				if (IsInMirror() && _LightingMirrorVertexLightingEnabled == 0)
				{
					poiFragData.toggleVertexLights = 0;
				}
				
				if (_LightingVertexLightingEnabled)
				{
					#if defined(VERTEXLIGHT_ON)
					float4 toLightX = unity_4LightPosX0 - i.worldPos.x;
					float4 toLightY = unity_4LightPosY0 - i.worldPos.y;
					float4 toLightZ = unity_4LightPosZ0 - i.worldPos.z;
					float4 lengthSq = 0;
					lengthSq += toLightX * toLightX;
					lengthSq += toLightY * toLightY;
					lengthSq += toLightZ * toLightZ;
					
					float4 lightAttenSq = unity_4LightAtten0;
					float4 atten = 1.0 / (1.0 + lengthSq * lightAttenSq);
					float4 vLightWeight = saturate(1 - (lengthSq * lightAttenSq / 25));
					poiLight.vAttenuation = min(atten, vLightWeight * vLightWeight);
					
					poiLight.vDotNL = 0;
					poiLight.vDotNL += toLightX * poiMesh.normals[1].x;
					poiLight.vDotNL += toLightY * poiMesh.normals[1].y;
					poiLight.vDotNL += toLightZ * poiMesh.normals[1].z;
					
					float4 corr = rsqrt(lengthSq);
					poiLight.vertexVDotNL = max(0, poiLight.vDotNL * corr);
					
					poiLight.vertexVDotNL = 0;
					poiLight.vertexVDotNL += toLightX * poiMesh.normals[0].x;
					poiLight.vertexVDotNL += toLightY * poiMesh.normals[0].y;
					poiLight.vertexVDotNL += toLightZ * poiMesh.normals[0].z;
					
					poiLight.vertexVDotNL = max(0, poiLight.vDotNL * corr);
					
					poiLight.vAttenuationDotNL = saturate(poiLight.vAttenuation * saturate(poiLight.vDotNL));
					
					[unroll]
					for (int index = 0; index < 4; index++)
					{
						poiLight.vPosition[index] = float3(unity_4LightPosX0[index], unity_4LightPosY0[index], unity_4LightPosZ0[index]);
						
						float3 vertexToLightSource = poiLight.vPosition[index] - poiMesh.worldPos;
						poiLight.vDirection[index] = normalize(vertexToLightSource);
						//poiLight.vAttenuationDotNL[index] = 1.0 / (1.0 + unity_4LightAtten0[index] * poiLight.vDotNL[index]);
						poiLight.vColor[index] = _LightingAdditiveLimited ? min(_LightingAdditiveLimit, unity_LightColor[index].rgb) : unity_LightColor[index].rgb;
						poiLight.vColor[index] = lerp(poiLight.vColor[index], dot(poiLight.vColor[index], float3(0.299, 0.587, 0.114)), _LightingAdditiveMonochromatic);
						poiLight.vHalfDir[index] = Unity_SafeNormalize(poiLight.vDirection[index] + poiCam.viewDir);
						poiLight.vDotNL[index] = dot(poiMesh.normals[1], poiLight.vDirection[index]);
						poiLight.vCorrectedDotNL[index] = .5 * (poiLight.vDotNL[index] + 1);
						poiLight.vDotLH[index] = saturate(dot(poiLight.vDirection[index], poiLight.vHalfDir[index]));
						
						poiLight.vDotNH[index] = dot(poiMesh.normals[1], poiLight.vHalfDir[index]);
						poiLight.vertexVDotNH[index] = saturate(dot(poiMesh.normals[0], poiLight.vHalfDir[index]));
					}
					#endif
				}
				
				//UNITY_BRANCH
				if (_LightingColorMode == 0) // Poi Custom Light Color
				
				{
					float3 magic = max(BetterSH9(normalize(unity_SHAr + unity_SHAg + unity_SHAb)), 0);
					float3 normalLight = _LightColor0.rgb + BetterSH9(float4(0, 0, 0, 1));
					
					float magiLumi = calculateluminance(magic);
					float normaLumi = calculateluminance(normalLight);
					float maginormalumi = magiLumi + normaLumi;
					
					float magiratio = magiLumi / maginormalumi;
					float normaRatio = normaLumi / maginormalumi;
					
					float target = calculateluminance(magic * magiratio + normalLight * normaRatio);
					float3 properLightColor = magic + normalLight;
					float properLuminance = calculateluminance(magic + normalLight);
					poiLight.directColor = properLightColor * max(0.0001, (target / properLuminance));
					
					poiLight.indirectColor = BetterSH9(float4(lerp(0, poiMesh.normals[1], _LightingIndirectUsesNormals), 1));
				}
				
				//UNITY_BRANCH
				if (_LightingColorMode == 1) // More standard approach to light color
				
				{
					float3 indirectColor = BetterSH9(float4(poiMesh.normals[1], 1));
					if (lightExists)
					{
						poiLight.directColor = _LightColor0.rgb;
						poiLight.indirectColor = indirectColor;
					}
					else
					{
						poiLight.directColor = indirectColor * 0.6;
						poiLight.indirectColor = indirectColor * 0.5;
					}
				}
				
				if (_LightingColorMode == 2) // UTS style
				
				{
					poiLight.indirectColor = saturate(max(half3(0.05, 0.05, 0.05) * _Unlit_Intensity, max(ShadeSH9(half4(0.0, 0.0, 0.0, 1.0)), ShadeSH9(half4(0.0, -1.0, 0.0, 1.0)).rgb) * _Unlit_Intensity));
					poiLight.directColor = max(poiLight.indirectColor, _LightColor0.rgb);
				}
				
				if (_LightingColorMode == 3) // OpenLit
				
				{
					float3 lightDirectionForSH9 = OpenLitLightingDirectionForSH9();
					OpenLitShadeSH9ToonDouble(lightDirectionForSH9, poiLight.directColor, poiLight.indirectColor);
					poiLight.directColor += _LightColor0.rgb;
					// OpenLit does a few other things by default like clamp direct colour
					// see https://github.com/lilxyzw/OpenLit/blob/main/Assets/OpenLit/core.hlsl#L174
					// Should we add (if) statements to override Poi versions of these things?
					// Or should we add new properties with same names with the same functions?
					
				}
				
				float lightMapMode = _LightingMapMode;
				//UNITY_BRANCH
				if (_LightingDirectionMode == 0)
				{
					poiLight.direction = _WorldSpaceLightPos0.xyz + unity_SHAr.xyz + unity_SHAg.xyz + unity_SHAb.xyz;
				}
				if (_LightingDirectionMode == 1 || _LightingDirectionMode == 2)
				{
					//UNITY_BRANCH
					if (_LightingDirectionMode == 1)
					{
						poiLight.direction = mul(unity_ObjectToWorld, _LightngForcedDirection).xyz;;
					}
					//UNITY_BRANCH
					if (_LightingDirectionMode == 2)
					{
						poiLight.direction = _LightngForcedDirection;
					}
					if (lightMapMode == 0)
					{
						lightMapMode == 1;
					}
				}
				
				if (_LightingDirectionMode == 3) // UTS
				
				{
					float3 defaultLightDirection = normalize(UNITY_MATRIX_V[2].xyz + UNITY_MATRIX_V[1].xyz);
					float3 lightDirection = normalize(lerp(defaultLightDirection, _WorldSpaceLightPos0.xyz, any(_WorldSpaceLightPos0.xyz)));
					poiLight.direction = lightDirection;
				}
				if (_LightingDirectionMode == 4) // OpenLit
				
				{
					poiLight.direction = OpenLitLightingDirection(); // float4 customDir = 0; // Do we want to give users to alter this (OpenLit always does!)?
					
				}
				
				if (_LightingDirectionMode == 5) // View Direction
				
				{
					float3 upViewDir = normalize(UNITY_MATRIX_V[1].xyz);
					float3 rightViewDir = normalize(UNITY_MATRIX_V[0].xyz);
					float yawOffset_Rads = radians(!IsInMirror() ? - _LightingViewDirOffsetYaw : _LightingViewDirOffsetYaw);
					float3 rotatedViewYaw = normalize(RotateAroundAxis(rightViewDir, upViewDir, yawOffset_Rads));
					float3 rotatedViewCameraMeshOffset = RotateAroundAxis((getCameraPosition() - (poiMesh.worldPos)), upViewDir, yawOffset_Rads);
					float pitchOffset_Rads = radians(!IsInMirror() ? _LightingViewDirOffsetPitch : - _LightingViewDirOffsetPitch);
					float3 rotatedViewPitch = RotateAroundAxis(rotatedViewCameraMeshOffset, rotatedViewYaw, pitchOffset_Rads);
					poiLight.direction = normalize(rotatedViewPitch);
				}
				
				if (!any(poiLight.direction))
				{
					poiLight.direction = float3(.4, 1, .4);
				}
				
				poiLight.direction = normalize(poiLight.direction);
				poiLight.attenuationStrength = _LightingCastedShadows;
				poiLight.attenuation = 1;
				if (!all(_LightColor0.rgb == 0.0))
				{
					UNITY_LIGHT_ATTENUATION(attenuation, i, poiMesh.worldPos)
					poiLight.attenuation *= attenuation;
				}
				
				if (!any(poiLight.directColor) && !any(poiLight.indirectColor) && lightMapMode == 0)
				{
					lightMapMode = 1;
					if (_LightingDirectionMode == 0)
					{
						poiLight.direction = normalize(float3(.4, 1, .4));
					}
				}
				
				poiLight.halfDir = normalize(poiLight.direction + poiCam.viewDir);
				poiLight.vertexNDotL = dot(poiMesh.normals[0], poiLight.direction);
				poiLight.nDotL = dot(poiMesh.normals[1], poiLight.direction);
				poiLight.nDotLSaturated = saturate(poiLight.nDotL);
				poiLight.nDotLNormalized = (poiLight.nDotL + 1) * 0.5;
				poiLight.nDotV = abs(dot(poiMesh.normals[1], poiCam.viewDir));
				poiLight.vertexNDotV = abs(dot(poiMesh.normals[0], poiCam.viewDir));
				poiLight.nDotH = dot(poiMesh.normals[1], poiLight.halfDir);
				poiLight.vertexNDotH = max(0.00001, dot(poiMesh.normals[0], poiLight.halfDir));
				poiLight.lDotv = dot(poiLight.direction, poiCam.viewDir);
				poiLight.lDotH = max(0.00001, dot(poiLight.direction, poiLight.halfDir));
				
				// Poi special light map
				if (lightMapMode == 0)
				{
					float3 ShadeSH9Plus = GetSHLength();
					float3 ShadeSH9Minus = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w) + float3(unity_SHBr.z, unity_SHBg.z, unity_SHBb.z) / 3.0;
					
					float3 greyScaleVector = float3(.33333, .33333, .33333);
					float bw_lightColor = dot(poiLight.directColor, greyScaleVector);
					float bw_directLighting = (((poiLight.nDotL * 0.5 + 0.5) * bw_lightColor * lerp(1, poiLight.attenuation, poiLight.attenuationStrength)) + dot(ShadeSH9(float4(poiMesh.normals[1], 1)), greyScaleVector));
					float bw_directLightingNoAtten = (((poiLight.nDotL * 0.5 + 0.5) * bw_lightColor) + dot(ShadeSH9(float4(poiMesh.normals[1], 1)), greyScaleVector));
					float bw_bottomIndirectLighting = dot(ShadeSH9Minus, greyScaleVector);
					float bw_topIndirectLighting = dot(ShadeSH9Plus, greyScaleVector);
					float lightDifference = ((bw_topIndirectLighting + bw_lightColor) - bw_bottomIndirectLighting);
					
					poiLight.lightMap = smoothstep(0, lightDifference, bw_directLighting - bw_bottomIndirectLighting);
					poiLight.lightMapNoAttenuation = smoothstep(0, lightDifference, bw_directLightingNoAtten - bw_bottomIndirectLighting);
				}
				// Normalized nDotL
				if (lightMapMode == 1)
				{
					poiLight.lightMapNoAttenuation = poiLight.nDotLNormalized;
					poiLight.lightMap = poiLight.nDotLNormalized * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				// Saturated nDotL
				if (lightMapMode == 2)
				{
					poiLight.lightMapNoAttenuation = poiLight.nDotLSaturated;
					poiLight.lightMap = poiLight.nDotLSaturated * lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				if (lightMapMode == 3)
				{
					poiLight.lightMapNoAttenuation = 1;
					poiLight.lightMap = lerp(1, poiLight.attenuation, poiLight.attenuationStrength);
				}
				poiLight.lightMapNoAttenuation *= poiLight.detailShadow;
				poiLight.lightMap *= poiLight.detailShadow;
				
				poiLight.directColor = max(poiLight.directColor, 0.0001);
				poiLight.indirectColor = max(poiLight.indirectColor, 0.0001);
				if (_LightingColorMode == 3)
				{
					// OpenLit
					poiLight.directColor = max(poiLight.directColor, _LightingMinLightBrightness);
				}
				else
				{
					poiLight.directColor = max(poiLight.directColor, poiLight.directColor * min(10000, (_LightingMinLightBrightness * rcp(calculateluminance(poiLight.directColor)))));
					poiLight.indirectColor = max(poiLight.indirectColor, poiLight.indirectColor * min(10000, (_LightingMinLightBrightness * rcp(calculateluminance(poiLight.indirectColor)))));
				}
				
				poiLight.directColor = lerp(poiLight.directColor, dot(poiLight.directColor, float3(0.299, 0.587, 0.114)), _LightingMonochromatic);
				poiLight.indirectColor = lerp(poiLight.indirectColor, dot(poiLight.indirectColor, float3(0.299, 0.587, 0.114)), _LightingMonochromatic);
				
				if (_LightingCapEnabled)
				{
					poiLight.directColor = min(poiLight.directColor, _LightingCap);
					poiLight.indirectColor = min(poiLight.indirectColor, _LightingCap);
				}
				
				if (_LightingForceColorEnabled)
				{
					poiLight.directColor = poiThemeColor(poiMods, _LightingForcedColor, _LightingForcedColorThemeIndex);
				}
				
				#endif
				
				#ifdef POI_PASS_ADD
				if (!_LightingAdditiveEnable)
				{
					return float4(mainTexture.rgb * .0001, 1);
				}
				
				#if defined(DIRECTIONAL)
				if (_DisableDirectionalInAdd)
				{
					return float4(mainTexture.rgb * .0001, 1);
				}
				#endif
				
				poiLight.direction = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz * _WorldSpaceLightPos0.w);
				#if defined(POINT) || defined(SPOT)
				#ifdef POINT
				unityShadowCoord3 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(poiMesh.worldPos, 1)).xyz;
				poiLight.attenuation = tex2D(_LightTexture0, dot(lightCoord, lightCoord).rr).r;
				#endif
				
				#ifdef SPOT
				unityShadowCoord4 lightCoord = mul(unity_WorldToLight, unityShadowCoord4(poiMesh.worldPos, 1));
				poiLight.attenuation = (lightCoord.z > 0) * UnitySpotCookie(lightCoord) * UnitySpotAttenuate(lightCoord.xyz);
				#endif
				#else
				UNITY_LIGHT_ATTENUATION(attenuation, i, poiMesh.worldPos)
				poiLight.attenuation = attenuation;
				#endif
				poiLight.additiveShadow = UNITY_SHADOW_ATTENUATION(i, poiMesh.worldPos);
				poiLight.attenuationStrength = _LightingAdditiveCastedShadows;
				poiLight.directColor = _LightingAdditiveLimited ? MaxLuminance(_LightColor0.rgb, _LightingAdditiveLimit) : _LightColor0.rgb;
				
				#if defined(POINT_COOKIE) || defined(DIRECTIONAL_COOKIE)
				poiLight.indirectColor = 0;
				#else
				poiLight.indirectColor = lerp(0, poiLight.directColor, _LightingAdditivePassthrough);
				poiLight.indirectColor = _LightingAdditiveLimited ? MaxLuminance(poiLight.indirectColor, _LightingAdditiveLimit) : poiLight.indirectColor;
				#endif
				
				poiLight.directColor = lerp(poiLight.directColor, dot(poiLight.directColor, float3(0.299, 0.587, 0.114)), _LightingAdditiveMonochromatic);
				poiLight.indirectColor = lerp(poiLight.indirectColor, dot(poiLight.indirectColor, float3(0.299, 0.587, 0.114)), _LightingAdditiveMonochromatic);
				
				poiLight.halfDir = normalize(poiLight.direction + poiCam.viewDir);
				poiLight.nDotL = dot(poiMesh.normals[1], poiLight.direction);
				poiLight.nDotLSaturated = saturate(poiLight.nDotL);
				poiLight.nDotLNormalized = (poiLight.nDotL + 1) * 0.5;
				poiLight.nDotV = abs(dot(poiMesh.normals[1], poiCam.viewDir));
				poiLight.nDotH = dot(poiMesh.normals[1], poiLight.halfDir);
				poiLight.lDotv = dot(poiLight.direction, poiCam.viewDir);
				poiLight.lDotH = dot(poiLight.direction, poiLight.halfDir);
				poiLight.vertexNDotL = dot(poiMesh.normals[0], poiLight.direction);
				poiLight.vertexNDotV = abs(dot(poiMesh.normals[0], poiCam.viewDir));
				poiLight.vertexNDotH = max(0.00001, dot(poiMesh.normals[0], poiLight.halfDir));
				
				// Normalized nDotL
				if (_LightingMapMode == 0 || _LightingMapMode == 1 || _LightingMapMode == 2)
				{
					poiLight.lightMap = poiLight.nDotLNormalized;
				}
				if (_LightingMapMode == 3)
				{
					poiLight.lightMap = 1;
				}
				poiLight.lightMap *= poiLight.detailShadow;
				poiLight.lightMapNoAttenuation = poiLight.lightMap;
				poiLight.lightMap *= lerp(1, poiLight.additiveShadow, poiLight.attenuationStrength);
				#endif
				
				//ifex _LightDataDebugEnabled==0
				if (_LightDataDebugEnabled)
				{
					#ifdef UNITY_PASS_FORWARDBASE
					//UNITY_BRANCH
					if (_LightingDebugVisualize <= 6)
					{
						switch(_LightingDebugVisualize)
						{
							case 0: // Direct Light Color
							return float4(poiLight.directColor + mainTexture.rgb * .0001, 1);
							break;
							case 1: // Indirect Light Color
							return float4(poiLight.indirectColor + mainTexture.rgb * .0001, 1);
							break;
							case 2: // Light Map
							return float4(poiLight.lightMap + mainTexture.rgb * .0001, 1);
							break;
							case 3: // Attenuation
							return float4(poiLight.attenuation + mainTexture.rgb * .0001, 1);
							break;
							case 4: // N Dot L
							return float4(poiLight.nDotLNormalized, poiLight.nDotLNormalized, poiLight.nDotLNormalized, 1) + mainTexture * .0001;
							break;
							case 5:
							return float4(poiLight.halfDir, 1) + mainTexture * .0001;
							break;
							case 6:
							return float4(poiLight.direction, 1) + mainTexture * .0001;
							break;
						}
					}
					else
					{
						return POI_SAFE_RGB1;
					}
					#endif
					#ifdef POI_PASS_ADD
					//UNITY_BRANCH
					if (_LightingDebugVisualize < 6)
					{
						return POI_SAFE_RGB1;
					}
					else
					{
						switch(_LightingDebugVisualize)
						{
							case 7:
							return float4(poiLight.directColor * poiLight.attenuation + mainTexture.rgb * .0001, 1);
							break;
							case 8:
							return float4(poiLight.attenuation + mainTexture.rgb * .0001, 1);
							break;
							case 9:
							return float4(poiLight.additiveShadow + mainTexture.rgb * .0001, 1);
							break;
							case 10:
							return float4(poiLight.nDotLNormalized + mainTexture.rgb * .0001, 1);
							break;
							case 11:
							return float4(poiLight.halfDir, 1) + mainTexture * .0001;
							break;
						}
					}
					#endif
				}
				//endex
				
				poiFragData.baseColor = mainTexture.rgb * poiThemeColor(poiMods, _Color.rgb, _ColorThemeIndex);
				poiFragData.alpha = mainTexture.a * _Color.a;
				
				//ifex _MainColorAdjustToggle==0
				#ifdef COLOR_GRADING_HDR
				#if defined(PROP_MAINCOLORADJUSTTEXTURE) || !defined(OPTIMIZER_ENABLED)
				float4 hueShiftAlpha = POI2D_SAMPLER_PAN(_MainColorAdjustTexture, _MainTex, poiUV(poiMesh.uv[_MainColorAdjustTextureUV], _MainColorAdjustTexture_ST), _MainColorAdjustTexturePan);
				#else
				float4 hueShiftAlpha = 1;
				#endif
				
				if (_MainHueGlobalMask > 0)
				{
					hueShiftAlpha.r = maskBlend(hueShiftAlpha.r, poiMods.globalMask[_MainHueGlobalMask - 1], _MainHueGlobalMaskBlendType);
				}
				if (_MainSaturationGlobalMask > 0)
				{
					hueShiftAlpha.b = maskBlend(hueShiftAlpha.b, poiMods.globalMask[_MainSaturationGlobalMask - 1], _MainSaturationGlobalMaskBlendType);
				}
				if (_MainBrightnessGlobalMask > 0)
				{
					hueShiftAlpha.g = maskBlend(hueShiftAlpha.g, poiMods.globalMask[_MainBrightnessGlobalMask - 1], _MainBrightnessGlobalMaskBlendType);
				}
				
				if (_MainHueShiftToggle)
				{
					float shift = _MainHueShift;
					#ifdef POI_AUDIOLINK
					//UNITY_BRANCH
					if (poiMods.audioLinkAvailable && _MainHueALCTEnabled)
					{
						shift += AudioLinkGetChronoTime(_MainALHueShiftCTIndex, _MainALHueShiftBand) * _MainHueALMotionSpeed;
					}
					#endif
					if (_MainHueShiftReplace)
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, hueShift(poiFragData.baseColor, shift + _MainHueShiftSpeed * _Time.x), hueShiftAlpha.r);
					}
					else
					{
						poiFragData.baseColor = hueShift(poiFragData.baseColor, frac((shift - (1 - hueShiftAlpha.r) + _MainHueShiftSpeed * _Time.x)));
					}
				}
				
				#if defined(PROP_MAINGRADATIONTEX) || !defined(OPTIMIZER_ENABLED)
				
				if (_MainGradationStrength && _ColorGradingToggle)
				{
					#if !defined(UNITY_COLORSPACE_GAMMA)
					float3 tempColor = OpenLitLinearToSRGB(poiFragData.baseColor);
					#endif
					tempColor.r = POI_SAMPLE_1D_X(_MainGradationTex, sampler_linear_clamp, tempColor.r).r;
					tempColor.g = POI_SAMPLE_1D_X(_MainGradationTex, sampler_linear_clamp, tempColor.g).g;
					tempColor.b = POI_SAMPLE_1D_X(_MainGradationTex, sampler_linear_clamp, tempColor.b).b;
					#if !defined(UNITY_COLORSPACE_GAMMA)
					tempColor = OpenLitSRGBToLinear(tempColor);
					#endif
					poiFragData.baseColor = lerp(poiFragData.baseColor, tempColor, _MainGradationStrength);
				}
				#endif
				
				poiFragData.baseColor = lerp(poiFragData.baseColor, dot(poiFragData.baseColor, float3(0.3, 0.59, 0.11)), - (_Saturation) * hueShiftAlpha.b);
				poiFragData.baseColor = saturate(lerp(poiFragData.baseColor, poiFragData.baseColor * (_MainBrightness + 1), hueShiftAlpha.g));
				#endif
				//endex
				
				#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
				
				if (_AlphaMaskMode)
				{
					float alphaMask = POI2D_SAMPLER_PAN(_AlphaMask, _MainTex, poiUV(poiMesh.uv[_AlphaMaskUV], _AlphaMask_ST), _AlphaMaskPan.xy).r;
					alphaMask = saturate(alphaMask * _AlphaMaskScale + _AlphaMaskValue);
					if (_AlphaMaskInvert) alphaMask = 1 - alphaMask;
					if (_AlphaMaskMode == 1) poiFragData.alpha = alphaMask;
					if (_AlphaMaskMode == 2) poiFragData.alpha = poiFragData.alpha * alphaMask;
					if (_AlphaMaskMode == 3) poiFragData.alpha = saturate(poiFragData.alpha + alphaMask);
					if (_AlphaMaskMode == 4) poiFragData.alpha = saturate(poiFragData.alpha - alphaMask);
				}
				#endif
				
				applyAlphaOptions(poiFragData, poiMesh, poiCam, poiMods);
				
				//ifex _GlitterEnable==0
				#ifdef _SUNDISK_SIMPLE
				applyGlitter(poiFragData, poiMesh, poiCam, poiLight, poiMods);
				#endif
				//endex
				
				if (_AlphaPremultiply)
				{
					poiFragData.baseColor *= saturate(poiFragData.alpha);
				}
				poiFragData.finalColor = poiFragData.baseColor;
				
				//UNITY_BRANCH
				if (_IgnoreFog == 0)
				{
					UNITY_APPLY_FOG(i.fogCoord, poiFragData.finalColor);
				}
				
				poiFragData.alpha = _AlphaForceOpaque ? 1 : poiFragData.alpha;
				
				//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
				ApplyAlphaToCoverage(poiFragData, poiMesh);
				//endex
				
				//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
				applyDithering(poiFragData, poiCam);
				//endex
				
				if (_Mode == POI_MODE_OPAQUE)
				{
					poiFragData.alpha = 1;
				}
				
				clip(poiFragData.alpha - _Cutoff);
				
				if (_Mode == POI_MODE_CUTOUT && !_AlphaToCoverage)
				{
					poiFragData.alpha = 1;
				}
				
				if (_AddBlendOp == 4)
				{
					poiFragData.alpha = saturate(poiFragData.alpha * _AlphaBoostFA);
				}
				
				if (_Mode != POI_MODE_TRANSPARENT)
				{
					poiFragData.finalColor *= poiFragData.alpha;
				}
				
				return float4(poiFragData.finalColor, poiFragData.alpha) + POI_SAFE_RGB0;
			}
			
			ENDCG
		}
		
		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			
			ZWrite [_ZWrite]
			Cull [_Cull]
			AlphaToMask Off
			ZTest [_ZTest]
			ColorMask [_ColorMask]
			Offset [_OffsetFactor], [_OffsetUnits]
			
			BlendOp [_BlendOp], [_BlendOpAlpha]
			Blend [_SrcBlend] [_DstBlend], [_SrcBlendAlpha] [_DstBlendAlpha]
			
			CGPROGRAM
			/*
			// Disable warnings we aren't interested in
			#if defined(UNITY_COMPILER_HLSL)
			#pragma warning(disable : 3205) // conversion of larger type to smaller
			#pragma warning(disable : 3568) // unknown pragma ignored
			#pragma warning(disable : 3571) // "pow(f,e) will not work for negative f"; however in majority of our calls to pow we know f is not negative
			#pragma warning(disable : 3206) // implicit truncation of vector type
			#endif
			*/
			#pragma target 5.0
			//ifex 0==0
			#pragma skip_optimizations d3d11
			//endex
			
			#pragma shader_feature_local _STOCHASTICMODE_DELIOT_HEITZ _STOCHASTICMODE_HEXTILE _STOCHASTICMODE_NONE
			
			//ifex _MainColorAdjustToggle==0
			#pragma shader_feature COLOR_GRADING_HDR
			//endex
			
			//#pragma shader_feature KEYWORD
			
			#pragma skip_variants LIGHTMAP_ON DYNAMICLIGHTMAP_ON LIGHTMAP_SHADOW_MIXING SHADOWS_SHADOWMASK DIRLIGHTMAP_COMBINED _MIXED_LIGHTING_SUBTRACTIVE
			#pragma skip_variants DECALS_OFF DECALS_3RT DECALS_4RT DECAL_SURFACE_GRADIENT _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma skip_variants _ADDITIONAL_LIGHT_SHADOWS
			#pragma skip_variants PROBE_VOLUMES_OFF PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
			#pragma skip_variants _SCREEN_SPACE_OCCLUSION
			
			//ifex _PoiParallax==0
			#pragma shader_feature_local POI_PARALLAX
			//endex
			
			//ifex _EnableEmission==0
			#pragma shader_feature _EMISSION
			//endex
			//ifex _EnableEmission1==0
			#pragma shader_feature_local POI_EMISSION_1
			//endex
			//ifex _EnableEmission2==0
			#pragma shader_feature_local POI_EMISSION_2
			//endex
			//ifex _EnableEmission3==0
			#pragma shader_feature_local POI_EMISSION_3
			//endex
			
			//ifex _GlitterEnable==0
			#pragma shader_feature _SUNDISK_SIMPLE
			//endex
			
			//ifex _PoiInternalParallax==0
			#pragma shader_feature_local POI_INTERNALPARALLAX
			//endex
			
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#pragma multi_compile_instancing
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_fog
			#define POI_PASS_SHADOW
			
			// UNITY Includes
			#include "UnityCG.cginc"
			#include "UnityStandardUtils.cginc"
			#include "AutoLight.cginc"
			#include "UnityLightingCommon.cginc"
			#include "UnityPBSLighting.cginc"
			#ifdef POI_PASS_META
			#include "UnityMetaPass.cginc"
			#endif
			#pragma vertex vert
			
			#pragma fragment frag
			
			#define DielectricSpec float4(0.04, 0.04, 0.04, 1.0 - 0.04)
			#define PI float(3.14159265359)
			
			#define POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, samplertex, coord, dx, dy) tex.SampleGrad(sampler##samplertex, coord, dx, dy)
			#define POI2D_SAMPLE_TEX2D_SAMPLERGRADD(tex, samp, uv, pan, dx, dy) tex.SampleGrad(samp, POI_PAN_UV(uv, pan), dx, dy)
			
			#define POI_PAN_UV(uv, pan) (uv + _Time.x * pan)
			#define POI2D_SAMPLER_PAN(tex, texSampler, uv, pan) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, POI_PAN_UV(uv, pan)))
			#define POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, POI_PAN_UV(uv, pan), dx, dy))
			#define POI2D_SAMPLER(tex, texSampler, uv) (UNITY_SAMPLE_TEX2D_SAMPLER(tex, texSampler, uv))
			#define POI_SAMPLE_1D_X(tex, samp, uv) tex.Sample(samp, float2(uv, 0.5))
			#define POI2D_SAMPLER_GRAD(tex, texSampler, uv, dx, dy) (POI2D_SAMPLE_TEX2D_SAMPLERGRAD(tex, texSampler, uv, dx, dy))
			#define POI2D_SAMPLER_GRADD(tex, texSampler, uv, dx, dy) tex.SampleGrad(texSampler, uv, dx, dy)
			#define POI2D_PAN(tex, uv, pan) (tex2D(tex, POI_PAN_UV(uv, pan)))
			#define POI2D(tex, uv) (tex2D(tex, uv))
			#define POI_SAMPLE_TEX2D(tex, uv) (UNITY_SAMPLE_TEX2D(tex, uv))
			#define POI_SAMPLE_TEX2D_PAN(tex, uv, pan) (UNITY_SAMPLE_TEX2D(tex, POI_PAN_UV(uv, pan)))
			#define POI_SAMPLE_CUBE_LOD(tex, samp, uv, lod) texCUBElod(tex, float4(uv, 0, lod))
			
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define TEXTURE2D_SCREEN(tex)                   Texture2DArray tex
			#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, float3(uv, unity_StereoEyeIndex))
			#else
			#define TEXTURE2D_SCREEN(tex)                   Texture2D tex
			#define POI_SAMPLE_SCREEN(tex, samp, uv)          tex.Sample(samp, uv)
			#endif
			
			// When using, properties won't properly lock at optimize time; needs macro evaluation implemented
			// #define POI2D_MAINTEX_SAMPLER_PAN_INLINED(tex, poiMesh) (POI2D_SAMPLER_PAN(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan))
			
			#define POI_SAFE_RGB0 float4(mainTexture.rgb * .0001, 0)
			#define POI_SAFE_RGB1 float4(mainTexture.rgb * .0001, 1)
			#define POI_SAFE_RGBA mainTexture
			
			#if defined(UNITY_COMPILER_HLSL)
			#define PoiInitStruct(type, name) name = (type)0;
			#else
			#define PoiInitStruct(type, name)
			#endif
			
			#define POI_ERROR(poiMesh, gridSize) lerp(float3(1, 0, 1), float3(0, 0, 0), fmod(floor((poiMesh.worldPos.x) * gridSize) + floor((poiMesh.worldPos.y) * gridSize) + floor((poiMesh.worldPos.z) * gridSize), 2) == 0)
			#define POI_NAN (asfloat(-1))
			
			#define POI_MODE_OPAQUE 0
			#define POI_MODE_CUTOUT 1
			#define POI_MODE_FADE 2
			#define POI_MODE_TRANSPARENT 3
			#define POI_MODE_ADDITIVE 4
			#define POI_MODE_SOFTADDITIVE 5
			#define POI_MODE_MULTIPLICATIVE 6
			#define POI_MODE_2XMULTIPLICATIVE 7
			#define POI_MODE_TRANSCLIPPING 9
			
			/*
			Texture2D ;
			float4 _ST;
			float2 Pan;
			float UV;
			float Stochastic;
			
			[HideInInspector][ThryWideEnum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Panosphere, 4, World Pos XZ, 5, Polar UV, 6, Distorted UV, 7 )]
			*/
			
			float _GrabMode;
			float _Mode;
			
			float _StochasticDeliotHeitzDensity;
			float _StochasticHexGridDensity;
			float _StochasticHexRotationStrength;
			float _StochasticHexFallOffContrast;
			float _StochasticHexFallOffPower;
			
			float _IgnoreFog;
			float _RenderingReduceClipDistance;
			int _FlipBackfaceNormals;
			float _AddBlendOp;
			float _Cull;
			
			float4 _Color;
			float _ColorThemeIndex;
			UNITY_DECLARE_TEX2D(_MainTex);
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
			float _MainPixelMode;
			float4 _MainTex_ST;
			float2 _MainTexPan;
			float _MainTexUV;
			float4 _MainTex_TexelSize;
			float _MainTexStochastic;
			#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
			Texture2D _BumpMap;
			#endif
			float4 _BumpMap_ST;
			float2 _BumpMapPan;
			float _BumpMapUV;
			float _BumpScale;
			float _BumpMapStochastic;
			#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _AlphaMask;
			float4 _AlphaMask_ST;
			float2 _AlphaMaskPan;
			float _AlphaMaskUV;
			float _AlphaMaskInvert;
			float _AlphaMaskMode;
			float _AlphaMaskScale;
			float _AlphaMaskValue;
			#endif
			float _Cutoff;
			//ifex _MainColorAdjustToggle==0
			float _MainColorAdjustToggle;
			#if defined(PROP_MAINCOLORADJUSTTEXTURE) || !defined(OPTIMIZER_ENABLED)
			Texture2D _MainColorAdjustTexture;
			#endif
			float4 _MainColorAdjustTexture_ST;
			float2 _MainColorAdjustTexturePan;
			float _MainColorAdjustTextureUV;
			float _MainHueShiftToggle;
			float _MainHueShiftReplace;
			float _MainHueShift;
			float _MainHueShiftSpeed;
			float _Saturation;
			float _MainBrightness;
			
			float _MainHueALCTEnabled;
			float _MainALHueShiftBand;
			float _MainALHueShiftCTIndex;
			float _MainHueALMotionSpeed;
			
			float _MainHueGlobalMask;
			float _MainHueGlobalMaskBlendType;
			float _MainSaturationGlobalMask;
			float _MainSaturationGlobalMaskBlendType;
			float _MainBrightnessGlobalMask;
			float _MainBrightnessGlobalMaskBlendType;
			
			#if defined(PROP_MAINGRADATIONTEX) || !defined(OPTIMIZER_ENABLED)
			Texture2D _MainGradationTex;
			#endif
			float _ColorGradingToggle;
			float _MainGradationStrength;
			//endex
			
			SamplerState sampler_linear_clamp;
			SamplerState sampler_linear_repeat;
			SamplerState sampler_trilinear_repeat;
			
			float _AlphaForceOpaque;
			float _AlphaMod;
			float _AlphaPremultiply;
			float _AlphaBoostFA;
			//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
			float _AlphaToCoverage;
			float _AlphaSharpenedA2C;
			float _AlphaMipScale;
			//endex
			
			//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
			float _AlphaDithering;
			float _AlphaDitherGradient;
			float _AlphaDitherBias;
			//endex
			
			//ifex _AlphaDistanceFade==0 && isNotAnimated(_AlphaDistanceFade)
			float _AlphaDistanceFade;
			float _AlphaDistanceFadeType;
			float _AlphaDistanceFadeMinAlpha;
			float _AlphaDistanceFadeMaxAlpha;
			float _AlphaDistanceFadeMin;
			float _AlphaDistanceFadeMax;
			float _AlphaDistanceFadeGlobalMask;
			float _AlphaDistanceFadeGlobalMaskBlendType;
			//endex
			
			//ifex _AlphaFresnel==0 && isNotAnimated(_AlphaFresnel)
			float _AlphaFresnel;
			float _AlphaFresnelAlpha;
			float _AlphaFresnelSharpness;
			float _AlphaFresnelWidth;
			float _AlphaFresnelInvert;
			float _AlphaFresnelGlobalMask;
			float _AlphaFresnelGlobalMaskBlendType;
			//endex
			
			//ifex _AlphaAngular==0 && isNotAnimated(_AlphaAngular)
			float _AlphaAngular;
			float _AngleType;
			float _AngleCompareTo;
			float3 _AngleForwardDirection;
			float _CameraAngleMin;
			float _CameraAngleMax;
			float _ModelAngleMin;
			float _ModelAngleMax;
			float _AngleMinAlpha;
			float _AlphaAngularGlobalMask;
			float _AlphaAngularGlobalMaskBlendType;
			//endex
			
			//ifex _AlphaAudioLinkEnabled==0 && isNotAnimated(_AlphaAudioLinkEnabled)
			float _AlphaAudioLinkEnabled;
			float2 _AlphaAudioLinkAddRange;
			float _AlphaAudioLinkAddBand;
			//endex
			
			float _AlphaGlobalMask;
			float _AlphaGlobalMaskBlendType;
			
			//ifex _PoiParallax==0
			#ifdef POI_PARALLAX
			
			sampler2D _HeightMap;
			float4 _HeightMap_ST;
			float2 _HeightMapPan;
			float _HeightMapUV;
			
			#if defined(PROP_HEIGHTMASK) || !defined(OPTIMIZER_ENABLED)
			Texture2D _Heightmask;
			float4 _Heightmask_ST;
			float2 _HeightmaskPan;
			float _HeightmaskUV;
			float _HeightmaskChannel;
			float _HeightmaskInvert;
			SamplerState _linear_repeat;
			#endif
			
			float _ParallaxUV;
			float _HeightStrength;
			float _HeightOffset;
			float _HeightStepsMin;
			float _HeightStepsMax;
			
			float _CurvatureU;
			float _CurvatureV;
			float _CurvFix;
			#endif
			//endex
			
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 color : COLOR;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				uint vertexId : SV_VertexID;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct VertexOut
			{
				float4 pos : SV_POSITION;
				float4 uv[2] : TEXCOORD0;
				float3 normal : TEXCOORD2;
				float4 tangent : TEXCOORD3;
				float4 worldPos : TEXCOORD4;
				float4 localPos : TEXCOORD5;
				float4 vertexColor : TEXCOORD6;
				float4 lightmapUV : TEXCOORD7;
				float2 fogCoord: TEXCOORD10;
				UNITY_SHADOW_COORDS(11)
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			struct PoiMesh
			{
				
				// 0 Vertex normal
				// 1 Fragment normal
				float3 normals[2];
				float3 objNormal;
				float3 tangentSpaceNormal;
				float3 binormal[2];
				float3 tangent[2];
				float3 worldPos;
				float3 localPos;
				float3 objectPosition;
				float isFrontFace;
				float4 vertexColor;
				float4 lightmapUV;
				// 0-3 UV0-UV3
				// 4 Panosphere UV
				// 5 world pos xz
				// 6 Polar UV
				// 7 Distorted UV
				float2 uv[9];
				float2 parallaxUV;
				float2 dx;
				float2 dy;
			};
			
			struct PoiCam
			{
				float3 viewDir;
				float3 forwardDir;
				float3 worldPos;
				float distanceToVert;
				float4 clipPos;
				float4 screenSpacePosition;
				float3 reflectionDir;
				float3 vertexReflectionDir;
				float3 tangentViewDir;
				float4 posScreenSpace;
				float2 posScreenPixels;
				float2 screenUV;
				float vDotN;
				float4 worldDirection;
				
			};
			
			struct PoiMods
			{
				float4 Mask;
				float audioLink[5];
				float audioLinkAvailable;
				float audioLinkVersion;
				float4 audioLinkTexture;
				float audioLinkViaLuma;
				float2 detailMask;
				float2 backFaceDetailIntensity;
				float globalEmission;
				float4 globalColorTheme[12];
				float globalMask[16];
				float ALTime[8];
			};
			
			struct PoiLight
			{
				
				float3 direction;
				float attenuation;
				float attenuationStrength;
				float3 directColor;
				float3 indirectColor;
				float occlusion;
				float shadowMask;
				float detailShadow;
				float3 halfDir;
				float lightMap;
				float lightMapNoAttenuation;
				float3 rampedLightMap;
				float vertexNDotL;
				float nDotL;
				float nDotV;
				float vertexNDotV;
				float nDotH;
				float vertexNDotH;
				float lDotv;
				float lDotH;
				float nDotLSaturated;
				float nDotLNormalized;
				#ifdef POI_PASS_ADD
				float additiveShadow;
				#endif
				float3 finalLighting;
				float3 finalLightAdd;
				float3 LTCGISpecular;
				float3 LTCGIDiffuse;
				float directLuminance;
				float indirectLuminance;
				float finalLuminance;
				
				#if defined(VERTEXLIGHT_ON)
				// Non Important Lights
				float4 vDotNL;
				float4 vertexVDotNL;
				float3 vColor[4];
				float4 vCorrectedDotNL;
				float4 vAttenuation;
				float4 vAttenuationDotNL;
				float3 vPosition[4];
				float3 vDirection[4];
				float3 vFinalLighting;
				float3 vHalfDir[4];
				half4 vDotNH;
				half4 vertexVDotNH;
				half4 vDotLH;
				#endif
				
			};
			
			struct PoiVertexLights
			{
				
				float3 direction;
				float3 color;
				float attenuation;
			};
			
			struct PoiFragData
			{
				float smoothness;
				float smoothness2;
				float metallic;
				float specularMask;
				float reflectionMask;
				
				float3 baseColor;
				float3 finalColor;
				float alpha;
				float3 emission;
				float toggleVertexLights;
			};
			
			float4 poiTransformClipSpacetoScreenSpaceFrag(float4 clipPos)
			{
				float4 positionSS = float4(clipPos.xyz * clipPos.w, clipPos.w);
				positionSS.xy = positionSS.xy / _ScreenParams.xy;
				return positionSS;
			}
			
			// glsl_mod behaves better on negative numbers, and
			// in some situations actually outperforms HLSL's fmod()
			#ifndef glsl_mod
			#define glsl_mod(x, y) (((x) - (y) * floor((x) / (y))))
			#endif
			
			uniform float random_uniform_float_only_used_to_stop_compiler_warnings = 0.0f;
			
			float2 poiUV(float2 uv, float4 tex_st)
			{
				return uv * tex_st.xy + tex_st.zw;
			}
			
			float2 vertexUV(in VertexOut o, int index)
			{
				switch(index)
				{
					case 0:
					return o.uv[0].xy;
					case 1:
					return o.uv[0].zw;
					case 2:
					return o.uv[1].xy;
					case 3:
					return o.uv[1].zw;
					default:
					return o.uv[0].xy;
				}
			}
			
			float2 vertexUV(in appdata v, int index)
			{
				switch(index)
				{
					case 0:
					return v.uv0.xy;
					case 1:
					return v.uv1.xy;
					case 2:
					return v.uv2.xy;
					case 3:
					return v.uv3.xy;
					default:
					return v.uv0.xy;
				}
			}
			
			//Lighting Helpers
			float calculateluminance(float3 color)
			{
				return color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
			}
			
			// Set by VRChat (as of open beta 1245)
			// _VRChatCameraMode: 0 => Normal, 1 => VR HandCam, 2 => Desktop Handcam, 3 => Screenshot/Photo
			// _VRChatMirrorMode: 0 => Normal, 1 => Mirror (VR), 2 => Mirror (Deskie)
			float _VRChatCameraMode;
			float _VRChatMirrorMode;
			
			float VRCCameraMode()
			{
				return _VRChatCameraMode;
			}
			
			float VRCMirrorMode()
			{
				return _VRChatMirrorMode;
			}
			
			bool IsInMirror()
			{
				return unity_CameraProjection[2][0] != 0.f || unity_CameraProjection[2][1] != 0.f;
			}
			
			bool IsOrthographicCamera()
			{
				return unity_OrthoParams.w == 1 || UNITY_MATRIX_P[3][3] == 1;
			}
			
			float shEvaluateDiffuseL1Geomerics_local(float L0, float3 L1, float3 n)
			{
				// average energy
				float R0 = max(0, L0);
				
				// avg direction of incoming light
				float3 R1 = 0.5f * L1;
				
				// directional brightness
				float lenR1 = length(R1);
				
				// linear angle between normal and direction 0-1
				//float q = 0.5f * (1.0f + dot(R1 / lenR1, n));
				//float q = dot(R1 / lenR1, n) * 0.5 + 0.5;
				float q = dot(normalize(R1), n) * 0.5 + 0.5;
				q = saturate(q); // Thanks to ScruffyRuffles for the bug identity.
				
				// power for q
				// lerps from 1 (linear) to 3 (cubic) based on directionality
				float p = 1.0f + 2.0f * lenR1 / R0;
				
				// dynamic range constant
				// should vary between 4 (highly directional) and 0 (ambient)
				float a = (1.0f - lenR1 / R0) / (1.0f + lenR1 / R0);
				
				return R0 * (a + (1.0f - a) * (p + 1.0f) * pow(q, p));
			}
			
			half3 BetterSH9(half4 normal)
			{
				float3 indirect;
				float3 L0 = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w) + float3(unity_SHBr.z, unity_SHBg.z, unity_SHBb.z) / 3.0;
				indirect.r = shEvaluateDiffuseL1Geomerics_local(L0.r, unity_SHAr.xyz, normal.xyz);
				indirect.g = shEvaluateDiffuseL1Geomerics_local(L0.g, unity_SHAg.xyz, normal.xyz);
				indirect.b = shEvaluateDiffuseL1Geomerics_local(L0.b, unity_SHAb.xyz, normal.xyz);
				indirect = max(0, indirect);
				indirect += SHEvalLinearL2(normal);
				return indirect;
			}
			
			// Silent's code ends here
			
			float3 getCameraForward()
			{
				#if UNITY_SINGLE_PASS_STEREO
				float3 p1 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 1, 1));
				float3 p2 = mul(unity_StereoCameraToWorld[0], float4(0, 0, 0, 1));
				#else
				float3 p1 = mul(unity_CameraToWorld, float4(0, 0, 1, 1)).xyz;
				float3 p2 = mul(unity_CameraToWorld, float4(0, 0, 0, 1)).xyz;
				#endif
				return normalize(p2 - p1);
			}
			
			half3 GetSHLength()
			{
				half3 x, x1;
				x.r = length(unity_SHAr);
				x.g = length(unity_SHAg);
				x.b = length(unity_SHAb);
				x1.r = length(unity_SHBr);
				x1.g = length(unity_SHBg);
				x1.b = length(unity_SHBb);
				return x + x1;
			}
			
			float3 BoxProjection(float3 direction, float3 position, float4 cubemapPosition, float3 boxMin, float3 boxMax)
			{
				#if UNITY_SPECCUBE_BOX_PROJECTION
				//UNITY_BRANCH
				if (cubemapPosition.w > 0)
				{
					float3 factors = ((direction > 0 ? boxMax : boxMin) - position) / direction;
					float scalar = min(min(factors.x, factors.y), factors.z);
					direction = direction * scalar + (position - cubemapPosition.xyz);
				}
				#endif
				return direction;
			}
			
			float poiMax(float2 i)
			{
				return max(i.x, i.y);
			}
			
			float poiMax(float3 i)
			{
				return max(max(i.x, i.y), i.z);
			}
			
			float poiMax(float4 i)
			{
				return max(max(max(i.x, i.y), i.z), i.w);
			}
			
			float3 calculateNormal(in float3 baseNormal, in PoiMesh poiMesh, in Texture2D normalTexture, in float4 normal_ST, in float2 normalPan, in float normalUV, in float normalIntensity)
			{
				float3 normal = UnpackScaleNormal(POI2D_SAMPLER_PAN(normalTexture, _MainTex, poiUV(poiMesh.uv[normalUV], normal_ST), normalPan), normalIntensity);
				return normalize(
				normal.x * poiMesh.tangent[0] +
				normal.y * poiMesh.binormal[0] +
				normal.z * baseNormal
				);
			}
			
			float remap(float x, float minOld, float maxOld, float minNew = 0, float maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float2 remap(float2 x, float2 minOld, float2 maxOld, float2 minNew = 0, float2 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float3 remap(float3 x, float3 minOld, float3 maxOld, float3 minNew = 0, float3 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float4 remap(float4 x, float4 minOld, float4 maxOld, float4 minNew = 0, float4 maxNew = 1)
			{
				return minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld);
			}
			
			float remapClamped(float minOld, float maxOld, float x, float minNew = 0, float maxNew = 1)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float2 remapClamped(float2 minOld, float2 maxOld, float2 x, float2 minNew, float2 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float3 remapClamped(float3 minOld, float3 maxOld, float3 x, float3 minNew, float3 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			
			float4 remapClamped(float4 minOld, float4 maxOld, float4 x, float4 minNew, float4 maxNew)
			{
				return clamp(minNew + (x - minOld) * (maxNew - minNew) / (maxOld - minOld), minNew, maxNew);
			}
			float2 calcParallax(in float height, in PoiCam poiCam)
			{
				return ((height * - 1) + 1) * (poiCam.tangentViewDir.xy / poiCam.tangentViewDir.z);
			}
			
			/*
			0: Zero	                float4(0.0, 0.0, 0.0, 0.0),
			1: One	                float4(1.0, 1.0, 1.0, 1.0),
			2: DstColor	            destinationColor,
			3: SrcColor	            sourceColor,
			4: OneMinusDstColor	    float4(1.0, 1.0, 1.0, 1.0) - destinationColor,
			5: SrcAlpha	            sourceColor.aaaa,
			6: OneMinusSrcColor	    float4(1.0, 1.0, 1.0, 1.0) - sourceColor,
			7: DstAlpha	            destinationColor.aaaa,
			8: OneMinusDstAlpha	    float4(1.0, 1.0, 1.0, 1.0) - destinationColor.,
			9: SrcAlphaSaturate     saturate(sourceColor.aaaa),
			10: OneMinusSrcAlpha	float4(1.0, 1.0, 1.0, 1.0) - sourceColor.aaaa,
			*/
			
			float4 poiBlend(const float sourceFactor, const  float4 sourceColor, const  float destinationFactor, const  float4 destinationColor, const float4 blendFactor)
			{
				float4 sA = 1 - blendFactor;
				const float4 blendData[11] = {
					float4(0.0, 0.0, 0.0, 0.0),
					float4(1.0, 1.0, 1.0, 1.0),
					destinationColor,
					sourceColor,
					float4(1.0, 1.0, 1.0, 1.0) - destinationColor,
					sA,
					float4(1.0, 1.0, 1.0, 1.0) - sourceColor,
					sA,
					float4(1.0, 1.0, 1.0, 1.0) - sA,
					saturate(sourceColor.aaaa),
					1 - sA,
				};
				
				return lerp(blendData[sourceFactor] * sourceColor + blendData[destinationFactor] * destinationColor, sourceColor, sA);
			}
			
			// Average
			float blendAverage(float base, float blend)
			{
				return (base + blend) / 2.0;
			}
			float3 blendAverage(float3 base, float3 blend)
			{
				return (base + blend) / 2.0;
			}
			
			// Color burn
			float blendColorBurn(float base, float blend)
			{
				return (blend == 0.0) ? blend : max((1.0 - ((1.0 - base) * rcp(random_uniform_float_only_used_to_stop_compiler_warnings + blend))), 0.0);
			}
			
			float3 blendColorBurn(float3 base, float3 blend)
			{
				return float3(blendColorBurn(base.r, blend.r), blendColorBurn(base.g, blend.g), blendColorBurn(base.b, blend.b));
			}
			
			// Color Dodge
			float blendColorDodge(float base, float blend)
			{
				return (blend == 1.0) ? blend : min(base / (1.0 - blend), 1.0);
			}
			
			float3 blendColorDodge(float3 base, float3 blend)
			{
				return float3(blendColorDodge(base.r, blend.r), blendColorDodge(base.g, blend.g), blendColorDodge(base.b, blend.b));
			}
			
			// Darken
			float blendDarken(float base, float blend)
			{
				return min(blend, base);
			}
			
			float3 blendDarken(float3 base, float3 blend)
			{
				return float3(blendDarken(base.r, blend.r), blendDarken(base.g, blend.g), blendDarken(base.b, blend.b));
			}
			
			// Exclusion
			float blendExclusion(float base, float blend)
			{
				return base + blend - 2.0 * base * blend;
			}
			float3 blendExclusion(float3 base, float3 blend)
			{
				return base + blend - 2.0 * base * blend;
			}
			
			// Reflect
			float blendReflect(float base, float blend)
			{
				return (blend == 1.0) ? blend : min(base * base / (1.0 - blend), 1.0);
			}
			
			float3 blendReflect(float3 base, float3 blend)
			{
				return float3(blendReflect(base.r, blend.r), blendReflect(base.g, blend.g), blendReflect(base.b, blend.b));
			}
			
			// Glow
			float blendGlow(float base, float blend)
			{
				return blendReflect(blend, base);
			}
			float3 blendGlow(float3 base, float3 blend)
			{
				return blendReflect(blend, base);
			}
			
			// Overlay
			float blendOverlay(float base, float blend)
			{
				return base < 0.5 ? (2.0 * base * blend) : (1.0 - 2.0 * (1.0 - base) * (1.0 - blend));
			}
			
			float3 blendOverlay(float3 base, float3 blend)
			{
				return float3(blendOverlay(base.r, blend.r), blendOverlay(base.g, blend.g), blendOverlay(base.b, blend.b));
			}
			
			// Hard Light
			float blendHardLight(float base, float blend)
			{
				return blendOverlay(blend, base);
			}
			float3 blendHardLight(float3 base, float3 blend)
			{
				return blendOverlay(blend, base);
			}
			
			// Vivid light
			float blendVividLight(float base, float blend)
			{
				return (blend < 0.5) ? blendColorBurn(base, (2.0 * blend)) : blendColorDodge(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendVividLight(float3 base, float3 blend)
			{
				return float3(blendVividLight(base.r, blend.r), blendVividLight(base.g, blend.g), blendVividLight(base.b, blend.b));
			}
			
			// Hard mix
			float blendHardMix(float base, float blend)
			{
				return (blendVividLight(base, blend) < 0.5) ? 0.0 : 1.0;
			}
			
			float3 blendHardMix(float3 base, float3 blend)
			{
				return float3(blendHardMix(base.r, blend.r), blendHardMix(base.g, blend.g), blendHardMix(base.b, blend.b));
			}
			
			// Lighten
			float blendLighten(float base, float blend)
			{
				return max(blend, base);
			}
			
			float3 blendLighten(float3 base, float3 blend)
			{
				return float3(blendLighten(base.r, blend.r), blendLighten(base.g, blend.g), blendLighten(base.b, blend.b));
			}
			
			// Linear Burn
			float blendLinearBurn(float base, float blend)
			{
				// Note : Same implementation as BlendSubtractf
				return max(base + blend - 1.0, 0.0);
			}
			
			float3 blendLinearBurn(float3 base, float3 blend)
			{
				// Note : Same implementation as BlendSubtract
				return max(base + blend - float3(1.0, 1.0, 1.0), float3(0.0, 0.0, 0.0));
			}
			
			// Linear Dodge
			float blendLinearDodge(float base, float blend)
			{
				// Note : Same implementation as BlendAddf
				return min(base + blend, 1.0);
			}
			
			float3 blendLinearDodge(float3 base, float3 blend)
			{
				// Note : Same implementation as BlendAdd
				return base + blend;
			}
			
			// Linear light
			float blendLinearLight(float base, float blend)
			{
				return blend < 0.5 ? blendLinearBurn(base, (2.0 * blend)) : blendLinearDodge(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendLinearLight(float3 base, float3 blend)
			{
				return float3(blendLinearLight(base.r, blend.r), blendLinearLight(base.g, blend.g), blendLinearLight(base.b, blend.b));
			}
			
			// Multiply
			float blendMultiply(float base, float blend)
			{
				return base * blend;
			}
			float3 blendMultiply(float3 base, float3 blend)
			{
				return base * blend;
			}
			
			// Negation
			float blendNegation(float base, float blend)
			{
				return 1.0 - abs(1.0 - base - blend);
			}
			float3 blendNegation(float3 base, float3 blend)
			{
				return float3(1.0, 1.0, 1.0) - abs(float3(1.0, 1.0, 1.0) - base - blend);
			}
			
			// Normal
			float blendNormal(float base, float blend)
			{
				return blend;
			}
			float3 blendNormal(float3 base, float3 blend)
			{
				return blend;
			}
			
			// Phoenix
			float blendPhoenix(float base, float blend)
			{
				return min(base, blend) - max(base, blend) + 1.0;
			}
			float3 blendPhoenix(float3 base, float3 blend)
			{
				return min(base, blend) - max(base, blend) + float3(1.0, 1.0, 1.0);
			}
			
			// Pin light
			float blendPinLight(float base, float blend)
			{
				return (blend < 0.5) ? blendDarken(base, (2.0 * blend)) : blendLighten(base, (2.0 * (blend - 0.5)));
			}
			
			float3 blendPinLight(float3 base, float3 blend)
			{
				return float3(blendPinLight(base.r, blend.r), blendPinLight(base.g, blend.g), blendPinLight(base.b, blend.b));
			}
			
			// Screen
			float blendScreen(float base, float blend)
			{
				return 1.0 - ((1.0 - base) * (1.0 - blend));
			}
			
			float3 blendScreen(float3 base, float3 blend)
			{
				return float3(blendScreen(base.r, blend.r), blendScreen(base.g, blend.g), blendScreen(base.b, blend.b));
			}
			
			// Soft Light
			float blendSoftLight(float base, float blend)
			{
				return (blend < 0.5) ? (2.0 * base * blend + base * base * (1.0 - 2.0 * blend)) : (sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend));
			}
			
			float3 blendSoftLight(float3 base, float3 blend)
			{
				return float3(blendSoftLight(base.r, blend.r), blendSoftLight(base.g, blend.g), blendSoftLight(base.b, blend.b));
			}
			
			// Subtract
			float blendSubtract(float base, float blend)
			{
				return max(base - blend, 0.0);
			}
			
			float3 blendSubtract(float3 base, float3 blend)
			{
				return max(base - blend, 0.0);
			}
			
			// Difference
			float blendDifference(float base, float blend)
			{
				return abs(base - blend);
			}
			
			float3 blendDifference(float3 base, float3 blend)
			{
				return abs(base - blend);
			}
			
			// Divide
			float blendDivide(float base, float blend)
			{
				return base / max(blend, 0.0001);
			}
			
			float3 blendDivide(float3 base, float3 blend)
			{
				return base / max(blend, 0.0001);
			}
			
			float blendMixed(float base, float blend)
			{
				return base + base * blend;
			}
			
			float3 blendMixed(float3 base, float3 blend)
			{
				return base + base * blend;
			}
			
			float3 customBlend(float3 base, float3 blend, float blendType, float alpha = 1)
			{
				float3 output = base;
				switch(blendType)
				{
					case 0: output = lerp(base, blend, alpha); break;
					case 2: output = base * lerp(1, blend, alpha); break;
					case 6: output = lerp(base, blendScreen(base, blend), alpha); break;
					case 7: output = blendSubtract(base, blend * alpha); break;
					case 8: output = lerp(base, blendLinearDodge(base, blend), alpha); break;
					case 9: output = lerp(base, blendOverlay(base, blend), alpha); break;
					case 20: output = lerp(base, blendMixed(base, blend), alpha); break;
					default: output = 0; break;
				}
				return output;
			}
			
			float3 customBlend(float base, float blend, float blendType, float alpha = 1)
			{
				float3 output = base;
				switch(blendType)
				{
					case 0: output = lerp(base, blend, alpha); break;
					case 2: output = base * lerp(1, blend, alpha); break;
					case 6: output = lerp(base, blendScreen(base, blend), alpha); break;
					case 7: output = blendSubtract(base, blend * alpha); break;
					case 8: output = lerp(base, blendLinearDodge(base, blend), alpha); break;
					case 9: output = lerp(base, blendOverlay(base, blend), alpha); break;
					case 20: output = lerp(base, blendMixed(base, blend), alpha); break;
					default: output = 0; break;
				}
				return output;
			}
			
			#define REPLACE 0
			#define SUBSTRACT 1
			#define MULTIPLY 2
			#define DIVIDE 3
			#define MIN 4
			#define MAX 5
			#define AVERAGE 6
			#define ADD 7
			
			float maskBlend(float baseMask, float blendMask, float blendType)
			{
				float output = 0;
				switch(blendType)
				{
					case REPLACE: output = blendMask; break;
					case SUBSTRACT: output = baseMask - blendMask; break;
					case MULTIPLY: output = baseMask * blendMask; break;
					case DIVIDE: output = baseMask / blendMask; break;
					case MIN: output = min(baseMask, blendMask); break;
					case MAX: output = max(baseMask, blendMask); break;
					case AVERAGE: output = (baseMask + blendMask) * 0.5; break;
					case ADD: output = baseMask + blendMask; break;
				}
				return saturate(output);
			}
			
			float globalMaskBlend(float baseMask, float globalMaskIndex, float blendType, PoiMods poiMods)
			{
				if (globalMaskIndex == 0)
				{
					return baseMask;
				}
				else
				{
					return maskBlend(baseMask, poiMods.globalMask[globalMaskIndex - 1], blendType);
				}
			}
			
			float random(float2 p)
			{
				return frac(sin(dot(p, float2(12.9898, 78.2383))) * 43758.5453123);
			}
			
			float2 random2(float2 p)
			{
				return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
			}
			
			float3 random3(float2 p)
			{
				return frac(sin(float3(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)), dot(p, float2(248.3, 315.9)))) * 43758.5453);
			}
			
			float3 random3(float3 p)
			{
				return frac(sin(float3(dot(p, float3(127.1, 311.7, 248.6)), dot(p, float3(269.5, 183.3, 423.3)), dot(p, float3(248.3, 315.9, 184.2)))) * 43758.5453);
			}
			
			float3 randomFloat3(float2 Seed, float maximum)
			{
				return (.5 + float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed), float2(12.9898, 78.233))) * 43758.5453)
				) * .5) * (maximum);
			}
			
			float3 randomFloat3Range(float2 Seed, float Range)
			{
				return (float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
				) * 2 - 1) * Range;
			}
			
			float3 randomFloat3WiggleRange(float2 Seed, float Range, float wiggleSpeed)
			{
				float3 rando = (float3(
				frac(sin(dot(Seed.xy, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(Seed.yx, float2(12.9898, 78.233))) * 43758.5453),
				frac(sin(dot(float2(Seed.x * Seed.y, Seed.y + Seed.x), float2(12.9898, 78.233))) * 43758.5453)
				) * 2 - 1);
				float speed = 1 + wiggleSpeed;
				return float3(sin((_Time.x + rando.x * PI) * speed), sin((_Time.x + rando.y * PI) * speed), sin((_Time.x + rando.z * PI) * speed)) * Range;
			}
			
			void poiDither(float4 In, float4 ScreenPosition, out float4 Out)
			{
				float2 uv = ScreenPosition.xy * _ScreenParams.xy;
				float DITHER_THRESHOLDS[16] = {
					1.0 / 17.0, 9.0 / 17.0, 3.0 / 17.0, 11.0 / 17.0,
					13.0 / 17.0, 5.0 / 17.0, 15.0 / 17.0, 7.0 / 17.0,
					4.0 / 17.0, 12.0 / 17.0, 2.0 / 17.0, 10.0 / 17.0,
					16.0 / 17.0, 8.0 / 17.0, 14.0 / 17.0, 6.0 / 17.0
				};
				uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
				Out = In - DITHER_THRESHOLDS[index];
			}
			
			static const float Epsilon = 1e-10;
			// The weights of RGB contributions to luminance.
			// Should sum to unity.
			static const float3 HCYwts = float3(0.299, 0.587, 0.114);
			static const float HCLgamma = 3;
			static const float HCLy0 = 100;
			static const float HCLmaxL = 0.530454533953517; // == exp(HCLgamma / HCLy0) - 0.5
			static const float3 wref = float3(1.0, 1.0, 1.0);
			#define TAU 6.28318531
			
			float3 HUEtoRGB(in float H)
			{
				float R = abs(H * 6 - 3) - 1;
				float G = 2 - abs(H * 6 - 2);
				float B = 2 - abs(H * 6 - 4);
				return saturate(float3(R, G, B));
			}
			
			float3 RGBtoHCV(in float3 RGB)
			{
				// Based on work by Sam Hocevar and Emil Persson
				float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
				float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
				float C = Q.x - min(Q.w, Q.y);
				float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
				return float3(H, C, Q.x);
			}
			
			float3 HSVtoRGB(in float3 HSV)
			{
				float3 RGB = HUEtoRGB(HSV.x);
				return ((RGB - 1) * HSV.y + 1) * HSV.z;
			}
			
			float3 RGBtoHSV(in float3 RGB)
			{
				float3 HCV = RGBtoHCV(RGB);
				float S = HCV.y / (HCV.z + Epsilon);
				return float3(HCV.x, S, HCV.z);
			}
			
			float3 HSLtoRGB(in float3 HSL)
			{
				float3 RGB = HUEtoRGB(HSL.x);
				float C = (1 - abs(2 * HSL.z - 1)) * HSL.y;
				return (RGB - 0.5) * C + HSL.z;
			}
			
			float3 RGBtoHSL(in float3 RGB)
			{
				float3 HCV = RGBtoHCV(RGB);
				float L = HCV.z - HCV.y * 0.5;
				float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
				return float3(HCV.x, S, L);
			}
			
			void DecomposeHDRColor(in float3 linearColorHDR, out float3 baseLinearColor, out float exposure)
			{
				// Optimization/adaptation of https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/GUI/ColorMutator.cs#L23 but skips weird photoshop stuff
				float maxColorComponent = max(linearColorHDR.r, max(linearColorHDR.g, linearColorHDR.b));
				bool isSDR = maxColorComponent <= 1.0;
				
				float scaleFactor = isSDR ? 1.0 : (1.0 / maxColorComponent);
				exposure = isSDR ? 0.0 : log(maxColorComponent) * 1.44269504089; // ln(2)
				
				baseLinearColor = scaleFactor * linearColorHDR;
			}
			
			float3 ApplyHDRExposure(float3 linearColor, float exposure)
			{
				return linearColor * pow(2, exposure);
			}
			
			// Transforms an RGB color using a matrix. Note that S and V are absolute values here
			float3 ModifyViaHSV(float3 color, float h, float s, float v)
			{
				float3 colorHSV = RGBtoHSV(color);
				colorHSV.x = frac(colorHSV.x + h);
				colorHSV.y = saturate(colorHSV.y + s);
				colorHSV.z = saturate(colorHSV.z + v);
				return HSVtoRGB(colorHSV);
			}
			
			float3 ModifyViaHSV(float3 color, float3 HSVMod)
			{
				return ModifyViaHSV(color, HSVMod.x, HSVMod.y, HSVMod.z);
			}
			
			float4x4 brightnessMatrix(float brightness)
			{
				return float4x4(
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				brightness, brightness, brightness, 1
				);
			}
			
			float4x4 contrastMatrix(float contrast)
			{
				float t = (1.0 - contrast) / 2.0;
				
				return float4x4(
				contrast, 0, 0, 0,
				0, contrast, 0, 0,
				0, 0, contrast, 0,
				t, t, t, 1
				);
			}
			
			float4x4 saturationMatrix(float saturation)
			{
				float3 luminance = float3(0.3086, 0.6094, 0.0820);
				
				float oneMinusSat = 1.0 - saturation;
				
				float3 red = luminance.x * oneMinusSat;
				red += float3(saturation, 0, 0);
				
				float3 green = luminance.y * oneMinusSat;
				green += float3(0, saturation, 0);
				
				float3 blue = luminance.z * oneMinusSat;
				blue += float3(0, 0, saturation);
				
				return float4x4(
				red, 0,
				green, 0,
				blue, 0,
				0, 0, 0, 1
				);
			}
			
			float4 PoiColorBCS(float4 color, float brightness, float contrast, float saturation)
			{
				return mul(color, mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation))));
			}
			float3 PoiColorBCS(float3 color, float brightness, float contrast, float saturation)
			{
				return mul(float4(color, 1), mul(brightnessMatrix(brightness), mul(contrastMatrix(contrast), saturationMatrix(saturation)))).rgb;
			}
			
			float3 linear_srgb_to_oklab(float3 c)
			{
				float l = 0.4122214708 * c.x + 0.5363325363 * c.y + 0.0514459929 * c.z;
				float m = 0.2119034982 * c.x + 0.6806995451 * c.y + 0.1073969566 * c.z;
				float s = 0.0883024619 * c.x + 0.2817188376 * c.y + 0.6299787005 * c.z;
				
				float l_ = pow(l, 1.0 / 3.0);
				float m_ = pow(m, 1.0 / 3.0);
				float s_ = pow(s, 1.0 / 3.0);
				
				return float3(
				0.2104542553 * l_ + 0.7936177850 * m_ - 0.0040720468 * s_,
				1.9779984951 * l_ - 2.4285922050 * m_ + 0.4505937099 * s_,
				0.0259040371 * l_ + 0.7827717662 * m_ - 0.8086757660 * s_
				);
			}
			
			float3 oklab_to_linear_srgb(float3 c)
			{
				float l_ = c.x + 0.3963377774 * c.y + 0.2158037573 * c.z;
				float m_ = c.x - 0.1055613458 * c.y - 0.0638541728 * c.z;
				float s_ = c.x - 0.0894841775 * c.y - 1.2914855480 * c.z;
				
				float l = l_ * l_ * l_;
				float m = m_ * m_ * m_;
				float s = s_ * s_ * s_;
				
				return float3(
				+ 4.0767416621 * l - 3.3077115913 * m + 0.2309699292 * s,
				- 1.2684380046 * l + 2.6097574011 * m - 0.3413193965 * s,
				- 0.0041960863 * l - 0.7034186147 * m + 1.7076147010 * s
				);
			}
			
			float3 hueShift(float3 color, float shift)
			{
				float3 oklab = linear_srgb_to_oklab(max(color, 0.0000000001));
				float hue = atan2(oklab.z, oklab.y);
				hue += shift * PI * 2;  // Add the hue shift
				
				float chroma = length(oklab.yz);
				oklab.y = cos(hue) * chroma;
				oklab.z = sin(hue) * chroma;
				
				return oklab_to_linear_srgb(oklab);
			}
			
			float3 hueShift(float4 color, float shift)
			{
				return hueShift(color.rgb, shift);
			}
			
			/*
			float3 hueShift(float3 color, float hueOffset)
			{
				color = RGBtoHSV(color);
				color.x = frac(hueOffset +color.x);
				return HSVtoRGB(color);
			}
			*/
			
			// LCH
			float xyzF(float t)
			{
				return lerp(pow(t, 1. / 3.), 7.787037 * t + 0.139731, step(t, 0.00885645));
			}
			float xyzR(float t)
			{
				return lerp(t * t * t, 0.1284185 * (t - 0.139731), step(t, 0.20689655));
			}
			
			float4x4 poiRotationMatrixFromAngles(float x, float y, float z)
			{
				float angleX = radians(x);
				float c = cos(angleX);
				float s = sin(angleX);
				float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
				0, c, -s, 0,
				0, s, c, 0,
				0, 0, 0, 1);
				
				float angleY = radians(y);
				c = cos(angleY);
				s = sin(angleY);
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
				0, 1, 0, 0,
				- s, 0, c, 0,
				0, 0, 0, 1);
				
				float angleZ = radians(z);
				c = cos(angleZ);
				s = sin(angleZ);
				float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
				s, c, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
				
				return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
			}
			
			float4x4 poiRotationMatrixFromAngles(float3 angles)
			{
				float angleX = radians(angles.x);
				float c = cos(angleX);
				float s = sin(angleX);
				float4x4 rotateXMatrix = float4x4(1, 0, 0, 0,
				0, c, -s, 0,
				0, s, c, 0,
				0, 0, 0, 1);
				
				float angleY = radians(angles.y);
				c = cos(angleY);
				s = sin(angleY);
				float4x4 rotateYMatrix = float4x4(c, 0, s, 0,
				0, 1, 0, 0,
				- s, 0, c, 0,
				0, 0, 0, 1);
				
				float angleZ = radians(angles.z);
				c = cos(angleZ);
				s = sin(angleZ);
				float4x4 rotateZMatrix = float4x4(c, -s, 0, 0,
				s, c, 0, 0,
				0, 0, 1, 0,
				0, 0, 0, 1);
				
				return mul(mul(rotateXMatrix, rotateYMatrix), rotateZMatrix);
			}
			
			float3 getCameraPosition()
			{
				#ifdef USING_STEREO_MATRICES
				return lerp(unity_StereoWorldSpaceCameraPos[0], unity_StereoWorldSpaceCameraPos[1], 0.5);
				#endif
				return _WorldSpaceCameraPos;
			}
			
			float2 calcPixelScreenUVs(half4 grabPos)
			{
				half2 uv = grabPos.xy / (grabPos.w + 0.0000000001);
				#if UNITY_SINGLE_PASS_STEREO
				uv.xy *= half2(_ScreenParams.x * 2, _ScreenParams.y);
				#else
				uv.xy *= _ScreenParams.xy;
				#endif
				
				return uv;
			}
			
			float CalcMipLevel(float2 texture_coord)
			{
				float2 dx = ddx(texture_coord);
				float2 dy = ddy(texture_coord);
				float delta_max_sqr = max(dot(dx, dx), dot(dy, dy));
				
				return 0.5 * log2(delta_max_sqr);
			}
			
			float inverseLerp(float A, float B, float T)
			{
				return (T - A) / (B - A);
			}
			
			float inverseLerp2(float2 a, float2 b, float2 value)
			{
				float2 AB = b - a;
				float2 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			float inverseLerp3(float3 a, float3 b, float3 value)
			{
				float3 AB = b - a;
				float3 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			float inverseLerp4(float4 a, float4 b, float4 value)
			{
				float4 AB = b - a;
				float4 AV = value - a;
				return dot(AV, AB) / dot(AB, AB);
			}
			
			/*
			MIT License
			
			Copyright (c) 2019 wraikny
			
			Permission is hereby granted, free of charge, to any person obtaining a copy
			of this software and associated documentation files (the "Software"), to deal
			in the Software without restriction, including without limitation the rights
			to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
			copies of the Software, and to permit persons to whom the Software is
			furnished to do so, subject to the following conditions:
			
			The above copyright notice and this permission notice shall be included in all
			copies or substantial portions of the Software.
			
			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
			IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
			FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
			AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
			LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
			OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
			SOFTWARE.
			
			VertexTransformShader is dependent on:
			*/
			
			float4 quaternion_conjugate(float4 v)
			{
				return float4(
				v.x, -v.yzw
				);
			}
			
			float4 quaternion_mul(float4 v1, float4 v2)
			{
				float4 result1 = (v1.x * v2 + v1 * v2.x);
				
				float4 result2 = float4(
				- dot(v1.yzw, v2.yzw),
				cross(v1.yzw, v2.yzw)
				);
				
				return float4(result1 + result2);
			}
			
			// angle : radians
			float4 get_quaternion_from_angle(float3 axis, float angle)
			{
				float sn = sin(angle * 0.5);
				float cs = cos(angle * 0.5);
				return float4(axis * sn, cs);
			}
			
			float4 quaternion_from_vector(float3 inVec)
			{
				return float4(0.0, inVec);
			}
			
			float degree_to_radius(float degree)
			{
				return (
				degree / 180.0 * PI
				);
			}
			
			float3 rotate_with_quaternion(float3 inVec, float3 rotation)
			{
				float4 qx = get_quaternion_from_angle(float3(1, 0, 0), radians(rotation.x));
				float4 qy = get_quaternion_from_angle(float3(0, 1, 0), radians(rotation.y));
				float4 qz = get_quaternion_from_angle(float3(0, 0, 1), radians(rotation.z));
				
				#define MUL3(A, B, C) quaternion_mul(quaternion_mul((A), (B)), (C))
				float4 quaternion = normalize(MUL3(qx, qy, qz));
				float4 conjugate = quaternion_conjugate(quaternion);
				
				float4 inVecQ = quaternion_from_vector(inVec);
				
				float3 rotated = (
				MUL3(quaternion, inVecQ, conjugate)
				).yzw;
				
				return rotated;
			}
			
			float4 transform(float4 input, float4 pos, float4 rotation, float4 scale)
			{
				input.rgb *= (scale.xyz * scale.w);
				input = float4(rotate_with_quaternion(input.xyz, rotation.xyz * rotation.w) + (pos.xyz * pos.w), input.w);
				return input;
			}
			
			float2 RotateUV(float2 _uv, float _radian, float2 _piv, float _time)
			{
				float RotateUV_ang = _radian;
				float RotateUV_cos = cos(_time * RotateUV_ang);
				float RotateUV_sin = sin(_time * RotateUV_ang);
				return (mul(_uv - _piv, float2x2(RotateUV_cos, -RotateUV_sin, RotateUV_sin, RotateUV_cos)) + _piv);
			}
			
			/*
			MIT END
			*/
			
			float3 RotateAroundAxis(float3 original, float3 axis, float radian)
			{
				float s = sin(radian);
				float c = cos(radian);
				float one_minus_c = 1.0 - c;
				
				axis = normalize(axis);
				float3x3 rot_mat = {
					one_minus_c * axis.x * axis.x + c, one_minus_c * axis.x * axis.y - axis.z * s, one_minus_c * axis.z * axis.x + axis.y * s,
					one_minus_c * axis.x * axis.y + axis.z * s, one_minus_c * axis.y * axis.y + c, one_minus_c * axis.y * axis.z - axis.x * s,
					one_minus_c * axis.z * axis.x - axis.y * s, one_minus_c * axis.y * axis.z + axis.x * s, one_minus_c * axis.z * axis.z + c
				};
				return mul(rot_mat, original);
			}
			
			float3 poiThemeColor(in PoiMods poiMods, in float3 srcColor, in float themeIndex)
			{
				if (themeIndex == 0) return srcColor;
				themeIndex -= 1;
				
				if (themeIndex <= 3)
				{
					return poiMods.globalColorTheme[themeIndex];
				}
				
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable)
				{
					return poiMods.globalColorTheme[themeIndex];
				}
				#endif
				
				return srcColor;
			}
			
			float3 lilToneCorrection(float3 c, float4 hsvg)
			{
				// gamma
				c = pow(abs(c), hsvg.w);
				// rgb -> hsv
				float4 p = (c.b > c.g) ? float4(c.bg, -1.0, 2.0 / 3.0) : float4(c.gb, 0.0, -1.0 / 3.0);
				float4 q = (p.x > c.r) ? float4(p.xyw, c.r) : float4(c.r, p.yzx);
				float d = q.x - min(q.w, q.y);
				float e = 1.0e-10;
				float3 hsv = float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
				// shift
				hsv = float3(hsv.x + hsvg.x, saturate(hsv.y * hsvg.y), saturate(hsv.z * hsvg.z));
				// hsv -> rgb
				return hsv.z - hsv.z * hsv.y + hsv.z * hsv.y * saturate(abs(frac(hsv.x + float3(1.0, 2.0 / 3.0, 1.0 / 3.0)) * 6.0 - 3.0) - 1.0);
			}
			
			float3 lilBlendColor(float3 dstCol, float3 srcCol, float3 srcA, int blendMode)
			{
				float3 ad = dstCol + srcCol;
				float3 mu = dstCol * srcCol;
				float3 outCol;
				if (blendMode == 0) outCol = srcCol;               // Normal
				if (blendMode == 1) outCol = ad;                   // Add
				if (blendMode == 2) outCol = max(ad - mu, dstCol); // Screen
				if (blendMode == 3) outCol = mu;                   // Multiply
				return lerp(dstCol, outCol, srcA);
			}
			
			float lilIsIn0to1(float f)
			{
				float value = 0.5 - abs(f - 0.5);
				return saturate(value / clamp(fwidth(value), 0.0001, 1.0));
			}
			
			float lilIsIn0to1(float f, float nv)
			{
				float value = 0.5 - abs(f - 0.5);
				return saturate(value / clamp(fwidth(value), 0.0001, nv));
			}
			
			float poiEdgeLinearNoSaturate(float value, float border)
			{
				return (value - border) / clamp(fwidth(value), 0.0001, 1.0);
			}
			
			float3 poiEdgeLinearNoSaturate(float value, float3 border)
			{
				return float3(
				(value - border.x) / clamp(fwidth(value), 0.0001, 1.0),
				(value - border.y) / clamp(fwidth(value), 0.0001, 1.0),
				(value - border.z) / clamp(fwidth(value), 0.0001, 1.0)
				);
			}
			
			float poiEdgeLinearNoSaturate(float value, float border, float blur)
			{
				float borderMin = saturate(border - blur * 0.5);
				float borderMax = saturate(border + blur * 0.5);
				return (value - borderMin) / saturate(borderMax - borderMin + fwidth(value));
			}
			
			float poiEdgeLinearNoSaturate(float value, float border, float blur, float borderRange)
			{
				float borderMin = saturate(border - blur * 0.5 - borderRange);
				float borderMax = saturate(border + blur * 0.5);
				return (value - borderMin) / saturate(borderMax - borderMin + fwidth(value));
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border)
			{
				//return (value - border) / clamp(fwidth(value), 0.0001, 1.0);
				
				float fwidthValue = fwidth(value);
				return smoothstep(border - fwidthValue, border + fwidthValue, value);
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border, float blur)
			{
				float fwidthValue = fwidth(value);
				float borderMin = saturate(border - blur * 0.5);
				float borderMax = saturate(border + blur * 0.5);
				return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
			}
			
			float poiEdgeNonLinearNoSaturate(float value, float border, float blur, float borderRange)
			{
				float fwidthValue = fwidth(value);
				float borderMin = saturate(border - blur * 0.5 - borderRange);
				float borderMax = saturate(border + blur * 0.5);
				return smoothstep(borderMin - fwidthValue, borderMax + fwidthValue, value);
			}
			
			float poiEdgeNonLinear(float value, float border)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border));
			}
			
			float poiEdgeNonLinear(float value, float border, float blur)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border, blur));
			}
			
			float poiEdgeNonLinear(float value, float border, float blur, float borderRange)
			{
				return saturate(poiEdgeNonLinearNoSaturate(value, border, blur, borderRange));
			}
			
			float poiEdgeLinear(float value, float border)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border));
			}
			
			float poiEdgeLinear(float value, float border, float blur)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border, blur));
			}
			
			float poiEdgeLinear(float value, float border, float blur, float borderRange)
			{
				return saturate(poiEdgeLinearNoSaturate(value, border, blur, borderRange));
			}
			// From https://github.com/lilxyzw/OpenLit/blob/main/Assets/OpenLit/core.hlsl
			float3 OpenLitLinearToSRGB(float3 col)
			{
				return LinearToGammaSpace(col);
			}
			
			float3 OpenLitSRGBToLinear(float3 col)
			{
				return GammaToLinearSpace(col);
			}
			
			float OpenLitLuminance(float3 rgb)
			{
				#if defined(UNITY_COLORSPACE_GAMMA)
				return dot(rgb, float3(0.22, 0.707, 0.071));
				#else
				return dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
				#endif
			}
			
			float3 AdjustLitLuminance(float3 rgb, float targetLuminance)
			{
				float currentLuminance;
				#if defined(UNITY_COLORSPACE_GAMMA)
				currentLuminance = dot(rgb, float3(0.22, 0.707, 0.071));
				#else
				currentLuminance = dot(rgb, float3(0.0396819152, 0.458021790, 0.00609653955));
				#endif
				
				float luminanceRatio = targetLuminance / currentLuminance;
				return rgb * luminanceRatio;
			}
			
			float3 ClampLuminance(float3 rgb, float minLuminance, float maxLuminance)
			{
				float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
				float minRatio = (currentLuminance != 0) ? minLuminance / currentLuminance : 1.0;
				float maxRatio = (currentLuminance != 0) ? maxLuminance / currentLuminance : 1.0;
				float luminanceRatio = clamp(min(maxRatio, max(minRatio, 1.0)), 0.0, 1.0);
				return lerp(rgb, rgb * luminanceRatio, luminanceRatio < 1.0);
			}
			
			float3 MaxLuminance(float3 rgb, float maxLuminance)
			{
				float currentLuminance = dot(rgb, float3(0.299, 0.587, 0.114));
				float luminanceRatio = (currentLuminance != 0) ? maxLuminance / currentLuminance : 1.0;
				return lerp(rgb, rgb * luminanceRatio, currentLuminance > maxLuminance);
			}
			
			float OpenLitGray(float3 rgb)
			{
				return dot(rgb, float3(1.0 / 3.0, 1.0 / 3.0, 1.0 / 3.0));
			}
			
			void OpenLitShadeSH9ToonDouble(float3 lightDirection, out float3 shMax, out float3 shMin)
			{
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 N = lightDirection * 0.666666;
				float4 vB = N.xyzz * N.yzzx;
				// L0 L2
				float3 res = float3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
				res.r += dot(unity_SHBr, vB);
				res.g += dot(unity_SHBg, vB);
				res.b += dot(unity_SHBb, vB);
				res += unity_SHC.rgb * (N.x * N.x - N.y * N.y);
				// L1
				float3 l1;
				l1.r = dot(unity_SHAr.rgb, N);
				l1.g = dot(unity_SHAg.rgb, N);
				l1.b = dot(unity_SHAb.rgb, N);
				shMax = res + l1;
				shMin = res - l1;
				#if defined(UNITY_COLORSPACE_GAMMA)
				shMax = OpenLitLinearToSRGB(shMax);
				shMin = OpenLitLinearToSRGB(shMin);
				#endif
				#else
				shMax = 0.0;
				shMin = 0.0;
				#endif
			}
			
			float3 OpenLitComputeCustomLightDirection(float4 lightDirectionOverride)
			{
				float3 customDir = length(lightDirectionOverride.xyz) * normalize(mul((float3x3)unity_ObjectToWorld, lightDirectionOverride.xyz));
				return lightDirectionOverride.w ? customDir : lightDirectionOverride.xyz; // .w isn't doc'd anywhere and is always 0 unless end user changes it
				
			}
			
			float3 OpenLitLightingDirectionForSH9()
			{
				float3 mainDir = _WorldSpaceLightPos0.xyz * OpenLitLuminance(_LightColor0.rgb);
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 sh9Dir = unity_SHAr.xyz * 0.333333 + unity_SHAg.xyz * 0.333333 + unity_SHAb.xyz * 0.333333;
				float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
				#else
				float3 sh9Dir = 0;
				float3 sh9DirAbs = 0;
				#endif
				
				float3 lightDirectionForSH9 = sh9Dir + mainDir;
				lightDirectionForSH9 = dot(lightDirectionForSH9, lightDirectionForSH9) < 0.000001 ? 0 : normalize(lightDirectionForSH9);
				return lightDirectionForSH9;
			}
			
			float3 OpenLitLightingDirection(float4 lightDirectionOverride)
			{
				float3 mainDir = _WorldSpaceLightPos0.xyz * OpenLitLuminance(_LightColor0.rgb);
				#if !defined(LIGHTMAP_ON) && UNITY_SHOULD_SAMPLE_SH
				float3 sh9Dir = unity_SHAr.xyz * 0.333333 + unity_SHAg.xyz * 0.333333 + unity_SHAb.xyz * 0.333333;
				float3 sh9DirAbs = float3(sh9Dir.x, abs(sh9Dir.y), sh9Dir.z);
				#else
				float3 sh9Dir = 0;
				float3 sh9DirAbs = 0;
				#endif
				float3 customDir = OpenLitComputeCustomLightDirection(lightDirectionOverride);
				
				return normalize(sh9DirAbs + mainDir + customDir);
			}
			
			float3 OpenLitLightingDirection()
			{
				float4 customDir = float4(0.001, 0.002, 0.001, 0.0);
				return OpenLitLightingDirection(customDir);
			}
			
			inline float4 CalculateFrustumCorrection()
			{
				float x1 = -UNITY_MATRIX_P._31 / (UNITY_MATRIX_P._11 * UNITY_MATRIX_P._34);
				float x2 = -UNITY_MATRIX_P._32 / (UNITY_MATRIX_P._22 * UNITY_MATRIX_P._34);
				return float4(x1, x2, 0, UNITY_MATRIX_P._33 / UNITY_MATRIX_P._34 + x1 * UNITY_MATRIX_P._13 + x2 * UNITY_MATRIX_P._23);
			}
			
			inline float CorrectedLinearEyeDepth(float z, float B)
			{
				return 1.0 / (z / UNITY_MATRIX_P._34 + B);
			}
			
			//Silent's code
			float2 sharpSample(float4 texelSize, float2 p)
			{
				p = p * texelSize.zw;
				float2 c = max(0.0, fwidth(p));
				p = floor(p) + saturate(frac(p) / c);
				p = (p - 0.5) * texelSize.xy;
				return p;
			}
			
			void applyToGlobalMask(inout PoiMods poiMods, int index, int blendType, float val)
			{
				float valBlended = saturate(maskBlend(poiMods.globalMask[index], val, blendType));
				switch(index)
				{
					case 0: poiMods.globalMask[0] = valBlended; break;
					case 1: poiMods.globalMask[1] = valBlended; break;
					case 2: poiMods.globalMask[2] = valBlended; break;
					case 3: poiMods.globalMask[3] = valBlended; break;
					case 4: poiMods.globalMask[4] = valBlended; break;
					case 5: poiMods.globalMask[5] = valBlended; break;
					case 6: poiMods.globalMask[6] = valBlended; break;
					case 7: poiMods.globalMask[7] = valBlended; break;
					case 8: poiMods.globalMask[8] = valBlended; break;
					case 9: poiMods.globalMask[9] = valBlended; break;
					case 10: poiMods.globalMask[10] = valBlended; break;
					case 11: poiMods.globalMask[11] = valBlended; break;
					case 12: poiMods.globalMask[12] = valBlended; break;
					case 13: poiMods.globalMask[13] = valBlended; break;
					case 14: poiMods.globalMask[14] = valBlended; break;
					case 15: poiMods.globalMask[15] = valBlended; break;
				}
			}
			
			void assignValueToVectorFromIndex(inout float4 vec, int index, float value)
			{
				switch(index)
				{
					case 0: vec[0] = value; break;
					case 1: vec[1] = value; break;
					case 2: vec[2] = value; break;
					case 3: vec[3] = value; break;
				}
			}
			
			// SNose
			float3 mod289(float3 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			float2 mod289(float2 x)
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			float3 permute(float3 x)
			{
				return mod289(((x * 34.0) + 1.0) * x);
			}
			
			float snoise(float2 v)
			{
				const float4 C = float4(0.211324865405187, // (3.0-sqrt(3.0))/6.0
				0.366025403784439, // 0.5*(sqrt(3.0)-1.0)
				- 0.577350269189626, // -1.0 + 2.0 * C.x
				0.024390243902439); // 1.0 / 41.0
				float2 i = floor(v + dot(v, C.yy));
				float2 x0 = v - i + dot(i, C.xx);
				float2 i1;
				i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod289(i); // Avoid truncation effects in permutation
				float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0))
				+ i.x + float3(0.0, i1.x, 1.0));
				
				float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
				m = m * m ;
				m = m * m ;
				float3 x = 2.0 * frac(p * C.www) - 1.0;
				float3 h = abs(x) - 0.5;
				float3 ox = floor(x + 0.5);
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * (a0 * a0 + h * h);
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot(m, g);
			}
			
			float nsqDistance(float2 a, float2 b)
			{
				return dot(a - b, a - b);
			}
			
			float poiInvertToggle(in float value, in float toggle)
			{
				return (toggle == 0 ? value : 1 - value);
			}
			
			// OKLAB
			
			float3 PoiBlendNormal(float3 dstNormal, float3 srcNormal)
			{
				return float3(dstNormal.xy + srcNormal.xy, dstNormal.z * srcNormal.z);
			}
			
			float3 lilTransformDirOStoWS(float3 directionOS, bool doNormalize)
			{
				if (doNormalize) return normalize(mul((float3x3)unity_ObjectToWorld, directionOS));
				else            return mul((float3x3)unity_ObjectToWorld, directionOS);
			}
			
			float2 poiGetWidthAndHeight(Texture2D tex)
			{
				uint width, height;
				tex.GetDimensions(width, height);
				return float2(width, height);
			}
			
			float2 poiGetWidthAndHeight(Texture2DArray tex)
			{
				uint width, height, element;
				tex.GetDimensions(width, height, element);
				return float2(width, height);
			}
			
			VertexOut vert(
			#ifndef POI_TESSELLATED
			appdata v
			#else
			tessAppData v
			#endif
			)
			{
				UNITY_SETUP_INSTANCE_ID(v);
				VertexOut o;
				PoiInitStruct(VertexOut, o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				#ifdef POI_TESSELLATED
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(v);
				#endif
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.tangent.xyz = UnityObjectToWorldDir(v.tangent);
				o.tangent.w = v.tangent.w;
				o.vertexColor = v.color;
				
				o.uv[0] = float4(v.uv0.xy, v.uv1.xy);
				o.uv[1] = float4(v.uv2.xy, v.uv3.xy);
				
				#if defined(LIGHTMAP_ON)
				o.lightmapUV.xy = v.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				#ifdef DYNAMICLIGHTMAP_ON
				o.lightmapUV.zw = v.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif
				
				o.localPos = v.vertex;
				o.worldPos = mul(unity_ObjectToWorld, o.localPos);
				
				float3 localOffset = float3(0, 0, 0);
				float3 worldOffset = float3(0, 0, 0);
				
				o.localPos.rgb += localOffset;
				o.worldPos.rgb += worldOffset;
				
				o.pos = UnityObjectToClipPos(o.localPos);
				
				#ifdef POI_PASS_OUTLINE
				#if defined(UNITY_REVERSED_Z)
				//DX
				o.pos.z += _Offset_Z * - 0.01;
				#else
				//OpenGL
				o.pos.z += _Offset_Z * 0.01;
				#endif
				#endif
				//o.grabPos = ComputeGrabScreenPos(o.pos);
				
				#ifndef FORWARD_META_PASS
				#if !defined(UNITY_PASS_SHADOWCASTER)
				UNITY_TRANSFER_SHADOW(o, o.uv[0].xy);
				#else
				v.vertex.xyz = o.localPos.xyz;
				TRANSFER_SHADOW_CASTER_NOPOS(o, o.pos);
				#endif
				#endif
				
				UNITY_TRANSFER_FOG(o, o.pos);
				
				if (_RenderingReduceClipDistance)
				{
					if (o.pos.w < _ProjectionParams.y * 1.01 && o.pos.w > 0)
					{
						#if defined(UNITY_REVERSED_Z) // DirectX
						o.pos.z = o.pos.z * 0.0001 + o.pos.w * 0.999;
						#else // OpenGL
						o.pos.z = o.pos.z * 0.0001 - o.pos.w * 0.999;
						#endif
					}
				}
				
				#ifdef POI_PASS_META
				o.pos = UnityMetaVertexPosition(v.vertex, v.uv1.xy, v.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);
				#endif
				
				return o;
			}
			
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, uv) : POI2D_SAMPLER(tex, texSampler, uv))
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan)) : POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (useStochastic ? DeliotHeitzSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), dx, dy) : POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			#if defined(_STOCHASTICMODE_HEXTILE)
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (useStochastic ? HextileSampleTexture(tex, sampler##texSampler, uv, false) : POI2D_SAMPLER(tex, texSampler, uv))
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (useStochastic ? HextileSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), false) : POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (useStochastic ? HextileSampleTexture(tex, sampler##texSampler, POI_PAN_UV(uv, pan), false, dx, dy) : POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			
			#ifndef POI2D_SAMPLER_STOCHASTIC
			#define POI2D_SAMPLER_STOCHASTIC(tex, texSampler, uv, useStochastic) (POI2D_SAMPLER(tex, texSampler, uv))
			#endif
			#ifndef POI2D_SAMPLER_PAN_STOCHASTIC
			#define POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, uv, pan, useStochastic) (POI2D_SAMPLER_PAN(tex, texSampler, uv, pan))
			#endif
			#ifndef POI2D_SAMPLER_PANGRAD_STOCHASTIC
			#define POI2D_SAMPLER_PANGRAD_STOCHASTIC(tex, texSampler, uv, pan, dx, dy, useStochastic) (POI2D_SAMPLER_PANGRAD(tex, texSampler, uv, pan, dx, dy))
			#endif
			
			// When using, properties won't properly lock at optimize time; needs macro evaluation implemented
			// #define POI2D_SAMPLER_STOCHASTIC_INLINED(tex, texSampler) (POI2D_SAMPLER_STOCHASTIC(tex, texSampler, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Stochastic))
			// #define POI2D_SAMPLER_PAN_STOCHASTIC_INLINED(tex, texSampler) (POI2D_SAMPLER_PAN_STOCHASTIC(tex, texSampler, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan, tex##Stochastic))
			
			// #define POI2D_MAINTEX_SAMPLER_STOCHASTIC_INLINED(tex) (POI2D_SAMPLER_STOCHASTIC(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Stochastic))
			// #define POI2D_MAINTEX_SAMPLER_PAN_STOCHASTIC_INLINED(tex) (POI2D_SAMPLER_PAN_STOCHASTIC(tex, _MainTex, poiUV(poiMesh.uv[tex##UV], tex##_ST), tex##Pan, tex##Stochastic))
			
			// Deliot, Heitz 2019 - Fast, but non-histogram-preserving (ends up looking a bit blurry and lower contrast)
			// https://eheitzresearch.wordpress.com/738-2/
			
			// Classic Magic Numbers fracsin
			#if !defined(_STOCHASTICMODE_NONE)
			float2 StochasticHash2D2D (float2 s)
			{
				return frac(sin(glsl_mod(float2(dot(s, float2(127.1,311.7)), dot(s, float2(269.5,183.3))), 3.14159)) * 43758.5453);
			}
			#endif
			
			#if defined(_STOCHASTICMODE_DELIOT_HEITZ)
			// UV Offsets and blend weights
			// UVBW[0...2].xy = UV Offsets
			// UVBW[0...2].z = Blend Weights
			float3x3 DeliotHeitzStochasticUVBW(float2 uv)
			{
				// UV transformed into triangular grid space with UV scaled by approximation of 2*sqrt(3)
				const float2x2 stochasticSkewedGrid = float2x2(1.0, -0.57735027, 0.0, 1.15470054);
				float2 skewUV = mul(stochasticSkewedGrid, uv * 3.4641 * _StochasticDeliotHeitzDensity);
				
				// Vertex IDs and barycentric coords
				float2 vxID = floor(skewUV);
				float3 bary = float3(frac(skewUV), 0);
				bary.z = 1.0 - bary.x - bary.y;
				
				float3x3 pos = float3x3(
				float3(vxID, 				bary.z),
				float3(vxID + float2(0, 1), bary.y),
				float3(vxID + float2(1, 0), bary.x)
				);
				
				float3x3 neg = float3x3(
				float3(vxID + float2(1, 1), 	 -bary.z),
				float3(vxID + float2(1, 0), 1.0 - bary.y),
				float3(vxID + float2(0, 1), 1.0 - bary.x)
				);
				
				return (bary.z > 0) ? pos : neg;
			}
			
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, float2 dx, float2 dy)
			{
				// UVBW[0...2].xy = UV Offsets
				// UVBW[0...2].z = Blend Weights
				float3x3 UVBW = DeliotHeitzStochasticUVBW(uv);
				
				//blend samples with calculated weights
				return 	mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[0].xy), dx, dy), UVBW[0].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[1].xy), dx, dy), UVBW[1].z) +
				mul(tex.SampleGrad(texSampler, uv + StochasticHash2D2D(UVBW[2].xy), dx, dy), UVBW[2].z) ;
			}
			
			float4 DeliotHeitzSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv)
			{
				float2 dx = ddx(uv), dy = ddy(uv);
				return DeliotHeitzSampleTexture(tex, texSampler, uv, dx, dy);
			}
			#endif // defined(_STOCHASTICMODE_DELIOT_HEITZ)
			
			#if defined(_STOCHASTICMODE_HEXTILE)
			// HexTiling: Slower, but histogram-preserving
			// SPDX-License-Idenfitier: MIT
			// Copyright (c) 2022 mmikk
			// https://github.com/mmikk/hextile-demo
			float2 HextileMakeCenUV(float2 vertex)
			{
				// 0.288675 ~= 1/(2*sqrt(3))
				const float2x2 stochasticInverseSkewedGrid = float2x2(1.0, 0.5, 0.0, 1.0/1.15470054);
				return mul(stochasticInverseSkewedGrid, vertex) * 0.288675;
			}
			
			float2x2 HextileLoadRot2x2(float2 idx, float rotStrength)
			{
				float angle = abs(idx.x * idx.y) + abs(idx.x + idx.y) + PI;
				
				// remap to +/-pi
				angle = glsl_mod(angle, 2 * PI);
				if(angle < 0)  angle += 2 * PI;
				if(angle > PI) angle -= 2 * PI;
				
				angle *= rotStrength;
				
				float cs = cos(angle), si = sin(angle);
				return float2x2(cs, -si, si, cs);
			}
			
			// UV Offsets and base blend weights
			// UVBWR[0...2].xy = UV Offsets
			// UVBWR[0...2].zw = rotation costh/sinth -> reconstruct rotation matrix with float2x2(UVBWR[n].z, -UVBWR[n].w, UVBWR[n].w, UVBWR[n].z)
			// UVBWR[3].xyz = Blend Weights (w unused) - needs luminance weighting
			float4x4 HextileUVBWR(float2 uv)
			{
				// Create Triangle Grid
				// Skew input space into simplex triangle grid (3.4641 ~= 2*sqrt(3))
				const float2x2 stochasticSkewedGrid = float2x2(1.0, -0.57735027, 0.0, 1.15470054);
				float2 skewedCoord = mul(stochasticSkewedGrid, uv * 3.4641 * _StochasticHexGridDensity);
				
				float2 baseId = float2(floor(skewedCoord));
				float3 temp = float3(frac(skewedCoord), 0);
				temp.z = 1 - temp.x - temp.y;
				
				float s = step(0.0, -temp.z);
				float s2 = 2 * s - 1;
				
				float3 weights = float3(-temp.z * s2, s - temp.y * s2, s - temp.x * s2);
				
				float2 vertex0 = baseId + float2(s, s);
				float2 vertex1 = baseId + float2(s, 1 - s);
				float2 vertex2 = baseId + float2(1 - s, s);
				
				float2 cen0 = HextileMakeCenUV(vertex0), cen1 = HextileMakeCenUV(vertex1), cen2 = HextileMakeCenUV(vertex2);
				float2x2 rot0 = float2x2(1, 0, 0, 1), rot1 = float2x2(1, 0, 0, 1), rot2 = float2x2(1, 0, 0, 1);
				
				if(_StochasticHexRotationStrength > 0)
				{
					rot0 = HextileLoadRot2x2(vertex0, _StochasticHexRotationStrength);
					rot1 = HextileLoadRot2x2(vertex1, _StochasticHexRotationStrength);
					rot2 = HextileLoadRot2x2(vertex2, _StochasticHexRotationStrength);
				}
				
				return float4x4(
				float4(mul(uv - cen0, rot0) + cen0 + StochasticHash2D2D(vertex0), rot0[0].x, -rot0[0].y),
				float4(mul(uv - cen1, rot1) + cen1 + StochasticHash2D2D(vertex1), rot1[0].x, -rot1[0].y),
				float4(mul(uv - cen2, rot2) + cen2 + StochasticHash2D2D(vertex2), rot2[0].x, -rot2[0].y),
				float4(weights, 0)
				);
			}
			
			float4 HextileSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, bool isNormalMap, float2 dUVdx, float2 dUVdy)
			{
				// For some reason doing this instead of just calculating it directly prevents it from \
				// breaking after a certain number of textures use it. I don't understand why yet
				float4x4 UVBWR = HextileUVBWR(uv);
				
				// 2D Rotation Matrices for dUVdx/dy
				// Not sure if this constant folds during compiling when rot is locked at 0, so force it
				float2x2 rot0 = float2x2(1, 0, 0, 1), rot1 = float2x2(1, 0, 0, 1), rot2 = float2x2(1, 0, 0, 1);
				
				if(_StochasticHexRotationStrength > 0)
				{
					rot0 = float2x2(UVBWR[0].z, -UVBWR[0].w, UVBWR[0].w, UVBWR[0].z);
					rot1 = float2x2(UVBWR[1].z, -UVBWR[1].w, UVBWR[1].w, UVBWR[1].z);
					rot2 = float2x2(UVBWR[2].z, -UVBWR[2].w, UVBWR[2].w, UVBWR[2].z);
				}
				
				// Weights
				float3 W = UVBWR[3].xyz;
				
				// Sample texture
				// float3x4 c = float3x4(
				// 	tex.SampleGrad(texSampler, UVBWR[0].xy, mul(dUVdx, rot0), mul(dUVdy, rot0)),
				// 	tex.SampleGrad(texSampler, UVBWR[1].xy, mul(dUVdx, rot1), mul(dUVdy, rot1)),
				// 	tex.SampleGrad(texSampler, UVBWR[2].xy, mul(dUVdx, rot2), mul(dUVdy, rot2))
				// );
				
				float4 c0 = tex.SampleGrad(texSampler, UVBWR[0].xy, mul(dUVdx, rot0), mul(dUVdy, rot0));
				float4 c1 = tex.SampleGrad(texSampler, UVBWR[1].xy, mul(dUVdx, rot1), mul(dUVdy, rot1));
				float4 c2 = tex.SampleGrad(texSampler, UVBWR[2].xy, mul(dUVdx, rot2), mul(dUVdy, rot2));
				
				// Blend samples using luminance
				// This is technically incorrect for normal maps, but produces very similar
				// results to blending using normal map gradients (steepness)
				const float3 Lw = float3(0.299, 0.587, 0.114);
				float3 Dw = float3(dot(c0.xyz, Lw), dot(c1.xyz, Lw), dot(c2.xyz, Lw));
				
				Dw = lerp(1.0, Dw, _StochasticHexFallOffContrast);
				W = Dw * pow(W, _StochasticHexFallOffPower);
				// In the original hextiling there's a Gain3 step here, but it seems to slow things down \
				// and cause the UVs to break, so I've omitted it. Looks fine without
				
				W /= (W.x + W.y + W.z);
				return W.x * c0 + W.y * c1 + W.z * c2;
			}
			
			float4 HextileSampleTexture(Texture2D tex, SamplerState texSampler, float2 uv, bool isNormalMap)
			{
				return HextileSampleTexture(tex, texSampler, uv, isNormalMap, ddx(uv), ddy(uv));
			}
			#endif // defined(_STOCHASTICMODE_HEXTILE)
			
			void applyAlphaOptions(inout PoiFragData poiFragData, in PoiMesh poiMesh, in PoiCam poiCam, in PoiMods poiMods)
			{
				poiFragData.alpha = saturate(poiFragData.alpha + _AlphaMod);
				
				if (_AlphaGlobalMask > 0)
				{
					poiFragData.alpha = maskBlend(poiFragData.alpha, poiMods.globalMask[_AlphaGlobalMask - 1], _AlphaGlobalMaskBlendType);
				}
				
				//ifex _AlphaDistanceFade==0 && isNotAnimated(_AlphaDistanceFade)
				if (_AlphaDistanceFade)
				{
					float3 position = _AlphaDistanceFadeType ? poiMesh.worldPos : poiMesh.objectPosition;
					float distanceFadeMultiplier = lerp(_AlphaDistanceFadeMinAlpha, _AlphaDistanceFadeMaxAlpha, smoothstep(_AlphaDistanceFadeMin, _AlphaDistanceFadeMax, distance(position, poiCam.worldPos)));
					if (_AlphaDistanceFadeGlobalMask > 0)
					{
						distanceFadeMultiplier = lerp(1, distanceFadeMultiplier, poiMods.globalMask[_AlphaDistanceFadeGlobalMask - 1]);
					}
					poiFragData.alpha *= distanceFadeMultiplier;
				}
				//endex
				
				//ifex _AlphaFresnel==0 && isNotAnimated(_AlphaFresnel)
				if (_AlphaFresnel)
				{
					float holoRim = saturate(1 - smoothstep(min(_AlphaFresnelSharpness, _AlphaFresnelWidth), _AlphaFresnelWidth, (poiCam.vDotN)));
					holoRim = abs(lerp(1, holoRim, _AlphaFresnelAlpha));
					holoRim = _AlphaFresnelInvert ? 1 - holoRim : holoRim;
					if (_AlphaFresnelGlobalMask > 0)
					{
						holoRim = lerp(1, holoRim, poiMods.globalMask[_AlphaFresnelGlobalMask - 1]);
					}
					poiFragData.alpha *= holoRim;
				}
				//endex
				
				//ifex _AlphaAngular==0 && isNotAnimated(_AlphaAngular)
				if (_AlphaAngular)
				{
					half cameraAngleMin = _CameraAngleMin / 180;
					half cameraAngleMax = _CameraAngleMax / 180;
					half modelAngleMin = _ModelAngleMin / 180;
					half modelAngleMax = _ModelAngleMax / 180;
					float3 pos = _AngleCompareTo == 0 ? poiMesh.objectPosition : poiMesh.worldPos;
					half3 cameraToModelDirection = normalize(pos - getCameraPosition());
					half3 modelForwardDirection = normalize(mul(unity_ObjectToWorld, normalize(_AngleForwardDirection.rgb)));
					half cameraLookAtModel = remapClamped(cameraAngleMax, cameraAngleMin, .5 * dot(cameraToModelDirection, getCameraForward()) + .5);
					half modelLookAtCamera = remapClamped(modelAngleMax, modelAngleMin, .5 * dot(-cameraToModelDirection, modelForwardDirection) + .5);
					float angularAlphaMod = 1;
					if (_AngleType == 0)
					{
						angularAlphaMod = max(cameraLookAtModel, _AngleMinAlpha);
					}
					else if (_AngleType == 1)
					{
						angularAlphaMod = max(modelLookAtCamera, _AngleMinAlpha);
					}
					else if (_AngleType == 2)
					{
						angularAlphaMod = max(cameraLookAtModel * modelLookAtCamera, _AngleMinAlpha);
					}
					if (_AlphaAngularGlobalMask > 0)
					{
						angularAlphaMod = lerp(1, angularAlphaMod, poiMods.globalMask[_AlphaAngularGlobalMask - 1]);
					}
					poiFragData.alpha *= angularAlphaMod;
				}
				//endex
				
				//ifex _AlphaAudioLinkEnabled==0 && isNotAnimated(_AlphaAudioLinkEnabled)
				#ifdef POI_AUDIOLINK
				if (poiMods.audioLinkAvailable && _AlphaAudioLinkEnabled)
				{
					poiFragData.alpha = saturate(poiFragData.alpha + lerp(_AlphaAudioLinkAddRange.x, _AlphaAudioLinkAddRange.y, poiMods.audioLink[_AlphaAudioLinkAddBand]));
				}
				#endif
				//endex
				
			}
			
			//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
			inline half Dither8x8Bayer(int x, int y)
			{
				// Premultiplied by 1/64
				const half dither[ 64 ] = {
					0.015625, 0.765625, 0.203125, 0.953125, 0.06250, 0.81250, 0.25000, 1.00000,
					0.515625, 0.265625, 0.703125, 0.453125, 0.56250, 0.31250, 0.75000, 0.50000,
					0.140625, 0.890625, 0.078125, 0.828125, 0.18750, 0.93750, 0.12500, 0.87500,
					0.640625, 0.390625, 0.578125, 0.328125, 0.68750, 0.43750, 0.62500, 0.37500,
					0.046875, 0.796875, 0.234375, 0.984375, 0.03125, 0.78125, 0.21875, 0.96875,
					0.546875, 0.296875, 0.734375, 0.484375, 0.53125, 0.28125, 0.71875, 0.46875,
					0.171875, 0.921875, 0.109375, 0.859375, 0.15625, 0.90625, 0.09375, 0.84375,
					0.671875, 0.421875, 0.609375, 0.359375, 0.65625, 0.40625, 0.59375, 0.34375
				};
				int r = y * 8 + x;
				return dither[r];
			}
			
			half calcDither(half2 grabPos)
			{
				return Dither8x8Bayer(glsl_mod(grabPos.x, 8), glsl_mod(grabPos.y, 8));
			}
			
			void applyDithering(inout PoiFragData poiFragData, in PoiCam poiCam)
			{
				if (_AlphaDithering)
				{
					float dither = calcDither(poiCam.posScreenPixels) - _AlphaDitherBias;
					poiFragData.alpha = saturate(poiFragData.alpha - (dither * (1 - poiFragData.alpha) * _AlphaDitherGradient));
				}
			}
			//endex
			
			//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
			void ApplyAlphaToCoverage(inout PoiFragData poiFragData, in PoiMesh poiMesh)
			{
				// Force Model Opacity to 1 if desired
				UNITY_BRANCH
				if (_Mode == 1)
				{
					UNITY_BRANCH
					if (_AlphaSharpenedA2C && _AlphaToCoverage)
					{
						// rescale alpha by mip level
						poiFragData.alpha *= 1 + max(0, CalcMipLevel(poiMesh.uv[0] * _MainTex_TexelSize.zw)) * _AlphaMipScale;
						// rescale alpha by partial derivative
						poiFragData.alpha = (poiFragData.alpha - _Cutoff) / max(fwidth(poiFragData.alpha), 0.0001) + _Cutoff;
						poiFragData.alpha = saturate(poiFragData.alpha);
					}
				}
			}
			//endex
			
			//ifex _PoiParallax==0
			#ifdef POI_PARALLAX
			inline float2 POM(in PoiLight poiLight, sampler2D heightMap, in PoiMesh poiMesh, float3 worldViewDir, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv)
			{
				#if defined(PROP_HEIGHTMASK) || !defined(OPTIMIZER_ENABLED)
				float heightMask = POI2D_SAMPLER_PAN(_Heightmask, _linear_repeat, poiUV(poiMesh.uv[_HeightmaskUV], _Heightmask_ST), _HeightmaskPan)[_HeightmaskChannel];
				if (_HeightmaskInvert)
				{
					heightMask = 1 - heightMask;
				}
				#else
				float heightMask = 1;
				#endif
				
				float2 uvs = poiUV(poiMesh.uv[_HeightMapUV], _HeightMap_ST);
				float2 dx = ddx(uvs);
				float2 dy = ddy(uvs);
				float3 result = 0;
				int stepIndex = 0;
				int numSteps = (int)lerp(maxSamples, minSamples, saturate(dot(poiMesh.normals[0], worldViewDir)));
				float layerHeight = 1.0 / numSteps;
				float2 plane = parallax * heightMask * (viewDirTan.xy / viewDirTan.z);
				uvs += refPlane * plane;
				float2 deltaTex = -plane * layerHeight;
				float2 prevTexOffset = 0;
				float prevRayZ = 1.0f;
				float prevHeight = 0.0f;
				float2 currTexOffset = deltaTex;
				float currRayZ = 1.0f - layerHeight;
				float currHeight = 0.0f;
				float intersection = 0;
				float2 finalTexOffset = 0;
				while (stepIndex < numSteps + 1)
				{
					result.z = dot(curv, currTexOffset * currTexOffset);
					currHeight = tex2Dgrad(heightMap, uvs + currTexOffset, dx, dy).r * (1 - result.z);
					if (currHeight > currRayZ)
					{
						stepIndex = numSteps + 1;
					}
					else
					{
						stepIndex++;
						prevTexOffset = currTexOffset;
						prevRayZ = currRayZ;
						prevHeight = currHeight;
						currTexOffset += deltaTex;
						currRayZ -= layerHeight * (1 - result.z) * (1 + _CurvFix);
					}
				}
				int sectionSteps = 10;
				int sectionIndex = 0;
				float newZ = 0;
				float newHeight = 0;
				while (sectionIndex < sectionSteps)
				{
					intersection = (prevHeight - prevRayZ) / (prevHeight - currHeight + currRayZ - prevRayZ);
					finalTexOffset = prevTexOffset +intersection * deltaTex;
					newZ = prevRayZ - intersection * layerHeight;
					newHeight = tex2Dgrad(heightMap, uvs + finalTexOffset, dx, dy).r;
					if (newHeight > newZ)
					{
						currTexOffset = finalTexOffset;
						currHeight = newHeight;
						currRayZ = newZ;
						deltaTex = intersection * deltaTex;
						layerHeight = intersection * layerHeight;
					}
					else
					{
						prevTexOffset = finalTexOffset;
						prevHeight = newHeight;
						prevRayZ = newZ;
						deltaTex = (1 - intersection) * deltaTex;
						layerHeight = (1 - intersection) * layerHeight;
					}
					sectionIndex++;
				}
				#ifdef UNITY_PASS_SHADOWCASTER
				if (unity_LightShadowBias.z == 0.0)
				{
					#endif
					if (result.z > 1)
					clip(-1);
					#ifdef UNITY_PASS_SHADOWCASTER
				}
				#endif
				
				return uvs + finalTexOffset;
			}
			/*
			float2 ParallaxOffsetMultiStep(float surfaceHeight, float strength, float2 uv, float3 tangentViewDir)
			{
				float2 uvOffset = 0;
				float2 prevUVOffset = 0;
				float stepSize = 1.0 / _HeightSteps;
				float stepHeight = 1;
				float2 uvDelta = tangentViewDir.xy * (stepSize * strength);
				float prevStepHeight = stepHeight;
				float prevSurfaceHeight = surfaceHeight;
				
				[unroll(20)]
				for (int j = 1; j <= _HeightSteps && stepHeight > surfaceHeight; j++)
				{
					prevUVOffset = uvOffset;
					prevStepHeight = stepHeight;
					prevSurfaceHeight = surfaceHeight;
					uvOffset -= uvDelta;
					stepHeight -= stepSize;
					surfaceHeight = POI2D_SAMPLER_PAN(_Heightmap, _MainTex, poiUV(uv + uvOffset, _Heightmap_ST), _HeightmapPan) + _HeightOffset;
				}
				
				[unroll(3)]
				for (int k = 0; k < 3; k++)
				{
					uvDelta *= 0.5;
					stepSize *= 0.5;
					
					if (stepHeight < surfaceHeight)
					{
						uvOffset += uvDelta;
						stepHeight += stepSize;
					}
					else
					{
						uvOffset -= uvDelta;
						stepHeight -= stepSize;
					}
					surfaceHeight = POI2D_SAMPLER_PAN(_Heightmap, _MainTex, poiUV(uv + uvOffset, _Heightmap_ST), _HeightmapPan) + _HeightOffset;
				}
				return uvOffset;
			}
			*/
			void applyParallax(inout PoiMesh poiMesh, in PoiLight poiLight, in PoiCam poiCam)
			{
				/*
				half h = POI2D_SAMPLER_PAN(_Heightmap, _linear_repeat, poiUV(poiMesh.uv[_HeightmaskUV], _Heightmap_ST), _HeightmapPan).r + _HeightOffset;
				#if defined(PROP_HEIGHTMASK) || !defined(OPTIMIZER_ENABLED)
				half m = POI2D_SAMPLER_PAN(_Heightmask, _linear_repeat, poiUV(poiMesh.uv[_HeightmaskUV], _Heightmask_ST), _HeightmaskPan).r + _HeightOffset;
				#else
				half m = 1 + _HeightOffset;
				#endif
				h = clamp(h, 0, 0.999);
				m = lerp(m, 1 - m, _HeightmaskInvert);
				#if defined(OPTIMIZER_ENABLED)das
				poiMesh.uv[_ParallaxUV] += ParallaxOffsetMultiStep(h, _HeightStrength * m, poiMesh.uv[_HeightmapUV], tangentViewDir / tangentViewDir.z);
				#else
				float2 offset = ParallaxOffsetMultiStep(h, _HeightStrength * m, poiMesh.uv[_HeightmapUV], tangentViewDir / tangentViewDir.z);
				if (_ParallaxUV == 0)       poiMesh.uv[0] += offset;
				if (_ParallaxUV == 1)       poiMesh.uv[1] += offset;
				if (_ParallaxUV == 2)       poiMesh.uv[2] += offset;
				if (_ParallaxUV == 3)       poiMesh.uv[3] += offset;
				if (_ParallaxUV == 4)       poiMesh.uv[4] += offset;
				if (_ParallaxUV == 5)       poiMesh.uv[5] += offset;
				if (_ParallaxUV == 6)       poiMesh.uv[6] += offset;
				if (_ParallaxUV == 7)       poiMesh.uv[7] += offset;
				#endif
				*/
				
				#if defined(OPTIMIZER_ENABLED)
				poiMesh.uv[_ParallaxUV] = POM(poiLight, _HeightMap, poiMesh, poiCam.viewDir, poiCam.tangentViewDir, _HeightStepsMin, _HeightStepsMax, _HeightStrength, 0, _HeightMap_ST.xy, float2(_CurvatureU, _CurvatureV));
				#else
				float2 offset = POM(poiLight, _HeightMap, poiMesh, poiCam.viewDir, poiCam.tangentViewDir, _HeightStepsMin, _HeightStepsMax, _HeightStrength, 0, _HeightMap_ST.xy, float2(_CurvatureU, _CurvatureV));
				if (_ParallaxUV == 0)       poiMesh.uv[0] = offset;
				if (_ParallaxUV == 1)       poiMesh.uv[1] = offset;
				if (_ParallaxUV == 2)       poiMesh.uv[2] = offset;
				if (_ParallaxUV == 3)       poiMesh.uv[3] = offset;
				if (_ParallaxUV == 4)       poiMesh.uv[4] = offset;
				if (_ParallaxUV == 5)       poiMesh.uv[5] = offset;
				if (_ParallaxUV == 6)       poiMesh.uv[6] = offset;
				if (_ParallaxUV == 7)       poiMesh.uv[7] = offset;
				#endif
			}
			#endif
			//endex
			
			float4 frag(VertexOut i, uint facing : SV_IsFrontFace) : SV_Target
			{
				
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				
				PoiMesh poiMesh;
				PoiInitStruct(PoiMesh, poiMesh);
				
				PoiLight poiLight;
				PoiInitStruct(PoiLight, poiLight);
				
				PoiVertexLights poiVertexLights;
				PoiInitStruct(PoiVertexLights, poiVertexLights);
				
				PoiCam poiCam;
				PoiInitStruct(PoiCam, poiCam);
				
				PoiMods poiMods;
				PoiInitStruct(PoiMods, poiMods);
				poiMods.globalEmission = 1;
				
				PoiFragData poiFragData;
				poiFragData.smoothness = 1;
				poiFragData.smoothness2 = 1;
				poiFragData.metallic = 1;
				poiFragData.specularMask = 1;
				poiFragData.reflectionMask = 1;
				poiFragData.emission = 0;
				poiFragData.baseColor = float3(0, 0, 0);
				poiFragData.finalColor = float3(0, 0, 0);
				poiFragData.alpha = 1;
				poiFragData.toggleVertexLights = 0;
				
				// Mesh Data
				poiMesh.objectPosition = mul(unity_ObjectToWorld, float3(0, 0, 0)).xyz;
				poiMesh.objNormal = mul(unity_WorldToObject, i.normal);
				poiMesh.normals[0] = i.normal;
				poiMesh.tangent[0] = i.tangent.xyz;
				poiMesh.binormal[0] = cross(i.normal, i.tangent.xyz) * (i.tangent.w * unity_WorldTransformParams.w);
				poiMesh.worldPos = i.worldPos.xyz;
				poiMesh.localPos = i.localPos.xyz;
				poiMesh.vertexColor = i.vertexColor;
				poiMesh.isFrontFace = facing;
				poiMesh.dx = ddx(poiMesh.uv[0]);
				poiMesh.dy = ddy(poiMesh.uv[0]);
				
				#ifndef POI_PASS_OUTLINE
				if (!poiMesh.isFrontFace && _FlipBackfaceNormals)
				{
					poiMesh.normals[0] *= -1;
					poiMesh.tangent[0] *= -1;
					poiMesh.binormal[0] *= -1;
				}
				#endif
				
				poiCam.viewDir = !IsOrthographicCamera() ? normalize(_WorldSpaceCameraPos - i.worldPos.xyz) : normalize(UNITY_MATRIX_I_V._m02_m12_m22);
				float3 tanToWorld0 = float3(poiMesh.tangent[0].x, poiMesh.binormal[0].x, poiMesh.normals[0].x);
				float3 tanToWorld1 = float3(poiMesh.tangent[0].y, poiMesh.binormal[0].y, poiMesh.normals[0].y);
				float3 tanToWorld2 = float3(poiMesh.tangent[0].z, poiMesh.binormal[0].z, poiMesh.normals[0].z);
				float3 ase_tanViewDir = tanToWorld0 * poiCam.viewDir.x + tanToWorld1 * poiCam.viewDir.y + tanToWorld2 * poiCam.viewDir.z;
				poiCam.tangentViewDir = normalize(ase_tanViewDir);
				
				// 0-3 UV0-UV3
				// 4 Panosphere UV
				// 5 world pos xz
				// 6 Polar UV
				// 6 Distorted UV
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
				poiMesh.lightmapUV = i.lightmapUV;
				#endif
				poiMesh.parallaxUV = poiCam.tangentViewDir.xy / max(poiCam.tangentViewDir.z, 0.0001);
				poiMesh.uv[0] = i.uv[0].xy;
				poiMesh.uv[1] = i.uv[0].zw;
				poiMesh.uv[2] = i.uv[1].xy;
				poiMesh.uv[3] = i.uv[1].zw;
				poiMesh.uv[4] = poiMesh.uv[0];
				poiMesh.uv[5] = poiMesh.uv[0];
				poiMesh.uv[6] = poiMesh.uv[0];
				poiMesh.uv[7] = poiMesh.uv[0];
				poiMesh.uv[8] = poiMesh.uv[0];
				
				//ifex _PoiParallax==0
				#ifdef POI_PARALLAX
				#ifndef POI_PASS_OUTLINE
				//return frac(i.tangentViewDir.x);
				//return float4(i.binormal.xyz,1);
				applyParallax(poiMesh, poiLight, poiCam);
				#endif
				#endif
				//endex
				
				float2 mainUV = poiUV(poiMesh.uv[_MainTexUV].xy, _MainTex_ST);
				
				if (_MainPixelMode)
				{
					mainUV = sharpSample(_MainTex_TexelSize, mainUV);
				}
				
				float4 mainTexture = POI2D_SAMPLER_PAN_STOCHASTIC(_MainTex, _MainTex, mainUV, _MainTexPan, _MainTexStochastic);
				
				#if defined(PROP_BUMPMAP) || !defined(OPTIMIZER_ENABLED)
				poiMesh.tangentSpaceNormal = UnpackScaleNormal(POI2D_SAMPLER_PAN_STOCHASTIC(_BumpMap, _MainTex, poiUV(poiMesh.uv[_BumpMapUV].xy, _BumpMap_ST), _BumpMapPan, _BumpMapStochastic), _BumpScale);
				#else
				poiMesh.tangentSpaceNormal = UnpackNormal(float4(0.5, 0.5, 1, 1));
				#endif
				
				poiMesh.normals[1] = normalize(
				poiMesh.tangentSpaceNormal.x * poiMesh.tangent[0] +
				poiMesh.tangentSpaceNormal.y * poiMesh.binormal[0] +
				poiMesh.tangentSpaceNormal.z * poiMesh.normals[0]
				);
				
				poiMesh.tangent[1] = cross(poiMesh.binormal[0], -poiMesh.normals[1]);
				poiMesh.binormal[1] = cross(-poiMesh.normals[1], poiMesh.tangent[0]);
				
				// Camera data
				poiCam.forwardDir = getCameraForward();
				poiCam.worldPos = _WorldSpaceCameraPos;
				poiCam.reflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[1]);
				poiCam.vertexReflectionDir = reflect(-poiCam.viewDir, poiMesh.normals[0]);
				//poiCam.distanceToModel = distance(poiMesh.modelPos, poiCam.worldPos);
				poiCam.clipPos = i.pos;
				poiCam.distanceToVert = distance(poiMesh.worldPos, poiCam.worldPos);
				poiCam.posScreenSpace = poiTransformClipSpacetoScreenSpaceFrag(poiCam.clipPos);
				#if defined(POI_GRABPASS) && defined(POI_PASS_BASE)
				poiCam.screenUV = poiCam.clipPos.xy / poiGetWidthAndHeight(_PoiGrab2);
				#endif
				poiCam.posScreenPixels = calcPixelScreenUVs(poiCam.posScreenSpace);
				poiCam.vDotN = abs(dot(poiCam.viewDir, poiMesh.normals[1]));
				
				poiCam.worldDirection.xyz = poiMesh.worldPos.xyz - poiCam.worldPos;
				poiCam.worldDirection.w = dot(poiCam.clipPos, CalculateFrustumCorrection());
				
				poiFragData.baseColor = mainTexture.rgb * poiThemeColor(poiMods, _Color.rgb, _ColorThemeIndex);
				poiFragData.alpha = mainTexture.a * _Color.a;
				
				//ifex _MainColorAdjustToggle==0
				#ifdef COLOR_GRADING_HDR
				#if defined(PROP_MAINCOLORADJUSTTEXTURE) || !defined(OPTIMIZER_ENABLED)
				float4 hueShiftAlpha = POI2D_SAMPLER_PAN(_MainColorAdjustTexture, _MainTex, poiUV(poiMesh.uv[_MainColorAdjustTextureUV], _MainColorAdjustTexture_ST), _MainColorAdjustTexturePan);
				#else
				float4 hueShiftAlpha = 1;
				#endif
				
				if (_MainHueGlobalMask > 0)
				{
					hueShiftAlpha.r = maskBlend(hueShiftAlpha.r, poiMods.globalMask[_MainHueGlobalMask - 1], _MainHueGlobalMaskBlendType);
				}
				if (_MainSaturationGlobalMask > 0)
				{
					hueShiftAlpha.b = maskBlend(hueShiftAlpha.b, poiMods.globalMask[_MainSaturationGlobalMask - 1], _MainSaturationGlobalMaskBlendType);
				}
				if (_MainBrightnessGlobalMask > 0)
				{
					hueShiftAlpha.g = maskBlend(hueShiftAlpha.g, poiMods.globalMask[_MainBrightnessGlobalMask - 1], _MainBrightnessGlobalMaskBlendType);
				}
				
				if (_MainHueShiftToggle)
				{
					float shift = _MainHueShift;
					#ifdef POI_AUDIOLINK
					//UNITY_BRANCH
					if (poiMods.audioLinkAvailable && _MainHueALCTEnabled)
					{
						shift += AudioLinkGetChronoTime(_MainALHueShiftCTIndex, _MainALHueShiftBand) * _MainHueALMotionSpeed;
					}
					#endif
					if (_MainHueShiftReplace)
					{
						poiFragData.baseColor = lerp(poiFragData.baseColor, hueShift(poiFragData.baseColor, shift + _MainHueShiftSpeed * _Time.x), hueShiftAlpha.r);
					}
					else
					{
						poiFragData.baseColor = hueShift(poiFragData.baseColor, frac((shift - (1 - hueShiftAlpha.r) + _MainHueShiftSpeed * _Time.x)));
					}
				}
				
				#if defined(PROP_MAINGRADATIONTEX) || !defined(OPTIMIZER_ENABLED)
				
				if (_MainGradationStrength && _ColorGradingToggle)
				{
					#if !defined(UNITY_COLORSPACE_GAMMA)
					float3 tempColor = OpenLitLinearToSRGB(poiFragData.baseColor);
					#endif
					tempColor.r = POI_SAMPLE_1D_X(_MainGradationTex, sampler_linear_clamp, tempColor.r).r;
					tempColor.g = POI_SAMPLE_1D_X(_MainGradationTex, sampler_linear_clamp, tempColor.g).g;
					tempColor.b = POI_SAMPLE_1D_X(_MainGradationTex, sampler_linear_clamp, tempColor.b).b;
					#if !defined(UNITY_COLORSPACE_GAMMA)
					tempColor = OpenLitSRGBToLinear(tempColor);
					#endif
					poiFragData.baseColor = lerp(poiFragData.baseColor, tempColor, _MainGradationStrength);
				}
				#endif
				
				poiFragData.baseColor = lerp(poiFragData.baseColor, dot(poiFragData.baseColor, float3(0.3, 0.59, 0.11)), - (_Saturation) * hueShiftAlpha.b);
				poiFragData.baseColor = saturate(lerp(poiFragData.baseColor, poiFragData.baseColor * (_MainBrightness + 1), hueShiftAlpha.g));
				#endif
				//endex
				
				#if defined(PROP_ALPHAMASK) || !defined(OPTIMIZER_ENABLED)
				
				if (_AlphaMaskMode)
				{
					float alphaMask = POI2D_SAMPLER_PAN(_AlphaMask, _MainTex, poiUV(poiMesh.uv[_AlphaMaskUV], _AlphaMask_ST), _AlphaMaskPan.xy).r;
					alphaMask = saturate(alphaMask * _AlphaMaskScale + _AlphaMaskValue);
					if (_AlphaMaskInvert) alphaMask = 1 - alphaMask;
					if (_AlphaMaskMode == 1) poiFragData.alpha = alphaMask;
					if (_AlphaMaskMode == 2) poiFragData.alpha = poiFragData.alpha * alphaMask;
					if (_AlphaMaskMode == 3) poiFragData.alpha = saturate(poiFragData.alpha + alphaMask);
					if (_AlphaMaskMode == 4) poiFragData.alpha = saturate(poiFragData.alpha - alphaMask);
				}
				#endif
				
				applyAlphaOptions(poiFragData, poiMesh, poiCam, poiMods);
				
				poiFragData.finalColor = poiFragData.baseColor;
				
				//UNITY_BRANCH
				if (_IgnoreFog == 0)
				{
					UNITY_APPLY_FOG(i.fogCoord, poiFragData.finalColor);
				}
				
				poiFragData.alpha = _AlphaForceOpaque ? 1 : poiFragData.alpha;
				
				//ifex _AlphaToCoverage==0 && isNotAnimated(_AlphaToCoverage)
				ApplyAlphaToCoverage(poiFragData, poiMesh);
				//endex
				
				//ifex _AlphaDithering==0 && isNotAnimated(_AlphaDithering)
				applyDithering(poiFragData, poiCam);
				//endex
				
				if (_Mode == POI_MODE_OPAQUE)
				{
					poiFragData.alpha = 1;
				}
				
				clip(poiFragData.alpha - _Cutoff);
				
				return float4(poiFragData.finalColor, poiFragData.alpha) + POI_SAFE_RGB0;
			}
			
			ENDCG
		}
		
	}
	CustomEditor "Thry.ShaderEditor"
}
