// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Yoakke.Grammar.Lr;
using Yoakke.Grammar.Lr.Lr0;

namespace Yoakke.Grammar.Tests
{
    public class SlrTableTests : LrTestBase<Lr0Item>
    {
        [Fact]
        public void FromLr0Grammar()
        {
            var grammar = ParseUtils.ParseGrammar(LrTestGrammars.Lr0Grammar);
            grammar.AugmentStartSymbol();
            this.Table = LrParsingTable.Slr(grammar);

            // Assert state count
            Assert.Equal(8, this.Table.StateAllocator.States.Count);

            // Assert item sets
            this.AssertState(
                out var i0,
                "S' -> _ S",
                "S -> _ a S b",
                "S -> _ a S c",
                "S -> _ d b");
            this.AssertState(
                out var i1,
                "S -> a _ S b",
                "S -> a _ S c",
                "S -> _ a S b",
                "S -> _ a S c",
                "S -> _ d b");
            this.AssertState(
                out var i2,
                "S -> d _ b");
            this.AssertState(
                out var i3,
                "S' -> S _");
            this.AssertState(
                out var i4,
                "S -> d b _");
            this.AssertState(
                out var i5,
                "S -> a S _ b",
                "S -> a S _ c");
            this.AssertState(
                out var i6,
                "S -> a S b _");
            this.AssertState(
                out var i7,
                "S -> a S c _");

            // Assert action table
            this.AssertAction(i0, "a", this.Shift(i1));
            this.AssertAction(i0, "d", this.Shift(i2));
            this.AssertAction(i1, "a", this.Shift(i1));
            this.AssertAction(i1, "d", this.Shift(i2));
            this.AssertAction(i2, "b", this.Shift(i4));
            this.AssertAction(i3, "$", Accept.Instance);
            this.AssertAction(i5, "b", this.Shift(i6));
            this.AssertAction(i5, "c", this.Shift(i7));
            foreach (var term in new[] { "$", "b", "c" })
            {
                this.AssertAction(i4, term, this.Reduce("S -> d b"));
                this.AssertAction(i6, term, this.Reduce("S -> a S b"));
                this.AssertAction(i7, term, this.Reduce("S -> a S c"));
            }

            // Assert goto table
            Assert.Equal(i3, this.Table.Goto[i0, new("S")]);
            Assert.Equal(i5, this.Table.Goto[i1, new("S")]);
        }

        [Fact]
        public void FromSlrGrammar()
        {
            var grammar = ParseUtils.ParseGrammar(LrTestGrammars.SlrGrammar);
            grammar.AugmentStartSymbol();
            this.Table = LrParsingTable.Slr(grammar);

            // Assert state count
            Assert.Equal(5, this.Table.StateAllocator.States.Count);

            // Assert item sets
            this.AssertState(
                out var i0,
                "S' -> _ S",
                "S -> _ E",
                "E -> _ 1 E",
                "E -> _ 1");
            this.AssertState(
                out var i1,
                "E -> 1 _ E",
                "E -> 1 _",
                "E -> _ 1 E",
                "E -> _ 1");
            this.AssertState(
                out var i2,
                "S' -> S _");
            this.AssertState(
                out var i3,
                "S -> E _");
            this.AssertState(
                out var i4,
                "E -> 1 E _");

            // Assert action table
            this.AssertAction(i0, "1", this.Shift(i1));
            this.AssertAction(i1, "$", this.Reduce("E -> 1"));
            this.AssertAction(i1, "1", this.Shift(i1));
            this.AssertAction(i2, "$", Accept.Instance);
            this.AssertAction(i3, "$", this.Reduce("S -> E"));
            this.AssertAction(i4, "$", this.Reduce("E -> 1 E"));

            // Assert goto table
            Assert.Equal(i2, this.Table.Goto[i0, new("S")]);
            Assert.Equal(i3, this.Table.Goto[i0, new("E")]);
            Assert.Equal(i4, this.Table.Goto[i1, new("E")]);
        }

        [Fact]
        public void FromLalrGrammar()
        {
            var grammar = ParseUtils.ParseGrammar(LrTestGrammars.LalrGrammar);
            grammar.AugmentStartSymbol();
            this.Table = LrParsingTable.Slr(grammar);

            // Assert state count
            Assert.Equal(11, this.Table.StateAllocator.States.Count);

            // Assert item sets
            this.AssertState(
                out var i0,
                "S' -> _ S",
                "S -> _ a A c",
                "S -> _ a B d",
                "S -> _ B c",
                "B -> _ z");
            this.AssertState(
                out var i1,
                "S -> a _ A c",
                "S -> a _ B d",
                "A -> _ z",
                "B -> _ z");
            this.AssertState(
                out var i2,
                "B -> z _");
            this.AssertState(
                out var i3,
                "S' -> S _");
            this.AssertState(
                out var i4,
                "S -> B _ c");
            this.AssertState(
                out var i5,
                "S -> B c _");
            this.AssertState(
                out var i6,
                "A -> z _",
                "B -> z _");
            this.AssertState(
                out var i7,
                "S -> a A _ c");
            this.AssertState(
                out var i8,
                "S -> a B _ d");
            this.AssertState(
                out var i9,
                "S -> a B d _");
            this.AssertState(
                out var i10,
                "S -> a A c _");

            // Assert action table
            this.AssertAction(i0, "a", this.Shift(i1));
            this.AssertAction(i0, "z", this.Shift(i2));
            this.AssertAction(i1, "z", this.Shift(i6));
            this.AssertAction(i2, "c", this.Reduce("B -> z"));
            this.AssertAction(i2, "d", this.Reduce("B -> z"));
            this.AssertAction(i3, "$", Accept.Instance);
            this.AssertAction(i4, "c", this.Shift(i5));
            this.AssertAction(i5, "$", this.Reduce("S -> B c"));
            this.AssertAction(i6, "c", this.Reduce("A -> z"), this.Reduce("B -> z"));
            this.AssertAction(i6, "d", this.Reduce("B -> z"));
            this.AssertAction(i7, "c", this.Shift(i10));
            this.AssertAction(i8, "d", this.Shift(i9));
            this.AssertAction(i9, "$", this.Reduce("S -> a B d"));
            this.AssertAction(i10, "$", this.Reduce("S -> a A c"));

            // Assert goto table
            Assert.Equal(i3, this.Table.Goto[i0, new("S")]);
            Assert.Equal(i4, this.Table.Goto[i0, new("B")]);
            Assert.Equal(i7, this.Table.Goto[i1, new("A")]);
            Assert.Equal(i8, this.Table.Goto[i1, new("B")]);
        }

        [Fact]
        public void FromClrGrammar()
        {
            var grammar = ParseUtils.ParseGrammar(LrTestGrammars.ClrGrammar);
            grammar.AugmentStartSymbol();
            this.Table = LrParsingTable.Slr(grammar);

            // Assert state count
            Assert.Equal(13, this.Table.StateAllocator.States.Count);

            // Assert item sets
            this.AssertState(
                out var i0,
                "S' -> _ S",
                "S -> _ a E a",
                "S -> _ b E b",
                "S -> _ a F b",
                "S -> _ b F a");
            this.AssertState(
                out var i1,
                "S -> a _ E a",
                "S -> a _ F b",
                "E -> _ e",
                "F -> _ e");
            this.AssertState(
                out var i2,
                "S -> b _ E b",
                "S -> b _ F a",
                "E -> _ e",
                "F -> _ e");
            this.AssertState(
                out var i3,
                "S' -> S _");
            this.AssertState(
                out var i4,
                "E -> e _",
                "F -> e _");
            this.AssertState(
                out var i5,
                "S -> b E _ b");
            this.AssertState(
                out var i6,
                "S -> b F _ a");
            this.AssertState(
                out var i7,
                "S -> b F a _");
            this.AssertState(
                out var i8,
                "S -> b E b _");
            this.AssertState(
                out var i9,
                "S -> a E _ a");
            this.AssertState(
                out var i10,
                "S -> a F _ b");
            this.AssertState(
                out var i11,
                "S -> a F b _");
            this.AssertState(
                out var i12,
                "S -> a E a _");

            // Assert action table
            this.AssertAction(i0, "a", this.Shift(i1));
            this.AssertAction(i0, "b", this.Shift(i2));
            this.AssertAction(i1, "e", this.Shift(i4));
            this.AssertAction(i2, "e", this.Shift(i4));
            this.AssertAction(i3, "$", Accept.Instance);
            this.AssertAction(i4, "a", this.Reduce("E -> e"), this.Reduce("F -> e"));
            this.AssertAction(i4, "b", this.Reduce("E -> e"), this.Reduce("F -> e"));
            this.AssertAction(i5, "b", this.Shift(i8));
            this.AssertAction(i6, "a", this.Shift(i7));
            this.AssertAction(i7, "$", this.Reduce("S -> b F a"));
            this.AssertAction(i8, "$", this.Reduce("S -> b E b"));
            this.AssertAction(i9, "a", this.Shift(i12));
            this.AssertAction(i10, "b", this.Shift(i11));
            this.AssertAction(i11, "$", this.Reduce("S -> a F b"));
            this.AssertAction(i12, "$", this.Reduce("S -> a E a"));

            // Assert goto table
            Assert.Equal(i3, this.Table.Goto[i0, new("S")]);
            Assert.Equal(i9, this.Table.Goto[i1, new("E")]);
            Assert.Equal(i10, this.Table.Goto[i1, new("F")]);
            Assert.Equal(i5, this.Table.Goto[i2, new("E")]);
            Assert.Equal(i6, this.Table.Goto[i2, new("F")]);
        }
    }
}
