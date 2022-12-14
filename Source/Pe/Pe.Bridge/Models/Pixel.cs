using System;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// ピクセル情報。
    /// </summary>
    public enum Px
    {
        /// <summary>
        /// 知らん。
        /// </summary>
        Unknown,
        /// <summary>
        /// 論理座標系。
        /// </summary>
        Logical,
        /// <summary>
        /// デバイス座標系。
        /// </summary>
        Device,
    }

    /// <summary>
    /// ピクセル情報を指定。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
    public sealed class PixelKindAttribute: System.Attribute
    {
        /// <summary>
        /// ピクセル情報を指定。
        /// </summary>
        /// <param name="px">ピクセル情報。</param>
        public PixelKindAttribute(Px px)
        {
            Px = px;
        }

        #region property

        /// <summary>
        /// ピクセル情報。
        /// </summary>
        public Px Px { get; }

        #endregion
    }
}
