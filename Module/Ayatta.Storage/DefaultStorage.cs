using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Ayatta.Storage
{
    /// <summary>
    /// Ĭ�����ݴ���
    /// </summary>
    public partial class DefaultStorage : BaseStorage
    {
        /// <summary>
        /// Ĭ�����ݴ���
        /// </summary>
        /// <param name="optionsAccessor"></param>
        /// <param name="logger"></param>
        public DefaultStorage(IOptions<StorageOptions> optionsAccessor, ILogger<DefaultStorage> logger) : base(optionsAccessor, logger)
        {

        }
    }
}