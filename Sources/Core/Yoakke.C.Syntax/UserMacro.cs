// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoakke.C.Syntax
{
    /// <summary>
    /// A <see cref="IMacro"/> that can be defined by the user.
    /// Has optional arguments, returns it's body with the arguments substituted.
    /// </summary>
    public class UserMacro : IMacro
    {
        private const string EmptyStringLiteral = "\"\"";

        public string Name { get; }

        public IReadOnlyList<string>? Parameters { get; }

        private readonly IReadOnlyList<MacroElement> body;

        public UserMacro(string name, IReadOnlyList<string>? parameters, IReadOnlyList<MacroElement> body)
        {
            this.Name = name;
            this.Parameters = parameters;
            this.body = body;
        }

        public IEnumerable<CToken> Expand(IReadOnlyList<IReadOnlyList<CToken>> arguments)
        {
            if (this.Parameters is not null && this.Parameters.Count != arguments.Count)
            {
                // There is a parameter count mismatch
                // Checked if the macro is variadic
                var variadic = this.Parameters.Count > 0 && this.Parameters[this.Parameters.Count - 1] == "...";
                // If not variadic, argument count mismatch is a hard error
                // It's variadic, which means we allow one less arguments (or more), because in the worst case,
                // we only didnt specify an arg for the variadic args
                if (!variadic || this.Parameters.Count - 1 > arguments.Count)
                {
                    // TODO: Proper error handling
                    throw new NotImplementedException();
                }
            }

            // Assign the arguments
            var argDict = new Dictionary<string, IReadOnlyList<CToken>>();
            if (this.Parameters is not null)
            {
                for (var i = 0; i < this.Parameters.Count;)
                {
                    var paramName = this.Parameters[i];
                    if (paramName == "...")
                    {
                        // Variadic argument, consume remaining
                        var arg = new List<CToken>();
                        for (; i < arguments.Count; ++i) arg.AddRange(arguments[i]);
                        argDict["__VA_ARGS__"] = arg;
                    }
                    else
                    {
                        // Regular arg
                        argDict[paramName] = arguments[i];
                        ++i;
                    }
                }
            }

            // Do the substitution
            var result = new List<CToken>();
            foreach (var element in this.body) result.AddRange(ExpandElement(argDict, element));
            return result;
        }

        private static IReadOnlyList<CToken> ExpandElement(
            IReadOnlyDictionary<string, IReadOnlyList<CToken>> args,
            MacroElement element) => element switch
            {
                MacroElement.Literal lit => args.TryGetValue(lit.Token.LogicalText, out var arg)
                    ? arg
                    : new CToken[] { lit.Token },

                MacroElement.Stringify str => new CToken[] { Stringify(args[str.Argument]) },

                MacroElement.Paste paste => Paste(ExpandElement(args, paste.Left), ExpandElement(args, paste.Right)),

                _ => throw new NotImplementedException(),
            };

        private static CToken Stringify(IReadOnlyList<CToken> tokens)
        {
            // TODO: Supply proper ranges?
            if (tokens.Count == 0) return new(default, EmptyStringLiteral, default, EmptyStringLiteral, CTokenType.StringLiteral);

            var result = new StringBuilder();
            // First token just gets pasted
            result.Append(tokens[0].LogicalText);
            for (var i = 1; i < tokens.Count; ++i)
            {
                var prev = tokens[i - 1];
                var current = tokens[i];
                var touching = prev.LogicalRange.End == current.LogicalRange.Start;
                // If not touching, separate with a space
                if (!touching) result.Append(' ');
                result.Append(current.LogicalText);
            }
            // Escape quotes
            result.Replace("\"", "\\\"");
            // Add quotes around
            result.Insert(0, '"').Append('"');
            // Create the string literal value
            var str = result.ToString();
            // TODO: Supply proper ranges?
            return new(default, str, default, str, CTokenType.StringLiteral);
        }

        private static IReadOnlyList<CToken> Paste(IReadOnlyList<CToken> left, IReadOnlyList<CToken> right)
        {
            // If either of them are empty, return the other simply
            // NOTE: This handles the case when both are empty
            if (right.Count == 0) return left;
            if (left.Count == 0) return right;

            // Neither of them are empty, which means we take the last token from left, the first from right
            // and try to mush them together, then return that with all the other tokens around
            var leftLast = left[left.Count - 1];
            var rightFirst = right[0];
            // Concatenate sources
            var pastedTokenSource = new StringBuilder()
                .Append(leftLast.LogicalText)
                .Append(rightFirst.LogicalText)
                .ToString();
            // Try to lex a single token
            var lexer = new CLexer(pastedTokenSource);
            var token = lexer.Next();
            if (token.Kind == CTokenType.End)
            {
                // TODO: Proper error handling
                throw new NotImplementedException();
            }
            // TODO: The range of the token is totally bogus, reconstruct it?
            // Now expect the end
            var endToken = lexer.Next();
            if (endToken.Kind != CTokenType.End)
            {
                // TODO: Proper error handling
                throw new NotImplementedException();
            }
            // Everything is alright, concatenate results
            return left
                .Take(left.Count - 1)
                .Append(token)
                .Concat(right.Skip(1))
                .ToList();
        }
    }
}
