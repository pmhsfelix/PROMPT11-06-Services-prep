using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;

internal class DelegatorChannelBase<TShape> : ChannelBase where TShape : class, IChannel {

    private TShape _innerChannel; // reference to the next channel

    private String _source; // part of the String to output
    
    protected TShape InnerChannel {
        get { return _innerChannel; }
    }

    protected DelegatorChannelBase(ChannelManagerBase channelManagerBase,
                                   TShape innerChannel,
                                   String source)
        : base(channelManagerBase) {
        if (innerChannel == null) {
            throw new ArgumentNullException("DelegatorChannelBase requires a non-null channel.", "innerChannel");
        }
        // set part of the String to print to console
        _source = String.Format("{0} CHANNEL STATE CHANGE: DelegatorChannelBase", source);
        // set the reference to the next channel
        _innerChannel = innerChannel;
    }

    public override T GetProperty<T>() {
        return this._innerChannel.GetProperty<T>();
    }

    #region CommunicationObject members

    protected override void OnAbort() {
        PrintHelper.Print(_source, "OnAbort");
        this._innerChannel.Abort();
    }

    protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, Object state) {
        PrintHelper.Print( _source, "OnBeginClose");
        return this._innerChannel.BeginClose(timeout, callback, state);
    }

    protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, Object state) {
        PrintHelper.Print(_source, "OnBeginOpen");
        return this._innerChannel.BeginOpen(timeout, callback, state);
    }

    protected override void OnClose(TimeSpan timeout) {
        PrintHelper.Print(_source, "OnClose");
        this._innerChannel.Close(timeout);
    }

    protected override void OnEndClose(IAsyncResult result) {
        PrintHelper.Print(_source, "OnEndClose");
        this._innerChannel.EndClose(result);
    }

    protected override void OnEndOpen(IAsyncResult result) {
        PrintHelper.Print(_source, "OnEndOpen");
        this._innerChannel.EndOpen(result);
    }

    protected override void OnOpen(TimeSpan timeout) {
        PrintHelper.Print(_source, "OnOpen");
        this._innerChannel.Open(timeout);
    }

    #endregion
}

