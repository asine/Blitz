using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Blitz.Client.Core.MVVM;

using NUnit.Framework;

namespace Blitz.Client.Core.Tests.MVVM
{
    [TestFixture]
    public class BooleanToVisibilityConverterTests
    {
        [Test]
        public void when_true_is_passed_then_Visible_is_returned()
        {
            var converter = new BooleanToVisibilityConverter();
            var result = converter.Convert(true, null, null, null);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
        }

        [Test]
        public void when_false_is_passed_then_Collapsed_is_returned()
        {
            var converter = new BooleanToVisibilityConverter();
            var result = converter.Convert(false, null, null, null);

            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
        }
    }
}