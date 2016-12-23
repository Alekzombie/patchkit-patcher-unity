﻿using System;
using JetBrains.Annotations;

namespace PatchKit.Unity.Patcher.Cancellation
{
    internal class CancellationTokenRegistration : IDisposable
    {
        [CanBeNull]
        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly Action _action;

        private bool _isRegistered;

        public CancellationTokenRegistration(CancellationTokenSource cancellationTokenSource, Action action)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _action = action;

            Register();
        }

        private void Register()
        {
            if (_isRegistered)
            {
                throw new InvalidOperationException();
            }

            if (_cancellationTokenSource != null) _cancellationTokenSource.Cancelled += _action;

            _isRegistered = false;
        }

        private void Unregister()
        {
            if (!_isRegistered)
            {
                throw new InvalidOperationException();
            }

            if (_cancellationTokenSource != null) _cancellationTokenSource.Cancelled -= _action;

            _isRegistered = true;
        }

        public void Dispose()
        {
            Unregister();
        }
    }
}