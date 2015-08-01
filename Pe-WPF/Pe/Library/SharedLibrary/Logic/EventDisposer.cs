﻿namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;

	public abstract class EventDisposerBase<TEventHandler>: DisposeFinalizeBase
	{
		public EventDisposerBase()
			: base()
		{ }

		#region property

		protected TEventHandler EventHandler { get; set; }
		protected Action<TEventHandler> ReleaseEvent { get; set; }

		#endregion

		#region function

		public TEventHandler Handling(TEventHandler eventHandler, Action<TEventHandler> releaseEvent)
		{
			if(EventHandler != null) {
				throw new InvalidOperationException("EventHandler");
			}
			if(ReleaseEvent != null) {
				throw new InvalidOperationException("ReleaseEvent");
			}

			if(eventHandler == null) {
				throw new ArgumentNullException("eventHandler");
			}
			if(releaseEvent == null) {
				throw new ArgumentNullException("releaseEvent");
			}

			ReleaseEvent = releaseEvent;
			return EventHandler = eventHandler;
		}

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				ReleaseEvent(EventHandler);
			}

			base.Dispose(disposing);
		}
		#endregion
	}

	public class EventDisposer: EventDisposerBase<Delegate>
	{
		public EventDisposer()
			: base()
		{ }

		//#region property

		//protected Delegate EventHandler { get; private set; }
		//protected Action<Delegate> ReleaseEvent { get; private set; }

		//#endregion

		//#region DisposeFinalizeBase

		//protected override void Dispose(bool disposing)
		//{
		//	if(!IsDisposed) {
		//		ReleaseEvent(EventHandler);
		//	}

		//	base.Dispose(disposing);
		//}
		//#endregion

		//#region function

		//public Delegate Handle(Delegate eventHandler, Action<Delegate> releaseEvent)
		//{
		//	if(EventHandler != null) {
		//		throw new InvalidOperationException("EventHandler");
		//	}
		//	if(ReleaseEvent != null) {
		//		throw new InvalidOperationException("ReleaseEvent");
		//	}

		//	if(eventHandler == null) {
		//		throw new ArgumentNullException("eventHandler");
		//	}
		//	if(releaseEvent == null) {
		//		throw new ArgumentNullException("releaseEvent");
		//	}

		//	ReleaseEvent = releaseEvent;
		//	return EventHandler = eventHandler;
		//}

		//#endregion
	}

	public class EventDisposer<TEventHandler>: EventDisposerBase<TEventHandler>
	{
		public EventDisposer()
			: base()
		{ }

		//#region property

		//protected TEventHandler EventHandler { get; private set; }
		//protected Action<TEventHandler> ReleaseEvent { get; private set; }

		//#endregion

		//#region DisposeFinalizeBase

		//protected override void Dispose(bool disposing)
		//{
		//	if(!IsDisposed) {
		//		if(Application.Current != null) {
		//			Application.Current.Dispatcher.Invoke(new Action(() => ReleaseEvent(EventHandler)));
		//		} else {
		//			ReleaseEvent(EventHandler);
		//		}
		//	}

		//	base.Dispose(disposing);
		//}
		//#endregion

		//#region function

		//public TEventHandler Handling(TEventHandler eventHandler, Action<TEventHandler> releaseEvent)
		//{
		//	if(EventHandler != null) {
		//		throw new InvalidOperationException("EventHandler");
		//	}
		//	if(ReleaseEvent != null) {
		//		throw new InvalidOperationException("ReleaseEvent");
		//	}

		//	if(eventHandler == null) {
		//		throw new ArgumentNullException("eventHandler");
		//	}
		//	if(releaseEvent == null) {
		//		throw new ArgumentNullException("releaseEvent");
		//	}

		//	ReleaseEvent = releaseEvent;
		//	return EventHandler = eventHandler;
		//}

		//#endregion
	}

}
