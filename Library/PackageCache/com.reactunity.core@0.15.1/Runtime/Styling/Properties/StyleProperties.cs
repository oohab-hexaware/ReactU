using System;
using System.Collections.Generic;
using Facebook.Yoga;

using ReactUnity.Styling.Animations;
using ReactUnity.Styling.Computed;
using ReactUnity.Styling.Converters;
using ReactUnity.Types;
using TMPro;
using UnityEngine;
using Cursor = ReactUnity.Types.Cursor;
using NavigationMode = UnityEngine.UI.Navigation.Mode;

namespace ReactUnity.Styling
{
    public static class StyleProperties
    {
        public static readonly StyleProperty<float> opacity = new StyleProperty<float>("opacity", 1f, true, converter: AllConverters.PercentageConverter);
        public static readonly StyleProperty<int> zIndex = new StyleProperty<int>("zIndex", 0, false);
        public static readonly StyleProperty<SortingLayer> sortingLayer = new StyleProperty<SortingLayer>("sortingLayer", default(SortingLayer), false);
        public static readonly StyleProperty<bool> visibility = new StyleProperty<bool>("visibility", true, converter: new BoolConverter(new string[] { "visible" }, new string[] { "hidden" }));
        public static readonly StyleProperty<PositionType> position = new StyleProperty<PositionType>("position", PositionType.Relative);
        public static readonly ValueListStyleProperty<Cursor> cursor = new ValueListStyleProperty<Cursor>("cursor");
        public static readonly StyleProperty<Isolation> isolation = new StyleProperty<Isolation>("isolation", Isolation.Auto);
        public static readonly StyleProperty<PointerEvents> pointerEvents = new StyleProperty<PointerEvents>("pointerEvents", PointerEvents.Auto);
        public static readonly StyleProperty<YogaValue2> borderTopLeftRadius = new StyleProperty<YogaValue2>("borderTopLeftRadius", YogaValue2.Zero, true, converter: AllConverters.BorderRadiusConverter);
        public static readonly StyleProperty<YogaValue2> borderTopRightRadius = new StyleProperty<YogaValue2>("borderTopRightRadius", YogaValue2.Zero, true, converter: AllConverters.BorderRadiusConverter);
        public static readonly StyleProperty<YogaValue2> borderBottomLeftRadius = new StyleProperty<YogaValue2>("borderBottomLeftRadius", YogaValue2.Zero, true, converter: AllConverters.BorderRadiusConverter);
        public static readonly StyleProperty<YogaValue2> borderBottomRightRadius = new StyleProperty<YogaValue2>("borderBottomRightRadius", YogaValue2.Zero, true, converter: AllConverters.BorderRadiusConverter);
        public static readonly StyleProperty<Color> borderLeftColor = new StyleProperty<Color>("borderLeftColor", Color.black, true);
        public static readonly StyleProperty<Color> borderRightColor = new StyleProperty<Color>("borderRightColor", Color.black, true);
        public static readonly StyleProperty<Color> borderTopColor = new StyleProperty<Color>("borderTopColor", Color.black, true);
        public static readonly StyleProperty<Color> borderBottomColor = new StyleProperty<Color>("borderBottomColor", Color.black, true);
        public static readonly StyleProperty<BorderStyle> borderLeftStyle = new StyleProperty<BorderStyle>("borderLeftStyle", BorderStyle.Solid, false);
        public static readonly StyleProperty<BorderStyle> borderRightStyle = new StyleProperty<BorderStyle>("borderRightStyle", BorderStyle.Solid, false);
        public static readonly StyleProperty<BorderStyle> borderTopStyle = new StyleProperty<BorderStyle>("borderTopStyle", BorderStyle.Solid, false);
        public static readonly StyleProperty<BorderStyle> borderBottomStyle = new StyleProperty<BorderStyle>("borderBottomStyle", BorderStyle.Solid, false);
        public static readonly StyleProperty<float> outlineOffset = new StyleProperty<float>("outlineOffset", 0f, true);
        public static readonly StyleProperty<float> outlineWidth = new StyleProperty<float>("outlineWidth", 0f, true);
        public static readonly StyleProperty<Color> outlineColor = new StyleProperty<Color>("outlineColor", Color.black, true);
        public static readonly StyleProperty<BorderStyle> outlineStyle = new StyleProperty<BorderStyle>("outlineStyle", BorderStyle.Solid, false);
        public static readonly StyleProperty<YogaValue2> transformOrigin = new StyleProperty<YogaValue2>("transformOrigin", YogaValue2.Center, true);
        public static readonly StyleProperty<YogaValue2> translate = new StyleProperty<YogaValue2>("translate", YogaValue2.Zero, true);
        public static readonly StyleProperty<YogaValue> translateZ = new StyleProperty<YogaValue>("translateZ", YogaValue.Point(0), true);
        public static readonly StyleProperty<Vector3> scale = new StyleProperty<Vector3>("scale", Vector3.one, true, false, AllConverters.ScaleConverter);
        public static readonly StyleProperty<Vector3> rotate = new StyleProperty<Vector3>("rotate", Vector3.zero, true, converter: AllConverters.RotateConverter);
        public static readonly StyleProperty<FontReference> fontFamily = new StyleProperty<FontReference>("fontFamily", FontReference.None, false, true);
        public static readonly StyleProperty<Color> color = new StyleProperty<Color>("color", ComputedCurrentColor.Instance, true, false);
        public static readonly StyleProperty<FontWeight> fontWeight = new StyleProperty<FontWeight>("fontWeight", FontWeight.Regular, false, true);
        public static readonly StyleProperty<FontStyles> fontStyle = new StyleProperty<FontStyles>("fontStyle", FontStyles.Normal, false, true, converter: new EnumConverter(typeof(FontStyles), true, new Dictionary<string, object> { { "linethrough", FontStyles.Strikethrough }, { "solid", FontStyles.Underline } }));
        public static readonly StyleProperty<TextTransform> textTransform = new StyleProperty<TextTransform>("textTransform", TextTransform.None, false, true);
        public static readonly StyleProperty<float> fontSize = new StyleProperty<float>("fontSize", ComputedFontSize.Default, true, false, AllConverters.FontSizeConverter);
        public static readonly StyleProperty<float> lineHeight = new StyleProperty<float>("lineHeight", new ComputedFontProperty(1, ComputedFontProperty.FontPropertyType.LineHeight), true, true, AllConverters.FontSizeConverter);
        public static readonly StyleProperty<float> letterSpacing = new StyleProperty<float>("letterSpacing", 0f, true, true, AllConverters.FontSizeConverter);
        public static readonly StyleProperty<float> wordSpacing = new StyleProperty<float>("wordSpacing", 0f, true, true, AllConverters.FontSizeConverter);
        public static readonly StyleProperty<TextAlignmentOptions> textAlign = new StyleProperty<TextAlignmentOptions>("textAlign", TextAlignmentOptions.Converted, false, true);
        public static readonly StyleProperty<VerticalAlignmentOptions> verticalAlign = new StyleProperty<VerticalAlignmentOptions>("verticalAlign", VerticalAlignmentOptions.Top, false, true);
        public static readonly StyleProperty<TextOverflowModes> textOverflow = new StyleProperty<TextOverflowModes>("textOverflow", TextOverflowModes.Overflow, false, true);
        public static readonly StyleProperty<bool> textWrap = new StyleProperty<bool>("textWrap", true, inherited: true, converter: new BoolConverter(new string[] { "wrap", "normal" }, new string[] { "nowrap" }));
        public static readonly StyleProperty<int> maxLines = new StyleProperty<int>("maxLines", (int) short.MaxValue, true, true);
        public static readonly StyleProperty<float> textStrokeWidth = new StyleProperty<float>("textStrokeWidth", 0f, true, true);
        public static readonly StyleProperty<Color> textStrokeColor = new StyleProperty<Color>("textStrokeColor", ComputedCurrentColor.Instance, true, true);
        public static readonly StyleProperty<string> content = new StyleProperty<string>("content", null, false);
        public static readonly StyleProperty<Appearance> appearance = new StyleProperty<Appearance>("appearance", Appearance.None);
        public static readonly StyleProperty<NavigationMode> navigation = new StyleProperty<NavigationMode>("navigation", NavigationMode.Automatic);
        public static readonly StyleProperty<float> stateDuration = new StyleProperty<float>("stateDuration", 0f, true, false, AllConverters.DurationConverter);
        public static readonly StyleProperty<ObjectFit> objectFit = new StyleProperty<ObjectFit>("objectFit", ObjectFit.Fill);
        public static readonly StyleProperty<YogaValue2> objectPosition = new StyleProperty<YogaValue2>("objectPosition", YogaValue2.Center, true);

        public static readonly ValueListStyleProperty<BoxShadow> boxShadow = new ValueListStyleProperty<BoxShadow>("boxShadow", BoxShadow.Default, true);

        public static readonly StyleProperty<ImageDefinition> borderImageSource = new StyleProperty<ImageDefinition>("borderImageSource");
        public static readonly StyleProperty<BorderImageSlice> borderImageSlice = new StyleProperty<BorderImageSlice>("borderImageSlice", BorderImageSlice.Auto, true);
        public static readonly FourDirectionalStyleProperty<BackgroundRepeat> borderImageRepeat = new FourDirectionalStyleProperty<BackgroundRepeat>("borderImageRepeat", BackgroundRepeat.Stretch, false);
        public static readonly FourDirectionalStyleProperty<YogaValue> borderImageOutset = new FourDirectionalStyleProperty<YogaValue>("borderImageOutset", YogaValue.Point(0), true);
        public static readonly FourDirectionalStyleProperty<YogaValue> borderImageWidth = new FourDirectionalStyleProperty<YogaValue>("borderImageWidth", YogaValue.Auto(), true);

        public static readonly StyleProperty<Color> backgroundColor = new StyleProperty<Color>("backgroundColor", new Color(0, 0, 0, 0), true);
        public static readonly ValueListStyleProperty<ImageDefinition> backgroundImage = new ValueListStyleProperty<ImageDefinition>("backgroundImage");
        public static readonly ValueListStyleProperty<YogaValue> backgroundPositionX = new ValueListStyleProperty<YogaValue>("backgroundPositionX");
        public static readonly ValueListStyleProperty<YogaValue> backgroundPositionY = new ValueListStyleProperty<YogaValue>("backgroundPositionY");
        public static readonly ValueListStyleProperty<BackgroundSize> backgroundSize = new ValueListStyleProperty<BackgroundSize>("backgroundSize", BackgroundSize.Auto);
        public static readonly ValueListStyleProperty<BackgroundRepeat> backgroundRepeatX = new ValueListStyleProperty<BackgroundRepeat>("backgroundRepeatX", BackgroundRepeat.Repeat);
        public static readonly ValueListStyleProperty<BackgroundRepeat> backgroundRepeatY = new ValueListStyleProperty<BackgroundRepeat>("backgroundRepeatY", BackgroundRepeat.Repeat);
        public static readonly StyleProperty<BackgroundBlendMode> backgroundBlendMode = new StyleProperty<BackgroundBlendMode>("backgroundBlendMode", BackgroundBlendMode.Normal);

        public static readonly ValueListStyleProperty<ImageDefinition> maskImage = new ValueListStyleProperty<ImageDefinition>("maskImage");
        public static readonly ValueListStyleProperty<YogaValue> maskPositionX = new ValueListStyleProperty<YogaValue>("maskPositionX");
        public static readonly ValueListStyleProperty<YogaValue> maskPositionY = new ValueListStyleProperty<YogaValue>("maskPositionY");
        public static readonly ValueListStyleProperty<BackgroundSize> maskSize = new ValueListStyleProperty<BackgroundSize>("maskSize", BackgroundSize.Auto);
        public static readonly ValueListStyleProperty<BackgroundRepeat> maskRepeatX = new ValueListStyleProperty<BackgroundRepeat>("maskRepeatX", BackgroundRepeat.Repeat);
        public static readonly ValueListStyleProperty<BackgroundRepeat> maskRepeatY = new ValueListStyleProperty<BackgroundRepeat>("maskRepeatY", BackgroundRepeat.Repeat);

        public static readonly ValueListStyleProperty<TransitionProperty> transitionProperty = new ValueListStyleProperty<TransitionProperty>("transitionProperty");
        public static readonly ValueListStyleProperty<float> transitionDuration = new ValueListStyleProperty<float>("transitionDuration", baseConverter: AllConverters.DurationConverter);
        public static readonly ValueListStyleProperty<TimingFunction> transitionTimingFunction = new ValueListStyleProperty<TimingFunction>("transitionTimingFunction", TimingFunctions.Default);
        public static readonly ValueListStyleProperty<float> transitionDelay = new ValueListStyleProperty<float>("transitionDelay", baseConverter: AllConverters.DurationConverter);
        public static readonly ValueListStyleProperty<AnimationPlayState> transitionPlayState = new ValueListStyleProperty<AnimationPlayState>("transitionPlayState");

        public static readonly StyleProperty<float> motionDuration = new StyleProperty<float>("motionDuration", 0f, false);
        public static readonly StyleProperty<TimingFunction> motionTimingFunction = new StyleProperty<TimingFunction>("motionTimingFunction", TimingFunctions.Default, false);
        public static readonly StyleProperty<float> motionDelay = new StyleProperty<float>("motionDelay", 0f, false);

        public static readonly ValueListStyleProperty<float> animationDelay = new ValueListStyleProperty<float>("animationDelay", baseConverter: AllConverters.DurationConverter);
        public static readonly ValueListStyleProperty<AnimationDirection> animationDirection = new ValueListStyleProperty<AnimationDirection>("animationDirection");
        public static readonly ValueListStyleProperty<float> animationDuration = new ValueListStyleProperty<float>("animationDuration", baseConverter: AllConverters.DurationConverter);
        public static readonly ValueListStyleProperty<AnimationFillMode> animationFillMode = new ValueListStyleProperty<AnimationFillMode>("animationFillMode");
        public static readonly ValueListStyleProperty<int> animationIterationCount = new ValueListStyleProperty<int>("animationIterationCount", 1, baseConverter: AllConverters.IterationCountConverter);
        public static readonly ValueListStyleProperty<string> animationName = new ValueListStyleProperty<string>("animationName");
        public static readonly ValueListStyleProperty<AnimationPlayState> animationPlayState = new ValueListStyleProperty<AnimationPlayState>("animationPlayState");
        public static readonly ValueListStyleProperty<TimingFunction> animationTimingFunction = new ValueListStyleProperty<TimingFunction>("animationTimingFunction", TimingFunctions.Default);

        public static readonly ValueListStyleProperty<AudioReference> audioClip = new ValueListStyleProperty<AudioReference>("audioClip");
        public static readonly ValueListStyleProperty<int> audioIterationCount = new ValueListStyleProperty<int>("audioIterationCount", 1);
        public static readonly ValueListStyleProperty<float> audioDelay = new ValueListStyleProperty<float>("audioDelay");

        public static readonly Dictionary<string, IStyleProperty> PropertyMap = new Dictionary<string, IStyleProperty>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "opacity", opacity },
            { "zIndex", zIndex },
            { "sortingLayer", sortingLayer },
            { "visibility", visibility },
            { "position", position },
            { "cursor", cursor },
            { "isolation", isolation },
            { "pointerEvents", pointerEvents },
            { "borderTopLeftRadius", borderTopLeftRadius },
            { "borderTopRightRadius", borderTopRightRadius },
            { "borderBottomLeftRadius", borderBottomLeftRadius },
            { "borderBottomRightRadius", borderBottomRightRadius },
            { "borderLeftColor", borderLeftColor },
            { "borderRightColor", borderRightColor },
            { "borderTopColor", borderTopColor },
            { "borderBottomColor", borderBottomColor },
            { "borderLeftStyle", borderLeftStyle },
            { "borderRightStyle", borderRightStyle},
            { "borderTopStyle", borderTopStyle},
            { "borderBottomStyle", borderBottomStyle},
            { "outlineOffset", outlineOffset },
            { "outlineWidth", outlineWidth },
            { "outlineColor", outlineColor },
            { "outlineStyle", outlineStyle },
            { "boxShadow", boxShadow },
            { "transformOrigin", transformOrigin },
            { "translate", translate },
            { "translateZ", translateZ },
            { "scale", scale },
            { "rotate", rotate },
            { "fontFamily", fontFamily },
            { "color", color },
            { "fontWeight", fontWeight },
            { "fontStyle", fontStyle },
            { "fontSize", fontSize },
            { "lineHeight", lineHeight },
            { "letterSpacing", letterSpacing },
            { "wordSpacing", wordSpacing },
            { "textAlign", textAlign },
            { "verticalAlign", verticalAlign },
            { "textOverflow", textOverflow },
            { "textWrap", textWrap },
            { "whiteSpace", textWrap },
            { "maxLines", maxLines },
            { "lineClamp", maxLines },
            { "textStrokeWidth", textStrokeWidth },
            { "textStrokeColor", textStrokeColor },
            { "content", content },
            { "appearance", appearance },
            { "navigation", navigation },
            { "stateDuration", stateDuration },
            { "objectFit", objectFit },
            { "objectPosition", objectPosition },

            { "borderImageSource", borderImageSource },
            { "borderImageSlice", borderImageSlice },
            { "borderImageRepeat", borderImageRepeat },
            { "borderImageOutset", borderImageOutset },
            { "borderImageWidth", borderImageWidth },

            { "backgroundColor", backgroundColor },
            { "backgroundImage", backgroundImage },
            { "backgroundPositionX", backgroundPositionX },
            { "backgroundPositionY", backgroundPositionY },
            { "backgroundSize", backgroundSize },
            { "backgroundRepeatX", backgroundRepeatX },
            { "backgroundRepeatY", backgroundRepeatY },
            { "backgroundBlendMode", backgroundBlendMode },

            { "maskImage", maskImage },
            { "maskPositionX", maskPositionX },
            { "maskPositionY", maskPositionY },
            { "maskSize", maskSize },
            { "maskRepeatX", maskRepeatX },
            { "maskRepeatY", maskRepeatY },

            { "transitionProperty", transitionProperty },
            { "transitionDuration", transitionDuration },
            { "transitionTimingFunction", transitionTimingFunction },
            { "transitionDelay", transitionDelay },
            { "transitionPlayState", transitionPlayState },

            { "transition-property", transitionProperty },
            { "transition-duration", transitionDuration },
            { "transition-timing-function", transitionTimingFunction },
            { "transition-delay", transitionDelay },
            { "transition-play-state", transitionPlayState },

            { "motionDuration", motionDuration },
            { "motionTimingFunction", motionTimingFunction },
            { "motionDelay", motionDelay },

            { "motion-duration", motionDuration },
            { "motion-timing-function", motionTimingFunction },
            { "motion-delay", motionDelay },

            { "animationDelay", animationDelay },
            { "animationDirection", animationDirection },
            { "animationDuration", animationDuration },
            { "animationFillMode", animationFillMode },
            { "animationIterationCount", animationIterationCount },
            { "animationName", animationName },
            { "animationPlayState", animationPlayState },
            { "animationTimingFunction", animationTimingFunction },

            { "animation-delay", animationDelay },
            { "animation-direction", animationDirection },
            { "animation-duration", animationDuration },
            { "animation-fill-mode", animationFillMode },
            { "animation-iteration-count", animationIterationCount },
            { "animation-name", animationName },
            { "animation-play-state", animationPlayState },
            { "animation-timing-function", animationTimingFunction },

            { "audioClip", audioClip },
            { "audioDelay", audioDelay },
            { "audioIterationCount", audioIterationCount },

            { "audio-clip", audioClip },
            { "audio-delay", audioDelay },
            { "audio-iteration-count", audioIterationCount },

            { "border-image-source", borderImageSource },
            { "border-image-slice", borderImageSlice },
            { "border-image-repeat", borderImageRepeat },
            { "border-image-outset", borderImageOutset },
            { "border-image-width", borderImageWidth },

            { "z-index", zIndex },
            { "sorting-layer", sortingLayer },
            { "pointer-events", pointerEvents },
            { "background-color", backgroundColor },
            { "background-image", backgroundImage },
            { "background-position-x", backgroundPositionX },
            { "background-position-y", backgroundPositionY },
            { "background-size", backgroundSize },
            { "background-repeat-x", backgroundRepeatX },
            { "background-repeat-y", backgroundRepeatY },
            { "background-blend-mode", backgroundBlendMode },
            { "mask-image", maskImage },
            { "mask-position-x", maskPositionX },
            { "mask-position-y", maskPositionY },
            { "mask-size", maskSize },
            { "mask-repeat-x", maskRepeatX },
            { "mask-repeat-y", maskRepeatY },
            { "border-top-left-radius", borderTopLeftRadius },
            { "border-top-right-radius", borderTopRightRadius },
            { "border-bottom-left-radius", borderBottomLeftRadius },
            { "border-bottom-right-radius", borderBottomRightRadius },
            { "border-left-color", borderLeftColor },
            { "border-right-color", borderRightColor },
            { "border-top-color", borderTopColor },
            { "border-bottom-color", borderBottomColor },
            { "border-left-style", borderLeftStyle },
            { "border-right-style", borderRightStyle},
            { "border-top-style", borderTopStyle},
            { "border-bottom-style", borderBottomStyle},
            { "outline-offset", outlineOffset },
            { "outline-width", outlineWidth },
            { "outline-color", outlineColor },
            { "outline-style", outlineStyle },
            { "box-shadow", boxShadow },
            { "transform-origin", transformOrigin },
            { "translate-z", translateZ },
            { "font-family", fontFamily },
            { "font-weight", fontWeight },
            { "font-style", fontStyle },
            { "text-decoration", fontStyle },
            { "text-transform", textTransform },
            { "font-size", fontSize },
            { "line-height", lineHeight },
            { "letter-spacing", letterSpacing },
            { "word-spacing", wordSpacing },
            { "text-align", textAlign },
            { "vertical-align", verticalAlign },
            { "text-overflow", textOverflow },
            { "text-wrap", textWrap },
            { "max-lines", maxLines },
            { "line-clamp", maxLines },
            { "text-stroke-color", textStrokeColor },
            { "text-stroke-width", textStrokeWidth },
            { "white-space", textWrap },
            { "object-fit", objectFit },
            { "object-position", objectPosition },
            { "state-duration", stateDuration },
        };
    }
}
