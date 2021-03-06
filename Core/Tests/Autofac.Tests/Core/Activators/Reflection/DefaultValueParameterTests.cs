﻿using System;
using System.Linq;
using System.Reflection;
using Autofac.Core.Activators.Reflection;
using Autofac.Tests.Util;
using NUnit.Framework;

namespace Autofac.Tests.Core.Activators.Reflection
{
    public class HasDefaultValues
    {
        public HasDefaultValues(string s, string t = "Hello")
        {
        }
    }

    [TestFixture]
    public class DefaultValueParameterTests
    {
        static ParameterInfo GetTestParameter(string name)
        {
            return typeof(HasDefaultValues).GetConstructors().Single()
                .GetParameters().Where(pi => pi.Name == name).Single();
        }

        [Test]
        public void DoesNotProvideValueWhenNoDefaultAvailable()
        {
            var dvp = new DefaultValueParameter();
            Func<object> vp;
            var dp = GetTestParameter("s").DefaultValue;
            Assert.IsFalse(dvp.CanSupplyValue(GetTestParameter("s"), new ContainerBuilder().Build(), out vp));
        }

        [Test]
        public void ProvidesValueWhenDefaultInitialiserPresent()
        {
            var dvp = new DefaultValueParameter();
            var u = GetTestParameter("t");
            Func<object> vp;
            var dp = u.DefaultValue;
            Assert.IsTrue(dvp.CanSupplyValue(u, new ContainerBuilder().Build(), out vp));
            Assert.AreEqual("Hello", vp());
        }
    }
}
