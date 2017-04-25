﻿using System.Collections.Generic;
using System.Linq;

namespace Glob
{
    abstract class GlobNode
    {
        protected GlobNode(GlobNodeType type)
        {
            this.Type = type;
        }

        public GlobNodeType Type { get; }
    }

    class Tree : GlobNode
    {
        public List<Segment> Segments { get; }

        public Tree(IEnumerable<Segment> segments)
            : base(GlobNodeType.Tree)
        {
            Segments = segments.ToList();
        }
    }

    class Root : Segment
    {
        public string Text { get; }

        public Root(string text = "")
            : base(GlobNodeType.Root)
        {
            Text = text;
        }
    }

    class DirectoryWildcard : Segment
    {
        public static readonly DirectoryWildcard Default = new DirectoryWildcard();

        private DirectoryWildcard()
            : base(GlobNodeType.DirectoryWildcard)
        {
        }
    }

    class DirectorySegment : Segment
    {
        public SubSegment[] SubSegments { get; }

        public DirectorySegment(IEnumerable<SubSegment> subSegments)
            : base(GlobNodeType.DirectorySegment)
        {
            SubSegments = subSegments.ToArray();
        }
    }

    abstract class Segment : GlobNode
    {
        protected Segment(GlobNodeType type)
            : base(type)
        {
        }
    }

    class StringWildcard : SubSegment
    {
        public static readonly StringWildcard Default = new StringWildcard();

        private StringWildcard()
            : base(GlobNodeType.StringWildcard)
        {
        }
    }

    class CharacterWildcard : SubSegment
    {
        public static readonly CharacterWildcard Default = new CharacterWildcard();

        private CharacterWildcard()
            : base(GlobNodeType.CharacterWildcard)
        {
        }
    }

    class CharacterSet : SubSegment
    {
        public bool Inverted { get; }
        public Identifier Characters { get; }

        public CharacterSet(Identifier characters, bool inverted)
            : base(GlobNodeType.CharacterSet)
        {
            Characters = characters;
            Inverted = inverted;
        }
    }

    class Identifier : SubSegment
    {
        public string Value { get; }

        public Identifier(string value) 
            : base(GlobNodeType.Identifier)
        {
            Value = value;
        }

        public static implicit operator Identifier(string value)
        {
            return new Identifier(value);
        }
    }

    class LiteralSet : SubSegment
    {
        public IEnumerable<Identifier> Literals { get; }

        public LiteralSet(params Identifier[] literals)
               : base(GlobNodeType.Identifier)
        {
            Literals = literals.ToList();
        }

        public LiteralSet(IEnumerable<Identifier> literals) 
            : base(GlobNodeType.Identifier)
        {
            Literals = literals.ToList();
        }
    }

    abstract class SubSegment : GlobNode
    {
        public SubSegment(GlobNodeType type) 
            : base(type)
        {
        }
    }

    enum GlobNodeType
    {
        Tree,

        // Segments
        Root,
        DirectoryWildcard,
        DirectorySegment,

        // SubSegments
        CharacterSet,
        Identifier,
        LiteralSet,
        StringWildcard,
        CharacterWildcard
    }
}