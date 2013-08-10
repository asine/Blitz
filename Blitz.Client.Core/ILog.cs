﻿using System;

namespace Blitz.Client.Core
{
    public interface ILog
    {
        void Warn(string format, params object[] args);
        void Error(Exception exception);
        void Info(string format, params object[] args);
    }
}