using System;
using System.Collections.Generic;

namespace ReactUnity.Styling.Shorthands
{
    internal static class AllShorthands
    {
        internal static readonly StyleShorthand All = new AllShorthand("all");
        internal static readonly StyleShorthand Margin = new FourDirectionalShorthand("margin", FourDirectionalShorthand.PropertyType.Margin);
        internal static readonly StyleShorthand Padding = new FourDirectionalShorthand("padding", FourDirectionalShorthand.PropertyType.Padding);
        internal static readonly StyleShorthand Inset = new FourDirectionalShorthand("inset", FourDirectionalShorthand.PropertyType.Inset);
        internal static readonly StyleShorthand BorderWidth = new FourDirectionalShorthand("border-width", FourDirectionalShorthand.PropertyType.BorderWidth);
        internal static readonly StyleShorthand BorderColor = new FourDirectionalShorthand("border-color", FourDirectionalShorthand.PropertyType.BorderColor);
        internal static readonly StyleShorthand BorderStyle = new FourDirectionalShorthand("border-style", FourDirectionalShorthand.PropertyType.BorderStyle);
        internal static readonly StyleShorthand BorderRadius = new FourDirectionalShorthand("border-radius", FourDirectionalShorthand.PropertyType.BorderRadius);
        internal static readonly StyleShorthand BorderImage = new BorderImageShorthand("border-image");
        internal static readonly StyleShorthand Outline = new BorderShorthand("outline", BorderShorthand.BorderSide.Outline);
        internal static readonly StyleShorthand Border = new BorderShorthand("border", BorderShorthand.BorderSide.All);
        internal static readonly StyleShorthand BorderTop = new BorderShorthand("border-top", BorderShorthand.BorderSide.Top);
        internal static readonly StyleShorthand BorderRight = new BorderShorthand("border-right", BorderShorthand.BorderSide.Right);
        internal static readonly StyleShorthand BorderBottom = new BorderShorthand("border-bottom", BorderShorthand.BorderSide.Bottom);
        internal static readonly StyleShorthand BorderLeft = new BorderShorthand("border-left", BorderShorthand.BorderSide.Left);
        internal static readonly StyleShorthand Flex = new FlexShorthand("flex");
        internal static readonly StyleShorthand FlexFlow = new FlexFlowShorthand("flex-flow");
        internal static readonly StyleShorthand Font = new FontShorthand("font");
        internal static readonly StyleShorthand Background = new BackgroundShorthand("background");
        internal static readonly StyleShorthand BackgroundPosition = new BackgroundPositionShorthand("background-position", StyleProperties.backgroundPositionX, StyleProperties.backgroundPositionY);
        internal static readonly StyleShorthand BackgroundRepeat = new BackgroundRepeatShorthand("background-repeat");
        internal static readonly StyleShorthand Mask = new MaskShorthand("mask");
        internal static readonly StyleShorthand MaskPosition = new BackgroundPositionShorthand("mask-position", StyleProperties.maskPositionX, StyleProperties.maskPositionY);
        internal static readonly StyleShorthand MaskRepeat = new BackgroundRepeatShorthand("mask-repeat");
        internal static readonly StyleShorthand TextStroke = new TextStrokeShorthand("text-stroke");
        internal static readonly StyleShorthand Transition = new TransitionShorthand("transition");
        internal static readonly StyleShorthand Motion = new MotionShorthand("motion");
        internal static readonly StyleShorthand Animation = new AnimationShorthand("animation");
        internal static readonly StyleShorthand Audio = new AudioShorthand("audio");
        internal static readonly StyleShorthand Transform = new TransformShorthand("transform");
        internal static readonly StyleShorthand Gap = new XYShorthand<float>("gap", LayoutProperties.RowGap, LayoutProperties.ColumnGap);
        internal static readonly Dictionary<string, StyleShorthand> Map = new Dictionary<string, StyleShorthand>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "all", All },
            { "margin", Margin },
            { "padding", Padding },
            { "inset", Inset },
            { "borderWidth", BorderWidth },
            { "borderColor", BorderColor },
            { "borderStyle", BorderStyle },
            { "borderRadius", BorderRadius },
            { "outline", Outline },
            { "border", Border },
            { "borderTop", BorderTop },
            { "borderRight", BorderRight },
            { "borderBottom", BorderBottom },
            { "borderLeft", BorderLeft },
            { "borderImage", BorderImage },
            { "flex", Flex },
            { "flexFlow", FlexFlow },
            { "font", Font },
            { "background", Background },
            { "backgroundPosition", BackgroundPosition },
            { "backgroundRepeat", BackgroundRepeat },
            { "mask", Mask },
            { "maskPosition", MaskPosition },
            { "maskRepeat", MaskRepeat },
            { "textStroke", TextStroke },
            { "transition", Transition },
            { "motion", Motion },
            { "animation", Animation },
            { "audio", Audio },
            { "transform", Transform },
            { "gap", Gap },

            { "border-width", BorderWidth },
            { "border-color", BorderColor },
            { "border-style", BorderStyle },
            { "border-radius", BorderRadius },
            { "border-top", BorderTop },
            { "border-right", BorderRight },
            { "border-bottom", BorderBottom },
            { "border-left", BorderLeft },
            { "border-image", BorderImage },
            { "flex-flow", FlexFlow },
            { "background-position", BackgroundPosition },
            { "background-repeat", BackgroundRepeat },
            { "mask-position", MaskPosition },
            { "mask-repeat", MaskRepeat },
            { "text-stroke", TextStroke },
        };

        internal static StyleShorthand GetShorthand(string name)
        {
            Map.TryGetValue(name, out var style);
            return style;
        }
    }
}
