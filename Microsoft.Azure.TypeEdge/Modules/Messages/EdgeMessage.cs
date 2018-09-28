﻿using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Azure.TypeEdge.Modules.Messages
{
    public abstract class EdgeMessage : IEdgeMessage
    {
        public IDictionary<string, string> Properties { get; set; }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }

        public void SetBytes(byte[] bytes)
        {
            var obj = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(bytes), GetType());

            this.CopyFrom(obj);
        }
    }
}