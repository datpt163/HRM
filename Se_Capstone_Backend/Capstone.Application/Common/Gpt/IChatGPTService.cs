﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Common.Gpt
{
    public interface IChatGPTService
    {
        Task<string> GetChatGptResponseAsync(string prompt);
    }
}