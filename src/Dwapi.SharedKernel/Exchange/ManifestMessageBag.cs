﻿using System;
using System.Collections.Generic;
using Dwapi.SharedKernel.Utility;

namespace Dwapi.SharedKernel.Exchange
{
    public class ManifestMessageBag
    {
        public Guid Session { get;  }
        public List<ManifestMessage> Messages { get; set; }=new List<ManifestMessage>();

        public ManifestMessageBag()
        {
        }

        public ManifestMessageBag(List<ManifestMessage> messages)
        {
            Session = LiveGuid.NewGuid();
            Messages = messages;
        }

        public static ManifestMessageBag Create(List<Manifest> manifests)
        {
            return new ManifestMessageBag(ManifestMessage.Create(manifests));
        }
    }
}
