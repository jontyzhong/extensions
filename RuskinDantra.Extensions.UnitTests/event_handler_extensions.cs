using System;
using FluentAssertions;
using NUnit.Framework;

namespace RuskinDantra.Extensions.UnitTests
{
	[TestFixture]
	public class event_handler_extensions
	{
		private class event_class
		{
			public event EventHandler SomeEvent;

			public event EventHandler SomeOtherEvent;

			public void Fire()
			{
				SomeEvent.Raise(this);
			}
		}

		[Test]
		public void should_remove_single_event_handler()
		{
			var eventClass = new event_class();
			bool eventFired = false;
			eventClass.SomeEvent += (sender, args) => eventFired = true;
			eventClass.RemoveEventHandlers(nameof(event_class.SomeEvent)).Should().BeTrue();
			eventClass.Fire();
			eventFired.Should().BeFalse();
		}

		[Test]
		public void should_remove_multiple_event_handler()
		{
			var eventClass = new event_class();
			bool eventFired1 = false;
			bool eventFired2 = false;
			eventClass.SomeEvent += (sender, args) => eventFired1 = true;
			eventClass.SomeEvent += (sender, args) => eventFired2 = true;
			eventClass.RemoveEventHandlers(nameof(event_class.SomeEvent)).Should().BeTrue();
			eventClass.Fire();
			eventFired1.Should().BeFalse();
			eventFired2.Should().BeFalse();
		}

		[Test]
		public void should_not_remove_event_handler_if_different_name_supplied()
		{
			var eventClass = new event_class();
			bool eventFired1 = false;
			bool eventFired2 = false;
			eventClass.SomeEvent += (sender, args) => eventFired1 = true;
			eventClass.SomeEvent += (sender, args) => eventFired2 = true;
			eventClass.RemoveEventHandlers(nameof(event_class.SomeOtherEvent)).Should().BeFalse();
			eventClass.Fire();
			eventFired1.Should().BeTrue();
			eventFired2.Should().BeTrue();
		}

		[Test]
		public void should_throw_if_event_does_not_exist()
		{
			var eventClass = new event_class();
			Action removeEventHandlerAction = () => eventClass.RemoveEventHandlers("noevent");
			removeEventHandlerAction.ShouldThrow<InvalidOperationException>();
		}
	}
}