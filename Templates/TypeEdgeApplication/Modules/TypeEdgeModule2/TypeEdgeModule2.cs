﻿using System;
using Microsoft.Azure.IoT.TypeEdge.Modules;
using Microsoft.Azure.IoT.TypeEdge.Modules.Endpoints;
using Microsoft.Azure.IoT.TypeEdge.Modules.Messages;
using Microsoft.Azure.IoT.TypeEdge.Twins;
using TypeEdgeApplication.Shared;
using TypeEdgeApplication.Shared.Messages;
using TypeEdgeApplication.Shared.Twins;

namespace Modules
{
    public class TypeEdgeModule2 : EdgeModule, ITypeEdgeModule2
    {
        public TypeEdgeModule2(ITypeEdgeModule1 proxy)
        {
            Input.Subscribe(proxy.Output, async msg =>
            {
                await Output.PublishAsync(new TypeEdgeModule2Output
                {
                    Data = msg.Data,
                    Metadata = DateTime.UtcNow.ToShortTimeString()
                });
                return MessageResult.Ok;
            });
        }

        public Output<TypeEdgeModule2Output> Output { get; set; }
        public Input<TypeEdgeModule1Output> Input { get; set; }
        public ModuleTwin<TypeEdgeModule2Twin> Twin { get; set; }
    }
}