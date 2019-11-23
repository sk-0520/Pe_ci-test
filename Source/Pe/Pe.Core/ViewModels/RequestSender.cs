using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using Prism.Mvvm;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    public class RequestSender: BindableBase
    {
        #region event

        public event EventHandler<RequestEventArgs>? Raised;

        #endregion

        public RequestSender()
        { }


        #region property

        static RequestParameter EmptyParameter { get; } = new RequestParameter();

        #endregion

        #region function

        static void EmptyCallback(RequestResponse requestResponse)
        { }

        void OnRaised(RequestParameter requestParameter, Action<RequestResponse> callback)
        {
            Raised!.Invoke(this, new RequestEventArgs(requestParameter, callback));
        }

        public void Send() => Send(EmptyParameter);
        public void Send(RequestParameter requestParameter) => Send(requestParameter, EmptyCallback);
        public void Send(RequestParameter requestParameter, Action<RequestResponse> callback)
        {
            OnRaised(requestParameter, callback);
        }

        public Task<RequestResponse> SendAsync(IDispatcherWrapper dispatcherWrapper) => SendAsync(EmptyParameter, dispatcherWrapper, CancellationToken.None);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter, IDispatcherWrapper dispatcherWrapper) => SendAsync(requestParameter, dispatcherWrapper, CancellationToken.None);
        public Task<RequestResponse> SendAsync(RequestParameter requestParameter, IDispatcherWrapper dispatcherWrapper, CancellationToken token)
        {
            var waitEvent = new ManualResetEventSlim(false);

            RequestResponse? result = null;
            void CustomCallback(RequestResponse requestResponse)
            {
                result = requestResponse;
                waitEvent.Set();
            };

            return Task.Run(() => {
                using(waitEvent) {
                    dispatcherWrapper.Begin(() => OnRaised(requestParameter, CustomCallback));
                    waitEvent.Wait(token);
                }
                return result ?? new RequestSilentResponse();
            });
        }


        #endregion
    }

    public static class RequestSenderExtensions
    {
        #region function

        public static void Send<TRequestResponse>(this RequestSender @this, RequestParameter requestParameter, Action<TRequestResponse> callback)
            where TRequestResponse : RequestResponse
        {
            @this.Send(requestParameter, r => {
                var response = (TRequestResponse)r;
                callback(response);
            });
        }

        public static TResult Send<TRequestResponse, TResult>(this RequestSender @this, RequestParameter requestParameter, Func<TRequestResponse, TResult> callback)
            where TRequestResponse : RequestResponse
        {
            TResult result = default!;

            @this.Send(requestParameter, r => {
                var response = (TRequestResponse)r;
                result = callback(response);
            });

            return result;
        }

        #endregion
    }
}
