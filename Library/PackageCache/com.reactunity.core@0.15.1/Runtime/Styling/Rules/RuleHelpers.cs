using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExCSS;

namespace ReactUnity.Styling.Rules
{
    public static class RuleHelpers
    {
        public static int ImportantSpecifity = 1 << 18;
        public static Regex SplitSelectorRegex = new Regex("\\s+");
        public static Regex NthChildRegex = new Regex(@"\((\-?\d*n)\s*\+\s*(\d+)\)");

        private static Dictionary<string, RuleSelectorPartType> BasicPartTypes = new Dictionary<string, RuleSelectorPartType>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "first-child", RuleSelectorPartType.FirstChild },
            { "last-child", RuleSelectorPartType.LastChild },
            { "before", RuleSelectorPartType.Before },
            { "after", RuleSelectorPartType.After },
            { "empty", RuleSelectorPartType.Empty },
            { "root", RuleSelectorPartType.Root },
            { "scope", RuleSelectorPartType.Scope },
            { "blank", RuleSelectorPartType.Blank },
            { "enabled", RuleSelectorPartType.Enabled },
            { "disabled", RuleSelectorPartType.Disabled },
            { "placeholder-shown", RuleSelectorPartType.PlaceholderShown },
            { "read-only", RuleSelectorPartType.ReadOnly },
            { "read-write", RuleSelectorPartType.ReadWrite },
            { "checked", RuleSelectorPartType.Checked },
            { "indeterminate", RuleSelectorPartType.Indeterminate },
            { "activatable", RuleSelectorPartType.Activatable },
            { "text", RuleSelectorPartType.Text },
            { "graphic", RuleSelectorPartType.Graphic },
        };

        public static List<RuleSelectorPart> ParseSelector(string selector, bool negated = false)
        {
            // Special selector for the root element, skip parsing in this case
            if (selector == "**") return null;

            var length = selector.Length;


            var paranCount = 0;
            var type = RuleSelectorPartType.Tag;
            var acc = new StringBuilder();
            var paranContent = new StringBuilder();

            var list = new List<RuleSelectorPart>();

            void end(RuleSelectorPartType nextType)
            {
                var nm = acc.ToString().Trim('"');
                var ignore = type == RuleSelectorPartType.None || string.IsNullOrWhiteSpace(nm)
                    || nm == "*" || nm == ">" || nm == "~" || nm == "+" || nm == "!";
                if (!ignore)
                {
                    if (type == RuleSelectorPartType.Special)
                    {
                        var paran = paranContent.ToString();
                        if (nm == "not") list.AddRange(ParseSelector(paran, !negated));
                        else if (BasicPartTypes.TryGetValue(nm, out var partType)) list.Add(new RuleSelectorPart() { Type = partType, Negated = negated });
                        else if (nm == "nth-child") list.Add(new RuleSelectorPart()
                        {
                            Type = RuleSelectorPartType.NthChild,
                            Negated = negated,
                            Parameter = new NthChildParameter(paran),
                        });
                        else if (nm == "nth-last-child") list.Add(new RuleSelectorPart()
                        {
                            Type = RuleSelectorPartType.NthLastChild,
                            Negated = negated,
                            Parameter = new NthChildParameter(paran),
                        });
                        else list.Add(new RuleSelectorPart() { Type = RuleSelectorPartType.State, Negated = negated, Parameter = nm });
                    }
                    else if (type == RuleSelectorPartType.Tag)
                    {
                        if (nm == "_after") list.Add(RuleSelectorPart.After);
                        else if (nm == "_before") list.Add(RuleSelectorPart.Before);
                        else list.Add(new RuleSelectorPart() { Name = nm, Type = type, Negated = negated });
                    }
                    else
                    {
                        string parameter = null;
                        if (type == RuleSelectorPartType.Attribute)
                        {
                            var splits = nm.Split(new char[] { '=' }, 2);
                            nm = splits[0].Trim();
                            if (nm.StartsWith("data-")) nm = nm.Substring(5);

                            parameter = splits.Length > 1 ? splits[1].Trim().Trim('"').Trim('\'') : null;
                        }
                        list.Add(new RuleSelectorPart() { Name = nm, Type = type, Negated = negated, Parameter = parameter });
                    }
                }
                else if (acc.Length == 0 && type == RuleSelectorPartType.Special && nextType == RuleSelectorPartType.Special)
                {
                    acc.Append("_");
                    type = RuleSelectorPartType.Tag;
                    return;
                }
                else if (nm == "!")
                {
                    list.Add(RuleSelectorPart.Important);
                }

                acc.Clear();
                paranContent.Clear();
                type = nextType;
            }

            var prevIsEscape = false;
            for (int i = 0; i < length; i++)
            {
                var ch = selector[i];
                if (!prevIsEscape && ch == '\\')
                {
                    prevIsEscape = true;
                    continue;
                }

                if (prevIsEscape) acc.Append(ch);
                else if (ch == '(')
                {
                    paranCount++;
                    if (paranCount > 1) paranContent.Append(ch);
                }
                else if (ch == ')')
                {
                    paranCount--;
                    if (paranCount == 0) end(RuleSelectorPartType.None);
                    else paranContent.Append(ch);
                }
                else if (paranCount > 0) paranContent.Append(ch);
                else if (ch == '.') end(RuleSelectorPartType.ClassName);
                else if (ch == '#') end(RuleSelectorPartType.Id);
                else if (ch == '_' && i == 0)
                {
                    // Special case for pseudo-elements
                    end(RuleSelectorPartType.Tag);
                    acc.Append(ch);
                }
                else if (ch == '[') end(RuleSelectorPartType.Attribute);
                else if (ch == ']') end(RuleSelectorPartType.Tag);
                else if (ch == ':') end(RuleSelectorPartType.Special);
                else acc.Append(ch);

                prevIsEscape = false;
            }
            end(RuleSelectorPartType.None);

            list.Sort();
            return list;
        }

        public static int GetSpecificity(Priority priority)
        {
            return (priority.Inlines << 24) + (priority.Ids << 16) + (priority.Classes << 8) + priority.Tags;
        }

        public static Dictionary<IStyleProperty, object> ConvertStyleDeclarationToRecord(StyleDeclaration rule, bool important)
        {
            var dic = new Dictionary<IStyleProperty, object>();

            foreach (var item in rule.Where(x => important == x.IsImportant))
            {
                var md = CssProperties.GetKey(item.Name);
                md?.Modify(dic, item.Value);
            }
            return dic;
        }

        public static Dictionary<IStyleProperty, object> ConvertStyleDeclarationToRecord(IDictionary<string, object> dc)
        {
            var dic = new Dictionary<IStyleProperty, object>();

            foreach (var item in dc)
            {
                var md = CssProperties.GetKey(item.Key);
                md?.Modify(dic, item.Value);
            }
            return dic;
        }

        public static string NormalizeSelector(string selector)
        {
            var spaced = new StringBuilder();
            var count = selector.Length;

            var prev = ' ';
            for (int i = 0; i < count; i++)
            {
                var ch = selector[i];

                if (prev == '\\')
                {
                    spaced.Append('\\');
                    spaced.Append(ch);
                    prev = '\0';
                    continue;
                }
                else if (ch == '\\')
                {
                    prev = ch;
                    continue;
                }
                else if (ch == '>' || ch == '+' || ch == '~')
                {
                    spaced.Append(' ');
                    spaced.Append(ch);
                    spaced.Append(' ');
                    prev = '\0';
                    continue;
                }
                else if (prev == ':' && ch == ':')
                {
                    spaced.Append(" ::");
                    prev = '\0';
                }
                else if (ch == ':')
                {
                    prev = ch;
                }
                else
                {
                    if (prev == ':') spaced.Append(prev);
                    spaced.Append(ch);
                    prev = ch;
                }
            }

            return NthChildRegex.Replace(SplitSelectorRegex.Replace(spaced.ToString().Trim(), " "), "($1+$2)");
        }
    }
}
