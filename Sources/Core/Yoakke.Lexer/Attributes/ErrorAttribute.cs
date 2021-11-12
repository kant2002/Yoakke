﻿// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;

namespace Yoakke.Lexer.Attributes;

/// <summary>
/// An attribute to mark an enum value as an error token type.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class ErrorAttribute : Attribute
{
}
