﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using TenisRanking.Utils;

namespace TenisRanking.Models
{
    public class ViewMessageFactory : IViewMessageFactory
    {
        private readonly IStringLocalizer<Resources.Resources> resources;

        public ViewMessageFactory(IStringLocalizer<Resources.Resources> resources)
        {
            this.resources = resources;
        }

        public ViewMessage Create(MessageStatus status, string messageName)
        {
            switch (status)
            {
                case MessageStatus.ERROR: return CreateErrorMessage(messageName);
                case MessageStatus.SUCCESS: return CreateSucessMessage(messageName);
                default: return this.CreateDefaultMessage();
            }
        }

        private ViewMessage CreateSucessMessage(string messageName)
        {
            return new ViewMessage() {Status = MessageStatus.SUCCESS, Message = resources[messageName]};
        }

        private ViewMessage CreateDefaultMessage()
        {
            return new ViewMessage() { Status = MessageStatus.NONE };
        }

        private ViewMessage CreateErrorMessage(string error)
        {
            return new ViewMessage() { Status = MessageStatus.ERROR, Message = error};
        }


    }
}