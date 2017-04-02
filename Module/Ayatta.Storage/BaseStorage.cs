using System;
using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Ayatta.Storage
{
    /// <summary>
    /// 数据存贮
    /// </summary>
    public abstract class BaseStorage
    {
        private ILogger logger { get; }

        private readonly StorageOptions options;

        /// <summary>
        /// BaseConnection
        /// </summary>
        protected IDbConnection BaseConn => new MySqlConnection(options.BaseConnStr);

        /// <summary>
        /// StoreConnection
        /// </summary>
        protected IDbConnection StoreConn => new MySqlConnection(options.StoreConnStr);

        /// <summary>
        /// TradeConn
        /// </summary>
        protected IDbConnection TradeConn => new MySqlConnection(options.TradeConnStr);

        /// <summary>
        /// WalletConn
        /// </summary>
        protected IDbConnection WalletConn => new MySqlConnection(options.WalletConnStr);

        /// <summary>
        /// PassportConnection
        /// </summary>
        protected IDbConnection PassportConn => new MySqlConnection(options.PassportConnStr);

        /// <summary>
        /// PromotioConnection
        /// </summary>
        protected IDbConnection PromotionConn => new MySqlConnection(options.PromotionConnStr);

        /// <summary>
        /// 数据存贮
        /// </summary>
        /// <param name="optionsAccessor"></param>
        /// <param name="logger"></param>
        protected BaseStorage(IOptions<StorageOptions> optionsAccessor, ILogger logger)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            options = optionsAccessor.Value;
            this.logger = logger;
        }

        /// <summary>
        /// 计时
        /// </summary>
        public Action<string, long> Elapsed { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public Action<string, Exception> Exceptioned { get; set; }

        protected void Try(string name, Action action)
        {
            try
            {
                if (options.Timing)
                {
                    var sw = Stopwatch.StartNew();
                    action();
                    sw.Stop();
                    OnElapsed(name, sw.ElapsedMilliseconds);
                }
                else
                {
                    action();
                }
            }
            catch (Exception e)
            {
                OnExceptioned(name, e);
            }
        }

        protected T Try<T>(string name, Func<T> func, T defaultVal = default(T))
        {
            try
            {
                var v = defaultVal;
                if (options.Timing)
                {
                    var sw = Stopwatch.StartNew();
                    v = func();
                    sw.Stop();
                    OnElapsed(name, sw.ElapsedMilliseconds);
                }
                else
                {
                    v = func();
                }
                return v;
            }
            catch (Exception e)
            {
                OnExceptioned(name, e);
                return defaultVal;
            }
        }

        protected void LogSql(string title, string message)
        {
            logger.LogInformation(title + " " + message);
        }

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="e">异常信息</param>
        private void OnExceptioned(string name, Exception e)
        {
            var handler = Exceptioned;
            if (handler != null)
            {
                handler(name, e);
            }
            else
            {
                logger.LogError(1, e, "方法 {name} 异常 {message}", name, e.Message);
            }
        }

        /// <summary>
        /// 计时
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="milliseconds">执行时长</param>
        private void OnElapsed(string name, long milliseconds)
        {
            var handler = Elapsed;
            if (handler != null)
            {
                handler(name, milliseconds);
            }
            else
            {
                if (milliseconds > options.TimingThreshold)
                {
                    logger.LogWarning(1, "方法 {name} 运行时间 {milliseconds} 毫秒", name, milliseconds);
                }
                else
                {
                    logger.LogInformation(1, "方法 {name} 运行时间 {milliseconds} 毫秒", name, milliseconds);
                }
            }
        }
    }
}