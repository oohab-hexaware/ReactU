using System;
using System.Collections.Generic;

namespace ReactUnity.Styling.Rules
{
    public class RuleTreeNode<T> : IComparable<RuleTreeNode<T>>
    {
        public RuleTreeNode<T> Parent;
        public string Selector;
        public List<RuleSelectorPart> ParsedSelector;
        public LinkedList<RuleTreeNode<T>> Children;

        public RuleRelationType RelationType = RuleRelationType.Parent;
        public RulePseudoType PseudoType = RulePseudoType.None;
        public T Data;

        public MediaQueryList MediaQuery { get; private set; }
        public IReactComponent Scope { get; private set; }
        private int RawSpecifity { get; set; } = 0;
        public int Specifity { get; private set; }

        static RuleTreeNode<T> CreateChildNode(RuleTreeNode<T> parent, MediaQueryList mq, IReactComponent scope, RulePseudoType pseudo)
        {
            if (parent.Children == null) parent.Children = new LinkedList<RuleTreeNode<T>>();

            var child = new RuleTreeNode<T>();
            child.Parent = parent;
            child.MediaQuery = mq;
            child.Scope = scope;
            child.PseudoType = pseudo;

            if (pseudo == RulePseudoType.Before)
            {
                child.RelationType = RuleRelationType.Pseudo;
                child.Selector = "::before";
                child.ParsedSelector = new List<RuleSelectorPart> { RuleSelectorPart.Before };
            }
            else if (pseudo == RulePseudoType.After)
            {
                child.RelationType = RuleRelationType.Pseudo;
                child.Selector = "::after";
                child.ParsedSelector = new List<RuleSelectorPart> { RuleSelectorPart.After };
            }

            parent.Children.AddLast(child);
            return child;
        }

        private void RecalculateSpecificity(int importanceOffset, bool important)
        {
            RawSpecifity = Parent == null ? 0 : Parent.RawSpecifity;

            if (important && RawSpecifity < RuleHelpers.ImportantSpecifity) RawSpecifity += RuleHelpers.ImportantSpecifity;

            if (ParsedSelector != null)
            {
                foreach (var selector in ParsedSelector)
                {
                    switch (selector.Type)
                    {
                        case RuleSelectorPartType.Id:
                            RawSpecifity += 1 << 12;
                            break;

                        case RuleSelectorPartType.Empty:
                        case RuleSelectorPartType.Text:
                        case RuleSelectorPartType.Activatable:
                        case RuleSelectorPartType.Blank:
                        case RuleSelectorPartType.Enabled:
                        case RuleSelectorPartType.Disabled:
                        case RuleSelectorPartType.PlaceholderShown:
                        case RuleSelectorPartType.ReadOnly:
                        case RuleSelectorPartType.ReadWrite:
                        case RuleSelectorPartType.Checked:
                        case RuleSelectorPartType.Indeterminate:
                        case RuleSelectorPartType.Hover:
                        case RuleSelectorPartType.Focus:
                        case RuleSelectorPartType.FocusVisible:
                        case RuleSelectorPartType.FocusWithin:
                        case RuleSelectorPartType.Active:
                        case RuleSelectorPartType.Enter:
                        case RuleSelectorPartType.Leave:
                        case RuleSelectorPartType.Attribute:
                        case RuleSelectorPartType.ClassName:
                            RawSpecifity += 1 << 6;
                            break;

                        case RuleSelectorPartType.Root:
                        case RuleSelectorPartType.Scope:
                        case RuleSelectorPartType.Before:
                        case RuleSelectorPartType.After:
                        case RuleSelectorPartType.FirstChild:
                        case RuleSelectorPartType.LastChild:
                        case RuleSelectorPartType.NthChild:
                        case RuleSelectorPartType.NthLastChild:
                        case RuleSelectorPartType.OnlyChild:
                        case RuleSelectorPartType.State:
                        case RuleSelectorPartType.Tag:
                            RawSpecifity += 1;
                            break;

                        case RuleSelectorPartType.Important:
                            if (RawSpecifity < RuleHelpers.ImportantSpecifity) RawSpecifity += RuleHelpers.ImportantSpecifity;
                            break;
                        default:
                            break;
                    }
                }
            }

            Specifity = RawSpecifity + importanceOffset * (1 << 24);
        }

        public RuleTreeNode<T> AddChildCascading(string selector, MediaQueryList mq, IReactComponent scope, int importanceOffset = 0)
        {
            var shadowParent = selector.StartsWith(":deep ") || selector.StartsWith(">>> ");
            var directParent = selector[0] == '>';
            var directSibling = selector[0] == '+';
            var sibling = selector[0] == '~';
            var important = selector[0] == '!';
            var hasRelative = shadowParent || directParent || directSibling || sibling || important;
            var selfIndex = hasRelative ? 1 : 0;

            var selectorSplit = RuleHelpers.SplitSelectorRegex.Split(selector.Trim(), selfIndex + 2);
            var selectorSelf = selectorSplit.Length > selfIndex ? selectorSplit[selfIndex] : null;
            var selectorOther = selectorSplit.Length > selfIndex + 1 ? selectorSplit[selfIndex + 1] : null;
            var hasChild = !string.IsNullOrWhiteSpace(selectorOther);

            if (hasRelative)
            {
                RelationType = directParent ? RuleRelationType.DirectParent :
                    shadowParent ? RuleRelationType.ShadowParent :
                    directSibling ? RuleRelationType.DirectSibling :
                    sibling ? RuleRelationType.Sibling :
                    important ? RuleRelationType.Self :
                    RuleRelationType.Parent;
            }

            var pseudoType = RulePseudoType.None;

            if (!(string.IsNullOrWhiteSpace(selectorSelf) || selectorSelf == "**"))
            {
                Selector = selectorSelf;
                ParsedSelector = RuleHelpers.ParseSelector(selectorSelf);

                if (ParsedSelector != null)
                {
                    for (int i = 0; i < ParsedSelector.Count; i++)
                    {
                        var sel = ParsedSelector[i];
                        if (sel.Type == RuleSelectorPartType.After || sel.Type == RuleSelectorPartType.Before)
                        {
                            if (sel.Type == RuleSelectorPartType.After) pseudoType = RulePseudoType.After;
                            else if (sel.Type == RuleSelectorPartType.Before) pseudoType = RulePseudoType.Before;
                            break;
                        }
                    }
                }
            }
            RecalculateSpecificity(importanceOffset, important);

            if (!hasChild)
            {
                if (pseudoType != RulePseudoType.None)
                {

                    if (ParsedSelector.Count > 1)
                    {
                        var pseudoChild = CreateChildNode(this, mq, scope, pseudoType);
                        pseudoChild.RecalculateSpecificity(importanceOffset, important);
                        return pseudoChild;
                    }
                    else
                    {
                        PseudoType = pseudoType;
                        RelationType = RuleRelationType.Pseudo;
                        return this;
                    }
                }


                return this;
            }
            else
            {
                if (pseudoType != RulePseudoType.None) return null;
                var child = CreateChildNode(this, mq, scope, RulePseudoType.None);
                return child.AddChildCascading(selectorOther, mq, scope, importanceOffset);
            }
        }

        public bool Matches(IReactComponent component)
        {
            return Matches(component, Scope);
        }

        public bool Matches(IReactComponent component, IReactComponent scope)
        {
            if (!ThisMatches(component, scope)) return false;

            // We are at root, all rules matched
            if (Parent == null) return true;

            if (MediaQuery != null && !MediaQuery.matches) return false;

            var relative = component;
            var runOnce = RelationType == RuleRelationType.DirectSibling || RelationType == RuleRelationType.DirectParent
                || RelationType == RuleRelationType.Self || RelationType == RuleRelationType.Pseudo;

            while (relative != null)
            {
                if (RelationType == RuleRelationType.Parent || RelationType == RuleRelationType.DirectParent)
                    relative = relative.Parent;
                else if (RelationType == RuleRelationType.Sibling || RelationType == RuleRelationType.DirectSibling)
                {
                    if (relative.Parent == null) return false;
                    var ind = relative.Parent.Children.IndexOf(relative);
                    if (ind == 0) return false;
                    relative = relative.Parent.Children[ind - 1];
                }
                else if (RelationType == RuleRelationType.ShadowParent)
                {
                    while (relative != null)
                    {
                        if (relative is IShadowComponent s)
                        {
                            relative = s.ShadowParent;
                            break;
                        }
                        relative = relative.Parent;
                    }
                }

                if (Parent.Matches(relative, scope)) return true;
                if (runOnce) return false;
            }

            return false;
        }


        private bool ThisMatches(IReactComponent component, IReactComponent scope)
        {
            // We are at root, all rules matched
            if (ParsedSelector == null) return true;

            // We reached the end of component hierarchy and there are still rules to process
            // This means the matching is incomplete
            if (component == null) return false;

            for (int i = 0; i < ParsedSelector.Count; i++)
            {
                var selected = ParsedSelector[i];
                if (selected.Matches(component, scope) == selected.Negated) return false;
            }
            return true;
        }

        public int CompareTo(RuleTreeNode<T> other)
        {
            return other.Specifity.CompareTo(Specifity);
        }
    }

    public enum RuleRelationType
    {
        Self = 0,
        Parent = 1,
        DirectParent = 2,
        Sibling = 3,
        DirectSibling = 4,
        ShadowParent = 5,
        Pseudo = 6,
    }

    public enum RulePseudoType
    {
        None = 0,
        Before = 1,
        After = 2,
    }

    public enum RuleSelectorPartType
    {
        None = 0,
        All = 1,
        Tag = 2,
        Id = 3,
        ClassName = 4,
        Attribute = 5,

        DirectDescendant = 10,
        AdjacentSibling = 11,
        Sibling = 12,
        Self = 13,
        ShadowDescendant = 14,

        Not = 20,

        // Standard pseudo classes
        FirstChild = 21,
        LastChild = 22,
        NthChild = 23,
        NthLastChild = 24,
        OnlyChild = 25,
        Empty = 26,
        Root = 27,
        Scope = 28,

        // Input related pseudo classes
        Blank = 30,
        Enabled = 31,
        Disabled = 32,
        PlaceholderShown = 33,
        ReadOnly = 34,
        ReadWrite = 35,
        Checked = 36,
        Indeterminate = 37,

        // Standard states
        Hover = 100,
        Focus = 101,
        FocusVisible = 102,
        FocusWithin = 103,
        Active = 104,

        // Custom states
        Enter = 200,
        Leave = 201,

        // Custom pseudo classes
        Activatable = 300,
        Text = 301,
        Graphic = 302,

        // Pseudo-elements
        Before = 500,
        After = 501,

        // Special
        Important = 1000,
        Special = 1001,
        State = 2000,
    }

    public class RuleSelectorPart : IComparable<RuleSelectorPart>
    {
        public static RuleSelectorPart Important = new RuleSelectorPart { Type = RuleSelectorPartType.Important };
        public static RuleSelectorPart Before = new RuleSelectorPart { Type = RuleSelectorPartType.Before };
        public static RuleSelectorPart After = new RuleSelectorPart { Type = RuleSelectorPartType.After };

        public bool Negated = false;
        public RuleSelectorPartType Type = RuleSelectorPartType.None;
        public string Name = null;
        public object Parameter = null;

        public int CompareTo(RuleSelectorPart other)
        {
            if (Negated && !other.Negated) return 1;
            if (!Negated && other.Negated) return -1;
            return Type.CompareTo(other.Type);
        }

        public bool Matches(IReactComponent component, IReactComponent scope = null)
        {
            switch (Type)
            {
                case RuleSelectorPartType.None:
                    return false;
                case RuleSelectorPartType.All:
                    return !component.IsPseudoElement;
                case RuleSelectorPartType.Tag:
                    return Name == component.Tag;
                case RuleSelectorPartType.Id:
                    return Name == component.Id;
                case RuleSelectorPartType.ClassName:
                    return component.ClassList != null && component.ClassList.Contains(Name);
                case RuleSelectorPartType.Attribute:
                    return component.Data.TryGetValue(Name, out var val) &&
                        (Parameter == null ? IsTruthy(val) : Equals(val, Parameter));
                case RuleSelectorPartType.DirectDescendant:
                case RuleSelectorPartType.AdjacentSibling:
                case RuleSelectorPartType.Sibling:
                case RuleSelectorPartType.Self:
                case RuleSelectorPartType.ShadowDescendant:
                    return true;
                case RuleSelectorPartType.Not:
                    break;
                case RuleSelectorPartType.FirstChild:
                    return !component.IsPseudoElement && component.Parent != null && component.Parent.Children[0] == component;
                case RuleSelectorPartType.LastChild:
                    return !component.IsPseudoElement && component.Parent != null && component.Parent.Children[component.Parent.Children.Count - 1] == component;
                case RuleSelectorPartType.NthChild:
                    return !component.IsPseudoElement && component.Parent != null && ((NthChildParameter) Parameter).Matches(component.Parent.Children.IndexOf(component) + 1);
                case RuleSelectorPartType.NthLastChild:
                    return !component.IsPseudoElement && component.Parent != null && ((NthChildParameter) Parameter).Matches(component.Parent.Children.Count - component.Parent.Children.IndexOf(component));
                case RuleSelectorPartType.Empty:
                    if (component is ITextComponent tc)
                        return string.IsNullOrEmpty(tc.Content);
                    else if (component is IContainerComponent cc)
                        return cc?.Children == null || cc.Children.Count == 0;
                    return true;
                case RuleSelectorPartType.Blank:
                    return component is IInputComponent ic && string.IsNullOrEmpty(ic.Value);
                case RuleSelectorPartType.PlaceholderShown:
                    return component is IInputComponent icp && icp.PlaceholderShown;
                case RuleSelectorPartType.Enabled:
                    return component is IActivatableComponent ace && !ace.Disabled;
                case RuleSelectorPartType.Disabled:
                    return component is IActivatableComponent acd && acd.Disabled;
                case RuleSelectorPartType.ReadOnly:
                    return component is IInputComponent icr && icr.ReadOnly;
                case RuleSelectorPartType.ReadWrite:
                    return component is IInputComponent icw && !icw.ReadOnly;
                case RuleSelectorPartType.Checked:
                    return component is IToggleComponent tgc && tgc.Checked;
                case RuleSelectorPartType.Indeterminate:
                    return component is IToggleComponent tgi && tgi.Indeterminate;
                case RuleSelectorPartType.OnlyChild:
                    return !component.IsPseudoElement && component.Parent != null && component.Parent.Children.Count == 1;
                case RuleSelectorPartType.Root:
                    return component is IHostComponent;
                case RuleSelectorPartType.Scope:
                    return scope != null && component == scope;
                case RuleSelectorPartType.Activatable:
                    return component is IActivatableComponent;
                case RuleSelectorPartType.Text:
                    return component is ITextComponent;
                case RuleSelectorPartType.Graphic:
                    return component is IGraphicComponent;
                case RuleSelectorPartType.Before:
                case RuleSelectorPartType.After:
                    return true;
                case RuleSelectorPartType.Hover:
                case RuleSelectorPartType.Focus:
                case RuleSelectorPartType.FocusVisible:
                case RuleSelectorPartType.FocusWithin:
                case RuleSelectorPartType.Active:
                    return true;
                case RuleSelectorPartType.Important:
                case RuleSelectorPartType.Special:
                    return true;
                case RuleSelectorPartType.State:
                    return component.StateStyles.GetStateOrSubscribe(Parameter as string);
                default:
                    break;
            }

            return false;
        }

        private static bool IsTruthy(object obj)
        {
            if (obj == null || obj is DBNull)
                return false;

            var str = obj as string;
            if (str != null)
                return !string.IsNullOrWhiteSpace(str) &&
                    !str.Trim().Equals(bool.FalseString, StringComparison.InvariantCultureIgnoreCase);

            try
            {
                if (System.Convert.ToDecimal(obj) == 0)
                    return false;
            }
            catch { }

            return true;
        }
    }

    public struct NthChildParameter
    {
        // An + B

        public int A;
        public int B;

        public NthChildParameter(string value)
        {
            if (value == "odd")
            {
                A = 2;
                B = 1;
            }
            else if (value == "even")
            {
                A = 2;
                B = 0;
            }
            else
            {
                var splits = value.Replace(" ", "").Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                A = 0;
                B = 0;
                foreach (var split in splits)
                {
                    if (split.Contains("n")) int.TryParse(split.Replace("n", ""), out A);
                    else int.TryParse(split, out B);
                }
            }
        }

        public bool Matches(int index)
        {
            var offset = index - B;
            if (A > 0) return offset >= 0 && offset % A == 0;
            else if (A < 0) return offset <= 0 && offset % A == 0;
            else return offset == 0;
        }
    }
}
