﻿using System;
using NUnit.Framework;

namespace Api.Tests.Acceptance
{
    internal class JourneyTests
    {
        protected Uri BaseAddress;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            BaseAddress = new Uri("http://localhost:5000");
        }
    }
}