﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core.Pipeline;

namespace Azure.Core.Extensions.Tests
{
    internal class TestClientOptions: ClientOptions
    {
        public string Property { get; set; }
        public int IntProperty { get; set; }

        public NestedOptions Nested { get; set; } = new NestedOptions();

        public class NestedOptions
        {
            public string Property { get; set; }
        }
    }
}