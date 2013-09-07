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

        [Test]
        public void when_Visible_is_passed_then_true_is_returned()
        {
            var converter = new BooleanToVisibilityConverter();
            var result = converter.ConvertBack(Visibility.Visible, null, null, null);

            Assert.That(result, Is.True);
        }

        [Test]
        public void when_Collapsed_is_passed_then_false_is_returned()
        {
            var converter = new BooleanToVisibilityConverter();
            var result = converter.ConvertBack(Visibility.Collapsed, null, null, null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void when_Hidden_is_passed_then_false_is_returned()
        {
            var converter = new BooleanToVisibilityConverter();
            var result = converter.ConvertBack(Visibility.Hidden, null, null, null);

            Assert.That(result, Is.False);
        }
    }
}