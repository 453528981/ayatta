
using Microsoft.Extensions.Options;

namespace Ayatta.Storage
{
    /// <summary>
    /// Configuration options for <see cref="StorageOptions"/>.
    /// </summary>
    public class StorageOptions : IOptions<StorageOptions>
    {

        /// <summary>
        /// 是否计时
        /// </summary>        
        public bool Timing { get; set; } = false;

        /// <summary>
        /// 计时警告阀值(毫秒)
        /// </summary>        
        public int TimingThreshold { get; set; } = 2 * 1000;

        /// <summary>
        /// Base库连接字符串
        /// </summary>
        public string BaseConnStr { get; set; } = "server=127.0.0.1;database=base;uid=root;pwd=root;charset=utf8";

        /// <summary>
        /// Weed库连接字符串
        /// </summary>
        public string WeedConnStr { get; set; } = "server=127.0.0.1;database=weed;uid=root;pwd=root;charset=utf8";

        /// <summary>
        /// Store库连接字符串
        /// </summary>
        public string StoreConnStr { get; set; } = "server=127.0.0.1;database=store;uid=root;pwd=root;charset=utf8";

        /// <summary>
        /// Trade库连接字符串
        /// </summary>
        public string TradeConnStr { get; set; } = "server=127.0.0.1;database=trade;uid=root;pwd=root;charset=utf8";

        /// <summary>
        /// Wallet库连接字符串
        /// </summary>
        public string WalletConnStr { get; set; } = "server=127.0.0.1;database=wallet;uid=root;pwd=root;charset=utf8";

        /// <summary>
        /// Passport库连接字符串
        /// </summary>
        public string PassportConnStr { get; set; } = "server=127.0.0.1;database=passport;uid=root;pwd=root;charset=utf8";

        /// <summary>
        /// Promotion库连接字符串
        /// </summary>
        public string PromotionConnStr { get; set; } = "server=127.0.0.1;database=promotion;uid=root;pwd=root;charset=utf8";

        StorageOptions IOptions<StorageOptions>.Value => this;
    }
}