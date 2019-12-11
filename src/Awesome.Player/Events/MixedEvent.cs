using System;
using System.Collections.Generic;
using System.Text;
using Awesome.Player.Models;
using Prism.Events;

namespace Awesome.Player.Events
{
	public class MixedEvent : PubSubEvent<(List<Note> notes, double PositionSliderWidth, double PositionSliderMaximum)>
	{
	}
}
