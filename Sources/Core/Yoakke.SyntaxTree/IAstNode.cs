// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System.Collections.Generic;

namespace Yoakke.SyntaxTree
{
    /// <summary>
    /// Interface for all AST nodes.
    /// </summary>
    public interface IAstNode
    {
        /// <summary>
        /// The child nodes of this one.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IAstNode>> ChildNodes { get; }

        /// <summary>
        /// The child node collections of this one.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IReadOnlyCollection<IAstNode>>> ChildNodeCollections { get; }

        /// <summary>
        /// The leaf objects of this node.
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> LeafObjects { get; }

        /// <summary>
        /// The leaf object collections of this node.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IReadOnlyCollection<object>>> LeafObjectCollections { get; }
    }
}