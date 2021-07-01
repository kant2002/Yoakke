// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoakke.X86
{
    /// <summary>
    /// An x86 address specification.
    /// </summary>
    public struct Address : IOperand
    {
        /// <summary>
        /// The optional <see cref="X86.Segment"/> override.
        /// </summary>
        public Segment? Segment { get; }

        /// <summary>
        /// The base address <see cref="Register"/>.
        /// </summary>
        public Register? Base { get; }

        /// <summary>
        /// A scaled offset.
        /// </summary>
        public ScaledIndex? ScaledIndex { get; }

        /// <summary>
        /// A displacement constant.
        /// </summary>
        public int Displacement { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="segment">The optional <see cref="X86.Segment"/> override.</param>
        /// <param name="base">The base address <see cref="Register"/>.</param>
        /// <param name="scaledIndex">A scaled offset.</param>
        /// <param name="displacement">A displacement constant.</param>
        public Address(Segment? segment = null, Register? @base = null, ScaledIndex? scaledIndex = null, int displacement = 0)
        {
            this.Segment = segment;
            this.Base = @base;
            this.ScaledIndex = scaledIndex;
            this.Displacement = displacement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="base">The base address <see cref="Register"/>.</param>
        /// <param name="scaledIndex">A scaled offset.</param>
        /// <param name="displacement">A displacement constant.</param>
        public Address(Register? @base, ScaledIndex? scaledIndex = null, int displacement = 0)
            : this(null, @base, scaledIndex, displacement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="scaledIndex">A scaled offset.</param>
        /// <param name="displacement">A displacement constant.</param>
        public Address(ScaledIndex scaledIndex, int displacement = 0)
            : this(null, null, scaledIndex, displacement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="displacement">A displacement constant.</param>
        public Address(int displacement)
            : this(null, null, null, displacement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="base">The base address <see cref="Register"/>.</param>
        /// <param name="displacement">A displacement constant.</param>
        public Address(Register @base, int displacement)
            : this(@base, null, displacement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="segment">The optional <see cref="X86.Segment"/> override.</param>
        /// <param name="displacement">A displacement constant.</param>
        public Address(Segment? segment, int displacement)
            : this(segment, null, null, displacement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="segment">The optional <see cref="X86.Segment"/> override.</param>
        /// <param name="scaledIndex">A scaled offset.</param>
        /// <param name="displacement">A displacement constant.</param>
        public Address(Segment? segment, ScaledIndex? scaledIndex, int displacement = 0)
            : this(segment, null, scaledIndex, displacement)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> struct.
        /// </summary>
        /// <param name="segment">The optional <see cref="X86.Segment"/> override.</param>
        /// <param name="base">The base address <see cref="Register"/>.</param>
        /// <param name="displacement">A displacement constant.</param>
        public Address(Segment? segment, Register? @base, int displacement)
            : this(segment, @base, null, displacement)
        {
        }
    }

    /// <summary>
    /// Represents a scaled index for X86 addressing.
    /// </summary>
    public readonly struct ScaledIndex
    {
        /// <summary>
        /// The index <see cref="Register"/>.
        /// </summary>
        public readonly Register Index;

        /// <summary>
        /// The index scaling constant. Must be 1, 2, 4 or 8.
        /// </summary>
        public readonly int Scale;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaledIndex"/> struct.
        /// </summary>
        /// <param name="index">The index <see cref="Register"/>.</param>
        /// <param name="scale">The index stacke.</param>
        public ScaledIndex(Register index, int scale)
        {
            if (scale != 1 && scale != 2 && scale != 4 && scale != 8)
            {
                throw new ArgumentOutOfRangeException(nameof(scale), "scale must be 1, 2, 4 or 8");
            }
            this.Index = index;
            this.Scale = scale;
        }

        public void Deconstruct(out Register index, out int scale)
        {
            index = this.Index;
            scale = this.Scale;
        }
    }
}