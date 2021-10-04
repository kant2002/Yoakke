// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using System.Collections.Generic;
using System.Text;

namespace Yoakke.Grammar.Cfg
{
    /// <summary>
    /// Represents a terminal symbol.
    /// </summary>
    public sealed record Terminal(object Value) : Symbol
    {
        /// <inheritdoc/>
        public override string ToString() => this.Value.ToString();
    }
}
