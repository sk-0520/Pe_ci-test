namespace ContentTypeTextNet.Pe.Bridge.Models
{
    //TODO: Pe 側で Polly を使ったような処理を隠蔽したい想いの実装

    public interface IPolicy
    {
        #region function

        IPolicyBuilder CreateBuilder();

        #endregion
    }

    public interface IPolicyBuilder
    {
        #region function
        #endregion
    }
}
